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
    [KnownType(typeof(ProcessJob))]
    public class ClientJob : ProcessJob
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ExcelOutput { get; set; }

    }
}
