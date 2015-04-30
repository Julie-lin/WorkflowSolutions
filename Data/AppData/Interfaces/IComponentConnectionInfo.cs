using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AppData.Interfaces
{
    public interface IComponentConnectionInfo
    {
        //following for connection
        Guid SourceId { get; set; }
        Guid SinkId { get; set; }
        string SourceConnectorName { get; set; }
        string SinkConnectorName { get; set; }
        string SourceArrowSymbol { get; set; }
        string SinkArrowSymbol { get; set; }
        int ZIndex { get; set; }

    }
}
