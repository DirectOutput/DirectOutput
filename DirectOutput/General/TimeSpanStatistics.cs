using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.General
{
    public class TimeSpanStatistics : SimpleStatistics<TimeSpan>
    {

        TimeSpan _TotalDuration = new TimeSpan(0);

        public TimeSpan TotalDuration
        {
            get { return _TotalDuration; }
        }

        public TimeSpan AverageDuration
        {
            get
            {
                if (ValuesCount == 0) return new TimeSpan(0);

                return TimeSpan.FromTicks(_TotalDuration.Ticks / ValuesCount);
            }
        }


        public new void AddValue(TimeSpan Duration)
        {
            base.AddValue(Duration);
            _TotalDuration += Duration;
        }


        public override string ToString()
        {

            string S = "Values count: {0}".Build(ValuesCount);
            if (ValuesCount > 0)
            {
                S += ", Avg.: {0} s, ".Build(AverageDuration.ToString("s\\.FFFFFFF"));
                S += "Min.: {0} s, ".Build(MinValue.ToString("s\\.FFFFFFF"));
                S += "Max.: {0} s".Build(MaxValue.ToString("s\\.FFFFFFF"));


            }
            return S;
        }

    }
}
