using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Workflow.Data.Interfaces
{
    [ServiceContract]
    public interface IProcessingService
    {
        [OperationContract(IsOneWay = true)]
        void ExecuteCompositeBatchSequentialActivity(ProcessBatch batch, List<ComponentNode> workflowNodes, Guid sessionName);

        [OperationContract(IsOneWay = true)]
        void ExecuteCompositeBatchParallelActivity(ProcessBatch batch, List<ComponentNode> workflowNodes, Guid sessionName);

        [OperationContract(IsOneWay = true)]
        void ExecutePostUploadActivity(long expId, long measurementId, List<ComponentNode> workflowNodes, Guid sessionName, Guid batchId);
        
        [OperationContract(IsOneWay = true)]
        void CancelWorkItem(List<Guid> rootToCanceledChildItemPath);

        [OperationContract]
        bool IsBatchRunning(Guid batchId);

        [OperationContract(IsOneWay = true)]
        void TestWorkflowAccess(int count);

        [OperationContract(IsOneWay = true)]
        void TestPassingBatchAsParam(ProcessBatch batch);

        [OperationContract(IsOneWay = true)]
        void TestPassingInterfaceAsParam(IProcessBatch batch);


        [OperationContract(IsOneWay = true)]
        void TestPassingComponentAsParam(ComponentNode workflowNode);


        [OperationContract(IsOneWay = true)]
        void TestPassingComponentListAsParam(List<ComponentNode> workflowNodes);

    }

}
