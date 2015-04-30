using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Workflow.Data;


namespace DiagramDesigner.ViewModels
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(ProcessJob))]
    public class ClientSample : ProcessJob, INotifyPropertyChanged
    {

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
        /// Path
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// comment
        /// </summary>
        [DataMember]
        public string Comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// ///jlin need this?
        [DataMember]
        public DateTime AcqusitionDate { get; set; }



        public static List<ClientSample> CreateTestClientSample(string rawFilePath)
        {
            List<ClientSample> samples = new List<ClientSample>();
            ClientSample sample = new ClientSample()
                                      {
                                          Name = "Sample1",
                                          SampleId = "s1",
                                          RawFile = @"C:\SimSourceData\drugx_01",
                                          RawFilePath = rawFilePath,
                                          Vial = "A:A1",
                                          
                                      };
            samples.Add(sample);

            sample = new ClientSample()
            {
                Name = "Sample2",
                SampleId = "s2",
                RawFile = @"C:\SimSourceData\drugx_02",
                RawFilePath = rawFilePath,
                Vial = "A:A2",

            };
            samples.Add(sample);
            sample = new ClientSample()
            {
                Name = "Sample3",
                SampleId = "s3",
                RawFile = @"C:\SimSourceData\drugx_03",
                RawFilePath = rawFilePath,
                Vial = "A:A3",
            };
            samples.Add(sample);

            sample = new ClientSample()
            {
                Name = "Sample4",
                SampleId = "s4",
                RawFile = @"C:\SimSourceData\drugx_04",
                RawFilePath = rawFilePath,
                Vial = "A:A4",
            };
            samples.Add(sample);

            sample = new ClientSample()
            {
                Name = "Sample5",
                SampleId = "s5",
                RawFile = @"C:\SimSourceData\drugx_05",
                RawFilePath = rawFilePath,
                Vial = "A:A5",
            };
            samples.Add(sample);
            return samples;
        }

        public static List<ProcessJob> CreateProcessJobs(string rawFilePath, List<ClientSample> samples, List<string> processes)
        {
            List<ProcessJob> jobs = new List<ProcessJob>();
            //List<ProcessJob> jobs = new List<ProcessJob>()
            //                            {
            //                                new SampleJob()
            //                                    {
            //                                        Name = "Sample1",
            //                                        SampleId = "s1",
            //                                        Id = System.Guid.NewGuid(),
            //                                        ProcessJobNames = processes,
            //                                        RawFile = @"C:\SimSourceData\drugx_01",
            //                                        RawFilePath = rawFilePath,
            //                                        Vial = "A:A1",
            //                                        CurrentRawFile = @"C:\SimSourceData\drugx_01"
                                                    
            //                                    },
            //                                new SampleJob()
            //                                    {
            //                                        Name = "Sample2",
            //                                        SampleId = "s2",
            //                                        Id = System.Guid.NewGuid(),
            //                                        ProcessJobNames = processes,
            //                                        RawFile = @"C:\SimSourceData\drugx_02",
            //                                        RawFilePath = rawFilePath,
            //                                        Vial = "A:A2",
            //                                        CurrentRawFile = @"C:\SimSourceData\drugx_02"
            //                                    },
            //                                new SampleJob()
            //                                    {
            //                                        Name = "Sample3",
            //                                        SampleId = "s3",
            //                                        Id = System.Guid.NewGuid(),
            //                                        ProcessJobNames = processes,
            //                                        RawFile = @"C:\SimSourceData\drugx_03",
            //                                        RawFilePath = rawFilePath,
            //                                        Vial = "A:A3",
            //                                        CurrentRawFile = @"C:\SimSourceData\drugx_03"
            //                                    },
            //                                new SampleJob()
            //                                    {
            //                                        Name = "Sample4",
            //                                        SampleId = "s4",
            //                                        Id = System.Guid.NewGuid(),
            //                                        ProcessJobNames = processes,
            //                                        RawFile = @"C:\SimSourceData\drugx_04",
            //                                        RawFilePath = rawFilePath,
            //                                        Vial = "A:A4",
            //                                        CurrentRawFile = @"C:\SimSourceData\drugx_04"
            //                                    },

            //                                new SampleJob()
            //                                    {
            //                                        Name = "Sample5",
            //                                        SampleId = "s5",
            //                                        Id = System.Guid.NewGuid(),
            //                                        ProcessJobNames = processes,
            //                                        RawFile = @"C:\SimSourceData\drugx_05",
            //                                        RawFilePath = rawFilePath,
            //                                        Vial = "A:A5",
            //                                        CurrentRawFile = @"C:\SimSourceData\drugx_05"
            //                                    },

            //                            };
            return jobs;

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
