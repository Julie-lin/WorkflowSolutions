using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Workflow.Data.Interfaces
{
    public interface IProcessJob
    {
        [DataMember]
        Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        string Name { get; set; }

        [DataMember]
        string Description { get; set; }

        [DataMember]
        Int64 JobWorkId { get; set; }

    }
}
