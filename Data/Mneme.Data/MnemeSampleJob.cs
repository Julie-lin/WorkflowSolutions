using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Workflow.Data;

namespace Mneme.Data
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(ProcessJob))]
    public class MnemeSampleJob : ProcessJob
    {
        public MnemeSampleJob()
        {
            
        }
        /// <summary>
        /// Raw file
        /// </summary>
        [DataMember]
        public string RawFile { get; set; }
        [DataMember]
        public string RawFilePath { get; set; }

        /// <summary>
        /// Sample Id
        /// </summary>
        [DataMember]
        public string SampleId { get; set; }

        public string WorkItemType
        {
            get { return "Sample"; }
        }

    }

}
