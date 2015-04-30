using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workflow.Data;


namespace AppData
{
    public class PmfResult : ExecutionResult
    {
        public PmfResult()
        {
            ExecutionName = "";
            TreeInfo = new List<ComponentNode>();
            ExecutionTreeInfo = new HashSet<Guid>();
            RawfileName = "";
        }
        //public string ExecutionName { get; set; }

        //public List<ComponentParam> TreeInfo{ get; set; }
        ////[XmlIgnore]
        //public HashSet<Guid> ExecutionTreeInfo { get; set; }

        //public string RawfileName{ get; set; }

        //public double TreeMassTolerance{ get; set; }

        //public List<SearchResult> SearchResult { get; set; }
    }

}
