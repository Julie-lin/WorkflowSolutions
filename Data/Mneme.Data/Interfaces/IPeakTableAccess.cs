using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mneme.Data.Interfaces
{
    public interface IPeakTableAccess
    {
        void WriteToPeakTable(List<IMnemePeak> peaks, Guid jobId);
    }
}
