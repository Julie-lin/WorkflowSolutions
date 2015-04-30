using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AppData;
using Workflow.Data;

namespace Mneme.Components
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(ComponentNode))]
    public class IsotopicClusterNode : ComponentNode, INotifyPropertyChanged// IGetComponentUI, IClientParam, INotifyPropertyChanged
    {
        public IsotopicClusterNode()
        {
            ComponentName = "IsotopicClusterNode";
            ComponentName = "Isotopic Cluster detect";
            CompopnentExcutionName = "IsotopicClusterProcess";
            _WriteToExcel = false;
        }

        private bool _WriteToExcel;
        [DataMember]
        public bool WriteToExcel
        {
            get { return _WriteToExcel; }
            set
            {
                if (_WriteToExcel != value)
                {
                    _WriteToExcel = value;
                    OnPropertyChanged("WriteToExcel");
                }
            }
        }

        private double _dynamicRange = 4.2;
        [DataMember]
        public double DynamicRange
        {
            get { return _dynamicRange; }
            set
            {
                if (_dynamicRange != value)
                {
                    _dynamicRange = value;
                    OnPropertyChanged("DynamicRange");
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

    }
}
