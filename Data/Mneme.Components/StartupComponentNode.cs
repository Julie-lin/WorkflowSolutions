using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using AppData;
using AppData.Interfaces;
using Workflow.Data;
using Workflow.Data.Interfaces;

namespace Mneme.Components
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(ComponentNode))]
    public class StartupComponentNode : ComponentNode, IGetComponentConnectionInfo, IStartUpNode, INotifyPropertyChanged  //, IGetComponentUI, IGetComponentConnectionInfo, IRecallFixeup, INotifyPropertyChanged
    {
        public StartupComponentNode()
        {
            ComponentName = "Startup";
            StartNode = true;
            Name = "Start up";
            CompopnentExcutionName = "ClientStartupProcess";
            BatchInitializeExcutionName = "BatchInitializer";
            //FolderSelection = new ThermoFolderSelection();
            //ComponentUi = new ComponentUIInfo();
            TreeConnectionInfo = new List<ComponentConnectionInfo>();
            ProcessRawFiles = new List<string>();
            RawFiles = new List<object>();
            BatchInitialized = false;
            CreateTestRawfileList();
        }
        private void CreateTestRawfileList()
        {
            ProcessRawFiles.Clear();
            List<string> list = new List<string>() { @"C:\Xcalibur\data\drugx_01.raw", @"C:\Xcalibur\data\drugx_02.raw" };
            ProcessRawFiles.AddRange(list);
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

        private string _description;
        [DataMember]
        [Browsable(false)]
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    //OnPropertyChanged("Description");
                }
            }
        }


        //        private ThermoFolderSelection _folderSelection;

        //[DataMember]
        //public ThermoFolderSelection FolderSelection
        //{
        //    get { return _folderSelection; }
        //    set
        //    {
        //        if (_folderSelection != value)
        //        {
        //            _folderSelection = value;
        //            OnPropertyChanged("FolderSelection");
        //        }
        //    }

        //}

        [DataMember]
        [Browsable(true)]
        public new string TreeName { get; set; }

        private List<string> _processRawFiles;
        [DataMember]
        [Browsable(false)]
        public List<string> ProcessRawFiles
        {
            get { return _processRawFiles; }
            set
            {
                if (_processRawFiles != value)
                {
                    _processRawFiles = value;
                }
            }
        }

        private List<object> _rawFiles;

        [XmlIgnore]
        public List<object> RawFiles
        {
            get { return _rawFiles; }
            set
            {
                if (_rawFiles != value)
                {
                    _rawFiles = value;
                    UpdateProcessRawFiles();
                    OnPropertyChanged("RawFiles");
                }
            }
        }

        [DataMember]
        [Browsable(false)]
        [XmlIgnore]
        public DbContext Entities { get; set; }

        [Browsable(false)]
        [XmlIgnore]
        public bool BatchInitialized { get; set; }

        private void UpdateProcessRawFiles()
        {
            ProcessRawFiles.Clear();
            foreach (var rawFile in RawFiles)
            {
                ProcessRawFiles.Add((string)rawFile);
            }
        }
        private void UpdateRawFiles()
        {
            RawFiles.Clear();
            foreach (var rawFile in ProcessRawFiles)
            {
                RawFiles.Add(rawFile);
            }
        }
        //[Browsable(false)]
        //[DataMember]
        //public ComponentUIInfo ComponentUi { get; set; }

        [DataMember]
        public List<ComponentConnectionInfo> TreeConnectionInfo { get; set; }


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
            UpdateRawFiles();
        }
        [DataMember]
        [Browsable(false)]
        public Guid ExecuteWorkflowId { get; set; }
    }
}
