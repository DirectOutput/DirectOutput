using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutput.Cab.Out.AdressableLedStrip
{
    /// <summary>
    /// The UMXControllerAutoConfigurator centralizes all incoming UMX Controllers addition from several sources.
    /// Could be from serial devices (Wemos, Teensy...) or others implementing the UMX Protocol (DudesCab for instance)
    /// That's why this AutoConfigurator is not populating devices at contruction.
    /// </summary>
    public class UMXControllerAutoConfigurator : IAutoConfigOutputController
    {
        static UMXControllerAutoConfigurator()
        {
        }

        private static List<UMXDevice> Devices = new List<UMXDevice>();

        public static void AddUMXDevice(UMXDevice dev)
        {
            if (!Devices.Contains(dev)) {
                dev.unitNo = (short)(Devices.Count + 1);
                Devices.Add(dev);
            }
        }

        public static void RemoveAllUMXDevices(Type deviceType)
        {
            Devices.RemoveAll(D => D.GetType() == deviceType);
        }

        /// <summary>
        /// Get the list of all DudesCab devices discovered in the system from the Windows USB device scan.
        /// </summary>
        public static List<UMXDevice> AllDevices()
        {
            return Devices;
        }

        /// <summary>
        /// This method detects and configures DudesCab output controllers automatically.
        /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoConfig(Cabinet Cabinet)
        {
            List<int> Preconfigured = new List<int>(Cabinet.OutputControllers.Where(OC => OC is UMXController).Select(C => ((UMXController)C).Number));
            foreach (var device in UMXControllerAutoConfigurator.AllDevices()) {
                if (!Preconfigured.Contains(device.UnitNo())) {
                    UMXController umxC = new UMXController(device.UnitNo());
                    umxC.UpdateCabinetFromConfig(Cabinet);
                }
            }
        }
    }
}
