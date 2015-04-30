using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Thermo.Workflows.Contracts.RealTime;
using Thermo.Workflows.RealTime;
using Workflow.Activities;
using Workflow.Data;
using Workflow.Data.Interfaces;

namespace MnemeConsoleHost
{
    //[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ProcessEngineService : IProcessingService
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IWorkflowInstanceManager _wfInstanceManager;
        public ProcessEngineService(IWorkflowInstanceManager manager)
        {
            _wfInstanceManager = manager;
            StartLog4Net();
        }

        public ProcessEngineService()
        {

            _wfInstanceManager = new WorkflowInstanceManager(null,
                                                             (message) => new ExceptionInfo(message).ToString())
            {
                OnWorkflowInstanceCompleted = (rootWorkItem) =>
                {
                    _log.Fatal("test ========== done with this workflow");
                    XmlSerializer serializer = new XmlSerializer(typeof(string));
                    using (Stream serializationStream = File.Create("C:\\Temp\\expInfo.xml"))
                    {
                        // serializer.Serialize(serializationStream, rootWorkItem);
                    }
                }
            };
        }

        #region Implementation of IProcessingService


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
            workflowApplication.Extensions.Add(_log);
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
            workflowApplication.Extensions.Add(_log);
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
            _log.Debug("test - Debug logging");
            _log.Info("test - Info logging");
            _log.Warn("test - Warn logging");
            _log.Error("test - Error logging");
            _log.Fatal("test - Fatal logging");

        }
    }

}
