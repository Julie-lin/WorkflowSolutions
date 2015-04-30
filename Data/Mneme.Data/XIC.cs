using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mneme.Data
{
    public class XIC
    {
        public XIC()
        {
            Times = new List<double>();
            Intensities = new List<double>();
            MZs = new List<double>();
            Charges = new List<int>();
        }
        public List<double> Times { get; set; }
        public List<double> Intensities { get; set; }
        public List<double> MZs { get; set; }
        public List<int> Charges { get; set; }
        //public double MZ { get; set; }
    }

    public class XICExtract : XIC
    {
        private Dictionary<int, List<double>> m_mzs;
        private Dictionary<int, List<double>> m_int;
        private Dictionary<int, List<double>> m_chg;

        public XICExtract()
        {
            m_chg = new Dictionary<int, List<double>>();
            m_int = new Dictionary<int, List<double>>();
            m_mzs = new Dictionary<int, List<double>>();
        }

        public void Add(TracePoint tp)
        {

        }
    }
}
