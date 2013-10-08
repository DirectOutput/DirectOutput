using System;
using System.Threading;
using DirectOutput.Cab.Out.DMX.ArtnetEngine;
using System.Xml.Serialization;
using System.Linq;

namespace DirectOutput.Cab.Out.DMX
{
    /// <summary>
    /// <a target="_blank" href="https://en.wikipedia.org/wiki/Art-Net">Art-Net</a> is a industry standard protocol used to control <a target="_blank" href="https://en.wikipedia.org/wiki/DMX512">DMX</a> lighting effects over othernet. Using Art-Net it is possible to connect a very wide range of lighting effects like <a target="_blank" href="https://www.google.ch/search?q=dmx+strobe">strobes</a> or <a target="_blank" href="https://www.google.ch/search?q=dmx+dimmer">dimmer packs</a>. There are tons of DMX controlled effects available on the market (from very cheap and small to very expensive and big). It might sounds a bit crazy, but with Art-net and DMX you could at keast in theory control a whole stage lighting system (this would likely make you feel like Tommy in the movie).
    /// 
    /// \image html DMX.png
    /// 
    /// To use Art-Net you will need a Art-Net node (unit that converts from ethernet to DMX protocol) and also some DMX controlled lighting effect. There are quite a few different Art-Net nodes available on the market and most of them should be compatible with the DirectOutput framework. For testing the Art-Net node sold on http://www.ulrichradig.de/home/index.php/avr/dmx-avr-artnetnode as a DIY kit was used. 
    /// 
    /// Each Art-Net node/DMX universe supports 512 DMX channels and several Art-Net nodes controlling different DMX universes can be used in parallel.
    /// 
    /// If you want to read more about Art-net, visit the website of <a href="http://www.artisticlicence.com">Artistic License</a>. The specs for Art-net can be found in the Resources -> User Guides & Datasheets section of the site.
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



        private void AddOutputs()
        {
            for (int i = 1; i <= 512; i++)
            {
                if (!Outputs.Any(x => ((DMXOutput)x).DmxChannel == i))
                {
                    Outputs.Add(new DMXOutput() { Name = "{0}.{1:000}".Build(Name, i), DmxChannel=i });
                }
            }
        }

        /// <summary>
        /// This method is called whenever the value of a output in the Outputs property changes its value.<br />
        /// It updates the internal arry holding the values for the DMX channels of the universe specified.
        /// </summary>
        /// <param name="Output">The output.</param>
        /// <exception cref="System.Exception">The OutputValueChanged event handler for ArtNet node {0} (controlling Dmx universe {1}) has been called by a sender which is not a DmxOutput..Build(Name, Universe)</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">ArtNet node {0} has received a update for a illegal dmx channel number ({1})..Build(Name, O.DmxChannel)</exception>
        public override void OnOutputValueChanged(IOutput Output)
        {
            if (!(Output is DMXOutput))
            {
                throw new Exception("The OutputValueChanged event handler for ArtNet node {0} (controlling Dmx universe {1}) has been called by a sender which is not a DmxOutput.".Build(Name, Universe));
            }

            DMXOutput O = (DMXOutput)Output;

            if (!O.DmxChannel.IsBetween(1, 512))
            {
                Log.Exception("ArtNet node {0} has received a update for a illegal dmx channel number ({1}).".Build(Name, O.DmxChannel));
                throw new ArgumentOutOfRangeException("ArtNet node {0} has received a update for a illegal dmx channel number ({1}).".Build(Name, O.DmxChannel));

            }

            lock (UpdateLocker)
            {
                if (DMXData[O.DmxChannel - 1] != O.Value)
                {
                    DMXData[O.DmxChannel - 1] = O.Value;
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
            AddOutputs();

   
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
            Outputs = new OutputList();

        }



    }
}
