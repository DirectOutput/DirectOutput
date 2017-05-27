using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.LWEquivalent;

namespace DirectOutput.Cab.Out.Pac
{
    /// <summary>
    /// Detects and configures a Philips Hue hub unit.
    /// </summary>
    public class PhilipsHueAutoConfigurator : IAutoConfigOutputController {
        #region IAutoConfigOutputController Member

        /// <summary>
        /// This method detects and configures Philips Hue hub controllers automatically
        /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoConfig(Cabinet Cabinet) {

            //Log.Write("ScheduleSettings name="+Cabinet.ScheduledSettings[0].Name+", clock="+ Cabinet.ScheduledSettings[0].ClockStart);

            Log.Write("PhilipsHueAutoConfigurator.AutoConfig started");
            if (!Cabinet.OutputControllers.Any(oc => oc is PhilipsHueHub && ((PhilipsHueHub)oc).Id == 0)) {
                PhilipsHueHub PHH = new PhilipsHueHub();

                //set id first
                PHH.Id = 0;

                //manual testing for now, the getter/setters will apply this to active static list PhilipsHueHubUnits[Id]
                PHH.HubKey = "YnuwKWbuBO5Sh7RHYBtcnO2QtJ9BNt8xT3Z7vj3W";
                PHH.HubIP = "10.0.1.174";
                PHH.ConnectHub();

                Log.Write("PhilipsHueAutoConfigurator.AutoConfig.. Detected [" + PHH.Id + "], name=" + PHH.Name);

                if (!Cabinet.OutputControllers.Contains(PHH.Name)) {
                    Cabinet.OutputControllers.Add(PHH);

                    Log.Write("Detected and added PhilipsHueHub Id {0} with name {1}".Build(PHH.Id, PHH.Name));

                    //+50 used to define start of directoutputconfig[50...].xml
                    if (!Cabinet.Toys.Any(T => T is LedWizEquivalent && ((LedWizEquivalent)T).LedWizNumber == PHH.Id - 0 + 50)) {
                        LedWizEquivalent LWE = new LedWizEquivalent();
                        LWE.LedWizNumber = PHH.Id - 0 + 50;
                        LWE.Name = "{0} Equivalent 1".Build(PHH.Name);
                        for (int i = 1; i <= 50; i++) {
                            LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(PHH.Name, i), LedWizEquivalentOutputNumber = i };
                            LWE.Outputs.Add(LWEO);
                        }
                        if (!Cabinet.Toys.Contains(LWE.Name)) {
                            Cabinet.Toys.Add(LWE);
                            Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for PhilipsHueHub with Id {2}".Build(LWE.LedWizNumber, LWE.Name, PHH.Id));
                        }
                    }
                }
            }
        }

        #endregion
    }
}
