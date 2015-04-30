using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AppData;
using AppData.Interfaces;
using Workflow.Data;

namespace Mneme.Components
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(ComponentNode))]
    public class PeakDiscoverNode : ComponentNode, IComponentValidation, INotifyPropertyChanged// IGetComponentUI, IClientParam, INotifyPropertyChanged
    {
        public PeakDiscoverNode()
        {
            ComponentUi = new ComponentUIInfo();
            ComponentName = "PeakDiscoverNode"; //this class name
            Name = "Peak Discover"; //any name
            CompopnentExcutionName = "FramePeakProcess"; //the process this node executing
            ComponentList = new List<string>();
        }
        private List<string> _componentList;
        [DataMember]
        public List<string> ComponentList
        {
            get { return _componentList; }
            set
            {
                if (_componentList != value)
                {
                    _componentList = value;
                    OnPropertyChanged("ComponentList");
                }
            }

        }

        private string _name;
        [DataMember]
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

        private string _processName = "Frame";
        [DataMember]
        public string ProcessName
        {
            get { return _processName; }
            set
            {
                if (_processName != value)
                {
                    _processName = value;
                    OnPropertyChanged("ProcessName");
                }
            }
        }

        [DataMember]
        private double _ppmTolerance;
        public double PPMTolerance
        {
            get { return _ppmTolerance; }
            set
            {
                if (Math.Abs(_ppmTolerance - value) < 0.000000001)
                {
                    _ppmTolerance = value;
                    OnPropertyChanged("PPMTolerance");
                }
            }
        }

        [DataMember]
        private double _frameWidth;
        public double FrameWidth
        {
            get { return _frameWidth; }
            set
            {
                if (Math.Abs(_frameWidth - value) < 0.000000001)
                {
                    _ppmTolerance = value;
                    OnPropertyChanged("FrameWidth");
                }
            }
        }


        [Browsable(false)]
        [DataMember]
        public ComponentUIInfo ComponentUi { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public List<string> ValidParentNodes { get; set; }
    }

}
