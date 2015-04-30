using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThermoFisher.CommonCore.Data.Business;
using ThermoFisher.CommonCore.Data.Interfaces;

namespace Mneme.Data
{
    public class MnemeChromatogramData : IChromatogramData
    {
        public MnemeChromatogramData()
        {
        }
        public MnemeChromatogramData(int first)
        {
            PositionsArray = new double[first][];
            ScanNumbersArray = new int[first][];
            IntensitiesArray = new double[first][];
            Length = first;
            Masses = new double[first];
        }
        public double[] Masses { get; set; }
        public double[][] PositionsArray { get; private set; }
        public int[][] ScanNumbersArray { get; private set; }
        public double[][] IntensitiesArray { get; private set; }
        public int Length { get; private set; }

        private int _currentCount = 0;
        public void AddChroSignal(MnemeChromatogramSignal signal)
        {
            PositionsArray[_currentCount] = signal.SignalTimes;
            ScanNumbersArray[_currentCount] = signal.SignalScans;
            IntensitiesArray[_currentCount] = signal.SignalIntensities;
            Masses[_currentCount] = signal.SignalMass;
            _currentCount++;
        }
        public void AddChroSignal(ChromatogramSignal signal)
        {
            PositionsArray[_currentCount] = signal.SignalTimes;
            ScanNumbersArray[_currentCount] = signal.SignalScans;
            IntensitiesArray[_currentCount] = signal.SignalIntensities;
            _currentCount++;
        }

    }
}
