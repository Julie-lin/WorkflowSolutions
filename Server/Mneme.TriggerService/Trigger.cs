using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Mneme.Components;
using Mneme.ProcessLocator;
using Mneme.Utility;
using Mneme.WCFUtility;
using RawDatabaseAccess;
using Workflow.Data;

namespace Mneme.TriggerService
{
    public partial class Trigger : ServiceBase
    {
        System.Timers.Timer _aTimer = new System.Timers.Timer();
        public Trigger()
        {
            InitializeComponent();
        }

        public void OnStartFromConsoleApp()
        {
            string[] emptyArgsList = null;
            OnStart(emptyArgsList);
        }
        public void OnStopFromConsoleApp()
        {
            //cleanup here Shutdown();
        }

        protected override void OnStart(string[] args)
        {
            _aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _aTimer.Interval = 1000000000;
            _aTimer.Enabled = true;
            
        }

        protected override void OnStop()
        {
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            //List<long> expIds = DatabaseHelp.GetNewlyUploadedExperiements();
            //if (expIds.Count > 0)
            //{
            //    foreach (long id in expIds)
            //    {
            //        //launch trace builder in seperte thread
            //        //LaunchTraceBuilderThread(id);
            //        Guid batchId = Guid.NewGuid(); //application keeps batchId for future query
            //        long expId = id; //experiment id = 0 if only execute workflow against measurement
            //        long measurementId = 1;
            //        string file = @"c:\temp";
            //        List<ComponentNode> list = XmlSerialization.OpenWorkflowFile(file, ProcessRunTimeLocator.GetAppExtraDataTypes());
            //        ChannelFactoryUtility.CallProcessServer(proxy => proxy.ExecutePostUploadActivity(expId, measurementId, list,
            //                                                    Guid.NewGuid(), batchId));
            //    }
            //}
        }

        public void LaunchTraceBuilderThread(long expId)
        {
            Func<long, long> traceBuild = StartTraceBuilderProcess;
            traceBuild.BeginInvoke(expId, EndTraceBuilderProcess, "");
        }

        private void EndTraceBuilderProcess(IAsyncResult result)
        {
            AsyncResult async = (AsyncResult)result;
            var search = (Func<long,long>)async.AsyncDelegate;

            long expId = search.EndInvoke(result);
           // DatabaseHelp.UpdateUploadTableStates(expId);
            Debug.WriteLine("expId is: " + expId.ToString());

        }

        private long StartTraceBuilderProcess(long expId)
        {
            TraceBuilder traceBuilder = new TraceBuilder();
            traceBuilder.BuildTraceFromRawFile((int)expId);
            return expId;
        }
    }
}


