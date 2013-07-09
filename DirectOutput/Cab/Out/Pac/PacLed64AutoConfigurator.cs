using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.LWEquivalent;
using DirectOutput.Cab.Toys;

namespace DirectOutput.Cab.Out.Pac
{
    /// <summary>
    /// This class detects all connected PacLed64 units and configures them.
    /// </summary>
    public class PacLed64AutoConfigurator : IAutoConfigOutputController
    {
        #region IAutoDetectOutputController Member

        /// <summary>
        /// This method detects and configures PacLed64 controllers automatically
       /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoDetect(Cabinet Cabinet)
        {
            foreach (int Id in PacDriveSingleton.Instance.PacLed64GetIdList())
            {
                if (!Cabinet.OutputControllers.Any(oc => oc is PacLed64 && ((PacLed64)oc).Id == Id))
                {
                    PacLed64 PL = new PacLed64();
                    PL.Id = Id;
                    PL.AddOutputs();
                    if (!Cabinet.OutputControllers.Contains(PL.Name))
                    {
                        Cabinet.OutputControllers.Add(PL);

                        Log.Write("Detected and added PacLed64 Id {0} with name {1}".Build(PL.Id, PL.Name));

                        bool NumberOccupied = false;
                        foreach (IToy Toy in Cabinet.Toys.Where(T => T is LedWizEquivalent))
                        {
                            LedWizEquivalent LWE = (LedWizEquivalent)Toy;
                            if (LWE.LedWizNumber == ((PL.Id - 1) * 2) + 20 || LWE.LedWizNumber == ((PL.Id - 1) * 2) + 20 + 1)
                            {
                                NumberOccupied = true;
                                break;
                            }
                        }
                        if (!NumberOccupied)
                        {
                            LedWizEquivalent LWE = new LedWizEquivalent();
                            LWE.LedWizNumber = (PL.Id - 1) * 2 + 20;
                            LWE.Name = "{0} Equivalent 1".Build(PL.Name);
                            for (int i = 1; i <= 32; i++)
                            {
                                IOutputNumbered ON = (IOutputNumbered)PL.Outputs.First(O => ((IOutputNumbered)O).Number == i);

                                LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{1}".Build(PL.Name, ON.Name), LedWizEquivalentOutputNumber = ON.Number };
                                LWE.Outputs.Add(LWEO);

                            }
                            if (!Cabinet.Toys.Contains(LWE.Name))
                            {
                                Cabinet.Toys.Add(LWE);
                            }

                            LWE = new LedWizEquivalent();
                            LWE.LedWizNumber = (PL.Id - 1) * 2 + 20 + 1;
                            LWE.Name = "{0} Equivalent 2".Build(PL.Name);
                            for (int i = 1; i <= 32; i++)
                            {
                                IOutputNumbered ON = (IOutputNumbered)PL.Outputs.First(O => ((IOutputNumbered)O).Number == i + 32);

                                LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{1}".Build(PL.Name, ON.Name), LedWizEquivalentOutputNumber = ON.Number - 32 };
                                LWE.Outputs.Add(LWEO);

                            }
                            if (!Cabinet.Toys.Contains(LWE.Name))
                            {
                                Cabinet.Toys.Add(LWE);
                                Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for PacLed64 with Id {2}".Build(LWE.LedWizNumber, LWE.Name, PL.Id));

                            }


                        }


                    }
                }


            }

        }

        #endregion
    }
}
