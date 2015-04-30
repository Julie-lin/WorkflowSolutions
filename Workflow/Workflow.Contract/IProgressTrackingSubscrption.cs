using System.ServiceModel;

namespace Thermo.Workflows.Contracts
{
    [ServiceContract]
    public interface IProgressTrackingSubscrption
    {
        [OperationContract]
        void SubscribeToProgressEvents();
        [OperationContract]
        WorkItemProgressObserver GetActiveWorkItemsStatus();
    }
}
