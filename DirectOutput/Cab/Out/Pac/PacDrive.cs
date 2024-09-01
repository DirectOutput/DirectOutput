﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Serialization;
using DirectOutput.General;
using DirectOutput.Cab.Schedules;
using DirectOutput.Cab.Overrides;

namespace DirectOutput.Cab.Out.Pac
{
    /// <summary>
    /// The PacDrive is a simple output controller with 16 digital/on off outputs.
    /// 
    /// DOF supports a the use of 1 PacDrive unit. This unit can be detected and configured automatically. If auto configuration is used, the generated LedWizEquivalent toy for the PacDrive will have number 19. This means that ini files numbered with 19 are automatically used to configure a PicDrive unit.
    /// 
    /// The outputs are by default turned on when the PacDrive unit is powered up. This controller class will turn off the PacDrive outputs upon initialisation and when it is finished.
    ///
    /// This unit is made and sold by <a target="_blank" href="http://www.ultimarc.com">Ultimarc</a>.
    /// 
    /// \image html pacdrivelogo.jpg
    /// </summary>
    public class PacDrive : OutputControllerBase, IOutputController
    {

        #region IOutputcontroller implementation
        /// <summary>
        /// Signals the workerthread that all pending updates for the PacDrive should be sent to the PacDrive.
        /// </summary>
        public override void Update()
        {
            PacDriveInstance.TriggerPacDriveUpdaterThread();
        }


        /// <summary>
        /// Initializes the PacDrive object.<br />
        /// This method does also start the workerthread which does the actual update work when Update() is called.<br />
        /// This method should only be called once. Subsequent calls have no effect.
        /// </summary>
        /// <param name="Cabinet">The cabinet object which is using the output controller instance.</param>
        public override void Init(Cabinet Cabinet)
        {
            AddOutputs();
            PacDriveInstance.Init(Cabinet);
            Log.Write("PacDrive initialized and updater thread started.");

        }

        /// <summary>
        /// Finishes the PacDrive object.<br/>
        /// Finish does also terminate the workerthread.
        /// </summary>
        public override void Finish()
        {
            PacDriveInstance.Finish();
            PacDriveInstance.ShutdownLighting();
            Log.Write("PacDrive finished and updater thread stopped.");

        }
        #endregion



        #region Outputs


        /// <summary>
        /// Adds the outputs for a PacDrive.<br/>
        /// A PacDrive has 16 outputs numbered from 1 to 64. This method adds OutputNumbered objects for all outputs to the list.
        /// </summary>
        private void AddOutputs()
        {
            for (int i = 1; i <= 16; i++)
            {
                if (!Outputs.Any(x => x.Number == i))
                {
                    Outputs.Add(new Output() { Name = "{0}.{1:00}".Build(Name, i), Number = i });
                }
            }
        }




        /// <summary>
        /// This method is called whenever the value of a output in the Outputs property changes its value.<br />
        /// It updates the internal array holding the states of the PacDrive outputs.
        /// </summary>
        /// <param name="Output">The output.</param>
        /// <exception cref="System.Exception">
        /// The OutputValueChanged event handler for the PacDrive uni has been called by a sender which is not a OutputNumbered.
        /// or
        /// PacDrive output numbers must be in the range of 1-16. The supplied output number {0} is out of range..Build(ON.Number)
        /// </exception>
        protected override void OnOutputValueChanged(IOutput Output)
        {

            if (!(Output is IOutput))
            {
                throw new Exception("The OutputValueChanged event handler for the PacDrive uni has been called by a sender which is not a OutputNumbered.");
            }
            IOutput ON = Output;
               

            if (!ON.Number.IsBetween(1, 64))
            {
                throw new Exception("PacDrive output numbers must be in the range of 1-16. The supplied output number {0} is out of range.".Build(ON.Number));
            }

            //check for table overrides
            ON = TableOverrideSettings.Instance.getnewrecalculatedOutput(ON, 19, 0);

            //check for scheduled setting, note there seems to only be support for a single [19] pacdrive?
            PacDriveInstance.UpdateValue(ScheduledSettings.Instance.getnewrecalculatedOutput(ON, 19, 0));
        }



