using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;

namespace Mneme.Utility
{
    public class EngineUtility
    {
        public static string GetDataPath()
        {
            var applicationPath = Directory.GetParent(Assembly.GetEntryAssembly().Location).FullName;
            string tempPath = Directory.GetParent(
                Directory.GetParent(
                Directory.GetParent(
                Directory.GetParent(applicationPath).FullName).FullName).FullName).FullName;
            return System.IO.Path.Combine(tempPath, "TestData");
        }

        
        public static long GetProcessMemoryConsumption(Process proc)
        {
            //http://stackoverflow.com/questions/2342023/how-to-measure-the-total-memory-consumption-of-the-current-process-programatical
            //Process proc = Process.GetCurrentProcess();
            //long set = proc.WorkingSet64;
            return proc.PrivateMemorySize64 / 1000000;
           // log("Memory consumption for this file is:  " + size.ToString() + " Mb");

        }

    }
}
