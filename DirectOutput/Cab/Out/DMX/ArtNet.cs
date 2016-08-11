using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Out.DMX.ArtnetEngine;

namespace DirectOutput.Cab.Out.DMX
{

    /// <summary>
    /// Artnet is a industry standard protocol used to control <a target="_blank" href="https://en.wikipedia.org/wiki/DMX512">DMX</a> lighting effects over ethernet. Using <a target="_blank" href="https://en.wikipedia.org/wiki/Art-Net">Art-Net</a> it is possible to connect a very wide range of lighting effects like <a target="_blank" href="https://www.google.ch/search?q=dmx+strobe">strobes</a> or <a target="_blank" href="https://www.google.ch/search?q=dmx+dimmer">dimmer packs</a>. There are tons of DMX controlled effects available on the market (from very cheap and small to very expensive and big). It might sounds a bit crazy, but with Art-net and DMX you could at least in theory control a whole stage lighting system (this would likely make you feel like Tommy in the movie).
    /// 
    /// To use Art-Net you will need a Art-Net node (unit that converts from ethernet to DMX protocol) and also some DMX controlled lighting effect. There are quite a few different Art-Net nodes available on the market and most of them should be compatible with the DirectOutput framework. For testing the Art-Net node <a target="_blank" href="http://www.ulrichradig.de/home/index.php/avr/dmx-avr-artnetnode">sold by Ulrich Radig</a> as a DIY kit was used. 
    /// 
    /// Each Art-Net node/DMX universe supports 512 DMX channels and several Art-Net nodes controlling different DMX universes can be used in parallel.
    /// 
    /// If you want to read more about Art-net, visit the website of <a href="http://www.artisticlicence.com">Artistic License</a>. The specs for Art-net can be found in the Resources - User Guides + Datasheets section of the site.
    /// 
    /// \image html DMX.png DMX
    /// </summary>
    public class ArtNet : OutputControllerCompleteBase
    {

        private Engine Engine = null;


        #region Universe property of type short with events
        #region Universe property core parts
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
            set
            {
                if (_Universe != value)
                {
                    OnUniverseChanging();
                    _Universe = value;
                    OnUniverseChanged();
                }
            }
        }

        /// <summary>
        /// Fires when the Universe property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> UniverseChanging;

        /// <summary>
        /// Fires when the Universe property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> UniverseChanged;
        #endregion

        /// <summary>
        /// Is called when the Universe property is about to change its value and fires the UniverseChanging event
        /// </summary>
        protected void OnUniverseChanging()
        {
            if (UniverseChanging != null) UniverseChanging(this, new EventArgs());

            //Insert more logic to execute before the Universe property changes here
        }

        /// <summary>
        /// Is called when the Universe property has changed its value and fires the UniverseChanged event
        /// </summary>
        protected void OnUniverseChanged()
        {
            //Insert more logic to execute after the Universe property has changed here
            OnPropertyChanged("Universe");
            if (UniverseChanged != null) UniverseChanged(this, new EventArgs());
        }

        #endregion


        #region BroadcastAddress property of type string with events
        #region BroadcastAddress property core parts
        private string _BroadcastAddress = "";

        /// <summary>
        /// Gets or sets the broadcast address for the ArtNet object.<br/>
        /// If no BroadcastAddress is set, the default address (255.255.255.255) will be used.
        /// </summary>
        /// <value>
        /// String containing broadcast address. If this parameter is not set the default broadcast address (255.255.255.255) will be used.
        /// Valid values are any IP adresses (e.g. 192.168.1.53).
        /// </value>
        public string BroadcastAddress
        {
            get { return _BroadcastAddress; }
            set
            {
                if (_BroadcastAddress != value)
                {
                    OnBroadcastAddressChanging();
                    _BroadcastAddress = value;
                    OnBroadcastAddressChanged();
                }
            }
        }

        /// <summary>
        /// Fires when the BroadcastAddress property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> BroadcastAddressChanging;

        /// <summary>
        /// Fires when the BroadcastAddress property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> BroadcastAddressChanged;
        #endregion

        /// <summary>
        /// Is called when the BroadcastAddress property is about to change its value and fires the BroadcastAddressChanging event
        /// </summary>
        protected void OnBroadcastAddressChanging()
        {
            if (BroadcastAddressChanging != null) BroadcastAddressChanging(this, new EventArgs());

            //Insert more logic to execute before the BroadcastAddress property changes here
        }

        /// <summary>
        /// Is called when the BroadcastAddress property has changed its value and fires the BroadcastAddressChanged event
        /// </summary>
        protected void OnBroadcastAddressChanged()
        {
            //Insert more logic to execute after the BroadcastAddress property has changed here
            OnPropertyChanged("BroadcastAddress");
            if (BroadcastAddressChanged != null) BroadcastAddressChanged(this, new EventArgs());
        }

        #endregion

        protected override int GetNumberOfConfiguredOutputs()
        {
            return 512;
        }

        protected override bool VerifySettings()
        {
            return true;
        }

        protected override void UpdateOutputs(byte[] OutputValues)
        {
            try
            {
                if (Engine != null)
                {
                    if (OutputValues.Length == 512)
                    {
                        Engine.SendDMX(BroadcastAddress, Universe, OutputValues, 512);
                    }
                    else
                    {
                        Log.Exception("{0} {1} sent the wrong number of bytes to output.".Build(this.GetType().Name, Name));
                        throw new Exception("{0} {1} sent the wrong number of bytes to output.".Build(this.GetType().Name, Name));
                    }
                }
                else
                {
                    string Msg = "{0} {1} (Universe: {2}, Broadcast Address: {3}) is not connected.".Build(new object[] { this.GetType().Name, Name, Universe, BroadcastAddress });
                    Log.Exception(Msg);
                    throw new Exception(Msg);
                }

            }
            catch (Exception E)
            {
                string Msg = "{0} {1} (Universe: {2}, Broadcast Address: {3}) could not send data: {4}".Build(new object[] { this.GetType().Name, Name, Universe, BroadcastAddress, E.Message });
                Log.Exception(Msg, E);
                throw new Exception(Msg, E);
            }
        }

        protected override void ConnectToController()
        {
            if (Engine == null)
            {

                try
                {
                    Engine = Engine.Instance;
                    Engine.SendDMX(BroadcastAddress, Universe, new byte[512], 512);
                }
                catch (Exception E)
                {
                    Engine = null;
                    string Msg = "{0} {1} (Universe: {2}, Broadcast Address: {3}) could not connect: {4}".Build(new object[] { this.GetType().Name, Name, Universe, BroadcastAddress, E.Message });
                    Log.Exception(Msg, E);
                    throw new Exception(Msg, E);
                }
            }
        }

        protected override void DisconnectFromController()
        {
            try
            {
                Engine.SendDMX(BroadcastAddress, Universe, new byte[512], 512);
            }
            catch { }

            if (Engine != null)
            {
                Engine = null;
            }
        }
    }
}
