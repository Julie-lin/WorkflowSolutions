using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Workflow.Data;

namespace AppData
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(ProcessGroup))]
    public class ClientGroup : ProcessGroup
    {
        public string Description { get; set; }
    }
}
