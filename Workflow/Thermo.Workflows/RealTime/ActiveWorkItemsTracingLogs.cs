using System;
using System.Collections.Generic;
using System.Diagnostics;
using Thermo.Workflows.Contracts;
using Thermo.Workflows.Contracts.RealTime;

namespace AcquisitionActivities.RealTime
{
    internal class ActiveWorkItemsTracingLogs
    {
        internal readonly WorkItemProgressObserver _workItemProgressObserver = new WorkItemProgressObserver();
        private readonly Dictionary<Guid, WorkItemWithHistory> _activeWorkItemsRoots = new Dictionary<Guid, WorkItemWithHistory>();
        private readonly object _sinkRoot = new object();

        public void RegisterWorkflowInstanceWithLookupTree(Guid workflowInstanceId, WorkItemWithHistory lookupTree, long messageIndex)
        {
            _workItemProgressObserver.RegisterWorkItemsWithHistoryTree(lookupTree, messageIndex);
            lock (_sinkRoot)
            {
                _activeWorkItemsRoots.Add(workflowInstanceId, lookupTree);
            }
        }

        public void PushMessage(Guid workflowInstanceId, TaskProgressCallbackMessage message, long messageIndex)
        {
            WorkItemWithHistory rootWorkItem = GetRootWorkItemForWorkflowInstance(workflowInstanceId);
            message.RouteToChild = rootWorkItem.RouteFromParentToChild(message.RouteToChild[0].Id)
                .ConvertAll(workItemWithChildren =>
                    new WorkItemId 
                    {
                        Id = workItemWithChildren.Id, 
                        Name = workItemWithChildren.Name,
                        WorkItemType = workItemWithChildren.WorkItemType
                    });
                
            _workItemProgressObserver.WorkItemLogCallback(message, messageIndex);
        }

        internal WorkItemWithHistory GetRootWorkItemForWorkflowInstance(Guid workflowInstanceId)
        {
            WorkItemWithHistory rootItem;
            if (!_activeWorkItemsRoots.TryGetValue(workflowInstanceId, out rootItem)) return null;
            return rootItem;
        }

        public void UnregisterWorkflowInstance(Guid workflowInstanceId, long messageIndex)
        {
            WorkItemWithHistory rootWorkItem = _activeWorkItemsRoots[workflowInstanceId];

            int initialCount = _workItemProgressObserver.PendingWorkItems.Count;
            _workItemProgressObserver.UnregisterWorkItemsWithHistoryTree(rootWorkItem.Id, messageIndex);
            Debug.Assert(_workItemProgressObserver.PendingWorkItems.Count == initialCount - 1);

            lock (_sinkRoot)
            {
                _activeWorkItemsRoots.Remove(workflowInstanceId);
            }
        }
    }
}
