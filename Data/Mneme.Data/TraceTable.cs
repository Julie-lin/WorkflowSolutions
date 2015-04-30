using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mneme.Data
{
    public class TraceTable
    {
        public TraceTable(double mzlo, double mzhi, double increment)
        {
            Traces = new Dictionary<double, TraceHelper>();
            BuildTable(mzlo, mzhi, increment);
        }
        private double m_mzlo = 0;
        private double m_mzhi = 0;
        private double m_increment = 0;
        public Dictionary<double, TraceHelper> Traces;
        public void Add(TracePoint tp)
        {
            double idx = Math.Floor(tp.MZ);
            if (Traces.ContainsKey(idx))
            {
                Traces[idx].Points.Add(tp);
            }
        }
        public void BuildTable(double mzlow, double mzhigh, double mzincrement)
        {
            double dmzlow = Math.Floor(mzlow);
            double dmzhigh = Math.Ceiling(mzhigh);
            for (double mzbase = dmzlow; mzbase < dmzhigh; mzbase = mzbase + mzincrement)
            {
                Traces[mzbase] = new TraceHelper(mzbase, mzbase + 1.0);
            }
            m_mzhi = dmzhigh;
            m_mzlo = dmzlow;
            m_increment = mzincrement;
        }
    }
}
