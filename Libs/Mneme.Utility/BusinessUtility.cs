using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mneme.Utility
{
    public class BusinessUtility
    {
        public static double GetMassTolerancePpm(double mass, double tolerance)
        {
            return mass * tolerance / 1.0E6;
        }
    }
}
