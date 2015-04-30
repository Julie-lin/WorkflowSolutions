using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workflow.Data;


namespace AppData
{
    public class ExecutionResult
    {
        public string ExecutionName { get; set; }

        public List<ComponentNode> TreeInfo { get; set; }
        //[XmlIgnore]
        public HashSet<Guid> ExecutionTreeInfo { get; set; }

        public string RawfileName { get; set; }

    }
}
