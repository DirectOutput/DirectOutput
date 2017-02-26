using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Serialization;
using DirectOutput.General;
using System.Globalization;
using DirectOutput.Cab.Schedules;

namespace DirectOutput.Cab.Out.Pac
{
    /// <summary>
    /// The Ultimate I/O is a output controller with 96 outputs all supporting 256 <a target="_blank" href="https://en.wikipedia.org/wiki/Pulse-width_modulation">pwm</a> levels with a PWM frequency of 100khz. Since the outputs of the unit are constant current drivers providing 20ma each, leds can be connected directly to the outputs (no resistor needed), but booster circuits must be used to driver higher loads (e.g. Cree leds). Up to 2 Ultimate I/O (currently) can technically be used with the DirectOutput framework as supported by the PacDrive SDK (ask Ultimarc for firmware to program second ID), but this has not been tested nor confirmed.
    /// 
    /// The framework supports auto detection and configuration of these units. If auto config is used, two LedWizEquivalent toys are added for each connected Ultimate I/O. The numbers of the LedWizEquivalents are based on the Id of the PacUIO.
    /// 
    /// This unit is made and sold by <a target="_blank" href="http://www.ultimarc.com">Ultimarc</a>.
    /// 
    /// The class was based on PacLed64.cs
    /// 
    /// \image html PacUIOLogo.png 
    /// </summary>
    public class PacUIO : OutputControllerBase, IOutputController
    {
        #region Id


        private object IdUpdateLocker = new object();
        private int _Id = -1;

        /// <summary>
        /// Gets or sets the Id of the Ultimate I/O.<br />
        /// The Id of the device must be unique and in the range of 0 to 1.<br />
        /// Please note: while PacLed64 is defined as 1-4, the UIO starts at 0.
        /// Setting changes the Name property, if it is blank or if the Name coresponds to PacUIO {Id}.
        /// </summary>
        /// <value>
        /// The unique Id of the PacUIO (Range 0-1).
        /// </value>
        /// <exception cref="System.Exception">
        /// PacUIO Ids must be between 0-1. The supplied Id {0} is out of range.
        /// </exception>
        public int Id
        {
            get { return _Id; }
            set
            {
                if (!value.IsBetween(0, 1))
                {
                    throw new Exception("PacUIO Ids must be between 0-1. The supplied Id {0} is out of range.".Build(value));
                }
                lock (IdUpdateLocker)
                {
                    if (_Id != value)
                    {

                        if (Name.IsNullOrWhiteSpace() || Name == "PacUIO {0:0}".Build(_Id))
                        {
                            Name = "PacUIO {0:0}".Build(value);
                        }

                        _Id = value;

                    }
                }
            }
        }

        #endregion



        #region IOutputcontroller implementation
        /// <summary>
        /// Signals the workerthread that all pending updates for the PacUIO should be sent to the PacUIO.
        /// </summary>
        public override void Update()
        {
            //Log.Write("PacUIO.Update");
            PacUIOUnits[Id].TriggerPacUIOUpdaterThread();
        }


        /// <summary>
        /// Initializes the PacUIO object.<br />
        /// This method does also start the workerthread which does the actual update work when Update() is called.<br />
        /// This method should only be called once. Subsequent calls have no effect.
        /// </summary>
        /// <param name="Cabinet">The cabinet object which is using the output controller instance.</param>
        public override void Init(Cabinet Cabinet)
        {
            AddOutputs();
            PacUIOUnits[Id].Init(Cabinet);
            Log.Write("PacUIO Id:{0} initialized and updater thread started.".Build(Id));
        }

        /// <summary>
        /// Finishes the PacUIO object.<br/>
        /// Finish does also terminate the workerthread.
        /// </summary>
        public override void Finish()
        {
            PacUIOUnits[Id].Finish();
            PacUIOUnits[Id].ShutdownLighting();
            Log.Write("PacUIO Id:{0} finished and updater thread stopped.".Build(Id));

        }
        #endregion



        #region Outputs


        /// <summary>
        /// Adds the outputs for a PacUIO.<br/>
        /// A PacUIO has 96 outputs numbered from 1 to 96. This method adds OutputNumbered objects for all outputs to the list.
        /// </summary>
        private void AddOutputs()
        {
            for (int i = 1; i <= 96; i++)
            {
                if (!Outputs.Any(x => (x).Number == i))
                {
                    Outputs.Add(new Output() { Name = "{0}.{1:00}".Build(Name, i), Number = i });
                }
            }
        }




