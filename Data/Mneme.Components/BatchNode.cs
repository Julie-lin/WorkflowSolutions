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
    public class BatchNode : ComponentNode, INotifyPropertyChanged //IGetComponentUI
    {
        public BatchNode()
        {
            ComponentName = "EndBatch";
            CompopnentExcutionName = "ClientBatchExecutable";
            CompNodeValidation = NodeValidationType.Batch;
            ComponentUi = new ComponentUIInfo();
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
