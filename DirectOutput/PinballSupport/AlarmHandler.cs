using System;
using System.Collections.Generic;
using System.Linq;
using DirectOutput.General.Statistics;

namespace DirectOutput.PinballSupport
{
    /// <summary>
    /// The AlarmHandler classed is used to execute scheduled events (e.g. regular updates on a effect) in the framework.<br/>
    /// Two types of alarms/scheduled events exist:<br/>
    /// - Alarm which on executes once
    /// - IntervalAlarm which executes at specified intervals undtil the alarm is unregistered.
    /// </summary>
    public class AlarmHandler
    {
        private TimeSpanStatisticsItem AlarmStatistics;
        private TimeSpanStatisticsItem IntervalAlarmStatistics;

        /// <summary>
        /// Gets the time when the next alarm (interval or single) is scheduled.
        /// </summary>
        /// <returns>DateTime for the next alarm. If no alarms are scheduled the MaxValue for DateTime is returned.</returns>
        public DateTime GetNextAlarmTime()
        {
            DateTime IA = GetNextIntervalAlarm();
            DateTime A = GetNextAlarm();

            if (IA < A)
            {
                return IA;
            }
            return A;
        }

        /// <summary>
        /// Executes all Alarmes which have expired until the specified AlarmTime..
        /// </summary>
        /// <param name="AlarmTime">The alarm time.</param>
        /// <returns>true if alarms have been executed, fals if no alarms have been executed.</returns>
        public bool ExecuteAlarms(DateTime AlarmTime)
        {
            bool AlarmsExecuted = false;

            AlarmsExecuted |= Alarm(AlarmTime);
            AlarmsExecuted |= IntervalAlarm(AlarmTime);

            return AlarmsExecuted;
        }



        

        #region IntervalAlarm
        private object IntervalAlarmLocker = new object();
        private List<IntervalAlarmSetting> IntervalAlarmList = new List<IntervalAlarmSetting>();
        private class IntervalAlarmSetting
        {
            public int IntervalMs { get; set; }
            public DateTime NextAlarm { get; set; }
            public Action IntervalAlarmHandler { get; set; }
            public IntervalAlarmSetting() { }
            public IntervalAlarmSetting(int IntervalMs, Action IntervalAlarmHandler)
            {
                this.IntervalAlarmHandler = IntervalAlarmHandler;
                this.IntervalMs = IntervalMs;
                this.NextAlarm = DateTime.Now.AddMilliseconds(IntervalMs);
            }
        }


        private DateTime GetNextIntervalAlarm()
        {
            if (IntervalAlarmList.Count > 0)
            {
                return IntervalAlarmList.Min(x => x.NextAlarm);
            }
            return DateTime.MaxValue;
        }

        private bool IntervalAlarm(DateTime AlarmTime)
        {
            lock (IntervalAlarmLocker)
            {
                bool AlarmTriggered = false;
                IntervalAlarmList.Where(x => x.NextAlarm <= AlarmTime).ToList().ForEach(delegate(IntervalAlarmSetting S)
                {
                    try
                    {
                        IntervalAlarmStatistics.MeasurementStart();
                        S.IntervalAlarmHandler();
                        IntervalAlarmStatistics.MeasurementStop();
                        AlarmTriggered = true;
                    }
                    catch (Exception E) {
                        Log.Exception("A exception occured for IntervalAlarmHandler {0}. Interval alarm will be disabled for this handler.".Build(S.IntervalAlarmHandler.ToString()), E);
                        S.IntervalMs = int.MaxValue;
                    }
                    if (S.NextAlarm.AddMilliseconds(S.IntervalMs) <= AlarmTime)
                    {
                        S.NextAlarm = AlarmTime.AddMilliseconds(1);
                    }
                    else
                    {
                        S.NextAlarm = S.NextAlarm.AddMilliseconds(S.IntervalMs);
                    }
                });
                return AlarmTriggered;
            }
        }

        /// <summary>
        /// Registers the method specified in IntervalAlarmHandler for interval alarms.<br/>
        /// Interval alarms are fired repeatedly at the specifed interval. Please note that the interval is probably no absoletely precise.
        /// </summary>
        /// <param name="IntervalMs">The alarm interval in milliseconds.</param>
        /// <param name="IntervalAlarmHandler">The handler for the alarm (delegate of parameterless method).</param>
        public void RegisterIntervalAlarm(int IntervalMs, Action IntervalAlarmHandler)
        {
            lock (IntervalAlarmLocker)
            {
                UnregisterIntervalAlarm(IntervalAlarmHandler);
                IntervalAlarmList.Add(new IntervalAlarmSetting(IntervalMs, IntervalAlarmHandler));
            }
        }


