using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.LWEquivalent;
using DirectOutput.Cab.Toys;

namespace DirectOutput.Cab.Out.Pac
{
    /// <summary>
    /// This class detects all connected Ultimate I/O units and configures them.
    /// </summary>
    public class PacUIOAutoConfigurator : IAutoConfigOutputController
    {
        #region IAutoDetectOutputController Member

        /// <summary>
        /// This method detects and configures Ultimate I/O controllers automatically
        /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoConfig(Cabinet Cabinet)
        {

            Log.Write("PacUIOAutoConfigurator.AutoConfig started");
            foreach (int Id in PacDriveSingleton.Instance.PacUIOGetIdList()) {
                if (!Cabinet.OutputControllers.Any(oc => oc is PacUIO && ((PacUIO)oc).Id == Id)) {
                    PacUIO PIO = new PacUIO();
                    PIO.Id = Id;

                    Log.Write("PacUIOAutoConfigurator.AutoConfig.. Detected "+ PacDriveSingleton.Instance.GetProductName(Id)+"["+PIO.Id + "], name=" + PIO.Name);

                    if (!Cabinet.OutputControllers.Contains(PIO.Name)) {
                        Cabinet.OutputControllers.Add(PIO);

                        Log.Write("Detected and added PacUIO Id {0} with name {1}".Build(PIO.Id, PIO.Name));

                        //+27 used to define start of directoutputconfig[27...].xml
                        if (!Cabinet.Toys.Any(T => T is LedWizEquivalent && ((LedWizEquivalent)T).LedWizNumber == PIO.Id - 0 + 27)) {
                            LedWizEquivalent LWE = new LedWizEquivalent();
                            LWE.LedWizNumber = PIO.Id - 0 + 27;
                            LWE.Name = "{0} Equivalent 1".Build(PIO.Name);
                            for (int i = 1; i <= 96; i++) {
                                LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(PIO.Name, i), LedWizEquivalentOutputNumber = i };
                                LWE.Outputs.Add(LWEO);
                            }
                            if (!Cabinet.Toys.Contains(LWE.Name)) {
                                Cabinet.Toys.Add(LWE);
                                Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for PacUIO with Id {2}".Build(LWE.LedWizNumber, LWE.Name, PIO.Id));
                            }
                        }


                    }
                }


            }

        }

        #endregion
    }
}
