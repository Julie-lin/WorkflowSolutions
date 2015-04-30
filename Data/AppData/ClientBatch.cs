using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Workflow.Data;
using Workflow.Data.Interfaces;


namespace AppData
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(ProcessBatch))]
    public class ClientBatch : ProcessBatch
    {
        public string Description { get; set; }
    }
}
