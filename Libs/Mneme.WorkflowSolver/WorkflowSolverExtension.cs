using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mneme.ProcessLocator;
using Workflow.Data;
using Workflow.Data.Interfaces;

namespace Mneme.WorkflowSolver
{
    public static class WorkflowSolverExtension
    {
        //complex solver
        public static ComponentNode SolveComplexComponentTree(this WorkflowSolver solver,
            List<ComponentNode> paramList,
            ComponentNode startNode,
            ICollection<ExtraProcessInfo> extraProcessInfos,
            INotificationCallback callback,
            ProcessBatch batch,
            ProcessGroup group,
            ProcessJob job)
        {
            ComponentNode nextComponent = null;
            IEnumerable<ComponentNode> children = ExcuteComplexStartNode(paramList, startNode,
                extraProcessInfos,
                callback, batch, group, job);
            foreach (var componentParam in children)
            {
                var comparam = ExcuteComplexComponentNode(paramList, componentParam, callback, batch, group, job);
                if (nextComponent == null)
                {
                    nextComponent = comparam;
                }
            }
            return nextComponent;
        }


        private static IEnumerable<ComponentNode> ExcuteComplexStartNode(List<ComponentNode> paramList,
            ComponentNode startNode,
            ICollection<ExtraProcessInfo> extraProcessInfos,
            INotificationCallback callback,
            ProcessBatch batch,
            ProcessGroup group,
            ProcessJob job)
        {
            Guid zero = new Guid();
            IEnumerable<ComponentNode> childParams = from p in paramList where (p.ParentIdList.FirstOrDefault(pl => pl == startNode.Id) != zero) select p;
            //IExecuteStartupComponent component = ProcessObjectLocator.LocateStartComponentProcess(startNode.CompopnentExcutionName);
            //if this work no requirement for registering with unity container
            Type tp = ProcessRunTimeLocator.GetExecutaleStartupType(startNode.CompopnentExcutionName);
            if (tp == null)
                return new ArraySegment<ComponentNode>();
            IExecuteStartupComponent component = (IExecuteStartupComponent)Activator.CreateInstance(tp);
            if (component != null)
            {

                //do waht ever client initiation here
                IResultForNextNode obj = component.ExecuteStartupComponent(batch, group, job, extraProcessInfos,
                    paramList, startNode, callback);
                //since startup node takes raw file usually open it
                foreach (var param in paramList)
                {
                    //set iRaw Data to each ComponentParameters
                    param.StartupResult = obj;
                    param.TreeExecutionTag = startNode.TreeExecutionTag;
                    param.ParentComponentResults = new List<IResultForNextNode>();
                    param.ParentComponentResults.Add(obj);
                    param.ProcessedParentCount = 0;
                }
            }
            return childParams;
        }

        private static ComponentNode ExcuteComplexComponentNode(List<ComponentNode> paramList,
            ComponentNode thisNode,
            INotificationCallback callback,
            ProcessBatch batch,
            ProcessGroup group,
            ProcessJob job)
        {
            ComponentNode nextNode = null;
            if (thisNode.CompNodeValidation == NodeValidationType.Group)
            {
                return thisNode;
            }

            Debug.WriteLine(thisNode.ComponentName);
            thisNode.ProcessedParentCount++;
            if (thisNode.ProcessedParentCount != thisNode.ParentIdList.Count)
            {
                //    _excutableInWait.Add(thisNode);
                return null;
            }
            Guid zero = new Guid();
            var childrenParams = (from p in paramList
                                  where (p.ParentIdList.FirstOrDefault(pl => pl == thisNode.Id) != zero)
                                  select p).ToList();

            //            IEnumerable<ComponentNode> childrenParams = from p in paramList where p.ParentId == thisNode.Id select p;
            //IExcuteComponent component = ProcessObjectLocator.LocateComponentProcess(thisNode.CompopnentExcutionName);
            Type tp = ProcessRunTimeLocator.GetExecutableType(thisNode.CompopnentExcutionName);
            if (tp == null)
                return null;
            IExcuteComponent component = (IExcuteComponent)Activator.CreateInstance(tp);

            if (component != null)
            {
                IResultForNextNode ret = component.ExcuteThermoComponent(paramList, thisNode, callback, batch, group, job);
                if (ret != null)
                {
                    ret.ThisNodeId = thisNode.Id;
                    foreach (var param in childrenParams)
                    {
                        param.ParentComponentResults.Add(ret);
                    }
                }
            }
            thisNode.ParentComponentResults.Clear();

            //_excutableInWait.Remove(thisNode);
            thisNode.ProcessedParentCount = 0;
            foreach (var childrenParam in childrenParams)
            {
                nextNode = ExcuteComplexComponentNode(paramList, childrenParam, callback, batch, group, job);
            }
            return nextNode;
        }

    }
}
