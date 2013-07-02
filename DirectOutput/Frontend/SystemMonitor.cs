using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.General;
using DirectOutput.General.Statistics;

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
            RefreshDurationStatistics();
        }

        private void RefreshDurationStatistics()
        {
            Pinball.TimeSpanStatistics.Sort();

            DurationStatistics.Rows.Clear();
            foreach (TimeSpanStatisticsItem Item in Pinball.TimeSpanStatistics)
            {
                int RowIndex = DurationStatistics.Rows.Add();
                DurationStatistics[StatGroup.Name, RowIndex].Value = (Item.GroupName.IsNullOrWhiteSpace()?"":Item.GroupName);
                DurationStatistics[StatName.Name, RowIndex].Value = Item.Name;
                DurationStatistics[StatCallsCount.Name, RowIndex].Value = "{0} calls".Build(Item.ValuesCount);
                if (Item.ValuesCount > 0)
                {
                    DurationStatistics[StatTotalDuration.Name, RowIndex].Value = "{0} s, ".Build(Item.TotalDuration.ToString("s\\.FFFFFFF"));
                    DurationStatistics[StatAvgDuration.Name, RowIndex].Value = "{0} s, ".Build(Item.AverageDuration.ToString("s\\.FFFFFFF"));
                    DurationStatistics[StatMinDuration.Name, RowIndex].Value = "{0} s, ".Build(Item.MinDuration.ToString("s\\.FFFFFFF"));
                    DurationStatistics[StatMaxDuration.Name, RowIndex].Value = "{0} s, ".Build(Item.MaxDuration.ToString("s\\.FFFFFFF"));
                }
            }
        }


        private void RefreshThreads() {

            ThreadDisplay.Rows.Clear();

            foreach (ThreadInfo TI in Pinball.ThreadInfoList)
            {
                int RowIndex = ThreadDisplay.Rows.Add();
                ThreadDisplay[ThreadName.Name, RowIndex].Value = TI.ThreadName;
                ThreadDisplay[ThreadHostObject.Name, RowIndex].Value = (!TI.HostName.IsNullOrEmpty() ? TI.HostName : "");
                ThreadDisplay[ThreadIsAlive.Name, RowIndex].Value = TI.IsAlive;
                ThreadDisplay[ThreadLastHeartBeat.Name, RowIndex].Value = TI.LastHeartBeat.ToString("HH:mm:ss");
                int Ms=(int)(DateTime.Now-TI.LastHeartBeat).TotalMilliseconds;
                if(Ms>TI.HeartBeatTimeOutMs) {
                    ThreadDisplay[ThreadLastHeartBeat.Name, RowIndex].Style.BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                }
                else if (Ms > TI.HeartBeatTimeOutMs / 2)
                {
                    int Red = ((65536 / (TI.HeartBeatTimeOutMs / 2) * (Ms - (TI.HeartBeatTimeOutMs / 2))) / 256).Limit(0, 255);
                    int Green = 255 - Red; 
                    ThreadDisplay[ThreadLastHeartBeat.Name, RowIndex].Style.BackColor = System.Drawing.Color.FromArgb(Red, Green, 0);
                }
                else
                {
                    ThreadDisplay[ThreadLastHeartBeat.Name, RowIndex].Style.BackColor = System.Drawing.Color.FromArgb(0, 255, 0);
                }
                ThreadDisplay[ThreadExceptions.Name, RowIndex].Value = TI.Exceptions.Count;
                ThreadDisplay[ThreadProcessorNumber.Name, RowIndex].Value = TI.ProcessorNumber;
            }


            ThreadDisplay.ClearSelection();
            ThreadDisplay.Refresh();

        }



    }
}
