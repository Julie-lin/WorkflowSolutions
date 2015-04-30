using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AppData;
using Mneme.Data;
using Mneme.Data.Interfaces;
using Workflow.Data.Interfaces;

namespace DiagramDesigner
{
    public  class Utility
    {
        public static void CallProcessServer(Action<IProcessingService> ProxyCall)
        {
            try
            {
                ChannelFactory<IProcessingService> channelFactory =
                    new ChannelFactory<IProcessingService>("processingServiceEndpoint");
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

        public static bool CallProcessServer(Func<IProcessingService, bool> ProxyCall)
        {
            try
            {
                ChannelFactory<IProcessingService> channelFactory =
                    new ChannelFactory<IProcessingService>("processingServiceEndpoint");
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


    }
}
