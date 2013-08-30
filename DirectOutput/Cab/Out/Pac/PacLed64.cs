using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Serialization;
using DirectOutput.General;
using DirectOutput.General.Statistics;

namespace DirectOutput.Cab.Out.Pac
{
    /// <summary>
    /// IOutputController class for PacLed64 controllers.<br />
    /// Updates for the physical PacLed64 units are sent in a separate thread per unit to ensure max. performance.
    /// </summary>
    public class PacLed64 : OutputControllerBase, IOutputController
    {
        #region Id


        private object IdUpdateLocker = new object();
        private int _Id = -1;

        /// <summary>
        /// Gets or sets the Id of the PacLed64.<br />
        /// The Id of the PacLed64 must be unique and in the range of 1 to 4.<br />
        /// Setting changes the Name property, if it is blank or if the Name coresponds to PacLed64 {Id}.
        /// </summary>
        /// <value>
        /// The unique Id of the PacLed64 (Range 1-4).
        /// </value>
        /// <exception cref="System.Exception">
        /// PacLed64 Ids must be between 1-4. The supplied Id {0} is out of range.
        /// </exception>
        public int Id
        {
            get { return _Id; }
            set
            {
                if (!value.IsBetween(1, 4))
                {
                    throw new Exception("PacLed64 Ids must be between 1-4. The supplied Id {0} is out of range.".Build(value));
                }
                lock (IdUpdateLocker)
                {
                    if (_Id != value)
                    {

                        if (Name.IsNullOrWhiteSpace() || Name == "PacLed64 {0:0}".Build(_Id))
                        {
                            Name = "PacLed64 {0:0}".Build(value);
                        }

                        _Id = value;

                    }
                }
            }
        }

        #endregion



        #region IOutputcontroller implementation
        /// <summary>
        /// Signals the workerthread that all pending updates for the PacLed64 should be sent to the PacLed64.
        /// </summary>
        public override void Update()
        {
            PacLed64Units[Id].TriggerPacLed64UpdaterThread();
        }


        /// <summary>
        /// Initializes the PacLed64 object.<br />
        /// This method does also start the workerthread which does the actual update work when Update() is called.<br />
        /// This method should only be called once. Subsequent calls have no effect.
        /// </summary>
        /// <param name="Cabinet">The Cabinet object which is using the PacLed64 instance.</param>
        public override void Init(Cabinet Cabinet)
        {
            
            PacLed64Units[Id].Init(Cabinet);
            Log.Write("PacLed64 Id:{0} initialized and updater thread started.".Build(Id));

        }

        /// <summary>
        /// Finishes the PacLed64 object.<br/>
        /// Finish does also terminate the workerthread.
        /// </summary>
        public override void Finish()
        {
            PacLed64Units[Id].Finish();
            PacLed64Units[Id].ShutdownLighting();
            Log.Write("PacLed64 Id:{0} finished and updater thread stopped.".Build(Id));

        }
        #endregion



        #region Outputs
        private OutputList _Outputs = new OutputList();
        /// <summary>
        /// OutputList containing the OutputNumbered objects for a PacLed64.
        /// </summary>
        public override OutputList Outputs
        {
            get { return _Outputs; }
            set
            {
                if (_Outputs != null)
                {
                    _Outputs.OutputValueChanged -= new OutputList.OutputValueChangedEventHandler(Outputs_OutputValueChanged);
                }

                _Outputs = value;

                if (_Outputs != null)
                {
                    _Outputs.OutputValueChanged += new OutputList.OutputValueChangedEventHandler(Outputs_OutputValueChanged);

                }

            }
        }

        /// <summary>
        /// Adds the outputs for a PacLed64.<br/>
        /// A PacLed64 has 64 outputs numbered from 1 to 64. This method adds OutputNumbered objects for all outputs to the list.
        /// </summary>
        public void AddOutputs()
        {
            for (int i = 1; i <= 64; i++)
            {
                if (!Outputs.Any(x => ((OutputNumbered)x).Number == i))
                {
                    Outputs.Add(new OutputNumbered() { Name = "PacLed64Output {0:0}.{1:00}".Build(Id, i), Number = i });
                }
            }
        }





