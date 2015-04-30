using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mneme.Data
{

    public class MnemeChromatogramSignal
    {
        public double SignalMass { get; set; }
        public int[] SignalScans { get; set; }
        public double[] SignalTimes { get; set; }
        public double[] SignalIntensities { get; set; }
    }
}
