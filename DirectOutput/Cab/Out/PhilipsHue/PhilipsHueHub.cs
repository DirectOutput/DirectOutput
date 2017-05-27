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
using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.HSB;
using Q42.HueApi.Interfaces;
using System.Threading.Tasks;

namespace DirectOutput.Cab.Out.Pac
{
    /// <summary>
    /// The Philips Hue family consists of Zigbee controlled lights and sensors using a hub (the Philips Hue hub).
    /// 
    /// The framework supports auto detection and configuration of these units.
    /// 
    /// Philips Hue is made and sold by <a target="_blank" href="http://www2.meethue.com">Philips</a>.
    /// 
    /// The class was based off PacUIO.cs, and implementation makes use of <a target="_blank" href="https://github.com/Q42/Q42.HueApi">Q42.HueApi</a>.
    /// As such it retains the 3-channel RGB style inputs, but converts that over to a single-channel #rrggbb hex string.
    /// Technically a single hub can control about 50 lights x3 = 150 input channels.
    /// /// 
    /// </summary>
    public class PhilipsHueHub : OutputControllerBase, IOutputController
    {
        #region Id


        private object IdUpdateLocker = new object();
        private int _Id = -1;
        private string _HubIP = "0.0.0.0";
        private string _HubKey = "longsecretkey";
        private string _HubDeviceType = "dof_app#pincab";

