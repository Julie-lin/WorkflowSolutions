using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace DiagramDesigner.ViewModels
{
    [Serializable]
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void InvokePropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs eventArgs = new PropertyChangedEventArgs(propertyName);
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null)
                changed(this, eventArgs);
        }

        private Dictionary<string, string> _dataErrors = new Dictionary<string, string>();
        public Dictionary<string, string> DataErrors
        {
            get { return _dataErrors; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string GetFieldDataError(string fieldName)
        {
            string error;
            _dataErrors.TryGetValue(fieldName, out error);

            return error;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="error"></param>
        /// <param name="raiseNotification"></param>
        public void SetFieldDataError(string fieldName, string error, bool raiseNotification)
        {
            if (string.IsNullOrEmpty(error))
            {
                _dataErrors.Remove(fieldName);

            }
            else
            {
                _dataErrors[fieldName] = error;

            }

            if (raiseNotification)
                this.InvokePropertyChanged(fieldName);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        public void ClearFieldError(string fieldName)
        {
            // _dataErrors[fieldName] = "";
            _dataErrors.Remove(fieldName);
            if (_dataErrors.Count == 0)
            {
                this.InvokePropertyChanged("");
            }
        }

        public void ClearFieldError(String fieldName, Boolean raiseNotification)
        {
            _dataErrors.Remove(fieldName);

            if (raiseNotification)
                InvokePropertyChanged(fieldName);
        }

        /// <summary>
        ///  Clears all column errors for this row
        /// </summary>
        public void ClearAllColumnErrors()
        {
            _dataErrors.Clear();
        }

        private static bool? _inDesignMode;

        public static bool InDesignMode
        {
            get
            {
                if (!_inDesignMode.HasValue)
                {
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    _inDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
                }
                return _inDesignMode.Value;
            }
        }
    }
}
