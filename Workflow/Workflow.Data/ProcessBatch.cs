using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Thermo.Data.Hierarchical;
using Workflow.Data.Interfaces;

namespace Workflow.Data
{
    [Serializable]
    [DataContract]
    public class ProcessBatch : ICancellableWorkItemWithChildren, IProcessBatch, ICloneable
    {
        /// <summary>
        /// 
        /// </summary>
        public ProcessBatch()
        {
            Id = Guid.NewGuid();
            Groups = new List<ProcessGroup>();
            ExtraInfo = new Collection<ExtraProcessInfo>();
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Guid Id
        {
            get;
            set;
        }

        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Extension { get; set; }

        [DataMember]
        public ICollection<ExtraProcessInfo> ExtraInfo { get; set; }

        [DataMember]
        public DateTime? ProcessTime { get; set; }
        [DataMember]
        public ICollection<ExtraProcessInfo> ProcessSummary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ProjectName { get; set; }

        public IProcessBatch AppBatch { get; set; }

        /// <summary>
        /// InitializeJobName
        /// </summary>
        [DataMember]
        public string InitializeJobName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string CleanupJobName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WorkItemType
        {
            get { return "Batch"; }
        }

        public bool IsVisible { get; set; }
        public IEnumerable<IWorkItemWithChildren> Children { get; private set; }

        [DataMember]
        public List<ProcessGroup> Groups { get; set; }

        public IProcessGroup GetProcessGroup(Guid groupId)
        {
            return Groups.FirstOrDefault(s => s.Id == groupId);
        }

        public bool HasCancellationRequested { get; set; }
        public bool SupportsCancellation { get; private set; }
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }

}
