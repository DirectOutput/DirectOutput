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
        //public class PhilipsHueAutoConfigurator  {
        #region IAutoConfigOutputController Member

        /// <summary>
        /// This method detects and configures Philips Hue bridge controllers automatically.
        /// It is slightly different from PacUIO in that it depends on both an entry in Cabinet.xml OutputControllers.PhilipsHueController for IP and key, and to reuse that instance in this AutoConfig to populate with toys.
        /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoConfig(Cabinet Cabinet) {

            //Log.Write("ScheduleSettings name="+Cabinet.ScheduledSettings[0].Name+", clock="+ Cabinet.ScheduledSettings[0].ClockStart);
            PhilipsHueController PHC = null;

            Log.Write("PhilipsHueAutoConfigurator.AutoConfig started...note, actual connection detection will happen asynchronously, and device disabled if not succesfull (check further down in the log)");

            //check if we need to create a new instance, or can reuse an instance found in cabinet.xml <outputcontrollers> list
            if (!Cabinet.OutputControllers.Any(oc => oc is PhilipsHueController && ((PhilipsHueController)oc).Id == 0)) {
                PHC = new PhilipsHueController();
            } else {
                foreach (IOutputController OC in Cabinet.OutputControllers) {
                    //Log.Write("PhilipsHueAutoConfigurator.AutoConfig...type=" + OC + ", name=" + OC.Name + ", type=" + OC.GetType());
                    if (OC is PhilipsHueController) {
                        Log.Write("PhilipsHueAutoConfigurator.AutoConfig...found existing instance, adding output toys to: "+ OC.Name);
                        PHC = (PhilipsHueController)OC;
                        break;
                    }
                }
            }

            //if we have an instance, start populating it with toys outputs
            if (PHC != null) {
                
                //set id first
                PHC.Id = 0;
                
                //manual testing for now, the getter/setters will apply this to active static list PhilipsHueControllerUnits[Id]
                //PHC.BridgeIP = "10.0.1.174";
                //PHC.BridgeKey = "ywCNFGOagGoJYtm16Kq4PS1tkGBAd3bj1ajg7uCk";
                //Log.Write("PhilipsHueAutoConfigurator.AutoConfig... <Name> = " + Cabinet.OutputControllers.ToString());

                PHC.ConnectHub();

                //if instance doesn't exist, add it to controllers
                if (!Cabinet.OutputControllers.Contains(PHC.Name)) {
                    Cabinet.OutputControllers.Add(PHC);
                    Log.Write("Detected and added PhilipsHueController Id {0} with name {1}".Build(PHC.Id, PHC.Name));
                }

                //+70 used to define start of directoutputconfig[70...].xml toys
                if (!Cabinet.Toys.Any(T => T is LedWizEquivalent && ((LedWizEquivalent)T).LedWizNumber == PHC.Id - 0 + 70)) {
                    LedWizEquivalent LWE = new LedWizEquivalent();
                    LWE.LedWizNumber = PHC.Id - 0 + 70;
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

        #endregion
    }
}
