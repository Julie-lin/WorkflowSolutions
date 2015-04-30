using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workflow.Data.Interfaces
{
    public interface IResultForNextNode
    {
        Guid ThisNodeId { get; set; }
        bool OkToContinue { get; set; }
        string ComponentNodeName { get; set; }
    }
}
