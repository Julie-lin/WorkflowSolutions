using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workflow.Data.Interfaces;

namespace Mneme.Components
{
    public class HelloWorldResult : IResultForNextNode
    {
        public HelloWorldResult()
        {
            OkToContinue = true;
        }

        public Guid ThisNodeId { get; set; }

        public bool OkToContinue { get; set; }
        public string ComponentNodeName { get; set; }
        //process specific properties
        public string HelloWorldString { get; set; }
    }
}
