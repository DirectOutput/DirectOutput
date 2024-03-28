using DirectOutput.Cab.Out.PS;
using DirectOutput.Cab.Toys.LWEquivalent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutput.Cab.Out.DudesCab
{
    /// <summary>
    /// This class is used to detect and configure DudesCab output controllers automatically.
    /// </summary>
    public class DudesCabAutoConfigurator : IAutoConfigOutputController
    {
        /// <summary>
        /// This method detects and configures DudesCab output controllers automatically.
        /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoConfig(Cabinet Cabinet)
        {
            const int UnitBias = 89;
            List<int> Preconfigured = new List<int>(Cabinet.OutputControllers.Where(OC => OC is DudesCab).Select(DC => ((DudesCab)DC).Number));
            IEnumerable<int> Numbers = DudesCab.AllDevices().Select(d => d.UnitNo());
            foreach (int N in Numbers) {
                if (!Preconfigured.Contains(N)) {
                    DudesCab dc = new DudesCab(N);
                    if (!Cabinet.OutputControllers.Contains(dc.Name)) {
                        Cabinet.OutputControllers.Add(dc);
                        Log.Write("Detected and added DudesCab Controller Nr. {0} with name {1}".Build(dc.Number, dc.Name));

                        if (!Cabinet.Toys.Any(T => T is LedWizEquivalent && ((LedWizEquivalent)T).LedWizNumber == dc.Number + UnitBias)) {
                            LedWizEquivalent LWE = new LedWizEquivalent();
                            LWE.LedWizNumber = dc.Number + UnitBias;
                            LWE.Name = "{0} Equivalent".Build(dc.Name);

                            for (int i = 1; i <= dc.NumberOfOutputs; i++) {
                                LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(dc.Name, i), LedWizEquivalentOutputNumber = i };
                                LWE.Outputs.Add(LWEO);
                            }

                            if (!Cabinet.Toys.Contains(LWE.Name)) {
                                Cabinet.Toys.Add(LWE);
                                Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for DudesCab Controller Nr. {2}".Build(
                                    LWE.LedWizNumber, LWE.Name, dc.Number) + ", {0}".Build(dc.NumberOfOutputs));
                            }
                        }
                    }
                }
            }
        }
    }
}
