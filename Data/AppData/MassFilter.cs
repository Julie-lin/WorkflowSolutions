using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AppData
{
    [Serializable]
    [DataContract]
    public class MassFilter : INotifyPropertyChanged
    {
        public MassFilter()
        {
            Name = "New filter";
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
            
        }

        private double _lowLimit;
        [DataMember]
        public double LowLimit
        {
            get { return _lowLimit; }
            set
            {
                if (_lowLimit != value)
                {
                    _lowLimit = value;
                    OnPropertyChanged("LowLimit");
                }
            }
            
            
        }
        private double _highLimit;
        [DataMember]
        public double HighLimit
        {
            get { return _highLimit; }
            set
            {
                if (_highLimit != value)
                {
                    _highLimit = value;
                    OnPropertyChanged("HighLimit");
                }
            }


        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
