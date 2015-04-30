using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mneme.Data.Interfaces
{
    public interface IMenePeakDiscover
    {
        MnemeChromatogramData GetChromatogramFromTrace(
            List<double> masses, double ppm, int startScan, int endScan);

        MnemeChromatogramData GetChromatogramFromTraceForFrame(double mz, double ppm, double rtLow,
                                                               double rtHigh);

        List<double> GetUnknownMassListforMassRangeAndScans(int threshold, double ppm,
                                                            double startMz, double endMz, 
                                                            int satrtScan, int endScan);

        List<double> GetUnknownMassListWithNScanRule(int threshold, double ppm, int nScanRule,
                                                     double startMz, double endMz, int startScan, int endScan);

        List<FrameDataPoint> GetFrameDataPoints(int threshold,  double tWidth, double ppm,
                                                    double startMz, double endMz,
                                                    int satrtScan, int endScan);

    }
}
