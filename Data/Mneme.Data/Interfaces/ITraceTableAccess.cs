using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Mneme.Data.Interfaces
{
    [ServiceContract]
    public interface ITraceTableAccess
    {
        [OperationContract]
        void AddToTraceTable(int measurementId);
    }
}
