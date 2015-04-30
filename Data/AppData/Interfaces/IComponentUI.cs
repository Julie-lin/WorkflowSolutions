using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppData.Interfaces
{
    public interface IComponentUI
    {
        //following for item
        double Left { get; set; }
        double Top { get; set; }
        double Width { get; set; }
        double Heigh { get; set; }
        int ZIndex { get; set; }
        bool IsGroup { get; set; }
        Guid ParentId { get; set; }
        string Content { get; set; }

    }
}
