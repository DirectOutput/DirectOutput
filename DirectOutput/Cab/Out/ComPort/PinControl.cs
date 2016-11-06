using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace DirectOutput.Cab.Out.ComPort
{
    /// <summary>
    /// PinControl is a Arduniobased output controller by http://www.vpforums.org/index.php?showuser=79113
    /// Is has 4 pwm output, 6 digital outputs. DOF supports any number of these controllers.
    /// Outputs 1,8,9,10 are pwm outputs.
    /// Outputs 2,3,4,5,6,7 are digital outputs.
    /// </summary>
    public class PinControl : OutputControllerCompleteBase
    {
        /// <summary>
        /// Gets or sets the COM port for the controller.
        /// </summary>
        /// <value>
        /// The COM port for the controller.
        /// </value>
        public string ComPort { get; set; }

        private SerialPort Port = null;
        private object PortLocker = new object();
        protected override int GetNumberOfConfiguredOutputs()
        {
            return 7 + 3;
        }

        protected override bool VerifySettings()
        {
            if (ComPort.IsNullOrWhiteSpace())
            {
                Log.Warning("ComPort is not set for {0} {1}.".Build(this.GetType().Name, Name));
                return false;
            }

            if (!SerialPort.GetPortNames().Any(x => x.Equals(ComPort, StringComparison.InvariantCultureIgnoreCase)))
            {
                Log.Warning("ComPort {2} is defined for {0} {1}, but does not exist.".Build(this.GetType().Name, Name, ComPort));
                return false;
            };

            return true;
        }

        byte[] OldValues = null;
        protected override void UpdateOutputs(byte[] OutputValues)
        {
            if (Port != null)
            {




                for (int i = 0; i < 7; i++)
                {
                    if (OldValues == null || OldValues[i] != OutputValues[i])
                    {
                        Port.Write("{0},{1}{2}#".Build(i + 1, (OutputValues[i] == 0 ? 2 : 1), (OutputValues[i] != 0 && i == 0 ? ",0,0," + OutputValues[i].ToString() : "")));

                    }
                }

                bool ColorChanged = false;
                bool IsBlack = true;
                for (int i = 8; i < 10; i++)
                {
                    if (OldValues == null || OldValues[i] != OutputValues[i])
                    {
                        ColorChanged = true;
                    };
                    if (OutputValues[i] != 0) IsBlack = false;
                }

                if (ColorChanged)
                {
                    if (IsBlack)
                    {
                        Port.Write("9,2#");
                    }
                    else
                    {
                        Port.Write("9,1,{0},{1},{2}#".Build(OutputValues[7], OutputValues[8], OutputValues[9]));
                    }
                }

                OldValues = (byte[])OutputValues.Clone();
            }
            else
            {
                throw new Exception("COM port {2} is not initialized for {0} {1}.".Build(this.GetType().Name, Name, ComPort));
            }

        }

        protected override void ConnectToController()
        {
            try
            {
                lock (PortLocker)
                {
                    if (Port != null)
                    {
                        DisconnectFromController();
                    }

                    OldValues = null;

                    Port = new SerialPort(ComPort, 115200, Parity.None, 8, StopBits.One);
                    Port.Open();
                }
            }
            catch (Exception E)
            {
                string Msg = "A exception occured while opening comport {2} for {0} {1}.".Build(this.GetType().Name, Name, ComPort);
                Log.Exception(Msg, E);
                throw new Exception(Msg, E);
            }
        }

        protected override void DisconnectFromController()
        {
            lock (PortLocker)
            {
                if (Port != null)
                {
                    Port.Close();
                    Port = null;
                    OldValues = null;
                }

            }
        }


    }
}
