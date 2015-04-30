using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AppData;
using DiagramDesigner.ViewModels;
using Mneme.Utility;
using Workflow.Data;
using Workflow.Data.Interfaces;

namespace Mneme.Data
{
    using System.ComponentModel;
 
    [Serializable]
    [DataContract]
    public class MnemeBatch : ProcessBatch
    {
        public MnemeBatch()
        {
            Id = Guid.NewGuid();
            Name = "TestAcq";
            ProjectName = "";
            //Samples = new List<MnemeSampleJob>();
            UserId = "user1";
            Extension = ".eBatch";
            EfConnectionString = string.Empty;
            AdoConnectionString = string.Empty;
            Path = EngineUtility.GetDataPath();
            ExcelOutput = Path + "\\" + "FramePeaks.xlsx";// @"C:\ThermoFisher\Mneme\TestData\FramePeaks.xlsx";
        }

        #region Implementation of IProcessBatch

        #endregion
        [DataMember]
//        [Browsable(false)]
        public string EfConnectionString { get; set; }
        [DataMember]
        public string AdoConnectionString { get; set; }
        [DataMember]
        public string ExcelOutput { get; set; }

        public ProcessBatch CreateBatchFromMeasurements(MnemeBatch batch, List<MeasurementIdNamePair> jobList)
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
            processBatch.AppBatch = this;

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
        public ProcessBatch Create1SequenceProcessBatch(MnemeBatch batch)
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
            var jobs = Create1ProcessJob(rawFilePath, processes);
            ProcessGroup seq = new ProcessGroup()
            {
                Name = "sequence 1",
                EndOfSequenceProcess = "ClientEndOfSequenceProcess",
                Jobs = jobs,
                Description = "group test"
            };
            List<ProcessGroup> seqs = new List<ProcessGroup>() { };
            seqs.Add(seq);


            //var jobs2 = CreateProcessJobs(rawFilePath, processes);
            //ProcessGroup seq2 = new ProcessGroup()
            //{
            //    Name = "sequence 2",
            //    EndOfSequenceProcess = "ClientEndOfSequenceProcess",
            //    Jobs = jobs2,
            //};
            //seqs.Add(seq2);

            processBatch.Groups = seqs;
            processBatch.AppBatch = this;

            return processBatch;

        }

        private static List<ProcessJob> Create1ProcessJob(string rawFilePath, List<string> processes)
        {
            List<ProcessJob> jobs = new List<ProcessJob>()
                                        {
                                            new MnemeSampleJob()
                                                {
                                                    Name = "Sample1",
                                                    Description = "job test",
                                                    SampleId = "s1",
                                                    Id = Guid.NewGuid(),
                                                    ProcessJobNames = processes,
                                                    RawFile = @"C:\Thermo\Mneme\RAW\011111-Disulfide\DilsulfideTest01.raw",
                                                    RawFilePath = rawFilePath,
                                                    JobWorkId = 1
                                                },

                                        };
            return jobs;

        }

        public ProcessBatch Create2SequenceProcessBatch(MnemeBatch batch)
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
            var jobs = CreateProcessJobs(rawFilePath, processes);
            ProcessGroup seq = new ProcessGroup()
            {
                Name = "sequence 1",
                EndOfSequenceProcess = "ClientEndOfSequenceProcess",
                Jobs = jobs,
                Description = "group test"
            };
            List<ProcessGroup> seqs = new List<ProcessGroup>() { };
            seqs.Add(seq);


            var jobs2 = CreateProcessJobs(rawFilePath, processes);
            ProcessGroup seq2 = new ProcessGroup()
            {
                Name = "sequence 2",
                EndOfSequenceProcess = "ClientEndOfSequenceProcess",
                Jobs = jobs2,
            };
            seqs.Add(seq2);

            processBatch.Groups = seqs;
            processBatch.AppBatch = this;

            return processBatch;

        }

        private static List<ProcessJob> CreateProcessJobs(string rawFilePath, List<string> processes)
        {
            List<ProcessJob> jobs = new List<ProcessJob>()
                                        {
                                            new MnemeSampleJob()
                                                {
                                                    Name = "Sample1",
                                                    Description = "job test",
                                                    SampleId = "s1",
                                                    Id = Guid.NewGuid(),
                                                    ProcessJobNames = processes,
                                                    RawFile = @"C:\SimSourceData\drugx_01",
                                                    RawFilePath = rawFilePath,
                                                    JobWorkId = 1
                                                },
                                            new MnemeSampleJob()
                                                {
                                                    Name = "Sample2",
                                                    Description = "job test2",
                                                    SampleId = "s2",
                                                    Id = Guid.NewGuid(),
                                                    ProcessJobNames = processes,
                                                    RawFile = @"C:\SimSourceData\drugx_02",
                                                    RawFilePath = rawFilePath,
                                                    JobWorkId = 1
                                                },
                                            new MnemeSampleJob()
                                                {
                                                    Name = "Sample3",
                                                    Description = "job test3",
                                                    SampleId = "s3",
                                                    Id = Guid.NewGuid(),
                                                    ProcessJobNames = processes,
                                                    RawFile = @"C:\SimSourceData\drugx_03",
                                                    RawFilePath = rawFilePath,
                                                    JobWorkId = 1
                                                },
                                            new MnemeSampleJob()
                                                {
                                                    Name = "Sample4",
                                                    Description = "job test4",
                                                    SampleId = "s4",
                                                    Id = Guid.NewGuid(),
                                                    ProcessJobNames = processes,
                                                    RawFile = @"C:\SimSourceData\drugx_04",
                                                    RawFilePath = rawFilePath,
                                                    JobWorkId = 1
                                                },

                                        };
            return jobs;

        }
    }

}
