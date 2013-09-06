using System;
using System.Collections.Generic;
using System.Linq;
using DirectOutput.General.Statistics;

namespace DirectOutput.PinballSupport
{
    /// <summary>
    /// The AlarmHandler classed is used to execute scheduled events (e.g. regular updates on a effect) in the framework.<br/>
    /// Two types of alarms/scheduled events exist:<br/>
    /// - Alarm which only executes once
    /// - IntervalAlarm which executes at specified intervals until the alarm is unregistered.
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
        /// <returns>true if alarms have been executed, false if no alarms have been executed.</returns>
        public bool ExecuteAlarms(DateTime AlarmTime)
        {
            bool AlarmsExecuted = false;

            AlarmsExecuted |= Alarm(AlarmTime);
            AlarmsExecuted |= IntervalAlarm(AlarmTime);

            return AlarmsExecuted;
        }





        #region IntervalAlarm
        private object IntervalAlarmLocker = new object();
        private List<IntervalAlarmSettingBase> IntervalAlarmList = new List<IntervalAlarmSettingBase>();
        private abstract class IntervalAlarmSettingBase
        {
            public int IntervalMs { get; set; }
            public DateTime NextAlarm { get; set; }
            public abstract void Execute();


        }

        private class IntervalAlarmSettingPara : IntervalAlarmSettingBase
        {

            public Action<object> IntervalAlarmHandler { get; set; }
            public object Para { get; set; }


            public override void Execute()
            {
                try
                {
                    IntervalAlarmHandler(Para);
                }
                catch (Exception E)
                {
                    throw new Exception("A exception occured in IntervalAlarm for AlarmHandler {0} with parameter {1}.".Build(IntervalAlarmHandler.ToString(), Para.ToString().Replace("\n", ",")), E);
                }
            }

            public IntervalAlarmSettingPara() { }
            public IntervalAlarmSettingPara(int IntervalMs, Action<object> IntervalAlarmHandler, object Para)
            {
                this.IntervalAlarmHandler = IntervalAlarmHandler;
                this.IntervalMs = IntervalMs;
                this.NextAlarm = DateTime.Now.AddMilliseconds(IntervalMs);
                this.Para = Para;
            }
        }

        private class IntervalAlarmSettingNoPara : IntervalAlarmSettingBase
        {

            public Action IntervalAlarmHandler { get; set; }


            public override void Execute()
            {
                try
                {
                    IntervalAlarmHandler();
                }
                catch (Exception E)
                {
                    throw new Exception("A exception occured in IntervalAlarm for AlarmHandler {0}.".Build(IntervalAlarmHandler.ToString()), E);
                }
            }

