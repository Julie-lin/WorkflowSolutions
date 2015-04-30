using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Mneme.Data;
using Mneme.Data.Interfaces;
using RawDataAccess;

namespace MnemeConsoleHost
{
    //jlin to do list consider adapter be on demand to save memory
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RawDataManagerService : IRawDataSummary
    {
        
        private RawDataSummary _adapter;

        public RawDataManagerService()
        {
            _adapter = new RawDataSummary();
        }
        public List<IdNamePair> GetlabNames()
        {
            AssertAdapterIsNull();
            return _adapter.GetlabNames();
        }

        public List<IdNamePair> GetProjectNames()
        {
            AssertAdapterIsNull();
            return _adapter.GetProjectNames();
        }

        public List<IdNamePair> GetExperimentNames()
        {
            AssertAdapterIsNull();
            return _adapter.GetExperimentNames();
        }

        public List<IdNamePair> GetMeasurementNames()
        {
            AssertAdapterIsNull();
            return _adapter.GetMeasurementNames();
        }

        public List<IdNamePair> GetProjectsByLab(string lab)
        {
            AssertAdapterIsNull();
            throw new NotImplementedException();
        }

        public List<IdNamePair> GetProjectsByLabId(int labId)
        {
            throw new NotImplementedException();
        }

        public List<IdNamePair> GetExperimentsNamesByProject(string project)
        {
            AssertAdapterIsNull();
            return _adapter.GetExperimentsNamesByProject(project);
        }

        public List<IdNamePair> GetExperimentsNamesByProjectId(int projectId)
        {

            return _adapter.GetExperimentsNamesByProjectId(projectId);
        }

        public List<IdNamePair> GetMeasurementNamesByExperiment(string experiment)
        {
            AssertAdapterIsNull();
            return GetMeasurementNamesByExperiment(experiment);
        }

        public List<IdNamePair> GetMeasurementNamesByExperimentId(int experimentId)
        {
            AssertAdapterIsNull();
            return _adapter.GetMeasurementNamesByExperimentId(experimentId);
        }

        public List<IdNamePair> GetMeasurementIdNamePair(string experiment)
        {
            AssertAdapterIsNull();
            return _adapter.GetMeasurementIdNamePair(experiment);
        }

        private void AssertAdapterIsNull()
        {
            if (_adapter == null)
            {
                throw new InvalidOperationException("");
            }
        }

    }
}
