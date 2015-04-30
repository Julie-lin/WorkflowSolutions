using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Mneme.Data
{
    [Serializable]
    [DataContract]
    public class MassIntensityCharge
    {
        public double[] Masses { get; set; }
        public float[] Intensities { get; set; }
        public float[] Charges { get; set; }
    }
}
