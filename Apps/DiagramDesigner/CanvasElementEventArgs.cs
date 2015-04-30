using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DiagramDesigner.Views;

namespace DiagramDesigner
{
    internal class CanvasElementEventArgs : EventArgs
    {
        internal List<DesignerItem> DesignItems { get; set; }
        internal List<Connection> Connections { get; set; }
        internal ComponentCanvas Canvas { get; set; }
    }
}
