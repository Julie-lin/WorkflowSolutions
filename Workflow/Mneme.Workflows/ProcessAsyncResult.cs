using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.Workflows
{
    public class ProcessAsyncResult : IAsyncResult
    {

        public ProcessAsyncResult(AsyncCallback callback, object asyncState)
        {
            Callback = callback;
            AsyncState = asyncState;
        }

        public Exception Exception { get; set; }
        public AsyncCallback Callback { get; set; }

        readonly ManualResetEvent done = new ManualResetEvent(false);
        internal void Set()
        {
            done.Set();
        }

        public bool IsCompleted
        {
            get { return done.WaitOne(0); }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return done; }
        }

        public object AsyncState
        {
            get;
            set;
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public bool Canceled { get; set; }

        public void Dispose()
        {
            done.Dispose();
        }
    }
}
