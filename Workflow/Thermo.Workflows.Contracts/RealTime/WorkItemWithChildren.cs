using System;
using System.Collections.Generic;
using Thermo.Data.Hierarchical;

namespace Thermo.Workflows.Contracts.RealTime
{
    public class WorkItemWithChildren: IWorkItemWithChildren
    {
        public Guid Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string WorkItemType { get; set; }

        IEnumerable<IWorkItemWithChildren> IWorkItemWithChildren.Children
        {
            get
            {
                if(Children == null) return new List<IWorkItemWithChildren>();

                return Children;
            }
        }

        public List<WorkItemWithChildren> Children
        {
            get; set;
        }
    }
}
