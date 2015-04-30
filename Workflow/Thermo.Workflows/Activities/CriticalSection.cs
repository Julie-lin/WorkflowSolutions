using System;
using System.Linq;
using System.Activities.Hosting;
using System.Activities;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.ComponentModel;
using AcquisitionActivities.RealTime;
using Thermo.Data.Hierarchical;
using Thermo.Workflows.Contracts.RealTime;
using Thermo.Workflows.Properties;

namespace Thermo.Workflows.Activities
{
     [Designer(typeof(CriticalSectionDesigner))]
    public sealed class CriticalSection : NativeActivity
    {
         public CriticalSection()
         {
             WorkItem = new InArgument<IWorkItemWithChildren>();
             PriorityTicket = new InArgument<long>(long.MinValue);
         }

        [RequiredArgument]
        public InArgument<string> QueueName { get; set; }
        public InArgument<IWorkItemWithChildren> WorkItem { get; set; }
        public InArgument<long> PriorityTicket { get; set; }
        public Activity Body { get; set; }
        private readonly Variable<string> _resumeBookmark = new Variable<string>();
         private readonly Variable<Exception> _bodyError = new Variable<Exception>();

        protected override bool CanInduceIdle
        {
            get
            {
                return true;
            }
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            metadata.AddImplementationVariable(_resumeBookmark);
            metadata.AddImplementationVariable(_bodyError);
            metadata.AddDefaultExtensionProvider(() => new CriticalSectionQueueExtension());
        }

        protected override void Execute(NativeActivityContext context)
        {
            string bookmarkName = Guid.NewGuid().ToString();
            _resumeBookmark.Set(context, bookmarkName);

            context.CreateBookmark(bookmarkName, OnResumeBody, BookmarkOptions.None);
            bool resumeImmediately = context.GetExtension<CriticalSectionQueueExtension>()
                .Enter(bookmarkName, QueueName.Get(context), PriorityTicket.Get(context));

            if(!resumeImmediately && WorkItem.Get(context) != null)
            {
                context.Track(
               new CustomTrackingRecord(WorkItemStatus.Waiting)
               {
                   Data = 
                        {
                            {CustomProgressTrackingDataKey.Target, WorkItem.Get(context)},
                            {CustomProgressTrackingDataKey.Message, 
                                String.Format(Resources.WaitingInTheQueue, QueueName.Get(context))}
                        }
               });
            }
        }

        protected override void Cancel(NativeActivityContext context)
        {
            base.Cancel(context);
            context.CancelChildren();
            if (_bodyError.Get(context) == null)
            {
                context.GetExtension<CriticalSectionQueueExtension>()
                    .Exit(_resumeBookmark.Get(context), QueueName.Get(context));
            }
        }

         private void OnResumeBody(NativeActivityContext context, Bookmark bookmark, object value)
         {
             context.ScheduleActivity(Body, OnBodyComplete, OnBodyFaulted);
         }

         private void OnBodyComplete(NativeActivityContext context, ActivityInstance completedinstance)
         {
             if (_bodyError.Get(context) == null)
             {
                 context.GetExtension<CriticalSectionQueueExtension>()
                     .Exit(_resumeBookmark.Get(context), QueueName.Get(context));
             }
             context.RemoveAllBookmarks();
         }

         private void OnBodyFaulted(NativeActivityFaultContext faultcontext, Exception propagatedexception, 
             ActivityInstance propagatedfrom)
         {
             faultcontext.GetExtension<CriticalSectionQueueExtension>()
                 .Exit(_resumeBookmark.Get(faultcontext), QueueName.Get(faultcontext));
             _bodyError.Set(faultcontext, propagatedexception);
             faultcontext.RemoveAllBookmarks();
         }
    }

    internal sealed class CriticalSectionQueueExtension : IWorkflowInstanceExtension
    {
        private static readonly object LockObject = new object();
        private WorkflowInstanceProxy _workflowInstance;
        private static readonly Dictionary<string, CriticaSectionQueue> RequestQueues = 
            new Dictionary<string, CriticaSectionQueue>();

        public IEnumerable<object> GetAdditionalExtensions()
        {
            return null;      
        }

        public void SetInstance(WorkflowInstanceProxy instance)
        {
            _workflowInstance = instance;
        }

