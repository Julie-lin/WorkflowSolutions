using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Activities;
using Mneme.Data;
using Mneme.Data.Interfaces;
using Workflow.Data;

namespace Mneme.Workflows
{

    public sealed class CreateBatchActivity : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> ConnectionString { get; set; }
        public InArgument<Guid> BatchId { get; set; }
        public InArgument<long> ExperimentId { get; set; }
        public InArgument<long> MeasurementId { get; set; }
        public OutArgument<ProcessBatch> Batch { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            string connection = context.GetValue(this.ConnectionString);
            long expId = context.GetValue(ExperimentId);
            long meauId = context.GetValue(MeasurementId);
            Guid batchId = context.GetValue(BatchId);
            ProcessBatch batch = new ProcessBatch();
            if (expId != 0)
            {
                batch = CreateBatchFromExperiment(batchId, expId);
            }
            else
            {
                batch = CreateBatchFromMeasurementId(batchId, meauId);
            }
            Batch.Set(context, batch);
    //        long priorityTicket = context.GetExtension<CriticalSectionQueueExtension>()
    //.GetPriorityTicket(queueName);
    //        PriorityTicket.Set(context, priorityTicket);
        }

        private ProcessBatch CreateBatchFromExperiment(Guid batchId, long expId)
        {
            //IRawDataSummary adapter = new RawDataSummary();
            //var measurements = adapter.GetMeasurementNamesByExperimentId(expId);
            return new ProcessBatch();//CreateProcessBatch(batchId, measurements);
        }
        private ProcessBatch CreateBatchFromMeasurementId(Guid batchId, long expId)
        {

            return CreateProcessBatchFromMeausrementId(batchId, expId);
        }

        private ProcessBatch CreateProcessBatchFromMeausrementId(Guid batchId, long mId)
        {
            var processBatch = new ProcessBatch()
            {
                Id = batchId,
                UserId = "me", //jlin how about user id?
                Name = "Post upload batch",
                Path = "",
                Extension = ".batch",
                ExtraInfo = new Collection<ExtraProcessInfo>()
                                                         {
                                                             new ExtraProcessInfo(){Key = "file", Value = "path"}
                                                         }
            };

            string rawFilePath = "";
            List<ProcessJob> jobs = CreateProcessJob(mId);
            ProcessGroup group = new ProcessGroup()
            {
                Name = "post upload group",
                Jobs = jobs
            };

            processBatch.Groups.Add(group);
            return processBatch;
        }
        private ProcessBatch CreateProcessBatch(Guid batchId, List<IdNamePair> measurements)
        {
            var processBatch = new ProcessBatch()
            {
                Id = batchId,
                UserId = "me", //jlin how about user id?
                Name = "Post upload batch",
                Path = "",
                Extension = ".batch",
                ExtraInfo = new Collection<ExtraProcessInfo>()
                                                         {
                                                             new ExtraProcessInfo(){Key = "file", Value = "path"}
                                                         }
            };

            string rawFilePath = "";
            List<ProcessJob> jobs = CreateProcessJobs(measurements);
            ProcessGroup group = new ProcessGroup()
            {
                Name = "post upload group",
                Jobs = jobs
            };

            processBatch.Groups.Add(group);
            return processBatch;
        }

        private static List<ProcessJob> CreateProcessJob(long mId)
        {
            List<ProcessJob> jobs = new List<ProcessJob>();

            ProcessJob job = new ProcessJob()
            {
                Name = "up loader default job",
                JobWorkId = mId
            };
            jobs.Add(job);
            return jobs;
            
        }
        public static List<ProcessJob> CreateProcessJobs(List<IdNamePair> measurements)
        {
            List<ProcessJob> jobs = new List<ProcessJob>();
            foreach (var m in measurements)
            {
                ProcessJob job = new ProcessJob()
                    {
                        Name = m.Name,
                        JobWorkId = m.Id
                    };
                jobs.Add(job);
            }
            return jobs;

        }

    }
}
