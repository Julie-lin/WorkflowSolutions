using System;
using System.ServiceModel;
using Thermo.Workflows.Contracts.RealTime;

namespace Thermo.Workflows.Contracts
{
    [ServiceContract]
    public interface IServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void WorkItemLogCallback(TaskProgressCallbackMessage callbackMessage, long messageIndex);

        [OperationContract(IsOneWay = true)]
        void RegisterWorkItemsWithHistoryTree(WorkItemWithHistory lookupTree, long messageIndex);

        [OperationContract(IsOneWay = true)]
        void UnregisterWorkItemsWithHistoryTree(Guid workItemRootId, long messageIndex);
    }
}
