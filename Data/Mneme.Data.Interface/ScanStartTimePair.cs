using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mneme.Data.Interface
{
    [Serializable]
    public class ScanStartTimePair
    {
        public int ScanNumber { get; set; }
        public double StartTime { get; set; }
        public string ScanType { get; set; }

    }
}
