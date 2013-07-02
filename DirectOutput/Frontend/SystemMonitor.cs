using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.General;

namespace DirectOutput.Frontend
{
    public partial class SystemMonitor : Form
    {
        private Pinball Pinball;
        public SystemMonitor(Pinball Pinball)
        {
            this.Pinball = Pinball;
            InitializeComponent();
            RefreshData();
        }



        public void RefreshData()
        {
            RefreshThreads();

        }


        private void RefreshThreads() {

            ThreadDisplay.Rows.Clear();

            foreach (ThreadInfo TI in Pinball.ThreadInfoList)
            {
                int RowIndex = ThreadDisplay.Rows.Add();
                ThreadDisplay["ThreadName", RowIndex].Value = TI.ThreadName;
                ThreadDisplay["ThreadHostName", RowIndex].Value = (!TI.HostName.IsNullOrEmpty() ? TI.HostName : "");
                ThreadDisplay["ThreadIsAlive", RowIndex].Value = TI.IsAlive;
                ThreadDisplay["ThreadLastHeartBeat", RowIndex].Value = TI.LastHeartBeat.ToString("HH:mm:ss");
                ThreadDisplay["ThreadExceptions", RowIndex].Value = TI.Exceptions.Count;
                ThreadDisplay["ThreadProcessorNumber", RowIndex].Value = TI.ProcessorNumber;
            }


            ThreadDisplay.ClearSelection();
            ThreadDisplay.Refresh();

        }



    }
}
