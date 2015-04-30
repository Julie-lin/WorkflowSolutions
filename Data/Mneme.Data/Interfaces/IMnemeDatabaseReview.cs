using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mneme.Data.Interfaces
{
    public interface IMnemeDatabaseReview
    {
        int GetLabCount();
        List<string> GetlabNames();
        int GetProjectCount();
        List<string> GetProjectNames();
        List<string> GetProjectsByLab(string lab);
        List<string> GetExperimentNames();
        List<string> GetExperimentsNamesByProject(string project);
        int GetMeasurementCount();
        List<string> GetMeasurementNamesByExperiment(string experiment);

    }
}
