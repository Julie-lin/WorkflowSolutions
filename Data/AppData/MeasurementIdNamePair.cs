using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AppData
{
    [Serializable]
    [DataContract]
    public class MeasurementIdNamePair
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

}
