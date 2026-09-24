using DirectOutput.Cab.Toys.LWEquivalent;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

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

        // Old (pre-HID-migration) PinOne firmware's VID/PID. That firmware
        // only exposes a USB CDC serial port - no HID vendor channel exists
        // on it, so it's found by looking up its COM port, not by HID scan.
        private const string LegacyHardwareId = "VID_0E8F&PID_9207";

        /// <summary>
        /// Finds a connected PinOne board, preferring the current-firmware
        /// USB HID vendor channel (unambiguous, no serial port involved at
        /// all) and falling back to a *targeted* lookup of a legacy
        /// (old-firmware) board's COM port via WMI - rather than blindly
        /// opening and probing every COM port on the system, which is slow
        /// and can interfere with unrelated serial devices.
        /// </summary>
        public static String GetDevice()
        {
            // 1) Current firmware: found via its HID vendor channel. This is
            //    an unambiguous signal (VID/PID + usage page/usage match),
            //    so no serial port is ever touched in this case.
            if (PinOneHidCommunication.FindDevice() != null)
            {
                return PinOne.HidSentinel;
            }

            // 2) Legacy firmware: look up the specific COM port via WMI
            //    instead of scanning every port on the system.
            string legacyPort = FindLegacyComPort(out bool wmiAvailable);
            if (legacyPort != "")
            {
                if (TryConnect(legacyPort))
                {
                    return legacyPort;
                }
            }
            else if (!wmiAvailable)
            {
                // The WMI query itself failed (as opposed to succeeding but
                // finding no match) - fall back to the original blind scan
                // as a safety net so legacy boards still get detected.
                foreach (string sp in SerialPort.GetPortNames())
                {
                    if (TryConnect(sp))
                    {
                        return sp;
                    }
                }
            }

            // 3) Last resort: ask the named-pipe server, in case another
            //    process already owns a legacy board's port.
            string comPort = "";
            PinOneCommunication communication = new PinOneCommunication("");
            if (communication.ConnectToServer())
            {
                comPort = communication.GetCOMPort();
            }

            return comPort;
        }

        /// <summary>
        /// Looks up the COM port associated with an old-firmware PinOne
        /// board (VID 0x0E8F / PID 0x9208) via WMI.
        /// </summary>
        /// <param name="wmiAvailable">False if the WMI query itself failed (as opposed to succeeding but finding no match), signaling the caller to fall back to a full port scan.</param>
        /// <returns>The COM port name (e.g. "COM5"), or "" if none was found.</returns>
        private static string FindLegacyComPort(out bool wmiAvailable)
        {
            wmiAvailable = true;
            try
            {
                using (var searcher = new ManagementObjectSearcher(
                    "SELECT Name, PNPDeviceID FROM Win32_PnPEntity WHERE PNPDeviceID LIKE '%" + LegacyHardwareId + "%'"))
                {
                    foreach (ManagementObject device in searcher.Get())
                    {
                        string name = device["Name"] as string;
                        if (string.IsNullOrEmpty(name))
                            continue;

                        Match match = Regex.Match(name, @"\(COM\d+\)");
                        if (match.Success)
                        {
                            return match.Value.Trim('(', ')');
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Exception("Error searching for legacy PinOne COM port via WMI", e);
                wmiAvailable = false;
            }

            return "";
        }

        /// <summary>
        /// Opens the given COM port and verifies it's a PinOne board by
        /// sending the CONNECT admin packet and waiting for the expected
        /// handshake response.
        /// </summary>
        private static bool TryConnect(string sp)
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
                        return true;
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

            return false;
        }

        #endregion
    }
}
