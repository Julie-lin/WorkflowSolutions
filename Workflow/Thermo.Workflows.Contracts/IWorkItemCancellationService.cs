using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Thermo.Workflows.Contracts
{
    [ServiceContract]
    public interface IWorkItemCancellationService
    {
        [OperationContract(IsOneWay = true)]
        void CancelWorkItem(List<Guid> rootToCanceledChildItemPath);
    }
}
