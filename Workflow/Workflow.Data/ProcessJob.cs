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
    public class ProcessJob : IProcessJob, ICancellableWorkItemWithChildren
    {
        /// <summary>
        /// JobStatus
        /// </summary>
        public enum JobStatus
        {
            /// <summary>
            /// 
            /// </summary>
            Waiting = 0,
            /// <summary>
            /// 
            /// </summary>
            Started,
            /// <summary>
            /// 
            /// </summary>
            QuantitationDone,
            /// <summary>
            /// 
            /// </summary>
            TargetScreeningDone,
            /// <summary>
            /// 
            /// </summary>
            UnknownScreeningDone,
            /// <summary>
            /// 
            /// </summary>
            AllDone,
            /// <summary>
            /// 
            /// </summary>
            Cancelled,
            /// <summary>
            /// 
            /// </summary>
            Aborted
        }
        /// <summary>
        /// 
        /// </summary>

        public ProcessJob()
        {
            Id = System.Guid.NewGuid();
            ProcessJobNames = new List<string>();
            Description = "";
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public long JobWorkId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string WorkItemType { get { return "ProcessJob"; } }

        public bool IsVisible { get; set; }
        public IEnumerable<IWorkItemWithChildren> Children { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<string> ProcessJobNames { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public JobStatus Status { get; set; }

        private string _currentJob { get; set; }
        [DataMember]
        public string CurrentJob
        {
            get { return _currentJob; }
            set { _currentJob = value; }
        }

        public bool ThrowException { get; set; }

        public bool HasCancellationRequested { get; set; }
        public bool SupportsCancellation { get; private set; }
    }

}
