using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mneme.Data.Interfaces
{
    public interface IMnemePeak
    {
        Guid JobId { get; set; }
         int PeakId { get; set; }
         double MZ { get; set; }
         double RT { get; set; }
         double Fit { get; set; }
         double Width { get; set; }
         double Area { get; set; }
         double Intensities { get; set; }
         string Times { get; set; }
         int Charge { get; set; }
         int Points { get; set; }
         double SN { get; set; }
         
    }
}
