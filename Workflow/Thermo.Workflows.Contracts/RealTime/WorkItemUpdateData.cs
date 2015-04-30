using System.Runtime.Serialization;

namespace Thermo.Workflows.Contracts.RealTime
{
    [DataContract]
    public class WorkItemUpdateData
    {
        [DataMember]
        public WorkItemWithHistory WorkItemWithHistory { get; set; }
        [DataMember]
        public TaskProgressCallbackMessage ProgressCallbackMessage { get; set; }
    }
}
