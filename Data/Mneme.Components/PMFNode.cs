using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AppData;
using AppData.Interfaces;
using Workflow.Data;

namespace Mneme.Components
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(ComponentNode))]
    public class PMFNode : ComponentNode, INotifyPropertyChanged//IGetComponentUI, IClientParam, INotifyPropertyChanged
    {
        public PMFNode()
        {
            ComponentName = "PMF";
            Name = "Peptide Mass Fingerprint";
            CompopnentExcutionName = "PMFSearchExecutable";
            ComponentList = new List<string>();
            FileSelection = new ThermoFileSelection() { Extension = "XML" };
            ComponentUi = new ComponentUIInfo();
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
        private ThermoFileSelection _fileSelection;
        [DataMember]
        public ThermoFileSelection FileSelection
        {
            get { return _fileSelection; }
            set
            {
                if (_fileSelection != value)
                {
                    _fileSelection = value;
                    OnPropertyChanged("FileSelection");
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
                if (_ppmTolerance != value)
                {
                    _ppmTolerance = value;
                    OnPropertyChanged("PPMTolerance");
                }
            }
        }


        private bool _createResult;
        [DataMember]
        public bool CreateResult
        {
            get { return _createResult; }
            set
            {
                if (_createResult != value)
                {
                    _createResult = value;
                    OnPropertyChanged("CreateResult");
                }
            }

        }

        private bool _createReport;
        [DataMember]
        public bool CreateReport
        {

            get { return _createReport; }
            set
            {
                if (_createReport != value)
                {
                    _createReport = value;
                    OnPropertyChanged("CreateResult");
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


    }
}
