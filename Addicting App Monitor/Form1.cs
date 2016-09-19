using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;

namespace Addicting_App_Monitor
{
    /*
     * Form1 handles most of the UI
     */
    public partial class Form1 : Form
    {
        private ProcessManager procManager;

        public Form1()
        {
            InitializeComponent();

            List<MonitoredApp> temp = new List<MonitoredApp>();

            procManager = new ProcessManager(temp);
            refreshListOfProcesses();
            var timer = new System.Threading.Timer(
                e => procManager.tick(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(5));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /*
         * Used when the list of processes wants to be refreshed
         */
        private void refreshListOfProcesses()
        {
            listBox1.Items.Clear();
            Process[] procs = Process.GetProcesses();
            foreach (Process proc in procs)
            {
                if (!listBox1.Items.Contains(proc.ProcessName) && !listBox2.Items.Contains(proc.ProcessName))
                {
                    listBox1.Items.Add(proc.ProcessName);
                }
            }
           
        }

        /*
         * Button used to start monitoring an application selected from the first list
         */
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                listBox2.Items.Add(listBox1.SelectedItem);

                procManager.monitor(new MonitoredApp(listBox1.SelectedItem.ToString(), MonitoredApp.DefaultMaxTime, TimeSpan.Zero, DateTime.Now.TimeOfDay));

                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }

        /*
         * Clicking the refresh button refreshes the list of processes
         */
        private void button2_Click(object sender, EventArgs e)
        {
            refreshListOfProcesses();
        }

        /*
         * Clicking the remove button stops a process from being monitored
         */
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                procManager.stopMonitoring(listBox2.SelectedItem.ToString());
                listBox2.Items.Remove(listBox2.SelectedItem);
                refreshListOfProcesses();
            }
        }

        /*
         * Selecting an item on the monitored apps list should change the statistics on the right side
         */
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                setStatusWindowByApp(procManager.getApp(listBox2.SelectedItem.ToString()));
            }
        }

        /*
         * When called, will update the statistics to this app
         */
        private void setStatusWindowByApp(MonitoredApp app)
        {
            if (app != null)
            {
                label3.Text = app.Name;
                label5.Text = app.TimeOpened.ToString();
                numericUpDown1.Value = app.MaxTime.Hours;
                numericUpDown2.Value = app.MaxTime.Minutes;
                numericUpDown3.Value = app.MaxTime.Seconds;
            }
        }

        /*
         * Change the max number of hours an app can stay on for
         */
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                procManager.getApp(listBox2.SelectedItem.ToString()).MaxTime = new TimeSpan(Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value), Convert.ToInt32(numericUpDown3.Value));
            }
        }

        /*
         * Change the max number of minutes an app can stay on for
         */
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                procManager.getApp(listBox2.SelectedItem.ToString()).MaxTime = new TimeSpan(Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value), Convert.ToInt32(numericUpDown3.Value));
            }

        }

        /*
         * Change the max number of seconds an app can stay on for
         */
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                procManager.getApp(listBox2.SelectedItem.ToString()).MaxTime = new TimeSpan(Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value), Convert.ToInt32(numericUpDown3.Value));
            }
        }
    }
}
