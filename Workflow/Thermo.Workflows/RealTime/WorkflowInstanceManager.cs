using System;
using System.Activities;
using System.Collections.Generic;
using System.ServiceModel;
using AcquisitionActivities.RealTime;
using Thermo.Workflows.Contracts;
using Thermo.Workflows.Contracts.RealTime;
using System.Linq;

namespace Thermo.Workflows.RealTime
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WorkflowInstanceManager : IWorkflowInstanceManager, IWorkItemCancellationService
    {
        readonly Dictionary<Guid, WorkflowApplication> _runningWorkflows = new Dictionary<Guid, WorkflowApplication>();
        private readonly object _sinkRoot = new object();

        public Action<WorkItemWithHistory> OnWorkflowInstanceCompleted { get; set; }

        private readonly ProgressTrackingParticipant _progressTrackingParticipant;
        public ProgressTrackingParticipant ProgressTrackingParticipant
        {
            get { return _progressTrackingParticipant; }
        }

        public WorkflowInstanceManager(IServiceCallback progressCallback, Func<Exception, string> errorFormatter)
        {
            _progressTrackingParticipant = new ProgressTrackingParticipant(errorFormatter, OnWorkflowInstanceRemoved)
            {
                ProgressObserver = progressCallback
            };
        }

        public bool IsRootWorkingItemRunning(Guid rootWorkItemId)
        {
            lock (_sinkRoot)
            {
                return _runningWorkflows.ContainsKey(rootWorkItemId);
            }
        }

        public void ExecuteWorkflowInstance(WorkflowApplication workflowInstance, Guid rootWorkItemId)
        {
            workflowInstance.Extensions.Add(ProgressTrackingParticipant);
            lock (_sinkRoot)
            {
                if(_runningWorkflows.ContainsKey(rootWorkItemId))
                {
                    throw new ArgumentException(
                        String.Format("Work item with Id '{0}' is already running.", rootWorkItemId), "rootWorkItemId");
                }
                workflowInstance.BeginRun(WorkflowInstanceEndExecute, workflowInstance);
                _runningWorkflows.Add(rootWorkItemId, workflowInstance);
            }
        }

        private void WorkflowInstanceEndExecute(IAsyncResult ar)
        {
            WorkflowApplication workflowInstance = (WorkflowApplication) ar.AsyncState;
            try
            {
                workflowInstance.EndRun(ar);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);    
            }
        }

        private void OnWorkflowInstanceRemoved(Guid rootWorkItemId, WorkItemWithHistory rootWorkItemWithHisory)
        {
            UnregisterWorkflowInstance(rootWorkItemId);

            if(OnWorkflowInstanceCompleted != null)
            {
                OnWorkflowInstanceCompleted(rootWorkItemWithHisory);
            }
        }

        private void UnregisterWorkflowInstance(Guid rootWorkItemId)
        {
            lock (_sinkRoot)
            {
                if (_runningWorkflows.ContainsKey(rootWorkItemId))
                    _runningWorkflows.Remove(rootWorkItemId);
            }
        }

        public void CancelWorkItem(List<Guid> rootToCanceledChildItemPath)
        {
            WorkflowApplication wfInstance;

            lock (_sinkRoot)
            {
                _runningWorkflows.TryGetValue(rootToCanceledChildItemPath[0], out wfInstance);
            }

            if (wfInstance != null)
            {
                Console.Write("cancellation requested for " + 
                    rootToCanceledChildItemPath[rootToCanceledChildItemPath.Count - 1] );

                string bookmarksBefore = String.Join(" , ", wfInstance.GetBookmarks().Select(bookmark => bookmark.BookmarkName).ToArray());
                BookmarkResumptionResult bookmarkResumptionResult =
                    wfInstance.ResumeBookmark(ProgressTrackingInitializer.CancellationBookmark,
                    rootToCanceledChildItemPath);
                string bookmarksAfter = String.Join(" , ", wfInstance.GetBookmarks().Select(bookmark => bookmark.BookmarkName).ToArray());
            }
        }
    }
}
