using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DiagramDesigner
{
    public class DelegateCommand : Command
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;
        private readonly Action _executeMethod;


        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action _executeMethod)
            : this(_executeMethod, null)
        {
        }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _executeMethod = executeMethod;
        }

        public override void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute(parameter);
            }

            if (_executeMethod != null)
            {
                _executeMethod();
            }
        }

        public override bool CanExecute(object parameter)
        {
            if (_canExecute == null) return true;
            return _canExecute(parameter);
        }
    }

    public abstract class Command : ICommand
    {
        private Dispatcher _dispatcher;

        protected Command()
        {
            if (Application.Current != null)
            {
                _dispatcher = Application.Current.Dispatcher;
            }
            else
            {
                // this is useful for unit tests where there is no app running
                _dispatcher = Dispatcher.CurrentDispatcher;
            }
        }


        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public abstract void Execute(object parameter);
        public abstract bool CanExecute(object parameter);

        protected virtual void OnCanExecuteChanged()
        {
            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.Invoke((ThreadStart)OnCanExecuteChanged, DispatcherPriority.Normal);
            }
            else
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}
