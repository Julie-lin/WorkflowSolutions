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
    //http://msdn.microsoft.com/en-us/library/ms730167.aspx abount known type
    [Serializable]
    [DataContract]
    [KnownType(typeof(ComponentNode))]
    public class TreeTopNode : ComponentNode, INotifyPropertyChanged// IGetComponentUI, IClientParam, INotifyPropertyChanged
    {
        public TreeTopNode()
        {
            ComponentUi = new ComponentUIInfo();
            ComponentName = "TreeTopNode";
            Name = "Tree top";
            CompopnentExcutionName = "TreeTopExecutable";
            ComponentList = new List<string>();

            Threshold = 50000;
            NScanRule = 3;
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
        private bool _dynamicTolerance;
        [DataMember]
        public bool DynamicTolerance
        {
            get { return _dynamicTolerance; }
            set
            {
                if (_dynamicTolerance != value)
                {
                    _dynamicTolerance = value;
                    OnPropertyChanged("DynamicTolerance");
                }
            }
        }


        //private bool _createResult;
        //[DataMember]
        //public bool CreateResult
        //{
        //    get { return _createResult; }
        //    set
        //    {
        //        if (_createResult != value)
        //        {
        //            _createResult = value;
        //            OnPropertyChanged("CreateResult");
        //        }
        //    }

        //}

        //private bool _createReport;
        //[DataMember]
        //public bool CreateReport
        //{

        //    get { return _createReport; }
        //    set
        //    {
        //        if (_createReport != value)
        //        {
        //            _createReport = value;
        //            OnPropertyChanged("CreateResult");
        //        }
        //    }

        //}

        //private string _fasta;
        //[DataMember]
        //public string Fasta
        //{
        //    get { return _fasta; }
        //    set
        //    {
        //        if (_fasta != value)
        //        {
        //            _fasta = value;
        //            OnPropertyChanged("Fasta");
        //        }
        //    }
        //}
        private int _nScanRule;
        [DataMember]
        public int NScanRule
        {
            get { return _nScanRule; }
            set
            {
                if (_nScanRule != value)
                {
                    _nScanRule = value;
                    OnPropertyChanged("NScanRule");
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
        private double _threshold;
        [DataMember]
        public double Threshold
        {
            get { return _threshold; }
            set
            {
                if (_threshold != value)
                {
                    _threshold = value;
                    OnPropertyChanged("Threshold");
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
