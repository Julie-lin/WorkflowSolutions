using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppData
{
    public class UIPMFSetting
    {
        public UIPMFSetting()
        {

            Extension = "XML";
        }
        public string File { get; set; }
        public string Extension { get; set; }
        public double PPMTolerance { get; set; }
    }

}
