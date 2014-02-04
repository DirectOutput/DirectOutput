using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Serialization;
using DirectOutput.General;
using DirectOutput.General.Statistics;

namespace DirectOutput.Cab.Out.SainSmart
{
    /// <summary>
    /// The SainSmart is a output controller with 4 or more relay outputs
    /// 
    /// The framework supports auto detection and configuration of these units.
    /// 
    /// This unit is made and sold by <a target="_blank" href="http://www.sainsmart.com">Ultimarc</a>.
    /// 
    /// The implemention of the SainSmartIO driver uses a separate thread per connected unit to ensure max. performance.
    /// 
    /// \image html SainSmartIOLogo.png 
    /// </summary>
    public class SainSmartIO : OutputControllerBase, IOutputController
    {
        #region Id

        private object IdUpdateLocker = new object();
        private int _Id = -1;

        /// <summary>
        /// Gets or sets the Id of the SainSmartIO.<br />
        /// The Id of the SainSmartIO must be unique and in the range of 1 to 4.<br />
        /// Setting changes the Name property, if it is blank or if the Name coresponds to SainSmartIO {Id}.
        /// </summary>
        /// <value>
        /// The unique Id of the SainSmartIO (Range 1-4).
        /// </value>
        /// <exception cref="System.Exception">
        /// SainSmartIO Ids must be between 1-4. The supplied Id {0} is out of range.
        /// </exception>
        public int Id
        {
            get { return _Id; }
            set
            {
                if (!value.IsBetween(1, 4))
                {
                    throw new Exception("SainSmartIO Ids must be between 1-4. The supplied Id {0} is out of range.".Build(value));
                }
                lock (IdUpdateLocker)
                {
                    if (_Id != value)
                    {

                        if (Name.IsNullOrWhiteSpace() || Name == "SainSmartIO {0:0}".Build(_Id))
                        {
                            Name = "SainSmartIO {0:0}".Build(value);
                        }

                        _Id = value;

                    }
                }
            }
        }

        #endregion



        #region IOutputcontroller implementation
        /// <summary>
        /// Signals the workerthread that all pending updates for the SainSmartIO should be sent to the SainSmartIO.
        /// </summary>
        public override void Update()
        {
            SainSmartIOUnits[Id].TriggerSainSmartIOUpdaterThread();
        }


        /// <summary>
        /// Initializes the SainSmartIO object.<br />
        /// This method does also start the workerthread which does the actual update work when Update() is called.<br />
        /// This method should only be called once. Subsequent calls have no effect.
        /// </summary>
        /// <param name="Cabinet">The Cabinet object which is using the SainSmartIO instance.</param>
        public override void Init(Cabinet Cabinet)
        {
            AddOutputs();   
            SainSmartIOUnits[Id].Init(Cabinet);
            Log.Write("SainSmartIO Id:{0} initialized and updater thread started.".Build(Id));

        }

        /// <summary>
        /// Finishes the SainSmartIO object.<br/>
        /// Finish does also terminate the workerthread.
        /// </summary>
        public override void Finish()
        {
            SainSmartIOUnits[Id].Finish();
            SainSmartIOUnits[Id].ShutdownLighting();
            Log.Write("SainSmartIO Id:{0} finished and updater thread stopped.".Build(Id));

        }
        #endregion



        #region Outputs


        /// <summary>
        /// Adds the outputs for a SainSmartIO.<br/>
        /// A SainSmartIO has 4-8 outputs.  
        /// </summary>
        private void AddOutputs()
        {
            for (int i = 1; i <= SainSmartIOUnit.MaxPorts; i++)
            {
                if (!Outputs.Any(x => ((OutputNumbered)x).Number == i))
                {
                    Outputs.Add(new OutputNumbered() { Name = "{0}.{1:00}".Build(Name, i), Number = i });
                }
            }
        }




        /// <summary>
        /// This method is called whenever the value of a output in the Outputs property changes its value.<br />
        /// It updates the internal array holding the states of the SainSmartIO outputs.
        /// </summary>
        /// <param name="Output">The output.</param>
        /// <exception cref="System.Exception">
        /// The OutputValueChanged event handler for SainSmartIO unit {0} (Id: {2:0}) has been called by a sender which is not a OutputNumbered.<br/>
        /// or<br/>
        /// SainSmartIO output numbers must be in the range of 1-64. The supplied output number {0} is out of range.
        /// </exception>
        protected override void OnOutputValueChanged(IOutput Output)
        {

            if (!(Output is OutputNumbered))
            {
                throw new Exception("The OutputValueChanged event handler for SainSmartIO unit {0} (Id: {2:0}) has been called by a sender which is not a OutputNumbered.".Build(Name, Id));
            }
            OutputNumbered ON = (OutputNumbered)Output;

            if (!ON.Number.IsBetween(1, 8))
            {
                throw new Exception("SainSmartIO output numbers must be in the range of 1-64. The supplied output number {0} is out of range.".Build(ON.Number));
            }

            SainSmartIOUnit S = SainSmartIOUnits[this.Id];
            S.UpdateValue(ON);
        }



