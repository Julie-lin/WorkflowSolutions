using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Workflow.Data;

namespace Workflow.Activities
{
    public class BatchCleaningAsyncResult : ProcessAsyncResult
    {
        public BatchCleaningAsyncResult(AsyncCallback callback, object asyncState)
            : base(callback, asyncState)
        {
        }

        public Thread RunningThread { get; set; }

        public ProcessBatch Batch { get; set; }

    }
}
