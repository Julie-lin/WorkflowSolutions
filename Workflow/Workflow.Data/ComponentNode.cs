using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Workflow.Data.Interfaces;

namespace Workflow.Data
{
    public enum NodeValidationType
    {
        NoType = -1,
        Start,
        Job,
        Group,
        Batch,
    }

    [Serializable]
    [DataContract]
    public class ComponentNode
    {
        public ComponentNode()
        {
            ParentIdList = new List<Guid>();
            ComponentName = "";
            CompopnentExcutionName = "";
            TreeExecutionTag = "";
            SingleInput = false;
            SingleOutput = false;
            CompNodeValidation = NodeValidationType.NoType;
            Chidren = new List<ComponentNode>();
        }
        [DataMember]
        [Browsable(false)]
        public Guid Id
        {
            get;
            set;
        }
        [DataMember]
        [Browsable(false)]
        public bool StartNode { get; set; }

        [DataMember]
        [Browsable(false)]
        public NodeValidationType CompNodeValidation { get; set; }
        [DataMember]
        [Browsable(false)]
        public bool SingleInput { get; set; }

        [DataMember]
        [Browsable(false)]
        public bool SingleOutput { get; set; }


        [DataMember]
        [Browsable(false)]
        public string TreeExecutionTag { get; set; }

        [DataMember]
        [Browsable(false)]
        public string CompopnentExcutionName { get; set; }

        [DataMember]
        [Browsable(false)]
        public string BatchInitializeExcutionName { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public int ProcessedParentCount { get; set; }

        [DataMember]
        [Browsable(false)]
        public List<Guid> ParentIdList { get; set; }

        [DataMember]
        public string ComponentName { get; set; }

        [DataMember]
        [Browsable(false)]
        public List<ComponentNode> Chidren { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public virtual IResultForNextNode StartupResult { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        [DataMember]
        public List<IResultForNextNode> ParentComponentResults { get; set; }


    }

}
