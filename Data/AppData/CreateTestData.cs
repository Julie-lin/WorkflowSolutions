using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DiagramDesigner.ViewModels;
using Workflow.Data;


namespace AppData
{
    public class CreateTestData
    {
        public ProcessBatch CreateBatchFromMeasurements(ClientBatch batch, List<MeasurementIdNamePair> jobList)
        {
            string path = System.IO.Path.Combine(batch.Path, batch.Name + batch.Extension);
            var processBatch = new ProcessBatch()
            {
                Id = batch.Id,
                UserId = "me",
                Name = batch.Name,
                Path = batch.Path,
                Description = "test batch",
                Extension = batch.Extension,
                ExtraInfo = new Collection<ExtraProcessInfo>()
                                                         {
                                                             new ExtraProcessInfo(){Key = "file", Value = path}
                                                         }
            };
            List<string> processes = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                processes.Add("TestProcess1");
            }
            string rawFilePath = @"C:\SimSourceData";
            var jobs = CreateProcessJobs(jobList, processes);
            ProcessGroup seq = new ProcessGroup()
            {
                Name = "sequence 1",
                EndOfSequenceProcess = "ClientEndOfSequenceProcess",
                Jobs = jobs,
                Description = "group test"
            };

            List<ProcessGroup> seqs = new List<ProcessGroup>() { };
            seqs.Add(seq);
            processBatch.Groups = seqs;
            processBatch.AppBatch = batch;

            return processBatch;
        }
        private static List<ProcessJob> CreateProcessJobs(List<MeasurementIdNamePair> rawFiles, List<string> processes)
        {
            List<ProcessJob> jobs = new List<ProcessJob>();
            foreach (var item in rawFiles)
            {
                ProcessJob job = new ClientSample()
                {
                    Name = item.Name,
                    Description = "job test",
                    Id = Guid.NewGuid(),
                    ProcessJobNames = processes,
                    RawFile = item.Name,
                    RawFilePath = item.Name,
                    JobWorkId = item.Id
                };
                jobs.Add(job);

            };
            return jobs;
        }
        public static ProcessBatch CreateProcessBatch(List<string> rawfiles)
        {
            var processBatch = new ProcessBatch()
            {
                Id = Guid.NewGuid(),
                UserId = "me",
                Name = "My process batch",
                Path = @"C:\Prototype\diagramTest\TestData",
                Extension = ".test",
                ExtraInfo = new Collection<ExtraProcessInfo>()
                                                         {
                                                             new ExtraProcessInfo(){Key = "file", Value = "path"}
                                                         }
            };

            string rawFilePath = @"C:\SimSourceData";
            var jobs = CreateProcessJobs(rawFilePath, rawfiles);
            ProcessGroup group = new ProcessGroup();
            {
            };

            List<ProcessGroup> seqs = new List<ProcessGroup>() { };
            //seqs.Add(seq);

           // processBatch.Sequences = seqs;
            return processBatch;
        }

        public static List<ProcessJob> CreateProcessJobs(string rawFilePath, List<string> rawfiles)
        {
            List<ProcessJob> jobs = new List<ProcessJob>();
            //foreach (var rawfile in rawfiles)
            //{

            //    SampleJob job = new SampleJob()
            //    {
            //        Name = rawfile,
            //        SampleId = "s1",
            //        Id = Guid.NewGuid(),
            //        RawFile = rawfile,
            //        RawFilePath = rawFilePath,
            //        Vial = "A:A1",
            //        CurrentRawFile = rawfile

            //    };
            //    jobs.Add(job);

          //  }
            return jobs;

        }

        public static List<ProcessJob> CreateSampleJobs(string rawFilePath, List<ClientSample> samples)
        {
            List<ProcessJob> jobs = new List<ProcessJob>();
            //foreach (var sample in samples)
            //{
            //    SampleJob job = new SampleJob()
            //    {
            //        Name = sample.Name,
            //        SampleId = "s1",
            //        Id = Guid.NewGuid(),
            //        RawFile = sample.RawFile,
            //        RawFilePath = rawFilePath,
            //        Vial = sample.Vial,
            //        CurrentRawFile = rawFilePath + "\\" + sample.RawFile

            //    };
            //    jobs.Add(job);

            //}
            return jobs;

        }
    }
}
