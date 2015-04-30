using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Thermo.Workflows.Contracts.RealTime
{
    [DataContract]
    public class TaskProgressCallbackMessage
    {
        public TaskProgressCallbackMessage()
        {
            TimeStamp = DateTime.Now;
        }

        [DataMember]
        public List<WorkItemId> RouteToChild { get; set; }
        [DataMember]
        public string MessageType { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public DateTime TimeStamp { get; set; }
        [DataMember]
        public long RecordNumber { get; set; }
        [DataMember]
        public string ActivityName { get; set; }
    }

    public static class WorkItemStatus
    {
        public const string Waiting = "Waiting";
        public const string Received = "Received";
        public const string Executing = "Executing";
        public const string Closed = "Closed";
        public const string Faulted = "Faulted";
        public const string Canceled = "Canceled";
        public const string Info = "Info";
    }

    [DataContract]
    public class WorkItemId
    {
        [DataMember]
        public Guid Id{ get; set;}
        [DataMember]
        public string Name{ get; set;}
        [DataMember]
        public string WorkItemType { get; set; }
    }
}
