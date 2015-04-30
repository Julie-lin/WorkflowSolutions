using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mneme.Data
{
    public class FrameDataPoint : IComparable
    {
        public double Intensity = 0;
        public double Mz = 0;
        public double AtStart = 0;
        public double AtStop = 0;
        public int Charge = 0;
        public FrameDataPoint()
        {
            
        }
        public FrameDataPoint(double i, double m, double tstart, double tstop, int charge)
        {
            Intensity = i;
            Mz = m;
            AtStart = tstart;
            AtStop = tstop;
            Charge = charge;
        }
        public int CompareTo(Object obj)
        {
            FrameDataPoint pobj = (FrameDataPoint)obj;
            if (pobj.Intensity > Intensity) return 1;
            if (pobj.Intensity < Intensity) return -1;
            return 0;
        }
    }
}
