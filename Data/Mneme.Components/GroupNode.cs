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
    public class GroupNode : ComponentNode, INotifyPropertyChanged //IGetComponentUI
    {
        public GroupNode()
        {
            ComponentName = "EndGroup";
            CompopnentExcutionName = "ClientGroupExecutable";
            CompNodeValidation = NodeValidationType.Group;
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
