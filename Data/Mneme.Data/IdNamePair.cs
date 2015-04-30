using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Mneme.Data
{
    [Serializable]
    [DataContract]
    public class IdNamePair
    {
        [DataMember]
        public Int64 Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
