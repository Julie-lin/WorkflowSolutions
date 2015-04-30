using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Mneme.ProcessLocator;
using Workflow.Data;
using Workflow.Data.Interfaces;

namespace Workflow.Activities
{
    public sealed class AsyncBatchActivity : AsyncCodeActivity
    {
        public InArgument<List<ComponentNode>> ComponentList{ get; set; }
        public InArgument<ComponentNode> BatchNode { get; set; }
        public InArgument<ProcessBatch> Batch { get; set; }
        public OutArgument<ComponentNode> NextNode { get; set; }
        
        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            var callBack = context.GetExtension<INotificationCallback>();
            if (callBack != null)
            {
                callBack.SendClientNotification(new ComponentProcessMessage().CreateServerMessage(Batch.Get(context).Id,
                                                                                                  new Guid(), new Guid(),
                                                                                                  "Batch component started"));
            }

            Thread thread = new Thread(DoProcess) { IsBackground = true };


            AsyncBatchResult result = new AsyncBatchResult(callback, state)
            {
                ComponentList = ComponentList.Get(context),
                BatchNode = BatchNode.Get(context),
                Batch = Batch.Get(context),
                BatchCallback = callBack,
                RunningThread = thread,
            };
            context.UserState = result;

            thread.Start(result);
            return result;
        }

        private void DoProcess(object state)
        {
            Thread.Sleep(100);
            AsyncBatchResult result = (AsyncBatchResult)state;
            if (result.BatchNode != null)
            {
                if (result.BatchNode.CompNodeValidation == NodeValidationType.Batch)
                {
                    result.NextNode = result.BatchNode.FindChild(result.ComponentList) ?? null;
                }
                else
                {
                    result.NextNode = result.BatchNode;//user may not select a batch node, then next must be start node
                }
            }


            try
            {
                if (result.BatchNode != null)
                {
                    //IExecuteBatchComponent component = ProcessObjectLocator.LocateBatchComponentProcess(result.BatchNode.CompopnentExcutionName);
                    Type tp = ProcessRunTimeLocator.GetExecutableBatchType(result.BatchNode.CompopnentExcutionName);
                    if (tp == null)
                        return;
                    IExecuteBatchComponent component = (IExecuteBatchComponent)Activator.CreateInstance(tp);

                    if (component != null)
                    {
                        var nextNodeResult = component.ExecuteComponent(result.Batch,
                            result.Batch.ExtraInfo, result.ComponentList,
                            result.BatchNode,
                            result.BatchCallback);
                        if (result.NextNode != null)
                        {
                            result.NextNode.ParentComponentResults.Add(nextNodeResult);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                throw;
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
            AsyncBatchResult endResult = result as AsyncBatchResult;
            if (endResult != null)
            {
                NextNode.Set(context, endResult.NextNode);
                endResult.Dispose();
            }

            if (endResult.Exception != null && !context.IsCancellationRequested)
            {
                throw new ApplicationException("Error generating reports. See inner for details", endResult.Exception);
            }
            if (endResult.Exception != null)
            {
                INotificationCallback callBack = context.GetExtension<INotificationCallback>();
                callBack.SendClientNotification(new ComponentProcessMessage().CreateErrorMessage(
                    Batch.Get(context).Id,
                    new Guid(),
                    new Guid(),
                    endResult.Exception.Message));
            }
            else if (endResult.NextNode == null)
            {
                INotificationCallback callBack = context.GetExtension<INotificationCallback>();
                if (callBack != null)
                {
                    callBack.SendClientNotification(new ComponentProcessMessage().CreateServerMessage(
                        Batch.Get(context).Id,
                        new Guid(),
                        new Guid(),
                        "Batch Ended"));
                }

            }

        }

        protected override void Cancel(AsyncCodeActivityContext context)
        {
            AsyncBatchResult result =
                context.UserState as AsyncBatchResult;
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
