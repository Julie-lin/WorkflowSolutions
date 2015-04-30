using System;
using System.Collections.Generic;

namespace Thermo.Data.Hierarchical
{
    public interface IWorkItemWithChildren
    {
        Guid Id { get;}
        string Name { get;}
        string WorkItemType { get; }
        IEnumerable<IWorkItemWithChildren> Children { get;}
    }
}
