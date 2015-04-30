using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Thermo.Data.Hierarchical;
using Thermo.Workflows.Contracts.RealTime;

namespace Thermo.Workflows.Contracts
{
    public static class WorkItemTreeHelperExtensions
    {
        public static IWorkItemWithChildren GoToChild(
            this IWorkItemWithChildren parent, IList<Guid> pathToChild)
        {
            IWorkItemWithChildren iterator = parent;
            if(pathToChild == null || pathToChild.Count == 0)
            {
                throw new ArgumentException("You need to provide a valid pathToChild", "pathToChild");
            }
            if(parent == null)
            {
                throw new ArgumentNullException("parent", "parent can not be null");
            }
            if(iterator.Id != pathToChild[0])
            {
                throw new ArgumentException("path to Child needs to start with the parent", "pathToChild");
            }

            foreach (Guid pathNode in pathToChild.Skip(1))
            {
                iterator = iterator.Children.First(child => child.Id == pathNode);
            }

            return iterator;
        }

        public static IEnumerable<IWorkItemWithChildren> SuccesorsAndSelf(this IWorkItemWithChildren parent)
        {
            Queue<IWorkItemWithChildren> nodesToVisit = new Queue<IWorkItemWithChildren>();
            nodesToVisit.Enqueue(parent);

            while (nodesToVisit.Count > 0)
            {
                IWorkItemWithChildren currentNode = nodesToVisit.Dequeue();
                foreach (IWorkItemWithChildren child in currentNode.Children)
                {
                    nodesToVisit.Enqueue(child);
                }
                yield return currentNode;
            }
        }

        private static List<IWorkItemWithChildren> RouteToParentLookup(Guid taskItem, IWorkItemWithChildren current)
        {
            if (current.Id == taskItem) return new List<IWorkItemWithChildren>{current};

            List<IWorkItemWithChildren> routeToParent = (from child in current.Children
                                              let route = RouteToParentLookup(taskItem, child)
                                              where route != null
                                              select route).FirstOrDefault();
            
            if (routeToParent != null)
            {
                routeToParent.Add(current);
            }
            return routeToParent;
        }

        public static List<IWorkItemWithChildren> RouteFromParentToChild(this IWorkItemWithChildren parent, Guid childId)
        {
            List<IWorkItemWithChildren> routeToParent = RouteToParentLookup(childId, parent);
            if (routeToParent != null)
            {
                routeToParent.Reverse();
            }
            return routeToParent;
        }

        public static WorkItemWithHistory CreateLookupTree(this IWorkItemWithChildren rootItem)
        {
            return new WorkItemWithHistory
            {
                Id = rootItem.Id,
                Name = rootItem.Name,
                WorkItemType = rootItem.WorkItemType,
                SupportsCancellation = rootItem is ICancellableWorkItemWithChildren
                && ((ICancellableWorkItemWithChildren)rootItem).SupportsCancellation
                //Children = new ObservableCollection<WorkItemWithHistory>(
                //    from child in rootItem.Children select CreateLookupTree(child)),
            };
        }
    }
}
