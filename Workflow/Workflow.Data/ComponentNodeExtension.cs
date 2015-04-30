using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workflow.Data
{
    public static class ComponentNodeExtension
    {
        public static void FindParentTree(this ComponentNode thisParam,
            List<ComponentNode> componentParams,
            HashSet<ComponentNode> parentTreeParams)
        {
            foreach (Guid id in thisParam.ParentIdList)
            {
                ComponentNode param = componentParams.FirstOrDefault(c => c.Id == id);
                if (param != null)
                {
                    parentTreeParams.Add(param);
                    FindParentTree(param, componentParams, parentTreeParams);
                }
            }
        }

        public static void FindParentTreeIds(this ComponentNode thisParam,
            List<ComponentNode> componentParams,
            HashSet<Guid> parentTreeParams)
        {
            foreach (Guid id in thisParam.ParentIdList)
            {
                ComponentNode param = componentParams.FirstOrDefault(c => c.Id == id);
                if (param != null)
                {
                    parentTreeParams.Add(param.Id);
                    FindParentTreeIds(param, componentParams, parentTreeParams);
                }
            }
        }

        public static ComponentNode FindNodeByName(this ComponentNode thisParam, string name,
                List<ComponentNode> componentParams)
        {
            return componentParams.FirstOrDefault(c => c.ComponentName == name);
        }

        public static string FindTreeExecutionTag(this ComponentNode thisParam, List<ComponentNode> componentParams)
        {
            ComponentNode param = componentParams.FirstOrDefault(c => c.StartNode);
            if (param != null)
            {
                return param.TreeExecutionTag;
            }
            return "";
        }

        public static List<ComponentNode> FindThisExecutableInTree(this ComponentNode thisParam, List<ComponentNode> componentParams)
        {
            return componentParams.FindAll(c => c.CompopnentExcutionName == thisParam.CompopnentExcutionName).ToList();
        }

        public static ComponentNode FindStartupNode(this ComponentNode thisParam, List<ComponentNode> componentParams)
        {
            return componentParams.FirstOrDefault(c => c.StartNode == true);
        }

        public static List<ComponentNode> FindChildren(this ComponentNode thisParam, List<ComponentNode> componentParams)
        {

            return componentParams.FindAll(c => (c.ParentIdList.FirstOrDefault(p => p == thisParam.Id) != new Guid()));
        }
        public static ComponentNode FindChild(this ComponentNode thisParam, List<ComponentNode> componentParams)
        {

            return thisParam.FindChildren(componentParams).Count > 0
                                              ? thisParam.FindChildren(componentParams)[0]
                                              : null;
        }

    }
}
