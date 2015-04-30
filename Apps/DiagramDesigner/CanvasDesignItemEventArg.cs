using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiagramDesigner
{
    internal class CanvasDesignItemEventArg : EventArgs
    {
        internal string TypeName { get; set; }
        internal string ComponentName { get; set; }
    }
}
