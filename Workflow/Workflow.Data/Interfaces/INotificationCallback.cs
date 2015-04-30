using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Workflow.Data.Interfaces
{
    [ServiceContract]
    public interface INotificationCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnNotification(string msgType, string msg);
        [OperationContract(IsOneWay = true)]
        void SendClientNotification(ComponentProcessMessage message);
    }

}
