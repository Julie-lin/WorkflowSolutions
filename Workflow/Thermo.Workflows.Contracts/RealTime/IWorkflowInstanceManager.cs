using System;
using System.Activities;
using System.Collections.Generic;

namespace Thermo.Workflows.Contracts.RealTime
{
    public interface IWorkflowInstanceManager
    {
        void ExecuteWorkflowInstance(WorkflowApplication workflowInstance, Guid rootWorkItemId);
        bool IsRootWorkingItemRunning(Guid rootWorkItemId);
        void CancelWorkItem(List<Guid> rootToCanceledChildItemPath);
    }
}
