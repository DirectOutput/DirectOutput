using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.LWEquivalent;

namespace DirectOutput.Cab.Out.Pac
{
    /// <summary>
    /// Detects and configures a Philips Hue bridge unit.
    /// </summary>
    public class PhilipsHueAutoConfigurator : IAutoConfigOutputController {
        #region IAutoConfigOutputController Member

        /// <summary>
        /// This method detects and configures Philips Hue bridge controllers automatically
        /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoConfig(Cabinet Cabinet) {

            //Log.Write("ScheduleSettings name="+Cabinet.ScheduledSettings[0].Name+", clock="+ Cabinet.ScheduledSettings[0].ClockStart);

            Log.Write("PhilipsHueAutoConfigurator.AutoConfig started...note, actual connection detection will happen asynchronously, and device disabled if not succesfull (check further down in the log)");
            if (!Cabinet.OutputControllers.Any(oc => oc is PhilipsHueController && ((PhilipsHueController)oc).Id == 0)) {
                PhilipsHueController PHC = new PhilipsHueController();

                //set id first
                PHC.Id = 0;

                //manual testing for now, the getter/setters will apply this to active static list PhilipsHueControllerUnits[Id]
                PHC.BridgeIP = "10.0.1.174";
                PHC.BridgeKey = "ywCNFGOagGoJYtm16Kq4PS1tkGBAd3bj1ajg7uCk";

                PHC.ConnectHub();

                if (!Cabinet.OutputControllers.Contains(PHC.Name)) {
                    Cabinet.OutputControllers.Add(PHC);

                    Log.Write("Detected and added PhilipsHueController Id {0} with name {1}".Build(PHC.Id, PHC.Name));

                    //+50 used to define start of directoutputconfig[50...].xml
                    if (!Cabinet.Toys.Any(T => T is LedWizEquivalent && ((LedWizEquivalent)T).LedWizNumber == PHC.Id - 0 + 50)) {
                        LedWizEquivalent LWE = new LedWizEquivalent();
                        LWE.LedWizNumber = PHC.Id - 0 + 50;
                        LWE.Name = "{0} Equivalent 1".Build(PHC.Name);
                        for (int i = 1; i <= 50; i++) {
                            LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(PHC.Name, i), LedWizEquivalentOutputNumber = i };
                            LWE.Outputs.Add(LWEO);
                        }
                        if (!Cabinet.Toys.Contains(LWE.Name)) {
                            Cabinet.Toys.Add(LWE);
                            Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for PhilipsHueController with Id {2}".Build(LWE.LedWizNumber, LWE.Name, PHC.Id));
                        }
                    }
                }
            }
        }

        #endregion
    }
}
