using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.LWEquivalent;
using DirectOutput.Cab.Toys;

namespace DirectOutput.Cab.Out.FTDIChip {
    /// <summary>
    /// This class detects all connected FT245RBitbangController units and configures them.
    /// </summary>
    public class FT245RBitbangControllerAutoConfigurator : IAutoConfigOutputController
    {
        #region IAutoDetectOutputController Member

        class DeviceInfo
        {
            public DeviceInfo(string serial, string desc)
            {
                this.serial = serial;
                this.desc = desc;
            }
            public string serial;
            public string desc;
        }

        /// <summary>
        /// This method detects and configures Sainsmart 8ch USB outputs automatically (possibly FT245RBitbangController in general?).
        /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoConfig(Cabinet Cabinet)
        {
            //Log.Write("FT245RBitbangControllerAutoConfigurator.AutoConfig started");

            FTDI dummyFTDI = new FTDI();
            uint amountDevices = 0;
            string callResult = "";
            List<DeviceInfo> devicelist = new List<DeviceInfo>();

            //fetch amount of devices, then kill instance
            callResult = dummyFTDI.GetNumberOfDevices(ref amountDevices).ToString();
            //Log.Write("FT245RBitbangControllerAutoConfigurator.AutoConfig: amount of devices detected=" + amountDevices + ", callresult=" + callResult);
            dummyFTDI.Close();
            dummyFTDI = null;

            //connect to each device using ftdi directly, snag the index and serial, then close and gc
            //do this seperate from adding instances in case multiple instances of ftdi would cause issues, as doing this too aggressive will cause locked exe / dll on exit
            for (uint i=0; i<amountDevices; i++)
            {
				using (FTDI connectFTDI = new FTDI())
				{
					string deviceSerial = "";
					string deviceDesc = "";
					connectFTDI.OpenByIndex(i);
					connectFTDI.GetSerialNumber(out deviceSerial);
					connectFTDI.GetDescription(out deviceDesc);
					devicelist.Add(new DeviceInfo(deviceSerial, deviceDesc));
					//Log.Write("i=" + i + ", serial device=" + deviceSerial);
					connectFTDI.Close();
				}
            }
            
            //next add instances of the controller to output, and all controller outputs
            for (int deviceIndex=0; deviceIndex < devicelist.Count; deviceIndex++)
            {
                FT245RBitbangController FTDevice = new FT245RBitbangController();
                FTDevice.Name = "FT245RBitbangController {0}".Build(deviceIndex);
                FTDevice.SerialNumber = devicelist[deviceIndex].serial;
                FTDevice.Description = devicelist[deviceIndex].desc;
                FTDevice.Id = deviceIndex;

                Log.Write("FT245RBitbangControllerAutoConfigurator.AutoConfig.. Detected FT245RBitbangController" + "["
                    + deviceIndex + "], name=" + FTDevice.Name
                    + ", description: " + FTDevice.Description
                    + ", serial #"+FTDevice.SerialNumber);

                if (!Cabinet.OutputControllers.Contains(FTDevice.Name))
                {
                    Cabinet.OutputControllers.Add(FTDevice);

                    Log.Write("Detected and added FT245RBitbangController Id {0} with name {1}".Build(deviceIndex, FTDevice.Name));

                    //+40 used to define start of directoutputconfig[40...].xml
                    if (!Cabinet.Toys.Any(T => T is LedWizEquivalent && ((LedWizEquivalent)T).LedWizNumber == deviceIndex - 0 + 40))
                    {
                        LedWizEquivalent LWE = new LedWizEquivalent();
                        LWE.LedWizNumber = deviceIndex - 0 + 40;
                        LWE.Name = "{0} Equivalent 1".Build(FTDevice.Name);
                        for (int outputIndex = 1; outputIndex <= 8; outputIndex++)
                        {
                            LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(FTDevice.Name, outputIndex), LedWizEquivalentOutputNumber = outputIndex };
                            LWE.Outputs.Add(LWEO);
                        }
                        if (!Cabinet.Toys.Contains(LWE.Name))
                        {
                            Cabinet.Toys.Add(LWE);
                            Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for PacUIO with Id {2}".Build(LWE.LedWizNumber, LWE.Name, deviceIndex));
                        }
                    }
                }

            }
            
        }

        #endregion
    }
}
