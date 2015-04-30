using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AppData;
using Workflow.Data;

namespace Mneme.Components
{
    using AppData.Interfaces;

    [Serializable]
    [DataContract]
    [KnownType(typeof(ComponentNode))]
    public class HelloWorldNode : ComponentNode, IComponentValidation, INotifyPropertyChanged// IGetComponentUI, IClientParam, INotifyPropertyChanged
    {
        public HelloWorldNode()
        {
            ComponentUi = new ComponentUIInfo();
            ComponentName = "HelloWorldNode"; //this class name
            Name = "Hello World"; //any name
            CompopnentExcutionName = "HelloWorldProcess"; //the process this node executing
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

        private string _helloWordMessage;
        [DataMember]
        public string HelloWordMessage
        {
            get { return _helloWordMessage; }
            set
            {
                if (_helloWordMessage != value)
                {
                    _name = value;
                    OnPropertyChanged("HelloWordMessage");
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
