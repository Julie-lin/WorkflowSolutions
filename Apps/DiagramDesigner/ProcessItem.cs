using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace DiagramDesigner
{
    public class ProcessItem : INotifyPropertyChanged
    {
        public ProcessItem()
        {
            Id = Guid.NewGuid();
            Children = new ObservableCollection<ProcessItem>();
            ProcessStatus = "Idle";
        }

        private string _processingStatus;
        public string ProcessStatus
        {
            get { return _processingStatus; }
            set
            {
                if (value != _processingStatus)
                {
                    _processingStatus = value;
                    OnPropertyChanged("ProcessStatus");
                }
            }

        }
        public Guid Id { get; set; }

        private string _name;

        [XmlAttribute]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }


        private int _crtDepthLevel;

        [XmlAttribute]
        public int CrtDepthLevel
        {
            get { return _crtDepthLevel; }
            set
            {
                if (value != _crtDepthLevel)
                {
                    _crtDepthLevel = value;
                    OnPropertyChanged("CrtDepthLevel");
                }
            }
        }

        [XmlAttribute]
        public object Job { get; set; }

        private ObservableCollection<ProcessItem> _children;

        public ObservableCollection<ProcessItem> Children
        {
            get { return _children; }
            set
            {
                if (value != _children)
                {
                    _children = value;
                    OnPropertyChanged("Children");
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
