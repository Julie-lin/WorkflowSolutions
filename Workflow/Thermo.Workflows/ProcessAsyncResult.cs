using System;
using System.Threading;

namespace Thermo.Workflows
{
    public class ProcessAsyncResult<T> : ProcessAsyncResult
    {
        public ProcessAsyncResult(AsyncCallback callback, object asyncState, T value) 
            : base(callback, asyncState)
        {
            Value = value;
        }

        public T Value { get; private set;  }
    }

    public class ProcessAsyncResult : IAsyncResult
    {

        public ProcessAsyncResult(AsyncCallback callback, object asyncState)
        {
            Callback = callback;
            AsyncState = asyncState;
        }

        public Exception Exception { get; set; }
        public AsyncCallback Callback { get; set; }

        readonly ManualResetEvent _done = new ManualResetEvent(false);
        internal void Set()
        {
            _done.Set();
        }

        public bool IsCompleted
        {
            get { return _done.WaitOne(0); }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return _done; }
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
            _done.Dispose();
        }
    }
}