        /// <summary>
        /// Handles the OutputValueChanged event of the Outputs property.<br />
        /// Updates the internal arrays holding the states of the PacLed64 outputs.
        /// </summary>
        /// <param name="sender">The source of the event (must be a OutputNumbered).</param>
        /// <param name="e">The <see cref="OutputEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.Exception">
        /// The OutputValueChanged event handler for PacLed64 unit {0} (Id: {2:0}) has been called by a sender which is not a OutputNumbered.<br/>
        /// or<br/>
        /// PacLed64 output numbers must be in the range of 1-64. The supplied output number {0} is out of range.
        /// </exception>
        void Outputs_OutputValueChanged(object sender, OutputEventArgs e)
        {

            if (!(e.Output is OutputNumbered))
            {
                throw new Exception("The OutputValueChanged event handler for PacLed64 unit {0} (Id: {2:0}) has been called by a sender which is not a OutputNumbered.".Build(Name, Id));
            }
            OutputNumbered ON = (OutputNumbered)e.Output;

            if (!ON.Number.IsBetween(1, 64))
            {
                throw new Exception("PacLed64 output numbers must be in the range of 1-64. The supplied output number {0} is out of range.".Build(ON.Number));
            }

            PacLed64Unit S = PacLed64Units[this.Id];
            S.UpdateValue(ON);
        }
        #endregion









        //~PacLed64()
        //{
        //    Dispose(false);
        //}

        //#region Dispose



        //public void Dispose()
        //{

        //    Dispose(true);
        //    GC.SuppressFinalize(this); // remove this from gc finalizer list
        //}
        //protected virtual void Dispose(bool disposing)
        //{
        //    // Check to see if Dispose has already been called.
        //    if (!this.disposed)
        //    {
        //        // If disposing equals true, dispose all managed
        //        // and unmanaged resources.
        //        if (disposing)
        //        {
        //            // Dispose managed resources.

        //        }

        //        // Call the appropriate methods to clean up
        //        // unmanaged resources here.
        //        // If disposing is false,
        //        // only the following code is executed.

        //        TerminatePacLed64();


        //        // Note disposing has been done.
        //        disposed = true;

        //    }
        //}
        //private bool disposed = false;

        //#endregion



        #region Constructor


