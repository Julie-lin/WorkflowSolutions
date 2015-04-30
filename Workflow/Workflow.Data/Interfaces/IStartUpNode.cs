using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Workflow.Data.Interfaces
{
    public interface IStartUpNode
    {
        [DataMember]
        Guid ExecuteWorkflowId { get; set; }

    }
}
