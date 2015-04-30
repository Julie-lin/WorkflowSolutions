using System;
using System.Collections.Generic;
using System.Text;

namespace Thermo.Data.Hierarchical
{
    public interface ICancellableWorkItemWithChildren 
        : IWorkItemWithChildren
    {
        bool HasCancellationRequested { get; set; }
        bool SupportsCancellation{ get; }
    }
}
