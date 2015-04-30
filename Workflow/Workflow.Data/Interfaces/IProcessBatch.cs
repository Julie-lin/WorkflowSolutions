using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Workflow.Data.Interfaces
{
    public interface IProcessBatch
    {
        [DataMember]
        Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        string Name { get; set; }
        [DataMember]
        string Path { get; set; }

        [DataMember]
        string Description { get; set; }
        //jlin to do list add propertiy list of iProcessgroup
        IProcessGroup GetProcessGroup(Guid sequenceId);
    }
}
