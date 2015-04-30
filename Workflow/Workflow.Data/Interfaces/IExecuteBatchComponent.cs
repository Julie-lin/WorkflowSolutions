using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workflow.Data.Interfaces
{
    public interface IExecuteBatchComponent
    {
        IResultForNextNode ExecuteComponent(IProcessBatch iBatch, ICollection<ExtraProcessInfo> extraProcessInfo,
                                        List<ComponentNode> componentParams,
                                        ComponentNode thisComponentParam,
                                        INotificationCallback callback);

    }
}
