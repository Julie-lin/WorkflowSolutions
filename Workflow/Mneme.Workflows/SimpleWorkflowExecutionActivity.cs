using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Text.RegularExpressions;
using System.Threading;
using Mneme.WorkflowSolver;
using Workflow.Data;
using Workflow.Data.Interfaces;

namespace Mneme.Workflows
{

    public sealed class SimpleWorkflowExecutionActivity : AsyncCodeActivity
    {
        [RequiredArgument]
        public OutArgument<ProcessBatch> Batch { get; set; }
        [RequiredArgument]
        public InArgument<List<ComponentNode>> ComponentParameters { get; set; }

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            INotificationCallback callBack = context.GetExtension<INotificationCallback>();
            if (callBack != null)
            {
            }
            Thread thread = new Thread(DoProcess) { IsBackground = true };
            var startNode = ComponentParameters.Get(context)[0].FindStartupNode(ComponentParameters.Get(context));
            Workflow.Activities.AsyncNodeResult result = new Workflow.Activities.AsyncNodeResult(callback, state)
            {
                ComponentList = ComponentParameters.Get(context),
                StartNode = startNode,
                Batch = Batch.Get(context),
                Job = (ProcessJob)(Batch.Get(context)).Groups[0].Jobs[0],
                Group = Batch.Get(context).Groups[0],
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
                WorkflowSolver.WorkflowSolver solver = new WorkflowSolver.WorkflowSolver();
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
            throw new NotImplementedException();
        }
    }
}
