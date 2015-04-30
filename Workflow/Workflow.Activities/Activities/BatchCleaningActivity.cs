using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Workflow.Data;

namespace Workflow.Activities
{
    public sealed class BatchCleaningActivity : AsyncCodeActivity
    {
        public InArgument<ProcessBatch> Target { get; set; }


        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            Thread thread = new Thread(DoProcess) { IsBackground = true };
            BatchCleaningAsyncResult result = new BatchCleaningAsyncResult(callback, state)
            {
                Batch = Target.Get(context),

                RunningThread = thread,
            };
            context.UserState = result;

            thread.Start(result);
            return result;
        }

        private void DoProcess(object state)
        {
            Thread.Sleep(5000);
            BatchCleaningAsyncResult result = (BatchCleaningAsyncResult)state;
            OnCompleted(result);

            //try
            //{
            //    IProcess process = ProcessObjectLocator.LocateProcess(result.Batch.CleanupJobName);
            //    if (process != null)
            //    {
            //        process.DoProcess(result.Batch, null, null);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    result.Exception = ex;
            //}
            //finally
            //{
            //    OnCompleted(result);
            //}
        }

        private static void OnCompleted(ProcessAsyncResult state)
        {
            state.Set();

            if (state.Callback != null)
            {
                state.Callback(state);
            }
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            BatchCleaningAsyncResult processResult = result as BatchCleaningAsyncResult;
            if (processResult == null)
            {
                return;
            }
            processResult.Dispose();

            if (processResult.Exception != null && !context.IsCancellationRequested)
            {
                throw new ApplicationException("Error generating reports. See inner for details", processResult.Exception);
            }
        }

        protected override void Cancel(AsyncCodeActivityContext context)
        {
            BatchCleaningAsyncResult result =
                context.UserState as BatchCleaningAsyncResult;
            context.MarkCanceled();

            if (result != null)
            {
                result.Canceled = true;
                if (result.RunningThread.IsAlive)
                {
                    result.RunningThread.Abort();
                }
            }
            base.Cancel(context);
        }
    }
}
