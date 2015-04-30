using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Workflow.Data;
using Workflow.Data.Interfaces;


namespace Utility
{
    public static class ProcessObjectLocator
    {
        private static readonly UnityContainer ProcessJobLocator;

        static ProcessObjectLocator()
        {
            if (ProcessJobLocator == null)
            {
                try
                {
                    ProcessJobLocator = new UnityContainer();

                    // UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                    //section.Configure(ProcessJobLocator, "ProcessJobLocator");

                    ProcessJobLocator.LoadConfiguration("ProcessJobLocator");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }

        }

        public static IProcess LocateProcess(string processName)
        {
            if (string.IsNullOrEmpty(processName))
            {
                return null;
            }
            return ProcessJobLocator.Resolve<IProcess>(processName);
        }

        public static IExcuteComponent LocateComponentProcess(string componentName)
        {
            if (string.IsNullOrEmpty(componentName))
            {
                return null;
            }
            return ProcessJobLocator.Resolve<IExcuteComponent>(componentName);
        }

        public static ComponentNode LocateComponentPama(string componentName)
        {
            if (string.IsNullOrEmpty(componentName))
            {
                return null;
            }
            return ProcessJobLocator.Resolve<ComponentNode>(componentName);
            
        }


    }
}
