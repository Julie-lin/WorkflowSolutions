using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workflow.Data.Interfaces
{
    public interface IExecuteStartupComponent
    {
        IResultForNextNode ExecuteStartupComponent(ProcessBatch batch, ProcessGroup group, ProcessJob job,
                                ICollection<ExtraProcessInfo> extraProcessInfos,
                                List<ComponentNode> componentParams,
                                ComponentNode thisComponentParam,
                                INotificationCallback callback);

    }
}
