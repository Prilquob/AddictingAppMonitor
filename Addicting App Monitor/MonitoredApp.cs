using System;


namespace Addicting_App_Monitor
{
    /*
     * Represents an application being monitored, how long it has been opened for, and how long it is allowed
     * stay open for the day
     */
    public class MonitoredApp
    {
        private static TimeSpan resetTime = new TimeSpan();
        private static TimeSpan defaultMaxTime = new TimeSpan(1, 0, 0);

        public MonitoredApp(string name, TimeSpan maxTime, TimeSpan timeOpened, TimeSpan lastTimeChecked)
        {
            this.name = name;
            this.maxTime = maxTime;
            this.timeOpened = timeOpened;
            this.lastTimeChecked = lastTimeChecked;
            this.openedLastCheck = true;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public TimeSpan MaxTime
        {
            get { return maxTime; }
            set { maxTime = value; }
        }

        public TimeSpan TimeOpened
        {
            get { return timeOpened; }
            set { timeOpened = value; }
        }

        public TimeSpan LastTimeChecked
        {
            get { return lastTimeChecked; }
            set { lastTimeChecked = value; }
        }

        public bool OpenedLastCheck
        {
            get { return openedLastCheck; }
            set { openedLastCheck = value; }
        }

        public static TimeSpan DefaultMaxTime
        {
            get { return MonitoredApp.defaultMaxTime; }
            set { MonitoredApp.defaultMaxTime = value; }
        }

        /*
         * Add time to the monitored app depending on how long it has been since the last time it was checked
         */
        public void addTime(TimeSpan time)
        {
            if (DateTime.Now.TimeOfDay - lastTimeChecked < resetTime)
            {
                timeOpened = TimeSpan.Zero;
            }
            lastTimeChecked = DateTime.Now.TimeOfDay;
            timeOpened = timeOpened.Add(time);
        }

        /*
         * Returns whether the app has stayed open for too long or not
         */
        public bool openedTooLong()
        {
            return timeOpened - maxTime > TimeSpan.Zero;
        }

    }
}