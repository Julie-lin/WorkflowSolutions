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
using Workflow.Data.Interfaces;

namespace Mneme.Components
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(ComponentNode))]
    public class TraceBuldNode : ComponentNode, IComponentValidation, INotifyPropertyChanged// IGetComponentUI, IClientParam, INotifyPropertyChanged
    {
        public TraceBuldNode()
        {
            ComponentUi = new ComponentUIInfo();
            ComponentName = "TraceBuldNode";
            Name = "TraceBuild";
            CompopnentExcutionName = "TraceBuildProcess";
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

        private string _connectionString;
        [DataMember]
        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                if (_connectionString != value)
                {
                    _name = value;
                    OnPropertyChanged("ConnectionString");
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
