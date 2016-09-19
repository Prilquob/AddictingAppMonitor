using System;

namespace Addicting_App_Monitor
{
    /*
     * Class responsible for managing apps being observed and determining which apps are currently active
     */
    public class ProcessManager
    {
        private List<MonitoredApp> monitoredApps;
        private List<Process> processes;

        public ProcessManager(List<MonitoredApp> monitoredApps)
        {
            this.monitoredApps = monitoredApps;
            updateProcesses();
        }

        /*
         * Adds a MonitoredApp to the monitoredApps list
         */
        public void monitor(MonitoredApp app)
        {
            bool foundMatch = false;
            foreach (MonitoredApp a in monitoredApps)
            {
                if (app.Name.Equals(a.Name))
                {
                    foundMatch = true;
                }
            }
            if (!foundMatch)
            {
                monitoredApps.Add(app);
            }
        }

        /*
         * Removes an app from the monitoredApps list based on the name
         */
        public void stopMonitoring(string name)
        {
            foreach (MonitoredApp app in monitoredApps)
            {
                if (app.Name.Equals(name))
                {
                    monitoredApps.Remove(app);
                    break;
                }
            }
        }

        /*
         * Every time the app ticks, add the amount of time the app has been on for since the last tick was checked
         * and close any apps that have been on for more than the maximum time
         */
        public void tick()
        {
            foreach (MonitoredApp app in monitoredApps)
            {
                if (isAppOpened(app.Name))
                {
                    if (app.OpenedLastCheck)
                    {
                        app.addTime(DateTime.Now.TimeOfDay - app.LastTimeChecked);
                        if (app.openedTooLong())
                        {
                            closeApps(app.Name);
                        }
                    }
                    app.OpenedLastCheck = true;
                }
                else
                {
                    app.LastTimeChecked = DateTime.Now.TimeOfDay;
                    app.OpenedLastCheck = false;
                }
            }
        }

        /*
         * Checks if an app that has a certain name is opened
         */
        public bool isAppOpened(string name)
        {
            foreach (Process proc in processes)
            {
                if (proc.ProcessName.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        /*
         * Close any app that has the same process name
         */
        public void closeApps(string name)
        {
            updateProcesses();
            foreach (Process proc in processes)
            {
                if (proc.ProcessName.Equals(name))
                {
                    proc.CloseMainWindow();
                    proc.WaitForExit();
                }
            }
        }

        /*
         * Updates the list of processes currently running
         */
        public void updateProcesses()
        {
            processes = Process.GetProcesses().ToList<Process>();
        }

        /*
         * Returns the MonitoredApp with the same name if it is being monitored already
         */
        public MonitoredApp getApp(string name)
        {
            foreach (MonitoredApp app in monitoredApps)
            {
                if (app.Name.Equals(name))
                {
                    return app;
                }
            }
            return null;
        }
    }
}