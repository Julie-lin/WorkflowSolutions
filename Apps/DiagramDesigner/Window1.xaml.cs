using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using AppData;
using DiagramDesigner.ViewModels;
using Thermo.Workflows.Contracts;
using Thermo.Workflows.Contracts.Client;

namespace DiagramDesigner
{
    public partial class Window1 : Window, INotifyPropertyChanged, IWorkItemCancellationService
    {
        private ServiceHost callbackServiceHost;
        //MessageCallbackHandler messageCallback = new MessageCallbackHandler();
        //private ServiceHost messageCallbackServiceHost;
        public Window1()
        {
            InitializeComponent();
           // this.listBox1.ItemsSource = RawFileViewModel.RawFiles;
            DataContext = this;

            //need to redesign in the future
            BatchSetupView.DataContext = MyDesigner.SelectionService.RawFileViewModel;
            BatchSetupView.SetViewModel(MyDesigner.SelectionService.RawFileViewModel);
            MyDesigner.SelectionService.RawFileViewModel.BatchSetupView = BatchSetupView;
            try
            {
                //MessagesFromServer = new ObservableCollection<string>();

                //ProgressCallbackService callbackService = new ProgressCallbackService(Dispatcher, false);

                //using (ChannelFactory<IProgressTrackingSubscrption> channelFactory =
                //        new ChannelFactory<IProgressTrackingSubscrption>("progressTrackingSubscription"))
                //{
                //    IProgressTrackingSubscrption progressTrackingSubscrption = channelFactory.CreateChannel();
                //    progressTrackingSubscrption.SubscribeToProgressEvents();

                //    ProgressMonitor = progressTrackingSubscrption.GetActiveWorkItemsStatus();
                //    callbackService.ProgressMonitor = ProgressMonitor;

                //    ProgressMonitor.Subscribe(this);
                //}

                //callbackServiceHost = new ServiceHost(callbackService);
                //callbackServiceHost.Open();

                //SetupProgressObserver();

                ////messageCallbackServiceHost = new ServiceHost(messageCallback);
                ////messageCallbackServiceHost.Open();
                ////messageCallback.MessageChanged += this.ProcessMessageChanged;

                App.Current.MainWindow = this;
            }
            catch (Exception)
            {
                
               
            }
        }

        public ObservableCollection<string> MessagesFromServer { get; set; }
        //private WorkItemProgressObserver _progressMonitor;
        private void SetupProgressObserver()
        {
            //_workItemProgressControl = new WorkItemProgressControl();
            //placeholder.Child = _workItemProgressControl;

            //_workItemProgressControl.CancellationService = this;

            //BindingOperations.SetBinding(_workItemProgressControl, WorkItemProgressControl.ProgressMonitorProperty,
            //                             new Binding
            //                             {
            //                                 Source = this,
            //                                 Path = new PropertyPath("ProgressMonitor")
            //                             });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void InvokePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void CancelWorkItem(List<Guid> rootToCanceledChildItemPath)
        {
            //using (ChannelFactory<IWorkItemCancellationService> channelFactory =
            //new ChannelFactory<IWorkItemCancellationService>("workItemCancellationService"))
            //{
            //    IWorkItemCancellationService workItemCancellationService = channelFactory.CreateChannel();
            //    workItemCancellationService.CancelWorkItem(rootToCanceledChildItemPath);
            //}

        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }


        private RawFileViewModel RawFileViewModel { get; set; }

        private void tabItem1_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.MyDesigner.SelectionService.CurrentSelectedPage = TabPageName.CanvasPage;
        }

        private void tabItem2_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.MyDesigner.SelectionService.CurrentSelectedPage = TabPageName.SampleSetupPage;
        }
        private void tabItem3_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.MyDesigner.SelectionService.CurrentSelectedPage = TabPageName.SampleSetupPage;
        }

        private void tabItem6_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.MyDesigner.SelectionService.CurrentSelectedPage = TabPageName.BatchSetupPage;
        }


        private Guid _sampleId;
        private void ProcessMessageChanged(object sender, EventArgs e)
        {
            var message = ((ProcessMessageEventArg) e).ProcessMessage.Message;
            
           // MessageBox.Show(((ProcessMessageEventArg)e).ProcessMessage.Message);

        }

        
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                //messageCallback.MessageChanged -= this.ProcessMessageChanged;
                //Process currentProcess = Process.GetCurrentProcess();
                //Utility.CallServer(proxy => proxy.CloseSessionService(currentProcess.Id.ToString()));
            }
            catch
            {
            }
        }




        private void componentCanvas1_GotMouseCapture(object sender, MouseEventArgs e)
        {

        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }


    }
}
