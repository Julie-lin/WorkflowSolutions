using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Workflow.Data
{
    public enum MessageSource
    {
        ServerBatchLevel = 0,
        ServerGroupLevel,
        ServerJobLevel,
        Client
    }
    [Serializable]
    [DataContract]
    public class ComponentProcessMessage
    {
        public ComponentProcessMessage()
        {
            HasError = false;
        }
        [DataMember]
        public MessageSource MessageSource { get; set; }

        [DataMember]
        public Guid BatchId { get; set; }
        [DataMember]
        public Guid GroupId { get; set; }
        [DataMember]
        public Guid JobId { get; set; }

        [DataMember]
        public Guid ComponentId { get; set; }
        [DataMember]
        public bool HasError { get; set; }
        [DataMember]
        public string Message { get; set; }
    }

    public static class ComponentProcessMessageExtension
    {
        public static ComponentProcessMessage CreateServerMessage(this ComponentProcessMessage thisMessage,
            Guid batchId, Guid groupId, Guid jobId, string message)
        {
            var source = MessageSource.ServerJobLevel;
            if (jobId == new Guid() && groupId == new Guid())
            {
                source = MessageSource.ServerBatchLevel;
            }
            else if (jobId == new Guid())
            {
                source = MessageSource.ServerGroupLevel;
            }
            return new ComponentProcessMessage()
            {
                MessageSource = source,
                BatchId = batchId,
                GroupId = groupId,
                JobId = jobId,
                Message = message
            };

        }
        public static ComponentProcessMessage CreateClientMessage(this ComponentProcessMessage thisMessage,
            Guid batchId, Guid groupId, Guid jobId, string message)
        {
            return new ComponentProcessMessage()
            {

                MessageSource = MessageSource.Client,
                BatchId = batchId,
                GroupId = groupId,
                JobId = jobId,
                Message = message
            };
        }

        public static ComponentProcessMessage CreateErrorMessage(this ComponentProcessMessage thisMessage,
            Guid batchId, Guid groupId, Guid jobId, string message)
        {
            var source = MessageSource.ServerJobLevel;
            if (jobId == new Guid() && groupId == new Guid())
            {
                source = MessageSource.ServerBatchLevel;
            }
            else if (jobId == new Guid())
            {
                source = MessageSource.ServerGroupLevel;
            }

            return new ComponentProcessMessage()
            {
                HasError = true,
                MessageSource = source,
                BatchId = batchId,
                GroupId = groupId,
                JobId = jobId,
                Message = message
            };
        }

    }

}
