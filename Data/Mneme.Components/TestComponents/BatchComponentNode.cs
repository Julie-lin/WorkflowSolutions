using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Workflow.Data;

namespace Mneme.Components
{
    [Serializable]
    [DataContract]
    public class BatchComponentNode : ComponentNode
    {
        public BatchComponentNode()
        {
            CompNodeValidation = NodeValidationType.Batch;
        }
        [DataMember]
        public string TestString { get; set; }
        [DataMember]
        public List<string> TestList { get; set; }

    }

}
