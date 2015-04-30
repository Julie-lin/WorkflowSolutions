using System;
using System.ServiceModel;
using Thermo.Workflows.Contracts;
using Thermo.Workflows.Contracts.RealTime;

namespace Thermo.Workflows.RealTime
{
    public class ProgressCallbackProxy : IServiceCallback, IDisposable
    {
        private ChannelFactory<IServiceCallback> _serviceCallbackChannel;
        private IServiceCallback _serviceCallbackProxy;

        private readonly object synkRoot = new object();

        public ProgressCallbackProxy()
        {
        }

        internal void Subscribe()
        {
            lock (synkRoot)
            {
                SetupChannel();
            }
        }

        private void SetupChannel()
        {
            TeardownChannel();
            _serviceCallbackChannel = new ChannelFactory<IServiceCallback>("progressCallbackEndpoint");
            _serviceCallbackProxy = _serviceCallbackChannel.CreateChannel();   
        }

        private void TeardownChannel()
        {
            if(_serviceCallbackChannel == null) return;
            try
            {
                _serviceCallbackChannel.Close();
            }
            catch{}

            _serviceCallbackChannel = null;
            _serviceCallbackProxy = null;
        }

        public void WorkItemLogCallback(TaskProgressCallbackMessage callbackMessage, long messageIndex)
        {
            InvokeCallback(() => _serviceCallbackProxy.WorkItemLogCallback(callbackMessage, messageIndex));
        }

        public void RegisterWorkItemsWithHistoryTree(WorkItemWithHistory lookupTree, long messageIndex)
        {
            InvokeCallback(() => _serviceCallbackProxy.RegisterWorkItemsWithHistoryTree(lookupTree, messageIndex));
        }

        public void UnregisterWorkItemsWithHistoryTree(Guid workItemRootId, long messageIndex)
        {
            InvokeCallback(() => _serviceCallbackProxy.UnregisterWorkItemsWithHistoryTree(workItemRootId, messageIndex));
        }

        private void InvokeCallback(Action callback)
        {
            lock (synkRoot)
            {
                if (_serviceCallbackProxy != null)
                {
                    int tryCount = 1;
                    while (true)
                    {
                        try
                        {
                            callback();
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (tryCount > 0)
                            {
                                tryCount--;
                                SetupChannel();
                            }
                            else
                            {
                                TeardownChannel();
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            TeardownChannel();
        }
    }
}
