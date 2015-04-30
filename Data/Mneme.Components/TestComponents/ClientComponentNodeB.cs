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
    public class ClientComponentNodeB : ComponentNode
    {
        public ClientComponentNodeB()
        {
            CompNodeValidation = NodeValidationType.Job;
        }
        [DataMember]
        public string TestString { get; set; }
        [DataMember]
        public List<string> TestList { get; set; }

    }

}
