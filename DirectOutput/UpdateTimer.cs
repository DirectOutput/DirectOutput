using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace DirectOutput
{
    public class UpdateTimer
    {
        private Timer Timer = new Timer();
        private DateTime TimerStart =DateTime.MaxValue;
        /// <summary>
        /// Gets the timestamp for the next update.
        /// </summary>
        /// <value>
        /// The timestamp for next update.
        /// </value>
        public DateTime NextUpdate
        {
            get
            {
                if (TimerStart == DateTime.MaxValue)
                {
                    return DateTime.MaxValue;
                }
                else
                {
                    return TimerStart.AddMilliseconds(Timer.Interval);
                }
            }
        }

        private int _IntervalMs=20;

        /// <summary>
        /// Gets or sets the update interval in milliseconds.
        /// </summary>
        /// <value>
        /// The update interval in milliseconds.
        /// </value>
        public int IntervalMs
        {
            get { return _IntervalMs; }
            set { _IntervalMs = value; }
        }

        private bool TimerRestart = false;
        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool AlarmTriggered = false;
            DateTime TimerElapsedTime = DateTime.Now;
            AlarmTriggered|= Alarm(TimerElapsedTime);
            AlarmTriggered|=IntervalAlarm(TimerElapsedTime);
            if (AlarmTriggered)
            {
                OnAlarmsTriggered();
            }

            if (TimerRestart)
            {
                Timer.Interval = (IntervalMs - (DateTime.Now - TimerElapsedTime).Milliseconds).Limit(1, 10000);
                TimerStart = DateTime.Now;
                Timer.Start();
            }
            else
            {
                TimerStart = DateTime.MaxValue;
            }
        }

        /// <summary>
        /// Occurs when after alarms have been tiggered.
        /// </summary>
        public event EventHandler<EventArgs> AlarmsTriggered;

        private void OnAlarmsTriggered()
        {
            if (AlarmsTriggered != null)
            {
                AlarmsTriggered(this, new EventArgs());
            }
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


        private bool IntervalAlarm(DateTime AlarmTime)
        {
            lock (IntervalAlarmLocker)
            {
                bool AlarmTriggered = false;
                IntervalAlarmList.Where(x => x.NextAlarm <= AlarmTime).ToList().ForEach(delegate(IntervalAlarmSetting S)
                {
                    try
                    {
                        S.IntervalAlarmHandler();
                        AlarmTriggered = true;
                    }
                    catch (Exception E) {
                        Log.Exception("A exception occured for IntervalAlarmHandler {0}. Interval alarm will be disable for this handler.".Build(S.IntervalAlarmHandler.ToString()), E);
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

        private bool Alarm(DateTime AlarmTime)
        {
            lock (AlarmLocker)
            {
                List<AlarmSetting> L = AlarmList.Where(x => x.AlarmTime <= AlarmTime).ToList();
                AlarmList.RemoveAll(x => x.AlarmTime <= AlarmTime);
                L.ForEach(delegate(AlarmSetting S) {
                    try
                    {
                        S.AlarmHandler();
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
        /// Inits the update timer and starts the timer.
        /// </summary>
        public void Init()
        {
            TimerRestart = true;
            TimerStart = DateTime.Now;
            Timer.Interval = IntervalMs;
            Timer.Start();
        }

        /// <summary>
        /// Finishes the update timer.
        /// </summary>
        public void Finish()
        {
            TimerRestart = false;
            Timer.Stop();
            TimerStart = DateTime.MaxValue;
            AlarmList.Clear();
            IntervalAlarmList.Clear();
        }


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTimer"/> class.
        /// </summary>
        public UpdateTimer()
        {
            Timer.Interval = IntervalMs;
            Timer.AutoReset = false;
            Timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTimer"/> class and sets the intervall to the specified IntervalMs.
        /// </summary>
        /// <param name="IntervalMs">The interval in milliseconds.</param>
        public UpdateTimer(int IntervalMs)
        {
            this.IntervalMs = IntervalMs;
            Timer.Interval = IntervalMs;
            Timer.AutoReset = false;
            Timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);

        }


        #endregion



    }
}
