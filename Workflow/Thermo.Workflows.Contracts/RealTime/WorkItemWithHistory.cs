using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Thermo.Data.Hierarchical;

namespace Thermo.Workflows.Contracts.RealTime
{
    [DataContract]
    public class WorkItemWithHistory : IWorkItemWithChildren, INotifyPropertyChanged
    {
        public WorkItemWithHistory()
        {
            Children = new ObservableCollection<WorkItemWithHistory>();
            MessageHistory = new ObservableCollection<CallbackMessageContent>();
        }

        private Guid _id;
        [DataMember]
        [XmlAttribute]
        public Guid Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    InvokePropertyChanged("Id");
                }
            }
        }

        private string _name;
        [DataMember]
        [XmlAttribute]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    InvokePropertyChanged("Name");
                }
            }
        }


        private string _workItemType;
        [DataMember]
        [XmlAttribute]
        public string WorkItemType
        {
            get { return _workItemType; }
            set
            {
                if (value != _workItemType)
                {
                    _workItemType = value;
                    InvokePropertyChanged("WorkItemType");
                }
            }
        }

        private bool _supportsCancellation;
        [DataMember]
        [XmlAttribute]
        public bool SupportsCancellation
        {
            get { return _supportsCancellation; }
            set
            {
                if (value != _supportsCancellation)
                {
                    _supportsCancellation = value;
                    InvokePropertyChanged("SupportsCancellation");
                }
            }
        }

        private int _crtDepthLevel;
        [DataMember]
        [XmlAttribute]
        public int CrtDepthLevel
        {
            get { return _crtDepthLevel; }
            set
            {
                if (value != _crtDepthLevel)
                {
                    _crtDepthLevel = value;
                    InvokePropertyChanged("CrtDepthLevel");
                }
            }
        }

        private ObservableCollection<CallbackMessageContent> _messageHistory;
        [DataMember]
        [XmlArray]
        public ObservableCollection<CallbackMessageContent> MessageHistory
        {
            get { return _messageHistory; }
            set
            {
                if (value != _messageHistory)
                {
                    _messageHistory = value;
                    InvokePropertyChanged("MessageHistory");
                }
            }
        }

        private ObservableCollection<WorkItemWithHistory> _children;
        [DataMember]
         [XmlArray]
        public ObservableCollection<WorkItemWithHistory> Children
        {
            get { return _children; }
            set
            {
                if (value != _children)
                {
                    _children = value;
                    InvokePropertyChanged("Children");
                }
            }
        }

         [XmlIgnore]
        IEnumerable<IWorkItemWithChildren> IWorkItemWithChildren.Children
        {
            get { return Children; }
        }

        private void InvokePropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string GetCurrentStatus()
        {
            if (MessageHistory.Count == 0)
                return WorkItemStatus.Received;

            return MessageHistory.Where(message => message.DepthLevel == 0).Last().MessageType;
        }

        public CallbackMessageContent GetLastUpdateMessage()
        {
            return MessageHistory.LastOrDefault();
        }
    }
}
