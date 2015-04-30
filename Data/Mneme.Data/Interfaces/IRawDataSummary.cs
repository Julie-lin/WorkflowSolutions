using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Mneme.Data.Interfaces
{
    [ServiceContract]
    public interface IRawDataSummary
    {
        [OperationContract]
        List<IdNamePair> GetlabNames(string connectionString);
        [OperationContract]
        List<IdNamePair> GetProjectNames(string connectionString);
        [OperationContract]
        List<IdNamePair> GetExperimentNames(string connectionString);
        [OperationContract]
        List<IdNamePair> GetMeasurementNames(string connectionString);
        [OperationContract]
        List<IdNamePair> GetProjectsByLab(string lab, string connectionString);
        [OperationContract]
        List<IdNamePair> GetProjectsByLabId(int labId, string connectionString);
        [OperationContract]
        List<IdNamePair> GetExperimentsNamesByProject(string project, string connectionString);
        [OperationContract]
        List<IdNamePair> GetExperimentsNamesByProjectId(int projectId, string connectionString);
        [OperationContract]
        List<IdNamePair> GetMeasurementNamesByExperiment(string experiment, string connectionString);
        [OperationContract]
        List<IdNamePair> GetMeasurementNamesByExperimentId(long experimentId, string connectionString);
        [OperationContract]
        List<IdNamePair> GetMeasurementIdNamePair(string experiment, string connectionString);

    }
}
