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
    /// The firmware for the Teensy based ledstrip controller is based on a slightly hacked version of Paul Stoffregens excellent OctoWS2811 LED Library which can easily drive up to 1100leds per channel on a Teensy 3.1 or later. More information on the OctoWS2811 lib can be found on the <a target="_blank" href="http://pjrc.com/teensy/td_libs_OctoWS2811.html">Teensy website</a>, on <a target="_blank" href="https://github.com/PaulStoffregen/OctoWS2811">Github</a> and many other places on the internet.
    /// 
    /// Ready to use, compiled firmware files can be downloaded from ??????? the source for the firmware is available on Github.
    /// 
    /// </summary>
    public class TeensyStripController : OutputControllerCompleteBase
    {
        private int[] NumberOfLedsPerStrip = new int[8];

        /// <summary>
        /// Gets or sets the number of leds of ledstrip connected to channel 1 of the Teensy.
        /// </summary>
        /// <value>
        /// The number of leds on the ledstrip connected to channel 1 of the Teensy.
        /// </value>
        public int NumberOfLedsStrip1
        {
            get
            {
                return NumberOfLedsPerStrip[0];
            }
            set
            {
                NumberOfLedsPerStrip[0] = value;
                base.SetupOutputs();
            }
        }
        /// <summary>
        /// Gets or sets the number of leds of ledstrip connected to channel 2 of the Teensy.
        /// </summary>
        /// <value>
        /// The number of leds on the ledstrip connected to channel 2 of the Teensy.
        /// </value>
        public int NumberOfLedsStrip2
        {
            get
            {
                return NumberOfLedsPerStrip[1];
            }
            set
            {
                NumberOfLedsPerStrip[1] = value;
                base.SetupOutputs();
            }
        }

        /// <summary>
        /// Gets or sets the number of leds of ledstrip connected to channel 3 of the Teensy.
        /// </summary>
        /// <value>
        /// The number of leds on the ledstrip connected to channel 3 of the Teensy.
        /// </value>
        public int NumberOfLedsStrip3
        {
            get
            {
                return NumberOfLedsPerStrip[2];
            }
            set
            {
                NumberOfLedsPerStrip[1] = value;
                base.SetupOutputs();
            }
        }
        /// <summary>
        /// Gets or sets the number of leds of ledstrip connected to channel 4 of the Teensy.
        /// </summary>
        /// <value>
        /// The number of leds on the ledstrip connected to channel 4 of the Teensy.
        /// </value>
        public int NumberOfLedsStrip4
        {
            get
            {
                return NumberOfLedsPerStrip[3];
            }
            set
            {
                NumberOfLedsPerStrip[3] = value;
                base.SetupOutputs();
            }
        }
        /// <summary>
        /// Gets or sets the number of leds of ledstrip connected to channel 5 of the Teensy.
        /// </summary>
        /// <value>
        /// The number of leds on the ledstrip connected to channel 5 of the Teensy.
        /// </value>
        public int NumberOfLedsStrip5
        {
            get
            {
                return NumberOfLedsPerStrip[4];
            }
            set
            {
                NumberOfLedsPerStrip[4] = value;
                base.SetupOutputs();
            }
        }

        /// <summary>
        /// Gets or sets the number of leds of ledstrip connected to channel 6 of the Teensy.
        /// </summary>
        /// <value>
        /// The number of leds on the ledstrip connected to channel 6 of the Teensy.
        /// </value>
        public int NumberOfLedsStrip6
        {
            get
            {
                return NumberOfLedsPerStrip[5];
            }
            set
            {
                NumberOfLedsPerStrip[5] = value;
                base.SetupOutputs();
            }
        }
        /// <summary>
        /// Gets or sets the number of leds of ledstrip connected to channel 7 of the Teensy.
        /// </summary>
        /// <value>
        /// The number of leds on the ledstrip connected to channel 7 of the Teensy.
        /// </value>
        public int NumberOfLedsStrip7
        {
            get
            {
                return NumberOfLedsPerStrip[6];
            }
            set
            {
                NumberOfLedsPerStrip[6] = value;
                base.SetupOutputs();
            }
        }
        /// <summary>
        /// Gets or sets the number of leds of ledstrip connected to channel 8 of the Teensy.
        /// </summary>
        /// <value>
        /// The number of leds on the ledstrip connected to channel 8 of the Teensy.
        /// </value>
        public int NumberOfLedsStrip8
        {
            get
            {
                return NumberOfLedsPerStrip[7];
            }
            set
            {
                NumberOfLedsPerStrip[7] = value;
                base.SetupOutputs();
            }
        }

        private string _ComPortName;

        /// <summary>
        /// Gets or sets the name (typicaly COM{Number}) of the virtual Com port the Teensy board is using.
        /// </summary>
        /// <value>
        /// The name of the Com port (typicaly COM{Number}) the Teensy board is using.
        /// </value>
        public string ComPortName
        {
            get { return _ComPortName; }
            set { _ComPortName = value; }
        }


        private int _ComPortTimeOutMs=200;

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
                    _ComPortTimeOutMs = value;
                }
                else
                {
                    _ComPortTimeOutMs = 200;
                    Log.Warning("The specified value {0} for the ComPortTimeOutMs is outside the valid range of 1 to 5000. Will use the default value of 200ms.".Build(value));
                }
            }
        }
        


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
            return !NumberOfLedsPerStrip.Any(Nr => Nr < 0) && !ComPortName.IsNullOrWhiteSpace() && SerialPort.GetPortNames().Any(PN => PN.Equals(ComPortName, StringComparison.InvariantCultureIgnoreCase));
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
            Thread.Sleep(100);
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
                Log.Exception("Could not put the controller into the commandmode. Will not send data to the controller.");
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
                BytesRead = ComPort.Read(ReceiveData, 0, 3);
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
                BytesRead = ComPort.Read(ReceiveData, 0, 1);
            }
            catch (Exception E)
            {
                throw new Exception("Expected 1 bytes after setting the number of leds per channel, but the read operation resulated in a exception. Will not send data to the controller.", E);
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
                BytesRead = ComPort.Read(ReceiveData, 0, 1);
            }
            catch (Exception E)
            {
                throw new Exception("Expected 1 bytes after clearing the buffer of the TeensyStripController, but the read operation resulated in a exception. Will not send data to the controller.", E);
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
                BytesRead = ComPort.Read(ReceiveData, 0, 1);
            }
            catch (Exception E)
            {
                throw new Exception("Expected 1 bytes after outputing the buffer of the TeensyStripController to the ledstrips, but the read operation resulated in a exception. Will not send data to the controller.", E);
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
    }
}
