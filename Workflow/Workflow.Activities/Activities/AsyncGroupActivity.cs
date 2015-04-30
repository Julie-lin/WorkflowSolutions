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
    public sealed class AsyncGroupActivity : AsyncCodeActivity
    {
        public InArgument<List<ComponentNode>> ComponentList { get; set; }
        public InArgument<ComponentNode> GroupNode { get; set; }
        public InArgument<ProcessBatch> Batch { get; set; }
        public InArgument<ProcessGroup> Group { get; set; }
        public OutArgument<ComponentNode> NextNode { get; set; }


        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            INotificationCallback callBack = context.GetExtension<INotificationCallback>();
            if (callBack != null)
            {
                callBack.SendClientNotification(new ComponentProcessMessage().CreateServerMessage(
                    Batch.Get(context).Id,
                    Group.Get(context).Id, new Guid(),
                    "Group Started"));
            }
            Thread thread = new Thread(DoProcess) { IsBackground = true };
            var result = new AsyncGroupResult(callback, state)
            {
                ComponentList = ComponentList.Get(context),
                GroupNode = GroupNode.Get(context),
                Batch = Batch.Get(context),
                Group = Group.Get(context),
                RunningThread = thread,
                GroupCallback = context.GetExtension<INotificationCallback>()
            };

            context.UserState = result;

            thread.Start(result);
            return result;
        }

        private void DoProcess(object state)
        {
            Thread.Sleep(100);

            AsyncGroupResult result = (AsyncGroupResult)state;

            if (result.GroupNode != null)
            if (result.GroupNode.CompNodeValidation == NodeValidationType.Group)
            {
                result.NextNode = result.GroupNode.FindChild(result.ComponentList) ?? null;
            }
            else
            {
                result.NextNode = result.GroupNode;//user may not select a group param, next is the batch param
            }

            try
            {
                if (result.GroupNode != null)
                {
                    //IExecuteGroupComponent component =
                    //    ProcessObjectLocator.LocateGroupComponentProcess(result.GroupNode.CompopnentExcutionName);

                    Type tp = ProcessRunTimeLocator.GetExecutableGroupType(result.GroupNode.CompopnentExcutionName);
                    if (tp == null)
                        return;
                    IExecuteGroupComponent component = (IExecuteGroupComponent)Activator.CreateInstance(tp);

                    if (component != null)
                    {
                        var nextNodeResult = component.ExecuteComponent(result.Batch, result.Group, result.Batch.ExtraInfo,
                                                                        result.ComponentList, result.GroupNode,
                                                                        result.GroupCallback);
                        if (result.NextNode != null)
                        {
                            result.NextNode.ParentComponentResults.Add(nextNodeResult);
                        }

                        //clear this group parent results
                        result.GroupNode.ParentComponentResults.Clear();

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
            AsyncGroupResult endResult = result as AsyncGroupResult;
            if (endResult != null)
            {
                NextNode.Set(context, endResult.NextNode);
                endResult.Dispose();
            }

            if (endResult.Exception != null)
            {
                INotificationCallback callBack = context.GetExtension<INotificationCallback>();
                callBack.SendClientNotification(new ComponentProcessMessage().CreateErrorMessage(
                    Batch.Get(context).Id,
                    Group.Get(context).Id,
                    new Guid(),
                    endResult.Exception.Message));
            }
            else
            {
                INotificationCallback callBack = context.GetExtension<INotificationCallback>();
                if (callBack != null)
                {
                    callBack.SendClientNotification(new ComponentProcessMessage().CreateServerMessage(
                        Batch.Get(context).Id,
                        Group.Get(context).Id,
                        new Guid(),
                        "Group Ended"));
                }
            }



            if (endResult.Exception != null && !context.IsCancellationRequested)
            {
                throw new ApplicationException("Error generating reports. See inner for details", endResult.Exception);
            }
        }

        protected override void Cancel(AsyncCodeActivityContext context)
        {
            AsyncGroupResult result =
                context.UserState as AsyncGroupResult;
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
