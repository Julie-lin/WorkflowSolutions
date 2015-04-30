using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.TriggerService
{
    static class Program
    {
        private static Mutex serviceRunning;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            try
            {
                //MultiProcessFileLog.WriteLine("Main() - ENTERED, ThermoFisher.MetQuest.WinService is STARTING");

                bool _mutexWasCreated;
                serviceRunning = new Mutex(true, @"Global\ThermoFisher.MetQuest.Service_Running", out _mutexWasCreated);
                if (_mutexWasCreated)
                {
                    try
                    {
                        Trigger service = new Trigger();

                        if (Environment.UserInteractive)
                        {
                            service.OnStartFromConsoleApp();
                            //Console.WriteLine("Press any key to stop program");
                            //Console.Read();
                            //service.OnStopFromConsoleApp();
                        }
                        else
                        {
                            ServiceBase[] servicesToRun = new ServiceBase[]
                                                          {
                                                              service
                                                          };

                            ServiceBase.Run(servicesToRun);
                        }
                    }
                    finally
                    {
                        serviceRunning.ReleaseMutex();
                    }
                }
                else
                {
                    Console.WriteLine("MetQuest Service is already running");
                }

            }
            catch (Exception ex)
            {
                // The documentation indicates that the following exceptions can happen.
                // Although I'm not sure under what circumstance. 
                // In my testing I was only able to get the UnauthorizedAccessException exception.
                // UnauthorizedAccessException The named mutex exists and has access control security, but the user does not have MutexRights..::.FullControl.
                // IOException - A Win32 error occurred.
                // ApplicationException - The named mutex cannot be created, perhaps because a wait handle of a different type has the same name.
                // ArgumentException  - name is longer than 260 characters.

                string msg;
                Type exType = ex.GetType();
                if (exType == typeof(UnauthorizedAccessException) || exType == typeof(IOException) || exType == typeof(ApplicationException) || exType == typeof(ArgumentException))
                {
                    msg = ex.GetType().Name + "\n" + ex.Message +
                          "\nthe ThermoFisher.MetQuest.Service is already running.\nThis instance will end.";
                }
                else
                {
                    msg = ex.GetType().Name + "\n" + ex.Message +
                          "\nThe Service is shutting down.";
                }
                Console.WriteLine(msg);
            }
            finally
            {
                //MultiProcessFileLog.WriteLine("Main() - EXITING, ThermoFisher.MetQuest.WinService is FINISHED");
            }

        } 
    }
}
