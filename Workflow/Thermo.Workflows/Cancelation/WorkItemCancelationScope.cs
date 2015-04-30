using System;
using System.Activities;
using System.Activities.Tracking;
using System.ComponentModel;
using System.Diagnostics;
using AcquisitionActivities.RealTime;
using Thermo.Data.Hierarchical;
using Thermo.Workflows.Contracts;
using Thermo.Workflows.Contracts.RealTime;
using Thermo.Workflows.Properties;

namespace Thermo.Workflows.Cancelation
{
    [Designer(typeof(WorkItemCancelationScopeDesigner))]
    public class WorkItemCancelationScope : NativeActivity
    {
        public Activity Action { get; set; }
        public Activity Cancelation { get; set; }

        private readonly Variable<bool> _suppressCancel = new Variable<bool>();
        private readonly Variable<Exception> _childFault = new Variable<Exception>();

        public WorkItemCancelationScope()
        {
            SupressExceptions = new InArgument<bool>(true);
        }

        [RequiredArgument]
        public InArgument<ICancellableWorkItemWithChildren> CancellableWorkItem { get; set; }

        public InArgument<bool> SupressExceptions { get; set; }

        public static string CancelableWorkItemBookmarkName(Guid workItemId)
        {
            return String.Format("CancelWorkItem_{0}", workItemId);
        }

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
            metadata.AddImplementationVariable(_suppressCancel);
            metadata.AddImplementationVariable(_childFault);
        }

        protected override void Execute(NativeActivityContext context)
        {
            ICancellableWorkItemWithChildren cancelationTarget = CancellableWorkItem.Get(context);
            if(cancelationTarget.HasCancellationRequested) return;

            PostTrackingRecord(context, cancelationTarget, WorkItemStatus.Executing);

            Debug.WriteLine("CancellableBookmark created: {0} - {1} -Workflow Id = {2} : {3}",
             CancelableWorkItemBookmarkName(cancelationTarget.Id),
             cancelationTarget.Name, context.WorkflowInstanceId, context.ActivityInstanceId);
            context.CreateBookmark(CancelableWorkItemBookmarkName(cancelationTarget.Id), OnWorkItemCanceled);

            context.ScheduleActivity(Action, onCompleted, onWorkItemFaulted);
        }

        private void onWorkItemFaulted(NativeActivityFaultContext faultcontext, 
            Exception propagatedexception, ActivityInstance propagatedfrom)
        {
            faultcontext.HandleFault();
            faultcontext.CancelChildren();
            _childFault.Set(faultcontext, propagatedexception);
            
        }


        private void OnWorkItemCanceled(NativeActivityContext context, Bookmark bookmark, object value)
        {
            if (!_suppressCancel.Get(context))
            {
                context.CancelChildren();
            }
        }

        internal static void MarkTasksAsCancelled(IWorkItemWithChildren root, NativeActivityContext context, string message)
        {
            foreach (ICancellableWorkItemWithChildren workItemWithChildren in root.SuccesorsAndSelf())
            {
                workItemWithChildren.HasCancellationRequested = true;
                PostTrackingRecord(context, workItemWithChildren, WorkItemStatus.Canceled, message);
            }
        }

        private void onCompleted(NativeActivityContext context, ActivityInstance completedinstance)
        {
            ICancellableWorkItemWithChildren target = CancellableWorkItem.Get(context);
            if(completedinstance.State == ActivityInstanceState.Canceled
                || completedinstance.State == ActivityInstanceState.Faulted)
            {
                string message = _childFault.Get(context) != null
                                     ? Resources.CancelledDueToAssociatedTaskFailure
                                     : Resources.CancelledUponUserRequest;

                MarkTasksAsCancelled(target, context, message);

                if(completedinstance.State == ActivityInstanceState.Faulted)
                {
                    PostTrackingRecord(context, target, WorkItemStatus.Faulted, Resources.FailedDueToChildTaskFailure);
                }
            }
            else
            {
                PostTrackingRecord(context, target, WorkItemStatus.Closed);
            }

            context.RemoveAllBookmarks();
            if ((completedinstance.State == ActivityInstanceState.Canceled)
                || (completedinstance.State == ActivityInstanceState.Faulted))
            {
                _suppressCancel.Set(context, true);
                if (Cancelation != null)
                {
                    context.ScheduleActivity(Cancelation, OnCompensationActivityCompleted);
                }
                else
                {
                    OnCompensationActivityCompleted(context, completedinstance);
                }
            }
        }

        private void OnCompensationActivityCompleted(
            NativeActivityContext context, ActivityInstance completedinstance)
        {
            _suppressCancel.Set(context, false);
            if (SupressExceptions.Get(context) == false && _childFault.Get(context) != null)
            {
                throw _childFault.Get(context);
            }
        }

        private static void PostTrackingRecord(NativeActivityContext context, 
            IWorkItemWithChildren target, string status, string message = null)
        {
            CustomTrackingRecord customTrackingRecord = 
                new CustomTrackingRecord(status)
                {
                    Data = 
                    {
                        {CustomProgressTrackingDataKey.Target, target},
                    }
                };
            if(!String.IsNullOrEmpty(message))
            {
                customTrackingRecord.Data.Add(CustomProgressTrackingDataKey.Message, message);       
            }
            
            context.Track(customTrackingRecord);
        }
    }
}
