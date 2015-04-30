using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mneme.Data
{
    [Serializable]
    public class ScanMassIntensityPair
    {
        public double[] Masses { get; set; }
        public double[] Intensities { get; set; }
    }
    [Serializable]
    public class ScanNumberMassIntensity
    {
        public int Index { get; set; }
        public double[] Masses { get; set; }
        public double[] Intensities { get; set; }

    }

}
