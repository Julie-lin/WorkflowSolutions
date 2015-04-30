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
    public class ReportNode : ComponentNode, INotifyPropertyChanged //IGetComponentUI
    {
        public ReportNode()
        {
            ComponentName = "Report";
            Name = "Report";
            CompopnentExcutionName = "ReportExcutable";

            PrinterName = "current configued printer";

            ComponentUi = new ComponentUIInfo();
        }


        private int _index;
        [DataMember]
        public int Index
        {
            get { return _index; }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    OnPropertyChanged("Index");
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
        private string _printerName;
        [DataMember]
        public string PrinterName
        {
            get { return _printerName; }
            set
            {
                if (_printerName != value)
                {
                    _printerName = value;
                    OnPropertyChanged("PrinterName");
                }
            }
        }
        private bool _exportPeakList;
        [DataMember]
        public bool ExportPeakList
        {
            get { return _exportPeakList; }
            set
            {
                if (_exportPeakList != value)
                {
                    _exportPeakList = value;
                    OnPropertyChanged("ExportPeakList");
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
