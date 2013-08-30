using System;
using System.Threading;
using DirectOutput.Cab.Out.DMX.ArtnetEngine;
using System.Xml.Serialization;


namespace DirectOutput.Cab.Out.DMX
{
    /// <summary>
    /// OutputController implementation for ArtNet (DMX via ethernet) nodes.<br/>
    /// Information on ArtNet can be found on http://en.wikipedia.org/wiki/Art-Net. <br/>
    /// Information on DMX can be found on http://en.wikipedia.org/wiki/DMX512.
    /// </summary>
    public class ArtNet : OutputControllerBase, IOutputController
    {

        private Engine Engine = null;
        private byte[] DMXData = new byte[512];
        private int LastDMXChannel = 0;
        private bool UpdateRequired = true;
        private object UpdateLocker = new object();

        private short _Universe = 0;

        /// <summary>
        /// Gets or sets the number of the Dmx universe for the ArtNet node.
        /// </summary>
        /// <value>
        /// The number of the Dmx universe.
        /// </value>
        public short Universe
        {
            get { return _Universe; }
            set { _Universe = value; }
        }

        private string _BroadcastAddress="";

        /// <summary>
        /// Gets or sets the broadcast address for the ArtNet object.<br/>
        /// If no BroadcastAddress is set, the default address (255.255.255.255) will be used.
        /// </summary>
        /// <value>
        /// String containing broadcast address.
        /// </value>
        public string BroadcastAddress
        {
            get { return _BroadcastAddress; }
            set { _BroadcastAddress = value; }
        }



        private OutputList _Outputs = new OutputList();
        /// <summary>
        /// OutputList containing the DMXOutput objects for the Artnet node.
        /// </summary>
        [XmlIgnoreAttribute]
        public new OutputList Outputs
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

        private void Outputs_OutputValueChanged(object sender, OutputEventArgs e)
        {
            if (!(e.Output is DMXOutput))
            {
                throw new Exception("The OutputValueChanged event handler for ArtNet node {0} (controlling Dmx universe {1}) has been called by a sender which is not a DmxOutput.".Build(Name,Universe));
            }

            DMXOutput O = (DMXOutput)e.Output;

            if (!O.DmxChannel.IsBetween(1, 512))
            {
                Log.Exception("ArtNet node {0} has received a update for a illegal dmx channel number ({1}).".Build(Name, O.DmxChannel));
                throw new ArgumentOutOfRangeException("ArtNet node {0} has received a update for a illegal dmx channel number ({1}).".Build(Name, O.DmxChannel));

            }

            lock (UpdateLocker)
            {
                if (DMXData[O.DmxChannel-1] != O.Value)
                {
                    DMXData[O.DmxChannel-1] = O.Value;
                    if (O.DmxChannel > LastDMXChannel)
                    {
                        LastDMXChannel = O.DmxChannel;
                    }
                    UpdateRequired = true;
                }
            }
        }

        /// <summary>
        /// Update triggers sending of the DMX data to the physical ArtNet node.<br/>
        /// If no updates are required (no channel values have changed) no data will be sent..
        /// </summary>
        public override void Update()
        {
            if (UpdateRequired)
            {
                UpdaterThreadSignal();
            }
        }

        /// <summary>
        /// Initializes the Artnet object.<br />
        /// Adds the output objects to the outputcollection of the ArtNet instance and starts the updater thread.
        /// </summary>
        /// <param name="Cabinet">The cabinet object which is using the ArtNet instance.</param>
        public override void Init(Cabinet  Cabinet)
        {
            if (Outputs.Count == 0)
            {
                for (int i = 1; i <= 512; i++)
                {
                    Outputs.Add(new DMXOutput() { DmxChannel = i, Name="{0}.{1:000}".Build(this.Name,i) });
                }
            }

   
            InitUpdaterThread();

            Log.Write("ArtNet node {0} (controlling universe {1}) initialized and updater thread started.".Build(Name,Universe));
        }

