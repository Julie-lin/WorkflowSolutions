using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Thermo.Workflows.Contracts.RealTime
{
    [DataContract]
    public class CallbackMessageContent : INotifyPropertyChanged
    {
        private string _messageType;
        [DataMember]
        [XmlAttribute]
        public string MessageType
        {
            get { return _messageType; }
            set
            {
                if (value != _messageType)
                {
                    _messageType = value;
                    InvokePropertyChanged("MessageType");
                }
            }
        }

        private string _message;
        [DataMember]
        [XmlText]
        public string Message
        {
            get { return _message; }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    InvokePropertyChanged("Message");
                }
            }
        }

        private string _activityName;
        [DataMember]
        [XmlAttribute]
        public string ActivityName
        {
            get { return _activityName; }
            set
            {
                if (value != _activityName)
                {
                    _activityName = value;
                    InvokePropertyChanged("ActivityName");
                }
            }
        }

        private DateTime _timeStamp;
        [DataMember]
        [XmlAttribute]
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set
            {
                if (value != _timeStamp)
                {
                    _timeStamp = value;
                    InvokePropertyChanged("TimeStamp");
                }
            }
        }

        private long _recordNumber;
        [DataMember]
        [XmlAttribute]
        public long RecordNumber
        {
            get { return _recordNumber; }
            set
            {
                if (value != _recordNumber)
                {
                    _recordNumber = value;
                    InvokePropertyChanged("RecordNumber");
                }
            }
        }

        private int _depthLevel;
        [DataMember]
        [XmlAttribute]
        public int DepthLevel
        {
            get { return _depthLevel; }
            set
            {
                if (value != _depthLevel)
                {
                    _depthLevel = value;
                    InvokePropertyChanged("DepthLevel");
                }
            }
        }

        private void InvokePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
