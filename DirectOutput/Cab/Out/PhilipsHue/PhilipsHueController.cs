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

namespace DirectOutput.Cab.Out.Pac {
    /// <summary>
    /// The Philips Hue family consists of Zigbee controlled lights and sensors using a bridge (the Philips Hue "hub").
    /// 
    /// The framework supports auto detection and configuration of these units.
    /// 
    /// Philips Hue is made and sold by <a target="_blank" href="http://www2.meethue.com">Philips</a>.
    /// 
    /// The class was based off PacUIO.cs, and implementation makes use of <a target="_blank" href="https://github.com/Q42/Q42.HueApi">Q42.HueApi</a>.
    /// As such it retains the 3-channel RGB style inputs, but converts that over to a single-channel #rrggbb hex string.
    /// Technically a single bridge can control about 50 lights x3 = 150 input channels (sensors are additionally ~60).
    /// 
    /// Before DOF can start communicating with the bridge it will need a valid key.
    /// To get a key off the bridge the Link-button on the bridge needs to be pressed, and an dof#pincab "user" needs to be registered in the bridge (whitelist) within 30 seconds.
    /// This will return a unique key (for instance "2P4R5UT6KAQcpOjFaqwLDrbikEEBsMIHY6z6Gjwg") which can then be used from that point on.
    /// The same registration on another bridge, or even the same, will create a new key.
    /// 
    /// To avoid duplicates / spamming the whitelist, and to avoid bugs crashing the bridge, currently this is the suggested approach for getting a key:
    /// 
    /// Step 1: set static IP to bridge
    /// Get IP of the bridge. Check your phone Hue App -> Settings -> My Bridge -> Network settings. It should default to DHCP. Change this to a static IP, and make note of it.
    /// 
    /// Step 2: whitelist DOF using a browser
    /// Open up the bridge API in a browser using your IP, example (replace IP with your static IP):
    /// http://10.0.1.174/debug/clip.html
    /// In the "CLIP API Debugger" it should say something like URL: "/api/1234/" with GET, PUT, POST, and DELETE-buttons.
    /// Copy&paste the following line into URL-field (not in your browser, but in the CLIP API Debugger):
    /// /api
    /// Copy&paste the following into the Message Body textfield:
    /// {"devicetype":"dof_app#pincab"}
    /// Next, run over to your bridge and press the physical Link button. You now have 30 seconds to run back to your browser, and press "POST"-button.
    /// You should now get a username in the Command Response textbox, for example: "ywCNFGOagGoJYtm16Kq4PS1tkGBAd3bj1ajg7uCk". Make note of this.
    /// 
    /// Step 3: add IP and key to Cabinet.xml
    /// Open up your Cabinet.xml, and add the following lines in the OutputControllers section, replacing the IP and key with your own:
    /// <PhilipsHueController>
	///		<Name>PhilipsHueController</Name>
	///		<BridgeIP>10.0.1.174</BridgeIP>
	///		<BridgeKey>ywCNFGOagGoJYtm16Kq4PS1tkGBAd3bj1ajg7uCk</BridgeKey>
	///	</PhilipsHueController>
    ///
    /// Step 4: add lights using http://configtool.vpuniverse.com/login.php
    /// A bridge can handle about 50 lights. Each light will multiplex RGB (3 channels) on each send, similar to using RGB-buttons on a PacLed64 or UIO.
    /// To match your output channels to a specific light, use your Android / iOS Philips Hue app and decide which light you need to control.
    /// Each light should have a number in it, for instance "10. Hue lightstrip 1". That 10-number is the light ID.
    /// Mapped to the individual RGB-outputs in DOF Config Tool port assignments this means: ((light ID -1) * 3) + 1 = ((10 - 1) *3) + 1 = 27 +1 = 28, resulting in port 28, 29, 30 (R, G, B).
    /// If your light ID was 3, you'd map it to port 7, 8, 9.
    /// If your light ID was 1, you'd map it to port 1, 2, 3.
    /// 
    /// 
    /// If you want to delete the key from your bridge, open up CLIP again, enter the API-URL (replace both keys with your own, they're the same used twice), then press DELETE:
    /// http://10.0.1.174/debug/clip.html
    /// /api/ywCNFGOagGoJYtm16Kq4PS1tkGBAd3bj1ajg7uCk/config/whitelist/ywCNFGOagGoJYtm16Kq4PS1tkGBAd3bj1ajg7uCk
    /// 
    /// 
    /// /// 
    /// </summary>
    public class PhilipsHueController : OutputControllerBase, IOutputController
    {
        #region Id


