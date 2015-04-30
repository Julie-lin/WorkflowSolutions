using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mneme.Data.Interface
{
    public interface IRawDataEx
    {
        //        IEnumerator<int> GetScanIterator(ScanFilter filter);
        List<int> GetScanList(string filter);
        int GetSpectraCount();


        List<ScanBasePeakIntensityPair> GetScanBasePeakIntensities(int startScan, int endScan);
        List<ScanStartTimePair> GetScanStartTimeDetails();
        double RetentionTimeFromClosetScanNumber(int scanNumber);
        bool IsMsScan(int scanNumber);
        int MsScanNumberFromRetentionTime(double rt);
    }
}
