using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace DirectOutput.Cab.Out.AdressableLedStrip
{
    /// <summary>
    /// The TeensyStripController is used to control up to 8 WS2811/WS2812 based ledstrips with up to 1100 leds per strip which are connected to a Teensy 3.1, 3.2 or later.
    /// 
    /// \image html TeensyOctoWS2811.jpg
    /// 
    /// The best place to get the hardware is probably the <a target="_blank" href="http://pjrc.com/teensy/">website</a> of the Teennsy inventor, where you can buy the <a target="_blank" href="http://pjrc.com/store/teensy32_pins.html">Teensy boards</a> (check if newer versions are available) and also a <a target="_blank" href="http://pjrc.com/store/octo28_adaptor.html">adapter board</a> which allows for easy connection of up to 8 led strips. There are also numerous other vendors of Teensy hardware (just ask Google).
    /// 
    /// The firmware for the Teensy based ledstrip controller is based on a slightly hacked version of Paul Stoffregens excellent OctoWS2811 LED Library which can easily drive up to 1100leds per channel on a Teensy 3.1 or later. More information on the OctoWS2811 lib can be found on the Teensy website, on Github and many other places on the internet.
    /// 
    /// Ready to use, compiled firmware files can be downloaded from the <a target="_blank" href="http://github.com/DirectOutput/TeensyStripController/releases/">Github page</a> for the TeensyStripController, the source code for the firmware is available on Github as well.
    /// 
    /// </summary>
    public class TeensyStripController : OutputControllerCompleteBase
    {
        private int[] NumberOfLedsPerStrip = new int[8];


        #region NumberOfLedsStrip1 property of type int with events
        #region NumberOfLedsStrip1 property core parts


        /// <summary>
        ///  NumberOfLedsStrip1 property of type int
        /// </summary>
        public int NumberOfLedsStrip1
        {
            get { return NumberOfLedsPerStrip[0]; }
            set
            {
                if (NumberOfLedsPerStrip[0] != value)
                {
                    OnNumberOfLedsStrip1Changing();
                    NumberOfLedsPerStrip[0] = value;
                    base.SetupOutputs();
                    OnNumberOfLedsStrip1Changed();
                }
            }
        }

        /// <summary>
        /// Fires when the NumberOfLedsStrip1 property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip1Changing;

        /// <summary>
        /// Fires when the NumberOfLedsStrip1 property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip1Changed;
        #endregion

        /// <summary>
        /// Is called when the NumberOfLedsStrip1 property is about to change its value and fires the NumberOfLedsStrip1Changing event
        /// </summary>
        protected void OnNumberOfLedsStrip1Changing()
        {
            if (NumberOfLedsStrip1Changing != null) NumberOfLedsStrip1Changing(this, new EventArgs());

            //Insert more logic to execute before the NumberOfLedsStrip1 property changes here
        }

        /// <summary>
        /// Is called when the NumberOfLedsStrip1 property has changed its value and fires the NumberOfLedsStrip1Changed event
        /// </summary>
        protected void OnNumberOfLedsStrip1Changed()
        {
            //Insert more logic to execute after the NumberOfLedsStrip1 property has changed here
            OnPropertyChanged("NumberOfLedsStrip1");
            if (NumberOfLedsStrip1Changed != null) NumberOfLedsStrip1Changed(this, new EventArgs());
        }

        #endregion


        #region NumberOfLedsStrip2 property of type int with events
        #region NumberOfLedsStrip2 property core parts


        /// <summary>
        ///  NumberOfLedsStrip2 property of type int
        /// </summary>
        public int NumberOfLedsStrip2
        {
            get { return NumberOfLedsPerStrip[1]; }
            set
            {
                if (NumberOfLedsPerStrip[1] != value)
                {
                    OnNumberOfLedsStrip2Changing();
                    NumberOfLedsPerStrip[1] = value;
                    base.SetupOutputs();
                    OnNumberOfLedsStrip2Changed();
                }
            }
        }

        /// <summary>
        /// Fires when the NumberOfLedsStrip2 property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip2Changing;

        /// <summary>
        /// Fires when the NumberOfLedsStrip2 property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip2Changed;
        #endregion

        /// <summary>
        /// Is called when the NumberOfLedsStrip2 property is about to change its value and fires the NumberOfLedsStrip2Changing event
        /// </summary>
        protected void OnNumberOfLedsStrip2Changing()
        {
            if (NumberOfLedsStrip2Changing != null) NumberOfLedsStrip2Changing(this, new EventArgs());

            //Insert more logic to execute before the NumberOfLedsStrip2 property changes here
        }

        /// <summary>
        /// Is called when the NumberOfLedsStrip2 property has changed its value and fires the NumberOfLedsStrip2Changed event
        /// </summary>
        protected void OnNumberOfLedsStrip2Changed()
        {
            //Insert more logic to execute after the NumberOfLedsStrip2 property has changed here
            OnPropertyChanged("NumberOfLedsStrip2");
            if (NumberOfLedsStrip2Changed != null) NumberOfLedsStrip2Changed(this, new EventArgs());
        }

        #endregion

        #region NumberOfLedsStrip3 property of type int with events
        #region NumberOfLedsStrip3 property core parts


        /// <summary>
        ///  NumberOfLedsStrip3 property of type int
        /// </summary>
        public int NumberOfLedsStrip3
        {
            get { return NumberOfLedsPerStrip[2]; }
            set
            {
                if (NumberOfLedsPerStrip[2] != value)
                {
                    OnNumberOfLedsStrip3Changing();
                    NumberOfLedsPerStrip[2] = value;
                    base.SetupOutputs();
                    OnNumberOfLedsStrip3Changed();
                }
            }
        }

        /// <summary>
        /// Fires when the NumberOfLedsStrip3 property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip3Changing;

        /// <summary>
        /// Fires when the NumberOfLedsStrip3 property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip3Changed;
        #endregion

        /// <summary>
        /// Is called when the NumberOfLedsStrip3 property is about to change its value and fires the NumberOfLedsStrip3Changing event
        /// </summary>
        protected void OnNumberOfLedsStrip3Changing()
        {
            if (NumberOfLedsStrip3Changing != null) NumberOfLedsStrip3Changing(this, new EventArgs());

            //Insert more logic to execute before the NumberOfLedsStrip3 property changes here
        }

        /// <summary>
        /// Is called when the NumberOfLedsStrip3 property has changed its value and fires the NumberOfLedsStrip3Changed event
        /// </summary>
        protected void OnNumberOfLedsStrip3Changed()
        {
            //Insert more logic to execute after the NumberOfLedsStrip3 property has changed here
            OnPropertyChanged("NumberOfLedsStrip3");
            if (NumberOfLedsStrip3Changed != null) NumberOfLedsStrip3Changed(this, new EventArgs());
        }

        #endregion

        #region NumberOfLedsStrip4 property of type int with events
        #region NumberOfLedsStrip4 property core parts


        /// <summary>
        ///  NumberOfLedsStrip4 property of type int
        /// </summary>
        public int NumberOfLedsStrip4
        {
            get { return NumberOfLedsPerStrip[3]; }
            set
            {
                if (NumberOfLedsPerStrip[3] != value)
                {
                    OnNumberOfLedsStrip4Changing();
                    NumberOfLedsPerStrip[3] = value;
                    base.SetupOutputs();
                    OnNumberOfLedsStrip4Changed();
                }
            }
        }

        /// <summary>
        /// Fires when the NumberOfLedsStrip4 property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip4Changing;

        /// <summary>
        /// Fires when the NumberOfLedsStrip4 property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip4Changed;
        #endregion

        /// <summary>
        /// Is called when the NumberOfLedsStrip4 property is about to change its value and fires the NumberOfLedsStrip4Changing event
        /// </summary>
        protected void OnNumberOfLedsStrip4Changing()
        {
            if (NumberOfLedsStrip4Changing != null) NumberOfLedsStrip4Changing(this, new EventArgs());

            //Insert more logic to execute before the NumberOfLedsStrip4 property changes here
        }

        /// <summary>
        /// Is called when the NumberOfLedsStrip4 property has changed its value and fires the NumberOfLedsStrip4Changed event
        /// </summary>
        protected void OnNumberOfLedsStrip4Changed()
        {
            //Insert more logic to execute after the NumberOfLedsStrip4 property has changed here
            OnPropertyChanged("NumberOfLedsStrip4");
            if (NumberOfLedsStrip4Changed != null) NumberOfLedsStrip4Changed(this, new EventArgs());
        }

        #endregion

        #region NumberOfLedsStrip5 property of type int with events
        #region NumberOfLedsStrip5 property core parts


        /// <summary>
        ///  NumberOfLedsStrip5 property of type int
        /// </summary>
        public int NumberOfLedsStrip5
        {
            get { return NumberOfLedsPerStrip[4]; }
            set
            {
                if (NumberOfLedsPerStrip[4] != value)
                {
                    OnNumberOfLedsStrip5Changing();
                    NumberOfLedsPerStrip[4] = value;
                    base.SetupOutputs();
                    OnNumberOfLedsStrip5Changed();
                }
            }
        }

        /// <summary>
        /// Fires when the NumberOfLedsStrip5 property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip5Changing;

        /// <summary>
        /// Fires when the NumberOfLedsStrip5 property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip5Changed;
        #endregion

        /// <summary>
        /// Is called when the NumberOfLedsStrip5 property is about to change its value and fires the NumberOfLedsStrip5Changing event
        /// </summary>
        protected void OnNumberOfLedsStrip5Changing()
        {
            if (NumberOfLedsStrip5Changing != null) NumberOfLedsStrip5Changing(this, new EventArgs());

            //Insert more logic to execute before the NumberOfLedsStrip5 property changes here
        }

        /// <summary>
        /// Is called when the NumberOfLedsStrip5 property has changed its value and fires the NumberOfLedsStrip5Changed event
        /// </summary>
        protected void OnNumberOfLedsStrip5Changed()
        {
            //Insert more logic to execute after the NumberOfLedsStrip5 property has changed here
            OnPropertyChanged("NumberOfLedsStrip5");
            if (NumberOfLedsStrip5Changed != null) NumberOfLedsStrip5Changed(this, new EventArgs());
        }

        #endregion

        #region NumberOfLedsStrip6 property of type int with events
        #region NumberOfLedsStrip6 property core parts


        /// <summary>
        ///  NumberOfLedsStrip6 property of type int
        /// </summary>
        public int NumberOfLedsStrip6
        {
            get { return NumberOfLedsPerStrip[5]; }
            set
            {
                if (NumberOfLedsPerStrip[5] != value)
                {
                    OnNumberOfLedsStrip6Changing();
                    NumberOfLedsPerStrip[5] = value;
                    base.SetupOutputs();
                    OnNumberOfLedsStrip6Changed();
                }
            }
        }

        /// <summary>
        /// Fires when the NumberOfLedsStrip6 property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip6Changing;

        /// <summary>
        /// Fires when the NumberOfLedsStrip6 property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip6Changed;
        #endregion

        /// <summary>
        /// Is called when the NumberOfLedsStrip6 property is about to change its value and fires the NumberOfLedsStrip6Changing event
        /// </summary>
        protected void OnNumberOfLedsStrip6Changing()
        {
            if (NumberOfLedsStrip6Changing != null) NumberOfLedsStrip6Changing(this, new EventArgs());

            //Insert more logic to execute before the NumberOfLedsStrip6 property changes here
        }

        /// <summary>
        /// Is called when the NumberOfLedsStrip6 property has changed its value and fires the NumberOfLedsStrip6Changed event
        /// </summary>
        protected void OnNumberOfLedsStrip6Changed()
        {
            //Insert more logic to execute after the NumberOfLedsStrip6 property has changed here
            OnPropertyChanged("NumberOfLedsStrip6");
            if (NumberOfLedsStrip6Changed != null) NumberOfLedsStrip6Changed(this, new EventArgs());
        }

        #endregion


        #region NumberOfLedsStrip7 property of type int with events
        #region NumberOfLedsStrip7 property core parts


        /// <summary>
        ///  NumberOfLedsStrip7 property of type int
        /// </summary>
        public int NumberOfLedsStrip7
        {
            get { return NumberOfLedsPerStrip[6]; }
            set
            {
                if (NumberOfLedsPerStrip[6] != value)
                {
                    OnNumberOfLedsStrip7Changing();
                    NumberOfLedsPerStrip[6] = value;
                    base.SetupOutputs();
                    OnNumberOfLedsStrip7Changed();
                }
            }
        }

        /// <summary>
        /// Fires when the NumberOfLedsStrip7 property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip7Changing;

        /// <summary>
        /// Fires when the NumberOfLedsStrip7 property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip7Changed;
        #endregion

        /// <summary>
        /// Is called when the NumberOfLedsStrip7 property is about to change its value and fires the NumberOfLedsStrip7Changing event
        /// </summary>
        protected void OnNumberOfLedsStrip7Changing()
        {
            if (NumberOfLedsStrip7Changing != null) NumberOfLedsStrip7Changing(this, new EventArgs());

            //Insert more logic to execute before the NumberOfLedsStrip7 property changes here
        }

        /// <summary>
        /// Is called when the NumberOfLedsStrip7 property has changed its value and fires the NumberOfLedsStrip7Changed event
        /// </summary>
        protected void OnNumberOfLedsStrip7Changed()
        {
            //Insert more logic to execute after the NumberOfLedsStrip7 property has changed here
            OnPropertyChanged("NumberOfLedsStrip7");
            if (NumberOfLedsStrip7Changed != null) NumberOfLedsStrip7Changed(this, new EventArgs());
        }

        #endregion

        #region NumberOfLedsStrip8 property of type int with events
        #region NumberOfLedsStrip8 property core parts


        /// <summary>
        ///  NumberOfLedsStrip8 property of type int
        /// </summary>
        public int NumberOfLedsStrip8
        {
            get { return NumberOfLedsPerStrip[7]; }
            set
            {
                if (NumberOfLedsPerStrip[7] != value)
                {
                    OnNumberOfLedsStrip8Changing();
                    NumberOfLedsPerStrip[7] = value;
                    base.SetupOutputs();
                    OnNumberOfLedsStrip8Changed();
                }
            }
        }

        /// <summary>
        /// Fires when the NumberOfLedsStrip8 property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip8Changing;

        /// <summary>
        /// Fires when the NumberOfLedsStrip8 property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> NumberOfLedsStrip8Changed;
        #endregion

        /// <summary>
        /// Is called when the NumberOfLedsStrip8 property is about to change its value and fires the NumberOfLedsStrip8Changing event
        /// </summary>
        protected void OnNumberOfLedsStrip8Changing()
        {
            if (NumberOfLedsStrip8Changing != null) NumberOfLedsStrip8Changing(this, new EventArgs());

            //Insert more logic to execute before the NumberOfLedsStrip8 property changes here
        }

        /// <summary>
        /// Is called when the NumberOfLedsStrip8 property has changed its value and fires the NumberOfLedsStrip8Changed event
        /// </summary>
        protected void OnNumberOfLedsStrip8Changed()
        {
            //Insert more logic to execute after the NumberOfLedsStrip8 property has changed here
            OnPropertyChanged("NumberOfLedsStrip8");
            if (NumberOfLedsStrip8Changed != null) NumberOfLedsStrip8Changed(this, new EventArgs());
        }

        #endregion


        #region ComPortName property of type string with events
        #region ComPortName property core parts
        private string _ComPortName = null;

        /// <summary>
        /// Gets or sets the name (typicaly COM{Number}) of the virtual Com port the Teensy board is using.
        /// </summary>
        /// <value>
        /// The name of the Com port (typicaly COM{Number}) the Teensy board is using.
        /// </value>
        public string ComPortName
        {
            get { return _ComPortName; }
            set
            {
                if (_ComPortName != value)
                {
                    OnComPortNameChanging();
                    _ComPortName = value;
                    OnComPortNameChanged();
                }
            }
        }

        /// <summary>
        /// Fires when the ComPortName property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> ComPortNameChanging;

        /// <summary>
        /// Fires when the ComPortName property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> ComPortNameChanged;
        #endregion

        /// <summary>
        /// Is called when the ComPortName property is about to change its value and fires the ComPortNameChanging event
        /// </summary>
        protected void OnComPortNameChanging()
        {
            if (ComPortNameChanging != null) ComPortNameChanging(this, new EventArgs());

            //Insert more logic to execute before the ComPortName property changes here
        }

        /// <summary>
        /// Is called when the ComPortName property has changed its value and fires the ComPortNameChanged event
        /// </summary>
        protected void OnComPortNameChanged()
        {
            //Insert more logic to execute after the ComPortName property has changed here
            OnPropertyChanged("ComPortName");
            if (ComPortNameChanged != null) ComPortNameChanged(this, new EventArgs());
        }

        #endregion





        #region ComPortTimeOutMs property of type int with events
        #region ComPortTimeOutMs property core parts
        private int _ComPortTimeOutMs = 200;

        /// <summary>
        /// Gets or sets the COM port timeout in milliseconds.
        /// This properties accepts values between 1 and 5000 milliseconds (default 200ms). If a value outside this range is specified, the properties value reverts to the default value of 200ms.
        /// </summary>
        /// <value>
        /// The COM port timeout in milliseconds (Valid range 1-5000ms, default: 200ms).
        /// </value>
        public int ComPortTimeOutMs
        {
            get { return _ComPortTimeOutMs; }
            set
            {
                if (value.IsBetween(1, 5000))
                {
                    if (_ComPortTimeOutMs != value)
                    {
                        OnComPortTimeOutMsChanging();
                        _ComPortTimeOutMs = value;
                        OnComPortTimeOutMsChanged();
                    }
                }
                else
                {
                    Log.Warning("The specified value {0} for the ComPortTimeOutMs is outside the valid range of 1 to 5000. Will use the default value of 200ms.".Build(value));
                    if (_ComPortTimeOutMs != 200)
                    {
                        OnComPortTimeOutMsChanging();
                        _ComPortTimeOutMs = 200;
                        OnComPortTimeOutMsChanged();
                    }
                }

       
            }
        }

        /// <summary>
        /// Fires when the ComPortTimeOutMs property is about to change its value
        /// </summary>
        public event EventHandler<EventArgs> ComPortTimeOutMsChanging;

        /// <summary>
        /// Fires when the ComPortTimeOutMs property has changed its value
        /// </summary>
        public event EventHandler<EventArgs> ComPortTimeOutMsChanged;
        #endregion

        /// <summary>
        /// Is called when the ComPortTimeOutMs property is about to change its value and fires the ComPortTimeOutMsChanging event
        /// </summary>
        protected void OnComPortTimeOutMsChanging()
        {
            if (ComPortTimeOutMsChanging != null) ComPortTimeOutMsChanging(this, new EventArgs());

            //Insert more logic to execute before the ComPortTimeOutMs property changes here
        }

        /// <summary>
        /// Is called when the ComPortTimeOutMs property has changed its value and fires the ComPortTimeOutMsChanged event
        /// </summary>
        protected void OnComPortTimeOutMsChanged()
        {
            //Insert more logic to execute after the ComPortTimeOutMs property has changed here
            OnPropertyChanged("ComPortTimeOutMs");
            if (ComPortTimeOutMsChanged != null) ComPortTimeOutMsChanged(this, new EventArgs());
        }

        #endregion


        /// <summary>
        /// This method returns the sum of the number of leds configured for the 8 output channels of the Teensy board.
        /// </summary>
        /// <returns>
        /// The sum of the number of leds configured for the 8 output channels of the Teensy board.
        /// </returns>
        protected override int GetNumberOfConfiguredOutputs()
        {
            return NumberOfLedsPerStrip.Sum() * 3;
        }

        /// <summary>
        /// Verifies if a valid ComPortName has been set and if the number of outputs per channel is >=0.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the verification is OK, otherwise <c>false</c>
        /// </returns>
        protected override bool VerifySettings()
        {
            if (ComPortName.IsNullOrWhiteSpace())
            {
                Log.Warning("The ComPortName has not been specified");
                return false;
            }

            if (!SerialPort.GetPortNames().Any(PN => PN == ComPortName))
            {
                Log.Warning("The specified Com-Port {0} was not found. Available com-ports: {1}".Build(ComPortName, string.Join(", ", SerialPort.GetPortNames())));
                return false;
            }

            if (NumberOfLedsPerStrip.Any(Nr => Nr < 0))
            {
                Log.Warning("At least one ledstrip has a invalid number of leds specified (<0).");
                return false;
            }
            return true;
        }


        SerialPort ComPort = null;
        int NumberOfLedsPerChannel = -1;

        protected override void UpdateOutputs(byte[] OutputValues)
        {
            if (ComPort == null)
            {
                throw new Exception("Comport is not initialized");
            }
            byte[] CommandData;
            byte[] AnswerData;
            int BytesRead;
            int SourcePosition = 0;
            for (int i = 0; i < 8; i++)
            {
                int NrOfLedsOnStrip = NumberOfLedsPerStrip[i];
                if (NrOfLedsOnStrip > 0)
                {
                    int TargetPosition = i * NumberOfLedsPerChannel;
                    CommandData = new byte[5] { (byte)'R', (byte)(TargetPosition >> 8), (byte)(TargetPosition & 255), (byte)(NrOfLedsOnStrip >> 8), (byte)(NrOfLedsOnStrip & 255) };

                    ComPort.Write(CommandData, 0, 5);
                    ComPort.Write(OutputValues, SourcePosition * 3, NrOfLedsOnStrip * 3);

                    BytesRead = -1;

                    AnswerData = new byte[1];

                    try
                    {
                        BytesRead = ComPort.Read(AnswerData, 0, 1);
                    }
                    catch (Exception E)
                    {
                        throw new Exception("A exception occured while waiting for the ACK after sending the data for channel {0} of the TeensyStripController.".Build(i + 1), E);
                    }
                    if (BytesRead != 1 || AnswerData[0] != (byte)'A')
                    {
                        throw new Exception("Received no answer or a unexpected answer while waiting for the ACK after sending the data for channel {0} of the TeensyStripController.".Build(i + 1));
                    }
                    SourcePosition += NrOfLedsOnStrip;
                }
            }

            CommandData = new byte[1] { (byte)'O' };
            ComPort.Write(CommandData, 0, 1);

            BytesRead = -1;
            AnswerData = new byte[1];

            try
            {
                BytesRead = ComPort.Read(AnswerData, 0, 1);
            }
            catch (Exception E)
            {
                throw new Exception("A exception occured while waiting for the ACK after sending the output command (O) to the TeensyStripController", E);
            }
            if (BytesRead != 1 || AnswerData[0] != (byte)'A')
            {
                throw new Exception("Received no answer or a unexpected answer while waiting for the ACK after sending the output command (O) to the TeensyStripController");
            }


        }


        //private byte[] PackData(byte[] OutputValues, int FirstLed, int NumberOfLeds)
        //{
        //    int OutputPosition = FirstLed * 3;
        //    int LedNr = 0;

        //    if (NumberOfLeds > 0)
        //    {
        //        while (LedNr < NumberOfLeds)
        //        {
        //            if (OutputValues[OutputPosition] == OutputValues[OutputPosition + 3] && OutputValues[OutputPosition + 1] == OutputValues[OutputPosition + 4] && OutputValues[OutputPosition + 2] == OutputValues[OutputPosition + 5])
        //            {
        //                //
        //            }
        //            else
        //            {

        //            }




        //        }



        //    }
        //    else
        //    {
        //        //No data to pack
        //        return new byte[1] { 0 };
        //    }


        //}


        /// <summary>
        /// This method is called when DOF wants to connect to the controller.
        /// </summary>
        protected override void ConnectToController()
        {
            DisconnectFromController();

            string[] PortNames = SerialPort.GetPortNames();
            if (!PortNames.Any(PN => PN == ComPortName))
            {
                throw new Exception("The specified Com-Port '{0}' does not exist. Found the following Com-Ports: {1}. Will not send data to the controller.".Build(ComPortName, string.Join(", ", PortNames)));
            }

            ComPort = new SerialPort();
            ComPort.ReadTimeout = ComPortTimeOutMs;
            ComPort.WriteTimeout = ComPortTimeOutMs;

            try
            {
                ComPort.PortName = ComPortName;
            }
            catch (Exception E)
            {
                throw new Exception("A exception occured while setting the name of the Com-port '{0}'. Found the following Com-Ports: {1}.  Will not send data to the controller.".Build(ComPortName, string.Join(", ", PortNames)), E);
            }

            try
            {
                ComPort.Open();
            }
            catch (Exception E)
            {
                throw new Exception("A exception occured while trying to open the Com-port '{0}'. Found the following Com-Ports: {1}.  Will not send data to the controller.".Build(ComPortName, string.Join(", ", PortNames)), E);
            }



            //Make sure, the controller is in the expected state (ready to receive commands)
            Thread.Sleep(50);
            ComPort.ReadExisting();

            bool CommandModeOK = false;
            for (int AttemptNr = 0; AttemptNr < 20; AttemptNr++)
            {

                ComPort.Write(new byte[] { 0 }, 0, 1);
                Thread.Sleep(20);
                if (ComPort.BytesToRead > 0)
                {
                    int Ret = ComPort.ReadByte();
                    if (Ret == (int)'A')
                    {
                        //Got a Ack, controller is ready for more commands
                        CommandModeOK = true;
                        break;
                    }
                    else if (Ret == (int)'N')
                    {
                        //Got a NACK. Controller is ready for more commands
                        CommandModeOK = true;
                        break;
                    }
                    else
                    {
                        //Must be the answer of a previous command. Just ignore.
                    }

                    //Got no anwser from com port. Mostly likely we are still inside a command which is expecting more data. Send a lot of 0 bytes to get out of this situation.
                    ComPort.Write(new byte[3 * 1000], 0, 3 * 1000);

                    Thread.Sleep(50);
                    //Get rid of all returned data and try again
                    ComPort.ReadExisting();
                }
            };
            if (!CommandModeOK)
            {
                Log.Exception("Could not put the controller on com-port {0} into the commandmode. Will not send data to the controller.".Build(ComPortName));
                DisconnectFromController();
                return;
            }


            //If we reach this point, we know that the controller is ready to accept commands.

            //Check max number of leds per channel
            ComPort.Write(new byte[] { (byte)'M' }, 0, 1);
            byte[] ReceiveData = new byte[3];
            int BytesRead = -1;

            try
            {
                BytesRead = ReadPortWait(ReceiveData, 0, 3);
            }
            catch (Exception E)
            {
                throw new Exception("Expected 3 bytes containing data on the max number of leds per channel, but the read operation resulted in a exception. Will not send data to the controller", E);
            }


            if (BytesRead != 3)
            {
                throw new Exception("The TeensyStripController did not send the expected 3 bytes containing the data on the max number of leds per channel. Received only {0} bytes. Will not send data to the controller".Build(BytesRead));
            }
            if (ReceiveData[2] != 'A')
            {
                throw new Exception("The TeensyStripController did not send a ACK after the data containing the max number of leds per channel. Will not send data to the controller");
            }
            int MaxNumberOfLedsPerChannel = ReceiveData[0] * 256 + ReceiveData[1];

            if (NumberOfLedsPerStrip.Any(Nr => Nr > MaxNumberOfLedsPerChannel))
            {
                throw new Exception("The TeensyStripController boards supports up to {0}} leds per channel, but you have defined up to {1} leds per channel. Will not send data to the controller.".Build(MaxNumberOfLedsPerChannel, NumberOfLedsPerStrip.Max()));
            }



            //Set number of leds per channel
            NumberOfLedsPerChannel = NumberOfLedsPerStrip.Max();
            ushort NrOfLeds = (ushort)NumberOfLedsPerChannel;
            byte[] CommandData = new byte[3] { (byte)'L', (byte)(NrOfLeds >> 8), (byte)(NrOfLeds & 255) };
            ComPort.Write(CommandData, 0, 3);
            ReceiveData = new byte[1];
            BytesRead = -1;
            try
            {
                BytesRead = ReadPortWait(ReceiveData, 0, 1);
            }
            catch (Exception E)
            {
                throw new Exception("Expected 1 bytes after setting the number of leds per channel, but the read operation resulted in a exception. Will not send data to the controller.", E);
            }

            if (BytesRead != 1 || ReceiveData[0] != (byte)'A')
            {
                throw new Exception("Expected a Ack (A) after setting the number of leds per channel, but received no answer or a unexpected answer. Will not send data to the controller.");

            }

            //Clear the buffer and turn off the leds.
            CommandData = new byte[1] { (byte)'C' };
            ComPort.Write(CommandData, 0, 1);
            ReceiveData = new byte[1];
            BytesRead = -1;
            try
            {
                BytesRead = ReadPortWait(ReceiveData, 0, 1);
            }
            catch (Exception E)
            {
                throw new Exception("Expected 1 bytes after clearing the buffer of the TeensyStripController, but the read operation resulted in a exception. Will not send data to the controller.", E);
            }

            if (BytesRead != 1 || ReceiveData[0] != (byte)'A')
            {
                throw new Exception("Expected a Ack (A) after clearing the buffer of the TeensyStripController, but received no answer or a unexpected answer. Will not send data to the controller.");
            }

            CommandData = new byte[1] { (byte)'O' };
            ComPort.Write(CommandData, 0, 1);
            ReceiveData = new byte[1];
            BytesRead = -1;
            try
            {
                BytesRead = ReadPortWait(ReceiveData, 0, 1);
            }
            catch (Exception E)
            {
                throw new Exception("Expected 1 bytes after outputing the buffer of the TeensyStripController to the ledstrips, but the read operation resulted in a exception. Will not send data to the controller.", E);
            }

            if (BytesRead != 1 || ReceiveData[0] != (byte)'A')
            {
                throw new Exception("Expected a Ack (A) after outputing the buffer of the TeensyStripController to the ledstrips, but received no answer or a unexpected answer. Will not send data to the controller.");
            }
        }




        /// <summary>
        /// Disconnects from the controller.
        /// </summary>
        protected override void DisconnectFromController()
        {
            if (ComPort != null)
            {
                try
                {
                    ComPort.Close();

                }
                finally
                {
                    ComPort = null;
                }
            }

        }


        /// <summary>
        /// Reads reads the specifed number of bytes into the given buffer.
        /// Waits until the specified number of bytes has been received or until the read timeout of the comport occurs.
        /// </summary>
        /// <param name="Buffer">The buffer.</param>
        /// <param name="BufferOffset">The buffer offset.</param>
        /// <param name="NumberOfBytes">The number of bytes.</param>
        /// <returns>Number of bytes read.</returns>
        private int ReadPortWait(byte[] Buffer, int BufferOffset, int NumberOfBytes)
        {

            byte[] ReadBuffer = new byte[1];
            for (int ByteNumber = 0; ByteNumber < NumberOfBytes; ByteNumber++)
            {
                int BytesRead = -1;

                try
                {
                    BytesRead = ComPort.Read(ReadBuffer, 0, 1);

                }
                catch (TimeoutException TE)
                {
                    throw new Exception("A TimeoutException occured while trying to read byte {0} of {1} from Com-Port {2}.".Build(ByteNumber + 1, NumberOfBytes, ComPort.PortName), TE);
                }
                catch (Exception E)
                {
                    throw new Exception("A exception occured while trying to read byte {0} of {1} from Com-Port {2}.".Build(ByteNumber + 1, NumberOfBytes, ComPort.PortName), E);
                }

                if (BytesRead != 1)
                {
                    throw new Exception("A exception occured while trying to read byte {0} of {1} from Com-Port {2}. Tried to read 1 byte, but received {3} bytes.".Build(new object[] { ByteNumber + 1, NumberOfBytes, ComPort.PortName, BytesRead }));
                }

                Buffer[BufferOffset + ByteNumber] = ReadBuffer[0];

            }

            return NumberOfBytes;

        }



    }
}
