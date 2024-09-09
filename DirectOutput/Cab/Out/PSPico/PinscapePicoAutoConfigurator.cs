using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectOutput.Cab.Out.PS;
using DirectOutput.Cab.Toys;
using DirectOutput.Cab.Toys.LWEquivalent;

namespace DirectOutput.Cab.Out.PSPico
{
	/// <summary>
	/// This class is used to detect and configure Pinscape Pico output controllers automatically.
	/// </summary>
	public class PinscapePicoAutoConfigurator : IAutoConfigOutputController
	{
		#region IAutoConfigOutputController Member

		/// <summary>
		/// Detect and configure Pinscape Pico units.
		/// </summary>
		/// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
		public void AutoConfig(Cabinet Cabinet)
		{
			// Set the base of the DOF internal unit numbering.  Pinscape Pico
			// units are positive small integers from 1 to 16*.  This maps to an
			// internal DOF range by adding the UnitBias, so Pinscape #1 becomes
			// DOF unit 120, Pinscape #2 -> DOF 121, etc.  The DOF numbering range
			// is arbitrary, but once established, it becomes a "well-known"
			// number that users and external tools rely on, so it can't be
			// changed once set (not without making everyone update all of their
			// other tools and config files, at least).
			//
			// *Pinscape Pico doesn't actually impose a hard upper bound in the
			// firmware, but the DOF unit numbering space is shared among all
			// device types, so we don't want to eat up too much of it.  We cap
			// the range arbitrarily at 1..16 for DOF purposes.
			const int UnitBias = 119;

			// enumerate pre-configured units
			List<int> preconfigured = new List<int>(Cabinet.OutputControllers.Where(oc => oc is PinscapePico).Select(psp => ((PinscapePico)psp).Number));

			// enumerate dynamically discovered units, found by scanning live HID instances
			IEnumerable<int> Numbers = PinscapePico.AllDevices().Select(d => d.UnitNo());
			foreach (int n in Numbers)
			{
				// if the device wasn't preconfigured (matching on unit number), add it dynamically
				if (!preconfigured.Contains(n))
				{
					// if it's not already in the live list (matching on name), add it
					PinscapePico p = new PinscapePico(n);
					if (!Cabinet.OutputControllers.Contains(p.Name))
					{
						// add the unit
						Cabinet.OutputControllers.Add(p);
						Log.Write("Detected and added Pinscape Pico Unit #{0} with name {1}".Build(p.Number, p.Name));

						// if we haven't set up the LedWizEquivalent for it yet, do so now
						if (!Cabinet.Toys.Any(t => t is LedWizEquivalent l && l.LedWizNumber == p.Number + UnitBias))
						{
							// add the LedWiz-equivalent unit
							LedWizEquivalent lwe = new LedWizEquivalent();
							lwe.LedWizNumber = p.Number + UnitBias;
							lwe.Name = "{0} Equivalent".Build(p.Name);

							// add the output ports
							for (int i = 1; i <= p.NumberOfOutputs; i++)
								lwe.Outputs.Add(new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(p.Name, i), LedWizEquivalentOutputNumber = i });

							// add it to the cabinet toy list if it's not there already
							if (!Cabinet.Toys.Contains(lwe.Name))
							{
								Cabinet.Toys.Add(lwe);
								Log.Write("Added LedWizEquivalent #{0} with name {1} for Pinscape Pico #{2}, {3} output ports".Build(
									lwe.LedWizNumber, lwe.Name, p.Number, p.NumberOfOutputs));
							}
						}
					}
				}
			}
		}

		#endregion
	}
}
