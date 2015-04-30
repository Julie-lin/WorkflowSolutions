using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mneme.Utility
{
    public class TestUtility
    {
        public static string GetDataPath()
        {
            var applicationPath = Directory.GetParent(Assembly.GetEntryAssembly().Location).FullName;
            string tempPath = Directory.GetParent(
                Directory.GetParent(
                Directory.GetParent(applicationPath).FullName).FullName).FullName;
            return System.IO.Path.Combine(tempPath, "TestData");
        }

    }
}
