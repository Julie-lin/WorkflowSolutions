using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workflow.Data.Interfaces;

namespace Mneme.Components
{
    
    public class TraceBuildResult : IResultForNextNode
    {
        public TraceBuildResult()
        {
            OkToContinue = true;
        }

        public Guid ThisNodeId { get; set; }

        public bool OkToContinue { get; set; }
        public string ComponentNodeName { get; set; }
        //process specific properties
        public string TraceBuldString { get; set; }
    }
}
