using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mneme.Interfaces
{
    [Serializable]
    public class ScanStartTimePair
    {
        public int ScanNumber { get; set; }
        public double StartTime { get; set; }
        public string ScanType { get; set; }

    }

}
