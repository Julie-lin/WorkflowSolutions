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
    public class IsotopicSearchNode : ComponentNode, INotifyPropertyChanged //IGetComponentUI, IClientParam, INotifyPropertyChanged
    {
        public IsotopicSearchNode()
        {
            ComponentName = "IsotopicSearch";
            CompopnentExcutionName = "IsotopeSearchExcutable";
            Mass = 300.09;
            ComponentUi = new ComponentUIInfo();
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



        private double _mass;
        [DataMember]
        public double Mass
        {
            get { return _mass; }
            set
            {
                if (_mass != value)
                {
                    _mass = value;
                    OnPropertyChanged("Mass");
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


        [DataMember]
        [Browsable(false)]
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
