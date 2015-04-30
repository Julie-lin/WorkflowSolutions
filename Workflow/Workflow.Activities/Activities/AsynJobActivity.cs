using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Mneme.WorkflowSolver;
using Workflow.Data;
using Workflow.Data.Interfaces;

namespace Workflow.Activities
{
    public sealed class AsynJobActivity : AsyncCodeActivity
    {
        [RequiredArgument]
        public InArgument<List<ComponentNode>> ComponentParameters { get; set; }
        public InArgument<ComponentNode> StartParameter { get; set; }
        [RequiredArgument]
        public InArgument<ProcessJob> Job { get; set; }
        [RequiredArgument]
        public InArgument<ProcessGroup> Group { get; set; }
        [RequiredArgument]
        public InArgument<ProcessBatch> Batch { get; set; }
        public OutArgument<ComponentNode> NextParameter { get; set; }

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {

            INotificationCallback callBack = context.GetExtension<INotificationCallback>();
            if (callBack != null)
            {
                callBack.SendClientNotification(new ComponentProcessMessage().CreateServerMessage(
                    Batch.Get(context).Id,
                    Group.Get(context).Id,
                    Job.Get(context).Id,
                    "Job started"));
            }
            Thread thread = new Thread(DoProcess) { IsBackground = true };
            AsyncNodeResult result = new AsyncNodeResult(callback, state)
            {
                ComponentList = ComponentParameters.Get(context),
                StartNode = StartParameter.Get(context),
                Job = (ProcessJob)Job.Get(context),
                Batch = Batch.Get(context),
                Group = Group.Get(context),
                JobCallback = context.GetExtension<INotificationCallback>(),
                RunningThread = thread,
            };
            context.UserState = result;

            thread.Start(result);
            return result;
        }

        private void DoProcess(object state)
        {
            Thread.Sleep(100);
            AsyncNodeResult result = (AsyncNodeResult)state;


            try
            {
                if (result.StartNode == null)
                    return;

                WorkflowSolver solver = new WorkflowSolver();
                ComponentNode next = solver.SolveComplexComponentTree(result.ComponentList,
                    result.StartNode,
                    result.Batch.ExtraInfo,
                    result.JobCallback,
                    result.Batch,
                    result.Group,
                    result.Job);

                result.NextNode = next;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                //throw;
            }
            finally
            {
                OnCompleted(result);
            }
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
            AsyncNodeResult endResult = result as AsyncNodeResult;
            if (endResult.Exception != null)
            {
                INotificationCallback callBack = context.GetExtension<INotificationCallback>();
                if (callBack != null)
                {
                    callBack.SendClientNotification(new ComponentProcessMessage().CreateErrorMessage(
                        Batch.Get(context).Id,
                        Group.Get(context).Id,
                        Job.Get(context).Id,
                        endResult.Exception.Message));
                }
                log4net.ILog log4Net = context.GetExtension<log4net.ILog>();
                if (log4Net != null)
                {
                    log4Net.Fatal(new ComponentProcessMessage().CreateErrorMessage(
                        Batch.Get(context).Id,
                        Group.Get(context).Id,
                        Job.Get(context).Id,
                        endResult.Exception.Message));
                }

            }
            else
            {
                INotificationCallback callBack = context.GetExtension<INotificationCallback>();
                if (callBack != null)
                {
                    callBack.SendClientNotification(new ComponentProcessMessage().CreateServerMessage(
                        Batch.Get(context).Id,
                        Group.Get(context).Id,
                        Job.Get(context).Id,
                        "Job Ended"));
                }

            }


            if (endResult != null && endResult.NextNode != null)
            {
                NextParameter.Set(context, endResult.NextNode);
                endResult.Dispose();
            }

            if (endResult.Exception != null && !context.IsCancellationRequested)
            {
                //throw new ApplicationException("Error generating reports. See inner for details", endResult.Exception);
            }
        }

        protected override void Cancel(AsyncCodeActivityContext context)
        {
            AsyncNodeResult result =
                context.UserState as AsyncNodeResult;
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
