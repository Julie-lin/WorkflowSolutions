using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Thermo.Data.Hierarchical;
using Workflow.Data.Interfaces;

namespace Workflow.Data
{
    [Serializable]
    [DataContract]
    public class ProcessGroup : IProcessGroup, ICancellableWorkItemWithChildren
    {
        public ProcessGroup()
        {
            Id = Guid.NewGuid();
            Jobs = new List<ProcessJob>();
            OkToContinue = true;
            EndOfSequenceProcess = "";
        }

        [DataMember]
        public bool OkToContinue { get; set; }

        [DataMember]
        public string EndOfSequenceProcess { get; set; }

        [DataMember]
        public List<ProcessJob> Jobs { get; set; }


        #region Implementation of IWorkItemWithChildren
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }

        public string Description { get; set; }

        public string WorkItemType { get { return "Group"; } }
        public bool IsVisible { get; set; }
        public IEnumerable<IWorkItemWithChildren> Children { get; private set; }

        #endregion

        public IProcessJob GetProcessJob(Guid jobId)
        {
            return Jobs.FirstOrDefault(j => j.Id == jobId);
        }

        public bool HasCancellationRequested { get; set; }
        public bool SupportsCancellation { get; private set; }
    }

}
