using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Windows.Threading;
using Thermo.Workflows.Contracts.RealTime;

namespace Thermo.Workflows.Contracts.Client
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ProgressCallbackService : IServiceCallback 
    {
        private WorkItemProgressObserver _progressMonitor;
        private object synckRoot = new object();
        private Queue<Action> unprocessedMessageQueue = new Queue<Action>();
        public Action<WorkItemWithHistory> OnWorkflowInstanceCompleted { get; set; }

        public WorkItemProgressObserver ProgressMonitor
        {
            get { return _progressMonitor; }
            set
            {
                lock (synckRoot)
                {
                    _progressMonitor = value;
                    ProcessQueue();   
                }
            }
        }

        private void ProcessQueue()
        {
            while (unprocessedMessageQueue.Count > 0)
            {
                unprocessedMessageQueue.Dequeue().Invoke();
            }
        }

        private Dispatcher _dispatcher;
        private readonly bool _cloneMessages;

        public ProgressCallbackService(Dispatcher dispatcher, bool sameProcessWithTheWorkflow)
        {
            _dispatcher = dispatcher;
            _cloneMessages = sameProcessWithTheWorkflow;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void WorkItemLogCallback(TaskProgressCallbackMessage callbackMessage, long messageIndex)
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.Invoke((Action<TaskProgressCallbackMessage, long>)WorkItemLogCallback,
                    DispatcherPriority.Normal, new object[] { callbackMessage, messageIndex});
            }
            else
            {
                TaskProgressCallbackMessage localCallbackMessage = GetLocalTaskProgressCallbackMessage(callbackMessage);
                lock (synckRoot)
                {
                    if (_progressMonitor == null)
                    {
                        unprocessedMessageQueue.Enqueue(
                            () => _progressMonitor.WorkItemLogCallback(localCallbackMessage, messageIndex));
                    }
                    else
                    {
                        _progressMonitor.WorkItemLogCallback(localCallbackMessage, messageIndex);
                    }
                }
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void RegisterWorkItemsWithHistoryTree(WorkItemWithHistory lookupTree, long messageIndex)
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.Invoke((Action<WorkItemWithHistory, long>)RegisterWorkItemsWithHistoryTree,
                    DispatcherPriority.Normal, new object[] { lookupTree, messageIndex });
            }
            else
            {
                WorkItemWithHistory localLookupTree = GetLocalWorkItemWithHistory(lookupTree);
                lock (synckRoot)
                {
                    if (_progressMonitor == null)
                    {
                        unprocessedMessageQueue.Enqueue(
                            () => _progressMonitor.RegisterWorkItemsWithHistoryTree(localLookupTree, messageIndex));
                    }
                    else
                    {
                        _progressMonitor.RegisterWorkItemsWithHistoryTree(localLookupTree, messageIndex);
                    }
                }
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void UnregisterWorkItemsWithHistoryTree(Guid workItemRootId, long messageIndex)
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.Invoke((Action<Guid, long>)UnregisterWorkItemsWithHistoryTree,
                    DispatcherPriority.Normal, new object[] { workItemRootId, messageIndex });
            }
            else
            {
                lock (synckRoot)
                {
                    if (_progressMonitor == null)
                    {
                        unprocessedMessageQueue.Enqueue(
                            () => _progressMonitor.UnregisterWorkItemsWithHistoryTree(workItemRootId, messageIndex));
                    }
                    else
                    {
                        WorkItemWithHistory completedWorkItem = null;
                        if(OnWorkflowInstanceCompleted != null)
                        {
                            completedWorkItem = _progressMonitor.
                                PendingWorkItems.First(workItemRoot => workItemRoot.Id == workItemRootId);
                        }
                        _progressMonitor.UnregisterWorkItemsWithHistoryTree(workItemRootId, messageIndex);
                        if(OnWorkflowInstanceCompleted != null && completedWorkItem != null)
                        {
                            OnWorkflowInstanceCompleted(completedWorkItem);
                        }
                    }
                }
            }
        }

        private DataContractSerializer _taskProgressCallbackMessageSerializer;
        private DataContractSerializer TaskProgressCallbackMessageSerializer
        {
            get
            {
                if(_taskProgressCallbackMessageSerializer == null)
                {
                    _taskProgressCallbackMessageSerializer = new DataContractSerializer(typeof(TaskProgressCallbackMessage));
                }
                return _taskProgressCallbackMessageSerializer;
            }
        }

        private DataContractSerializer _workItemWithHistorySerializer;
        private DataContractSerializer WorkItemWithHistorySerialiser
        {
            get
            {
                if(_workItemWithHistorySerializer == null)
                {
                    _workItemWithHistorySerializer = new DataContractSerializer(typeof(WorkItemWithHistory));
                }
                return _workItemWithHistorySerializer;
            }
        }


        private TaskProgressCallbackMessage GetLocalTaskProgressCallbackMessage(TaskProgressCallbackMessage callbackMessage)
        {
            if (_cloneMessages == false) return callbackMessage;

            using(MemoryStream memoryStream = new MemoryStream())
            {
                TaskProgressCallbackMessageSerializer.WriteObject(memoryStream, callbackMessage);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (TaskProgressCallbackMessage) TaskProgressCallbackMessageSerializer.ReadObject(memoryStream);
            }
        }

        private WorkItemWithHistory GetLocalWorkItemWithHistory(WorkItemWithHistory lookupTree)
        {
            if (_cloneMessages == false) return lookupTree;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                WorkItemWithHistorySerialiser.WriteObject(memoryStream, lookupTree);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (WorkItemWithHistory)WorkItemWithHistorySerialiser.ReadObject(memoryStream);
            }
        }
    }
}
