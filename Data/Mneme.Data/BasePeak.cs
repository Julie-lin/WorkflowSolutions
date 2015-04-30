using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Mneme.Data
{
    [Serializable]
    [DataContract]
    public class BasePeak
    {
        [DataMember]
        public double Mass { get; set; }
        [DataMember]
        public double Intensity { get; set; }
    }
}
