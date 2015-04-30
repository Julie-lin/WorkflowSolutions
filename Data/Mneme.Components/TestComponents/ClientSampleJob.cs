using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Workflow.Data;


namespace Mneme.Components
{
    [Serializable]
    [DataContract]
    public class ClientSampleJob : ProcessJob
    {
        public ClientSampleJob()
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


        /// <summary>
        /// Vial
        /// </summary>
        [DataMember]
        public string Vial { get; set; }

        /// <summary>
        /// Injection volumn
        /// </summary>
        [DataMember]
        public double InjectionVolume { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// ///jlin need this?
        [DataMember]
        public DateTime AcqusitionDate { get; set; }

        public string WorkItemType
        {
            get { return "Sample"; }
        }

    }

}
