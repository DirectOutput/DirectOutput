using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.LWEquivalent;
using DirectOutput.Cab.Toys;

namespace DirectOutput.Cab.Out.LW
{
    /// <summary>
    /// This class is used to detect and configure LedWiz output controllers automatically.
    /// </summary>
    public class LedWizAutoConfigurator : IAutoConfigOutputController
    {
        #region IAutoConfigOutputController Member

        /// <summary>
        /// This method detects and configures LedWiz output controllers automatically.
       /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoConfig(Cabinet Cabinet)
        {


            List<int> Preconfigured = new List<int>(Cabinet.OutputControllers.Where(OC => OC is LedWiz).Select(LW => ((LedWiz)LW).Number));
            List<int> Numbers = LedWiz.GetLedwizNumbers();
            foreach (int N in Numbers)
            {
                if (!Preconfigured.Contains(N))
                {
                    LedWiz LW = new LedWiz(N);
                    if (!Cabinet.OutputControllers.Contains(LW.Name))
                    {
                        
                        Cabinet.OutputControllers.Add(LW);
                        Log.Write("Detected and added LedWiz Nr. {0} with name {1}".Build(LW.Number, LW.Name));

                        bool NumberOccupied = false;
                        foreach (IToy Toy in Cabinet.Toys.Where(T => T is LedWizEquivalent))
                        {
                            LedWizEquivalent LWE = (LedWizEquivalent)Toy;
                            if (LWE.LedWizNumber == LW.Number)
                            {
                                NumberOccupied = true;
                                break;
                            }
                        }
                        if (!NumberOccupied)
                        {
                            LedWizEquivalent LWE = new LedWizEquivalent();
                            LWE.LedWizNumber = LW.Number;
                            LWE.Name = "{0} Equivalent".Build(LW.Name);
                            foreach (IOutput O in LW.Outputs)
                            {
                                LedWizOutput LWO = (LedWizOutput)O;

                                LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{1}".Build(LW.Name, LWO.Name), LedWizEquivalentOutputNumber = LWO.LedWizOutputNumber };
                                LWE.Outputs.Add(LWEO);
                             
                            }
                            if (!Cabinet.Toys.Contains(LWE.Name))
                            {
                                Cabinet.Toys.Add(LWE);
                                Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for Ledwiz Nr. {2}".Build(LWE.LedWizNumber,LWE.Name,LW.Number));
                            }

                        }
                    }
                }
            }





        }

        #endregion
    }
}
