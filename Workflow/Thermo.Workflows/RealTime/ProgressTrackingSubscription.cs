using System.ServiceModel;
using Thermo.Workflows.Contracts;

namespace Thermo.Workflows.RealTime
{
     [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ProgressTrackingSubscription : IProgressTrackingSubscrption
    {
        private readonly ProgressCallbackProxy _callbackProxy;
        private readonly WorkItemProgressObserver _progressObserver;

        public ProgressTrackingSubscription(ProgressCallbackProxy callbackProxy,
            WorkItemProgressObserver progressObserver)
        {
            _callbackProxy = callbackProxy;
            _progressObserver = progressObserver;
        }

        public void SubscribeToProgressEvents()
        {
            _callbackProxy.Subscribe();
        }

        public WorkItemProgressObserver GetActiveWorkItemsStatus()
        {
            return _progressObserver;
        }
    }
}
