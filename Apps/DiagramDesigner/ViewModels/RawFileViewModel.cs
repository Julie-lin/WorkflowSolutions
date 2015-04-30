using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;
using AppData;
using DiagramDesigner.Views;
using Mneme.Components;
using Mneme.Data;
using Mneme.ProcessLocator;
using Mneme.Utility;
using Mneme.WCFUtility;
using Workflow.Data;


namespace DiagramDesigner.ViewModels
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading;

    using Mneme.Data.Interfaces;
    using Mneme.UiUtility;

    public class RawFileViewModel : ViewModelBase
    {
        private BackgroundWorker _backgroundWorker = null;
        public event EventHandler WorkflowChanged;

        public BatchSetupView BatchSetupView { get; set; }
        public RawFileViewModel()
        {
            ProgressMessage = new ObservableCollection<string>();
            UiProcessBatch = new ObservableCollection<ProcessItem>();
            Measurements = new ObservableCollection<IdNamePairProcessItem>();
            BatchName = "MyfirstBatch";

            ObservableCollection<ProcessItem> seq = new ObservableCollection<ProcessItem>();
            CurrentGroup = seq;
            ProcessItem item = new IdNamePairProcessItem();
            item.Name = "group1";
            item.ProcessStatus = "idle";
            item.Children = seq;
            item.CrtDepthLevel = 1;
            UiProcessBatch.Add(item);

            ShowProgressBar = true;
            //ObservableCollection<ProcessItem> seq2 = CreateTestSequence(rawfiles);
            //ProcessItem item2 = new ProcessItem();
            //item2.Name = "group2";
            //item2.Children = seq2;
            //UiProcessBatch.Add(item2);


        }

        public List<ComponentNode>  GetComponents()
        {
            List<ComponentNode> comps = new List<ComponentNode>();
            if (_designerItems == null )
            {
                if (string.IsNullOrEmpty(WorkflowFile))
                {
                    //MessageBox.Show("Please select a work flow");
                    return new List<ComponentNode>();
                }
                else
                {
                    comps = XmlSerialization.OpenWorkflowFile(WorkflowFile, ProcessRunTimeLocator.GetAppExtraDataTypes());
                }
                
            }
            else
            {
                foreach (var item in _designerItems)
                {
                    comps.Add(item.ThisComponent);
                }
                
            }
            
            return comps;
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

        
        public void SetViewCanvas( ComponentCanvas canvas)
        {
            //BatchSetupView.ComponentCanvas.Children.Clear();
            //BatchSetupView.ComponentCanvas = canvas;
            //BatchSetupView.ComponentCanvas.InvalidateVisual();
        }
        public string GetEfConnectionString()
        {
            return BatchSetupView.GetEfConnectionString();
        }
        public string GetAdoConnectionString()
        {
            return BatchSetupView.GetAdoConnectionString();
        }
        public string GetDbMachineName()
        {
            return BatchSetupView.GetDbMachineName();
        }

        public ComponentCanvas GetBatchSetupViewCanvas()
        {
            return null;
        }
        public void ClearBatchSetupCanvas()
        {
            BatchSetupView.ClearCanvaschildren();
        }
        public void AddConnectionToBatchSetupView(IEnumerable<XElement> connectionsXML)
        {
            BatchSetupView.AddConnectionsToCanvas(connectionsXML);
        }
        
        public void AddGroup()
        {
            ObservableCollection<ProcessItem> seq = new ObservableCollection<ProcessItem>();
            CurrentGroup = seq;
            ProcessItem item = new IdNamePairProcessItem();
            item.Name = "group" + (UiProcessBatch.Count + 1).ToString();
            item.Children = seq;
            item.CrtDepthLevel = 1;
            UiProcessBatch.Add(item);

        }
        //ShowProgressBar
        private bool _showProgressBar;
        public bool ShowProgressBar
        {
            get { return _showProgressBar; }
            set
            {
                _showProgressBar = value;

                InvokePropertyChanged("ShowProgressBar");
        }
        }


        private string _workflowFile;
        public string WorkflowFile
        {
            get { return _workflowFile; }
            set
            {
                _workflowFile = value;

                InvokePropertyChanged("WorkflowFile");
            }
        }

        //private ReadOnlyObservableCollection<IdNamePairProcessItem> _measurements;
        private ObservableCollection<IdNamePairProcessItem> _measurements;
        public ObservableCollection<IdNamePairProcessItem> Measurements
        {
            get { return _measurements; }
            set
            {
                if (value != _measurements)
                {
                    _measurements = value;
                    //_processBatch.Sequences
                    InvokePropertyChanged("Measurements");
                }
            }

        }
        private ObservableCollection<ProcessItem> _currentGroup;
        
        public ObservableCollection<ProcessItem> CurrentGroup
        {
            get { return _currentGroup; }
            set
            {
                if (value != _currentGroup)
                {
                    _currentGroup = value;
                    //_processBatch.Sequences
                    InvokePropertyChanged("CurrentGroup");
                }
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

        public void ProcessMessageChanged(object sender, EventArgs e)
        {
            var message = ((ProcessMessageEventArg) e).ProcessMessage;
            AddMessagetoJobItems(message);


            // MessageBox.Show(((ProcessMessageEventArg)e).ProcessMessage.Message);

        }

        private ObservableCollection<string> _progressMessage;
        public ObservableCollection<string> ProgressMessage
        {
            get { return _progressMessage; }
            set
            {
                if (value != _progressMessage)
                {
                    _progressMessage = value;
                    //_processBatch.Sequences
                    InvokePropertyChanged("ProgressMessage");
                }
            }
            
        }

        private void AddMessagetoJobItems(ComponentProcessMessage message)
        {
            if (message.MessageSource == MessageSource.ServerBatchLevel && message.Message == "Batch Ended")
            {
                foreach (var groupItem in UiProcessBatch)
                {
                    groupItem.ProcessStatus = "Finished";
                }
            }
            else if (message.MessageSource == MessageSource.ServerGroupLevel && message.Message == "Group Ended")
            {
                var group = UiProcessBatch.FirstOrDefault(c => c.Id == message.GroupId);
                if (group != null)
                    group.ProcessStatus = "Finished";

            }
            else
            {
                foreach (var groupItem in UiProcessBatch)
                {
                    var job = groupItem.Children.FirstOrDefault(c => c.Id == message.JobId);
                    if (job != null && message.Message == "Job Ended")
                    {
                        job.ProcessStatus = "Finished";
                        string ms = string.Format("job {0} get message from {1} : {2}", job.Name,
                            message.MessageSource.ToString(), message.Message);
                        ProgressMessage.Add(ms);
                    }
                }
            }
        }
        public void AddJobToCurrentGroup()
        {
            int count = 0;
            for (int i = 0; i < 1000; i++)
            {
                {
                    string newName = "Job" + "_" + count;
                    if (CurrentGroup.FirstOrDefault(n => n.Name.Equals(newName)) == null)
                    {
                        CurrentGroup.Add(new IdNamePairProcessItem() { Name = newName });
                        break;
                    }
                }
                count++;
            }

        }

        public void AddRawFileJobsToCurrentGroup(string file)
        {
            ProcessItem item = new IdNamePairProcessItem() { Name = file };
            CurrentGroup.Add(item);
        }

        public void AddRawFileJobsToCurrentGroup(IdNamePairProcessItem item)
        {
            if (item.CrtDepthLevel == 3)
            {
                ProcessItem addedItem = new IdNamePairProcessItem()
                {
                    Name = item.Name,
                    MeasurementId = item.MeasurementId
                };
                CurrentGroup.Add(addedItem);
            }
            else if (item.CrtDepthLevel == 2)
            {
                foreach (IdNamePairProcessItem projectItem in item.Children)
                {
                    ProcessItem addedItem = new IdNamePairProcessItem()
                    {
                        Name = projectItem.Name,
                        MeasurementId = projectItem.MeasurementId
                    };
                    CurrentGroup.Add(addedItem);
                }
            }
            else if (item.CrtDepthLevel == 1)
            {
                foreach (IdNamePairProcessItem projectItem in item.Children)
                {
                    foreach (IdNamePairProcessItem chilIten in projectItem.Children)
                    {
                        ProcessItem addedItem = new IdNamePairProcessItem()
                        {
                            Name = chilIten.Name,
                            MeasurementId = chilIten.MeasurementId
                        };
                        CurrentGroup.Add(addedItem);
                    }
                }
            }

        }

        public void RemoveItemFromCurrentGroup(ProcessItem item)
        {
            string name = item.Name;
            var child = CurrentGroup.FirstOrDefault(i => i.Name == name);
            if (child != null && child.Name == name)
            {
                CurrentGroup.Remove(child);
            }
        }

        public string BatchName { get; set; }
        public void SetUIProcessBatchRunningStatus()
        {
            foreach (var item in UiProcessBatch)
            {
                item.ProcessStatus = "Executing";
                foreach (var child in item.Children)
                {
                    child.ProcessStatus = "Executing";
                    foreach (var subChild in child.Children)
                    {
                        if (subChild != null)
                        {
                            subChild.ProcessStatus = "Executing";
                        }
                    }
                }
            }
        }
        public ProcessBatch GetProcessBatch()
        {
            var processBatch = new MnemeBatch()
            {
                Id = Guid.NewGuid(),
                UserId = "me",
                Name = BatchName,
                Path = @"C:\Prototype\WorkflowTest\TestData",
                Extension = ".test",
                ExtraInfo = new Collection<ExtraProcessInfo>()
                                                         {
                                                             new ExtraProcessInfo(){Key = "file", Value = "path"}
                                                         }
            };
            List<ProcessGroup> seqs = new List<ProcessGroup>() { };
            string rawFilePath = @"C:\SimSourceData";
            int count = 0;
            foreach (var item in UiProcessBatch)
            {
                var jobs = CreateProcessJobs(item, rawFilePath);
                if (jobs.Count > 0)
                {
                    ProcessGroup seq = new ProcessGroup()
                    {
                        Id = Guid.NewGuid(),//item.Id,
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

        private List<ProcessJob> CreateProcessJobs(ProcessItem processItem, string path)
        {
            List<ProcessJob> jobs = new List<ProcessJob>();
            foreach (IdNamePairProcessItem subItem in processItem.Children)
            {
                var job = new MnemeSampleJob()
                {
                    Name = Path.GetFileName(subItem.Name),
                    SampleId = "s1",
                    Id = Guid.NewGuid(),//subItem.Id,
                    RawFile = subItem.Name,
                    RawFilePath = path,
                    JobWorkId = subItem.MeasurementId
                };
                jobs.Add(job);

            }
            return jobs;
        }

        
        public void FireDesignItemChanged(List<DesignerItem> items, List<Connection> connections, 
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
        public void FireWorkflowChanged(List<DesignerItem> items, List<Connection> connections, 
            ComponentCanvas canvas)
        {
            if (WorkflowChanged != null)
            {
                WorkflowChanged(this, new CanvasElementEventArgs() { DesignItems = items, 
                    Connections = connections,
                    Canvas = canvas
                });
            }
        }

        public void GetSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            BatchSetupView.ShowLoadingBar(Visibility.Hidden);
            try
            {
                Measurements.Clear();
                string machineName = this.GetDbMachineName();
                string connectionString = this.GetEfConnectionString();
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("no connection string");
                    return;
                }
                this.Measurements = new ObservableCollection<IdNamePairProcessItem>();
                _machineName = machineName;
                _connectionString = connectionString;

                using (BusyCursor busyCursor = new BusyCursor())
                {

                    this.PopulateTree();
                }
                //_backgroundWorker = new BackgroundWorker();
                //_backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWork_DoWork);
                //_backgroundWorker.WorkerReportsProgress = true;
                ////_backgroundWorker.WorkerSupportsCancellation = true;
                //_backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
                //_backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                //_backgroundWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                
            }
        }

        private string _machineName;
        private string _connectionString;
        void backgroundWork_DoWork(object sender, DoWorkEventArgs e)
        {
            Debug.WriteLine("backgroundWork_DoWork ticked");
            _backgroundWorker.ReportProgress(2, "still progress");
            PopulateTree();
            //if (_backgroundWorker != null && _sortedResults.Count > 0)
            //    _backgroundWorker.ReportProgress(_count / _sortedResults.Count);

            //using (BusyCursor busyCursor = new BusyCursor())
            {

                Thread.Sleep(5000);
            }
        }
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                //this.Measurements.Clear();
            }

            // Change the value of the ProgressBar to the BackgroundWorker progress.

        }
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Measurements = _tempMeasurement;
            _backgroundWorker = null;
        }
        ObservableCollection<IdNamePairProcessItem> _tempMeasurement = new ObservableCollection<IdNamePairProcessItem>();
        private void PopulateTree()
        {

            try
            {
                _tempMeasurement.Clear();
                IRawDataSummary service = ChannelFactoryUtility.CreateRawDataSummaryService(_machineName);
                List<IdNamePair> projects = service.GetProjectNames(_connectionString);
                //List<IdNamePair> projects = ChannelFactoryUtility.CallRawDataSummaryServer(proxy => proxy.GetProjectNames(connectionString));
                foreach (var idNamePair in projects)
                {
                    IdNamePairProcessItem pItem = new IdNamePairProcessItem()
                    {
                        Name = idNamePair.Id + " - " + idNamePair.Name,
                        MeasurementId = idNamePair.Id,
                        CrtDepthLevel = 1 //project level is 1
                    };
                    //_tempMeasurement.Add(pItem);
                    this.Measurements.Add(pItem);

                    List<IdNamePair> experiments = service.GetExperimentsNamesByProjectId((int)idNamePair.Id, _connectionString);
                    //List<IdNamePair> experiments = ChannelFactoryUtility.CallRawDataSummaryServer(proxy => proxy.GetExperimentsNamesByProjectId((int)idNamePair.Id, connectionString));
                    foreach (var experiment in experiments)
                    {
                        IdNamePairProcessItem eItem = new IdNamePairProcessItem()
                        {
                            Name = experiment.Id + " - " + experiment.Name,
                            MeasurementId = experiment.Id,
                            CrtDepthLevel = 2 //experiment level is 2
                        };
                        pItem.Children.Add(eItem);
                        List<IdNamePair> measurements = service.GetMeasurementNamesByExperimentId(
                            (int)eItem.MeasurementId, _connectionString);
                        //List<IdNamePair> measurements = ChannelFactoryUtility.CallRawDataSummaryServer(proxy => proxy.GetMeasurementNamesByExperimentId((int)eItem.MeasurementId, connectionString));
                        foreach (var measurement in measurements)
                        {
                            IdNamePairProcessItem mItem = new IdNamePairProcessItem()
                            {
                                Name = measurement.Id + " - " + measurement.Name,
                                MeasurementId = measurement.Id,
                                CrtDepthLevel = 3 //meaurement level is 3
                            };
                            eItem.Children.Add(mItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            BatchSetupView.ShowLoadingBar(Visibility.Hidden);
        }


        }
    }
