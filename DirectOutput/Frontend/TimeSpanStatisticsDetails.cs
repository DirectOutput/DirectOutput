using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.General.Statistics;

namespace DirectOutput.Frontend
{
    public partial class TimeSpanStatisticsDetails : Form
    {
        private TimeSpanStatisticsItem TimeSpanStatisticsItem;

        public TimeSpanStatisticsDetails(TimeSpanStatisticsItem TimeSpanStatisticsItem)
        {
            InitializeComponent();
            this.TimeSpanStatisticsItem = TimeSpanStatisticsItem;
            ShowData();
        }

        public void ShowData()
        {
            DetailGroup.Text = TimeSpanStatisticsItem.GroupName;
            DetailName.Text = TimeSpanStatisticsItem.Name;
            DetailTotalDuration.Text = TimeSpanStatisticsItem.TotalDuration.Format();
            DetailValuesCount.Text = TimeSpanStatisticsItem.ValuesCount.ToString();
            DetailAvgDuration.Text = TimeSpanStatisticsItem.AverageDuration.Format();
            DetailMinDuration.Text = TimeSpanStatisticsItem.MinDuration.Format();
            DetailMaxDuration.Text = TimeSpanStatisticsItem.MaxDuration.Format();

            int RowIndex;
            MinDurationsGrid.Rows.Clear();
            TimeSpanStatisticsItem.MinDurations.Sort();
            foreach (TimeSpan TS in TimeSpanStatisticsItem.MinDurations)
            {
                RowIndex = MinDurationsGrid.Rows.Add();
                MinDurationsGrid[MinDurations.Name, RowIndex].Value = TS.Format();
            }
            MaxDurationsGrid.Rows.Clear();
            TimeSpanStatisticsItem.MaxDurations.Sort();
            foreach (TimeSpan TS in TimeSpanStatisticsItem.MaxDurations)
            {
                RowIndex = MaxDurationsGrid.Rows.Add();
                MaxDurationsGrid[MaxDurations.Name, RowIndex].Value = TS.Format();
            }
        }

    }
}
