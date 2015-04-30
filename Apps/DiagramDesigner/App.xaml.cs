using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Mneme.ProcessLocator;

namespace DiagramDesigner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ProcessRunTimeLocator.RegisterMnemeComponent();
            Window1 mainWindow = new Window1();
            mainWindow.Show();
            Debug.WriteLine("");
            

        }


        private void Application_Exit(object sender, ExitEventArgs e)
        {
           
        }


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            
        }

    }
}
