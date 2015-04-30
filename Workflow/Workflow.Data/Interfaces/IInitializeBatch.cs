using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workflow.Data.Interfaces
{
    public interface IInitializeBatch
    {
        IResultForNextNode ExecuteBatchInitializer(ProcessBatch iBatch, List<ComponentNode> components);
    }
}
