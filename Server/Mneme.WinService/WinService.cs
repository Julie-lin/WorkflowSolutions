using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Mneme.ProcessLocator;
using Mneme.Utility;
using Thermo.Workflows.Contracts.RealTime;
using Thermo.Workflows.RealTime;

namespace Mneme.WinService
{
    //http://stackoverflow.com/questions/13602016/windows-service-the-underlying-provider-failed-on-open
    //running as window service may fail, see above
    public partial class WinService : ServiceBase
    {
        private List<ServiceHost> _serviceHostList = null;
        //jlin ?? static can't be redirect to diffrente log location
        //internal static log4net.ILog Log = null;//log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WinService()
        {
            InitializeComponent();
            InitializeLog();
        }

        private static void InitializeLog()
        {

            //Type dtype = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
            //log4net.Config.XmlConfigurator.Configure();
            //Log = log4net.LogManager.GetLogger(dtype);
            
        }

        public void OnStartFromConsoleApp()
        {
            string[] emptyArgsList = null;
            OnStart(emptyArgsList);
        }
        public void OnStopFromConsoleApp()
        {
            Shutdown();
        }
        private void Shutdown()
        {
            AbortAndCloseServiceHosts();
        }


        protected override void OnStart(string[] args)
        {
            ServerInitiate();
            StartWebServices();
        }

        private void ServerInitiate()
        {
            ProcessRunTimeLocator.RegisterMnemeComponent();
        }

        protected override void OnStop()
        {
        }

        private void StartWebServices()
        {
            //Debugger.Launch();
            List<ServiceHost> hostList = CreateServiceHosts();
            TryHarder.Retry(
                () =>
                {
                    try
                    {
                        foreach (ServiceHost host in hostList)
                        {
                            host.Open();
                        }
                    }
                    catch (Exception ex)
                    {
                        AbortAndCloseServiceHosts();
                        throw;
                    }
                }, 6, TimeSpan.FromSeconds(10), typeof(Exception));

        }
        private void AbortAndCloseServiceHosts()
        {
            try
            {
                //MultiProcessFileLog.WriteLine("WinService.AbortAndCloseServiceHosts() - ENTERED");

                if (_serviceHostList != null)
                {
                    foreach (ServiceHost host in _serviceHostList)
                    {
                        host.Abort();
                    }
                    _serviceHostList = null;
                }
            }
            finally
            {
                //MultiProcessFileLog.WriteLine("WinService.AbortAndCloseServiceHosts() - EXITING");
            }
        }
        private List<ServiceHost> CreateServiceHosts()
        {
            if (_serviceHostList != null)
            {
                Debug.Assert(true, "Program Error: attempting to create ServiceHost's a second time.\nPlease call AbortAndCloseServiceHosts() before attempting to recreate the ServiceHost's.");
            }
            else
            {

                WorkflowInstanceManager wfInstanceManager = new WorkflowInstanceManager(
                    null,
                    (exception) => new ExceptionInfo(exception).ToString())
                {
                    OnWorkflowInstanceCompleted = (rootWorkItem) =>
                    {
                       // Log.Fatal("test ========== done with this workflow");
                        XmlSerializer serializer = new XmlSerializer(typeof(WorkItemWithHistory));
                        using (Stream serializationStream = File.Create("C:\\Temp\\expInfo.xml"))
                        {
                            serializer.Serialize(serializationStream, rootWorkItem);
                        }
                    }
                };

                ProcessEngineService processingServer = new ProcessEngineService(wfInstanceManager);
                
                //jlin prepare for dynamically resolve component known type
                ServiceHost ProcessEngineHost = new ServiceHost(processingServer);
                foreach (ServiceEndpoint endpoint in ProcessEngineHost.Description.Endpoints)
                {
                    foreach (OperationDescription operation in endpoint.Contract.Operations)
                    {
                        operation.Behaviors.Find<DataContractSerializerOperationBehavior>().DataContractResolver = new MnemeTypeResolver();
                    }
                    
                }
                _serviceHostList = new List<ServiceHost>()
                                   {
                                       //new ServiceHost(rawDatabaseSummary),
                                       //new ServiceHost(processingServer)
                                       ProcessEngineHost
                                    };
            }
            return _serviceHostList;
        }

    }
}
