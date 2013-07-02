using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;

namespace DirectOutput.General.Statistics
{
    public class TimeSpanStatisticsList:NamedItemList<TimeSpanStatisticsItem>
    {

        new public void Sort()
        {
            try
            {
                Sort((TSI1, TSI2) => (TSI1.GroupName == TSI2.GroupName ? TSI1.Name.CompareTo(TSI2.Name) : TSI1.GroupName.CompareTo(TSI2.GroupName)));
            }
            catch { }
        }

        public void AddDuration(string Name, TimeSpan Duration)
        {
            if (!Contains(Name))
            {
                this.Add(new TimeSpanStatisticsItem() {Name=Name});
            };
            this[Name].AddDuration(Duration);
        }
    }
}
