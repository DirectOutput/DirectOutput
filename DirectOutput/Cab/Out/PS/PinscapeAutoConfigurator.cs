using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys;
using DirectOutput.Cab.Toys.LWEquivalent;

namespace DirectOutput.Cab.Out.PS
{
    /// <summary>
    /// This class automatically detects and configures Pinscape Controllers (for KL25Z).
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
			// Set the base of the DOF internal unit numbering.  Pinscape KL25Z
			// units are positive small integers from 1 to 15.  This maps to an
            // internal DOF range by adding the UnitBias, so Pinscape #1 becomes
            // DOF unit 51, Pinscape #2 -> DOF 52, etc.  The DOF numbering range
            // is arbitrary, but once established, it becomes a "well-known"
            // number that users and external tools rely on, so it can't be
            // changed once set (not without making everyone update all of their
            // other tools and config files, at least).
			const int UnitBias = 50;

            // enumerate pre-configured units (from cabinet config files)
            List<int> Preconfigured = new List<int>(Cabinet.OutputControllers.Where(OC => OC is Pinscape).Select(PS => ((Pinscape)PS).Number));

            // enumerate dynamically discovered devices from the live HID scan
			IEnumerable<int> Numbers = Pinscape.AllDevices().Select(d => d.UnitNo());
            foreach (int n in Numbers)
            {
                if (!Preconfigured.Contains(n))
                {
					Pinscape p = new Pinscape(n);
                    if (!Cabinet.OutputControllers.Contains(p.Name))
					{
                        Cabinet.OutputControllers.Add(p);
                        Log.Write("Detected and added Pinscape Controller (KL25Z) #{0} with name {1}".Build(p.Number, p.Name));

						if (!Cabinet.Toys.Any(t => t is LedWizEquivalent l && l.LedWizNumber == p.Number + UnitBias))
						{
							LedWizEquivalent lwe = new LedWizEquivalent();
                            lwe.LedWizNumber = p.Number + UnitBias;
							lwe.Name = "{0} Equivalent".Build(p.Name);

                            for (int i = 1 ; i <= p.NumberOfOutputs ; i++)
                            {
								LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(p.Name, i), LedWizEquivalentOutputNumber = i };
                                lwe.Outputs.Add(LWEO);
                            }

                            if (!Cabinet.Toys.Contains(lwe.Name))
							{
                                Cabinet.Toys.Add(lwe);
								Log.Write("Added LedWizEquivalent #{0} with name {1} for Pinscape Controller (KL25Z) #{2}".Build(
									lwe.LedWizNumber, lwe.Name, p.Number) + ", {0}".Build(p.NumberOfOutputs));
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