        /// <summary>
        /// This method is called whenever the value of a output in the Outputs property changes its value.<br />
        /// It updates the internal array holding the states of the PacUIO outputs.
        /// </summary>
        /// <param name="Output">The output.</param>
        /// <exception cref="System.Exception">
        /// The OutputValueChanged event handler for PacUIO unit {0} (Id: {2:0}) has been called by a sender which is not a OutputNumbered.<br/>
        /// or<br/>
        /// PacUIO output numbers must be in the range of 1-96. The supplied output number {0} is out of range.
        /// </exception>
        protected override void OnOutputValueChanged(IOutput Output)
        {
            IOutput ON = Output;

            if (!ON.Number.IsBetween(1, 96)) {
                throw new Exception("PacUIO output numbers must be in the range of 1-96. The supplied output number {0} is out of range.".Build(ON.Number));
            }

            PacUIOUnit S = PacUIOUnits[this.Id];
            S.UpdateValue(ScheduledSettings.Instance.getnewrecalculatedOutput (ON, 27, Id));
        }

        #endregion
        



        
        #region Constructor


        /// <summary>
        /// Initializes the <see cref="PacUIO"/> class.
        /// </summary>
        static PacUIO()
        {

            PacUIOUnits = new Dictionary<int, PacUIOUnit>();
            for (int i = 0; i <= 2; i++) {
                    PacUIOUnits.Add(i, new PacUIOUnit(i));
            }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacUIO"/> class.
        /// </summary>
        public PacUIO()
        {

            Outputs = new OutputList();

        }



        /// <summary>
        /// Initializes a new instance of the <see cref="PacUIO"/> class.
        /// </summary>
        /// <param name="Id">The number of the PacUIO (0-1).</param>
        public PacUIO(int Id)
            : this()
        {
            this.Id = Id;
        }

        #endregion






        #region Internal class for PacUIO output states and update methods

        private static Dictionary<int, PacUIOUnit> PacUIOUnits = new Dictionary<int, PacUIOUnit>();

        private class PacUIOUnit
        {
            //private Pinball Pinball;


            private const int MaxUpdateFailCount = 5;


            public int Id { get; private set; }

            private int Index { get; set; }

            private PacDriveSingleton PDSingleton;

            //array, one per output channel
            private byte[] NewValue = new byte[96];
            private byte[] CurrentValue = new byte[96];


            private byte[] LastValueSent = new byte[96];
            private bool[] LastStateSent = new bool[96];

            public bool UpdateRequired = true;

            public object PacUIOUpdateLocker = new object();
            public object ValueChangeLocker = new object();

            public Thread PacUIOUpdater;
            public bool KeepPacUIOUpdaterAlive = false;
            public object PacUIOUpdaterThreadLocker = new object();


            public void Init(Cabinet Cabinet)
            {
                StartPacUIOUpdaterThread();
            }

            public void Finish()
            {

                TerminatePacUIOUpdaterThread();
                ShutdownLighting();

            }

            public void UpdateValue(IOutput Output)
            {
                //Skip update on output numbers which are out of range
                if (!Output.Number.IsBetween(1, 96)) return;

                int ZeroBasedOutputNumber = Output.Number - 1;
                lock (ValueChangeLocker)
                {
                    if (NewValue[ZeroBasedOutputNumber] != Output.Value)
                    {
                        NewValue[ZeroBasedOutputNumber] = Output.Value;
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
                    if (PacUIOUpdater != null)
                    {
                        return PacUIOUpdater.IsAlive;
                    }
                    return false;
                }
            }

            public void StartPacUIOUpdaterThread()
            {
                lock (PacUIOUpdaterThreadLocker)
                {
                    if (!IsUpdaterThreadAlive)
                    {
                        KeepPacUIOUpdaterAlive = true;
                        PacUIOUpdater = new Thread(PacUIOUpdaterDoIt);
                        PacUIOUpdater.Name = "PacUIO {0:0} updater thread".Build(Id);
                        PacUIOUpdater.Start();
                    }
                }
            }
            public void TerminatePacUIOUpdaterThread()
            {
                lock (PacUIOUpdaterThreadLocker)
                {
                    if (PacUIOUpdater != null)
                    {
                        try
                        {
                            KeepPacUIOUpdaterAlive = false;
                            TriggerPacUIOUpdaterThread();
                            if (!PacUIOUpdater.Join(1000))
                            {
                                PacUIOUpdater.Abort();
                            }

                        }
                        catch (Exception E)
                        {
                            Log.Exception("A error occurd during termination of {0}.".Build(PacUIOUpdater.Name), E);
                            throw new Exception("A error occurd during termination of {0}.".Build(PacUIOUpdater.Name), E);
                        }
                        PacUIOUpdater = null;
                    }
                }
            }

            bool TriggerUpdate = false;
            public void TriggerPacUIOUpdaterThread()
            {
                //Log.Write("PacUIO.TriggerPacUIOUpdaterThread");
                TriggerUpdate = true;
                lock (PacUIOUpdaterThreadLocker)
                {
                    //Log.Write("PacUIO.TriggerPacUIOUpdaterThread, Monitor.Pulse");
                    Monitor.Pulse(PacUIOUpdaterThreadLocker);
                }
            }


            //TODO: Check if thread should really terminate on failed updates
            private void PacUIOUpdaterDoIt()
            {
                //Log.Write("PacUIO.PacUIOUpdaterDoIt START");
                try
                {
                    ResetFadeTime();

                }
                catch (Exception E)
                {
                    Log.Exception("A exception occured while setting the fadetime for PacUIO {0} to 0.".Build(Index), E);
                    throw;
                }
                int FailCnt = 0;
                while (KeepPacUIOUpdaterAlive)
                {
                    try
                    {
                        if (IsPresent)
                        {

                            SendPacUIOUpdate();

                        }
                        FailCnt = 0;
                    }
                    catch (Exception E)
                    {
                        Log.Exception("A error occured when updating PacUIO {0}".Build(Id), E);
                        //Pinball.ThreadInfoList.RecordException(E);
                        FailCnt++;

                        if (FailCnt > MaxUpdateFailCount)
                        {
                            Log.Exception("More than {0} consecutive updates failed for PacUIO {1}. Updater thread will terminate.".Build(MaxUpdateFailCount, Id));
                            KeepPacUIOUpdaterAlive = false;
                        }
                    }
                    //Pinball.ThreadInfoList.HeartBeat();
                    if (KeepPacUIOUpdaterAlive)
                    {
                        lock (PacUIOUpdaterThreadLocker)
                        {
                            while (!TriggerUpdate && KeepPacUIOUpdaterAlive)
                            {
                                //Log.Write("PacUIO.PacUIOUpdaterDoIt Monitor.wait START");
                                Monitor.Wait(PacUIOUpdaterThreadLocker, 50);  // Lock is released while we’re waiting
                                //Log.Write("PacUIO.PacUIOUpdaterDoIt Monitor.wait DONE");
                                //Pinball.ThreadInfoList.HeartBeat();
                            }

                        }

                    }
                    TriggerUpdate = false;
                }
                //Log.Write("PacUIO.PacUIOUpdaterDoIt END");
                //Pinball.ThreadInfoList.ThreadTerminates();
            }



            //private bool ForceFullUpdate = true;
            /*
             * variable introduced as true 20130809 / https://github.com/DirectOutput/DirectOutput/commit/090a0f0a2c778bb140968aa9be5888af6908c60c
             * setting this true would in effect force update the entire led array through the pacsdk PacLed64SetLEDIntensities every ~40ms (all groups, all ports)
             * causing the led array to act as a "lossless pixel renderer".
             * during attack from mars this would cause the uio to freeze up from 30s to 5 minutes of playtime (v1.33, onboard led would stop lighting) with stuck flippers and lights until shutdown and / or unplugging power + usb.
             * by setting this back to false, PacLed64SetLEDIntensities should only be triggered during high amount of events (more than 50, typically 1-5 are triggered at the same time)
             * this change could bring performance boosts as the uio is less "offline", and there are less calls.
             * 
             * should this be applied to the pacled64 as well?
             * */
            private bool ForceFullUpdate = false;
            private void SendPacUIOUpdate() {
                //Log.Write("PacUIO.SendPacUIOUpdate START");
                if (IsPresent) {
                    lock (PacUIOUpdateLocker) {
                        lock (ValueChangeLocker) {
                            if (!UpdateRequired && !ForceFullUpdate) return;

                            CopyNewToCurrent();
                            UpdateRequired = false;
                        }

                        byte IntensityUpdatesRequired = 0;
                        byte StateUpdatesRequired = 0;
                        if (!ForceFullUpdate) {

                            //uio has 1-12 groups for a total of 1-96 leds, with each group having 8 ports (g and p)
                            for (int g = 0; g < 12; g++) {
                                bool StateUpdateRequired = false;
                                for (int p = 0; p < 8; p++) {
                                    int o = g << 3 | p;
                                    if (CurrentValue[o] > 0) {
                                        if (CurrentValue[o] != LastValueSent[o]) {
                                            IntensityUpdatesRequired++;
                                        } else if (!LastStateSent[o]) {
                                            StateUpdateRequired = true;
                                        }
                                    } else if (LastStateSent[o]) {
                                        StateUpdateRequired = true;
                                    }

                                }
                                if (StateUpdateRequired) StateUpdatesRequired++;
                                StateUpdateRequired = false;
                            }
                        }

                        //Log.Write("PacUIO.SendPacUIOUpdate PDSingleton.PacLed64SetLEDIntensities...IntensityUpdatesRequired=" + IntensityUpdatesRequired+ ", StateUpdatesRequired="+ StateUpdatesRequired+", total="+(IntensityUpdatesRequired + StateUpdatesRequired));

                        //if (ForceFullUpdate || (IntensityUpdatesRequired + StateUpdatesRequired) > 30) {
                        if (ForceFullUpdate || (IntensityUpdatesRequired + StateUpdatesRequired) > 50) {
                            //more than 50 update calls required, send as one long update
                            //TODO: note, this call requires a delay, according to pacdrivesdk, of about 20ms before any new calls are made...which doesn't seem implemented yet
                            PDSingleton.PacLed64SetLEDIntensities(Index, CurrentValue);
                            //Log.Write("PacUIO.SendPacUIOUpdate PDSingleton.PacLed64SetLEDIntensities...done");
                            Array.Copy(CurrentValue, LastValueSent, CurrentValue.Length);
                            for (int i = 0; i < 96; i++) {
                                LastStateSent[i] = (LastValueSent[i] > 0);
                            }
                        } else {
                            //Will send separate intensity and state updates.
                            //seperate intensity and state updates, as recommended by the pacdrive sdk
                            //uio has 1-12 groups for a total of 1-96 leds, with each group having 8 ports (g and p)
                            for (int g = 0; g < 12; g++) {
                                int Mask = 0;
                                bool StateUpdateRequired = false;
                                for (int p = 0; p < 8; p++) {
                                    int o = g << 3 | p;
                                    if (CurrentValue[o] > 0) {
                                        if (CurrentValue[o] != LastValueSent[o]) {
                                            PDSingleton.PacLed64SetLEDIntensity(Index, o, CurrentValue[o]);
                                            LastStateSent[o] = true;
                                            LastValueSent[o] = CurrentValue[o];
                                        } else if (!LastStateSent[o]) {
                                            Mask |= (1 << p);
                                            StateUpdateRequired = true;
                                            LastStateSent[o] = true;
                                        }
                                    } else if (LastStateSent[o]) {
                                        StateUpdateRequired = true;
                                        LastStateSent[o] = false;
                                        LastValueSent[o] = 0;
                                    }

                                }
                                if (StateUpdateRequired) {
                                    //Log.Write("PacUIO.SendPacUIOUpdate  0: PDSingleton.PacLed64SetLEDStates...index=" + Index+", group="+(g+1));
                                    PDSingleton.PacLed64SetLEDStates(Index, g + 1, (byte)Mask);
                                    //Log.Write("PacUIO.SendPacUIOUpdate  1: PDSingleton.PacLed64SetLEDStates...done");
                                    StateUpdateRequired = false;
                                }

                            }
                        }


                    }
                    //ForceFullUpdate = false;
                    //Log.Write("PacUIO.SendPacUIOUpdate END");
                } else {
                    ForceFullUpdate = true;
                }

            }


            public void ShutdownLighting()
            {
                Log.Write("PacUIO.ShutdownLighting");
                PDSingleton.PacLed64SetLEDStates(0, 0, 0);
                LastStateSent.Fill(false);
            }

            private void ResetFadeTime()
            {
                Log.Write("PacUIO.ResetFadeTime");
                PDSingleton.PacLed64SetLEDFadeTime(Index, 0);

            }



            private bool IsPresent
            {
                get
                {
                    //if (!Id.IsBetween(1, 4)) return false;
                    if (!Id.IsBetween(0, 3)) return false;
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
                TriggerPacUIOUpdaterThread();
            }

            private void InitUnit()
            {
                if (Index >= 0)
                {
                    LastValueSent.Fill((byte)0);

                    PDSingleton.PacLed64SetLEDIntensities(Index, LastValueSent);
                    LastStateSent.Fill(false);
                    LastValueSent.Fill((byte)0);
                }
            }


            public PacUIOUnit(int Id)
            {
                this.Id = Id;

                PDSingleton = PacDriveSingleton.Instance;
                PDSingleton.OnPacAttached += new PacDriveSingleton.PacAttachedDelegate(Instance_OnPacAttached);
                PDSingleton.OnPacRemoved += new PacDriveSingleton.PacRemovedDelegate(Instance_OnPacRemoved);

                this.Index = PacDriveSingleton.Instance.PacUIOGetIndexForDeviceId(Id);

                NewValue.Fill((byte)0);

                InitUnit();


            }


        }



        #endregion




    }
}
