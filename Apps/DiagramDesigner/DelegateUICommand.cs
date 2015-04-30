using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiagramDesigner
{
    public class DelegateUICommand : DelegateCommand
    {
        public string Name { get; set; }

        public DelegateUICommand(string name, Action<object> execute)
            : this(name, execute, null)
        {
        }

        public DelegateUICommand(string name, Action<object> execute, Predicate<object> canExecute)
            : base(execute, canExecute)
        {
            Name = name;
        }
    }
}
