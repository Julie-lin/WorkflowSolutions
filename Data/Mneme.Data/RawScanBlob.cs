using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mneme.Data
{
    [Serializable]
    public class RawScanBlob
    {
        public float[] Intensity { get; set; }
        public double[] MZ { get; set; }
        public Int16[] Charge { get; set; }
        public float[] Baseline { get; set; }
        public float[] Noise { get; set; }
        public float[] Resolution { get; set; }
        public string Flags { get; set; }
    }
                    

}
