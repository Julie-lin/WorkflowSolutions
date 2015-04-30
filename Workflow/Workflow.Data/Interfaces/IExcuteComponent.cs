using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workflow.Data.Interfaces
{

    public interface IExcuteComponent
    {
        IResultForNextNode ExcuteThermoComponent(List<ComponentNode> componentParams,
                                                 ComponentNode thisComponentParam,
                                                 INotificationCallback callback,
                                                 ProcessBatch batch,
                                                 ProcessGroup groupId,
                                                 ProcessJob jobId);

    }
}