            public IntervalAlarmSettingNoPara() { }
            public IntervalAlarmSettingNoPara(int IntervalMs, Action IntervalAlarmHandler)
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
                IntervalAlarmList.Where(x => x.NextAlarm <= AlarmTime).ToList().ForEach(delegate(IntervalAlarmSettingBase S)
                {
                    IntervalAlarmStatistics.MeasurementStart();
                    try
                    {
                        S.Execute();
                    }
                    catch (Exception E)
                    {
                        Log.Exception("A exception occured when excuting the handler for a interval alarm. This interval alarm will be disabled.", E);
                    }
                    IntervalAlarmStatistics.MeasurementStop();
                    AlarmTriggered = true;

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
        /// Interval alarms are fired repeatedly at the specifed interval. Please note that the interval is likely not absoletely precise.
        /// </summary>
        /// <param name="IntervalMs">The alarm interval in milliseconds.</param>
        /// <param name="IntervalAlarmHandler">The handler for the alarm (delegate of parameterless method).</param>
        public void RegisterIntervalAlarm(int IntervalMs, Action IntervalAlarmHandler)
        {
            lock (IntervalAlarmLocker)
            {
                UnregisterIntervalAlarm(IntervalAlarmHandler);
                IntervalAlarmList.Add(new IntervalAlarmSettingNoPara(IntervalMs, IntervalAlarmHandler));
            }
        }


        /// <summary>
        /// Unregisters the specified IntervalAlarmHandler.
        /// </summary>
        /// <param name="IntervalAlarmHandler">The interval alarm handler.</param>
        public void UnregisterIntervalAlarm(Action IntervalAlarmHandler)
        {
            lock (IntervalAlarmLocker)
            {
                IntervalAlarmList.RemoveAll(x => x is IntervalAlarmSettingNoPara && ((IntervalAlarmSettingNoPara)x).IntervalAlarmHandler == IntervalAlarmHandler);
            }
        }


        /// <summary>
        /// Registers the method specified in IntervalAlarmHandler for interval alarms.<br />
        /// Interval alarms are fired repeatedly at the specifed interval. Please note that the interval is likely not absoletely precise.
        /// </summary>
        /// <param name="IntervalMs">The alarm interval in milliseconds.</param>
        /// <param name="IntervalAlarmHandler">The handler for the alarm (delegate of method with one parameter of type object).</param>
        /// <param name="Parameter">The parameter for the interval alarm.</param>
        public void RegisterIntervalAlarm(int IntervalMs, Action<object> IntervalAlarmHandler, object Parameter)
        {
            lock (IntervalAlarmLocker)
            {
                UnregisterIntervalAlarm(IntervalAlarmHandler);
                IntervalAlarmList.Add(new IntervalAlarmSettingPara(IntervalMs, IntervalAlarmHandler, Parameter));
            }
        }


        /// <summary>
        /// Unregisters the specified IntervalAlarmHandler.
        /// </summary>
        /// <param name="IntervalAlarmHandler">The interval alarm handler.</param>
        public void UnregisterIntervalAlarm(Action<object> IntervalAlarmHandler)
        {
            lock (IntervalAlarmLocker)
            {
                IntervalAlarmList.RemoveAll(x => x is IntervalAlarmSettingPara && ((IntervalAlarmSettingPara)x).IntervalAlarmHandler == IntervalAlarmHandler);
            }
        }

        #endregion



        #region Alarm
        private object AlarmLocker = new object();
        private List<AlarmSettingsBase> AlarmList = new List<AlarmSettingsBase>();

        private abstract class AlarmSettingsBase
        {
            public DateTime AlarmTime { get; set; }
            public abstract void Execute();
        }

        private class AlarmSettingNoPara : AlarmSettingsBase
        {

            public Action AlarmHandler { get; set; }


            public override void Execute()
            {
                try
                {
                    AlarmHandler();
                }
                catch (Exception E)
                {
                    Log.Exception("A exception occured for AlarmHandler {0}.".Build(AlarmHandler.ToString()), E);
                }
            }

            public AlarmSettingNoPara() { }
            public AlarmSettingNoPara(DateTime AlarmTime, Action AlarmHandler)
            {
                this.AlarmTime = AlarmTime;
                this.AlarmHandler = AlarmHandler;

            }
        }

        private class AlarmSettingPara : AlarmSettingsBase
        {

            public Action<object> AlarmHandler { get; set; }
            public object Para { get; set; }

            public override void Execute()
            {
                try
                {
                    AlarmHandler(Para);
                }
                catch (Exception E)
                {
                    Log.Exception("A exception occured for AlarmHandler {0} with parameter {1}.".Build(AlarmHandler.ToString(), Para.ToString().Replace("\n", ",")), E);
                }
            }

            public AlarmSettingPara() { }
            public AlarmSettingPara(DateTime AlarmTime, Action<object> AlarmHandler, object Para)
            {
                this.AlarmTime = AlarmTime;
                this.AlarmHandler = AlarmHandler;
                this.Para = Para;
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
                List<AlarmSettingsBase> L = AlarmList.Where(x => x.AlarmTime <= AlarmTime).ToList();
                AlarmList.RemoveAll(x => x.AlarmTime <= AlarmTime);
                L.ForEach(delegate(AlarmSettingsBase S)
                {
                    S.Execute();
                });
                return (L.Count > 0);
            }
        }





        /// <summary>
        /// Registers the specied AlarmHandler for a alarm after the specified duration.
        /// </summary>
        /// <param name="DurationMs">The duration until the alarm fires in milliseconds.</param>
        /// <param name="AlarmHandler">The alarm handler.</param>
        /// <param name="DontUnregister">If set to <c>true</c> previously registered alarms for the same handler are no unregistered before registering the handler.</param>
        public void RegisterAlarm(int DurationMs, Action AlarmHandler, bool DontUnregister = false)
        {
            RegisterAlarm(DateTime.Now.AddMilliseconds(DurationMs), AlarmHandler, DontUnregister);
        }

        /// <summary>
        /// Registers the specified AlarmHandler for a alarm at the certain time.
        /// </summary>
        /// <param name="AlarmTime">The alarm time.</param>
        /// <param name="AlarmHandler">The alarm handler.</param>
        public void RegisterAlarm(DateTime AlarmTime, Action AlarmHandler, bool DontUnregister = false)
        {
            lock (AlarmLocker)
            {
                if (!DontUnregister) UnregisterAlarm(AlarmHandler);
                AlarmList.Add(new AlarmSettingNoPara(AlarmTime, AlarmHandler));
            }
        }


        /// <summary>
        /// Unregisters all alarm for the specified alarm handler.
        /// </summary>
        /// <param name="AlarmHandler">The alarm handler.</param>
        public void UnregisterAlarm(Action AlarmHandler)
        {
            lock (AlarmLocker)
            {
                AlarmList.RemoveAll(x => x is AlarmSettingNoPara && ((AlarmSettingNoPara)x).AlarmHandler == AlarmHandler);
            }
        }


        /// <summary>
        /// Registers the specied AlarmHandler for a alarm after the specified duration.
        /// </summary>
        /// <param name="DurationMs">The duration until the alarm fires in milliseconds.</param>
        /// <param name="AlarmHandler">The alarm handler.</param>
        /// <param name="Parameter">The parameter value for the alarm.</param>
        /// <param name="DontUnregister">If set to <c>true</c> previously registered alarms for the same handler are no unregistered before registering the handler.</param>
        public void RegisterAlarm(int DurationMs, Action<object> AlarmHandler, object Parameter, bool DontUnregister = false)
        {
            RegisterAlarm(DateTime.Now.AddMilliseconds(DurationMs), AlarmHandler, Parameter, DontUnregister);
        }

        /// <summary>
        /// Registers the specified AlarmHandler for a alarm at the certain time.
        /// </summary>
        /// <param name="AlarmTime">The alarm time.</param>
        /// <param name="AlarmHandler">The alarm handler.</param>
        /// <param name="Parameter">The parameter value for the alarm.</param>
        /// <param name="DontUnregister">If set to <c>true</c> previously registered alarms for the same handler are no unregistered before registering the handler.</param>
        public void RegisterAlarm(DateTime AlarmTime, Action<object> AlarmHandler, object Parameter, bool DontUnregister = false)
        {
            lock (AlarmLocker)
            {
                if(!DontUnregister) UnregisterAlarm(AlarmHandler);
                AlarmList.Add(new AlarmSettingPara(AlarmTime, AlarmHandler, Parameter));
            }
        }


        /// <summary>
        /// Unregisters all alarms for the specified alarm handler.
        /// </summary>
        /// <param name="AlarmHandler">The alarm handler.</param>
        public void UnregisterAlarm(Action<object> AlarmHandler)
        {
            lock (AlarmLocker)
            {
                AlarmList.RemoveAll(x => x is AlarmSettingPara && ((AlarmSettingPara)x).AlarmHandler == AlarmHandler);
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
