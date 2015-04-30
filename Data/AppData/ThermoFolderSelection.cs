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

    public class ThermoFolderSelection : INotifyPropertyChanged
    {
        private string _folderName = "";
        [DataMember]
        public String FolderName
        {
            get { return _folderName; }
            set
            {
                if (_folderName != value)
                {
                    _folderName = value;
                    OnPropertyChanged("FolderName");
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