        /// <summary>
        /// Finishes the ArtNet object
        /// </summary>
        public override void Finish()
        {
            FinishUpdaterThread();
            Log.Write("ArtNet node {0} (controlling universe {1}) finished and updater thread stopped.".Build(Name, Universe));

        }



        #region UpdaterThread
        /// <summary>
        /// Inits the updater thread.
        /// </summary>
        /// <exception cref="System.Exception">Artnet node {0} updater thread could not start.</exception>
        private void InitUpdaterThread()
        {

            if (!UpdaterThreadIsActive)
            {
                KeepUpdaterThreadAlive = true;
                try
                {
                    UpdaterThread = new Thread(MainThreadDoIt);
                    UpdaterThread.Name = "ArtNet node {0} updater thread ".Build(Name);
                    UpdaterThread.Start();
                }
                catch (Exception E)
                {
                    Log.Exception("Artnet node {0} updater thread could not start.".Build(Name), E);
                    throw new Exception("Artnet node {0} updater thread could not start.".Build(Name), E);
                }
            }
        }

        /// <summary>
        /// Finishes the updater thread.
        /// </summary>
        /// <exception cref="System.Exception">A error occured during termination of ArtNet updater thread.</exception>
        private void FinishUpdaterThread()
        {
            if (UpdaterThread != null)
            {
                try
                {
                    KeepUpdaterThreadAlive = false;
                    lock (UpdaterThreadLocker)
                    {
                        Monitor.Pulse(UpdaterThreadLocker);
                    }
                    if (!UpdaterThread.Join(1000))
                    {
                        UpdaterThread.Abort();
                    }
                    UpdaterThread = null;
                }
                catch (Exception E)
                {
                    Log.Exception("A error occured during termination of ArtNet updater thread.", E);
                    throw new Exception("A error occured during termination of ArtNet updater thread.", E);
                }
            }
        }


        /// <summary>
        /// Indicates whether the UpdaterThread of the Artnet instance is active or not.
        /// </summary>
        public bool UpdaterThreadIsActive
        {
            get
            {
                if (UpdaterThread != null)
                {
                    if (UpdaterThread.IsAlive)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Signals the updater thread to continue its work (if currently sleeping).
        /// </summary>
        private void UpdaterThreadSignal()
        {

            lock (UpdaterThreadLocker)
            {
                Monitor.Pulse(UpdaterThreadLocker);
            }
        }


        private Thread UpdaterThread { get; set; }
        private object UpdaterThreadLocker = new object();
        private bool KeepUpdaterThreadAlive = true;




        /// <summary>
        /// This is the main method of the ArtNet object updater thread.
        /// </summary>
        private void MainThreadDoIt()
        {
            if (Engine == null)
            {
                Engine = new Engine(Name, BroadcastAddress);
                
            }

            //Send all channels to ensure that there are defined values
            Engine.SendDMX(Universe, DMXData, 512);

            while (KeepUpdaterThreadAlive)
            {

                if (UpdateRequired)
                {
                    lock (UpdateLocker)
                    {
                        UpdateRequired = false;
                        Engine.SendDMX(Universe, DMXData, ((LastDMXChannel | 1) + 1).Limit(2, 512));
                    }

                }

                if (KeepUpdaterThreadAlive)
                {
                    lock (UpdaterThreadLocker)
                    {
                        while (UpdateRequired == false && KeepUpdaterThreadAlive)
                        {
                            Monitor.Wait(UpdaterThreadLocker, 50);  // Lock is released while we’re waiting
                        }
                    }
                }

            }
            if (Engine != null)
            {
               
                Engine = null;
            }

        }
        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="ArtNet"/> class.
        /// </summary>
        public ArtNet() {
            Outputs.OutputValueChanged += new OutputList.OutputValueChangedEventHandler(Outputs_OutputValueChanged);
        }



    }
}
