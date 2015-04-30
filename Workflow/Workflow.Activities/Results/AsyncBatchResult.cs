using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Workflow.Data;
using Workflow.Data.Interfaces;

namespace Workflow.Activities
{
    public class AsyncBatchResult : ProcessAsyncResult
    {
        public AsyncBatchResult(AsyncCallback callback, object asyncState)
            : base(callback, asyncState)
        {
        }

        public Thread RunningThread { get; set; }

        public ComponentNode BatchNode { get; set; }
        public ComponentNode NextNode { get; set; }
        public List<ComponentNode> ComponentList { get; set; }
        public ProcessBatch Batch { get; set; }
        public INotificationCallback BatchCallback { get; set; }

    }
}
