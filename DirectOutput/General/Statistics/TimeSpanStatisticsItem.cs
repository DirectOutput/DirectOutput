using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;

namespace DirectOutput.General.Statistics
{
    public class TimeSpanStatisticsItem:NamedItemBase
    {
        private DateTime StartTime=DateTime.MinValue;

        public void MeasurementStart()
        {
            StartTime = DateTime.Now;
        }

        public void MeasurementStop()
        {
            if (StartTime != DateTime.MinValue)
            {
                AddDuration(DateTime.Now - StartTime);
                StartTime = DateTime.MinValue;
            }
        }

        private string _GroupName="<Not grouped>";

        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName= value; }
        }
        

        TimeSpan _TotalDuration = new TimeSpan(0);

        public TimeSpan TotalDuration
        {
            get { return _TotalDuration; }
        }

        public long ValuesCount { get; private set; }

        public TimeSpan AverageDuration
        {
            get
            {
                if (ValuesCount == 0) return new TimeSpan(0);

                return TimeSpan.FromTicks(_TotalDuration.Ticks / ValuesCount);
            }
        }


        List<TimeSpan> _MaxDurations = new List<TimeSpan>();

        public TimeSpan MaxDuration
        {
            get
            {
                return MaxDurations.Max();
            }
        }

        public List<TimeSpan> MaxDurations
        {
            get { return _MaxDurations; }
            private set { _MaxDurations = value; }
        }


        List<TimeSpan> _MinDurations = new List<TimeSpan>();

        public TimeSpan MinDuration
        {
            get
            {
                return MinDurations.Min();
            }
        }


        public List<TimeSpan> MinDurations
        {
            get { return _MinDurations; }
            private set { _MinDurations = value; }
        }



        private TimeSpan MaxBoundary;
        private TimeSpan MinBoundary;
        private bool MinMaxBufferFull = false;
        public void AddDuration(TimeSpan Duration)
        {
            _TotalDuration += Duration;
            ValuesCount++;
            if (!MinMaxBufferFull)
            {
                MaxDurations.Add(Duration);
                MinDurations.Add(Duration);
                if (MaxDurations.Count > 9)
                {
                    MaxBoundary = MaxDurations.Min();
                    MinBoundary = MinDurations.Max();
                    MinMaxBufferFull = true;
                }
                return;
            }


            if (Duration.CompareTo(MaxBoundary) > 0)
            {
                MaxDurations.Remove(MaxBoundary);
                MaxDurations.Add(Duration);
                MaxBoundary = MaxDurations.Min();
            }
            if (Duration.CompareTo(MinBoundary) < 0)
            {
                MinDurations.Remove(MinBoundary);
                MinDurations.Add(Duration);
                MinBoundary = MinDurations.Max();
            }



        }




        public override string ToString()
        {

            string S = "{0}, ".Build(Name);
            S+= "Values count: {0}".Build(ValuesCount);
            if (ValuesCount > 0)
            {
                S += ", Total: {0}, ".Build(TotalDuration.Format());
                S += "Avg.: {0}, ".Build(AverageDuration.Format());
                S += "Min.: {0}, ".Build(MinDuration.Format());
                S += "Max.: {0}".Build(MaxDuration.Format());


            }
            return S;
        }


    }
}
