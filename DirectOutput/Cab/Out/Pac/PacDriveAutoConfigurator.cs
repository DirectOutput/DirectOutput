using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.LWEquivalent;

namespace DirectOutput.Cab.Out.Pac
{
    /// <summary>
    /// Detectes and configures a PacDrive unit.
    /// </summary>
    public class PacDriveAutoConfigurator : IAutoConfigOutputController
    {
        #region IAutoConfigOutputController Member

        /// <summary>
        /// This method detects and configures a PacDrive output controller.
        /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoConfig(Cabinet Cabinet)
        {
            int Index = PacDriveSingleton.Instance.PacDriveGetIndex();
            if (Index >= 0)
            {
                if (!Cabinet.OutputControllers.Any(oc => oc is PacDrive))
                {
                    PacDrive PD = new PacDrive();
                    if (!Cabinet.OutputControllers.Contains(PD.Name))
                    {
                        Cabinet.OutputControllers.Add(PD);

                        Log.Write("Detected and added PacDrive");

                        if (!Cabinet.Toys.Any(T => T is LedWizEquivalent && ((LedWizEquivalent)T).LedWizNumber != 19))
                        {


                            LedWizEquivalent LWE = new LedWizEquivalent();
                            LWE.LedWizNumber = 19;
                            LWE.Name = "{0} Equivalent".Build(PD.Name);
                            for (int i = 1; i <= 16; i++)
                            {
                                LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}.{1:00}".Build(PD.Name, i), LedWizEquivalentOutputNumber = i };
                                LWE.Outputs.Add(LWEO);

                            }
                            if (!Cabinet.Toys.Contains(LWE.Name))
                            {
                                Cabinet.Toys.Add(LWE);
                                Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for PacDrive".Build(LWE.LedWizNumber, LWE.Name));
                            }


                        }
                    }
                }
            }
        }

        #endregion
    }
}
