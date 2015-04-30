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
    public class ProcessHolderNode : ComponentNode, IComponentValidation, INotifyPropertyChanged// IGetComponentUI, IClientParam, INotifyPropertyChanged
    {
        public ProcessHolderNode()
        {
            ComponentUi = new ComponentUIInfo();
            ComponentName = "ProcessHolderNode";
            Name = "Process holder node";
            CompopnentExcutionName = "UpdateStatusProcess";
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

        private string _processName;
        [DataMember]
        public string ProcessName
        {
            get { return _processName; }
            set
            {
                if (_processName != value)
                {
                    _name = value;
                    OnPropertyChanged("ProcessName");
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
