using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

using Workflow.Data.Interfaces;

namespace Mneme.Components
{
    public class TreetopResult : IResultForNextNode
    {
        public TreetopResult()
        {
            OkToContinue = true;
        }

        public Guid ThisNodeId { get; set; }

        public bool OkToContinue { get; set; }
        public string ComponentNodeName { get; set; }
        public DbContext Entities { get; set; }
        //public List<TTPeak> PeakResultList { get; set; }
    }
}
