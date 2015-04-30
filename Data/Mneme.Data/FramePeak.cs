using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Mneme.Data.Interfaces;

namespace Mneme.Data
{
    [Serializable]
    [DataContract]
    public class FramePeak : IFramePeak
    {
        public FramePeak()
        {
            Times = string.Empty;
        }
        [DataMember]
        public long MeasurementId { get; set; }
        [DataMember]
        public int PeakId { get; set; }
        [DataMember]
        public double MZ { get; set; }
        [DataMember]
        public double RT { get; set; }
        [DataMember]
        public double Fit { get; set; }
        [DataMember]
        public double Width { get; set; }
        [DataMember]
        public double Area { get; set; }
        [DataMember]
        public double Intensities { get; set; }
        [DataMember]
        public string Times { get; set; }
        [DataMember]
        public int Charge { get; set; }
        [DataMember]
        public int Points { get; set; }
        [DataMember]
        public double SN { get; set; }

    }
}
