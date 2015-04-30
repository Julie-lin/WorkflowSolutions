using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppData.Interfaces
{
    public interface IGetComponentUI
    {
        ComponentUIInfo ComponentUi { get; set; }
    }

    public interface IGetComponentConnectionInfo
    {
        List<ComponentConnectionInfo> TreeConnectionInfo { get; set; }
    }

    public interface IClientParam
    {
        bool CreateResult { get; set; }
        bool CreateReport { get; set; }
    }
}