        private object IdUpdateLocker = new object();
        //private int _Id = -1;
        private int _Id = 0;
        private string _BridgeIP = "";
        private string _BridgeKey = "";
        private string _BridgeDeviceType = "dof_app#pincab";

        /// <summary>
        /// Gets or sets the Id of the Philips Hue.<br />
        /// The Id of the device must be unique and in the range of 0 to 1.<br />
        /// Setting changes the Name property, if it is blank or if the Name coresponds to PhilipsHueController {Id}.
        /// </summary>
        /// <value>
        /// The unique Id of the PhilipsHueController (Range 0-1).
        /// </value>
        /// <exception cref="System.Exception">
        /// PacUIO Ids must be between 0-1. The supplied Id {0} is out of range.
        /// </exception>
        public int Id {
            get { return _Id; }
            set {
                if (!value.IsBetween(0, 1)) {
                    throw new Exception("PhilipsHueController Ids must be between 0-1. The supplied Id {0} is out of range.".Build(value));
                } lock (IdUpdateLocker) {
                    if (_Id != value) {

                        if (Name.IsNullOrWhiteSpace() || Name == "PhilipsHueController {0:0}".Build(_Id)) {
                            Name = "PhilipsHueController {0:0}".Build(value);
                        }

                        _Id = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets active bridge IP.
        /// </summary>
        public string BridgeIP {
            get { return _BridgeIP; }
            set {
                lock (IdUpdateLocker) {
                    PhilipsHueControllerUnits[Id].BridgeIP = value;
                    _BridgeIP = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets active bridge user key.
        /// </summary>
        public string BridgeKey {
            get { return _BridgeKey; }
            set {
                lock (IdUpdateLocker) {
                    PhilipsHueControllerUnits[Id].BridgeKey = value;
                    _BridgeKey = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets active bridge device type / "app id".
        /// For an API to communicate with a Philips Hue bridge, a first-time pairing mode using BridgeDeviceType as ID is required.
        /// Once this pairing is complete, a BridgeKey will be generated.
        /// To start communicating with a bridge, BridgeIP and BridgeKey is required as handshake before controlling lights.
        /// Ideally this should not be changed / set, only read during initial pairing mode to get the actual key.
        /// </summary>
        public string BridgeDeviceType {
            get { return _BridgeDeviceType; }
            set {
                lock (IdUpdateLocker) {
                    PhilipsHueControllerUnits[Id].BridgeDeviceType = value;
                    _BridgeDeviceType = value;
                }
            }
        }



        #endregion



        #region IOutputcontroller implementation
        /// <summary>
        /// Signals the workerthread that all pending updates for the PhilipsHueController should be issued.
        /// </summary>
        public override void Update() {
            //Log.Write("PhilipsHueController.Update");
            PhilipsHueControllerUnits[Id].TriggerPhilipsHueControllerUpdaterThread();
        }


        /// <summary>
        /// Initializes the PhilipsHueController object.<br />
        /// This method does also start the workerthread which does the actual update work when Update() is called.<br />
        /// This method should only be called once. Subsequent calls have no effect.
        /// Method was adjusted to work on its own when the AutoConfigurator was removed, and will attempt doing the same thing (check connection).
        /// </summary>
        /// <param name="Cabinet">The cabinet object which is using the output controller instance.</param>
        public override void Init(Cabinet Cabinet) {
            //Id = 0;
            AddOutputs();
            PhilipsHueControllerUnits[Id].Init(Cabinet);
            Log.Write("PhilipsHueController Id:{0} initialized and updater thread started.".Build(Id));
        }

        /// <summary>
        /// Finishes the PhilipsHueController object.<br/>
        /// Finish does also terminate the workerthread.
        /// </summary>
        public override void Finish() {
            PhilipsHueControllerUnits[Id].Finish();
            PhilipsHueControllerUnits[Id].ShutdownLighting();
            Log.Write("PhilipsHueController Id:{0} finished and updater thread stopped.".Build(Id));
        }
        #endregion



        #region Outputs


        /// <summary>
        /// Adds the outputs for a PhilipsHueController.<br/>
        /// A Philips Hue bridge has support for 50 combined RGB outputs numbered from 1 to 50. This method adds OutputNumbered objects for all outputs to the list.
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
        /// The OutputValueChanged event handler for PhilipsHueController unit {0} (Id: {2:0}) has been called by a sender which is not a OutputNumbered.<br/>
        /// or<br/>
        /// PhilipsHueController output numbers must be in the range of 1-150. The supplied output number {0} is out of range.
        /// </exception>
        protected override void OnOutputValueChanged(IOutput Output) {
            IOutput ON = Output;

            if (!ON.Number.IsBetween(1, 150)) {
                throw new Exception("PhilipsHueController output numbers must be in the range of 1-150. The supplied output number {0} is out of range.".Build(ON.Number));
            }

            PhilipsHueControllerUnit S = PhilipsHueControllerUnits[this.Id];

            //[50-51]
            S.UpdateValue(ScheduledSettings.Instance.getnewrecalculatedOutput (ON, 50, Id));
        }

        #endregion
        



        
        #region Constructor


        /// <summary>
        /// Initializes the <see cref="PhilipsHueController"/> class.
        /// </summary>
        static PhilipsHueController() {

            PhilipsHueControllerUnits = new Dictionary<int, PhilipsHueControllerUnit>();
            for (int i = 0; i <= 1; i++) {
                PhilipsHueControllerUnits.Add(i, new PhilipsHueControllerUnit(i));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhilipsHueController"/> class.
        /// </summary>
        public PhilipsHueController() {
            Outputs = new OutputList();
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="PhilipsHueController"/> class.
        /// </summary>
        /// <param name="Id">The number of the PhilipsHueController (0-1).</param>
        public PhilipsHueController(int Id) : this() {
            this.Id = Id;
        }

        public void ConnectHub() {
            //Log.Write("PhilipsHueController.ConnectHub... attempting to connect to hub... ip=" + BridgeIP + " key=" + BridgeKey);
            PhilipsHueControllerUnits[Id].ConnectUnit();
        }

        /*public async Task<bool> ConnectedToHub() {
            await PhilipsHueControllerUnits[Id].CheckConnection();
            return PhilipsHueControllerUnits[Id].BridgeConnected;
        }*/

        #endregion






        #region Internal class for PhilipsHueController output states and update methods

        private static Dictionary<int, PhilipsHueControllerUnit> PhilipsHueControllerUnits = new Dictionary<int, PhilipsHueControllerUnit>();

        private class PhilipsHueControllerUnit {
            private const int MaxUpdateFailCount = 5;
            public int Id { get; private set; }
            private int Index { get; set; }
            private ILocalHueClient hueClient;

            //array, one per output, each output being combined rgb unlike pacuio
            private byte[] NewValue = new byte[150];
            private byte[] CurrentValue = new byte[150];

            private byte[] LastValueSent = new byte[150];
            private bool[] LastStateSent = new bool[150];

            /// <summary>
            /// To avoid turning of all lights in a setup, including for instance a whole house or apartment, keep note of which has been adjusted and then turn those off during shutdown to avoia affecting others.
            /// Normally 0-based, but we're being lazy and matching with light id from the Hue app which is 1-based.
            /// </summary>
            private bool[] affectedLights = new bool[50];

            public bool UpdateRequired = true;

            public object PhilipsHueControllerUpdateLocker = new object();
            public object ValueChangeLocker = new object();

            public Thread PhilipsHueControllerUpdater;
            public bool KeepPhilipsHueControllerUpdaterAlive = false;
            public object PhilipsHueControllerUpdaterThreadLocker = new object();

            public string BridgeIP = "0.0.0.0";
            public string BridgeKey = "longsecret";
            public string BridgeDeviceType = "dof_app#pincab";
            public bool BridgeConnected = false;

            /// <summary>
            /// Bridge communication delay in milliseconds. Value is intended to stagger / reduce communication to improve performance and let the issued commands complete.
            /// If the bridge is spammed with commands like an UIO lights will stay off or remain unchanged, then randomly flicker.
            /// Consider a delay of at least 300ms initially, meaning all values inbetween will be ignored.
            /// There is a danger when staggering / ignoring commands resulting in in missing commands, for instance resulting in a dim light instead of its last command being complete dark.
            /// To try and combat this, do a connection check to the bridge every now and then, and use a slightly higher value than the one detected.
            /// </summary>
            private int bridgecommunicationDelay = 300;

            /// <summary>
            /// Any detected values below 50ms will not be realistic and can trigger bridge overload (high queue depths).
            /// Using 100ms minimum as recommended by https://developers.meethue.com/documentation/hue-system-performance
            /// </summary>
            private int bridgecommunicationminimumDelay = 100;

            /// <summary>
            /// Introduce a safety buffer to avoid overloading the bridge.
            /// </summary>
            private double bridgecommunicationdelayFactor = 1.2;

            /// <summary>
            /// How often to check for connection. By doing this we can calibrate bridgecommunicationDelay dynamically and adjust for actual communication latency over time.
            /// </summary>
            private int bridgecommunicationPing = 3000;

            //ticks is ms * 10000
            private long bridgecommunicationlastTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            private long bridgecommunicationlastpingTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            public void Init(Cabinet Cabinet) {
                StartPhilipsHueControllerUpdaterThread();
            }

            public void Finish() {
                TerminatePhilipsHueControllerUpdaterThread();
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
                    if (PhilipsHueControllerUpdater != null) {
                        return PhilipsHueControllerUpdater.IsAlive;
                    }
                    return false;
                }
            }

            public void StartPhilipsHueControllerUpdaterThread() {
                lock (PhilipsHueControllerUpdaterThreadLocker) {
                    if (!IsUpdaterThreadAlive) {
                        KeepPhilipsHueControllerUpdaterAlive = true;
                        PhilipsHueControllerUpdater = new Thread(PhilipsHueControllerUpdaterDoIt);
                        PhilipsHueControllerUpdater.Name = "PhilipsHueController {0:0} updater thread".Build(Id);
                        PhilipsHueControllerUpdater.Start();
                    }
                }
            }

            public void TerminatePhilipsHueControllerUpdaterThread() {
                lock (PhilipsHueControllerUpdaterThreadLocker) {
                    if (PhilipsHueControllerUpdater != null) {
                        try {
                            KeepPhilipsHueControllerUpdaterAlive = false;
                            TriggerPhilipsHueControllerUpdaterThread();
                            if (!PhilipsHueControllerUpdater.Join(1000)) {
                                PhilipsHueControllerUpdater.Abort();
                            }
                        } catch (Exception E) {
                            Log.Exception("A error occurd during termination of {0}.".Build(PhilipsHueControllerUpdater.Name), E);
                            throw new Exception("A error occurd during termination of {0}.".Build(PhilipsHueControllerUpdater.Name), E);
                        }
                        PhilipsHueControllerUpdater = null;
                    }
                }
            }

            bool TriggerUpdate = false;
            public void TriggerPhilipsHueControllerUpdaterThread() {
                //Log.Write("PhilipsHueController.TriggerPhilipsHueControllerUpdaterThread");
                TriggerUpdate = true;
                lock (PhilipsHueControllerUpdaterThreadLocker) {
                    //Log.Write("PhilipsHueController.TriggerPhilipsHueControllerUpdaterThread, Monitor.Pulse");
                    Monitor.Pulse(PhilipsHueControllerUpdaterThreadLocker);
                }
            }


            //TODO: Check if thread should really terminate on failed updates
            private void PhilipsHueControllerUpdaterDoIt() {
                //Log.Write("PhilipsHueController.PhilipsHueControllerUpdaterDoIt START");
                try {
                    ResetFadeTime();
                } catch (Exception E) {
                    Log.Exception("A exception occured while setting the fadetime for PhilipsHueController {0} to 0.".Build(Index), E);
                    throw;
                }
                int FailCnt = 0;
                while (KeepPhilipsHueControllerUpdaterAlive) {
                    try {
                        if (IsPresent) {
                            SendPhilipsHueControllerUpdate();
                        }
                        FailCnt = 0;
                    } catch (Exception E) {
                        Log.Exception("A error occured when updating PhilipsHueController {0}".Build(Id), E);
                        FailCnt++;

                        if (FailCnt > MaxUpdateFailCount) {
                            Log.Exception("More than {0} consecutive updates failed for PhilipsHueController {1}. Updater thread will terminate.".Build(MaxUpdateFailCount, Id));
                            KeepPhilipsHueControllerUpdaterAlive = false;
                        }
                    }
                    if (KeepPhilipsHueControllerUpdaterAlive) {
                        lock (PhilipsHueControllerUpdaterThreadLocker) {
                            while (!TriggerUpdate && KeepPhilipsHueControllerUpdaterAlive) {
                                Monitor.Wait(PhilipsHueControllerUpdaterThreadLocker, 50);
                            }
                        }
                    }
                    TriggerUpdate = false;
                }
            }

            //private bool ForceFullUpdate = false;
            private void SendPhilipsHueControllerUpdate() {
                //Log.Write("PhilipsHueController.SendPhilipsHueControllerUpdate START");
                long bridgecommunicationDelta = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - bridgecommunicationlastTimestamp;
                long bridgecommunicationpingDelta = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - bridgecommunicationlastpingTimestamp;
                
                int bridgebulbID = 0;
                if (IsPresent) {
                    lock (PhilipsHueControllerUpdateLocker) {
                        lock (ValueChangeLocker) {
                            if (!UpdateRequired) return;

                            CopyNewToCurrent();
                            UpdateRequired = false;
                        }

                        //seperate intensity and state updates, convert byte to hex values as a single output can carry rrggbb
                        for (int i = 0; i < 150; i++) {

                            //hue sends a combined / multiplexed rrggbb output, scan and combine 3 inputs into one output if any of the three inputs differ
                            if (CurrentValue[i] != LastValueSent[i] || CurrentValue[i + 1] != LastValueSent[i + 1] || CurrentValue[i + 2] != LastValueSent[i + 2]) {
                                LightCommand newbridgeCommand = new LightCommand();
                                bridgebulbID = ((i + 2 + 1) / 3);
                                string newhexColor = CurrentValue[i].ToString("x2") + CurrentValue[i + 1].ToString("x2") + CurrentValue[i + 2].ToString("x2");

                                //transition up from 0 / black as quickly as possible, but always transition down slowly
                                //this can be used to fade down the amount of milliseconds until next send to try and have the light actually do something useful to compensate for communication lag
                                if ((LastValueSent[i] == 0 && CurrentValue[i] >0) || (LastValueSent[i+1] == 0 && CurrentValue[i+1] > 0) || (LastValueSent[i+2] == 0 && CurrentValue[i+2] > 0)) {
                                    newbridgeCommand.TransitionTime = new TimeSpan(0);
                                } else {
                                    newbridgeCommand.TransitionTime = TimeSpan.FromMilliseconds(bridgecommunicationDelay);
                                }

                                //if all black, override transitiontime to smoother delay, and turn off
                                if (CurrentValue[i] == 0 && CurrentValue[i + 1] == 0 && CurrentValue[i + 2] == 0) {
                                    newbridgeCommand.TurnOff();
                                    newbridgeCommand.On = false;
                                    newbridgeCommand.TransitionTime = TimeSpan.FromMilliseconds(bridgecommunicationDelay);
                                } else {
                                    newbridgeCommand.On = true;
                                    newbridgeCommand.SetColor(new RGBColor(newhexColor));

                                    //set light id to affected for shutdown, stored in zero-based index
                                    affectedLights[bridgebulbID-1] = true;
                                }

                                LastValueSent[i] = CurrentValue[i];
                                LastValueSent[i + 1] = CurrentValue[i + 1];
                                LastValueSent[i + 2] = CurrentValue[i + 2];

                                //controlled commands, avoid spamming the bridge too quickly unless last command is black (power off) as this will result in delayed and inconsistent commands long after being sent (still in bridge queue)
                                if (bridgecommunicationDelta >= bridgecommunicationDelay || newbridgeCommand.On == false || newhexColor.ToLower() == "000000") {
                                    //Log.Write("PhilipsHueController.SendPhilipsHueControllerUpdate IP="+BridgeIP+", i=" + i + ", RGB single output 1-based=" + bridgebulbID + "/50, RGB hex=" + newhexColor + ", delta=" + bridgecommunicationDelta);
                                    hueClient.SendCommandAsync(newbridgeCommand, new List<string> { bridgebulbID.ToString() });

                                    //reset timestamp
                                    bridgecommunicationlastTimestamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                                } else {
                                    //Log.Write("PhilipsHueController.SendPhilipsHueControllerUpdate: staggered updates, did not send " + newhexColor + " to bulb #" + bridgebulbID + "/50, RGB hex=" + newhexColor + ", bridgecommunicationlastTimestamp=" + bridgecommunicationlastTimestamp + "ms, current=" + DateTime.Now.Millisecond + ", delta = " + bridgecommunicationDelta + "ms");
                                }


                            }

                            //skip ahead to start of next rgb input
                            i += 2;
                        }

                        //check if we should check bridge ping and recalibrate
                        if (bridgecommunicationpingDelta >= bridgecommunicationPing) {
                            bridgecommunicationlastpingTimestamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                            CheckConnection();
                        }
                    }
                    //Log.Write("PhilipsHueController.SendPhilipsHueControllerUpdate END");
                }

            }


            public void ShutdownLighting() {
                //Log.Write("PhilipsHueController.ShutdownLighting");
                int currentlightID = 0;
                for (int i=0; i<affectedLights.Length; i++) { 
                    if (affectedLights[i] == true) {
                        currentlightID = i + 1;
                        LightCommand newbridgeCommand = new LightCommand();
                        newbridgeCommand.TurnOff();
                        newbridgeCommand.On = false;
                        newbridgeCommand.TransitionTime = TimeSpan.FromMilliseconds(2000);
                        Log.Write("PhilipsHueController.ShutdownLighting, Turning off light #" + currentlightID);
                        hueClient.SendCommandAsync(newbridgeCommand, new List<string> { currentlightID.ToString() });
                    }
                }

                LastStateSent.Fill(false);
            }

            private void ResetFadeTime() {
                Log.Write("PhilipsHueController.ResetFadeTime");
                //PDSingletonn.PacLed64SetLEDFadeTime(Index, 0);
            }



            private bool IsPresent {
                get {
                    if (!Id.IsBetween(0, 3) || BridgeConnected == false) return false;
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
            /// Connects to bridge.
            /// </summary>
            public void ConnectUnit() {

                if (BridgeIP == "" || BridgeKey == "") {
                    Log.Write("PhilipsHueController.PhilipsHueControllerUnit... unable to connect to bridge, no IP or key supplied in Cabinet.xml");
                } else {
                    Log.Write("PhilipsHueController.PhilipsHueControllerUnit... connecting to bridge... ip=" + BridgeIP + " key=" + BridgeKey);
                    hueClient = new LocalHueClient(BridgeIP);
                    hueClient.Initialize(BridgeKey);

                    //now check connection
                    CheckConnection();
                }
                

                
            }

            /// <summary>
            /// Checks connection and adjusts communication delay dynamically.
            /// </summary>
            /// <returns></returns>
            public async Task CheckConnection() {
                long startTimestamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
                long deltaTimestamp = 0;
                long adjustedPing = 0;
                bool connectionResult = await hueClient.CheckConnection();

                //update connection status
                BridgeConnected = connectionResult;

                if (connectionResult == true) {
                    deltaTimestamp = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - startTimestamp;
                    adjustedPing = (int)Math.Round((int)deltaTimestamp * bridgecommunicationdelayFactor);

                    if (adjustedPing > bridgecommunicationminimumDelay) {
                        bridgecommunicationDelay = (int)adjustedPing;
                        Log.Write("PhilipsHueController.CheckConnection IP="+BridgeIP+"... current connection lag =" + deltaTimestamp + "ms, recalibrating from " + bridgecommunicationDelay + "ms -> " + deltaTimestamp + "ms, buffer factor=" + bridgecommunicationdelayFactor + " -> " + adjustedPing + "ms");
                    } else {
                        bridgecommunicationDelay = bridgecommunicationminimumDelay;
                        Log.Write("PhilipsHueController.CheckConnection IP=" + BridgeIP + "... current connection lag =" + deltaTimestamp + "ms, ignoring and using minimum " + bridgecommunicationminimumDelay + "ms to avoid bridge overload");
                    }
                } else {
                    Log.Write("PhilipsHueController.CheckConnection... lost connection, or unable to connect to bridge using BridgeIP=" + BridgeIP+ " and BridgeKey=" + BridgeKey+"... will ignore for now and attempt reconnecting every "+bridgecommunicationPing+"ms");
                }

                
                
            }

            public PhilipsHueControllerUnit(int Id) {
                this.Id = Id;
                this.Index = Id;

                NewValue.Fill((byte)0);

                //assume all needed lights are false / off by default
                affectedLights.Fill(false);
                InitUnit();
                //ConnectUnit();
            }


        }



        #endregion




    }
}