        public long GetPriorityTicket(string queueName)
        {
            lock (LockObject)
            {
                CriticaSectionQueue requestQueue = GetRequestQueue(queueName);
                return requestQueue.GetPriorityTicket();
            }
        }

        public bool Enter(string bookmark, string queueName, long priorityTicket)
        {
            int requestQueueCount;
            lock(LockObject)
            {
                CriticaSectionQueue requestQueue = GetRequestQueue(queueName);

                CriticalSectionResumptionInfo criticalSectionResumptionInfo = 
                    requestQueue.Add(this, bookmark, priorityTicket);
                requestQueueCount = requestQueue.ResumptionQueue.Count;
                if(requestQueueCount == 1)
                {
                    requestQueue.ExecutingCriticalSection = criticalSectionResumptionInfo;
                }
            }

            if (requestQueueCount == 1)
            {
                _workflowInstance.BeginResumeBookmark(new Bookmark(bookmark), null, null, null);
                return true;
            }
            return false;
        }

        public void Exit(string bookmark, string queueName)
        {
            CriticalSectionResumptionInfo nextInQueue = null;
            lock (LockObject)
            {
                CriticaSectionQueue requestQueue = GetRequestQueue(queueName);
                
                CriticalSectionResumptionInfo exiting = requestQueue.ResumptionQueue.Find(
                    resumptionInfo => resumptionInfo.ResumptionBookmark == bookmark);
                requestQueue.ResumptionQueue.Remove(exiting);


                if (exiting == requestQueue.ExecutingCriticalSection)
                {
                    if (requestQueue.ResumptionQueue.Count > 0)
                    {
                        nextInQueue = requestQueue.ResumptionQueue[0];
                        requestQueue.ExecutingCriticalSection = nextInQueue;
                    }
                    else
                    {
                        requestQueue.ExecutingCriticalSection = null;
                    }
                }
            }
            if(nextInQueue != null)
            {
                nextInQueue.WorkflowInstanceExtension._workflowInstance.BeginResumeBookmark(
                    new Bookmark(nextInQueue.ResumptionBookmark), null, null, null);
            }
        }


        private static CriticaSectionQueue GetRequestQueue(string queueName)
        {
            CriticaSectionQueue requestQueue;
            if(!RequestQueues.TryGetValue(queueName, out requestQueue))
            {
                requestQueue = new CriticaSectionQueue(queueName);
                RequestQueues.Add(queueName, requestQueue);
            }
            return requestQueue;
        }

        private class CriticaSectionQueue
        {
            public CriticaSectionQueue(string queueName)
            {
                QueueName = queueName;
                NextPriorityTicket = 0;
                ResumptionQueue = new List<CriticalSectionResumptionInfo>();
            }

            private long NextPriorityTicket { get; set; }
            public string QueueName { get; private set; }
            public CriticalSectionResumptionInfo ExecutingCriticalSection { get; set; }
            public List<CriticalSectionResumptionInfo> ResumptionQueue {get; set;}

            public long GetPriorityTicket()
            {
                return NextPriorityTicket++;
            }

            public CriticalSectionResumptionInfo Add(CriticalSectionQueueExtension workflowInstaceExtension, 
                string bookmark, long priorityTicket)
            {
                if(priorityTicket == long.MinValue)
                {
                    priorityTicket = GetPriorityTicket();
                }

                int positionInTheQueue = ResumptionQueue.TakeWhile(
                    resumptionItem => resumptionItem.PriorityTicket <= priorityTicket).Count();

                CriticalSectionResumptionInfo criticalSectionResumptionInfo = 
                    new CriticalSectionResumptionInfo( workflowInstaceExtension, bookmark, priorityTicket);
                ResumptionQueue.Insert(positionInTheQueue, criticalSectionResumptionInfo);
                return criticalSectionResumptionInfo;
            }
        }

        private class CriticalSectionResumptionInfo
        {
            public CriticalSectionResumptionInfo(CriticalSectionQueueExtension workflowInstanceExtension, 
                string resumptionBookmark, long priorityTicket)
            {
                WorkflowInstanceExtension = workflowInstanceExtension;
                ResumptionBookmark = resumptionBookmark;
                PriorityTicket = priorityTicket;
            }

            public long PriorityTicket { get; private set; }
            public CriticalSectionQueueExtension WorkflowInstanceExtension { get; private set; }
            public string ResumptionBookmark { get; private set; }
        }
    }
}