        /// <summary>
        /// Gets or sets the Id of the Philips Hue.<br />
        /// The Id of the device must be unique and in the range of 0 to 1.<br />
        /// Setting changes the Name property, if it is blank or if the Name coresponds to PhilipsHueHub {Id}.
        /// </summary>
        /// <value>
        /// The unique Id of the PhilipsHueHub (Range 0-1).
        /// </value>
        /// <exception cref="System.Exception">
        /// PacUIO Ids must be between 0-1. The supplied Id {0} is out of range.
        /// </exception>
        public int Id {
            get { return _Id; }
            set {
                if (!value.IsBetween(0, 1)) {
                    throw new Exception("PhilipsHueHub Ids must be between 0-1. The supplied Id {0} is out of range.".Build(value));
                } lock (IdUpdateLocker) {
                    if (_Id != value) {

                        if (Name.IsNullOrWhiteSpace() || Name == "PhilipsHueHub {0:0}".Build(_Id)) {
                            Name = "PhilipsHueHub {0:0}".Build(value);
                        }

                        _Id = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets active hub IP.
        /// </summary>
        public string HubIP {
            get { return _HubIP; }
            set {
                lock (IdUpdateLocker) {
                    PhilipsHueHubUnits[Id].HubIP = value;
                    _HubIP = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets active hub user key.
        /// </summary>
        public string HubKey {
            get { return _HubKey; }
            set {
                lock (IdUpdateLocker) {
                    PhilipsHueHubUnits[Id].HubKey = value;
                    _HubKey = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets active hub device type / "app id".
        /// For an API to communicate with a Philips Hue hub, a first-time pairing mode using HubDeviceType as ID is required.
        /// Once this pairing is complete, a HubKey will be generated.
        /// To start communicating with a hub, HubIP and HubKey is required as handshake before controlling lights.
        /// Ideally this should not be changed / set, only read during initial pairing mode to get the actual key.
        /// </summary>
        public string HubDeviceType {
            get { return _HubDeviceType; }
            set {
                lock (IdUpdateLocker) {
                    PhilipsHueHubUnits[Id].HubDeviceType = value;
                    _HubDeviceType = value;
                }
            }
        }



        #endregion



        #region IOutputcontroller implementation
        /// <summary>
        /// Signals the workerthread that all pending updates for the PhilipsHueHub should be issued.
        /// </summary>
        public override void Update() {
            //Log.Write("PhilipsHueHub.Update");
            PhilipsHueHubUnits[Id].TriggerPhilipsHueHubUpdaterThread();
        }


        /// <summary>
        /// Initializes the PhilipsHueHub object.<br />
        /// This method does also start the workerthread which does the actual update work when Update() is called.<br />
        /// This method should only be called once. Subsequent calls have no effect.
        /// </summary>
        /// <param name="Cabinet">The cabinet object which is using the output controller instance.</param>
        public override void Init(Cabinet Cabinet) {
            AddOutputs();
            PhilipsHueHubUnits[Id].Init(Cabinet);
            Log.Write("PhilipsHueHub Id:{0} initialized and updater thread started.".Build(Id));
        }

        /// <summary>
        /// Finishes the PhilipsHueHub object.<br/>
        /// Finish does also terminate the workerthread.
        /// </summary>
        public override void Finish() {
            PhilipsHueHubUnits[Id].Finish();
            PhilipsHueHubUnits[Id].ShutdownLighting();
            Log.Write("PhilipsHueHub Id:{0} finished and updater thread stopped.".Build(Id));
        }
        #endregion



        #region Outputs


        /// <summary>
        /// Adds the outputs for a PhilipsHueHub.<br/>
        /// A Philips Hue hub has support for 50 combined RGB outputs numbered from 1 to 50. This method adds OutputNumbered objects for all outputs to the list.
        /// To keep compatibility with DOF Config Tool, each individual R, G and B input gets multiplexed at runtime into one output by multiplying 50x3 = 150 inputs / outputs.
        /// </summary>
        private void AddOutputs() {
            for (int i = 1; i <= 150; i++) {
                if (!Outputs.Any(x => (x).Number == i)) {
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
        /// The OutputValueChanged event handler for PhilipsHueHub unit {0} (Id: {2:0}) has been called by a sender which is not a OutputNumbered.<br/>
        /// or<br/>
        /// PhilipsHueHub output numbers must be in the range of 1-150. The supplied output number {0} is out of range.
        /// </exception>
        protected override void OnOutputValueChanged(IOutput Output) {
            IOutput ON = Output;

            if (!ON.Number.IsBetween(1, 150)) {
                throw new Exception("PhilipsHueHub output numbers must be in the range of 1-150. The supplied output number {0} is out of range.".Build(ON.Number));
            }

            PhilipsHueHubUnit S = PhilipsHueHubUnits[this.Id];

            //[50-51]
            S.UpdateValue(ScheduledSettings.Instance.getnewrecalculatedOutput (ON, 50, Id));
        }

        #endregion
        



        
        #region Constructor


        /// <summary>
        /// Initializes the <see cref="PhilipsHueHub"/> class.
        /// </summary>
        static PhilipsHueHub() {

            PhilipsHueHubUnits = new Dictionary<int, PhilipsHueHubUnit>();
            for (int i = 0; i <= 1; i++) {
                PhilipsHueHubUnits.Add(i, new PhilipsHueHubUnit(i));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhilipsHueHub"/> class.
        /// </summary>
        public PhilipsHueHub() {
            Outputs = new OutputList();
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="PhilipsHueHub"/> class.
        /// </summary>
        /// <param name="Id">The number of the PhilipsHueHub (0-1).</param>
        public PhilipsHueHub(int Id) : this() {
            this.Id = Id;
        }

        public void ConnectHub() {
            Log.Write("PhilipsHueHub.ConnectHub... attempting to connect to hub... ip=" + HubIP + " key=" + HubKey);
            PhilipsHueHubUnits[Id].ConnectUnit();
        }

        #endregion






        #region Internal class for PhilipsHueHub output states and update methods

        private static Dictionary<int, PhilipsHueHubUnit> PhilipsHueHubUnits = new Dictionary<int, PhilipsHueHubUnit>();

        private class PhilipsHueHubUnit {
            private const int MaxUpdateFailCount = 5;
            public int Id { get; private set; }
            private int Index { get; set; }
            private ILocalHueClient hueClient;

            //array, one per output, each output being combined rgb unlike pacuio
            private byte[] NewValue = new byte[150];
            private byte[] CurrentValue = new byte[150];

            private byte[] LastValueSent = new byte[150];
            private bool[] LastStateSent = new bool[150];

            public bool UpdateRequired = true;

            public object PhilipsHueHubUpdateLocker = new object();
            public object ValueChangeLocker = new object();

            public Thread PhilipsHueHubUpdater;
            public bool KeepPhilipsHueHubUpdaterAlive = false;
            public object PhilipsHueHubUpdaterThreadLocker = new object();

            public string HubIP = "0.0.0.0";
            public string HubKey = "longsecret";
            public string HubDeviceType = "dof_app#pincab";

            /// <summary>
            /// Hub communication delay in milliseconds. Value is intended to stagger / reduce communication to improve performance and let the issued commands complete.
            /// If the hub is spammed with commands like an UIO lights will stay off or remain unchanged, then randomly flicker.
            /// Consider a delay of at least 300ms initially, meaning all values inbetween will be ignored.
            /// There is a danger when staggering / ignoring commands resulting in in missing commands, for instance resulting in a dim light instead of its last command being complete dark.
            /// To try and combat this, do a connection check to the hub every now and then, and use a slightly higher value than the one detected.
            /// </summary>
            private int hubcommunicationDelay = 300;

            /// <summary>
            /// Any detected values below 50ms will not be realistic and can trigger hub overload (high queue depths).
            /// </summary>
            private int hubcommunicationminimumDelay = 50;

            /// <summary>
            /// Introduce a safety buffer to avoid overloading the hub.
            /// </summary>
            private double hubcommunicationdelayFactor = 1.2;

            /// <summary>
            /// How often to check for connection. By doing this we can calibrate hubcommunicationDelay dynamically and adjust for actual communication latency over time.
            /// </summary>
            private int hubcommunicationPing = 3000;

            //ticks is ms * 10000
            private long hubcommunicationlastTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            private long hubcommunicationlastpingTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            public void Init(Cabinet Cabinet) {
                StartPhilipsHueHubUpdaterThread();
            }

            public void Finish() {
                TerminatePhilipsHueHubUpdaterThread();
                ShutdownLighting();
            }

            public void UpdateValue(IOutput Output) {
                //Skip update on output numbers which are out of range
                if (!Output.Number.IsBetween(1, 150)) return;

                int ZeroBasedOutputNumber = Output.Number - 1;
                lock (ValueChangeLocker) {
                    if (NewValue[ZeroBasedOutputNumber] != Output.Value) {
                        NewValue[ZeroBasedOutputNumber] = Output.Value;
                        UpdateRequired = true;
                    }
                }
            }
            private void CopyNewToCurrent() {
                lock (ValueChangeLocker) {
                    Array.Copy(NewValue, CurrentValue, NewValue.Length);
                }
            }

            public bool IsUpdaterThreadAlive {
                get {
                    if (PhilipsHueHubUpdater != null) {
                        return PhilipsHueHubUpdater.IsAlive;
                    }
                    return false;
                }
            }

            public void StartPhilipsHueHubUpdaterThread() {
                lock (PhilipsHueHubUpdaterThreadLocker) {
                    if (!IsUpdaterThreadAlive) {
                        KeepPhilipsHueHubUpdaterAlive = true;
                        PhilipsHueHubUpdater = new Thread(PhilipsHueHubUpdaterDoIt);
                        PhilipsHueHubUpdater.Name = "PhilipsHueHub {0:0} updater thread".Build(Id);
                        PhilipsHueHubUpdater.Start();
                    }
                }
            }

            public void TerminatePhilipsHueHubUpdaterThread() {
                lock (PhilipsHueHubUpdaterThreadLocker) {
                    if (PhilipsHueHubUpdater != null) {
                        try {
                            KeepPhilipsHueHubUpdaterAlive = false;
                            TriggerPhilipsHueHubUpdaterThread();
                            if (!PhilipsHueHubUpdater.Join(1000)) {
                                PhilipsHueHubUpdater.Abort();
                            }
                        } catch (Exception E) {
                            Log.Exception("A error occurd during termination of {0}.".Build(PhilipsHueHubUpdater.Name), E);
                            throw new Exception("A error occurd during termination of {0}.".Build(PhilipsHueHubUpdater.Name), E);
                        }
                        PhilipsHueHubUpdater = null;
                    }
                }
            }

            bool TriggerUpdate = false;
            public void TriggerPhilipsHueHubUpdaterThread() {
                //Log.Write("PhilipsHueHub.TriggerPhilipsHueHubUpdaterThread");
                TriggerUpdate = true;
                lock (PhilipsHueHubUpdaterThreadLocker) {
                    //Log.Write("PhilipsHueHub.TriggerPhilipsHueHubUpdaterThread, Monitor.Pulse");
                    Monitor.Pulse(PhilipsHueHubUpdaterThreadLocker);
                }
            }


            //TODO: Check if thread should really terminate on failed updates
            private void PhilipsHueHubUpdaterDoIt() {
                //Log.Write("PhilipsHueHub.PhilipsHueHubUpdaterDoIt START");
                try {
                    ResetFadeTime();
                } catch (Exception E) {
                    Log.Exception("A exception occured while setting the fadetime for PhilipsHueHub {0} to 0.".Build(Index), E);
                    throw;
                }
                int FailCnt = 0;
                while (KeepPhilipsHueHubUpdaterAlive) {
                    try {
                        if (IsPresent) {
                            SendPhilipsHueHubUpdate();
                        }
                        FailCnt = 0;
                    } catch (Exception E) {
                        Log.Exception("A error occured when updating PhilipsHueHub {0}".Build(Id), E);
                        FailCnt++;

                        if (FailCnt > MaxUpdateFailCount) {
                            Log.Exception("More than {0} consecutive updates failed for PhilipsHueHub {1}. Updater thread will terminate.".Build(MaxUpdateFailCount, Id));
                            KeepPhilipsHueHubUpdaterAlive = false;
                        }
                    }
                    if (KeepPhilipsHueHubUpdaterAlive) {
                        lock (PhilipsHueHubUpdaterThreadLocker) {
                            while (!TriggerUpdate && KeepPhilipsHueHubUpdaterAlive) {
                                Monitor.Wait(PhilipsHueHubUpdaterThreadLocker, 50);
                            }
                        }
                    }
                    TriggerUpdate = false;
                }
            }

            //private bool ForceFullUpdate = false;
            private void SendPhilipsHueHubUpdate() {
                //Log.Write("PhilipsHueHub.SendPhilipsHueHubUpdate START");
                long hubcommunicationDelta = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - hubcommunicationlastTimestamp;
                long hubcommunicationpingDelta = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - hubcommunicationlastpingTimestamp;
                
                int hubbulbID = 0;
                if (IsPresent) {
                    lock (PhilipsHueHubUpdateLocker) {
                        lock (ValueChangeLocker) {
                            if (!UpdateRequired) return;

                            CopyNewToCurrent();
                            UpdateRequired = false;
                        }

                        //seperate intensity and state updates, convert byte to hex values as a single output can carry rrggbb
                        for (int i = 0; i < 150; i++) {

                            //hue sends a combined / multiplexed rrggbb output, scan and combine 3 inputs into one output if any of the three inputs differ
                            if (CurrentValue[i] != LastValueSent[i] || CurrentValue[i + 1] != LastValueSent[i + 1] || CurrentValue[i + 2] != LastValueSent[i + 2]) {
                                LastValueSent[i] = CurrentValue[i];
                                LastValueSent[i + 1] = CurrentValue[i + 1];
                                LastValueSent[i + 2] = CurrentValue[i + 2];


                                LightCommand newhubCommand = new LightCommand();
                                hubbulbID = ((i + 2 + 1) / 3);
                                string newhexColor = CurrentValue[i].ToString("x2") + CurrentValue[i + 1].ToString("x2") + CurrentValue[i + 2].ToString("x2");

                                //todo: transition up quickly, but always transition down slowly
                                //this can be used to fade down the amount of milliseconds until next send to try and have the hue actually do something useful to compensate for communication lag

                                //if (hubcommunicationDelay >)
                                //newhubCommand.TransitionTime = new TimeSpan(0);
                                newhubCommand.TransitionTime = TimeSpan.FromMilliseconds(hubcommunicationDelay);


                                if (CurrentValue[i] == 0 && CurrentValue[i + 1] == 0 && CurrentValue[i + 2] == 0) {
                                    newhubCommand.TurnOff();
                                    newhubCommand.On = false;
                                } else {
                                    newhubCommand.On = true;
                                    newhubCommand.SetColor(new RGBColor(newhexColor));
                                }

                                //controlled commands, avoid spamming the hub too quickly unless last command is black (power off) as this will result in delayed and inconsistent commands long after being sent (still in hub queue)
                                //if (hubcommunicationDelta >= hubcommunicationDelay || newhubCommand.On == false) {// || CurrentValue[i] == 0 || CurrentValue[i+1] == 0 || CurrentValue[i+2] == 0) {
                                if (hubcommunicationDelta >= hubcommunicationDelay || newhubCommand.On == false || newhexColor.ToLower() == "000000") {
                                    Log.Write("PhilipsHueHub.SendPhilipsHueHubUpdate, i=" + i + ", RGB single output 1-based=" + hubbulbID + "/50, RGB hex=" + newhexColor + ", hubcommunicationlastTimestamp=" + hubcommunicationlastTimestamp + ", delta=" + hubcommunicationDelta);
                                    hueClient.SendCommandAsync(newhubCommand, new List<string> { hubbulbID.ToString() });

                                    //reset timestamp
                                    hubcommunicationlastTimestamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                                } else {
                                    //Log.Write("PhilipsHueHub.SendPhilipsHueHubUpdate: staggered updates, did not send " + newhexColor + " to bulb #" + hubbulbID + "/50, RGB hex=" + newhexColor + ", hubcommunicationlastTimestamp=" + hubcommunicationlastTimestamp + "ms, current=" + DateTime.Now.Millisecond + ", delta = " + hubcommunicationDelta + "ms");
                                }


                            }

                            //skip ahead to start of next rgb input
                            i += 2;

                        }

                        //check if we should check hub ping and recalibrate
                        if (hubcommunicationpingDelta >= hubcommunicationPing) {
                            hubcommunicationlastpingTimestamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                            CheckConnection();
                        }
                    }
                    //Log.Write("PhilipsHueHub.SendPhilipsHueHubUpdate END");
                }

            }


            public void ShutdownLighting() {
                Log.Write("PhilipsHueHub.ShutdownLighting");
                //PDSingletonn.PacLed64SetLEDStates(0, 0, 0);
                LastStateSent.Fill(false);
            }

            private void ResetFadeTime() {
                Log.Write("PhilipsHueHub.ResetFadeTime");
                //PDSingletonn.PacLed64SetLEDFadeTime(Index, 0);
            }



            private bool IsPresent {
                get {
                    if (!Id.IsBetween(0, 3)) return false;
                    return Index >= 0;
                }
            }

            private void InitUnit() {
                if (Index >= 0) {
                    LastValueSent.Fill((byte)0);

                    LastStateSent.Fill(false);
                    LastValueSent.Fill((byte)0);
                }
            }

            /// <summary>
            /// Connects to hub.
            /// </summary>
            public void ConnectUnit() {
                Log.Write("PhilipsHueHub.PhilipsHueHubUnit... connecting to hub... ip=" + HubIP + " key=" + HubKey);

                hueClient = new LocalHueClient(HubIP);
                hueClient.Initialize(HubKey);
            }

            /// <summary>
            /// Checks connection and adjusts communication delay dynamically.
            /// </summary>
            /// <returns></returns>
            public async Task CheckConnection() {
                long startTimestamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                long deltaTimestamp = 0;
                long adjustedPing = 0;
                var result = await hueClient.CheckConnection();

                deltaTimestamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - startTimestamp;
                adjustedPing = (int)Math.Round((int)deltaTimestamp * hubcommunicationdelayFactor);

                if (adjustedPing > hubcommunicationminimumDelay) {
                    hubcommunicationDelay = (int)adjustedPing;
                    Log.Write("PhilipsHueHub.CheckConnection... current connection lag =" + deltaTimestamp + "ms, recalibrating from " + hubcommunicationDelay + "ms -> " + deltaTimestamp + "ms, buffer factor=" + hubcommunicationdelayFactor + " -> " + adjustedPing + "ms");
                } else {
                    hubcommunicationDelay = hubcommunicationminimumDelay;
                    Log.Write("PhilipsHueHub.CheckConnection... current connection lag =" + deltaTimestamp + "ms, ignoring and using minimum "+ hubcommunicationminimumDelay + "ms to avoid hub overload");
                }
                
            }

            public PhilipsHueHubUnit(int Id) {
                this.Id = Id;
                this.Index = Id;

                NewValue.Fill((byte)0);
                InitUnit();
                //ConnectUnit();
            }


        }



        #endregion




    }
}
