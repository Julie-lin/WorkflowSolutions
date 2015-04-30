using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThermoFisher.CommonCore.Data.Business;
using ThermoFisher.CommonCore.Data.Interfaces;

namespace Mneme.Data
{
    public class MnemeChromatogramSettings : IChromatogramSettings
    {
        public MnemeChromatogramSettings()
        {
        }

        public MnemeChromatogramSettings(double delayInMin, string filter, double fragmentMass,
            bool includeRef,
            int massRangeCount,
            Range[] massRanges,
            TraceType trace)
        {
            DelayInMin = delayInMin;
            Filter = filter;
            FragmentMass = fragmentMass;
            IncludeReference = includeRef;
            MassRangeCount = massRangeCount;
            MassRanges = massRanges;
            Trace = trace;
        }
        public double DelayInMin { get; private set; }
        public string Filter { get; private set; }
        public double FragmentMass { get; private set; }
        public bool IncludeReference { get; private set; }
        public int MassRangeCount { get; private set; }
        public Range[] MassRanges { get; private set; }
        public TraceType Trace { get; private set; }
    }
}
