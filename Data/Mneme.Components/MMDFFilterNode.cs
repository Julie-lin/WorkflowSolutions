using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using AppData;
using AppData.Interfaces;
using Workflow.Data;

namespace Mneme.Components
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(ComponentNode))]
    public class MMDFFilterNode : ComponentNode, IGetComponentUI, IClientParam, IRecallFixeup, INotifyPropertyChanged
    {
        public MMDFFilterNode()
        {
            ComponentName = "MMDFFilter";
            CompopnentExcutionName = "MMDFExecutable";
            FolderSelection = new ThermoFolderSelection();
            ComponentUi = new ComponentUIInfo();
            Filters = new List<MassFilter>();
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


        private List<MassFilter> _filters;
        [DataMember]
        public List<MassFilter> Filters
        {
            get { return _filters; }
            set
            {
                if (_filters != value)
                {
                    _filters = value;
                }
            }
        }

        private List<object> _mmdfFilters = new List<object>() { new MassFilter() };

        [XmlIgnore]
        public List<object> MMDFFilters
        {
            get { return _mmdfFilters; }
            set
            {
                if (_mmdfFilters != value)
                {
                    _mmdfFilters = value;
                    UpdateFilters();
                    OnPropertyChanged("MMDFFilters");
                }
            }

        }
        private void UpdateMmdfFilters()
        {
            _mmdfFilters.Clear();
            foreach (var massFilter in Filters)
            {
                _mmdfFilters.Add(massFilter);
            }
        }
        private void UpdateFilters()
        {
            Filters.Clear();
            foreach (var filter in _mmdfFilters)
            {
                Filters.Add((MassFilter)filter);
            }
        }
        private ThermoFolderSelection _folderSelection;
        [DataMember]
        public ThermoFolderSelection FolderSelection
        {
            get { return _folderSelection; }
            set
            {
                if (_folderSelection != value)
                {
                    _folderSelection = value;
                    OnPropertyChanged("FolderSelection");
                }
            }

        }

        [Browsable(false)]
        [DataMember]
        public ComponentUIInfo ComponentUi { get; set; }


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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public void FixupCollectionItems()
        {
            UpdateMmdfFilters();
        }
    }

    public static class MMDFExtension
    {
        public static void CreateTestData(this MMDFFilterNode test)
        {
            //test.MMDFFilters = new List<object>()
            //                 {
            //                     new MassFilter() {Name = "filter 1", LowLimit = 1.1, HighLimit = 5.1},
            //                     new MassFilter() {Name = "filter 2", LowLimit = 1.0, HighLimit = 5.0}
            //                 };

        }

    }

}
