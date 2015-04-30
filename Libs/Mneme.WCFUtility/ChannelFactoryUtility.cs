using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Mneme.Data;
using Mneme.Data.Interfaces;
using Mneme.ProcessLocator;
using Workflow.Data.Interfaces;

namespace Mneme.WCFUtility
{
    public class ChannelFactoryUtility
    {
        public static void CallProcessServer(Action<IProcessingService> ProxyCall)
        {
            try
            {
                ChannelFactory<IProcessingService> channelFactory =
                    new ChannelFactory<IProcessingService>("processingServiceEndpoint");

                foreach (var operation in channelFactory.Endpoint.Contract.Operations)
                {
                    //to resolve know type
                    operation.Behaviors.Find<DataContractSerializerOperationBehavior>().DataContractResolver = new MnemeTypeResolver();
                }

                var channel = channelFactory.CreateChannel();
                ProxyCall(channel);

                //using (ChannelFactory<IProcessingService> channelFactory =
                //       new ChannelFactory<IProcessingService>("processingServiceEndpoint"))
                //{
                //    ProxyCall(channelFactory.CreateChannel());
                //}

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.InnerException.Message);

            }
        }

        class MnemeIdentity : EndpointIdentity
        {
            
        }

        
        public static bool CallProcessServer(Func<IProcessingService, bool> ProxyCall)
        {
            try
            {
                ChannelFactory<IProcessingService> channelFactory =
                    new ChannelFactory<IProcessingService>("processingServiceEndpoint");
                foreach (var operation in channelFactory.Endpoint.Contract.Operations)
                {
                    //to resolve know type
                    operation.Behaviors.Find<DataContractSerializerOperationBehavior>().DataContractResolver = new MnemeTypeResolver();
                }
                var channel = channelFactory.CreateChannel();
                return ProxyCall(channel);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.InnerException.Message);
                return false;
            }
        }

        public static void CallRawDataSummaryServer(Action<IRawDataSummary> ProxyCall)
        {
            try
            {
                ChannelFactory<IRawDataSummary> channelFactory =
                    new ChannelFactory<IRawDataSummary>("rawDatabaseSummaryServiceEndpoint");
                var channel = channelFactory.CreateChannel();
                ProxyCall(channel);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.InnerException.Message);

            }
        }

        public static List<IdNamePair> CallRawDataSummaryServer(Func<IRawDataSummary, List<IdNamePair>> ProxyCall)
        {
            try
            {
                ChannelFactory<IRawDataSummary> channelFactory =
                    new ChannelFactory<IRawDataSummary>("rawDatabaseSummaryServiceEndpoint");
                var channel = channelFactory.CreateChannel();
                return ProxyCall(channel);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.InnerException.Message);
                return new List<IdNamePair>();
            }
        }

        public static IRawDataSummary CreateRawDataSummaryService(string serverName)
        {
            serverName = string.IsNullOrEmpty(serverName) ? "localhost" : serverName;

            string uri = string.Format("net.tcp://{0}:9033/RawDataManagerService", serverName);
            string identity = string.Format("RawDataManagerService/{0}", serverName);
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;

            // var myBinding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(new Uri(uri),//Uri("http://localhost:9033/RawDataManagerService"),
                            EndpointIdentity.CreateSpnIdentity(identity));

            var channelFactory = new ChannelFactory<IRawDataSummary>(binding, endpoint);

            IRawDataSummary client = null;

            try
            {
                client = channelFactory.CreateChannel();
                //client.MyServiceOperation();
                //((ICommunicationObject)client).Close();
            }
            catch
            {
                if (client != null)
                {
                    ((ICommunicationObject)client).Abort();
                }
            }
            return client;
        }
        public static IProcessingService CreateProcessingEngineService(string serverName)
        {
            serverName = string.IsNullOrEmpty(serverName) ? "localhost" : serverName;
                
            string uri = string.Format("net.tcp://{0}:9033/ProcessEngineService", serverName);
            string identity = string.Format("ProcessEngineService/{0}", serverName);
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;

            // var myBinding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(new Uri(uri),//Uri("http://localhost:9033/ProcessEngineService"),
                            EndpointIdentity.CreateSpnIdentity(identity));
            var channelFactory = new ChannelFactory<IProcessingService>(binding, endpoint);

            foreach (var operation in channelFactory.Endpoint.Contract.Operations)
            {
                //to resolve know type
                operation.Behaviors.Find<DataContractSerializerOperationBehavior>().DataContractResolver = new MnemeTypeResolver();
            }


            IProcessingService client = null;

            try
            {
                client = channelFactory.CreateChannel();
                //client.MyServiceOperation();
                //((ICommunicationObject)client).Close();
            }
            catch
            {
                if (client != null)
                {
                    ((ICommunicationObject)client).Abort();
                }
            }
            return client;
        }

    }
}
