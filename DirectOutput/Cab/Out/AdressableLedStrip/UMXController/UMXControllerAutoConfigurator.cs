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
    /// That's why this AutoConfigurator is not populating devices at construction.
    /// Other IAutoConfigOutputController can populate UMXDevices to this Configurator, controllers will be automatically created if the AutoConfig was already called.
    /// </summary>
    public class UMXControllerAutoConfigurator : IAutoConfigOutputController
    {
        static UMXControllerAutoConfigurator()
        {
        }

        private static List<UMXDevice> Devices = new List<UMXDevice>();

        private static bool inited = false;

        private static Cabinet cabinet = null;

        public static void AddUMXDevice(UMXDevice dev)
        {
            if (!Devices.Contains(dev)) {
                dev.unitNo = (short)(Devices.Count + 1);
                Devices.Add(dev);

                try {
                    //Because Autoconfig are not sorted, this one could already have done its AutoConfig call before some implementations
                    //So adding new controller to cabinet there, didn't happen with Dude's cab UMX implementation.
                    if (inited && cabinet != null) {
                        UMXController umxC = new UMXController() { Number = dev.UnitNo() };
                        Log.Instrumentation("UMX", $"Adding new device {dev.name} & controller {umxC.Name} to cabinet after UMXControllerAutoConfigurator initialization");
                        umxC.UpdateCabinetFromConfig(cabinet);
                    }
                } catch (Exception e) { 
                    Log.Exception(e);
                }
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
            cabinet = Cabinet;
            List<int> Preconfigured = new List<int>(Cabinet.OutputControllers.Where(OC => OC is UMXController).Select(C => ((UMXController)C).Number));
            foreach (var device in UMXControllerAutoConfigurator.AllDevices()) {
                if (!Preconfigured.Contains(device.UnitNo())) {
                    UMXController umxC = new UMXController() { Number = device.UnitNo() };
                    umxC.UpdateCabinetFromConfig(cabinet);
                }
            }
            inited = true;
        }
    }
}
