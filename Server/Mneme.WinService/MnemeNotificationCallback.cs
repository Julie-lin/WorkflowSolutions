using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workflow.Data;
using Workflow.Data.Interfaces;

namespace Mneme.WinService
{
    public class MnemeNotificationCallback : INotificationCallback
    {
        public void OnNotification(string msgType, string msg)
        {
            throw new NotImplementedException();
        }

        public void SendClientNotification(ComponentProcessMessage message)
        {
            if (message.HasError)
            {
                //WinService.Log.Error(message);
            }
            else
            {
                //WinService.Log.Info(message);
            }

            //log.Debug("Other Class - Debug logging");
            //log.Info("Other Class - Info logging");
            //log.Warn("Other Class - Warn logging");
            //log.Error("Other Class - Error logging");
            //log.Fatal("TestLogerClass - Fatal logging");
            
        }
    }
}