        /// <summary>
        /// Unregisters the specified ItervalAlarmHandler.
        /// </summary>
        /// <param name="IntervalAlarmHandler">The interval alarm handler.</param>
        public void UnregisterIntervalAlarm(Action IntervalAlarmHandler)
        {
            lock (IntervalAlarmLocker)
            {
                IntervalAlarmList.RemoveAll(x => x.IntervalAlarmHandler == IntervalAlarmHandler);
            }
        }
        #endregion



        #region Alarm
        private object AlarmLocker = new object();
        private List<AlarmSetting> AlarmList = new List<AlarmSetting>();
        private class AlarmSetting
        {
            public DateTime AlarmTime { get; set; }
            public Action AlarmHandler { get; set; }
            public AlarmSetting() { }
            public AlarmSetting(DateTime AlarmTime, Action AlarmHandler)
            {
                this.AlarmTime = AlarmTime;
                this.AlarmHandler = AlarmHandler;
            }
        }

        private DateTime GetNextAlarm()
        {
            if (AlarmList.Count > 0)
            {
                return AlarmList.Min(x => x.AlarmTime);
            }
            return DateTime.MaxValue;
        }

        private bool Alarm(DateTime AlarmTime)
        {
            
            lock (AlarmLocker)
            {
                List<AlarmSetting> L = AlarmList.Where(x => x.AlarmTime <= AlarmTime).ToList();
                AlarmList.RemoveAll(x => x.AlarmTime <= AlarmTime);
                L.ForEach(delegate(AlarmSetting S) {
                    try
                    {
                        AlarmStatistics.MeasurementStart();
                        S.AlarmHandler();
                        AlarmStatistics.MeasurementStop();
                    }
                    catch (Exception E) {
                        Log.Exception("A exception occured for AlarmHandler {0}.".Build(S.AlarmHandler.ToString()), E);
                    }
                });
                return (L.Count > 0);
            }
        }

        /// <summary>
        /// Registers the specied AlarmHandler for a alarm after the specified duration.
        /// </summary>
        /// <param name="DurationMs">The duration until the alarm fires in milliseconds.</param>
        /// <param name="AlarmHandler">The alarm handler.</param>
        public void RegisterAlarm(int DurationMs, Action AlarmHandler)
        {
            RegisterAlarm(DateTime.Now.AddMilliseconds(DurationMs), AlarmHandler);
        }

        /// <summary>
        /// Registers the specified AlarmHandler for a alarm at the certain time.
        /// </summary>
        /// <param name="AlarmTime">The alarm time.</param>
        /// <param name="AlarmHandler">The alarm handler.</param>
        public void RegisterAlarm(DateTime AlarmTime, Action AlarmHandler)
        {
            lock (AlarmLocker)
            {
                UnregisterAlarm(AlarmHandler);
                AlarmList.Add(new AlarmSetting(AlarmTime, AlarmHandler));
            }
        }


        /// <summary>
        /// Unregisters the alarm for the specified alarm handler.
        /// </summary>
        /// <param name="AlarmHandler">The alarm handler.</param>
        public void UnregisterAlarm(Action AlarmHandler)
        {
            lock (AlarmLocker)
            {
                AlarmList.RemoveAll(x => x.AlarmHandler == AlarmHandler);
            }
        }
        #endregion


        /// <summary>
        /// Inits the object.<br/>
        /// </summary>
        public void Init(Pinball Pinball)
        {
            AlarmStatistics = new TimeSpanStatisticsItem() { Name = "Alarm calls", GroupName = "Pinball - Alarm calls" };
            Pinball.TimeSpanStatistics.Add(AlarmStatistics);
            IntervalAlarmStatistics = new TimeSpanStatisticsItem() { Name = "Interval Alarm calls", GroupName = "Pinball - Alarm calls" };
            Pinball.TimeSpanStatistics.Add(IntervalAlarmStatistics);
        }

        /// <summary>
        /// Finishes the object. Clears all alarm lists.
        /// </summary>
        public void Finish()
        {

            AlarmList.Clear();
            IntervalAlarmList.Clear();
        }


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AlarmHandler"/> class.
        /// </summary>
        public AlarmHandler()
        {

           
        }




        #endregion



    }
}