        /// <summary>
        /// Initializes the <see cref="PacLed64"/> class.
        /// </summary>
        static PacLed64()
        {
            PacLed64Units = new Dictionary<int, PacLed64Unit>();
            for (int i = 1; i <= 4; i++)
            {
                PacLed64Units.Add(i, new PacLed64Unit(i));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacLed64"/> class.
        /// </summary>
        public PacLed64()
        {
            Outputs = new OutputList();

        }



        /// <summary>
        /// Initializes a new instance of the <see cref="PacLed64"/> class.
        /// </summary>
        /// <param name="Number">The number of the PacLed64 (1-4).</param>
        public PacLed64(int Id)
            : this()
        {
            this.Id = Id;
        }

        #endregion






        #region Internal class for PacLed64 output states and update methods

        private static Dictionary<int, PacLed64Unit> PacLed64Units = new Dictionary<int, PacLed64Unit>();

        private class PacLed64Unit
        {
            private Pinball Pinball;

            private TimeSpanStatisticsItem UpdateTimeStatistics;
            private TimeSpanStatisticsItem PWMUpdateTimeStatistics;
            private TimeSpanStatisticsItem OnOffUpdateTimeStatistics;

            private const int MaxUpdateFailCount = 5;


            public int Id { get; private set; }

            private int Index { get; set; }

            private PacDriveSingleton PDSingleton;

            private byte[] NewValue = new byte[64];
            private byte[] CurrentValue = new byte[64];


            private byte[] LastValueSent = new byte[64];
            private bool[] LastStateSent = new bool[64];

            public bool UpdateRequired = true;

            public object PacLed64UpdateLocker = new object();
            public object ValueChangeLocker = new object();

            public Thread PacLed64Updater;
            public bool KeepPacLed64UpdaterAlive = false;
            public object PacLed64UpdaterThreadLocker = new object();


            public void Init(Cabinet Cabinet)
            {
                this.Pinball = Cabinet.Pinball;
                if (!Pinball.TimeSpanStatistics.Contains("PacLed64 {0:0} update calls".Build(Id)))
                {
                    UpdateTimeStatistics = new TimeSpanStatisticsItem() { Name = "PacLed64 {0:0} update calls".Build(Id), GroupName = "OutputControllers - PacLed64" };
                    Pinball.TimeSpanStatistics.Add(UpdateTimeStatistics);
                }
                else
                {
                    UpdateTimeStatistics = Pinball.TimeSpanStatistics["PacLed64 {0:0} update calls".Build(Id)];
                }
                if (!Pinball.TimeSpanStatistics.Contains("PacLed64 {0:0} PWM updates".Build(Id)))
                {
                    PWMUpdateTimeStatistics = new TimeSpanStatisticsItem() { Name = "PacLed64 {0:0} PWM updates".Build(Id), GroupName = "OutputControllers - PacLed64" };
                    Pinball.TimeSpanStatistics.Add(PWMUpdateTimeStatistics);
                }
                else
                {
                    PWMUpdateTimeStatistics = Pinball.TimeSpanStatistics["PacLed64 {0:0} PWM updates".Build(Id)];
                }
                if (!Pinball.TimeSpanStatistics.Contains("PacLed64 {0:0} OnOff updates".Build(Id)))
                {
                    OnOffUpdateTimeStatistics = new TimeSpanStatisticsItem() { Name = "PacLed64 {0:0} OnOff updates".Build(Id), GroupName = "OutputControllers - PacLed64" };
                    Pinball.TimeSpanStatistics.Add(OnOffUpdateTimeStatistics);
                }
                else
                {
                    OnOffUpdateTimeStatistics = Pinball.TimeSpanStatistics["PacLed64 {0:0} OnOff updates".Build(Id)];
                }
                StartPacLed64UpdaterThread();
            }

            public void Finish()
            {

                TerminatePacLed64UpdaterThread();
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
                lock (ValueChangeLocker)
                {
                    if (NewValue[ZeroBasedOutputNumber] != OutputNumbered.Value)
                    {
                        NewValue[ZeroBasedOutputNumber] = OutputNumbered.Value;
                        UpdateRequired = true;
                    }
                }

            }
            private void CopyNewToCurrent()
            {
                lock (ValueChangeLocker)
                {
                    Array.Copy(NewValue, CurrentValue, NewValue.Length);

                }
            }

            public bool IsUpdaterThreadAlive
            {
                get
                {
                    if (PacLed64Updater != null)
                    {
                        return PacLed64Updater.IsAlive;
                    }
                    return false;
                }
            }

            public void StartPacLed64UpdaterThread()
            {
                lock (PacLed64UpdaterThreadLocker)
                {
                    if (!IsUpdaterThreadAlive)
                    {
                        KeepPacLed64UpdaterAlive = true;
                        PacLed64Updater = new Thread(PacLed64UpdaterDoIt);
                        PacLed64Updater.Name = "PacLed64 {0:0} updater thread".Build(Id);
                        PacLed64Updater.Start();
                    }
                }
            }
            public void TerminatePacLed64UpdaterThread()
            {
                lock (PacLed64UpdaterThreadLocker)
                {
                    if (PacLed64Updater != null)
                    {
                        try
                        {
                            KeepPacLed64UpdaterAlive = false;
                            lock (PacLed64Updater)
                            {
                                Monitor.Pulse(PacLed64Updater);
                            }
                            if (!PacLed64Updater.Join(1000))
                            {
                                PacLed64Updater.Abort();
                            }

                        }
                        catch (Exception E)
                        {
                            Log.Exception("A error occurd during termination of {0}.".Build(PacLed64Updater.Name), E);
                            throw new Exception("A error occurd during termination of {0}.".Build(PacLed64Updater.Name), E);
                        }
                        PacLed64Updater = null;
                    }
                }
            }

            bool TriggerUpdate = false;
            public void TriggerPacLed64UpdaterThread()
            {
                TriggerUpdate = true;
                lock (PacLed64UpdaterThreadLocker)
                {
                    Monitor.Pulse(PacLed64UpdaterThreadLocker);
                }
            }


            //TODO: Check if thread should really terminate on failed updates
            private void PacLed64UpdaterDoIt()
            {
                Pinball.ThreadInfoList.HeartBeat("PacLed64 {0:0}".Build(Id));


                int FailCnt = 0;
                while (KeepPacLed64UpdaterAlive)
                {
                    try
                    {
                        if (IsPresent)
                        {
                            UpdateTimeStatistics.MeasurementStart();
                            SendPacLed64Update();
                            UpdateTimeStatistics.MeasurementStop();
                        }
                        FailCnt = 0;
                    }
                    catch (Exception E)
                    {
                        Log.Exception("A error occured when updating PacLed64 {0}".Build(Id), E);
                        Pinball.ThreadInfoList.RecordException(E);
                        FailCnt++;

                        if (FailCnt > MaxUpdateFailCount)
                        {
                            Log.Exception("More than {0} consecutive updates failed for PacLed64 {1}. Updater thread will terminate.".Build(MaxUpdateFailCount, Id));
                            KeepPacLed64UpdaterAlive = false;
                        }
                    }
                    Pinball.ThreadInfoList.HeartBeat();
                    if (KeepPacLed64UpdaterAlive)
                    {
                        lock (PacLed64UpdaterThreadLocker)
                        {
                            while (!TriggerUpdate && KeepPacLed64UpdaterAlive)
                            {
                                Monitor.Wait(PacLed64UpdaterThreadLocker, 50);  // Lock is released while we’re waiting
                                Pinball.ThreadInfoList.HeartBeat();
                            }

                        }

                    }
                    TriggerUpdate = false;
                }
                Pinball.ThreadInfoList.ThreadTerminates();
            }



            private bool ForceFullUpdate = true;
            private void SendPacLed64Update()
            {
                if (IsPresent)
                {

                    lock (PacLed64UpdateLocker)
                    {
                        lock (ValueChangeLocker)
                        {
                            if (!UpdateRequired && !ForceFullUpdate) return;

                            CopyNewToCurrent();

                            UpdateRequired = false;
                        }

                        byte IntensityUpdatesRequired = 0;
                        byte StateUpdatesRequired = 0;
                        if (!ForceFullUpdate)
                        {

                            for (int g = 0; g < 8; g++)
                            {

                                bool StateUpdateRequired = false;
                                for (int p = 0; p < 8; p++)
                                {
                                    int o = g << 3 | p;
                                    if (CurrentValue[o] > 0)
                                    {
                                        if (CurrentValue[o] != LastValueSent[o])
                                        {
                                            IntensityUpdatesRequired++;
                                        }
                                        else if (!LastStateSent[o])
                                        {
                                            StateUpdateRequired = true;
                                        }
                                    }
                                    else if (LastStateSent[o])
                                    {
                                        StateUpdateRequired = true;
                                    }

                                }
                                if (StateUpdateRequired) StateUpdatesRequired++;
                                StateUpdateRequired = false;
                            }
                        }
                        if (ForceFullUpdate || (IntensityUpdatesRequired + StateUpdatesRequired) > 30)
                        {
                            //more than 30 update calls required. Will send intensity updates for all outputs.
                            PDSingleton.PacLed64SetLEDIntensities(Index, CurrentValue);
                            Array.Copy(CurrentValue, LastValueSent, CurrentValue.Length);
                            for (int i = 0; i < 64; i++)
                            {
                                LastStateSent[i] = (LastValueSent[i] > 0);
                            }
                        }
                        else
                        {
                            //Will send separate intensity and state updates.
                            for (int g = 0; g < 8; g++)
                            {
                                int Mask = 0;
                                bool StateUpdateRequired = false;
                                for (int p = 0; p < 8; p++)
                                {
                                    int o = g << 3 | p;
                                    if (CurrentValue[o] > 0)
                                    {
                                        if (CurrentValue[o] != LastValueSent[o])
                                        {
                                            PDSingleton.PacLed64SetLEDIntensity(Index, o, CurrentValue[o]);
                                            LastStateSent[o] = true;
                                            LastValueSent[o] = CurrentValue[o];
                                        }
                                        else if (!LastStateSent[o])
                                        {
                                            Mask |= (1 << p);
                                            StateUpdateRequired = true;
                                            LastStateSent[o] = true;
                                        }
                                    }
                                    else if (LastStateSent[o])
                                    {
                                        StateUpdateRequired = true;
                                        LastStateSent[o] = false;
                                    }

                                }
                                if (StateUpdateRequired)
                                {
                                    PDSingleton.PacLed64SetLEDStates(Index, g + 1, (byte)Mask);
                                    StateUpdateRequired = false;
                                }

                            }
                        }


                    }
                    ForceFullUpdate = false;
                }
                else
                {
                    ForceFullUpdate=true;
                }
            }


            public void ShutdownLighting()
            {
                PDSingleton.PacLed64SetLEDStates(0, 0, 0);
                LastStateSent.Fill(false);
            }




            private bool IsPresent
            {
                get
                {
                    if (!Id.IsBetween(1, 4)) return false;
                    return Index >= 0;
                }
            }


            void Instance_OnPacRemoved(int Index)
            {
                this.Index = PDSingleton.PacLed64GetIndexForDeviceId(Id);
            }

            void Instance_OnPacAttached(int Index)
            {
                this.Index = PDSingleton.PacLed64GetIndexForDeviceId(Id);
                InitUnit();
                TriggerPacLed64UpdaterThread();
            }

            private void InitUnit()
            {
                LastValueSent.Fill((byte)0);
                PDSingleton.PacLed64SetLEDIntensities(Index, LastValueSent);
                LastStateSent.Fill(false);
                LastValueSent.Fill((byte)0);
            }


            public PacLed64Unit(int Id)
            {
                this.Id = Id;
                PDSingleton = PacDriveSingleton.Instance;
                PDSingleton.OnPacAttached += new PacDriveSingleton.PacAttachedDelegate(Instance_OnPacAttached);
                PDSingleton.OnPacRemoved += new PacDriveSingleton.PacRemovedDelegate(Instance_OnPacRemoved);
                this.Index = PacDriveSingleton.Instance.PacLed64GetIndexForDeviceId(Id);
                NewValue.Fill((byte)0);
                InitUnit();


            }


        }



        #endregion




    }
}
