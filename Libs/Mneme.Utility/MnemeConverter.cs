using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mneme.Utility
{
    public class MnemeConverter
    {
        public static double[] ConvertFloatArrayToDoubleArray(float[] floats)
        {
            double[] dd = new double[floats.Count()];
            for (int i = 0; i < floats.Count(); i++)
            {
                dd[i] = floats[i];

            }
            return dd;
        }
    }
}