        #endregion









        //~PacDrive()
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

        //        TerminatePacDrive();


        //        // Note disposing has been done.
        //        disposed = true;

        //    }
        //}
        //private bool disposed = false;

        //#endregion



        #region Constructor


        /// <summary>
        /// Initializes the <see cref="PacDriveInstance"/> class.
        /// </summary>
        static PacDrive()
        {
            PacDriveInstance = new PacDriveUnit();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacDriveInstance"/> class.
        /// </summary>
        public PacDrive()
        {
            Outputs = new OutputList();
            Name = "PacDrive";
        }




        #endregion






        #region Internal class for PacDrive output states and update methods

        private static PacDriveUnit PacDriveInstance=null;

        private class PacDriveUnit
        {
            //private Pinball Pinball;

           

            private const int MaxUpdateFailCount = 5;

            private PacDriveSingleton PDSingleton;

            private ushort NewValue;
            private ushort CurrentValue;
            private bool UpdateRequired = true;

            private int Index = -1;

            public object PacDriveUpdateLocker = new object();
            public object ValueChangeLocker = new object();

            public Thread PacDriveUpdater;
            public bool KeepPacDriveUpdaterAlive = false;
            public object PacDriveUpdaterThreadLocker = new object();


            public void Init(Cabinet Cabinet)
            {
                //this.Pinball = Cabinet.Pinball;
                //if (!Pinball.TimeSpanStatistics.Contains("PacDrive update calls"))
                //{
                //    UpdateTimeStatistics = new TimeSpanStatisticsItem() { Name = "PacDrive update calls", GroupName = "OutputControllers - PacDrive" };
                //    Pinball.TimeSpanStatistics.Add(UpdateTimeStatistics);
                //}
                //else
                //{
                //    UpdateTimeStatistics = Pinball.TimeSpanStatistics["PacDrive update calls"];
                //}

                StartPacDriveUpdaterThread();
            }

            public void Finish()
            {

                TerminatePacDriveUpdaterThread();
                ShutdownLighting();
        

            }

            public void UpdateValue(IOutput Output)
            {
                //Skip update on output numbers which are out of range
                if (!Output.Number.IsBetween(1, 16)) return;

                int ZeroBasedOutputNumber = Output.Number - 1;
                ushort Mask = (ushort)(1 << ZeroBasedOutputNumber);
                lock (ValueChangeLocker)
                {
                    if (Output.Value != 0)
                    {
                        NewValue |= Mask;
                    }
                    else
                    {
                        NewValue &= (ushort)~Mask;
                    }
                    UpdateRequired = true;
                }

            }
            private void CopyNewToCurrent()
            {
                lock (ValueChangeLocker)
                {
                    CurrentValue = NewValue;

                }
            }

            public bool IsUpdaterThreadAlive
            {
                get
                {
                    if (PacDriveUpdater != null)
                    {
                        return PacDriveUpdater.IsAlive;
                    }
                    return false;
                }
            }

            public void StartPacDriveUpdaterThread()
            {
                lock (PacDriveUpdaterThreadLocker)
                {
                    if (!IsUpdaterThreadAlive)
                    {
                        KeepPacDriveUpdaterAlive = true;
                        PacDriveUpdater = new Thread(PacDriveUpdaterDoIt);
                        PacDriveUpdater.Name = "PacDrive updater thread";
                        PacDriveUpdater.Start();
                    }
                }
            }
            public void TerminatePacDriveUpdaterThread()
            {
                lock (PacDriveUpdaterThreadLocker)
                {
                    if (PacDriveUpdater != null)
                    {
                        try
                        {
                            KeepPacDriveUpdaterAlive = false;
                            TriggerPacDriveUpdaterThread();
                            if (!PacDriveUpdater.Join(1000))
                            {
                                PacDriveUpdater.Abort();
                            }

                        }
                        catch (Exception E)
                        {
                            Log.Exception("A error occurd during termination of {0}.".Build(PacDriveUpdater.Name), E);
                            throw new Exception("A error occurd during termination of {0}.".Build(PacDriveUpdater.Name), E);
                        }
                        PacDriveUpdater = null;
                    }
                }
            }

            bool TriggerUpdate = false;
            public void TriggerPacDriveUpdaterThread()
            {
                if (!UpdateRequired) return;
                TriggerUpdate = true;
                UpdateRequired = false;
                lock (PacDriveUpdaterThreadLocker)
                {
                    Monitor.Pulse(PacDriveUpdaterThreadLocker);
                }
            }


            //TODO: Check if thread should really terminate on failed updates
            private void PacDriveUpdaterDoIt()
            {
                //Pinball.ThreadInfoList.HeartBeat("PacDrive");


                int FailCnt = 0;
                while (KeepPacDriveUpdaterAlive)
                {
                    try
                    {
                        if (IsPresent)
                        {
                          
                            SendPacDriveUpdate();
                     
                        }
                        FailCnt = 0;
                    }
                    catch (Exception E)
                    {
                        Log.Exception("A error occurred when updating PacDrive", E);
                        //Pinball.ThreadInfoList.RecordException(E);
                        FailCnt++;

                        if (FailCnt > MaxUpdateFailCount)
                        {
                            Log.Exception("More than {0} consecutive updates failed for PacDrive. Updater thread will terminate.".Build(MaxUpdateFailCount));
                            KeepPacDriveUpdaterAlive = false;
                        }
                    }
                    //Pinball.ThreadInfoList.HeartBeat();
                    if (KeepPacDriveUpdaterAlive)
                    {
                        lock (PacDriveUpdaterThreadLocker)
                        {
                            while (!TriggerUpdate && KeepPacDriveUpdaterAlive)
                            {
                                Monitor.Wait(PacDriveUpdaterThreadLocker, 50);  // Lock is released while we’re waiting
                                //Pinball.ThreadInfoList.HeartBeat();
                            }

                        }

                    }
                    TriggerUpdate = false;
                }

                ShutdownLighting();

                //Pinball.ThreadInfoList.ThreadTerminates();
            }



            private bool ForceFullUpdate = true;
            private void SendPacDriveUpdate()
            {
                if (IsPresent)
                {

                    lock (PacDriveUpdateLocker)
                    {
                        lock (ValueChangeLocker)
                        {
                            if (NewValue == CurrentValue && !ForceFullUpdate) return;

                            CopyNewToCurrent();

                        }


                        PDSingleton.PacDriveUHIDSetLEDStates(Index, CurrentValue);

                    }
                    ForceFullUpdate = false;
                }
                else
                {
                    ForceFullUpdate = true;
                }
            }


            public void ShutdownLighting()
            {
                lock (PacDriveUpdateLocker)
                {
                    lock (ValueChangeLocker)
                    {
                        CurrentValue = 0;
                        NewValue = 0;

                        PDSingleton.PacDriveUHIDSetLEDStates(Index, 0);
                    }
                }
            }



            private bool IsPresent
            {
                get
                {
                    return Index >= 0;
                }
            }


            void Instance_OnPacRemoved(int Index)
            {
                this.Index = PDSingleton.PacDriveGetIndex();
            }

            void Instance_OnPacAttached(int Index)
            {
                this.Index = PDSingleton.PacDriveGetIndex();
                InitUnit();
                TriggerPacDriveUpdaterThread();
            }

            private void InitUnit()
            {
                lock (ValueChangeLocker)
                {
                    NewValue = 0;
                    CurrentValue = 65535;
                };
                SendPacDriveUpdate();
            }


            public PacDriveUnit()
            {
                PDSingleton = PacDriveSingleton.Instance;
                PDSingleton.OnPacAttached += new PacDriveSingleton.PacAttachedDelegate(Instance_OnPacAttached);
                PDSingleton.OnPacRemoved += new PacDriveSingleton.PacRemovedDelegate(Instance_OnPacRemoved);
                this.Index = PacDriveSingleton.Instance.PacDriveGetIndex();
                InitUnit();


            }


        }



        #endregion




    }
}
