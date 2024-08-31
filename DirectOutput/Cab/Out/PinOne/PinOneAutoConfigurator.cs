using DirectOutput.Cab.Toys.LWEquivalent;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace DirectOutput.Cab.Out.PinOne
{
    public class PinOneAutoConfigurator : IAutoConfigOutputController
    {
        #region IAutoConfigOutputController Member

        /// <summary>
        /// This method detects and configures PinOne output controllers automatically.
        /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoConfig(Cabinet Cabinet)
        {
            const int UnitBias = 10;
            List<string> Preconfigured = new List<string>(Cabinet.OutputControllers.Where(OC => OC is PinOne).Select(PO => ((PinOne)PO).ComPort));
            String comPort = GetDevice();


            if (!Preconfigured.Contains(comPort) && comPort != "")
            {
                PinOne p = new PinOne(comPort);
                if (!Cabinet.OutputControllers.Contains(p.Name))
                {
                    Cabinet.OutputControllers.Add(p);
                    Log.Write("Detected and added PinOne Controller Nr. {0} with name {1}".Build(p.Number, p.Name));

                    if (!Cabinet.Toys.Any(T => T is LedWizEquivalent && ((LedWizEquivalent)T).LedWizNumber == p.Number + UnitBias))
                    {
                        LedWizEquivalent LWE = new LedWizEquivalent();
                        LWE.LedWizNumber = p.Number + UnitBias;
                        LWE.Name = "{0} Equivalent".Build(p.Name);

                        for (int i = 1; i <= p.NumberOfOutputs; i++)
                        {
                            LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(p.Name, i), LedWizEquivalentOutputNumber = i };
                            LWE.Outputs.Add(LWEO);
                        }

                        if (!Cabinet.Toys.Contains(LWE.Name))
                        {
                            Cabinet.Toys.Add(LWE);
                            Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for PinOne Controller Nr. {2}".Build(
                                LWE.LedWizNumber, LWE.Name, p.Number) + ", {0}".Build(p.NumberOfOutputs));
                        }
                    }
                }
            }

        }

        public static String GetDevice()
        {
            foreach (string sp in System.IO.Ports.SerialPort.GetPortNames())
            {
                SerialPort Port = null;
                try
                {

                    Port = new SerialPort(sp, 2000000, Parity.None, 8, StopBits.One);
                    Port.NewLine = "\r\n";
                    Port.ReadTimeout = 100;
                    Port.WriteTimeout = 100;
                    Port.Open();
                    Port.DtrEnable = true;
                    Port.Write(new byte[] { 0, 251, 0, 0, 0, 0, 0, 0, 0 }, 0, 9);
                    while (true)
                    {
                        string result = Port.ReadLine();
                        if (result == "DEBUG,CSD Board Connected")
                        {
                            Port.Close();
                            return sp;
                        }
                    }
                }
                catch (Exception)
                {
                    if (Port != null)
                    {
                        Port.Close();
                    }
                }
            }


            string comPort = "";
            PinOneCommunication communication = new PinOneCommunication("");
            if (communication.ConnectToServer())
            {
                comPort = communication.GetCOMPort();
            }
            
            return comPort;
        }

        #endregion
    }
}
