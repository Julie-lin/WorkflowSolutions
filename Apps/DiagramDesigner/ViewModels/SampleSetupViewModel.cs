using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AppData;
using DiagramDesigner.Views;
using Workflow.Data;


namespace DiagramDesigner.ViewModels
{
    public class SampleSetupViewModel : ViewModelBase
    {
        public event EventHandler WorkflowChanged;
        public SampleSetupViewModel()
        {
            BatchName = "sampleProcess";
            _samples = new ObservableCollection<ClientSample>();
            UiProcessBatch = new ObservableCollection<ProcessItem>(){new ProcessItem(){Name = "Group1", CrtDepthLevel = 1}};
            CurrentGroup = _processBatch[0];
            
            CreateTestData();

        }

        private ProcessItem _currentGroup;

        public ProcessItem CurrentGroup
        {
            get { return _currentGroup; }
            set
            {
                if (value != _currentGroup)
                {
                    _currentGroup = value;
                    SetupCurrentSampleJobs();
                    InvokePropertyChanged("CurrentGroup");
                }
            }

        }

        private void SetupCurrentSampleJobs()
        {
            Samples.Clear();
            foreach (var item in _currentGroup.Children)
            {
                Samples.Add((ClientSample)item.Job);
            }
        }

        public void AddGroup()
        {
            string name = "Group" + UiProcessBatch.Count + 1;
            ProcessItem newGroup = new ProcessItem(){Name = name};
            UiProcessBatch.Add(newGroup);
            CurrentGroup = newGroup;
            Samples.Clear();
        }
        public void AddSampleToSamples(string fileName)
        {
            ClientSample sample = new ClientSample()
                                      {
                                          RawFile = Path.GetFileNameWithoutExtension(fileName),
                                          RawFilePath = Path.GetFullPath(fileName),
                                          Name = Path.GetFileNameWithoutExtension(fileName)
                                      };
            Samples.Add(sample);
        }

        public void RebindingSamplesToCurrentGroup()
        {
            CurrentGroup.Children.Clear();
            foreach (var sample in Samples)
            {
                ProcessItem item = new ProcessItem(){Name = sample.Name, Job = sample};
                CurrentGroup.Children.Add(item);
            }
        }

        public object CurrentJob { get; set; }
        private ObservableCollection<ProcessItem> _processBatch;
        [XmlArray]
        public ObservableCollection<ProcessItem> UiProcessBatch
        {
            get { return _processBatch; }
            set
            {
                if (value != _processBatch)
                {
                    _processBatch = value;
                    //_processBatch.Sequences
                    InvokePropertyChanged("ProcessBatch");
                }
            }

        }

        private List<DesignerItem> _designerItems;
        public List<DesignerItem> DesignerItems
        {
            set
            {
                if (_designerItems != value)
                {
                    _designerItems = value;
                }
            }
        }
        public List<ComponentNode> GetComponents()
        {
            List<ComponentNode> comps = new List<ComponentNode>();
            foreach (var item in _designerItems)
            {
                comps.Add(item.ThisComponent);
            }
            return comps;
        }
        public string BatchName { get; set; }
        public ProcessBatch GetProcessBatch()
        {
            var processBatch = new ProcessBatch()
            {
                Id = Guid.NewGuid(),
                UserId = "me",
                Name = "My process batch",
                Path = @"C:\Prototype\WorkflowTest\TestData",
                Extension = ".test",
                ExtraInfo = new Collection<ExtraProcessInfo>()
                                                         {
                                                             new ExtraProcessInfo(){Key = "file", Value = "path"}
                                                         }
            };
            string rawFilePath = @"C:\SimSourceData";
            List<ProcessGroup> seqs = new List<ProcessGroup>() { };
            int count = 0;
            foreach (var item in UiProcessBatch)
            {
                var jobs = CreateProcessJobs(item, rawFilePath);
                if (jobs.Count > 0)
                {
                    ProcessGroup seq = new ProcessGroup()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Jobs = jobs
                    };
                    count++;
                    seqs.Add(seq);

                }


            }


            processBatch.Groups = seqs;
            return processBatch;
        }

        public List<ProcessJob> CreateProcessJobs(ProcessItem processItem, string path)
        {
            List<ProcessJob> jobs = new List<ProcessJob>();
            foreach (var subItem in processItem.Children)
            {
                ClientSample sample = (ClientSample)subItem.Job;
                if (sample == null)
                    break;
                //SampleJob job = new SampleJob()
                //{
                //    Name = Path.GetFileName(subItem.Name),
                //    SampleId = "s1",
                //    Id = subItem.Id,
                //    RawFile = sample.RawFile,
                //    RawFilePath = sample.RawFilePath,
                //    Vial = "A:A1",
                //    CurrentRawFile = sample.RawFile

                //};
                //jobs.Add(job);

            }
            return jobs;

        }

        private void CreateTestData ()
        {
            string path = @"C:\SimSourceData";
            List<ClientSample> ss = ClientSample.CreateTestClientSample(path);
            foreach (var clientSample in ss)
            {
                _samples.Add(clientSample);   
            }
            RebindingSamplesToCurrentGroup();
        }

        private ObservableCollection<ClientSample> _samples;
        public ObservableCollection<ClientSample> Samples
        {
            get { return _samples; }
            set
            {
                if (_samples != value)
                {
                    _samples = value;
                    InvokePropertyChanged("Samples");
                }
            }
        }

        private void CreateTestResults()
        {
            //Samples.Add(new ClientSample("first ", 11));
            //Samples.Add(new ClientSample("second ", 22222));
        }
        public void FireWorkflowChanged(List<DesignerItem> items, List<Connection> connections,
            ComponentCanvas canvas)
        {
            if (WorkflowChanged != null)
            {
                WorkflowChanged(this, new CanvasElementEventArgs()
                {
                    DesignItems = items,
                    Connections = connections,
                    Canvas = canvas
                });
            }
        }

    }
}
