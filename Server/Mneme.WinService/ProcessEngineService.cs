using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Mneme.Workflows;
using Thermo.Workflows.Contracts.RealTime;
using Thermo.Workflows.RealTime;
using Workflow.Activities;
using Workflow.Data;

using Workflow.Data.Interfaces;

namespace Mneme.WinService
{

    //[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ProcessEngineService : IProcessingService
    {
        //private static log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IWorkflowInstanceManager _wfInstanceManager;
        private MnemeNotificationCallback _callBack = new MnemeNotificationCallback();
        
        public ProcessEngineService(IWorkflowInstanceManager manager)
        {
            _wfInstanceManager = manager;
            //TryLog4Net();
        }

        public ProcessEngineService()
        {

            _wfInstanceManager = new WorkflowInstanceManager(null,
                                                             (message) => new ExceptionInfo(message).ToString())
            {
                OnWorkflowInstanceCompleted = (rootWorkItem) =>
                {
                    //WinService.Log.Fatal("test ========== done with this workflow");
                    XmlSerializer serializer = new XmlSerializer(typeof(string));
                    using (Stream serializationStream = File.Create("C:\\Temp\\expInfo.xml"))
                    {
                        // serializer.Serialize(serializationStream, rootWorkItem);
                    }
                }
            };
            
            //Mneme.LogService.Logger.Instance.Initialize("FileAppender", "AAA.log");
            //LogService.Logger.Instance.Info("AAAAAAAAAAAAAbbbbbbbbbbb");
        }

        private void TryLog4Net()
        {
            //Mneme.LogService.Logger.Instance.Initialize("FileAppender", @"c:\temp");
            //LogService.Logger.Instance.Info("AAAAAAAAAAAAAbbbbbbbbbbb");

            //ILog dblog = LogManager.GetLogger("ADONetAppender");
            //log4net.Config.XmlConfigurator.Configure();
            //dblog.Info("test message");
            //dblog.Info("logging to db");

            //Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
            //if (hierarchy != null && hierarchy.Configured)
            //{
            //    foreach (IAppender appender in hierarchy.GetAppenders())
            //    {
            //        if (appender is AdoNetAppender)
            //        {
            //            var adoNetAppender = (AdoNetAppender)appender;
            //            adoNetAppender.ConnectionString =
            //                @"Data Source=localhost\mneme;Initial Catalog=mnemeRawdata;Integrated Security=False;Persist Security Info=True;User ID=mneme;Password=mneme";
            //            //"metadata=res://*/MnemeRawData.csdl|res://*/MnemeRawData.ssdl|res://*/MnemeRawData.msl;provider=System.Data.SqlClient;provider connection string=\"Data Source=localhost\\mneme;Initial Catalog=MnemeRawData;Integrated Security=False;Persist Security Info=True;User ID=mneme;Password=mneme\""; // ConfigurationManager.AppSettings["YOURCONNECTIONSTRINGKEY"].ToString();
            //            //"Data Source=localhost\\mneme;Initial Catalog=mnemeRawdata;Integrated Security=False;Persist Security Info=True;User ID=mneme;Password=mneme"
            //            adoNetAppender.ActivateOptions(); //Refresh AdoNetAppenders Settings
            //        }
            //    }
            //}

            //foreach (IAppender appender in hierarchy.GetAppenders())
            //{
            //    if (appender is AdoNetAppender)
            //    {
            //        var adoNetAppender = (AdoNetAppender)appender;
            //        Debug.WriteLine(adoNetAppender.ConnectionString);
            //        Debug.WriteLine(adoNetAppender.CommandText);
            //    }
            //}

            //ILog dblog = LogManager.GetLogger("ADONetAppender");
            //dblog.Info("test message");
            //dblog.Info("logging to db");


//            StartLog4Net();
//            TestLogerClass.TestLogger(WinService.Log);
            
        }
        #region Implementation of 


        public void ExecuteCompositeBatchSequentialActivity(ProcessBatch batch, List<ComponentNode> workflowNodes,
                                                            Guid sessionName)
        {

            WorkflowApplication workflowApplication = new WorkflowApplication(
                new CompositeBatchSequentialActivity(),
                                   new Dictionary<string, object>
                                           {
                                               {"Batch", batch},
                                               {"ComponentList", workflowNodes},
                                               {"SessionName", sessionName}
                                           });
            //workflowApplication.Extensions.Add(WinService.Log);
            workflowApplication.Extensions.Add(_callBack);
            _wfInstanceManager.ExecuteWorkflowInstance(
                        workflowApplication,
                            batch.Id);

            //WorkflowInvoker.Invoke(new CompositeBatchSequentialActivity(),
            //                       new Dictionary<string, object>
            //                               {
            //                                   {"Batch", batch},
            //                                   {"ComponentList", workflowNodes},
            //                                   {"StartNode", startNode},
            //                                   {"SessionName", sessionName}
            //                               });

        }

        public void ExecuteCompositeBatchParallelActivity(ProcessBatch batch, List<ComponentNode> workflowNodes,
                                                            Guid sessionName)
        {

            WorkflowApplication workflowApplication = new WorkflowApplication(
                new CompositeBatchParallelActivity(),
                                   new Dictionary<string, object>
                                           {
                                               {"Batch", batch},
                                               {"ComponentList", workflowNodes},
                                               {"SessionName", sessionName}
                                           });
            //workflowApplication.Extensions.Add(WinService.Log);
            workflowApplication.Extensions.Add(_callBack);
            _wfInstanceManager.ExecuteWorkflowInstance(
                        workflowApplication,
                            batch.Id);

            //WorkflowInvoker.Invoke(new CompositeBatchSequentialActivity(),
            //                       new Dictionary<string, object>
            //                               {
            //                                   {"Batch", batch},
            //                                   {"ComponentList", workflowNodes},
            //                                   {"SessionName", sessionName}
            //                               });

        }
        //jlin if process measurement only expId = 0
        //otherwise measurement id = 0
        public void ExecutePostUploadActivity(long expId, long measurementId, List<ComponentNode> workflowNodes, 
            Guid sessionName, Guid batchId)
        {
                WorkflowApplication workflowApplication = new WorkflowApplication(
                new PostUploadActivity(),
                new Dictionary<string, object>
                                    {
                                        {"ExperimentId", expId},
                                        {"ComponentList", workflowNodes},
                                        {"SessionName", sessionName},
                                        {"BatchId", batchId},
                                        {"MeasurementId", measurementId}
                                    });
                //workflowApplication.Extensions.Add(WinService.Log);
                workflowApplication.Extensions.Add(_callBack);
                _wfInstanceManager.ExecuteWorkflowInstance(
                            workflowApplication,
                                batchId);


        }

        public void CancelWorkItem(List<Guid> rootToCanceledChildItemPath)
        {
            _wfInstanceManager.CancelWorkItem(rootToCanceledChildItemPath);
        }


        public bool IsBatchRunning(Guid batchId)
        {
            return _wfInstanceManager.IsRootWorkingItemRunning(batchId);
        }

        public void TestWorkflowAccess(int count)
        {
            Console.WriteLine(count.ToString());
            //throw new NotImplementedException();
        }

        public void TestPassingBatchAsParam(ProcessBatch batch)
        {
            Console.WriteLine(batch.Name);

        }

        public void TestPassingInterfaceAsParam(IProcessBatch batch)
        {
            Console.WriteLine(batch.Name);

        }

        public void TestPassingComponentAsParam(ComponentNode workflowNode)
        {
            Console.WriteLine(workflowNode.ComponentName);

        }

        public void TestPassingComponentListAsParam(List<ComponentNode> workflowNodes)
        {
            Console.WriteLine(workflowNodes.Count);

        }

        #endregion

        private static void StartLog4Net()
        {
            //WinService.Log.Debug("test - Debug logging");
            //WinService.Log.Info("test - Info logging");
            //WinService.Log.Warn("test - Warn logging");
            //WinService.Log.Error("test - Error logging");
            //WinService.Log.Fatal("test - Fatal logging");

        }
    }
}