        #endregion



        #region Constructor


        /// <summary>
        /// Initializes the <see cref="SainSmartIO"/> class.
        /// </summary>
        static SainSmartIO()
        {
            SainSmartIOUnits = new Dictionary<int, SainSmartIOUnit>();
            for (int i = 1; i <= 4; i++)
            {
                SainSmartIOUnits.Add(i, new SainSmartIOUnit(i));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SainSmartIO"/> class.
        /// </summary>
        public SainSmartIO()
        {
            Outputs = new OutputList();

        }



        /// <summary>
        /// Initializes a new instance of the <see cref="SainSmartIO"/> class.
        /// </summary>
        /// <param name="Id">The number of the SainSmartIO (1-4).</param>
        public SainSmartIO(int Id)
            : this()
        {
            this.Id = Id;
        }

        #endregion






        #region Internal class for SainSmartIO output states and update methods

        private static Dictionary<int, SainSmartIOUnit> SainSmartIOUnits = new Dictionary<int, SainSmartIOUnit>();

        private class SainSmartIOUnit
        {
            private Pinball Pinball;

            private TimeSpanStatisticsItem UpdateTimeStatistics;
            private TimeSpanStatisticsItem PWMUpdateTimeStatistics;
            private TimeSpanStatisticsItem OnOffUpdateTimeStatistics;

            private const int MaxUpdateFailCount = 5;
            public const int MaxPorts = 8;


            public int Id { get; private set; }

            private int Index { get; set; }

            private relayBoard PDSingleton;

            private byte[] NewValue = new byte[MaxPorts];
            private byte[] LastValueSent = new byte[MaxPorts];
 
            public object SainSmartIOUpdateLocker = new object();
            public object NewValueLocker = new object();

            public Thread SainSmartIOUpdater;
            public bool KeepSainSmartIOUpdaterAlive = false;
            public object SainSmartIOUpdaterThreadLocker = new object();


            public void Init(Cabinet Cabinet)
            {
                this.Pinball = Cabinet.Pinball;
                if (!Pinball.TimeSpanStatistics.Contains("SainSmartIO {0:0} update calls".Build(Id)))
                {
                    UpdateTimeStatistics = new TimeSpanStatisticsItem() { Name = "SainSmartIO {0:0} update calls".Build(Id), GroupName = "OutputControllers - SainSmartIO" };
                    Pinball.TimeSpanStatistics.Add(UpdateTimeStatistics);
                }
                else
                {
                    UpdateTimeStatistics = Pinball.TimeSpanStatistics["SainSmartIO {0:0} update calls".Build(Id)];
                }
                if (!Pinball.TimeSpanStatistics.Contains("SainSmartIO {0:0} PWM updates".Build(Id)))
                {
                    PWMUpdateTimeStatistics = new TimeSpanStatisticsItem() { Name = "SainSmartIO {0:0} PWM updates".Build(Id), GroupName = "OutputControllers - SainSmartIO" };
                    Pinball.TimeSpanStatistics.Add(PWMUpdateTimeStatistics);
                }
                else
                {
                    PWMUpdateTimeStatistics = Pinball.TimeSpanStatistics["SainSmartIO {0:0} PWM updates".Build(Id)];
                }
                if (!Pinball.TimeSpanStatistics.Contains("SainSmartIO {0:0} OnOff updates".Build(Id)))
                {
                    OnOffUpdateTimeStatistics = new TimeSpanStatisticsItem() { Name = "SainSmartIO {0:0} OnOff updates".Build(Id), GroupName = "OutputControllers - SainSmartIO" };
                    Pinball.TimeSpanStatistics.Add(OnOffUpdateTimeStatistics);
                }
                else
                {
                    OnOffUpdateTimeStatistics = Pinball.TimeSpanStatistics["SainSmartIO {0:0} OnOff updates".Build(Id)];
                }
                StartSainSmartIOUpdaterThread();
            }

            public void Finish()
            {

                TerminateSainSmartIOUpdaterThread();
                ShutdownLighting();
                this.Pinball = null;
                UpdateTimeStatistics = null;
                PWMUpdateTimeStatistics = null;
            }

            public void UpdateValue(OutputNumbered OutputNumbered)
            {
                //Skip update on output numbers which are out of range
                if (!OutputNumbered.Number.IsBetween(1, 64)) return;

                int ZeroBasedOutputNumber = OutputNumbered.Number - 1;
                //PDSingleton.RelaySwitch((Relaynum)ZeroBasedOutputNumber, OutputNumbered.Value > 0 ? Relaystate.ON : Relaystate.OFF);

                lock (NewValueLocker)
                {
                    if (NewValue[ZeroBasedOutputNumber] != OutputNumbered.Value)
                    {
                        NewValue[ZeroBasedOutputNumber] = OutputNumbered.Value;
                    }
                }
            }

            public bool IsUpdaterThreadAlive
            {
                get
                {
                    if (SainSmartIOUpdater != null)
                    {
                        return SainSmartIOUpdater.IsAlive;
                    }
                    return false;
                }
            }

            public void StartSainSmartIOUpdaterThread()
            {
                lock (SainSmartIOUpdaterThreadLocker)
                {
                    if (!IsUpdaterThreadAlive)
                    {
                        KeepSainSmartIOUpdaterAlive = true;
                        SainSmartIOUpdater = new Thread(SainSmartIOUpdaterDoIt);
                        SainSmartIOUpdater.Name = "SainSmartIO {0:0} updater thread".Build(Id);
                        SainSmartIOUpdater.Start();
                    }
                }
            }

            public void TerminateSainSmartIOUpdaterThread()
            {
                lock (SainSmartIOUpdaterThreadLocker)
                {
                    if (SainSmartIOUpdater != null)
                    {
                        try
                        {
                            KeepSainSmartIOUpdaterAlive = false;
                            lock (SainSmartIOUpdater)
                            {
                                Monitor.Pulse(SainSmartIOUpdater);
                            }
                            if (!SainSmartIOUpdater.Join(1000))
                            {
                                SainSmartIOUpdater.Abort();
                            }
                        }
                        catch (Exception E)
                        {
                            Log.Exception("A error occurd during termination of {0}.".Build(SainSmartIOUpdater.Name), E);
                            throw new Exception("A error occurd during termination of {0}.".Build(SainSmartIOUpdater.Name), E);
                        }
                        SainSmartIOUpdater = null;
                    }
                }
            }

            bool TriggerUpdate = false;
            public void TriggerSainSmartIOUpdaterThread()
            {
                TriggerUpdate = true;
                lock (SainSmartIOUpdaterThreadLocker)
                {
                    Monitor.Pulse(SainSmartIOUpdaterThreadLocker);
                }
            }


            //TODO: Check if thread should really terminate on failed updates
            private void SainSmartIOUpdaterDoIt()
            {
                Pinball.ThreadInfoList.HeartBeat("SainSmartIO {0:0}".Build(Id));

                int FailCnt = 0;
                while (KeepSainSmartIOUpdaterAlive)
                {
                    try
                    {
                        if (IsPresent)
                        {
                            UpdateTimeStatistics.MeasurementStart();
                            SendSainSmartIOUpdate();
                            UpdateTimeStatistics.MeasurementStop();
                        }
                        FailCnt = 0;
                    }
                    catch (Exception E)
                    {
                        Log.Exception("A error occured when updating SainSmartIO {0}".Build(Id), E);
                        Pinball.ThreadInfoList.RecordException(E);
                        FailCnt++;

                        if (FailCnt > MaxUpdateFailCount)
                        {
                            Log.Exception("More than {0} consecutive updates failed for SainSmartIO {1}. Updater thread will terminate.".Build(MaxUpdateFailCount, Id));
                            KeepSainSmartIOUpdaterAlive = false;
                        }
                    }
                    Pinball.ThreadInfoList.HeartBeat();
                    if (KeepSainSmartIOUpdaterAlive)
                    {
                        lock (SainSmartIOUpdaterThreadLocker)
                        {
                            while (!TriggerUpdate && KeepSainSmartIOUpdaterAlive)
                            {
                                Monitor.Wait(SainSmartIOUpdaterThreadLocker, 1000);  // Lock is released while we’re waiting
                                Pinball.ThreadInfoList.HeartBeat();
                            }

                        }

                    }
                    TriggerUpdate = false;
                }
                Pinball.ThreadInfoList.ThreadTerminates();
            }



            private void SendSainSmartIOUpdate()
            {
                if (IsPresent)
                {
                    byte outputValue = 0x0;
                    bool changed = false;

                    for (int o = 0; o < LastValueSent.Length; o++)
                    {
                        byte newValue;
                        lock (NewValueLocker)
                        {
                            newValue = NewValue[o];
                        }
                        if (newValue > 0)
                            outputValue |= (byte)(1 << o);

                        if (newValue != LastValueSent[o])
                        {
                            //Log.Write(String.Format("SainSmartIO updating {0} to {1}.", o, newValue));

                            //PDSingleton.RelaySwitch((Relaynum)o, newValue > 0 ? Relaystate.ON : Relaystate.OFF);
                            changed=true;
                            LastValueSent[o] = newValue;
                        }
                        
                    }
                    if (changed)
                        PDSingleton.RelayAll(outputValue);

                }
            }


            public void ShutdownLighting()
            {
                for (int i = 0; i < MaxPorts; i++)
                {
                    PDSingleton.RelaySwitch((Relaynum)i, Relaystate.OFF);
                }              
            }

            private bool IsPresent
            {
                get
                {
                    if (!Id.IsBetween(1, 4)) return false;
                    return Index >= 0;
                }
            }
        
            private void InitUnit()
            {
                LastValueSent.Fill((byte)0);
                LastValueSent.Fill((byte)0);
            }


            public SainSmartIOUnit(int Id)
            {
                this.Id = Id;
                PDSingleton = relayBoard.Instance;
             
                NewValue.Fill((byte)0);
                InitUnit();
            }


        }



        #endregion




    }
}
