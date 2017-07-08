using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys.LWEquivalent;
using DirectOutput.Cab.Toys;

namespace DirectOutput.Cab.Out.PS
{
    /// <summary>
    /// This class is used to detect and configure Pinscape output controllers automatically.
    /// </summary>
    public class PinscapeAutoConfigurator : IAutoConfigOutputController
    {
        #region IAutoConfigOutputController Member

        /// <summary>
        /// This method detects and configures Pinscape output controllers automatically.
        /// </summary>
        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
        public void AutoConfig(Cabinet Cabinet)
		{
			const int UnitBias = 50;
            List<int> Preconfigured = new List<int>(Cabinet.OutputControllers.Where(OC => OC is Pinscape).Select(PS => ((Pinscape)PS).Number));
			IEnumerable<int> Numbers = Pinscape.AllDevices().Select(d => d.GetUnitNo());
            foreach (int N in Numbers)
            {
                if (!Preconfigured.Contains(N))
                {
					Pinscape p = new Pinscape(N);
                    if (!Cabinet.OutputControllers.Contains(p.Name))
					{
                        Cabinet.OutputControllers.Add(p);
                        Log.Write("Detected and added Pinscape Controller Nr. {0} with name {1}".Build(p.Number, p.Name));

						if (!Cabinet.Toys.Any(T => T is LedWizEquivalent && ((LedWizEquivalent)T).LedWizNumber == p.Number + UnitBias))
						{
							LedWizEquivalent LWE = new LedWizEquivalent();
                            LWE.LedWizNumber = p.Number + UnitBias;
							LWE.Name = "{0} Equivalent".Build(p.Name);

                            for (int i = 1 ; i <= p.NumberOfOutputs ; i++)
                            {
								LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(p.Name, i), LedWizEquivalentOutputNumber = i };
                                LWE.Outputs.Add(LWEO);
                            }

                            if (!Cabinet.Toys.Contains(LWE.Name))
							{
                                Cabinet.Toys.Add(LWE);
								Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for Pinscape Controller Nr. {2}".Build(
									LWE.LedWizNumber, LWE.Name, p.Number) + ", {0}".Build(p.NumberOfOutputs));
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
