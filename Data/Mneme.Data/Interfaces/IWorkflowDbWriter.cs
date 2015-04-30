using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workflow.Data;

namespace Mneme.Data.Interfaces
{
    public interface IWorkflowDbWriter
    {
        void AddBatchToDatabase(ProcessBatch batch, ProcessGroup group, ProcessJob job,
                                        List<ComponentNode> componentParams);

        void AddBatchToDatabase(ProcessBatch batch, List<ComponentNode> componentParams);
        void WriteToPeakTable(List<IMnemePeak> peaks, Guid jobId);
    }
}
