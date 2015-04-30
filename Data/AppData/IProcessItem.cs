using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AppData
{
    public interface IProcessItem
    {
        string Name { get; }
        ObservableCollection<IProcessItem> Children { get; }
    }
}
