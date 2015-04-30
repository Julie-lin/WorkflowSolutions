using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Workflow.Data.Interfaces;

namespace Workflow.Activities
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

        public static IExecuteStartupComponent LocateStartComponentProcess(string executionClassName)
        {
            if (string.IsNullOrEmpty(executionClassName))
            {
                return null;
            }
            return ProcessJobLocator.Resolve<IExecuteStartupComponent>(executionClassName);
        }
        public static IExcuteComponent LocateComponentProcess(string executionClassName)
        {
            if (string.IsNullOrEmpty(executionClassName))
            {
                return null;
            }
            return ProcessJobLocator.Resolve<IExcuteComponent>(executionClassName);
        }

        public static IExecuteGroupComponent LocateGroupComponentProcess(string executionClassName)
        {
            if (string.IsNullOrEmpty(executionClassName))
            {
                return null;
            }
            return ProcessJobLocator.Resolve<IExecuteGroupComponent>(executionClassName);
        }
        public static IExecuteBatchComponent LocateBatchComponentProcess(string executionClassName)
        {
            if (string.IsNullOrEmpty(executionClassName))
            {
                return null;
            }
            return ProcessJobLocator.Resolve<IExecuteBatchComponent>(executionClassName);
        }

        public static IInitializeBatch LocateBatchInitializeProcess(string executionClassName)
        {
            if (string.IsNullOrEmpty(executionClassName))
            {
                return null;
            }
            return ProcessJobLocator.Resolve<IInitializeBatch>(executionClassName);
        }

    }
}
