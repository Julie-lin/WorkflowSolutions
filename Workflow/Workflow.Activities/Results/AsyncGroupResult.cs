using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Workflow.Data;
using Workflow.Data.Interfaces;

namespace Workflow.Activities
{
    public class AsyncGroupResult : ProcessAsyncResult
    {
        public AsyncGroupResult(AsyncCallback callback, object asyncState)
            : base(callback, asyncState)
        {
        }

        public Thread RunningThread { get; set; }

        public ComponentNode GroupNode { get; set; }
        public ComponentNode NextNode { get; set; }
        public List<ComponentNode> ComponentList { get; set; }
        public ProcessGroup Group { get; set; }
        public ProcessBatch Batch { get; set; }

        public INotificationCallback GroupCallback { get; set; }

    }
}
