using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Thermo.Workflows.Contracts.RealTime;
using Thermo.Workflows.RealTime;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace MnemeConsoleHost
{
    class Program
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            Debug.Listeners.Clear();
            HostProcessingService();
            //HostRawDatabaseSummary();
        }

        private static void HostProcessingService()
        {
            WorkflowInstanceManager wfInstanceManager = new WorkflowInstanceManager(
                null,
                (exception) => new ExceptionInfo(exception).ToString())
            {
                OnWorkflowInstanceCompleted = (rootWorkItem) =>
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(WorkItemWithHistory));
                    using (Stream serializationStream = File.Create("C:\\Temp\\expInfo.xml"))
                    {
                        serializer.Serialize(serializationStream, rootWorkItem);
                    }
                }
            };

            ProcessEngineService processingServer = new ProcessEngineService(wfInstanceManager);
            RawDataManagerService rawDatabaseSummary = new RawDataManagerService();
            using (ServiceHost serviceHost = new ServiceHost(processingServer))
            using (ServiceHost rawDatabaseSummaryHost = new ServiceHost(rawDatabaseSummary))
            {
                serviceHost.Open();
                rawDatabaseSummaryHost.Open();
                StartLog4Net();
                //TestLogerClass.TestLogger();
                Console.WriteLine("Server is running. Press return to stop...");
                Console.ReadLine();
            }
        }

        private static void HostRawDatabaseSummary()
        {
            RawDataManagerService rawDatabaseSummary = new RawDataManagerService();
            using (ServiceHost rawDatabaseSummaryHost = new ServiceHost(rawDatabaseSummary))
            {
                rawDatabaseSummaryHost.Open();
                Console.WriteLine("Server is running. Press return to stop...");
                

                
                Console.ReadLine();
            }
        }

        private static void StartLog4Net()
        {
            //_log.Debug("test - Debug logging");
            //_log.Info("test - Info logging");
            //_log.Warn("test - Warn logging");
            //_log.Error("test - Error logging");
            //_log.Fatal("test - Fatal logging");
            
        }
    }
}

