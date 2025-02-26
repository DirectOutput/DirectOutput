using DirectOutput.Cab.Toys.LWEquivalent;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DirectOutput.Cab.Out.DudesCab;
using System.Xml.Linq;
using DirectOutput.FX;
using DirectOutput.General;
using DirectOutput.Cab.Toys.Layer;
using System.Drawing.Imaging;
using DirectOutput.Cab.Toys.Hardware;
using System.Windows.Forms;
using static DirectOutput.Cab.Out.AdressableLedStrip.UMXDevice;

namespace DirectOutput.Cab.Out.AdressableLedStrip
{

    public class UMXController : OutputControllerCompleteBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UMXController"/> class with a given unit number.
        /// </summary>
        /// <param name="Number">The number of the UMX controller.</param>
        public UMXController(int Number)
        {
            this.Number = Number;
        }


        #region Number

        private object NumberUpdateLocker = new object();
        private int _Number = -1;

        /// <summary>
        /// Gets or sets the unit number of the controller.<br />
        /// The unit number must be unique.<br />
        /// Setting changes the Name property, if it is blank or if the Name coresponds to "DudesCab Controller {0}".
        /// </summary>
        /// <value>
        /// The unique unit number of the controller (Range 1-5).
        /// </value>
        /// <exception cref="System.Exception">
        /// Dude's Cab Unit Numbers must be between 1-5. The supplied number {0} is out of range.
        /// </exception>
        public int Number
        {
            get { return _Number; }
            set {
                lock (NumberUpdateLocker) {
                    // if the unit number changed, update it and attach to the new unit
                    if (_Number != value) {
                        // if we used a default name for the old unit number, change to the default
                        // name for the new unit number
                        if (Name.IsNullOrWhiteSpace() || Name == "UMX Controller {0:00}".Build(_Number)) {
                            Name = "UMX Controller {0:00}".Build(value);
                        }

                        // remember the new unit number
                        _Number = value;

                        // attach to the new device record for this unit number, updating the output list to match
                        this.Dev = UMXControllerAutoConfigurator.AllDevices().First(D => D.UnitNo() == Number);
                    }
                }
            }
        }

        #endregion

        protected override void ConnectToController()
        {
            Dev?.ResetDataLines();
        }

        protected override void DisconnectFromController()
        {
            Dev?.SendCommand(UMXDevice.UMXCommand.UMX_AllOff);
        }

        protected override int GetNumberOfConfiguredOutputs()
        {
            return Dev.NumOutputs();
        }

        protected override void UpdateOutputs(byte[] OutputValues)
        {
            Dev?.UpdateOutputs(OutputValues);
        }

        protected override bool VerifySettings()
        {
            if (Dev == null) return false;

            if (Dev.NumOutputs() == 0) return false;

            if (!Dev.VerifySettings()) return false;

            return true;
        }

        public void UpdateCabinetFromConfig(Cabinet cabinet)
        {
            Dev?.Initialize();
            Log.Write($"UMX Infos : Name {Dev?.name}, UMX Version {Dev?.umxVersion}, MaxDataLines {Dev?.maxDataLines}, MaxNbLeds {Dev?.maxNbLeds}");
            Log.Write($"UMX Config :\nEnabled {Dev?.enabled}, LedChipset {Dev?.ledChipset}, LedWizEquivalent {Dev?.ledWizEquivalent}");
            Log.Write($"TestOnReset {Dev?.testOnReset}({Dev?.testOnResetDuration}s), TestOnConnect {Dev?.testOnConnect}({Dev?.testOnConnectDuration}s), TestBrightness: {Dev?.testBrightness}");
            Log.Write($"{Dev?.LedStrips.Count} ledstrips :");
            for (int numstrip = 0; numstrip < Dev?.LedStrips.Count; numstrip++) {
                var ledstrip = Dev?.LedStrips[numstrip];
                Log.Write($"\t[{numstrip}] {ledstrip.Name} => W/H:{ledstrip.Width}/{ledstrip.Height} ({ledstrip.NbLeds} leds), FirstLed: {ledstrip.FirstLedIndex}, Dof:{ledstrip.DofOutputNum}, Brightness:{ledstrip.Brightness} [{ledstrip.FadeMode},{ledstrip.Arrangement},{ledstrip.ColorOrder}]");
                if (ledstrip.Splits.Count == 1) {
                    Log.Write($"\t\t1 split : {ledstrip.Splits.Last().NbLeds} leds on line {ledstrip.Splits.Last().DataLine}");
                } else {
                    Log.Write($"\t\t{ledstrip.Splits.Count} splits :");
                    foreach (var split in ledstrip.Splits) {
                        Log.Write($"\t\t\t{split.NbLeds} leds on line {split.DataLine}");
                    }
                }
            }
            Log.Write($"{Dev?.totalLeds} leds, {Dev?.NumOutputs()} outputs configured");
            Dev?.CreateDataLines();
            Log.Instrumentation("UMX", $"{Dev?.DataLines.Length} Output lines generated");
            foreach(var line in Dev?.DataLines) {
                Log.Instrumentation("UMX", $"\t{line.NbLeds} leds");
            }

            if (!cabinet.Toys.Any(T => T is LedWizEquivalent && ((LedWizEquivalent)T).LedWizNumber == Dev.ledWizEquivalent)) {
                //Create LedwizEquivalent
                LedWizEquivalent LWE = new LedWizEquivalent();
                LWE.LedWizNumber = Dev.ledWizEquivalent;
                LWE.Name = $"{Dev.name}#{Dev.unitNo} Equivalent";

                if (!cabinet.Toys.Contains(LWE.Name)) {
                    cabinet.Toys.Add(LWE);
                    Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for UMXController Nr. {2}".Build(
                        LWE.LedWizNumber, LWE.Name, Dev.unitNo) + ", {0}".Build(Dev.LedStrips.Count));
                }

                //Sort Ledstrip per DataLines
                Dev.LedStrips.Sort((L1,L2) => L1.FirstDataline-L2.FirstDataline);

                foreach(var ledstripDesc in Dev.LedStrips) {
                    var ledstrip = new LedStrip() {
                        Brightness = ledstripDesc.Brightness,
                        ColorOrder = ledstripDesc.ColorOrder,
                        FirstLedNumber = ledstripDesc.FirstLedIndex+1,
                        Height = ledstripDesc.Height,
                        Width = ledstripDesc.Width,
                        LedStripArrangement = ledstripDesc.Arrangement,
                        FadingCurveName = ledstripDesc.FadeMode.ToString(),
                        Name = $"Ledstrip_StartLed{ledstripDesc.FirstLedIndex + 1}",
                        OutputControllerName = Name
                    };

                    cabinet.Toys.Add(ledstrip);

                    LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = ledstrip.Name, LedWizEquivalentOutputNumber = ledstripDesc.DofOutputNum };
                    LWE.Outputs.Add(LWEO);
                }
            }

            if (Dev?.testOnConnect != UMXDevice.TestMode.None) {
                byte[] parameters = new byte[] {
                    (byte)(Dev?.testOnConnect),
                    (byte)(Dev?.testOnConnectDuration),
                };
                Dev?.SendCommand(UMXDevice.UMXCommand.UMX_StartTest, parameters);
            }
        }

        #region Device
        // my device
        private UMXDevice Dev;
        #endregion
    }
}
