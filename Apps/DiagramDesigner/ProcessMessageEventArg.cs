using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workflow.Data;


namespace DiagramDesigner
{
    internal class ProcessMessageEventArg : EventArgs
    {
        internal ComponentProcessMessage ProcessMessage { get; set; }

    }
}
