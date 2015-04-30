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
    public class ComponentConnectionInfo// : IComponentConnectionInfo
    {
        public ComponentConnectionInfo()
        {
            SourceConnectorName = "";
        }
        [DataMember]
        public Guid SourceId { get; set; }
        [DataMember]
        public Guid SinkId { get; set; }
        [DataMember]
        public string SourceConnectorName { get; set; }
        [DataMember]
        public string SinkConnectorName { get; set; }
        [DataMember]
        public string SourceArrowSymbol { get; set; }
        [DataMember]
        public string SinkArrowSymbol { get; set; }
        [DataMember]
        public int ZIndex { get; set; }


    }


}
