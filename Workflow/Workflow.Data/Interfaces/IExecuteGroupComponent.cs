using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workflow.Data.Interfaces
{
    public interface IExecuteGroupComponent
    {
        IResultForNextNode ExecuteComponent(ProcessBatch batch, ProcessGroup group, 
                                        ICollection<ExtraProcessInfo> extraProcessInfo,
                                        List<ComponentNode> componentParams,
                                        ComponentNode thisComponentParam,
                                        INotificationCallback callback);
    }
}
