using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AppData.Interfaces;

namespace AppData
{
    [Serializable]
    [DataContract]
    public class ComponentUIInfo : IComponentUI
    {
        [DataMember]
        public double Left { get; set; }
        [DataMember]
        public double Top { get; set; }
        [DataMember]
        public double Width { get; set; }
        [DataMember]
        public double Heigh { get; set; }
        [DataMember]
        public int ZIndex { get; set; }
        [DataMember]
        public bool IsGroup { get; set; }
        [DataMember]
        public Guid ParentId { get; set; }
        [DataMember]
        public string Content { get; set; }

    }
}
