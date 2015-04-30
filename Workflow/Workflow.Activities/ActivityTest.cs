using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workflow.Data;

namespace Workflow.Activities
{
    public class ActivityTest
    {
        public ActivityTest()
        {
            Id = Guid.NewGuid();
            Job = new ProcessJob();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ProcessJob Job { get; set; }
    }
}
