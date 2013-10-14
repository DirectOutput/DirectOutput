using System.ComponentModel;
using System.Collections.Generic;

namespace DirectOutput.GlobalConfiguration
{
    /// <summary>
    /// List of LedControlIniFile objects.
    /// </summary>
    public class LedControlIniFileList : List<LedControlIniFile>
    {
        /// <summary>
        /// Adds the the specified filename and LedWizNumber combination to the list.
        /// </summary>
        /// <param name="Filename">The filename.</param>
        /// <param name="LedWizNumber">The led wiz number.</param>
        public void Add(string Filename, int LedWizNumber)
        {
            LedControlIniFile I = new LedControlIniFile(Filename,LedWizNumber);
            Add(I);
        }

        /// <summary>
        /// Adds the specified LedControlIniFile object to the list.
        /// </summary>
        /// <param name="LedControlIniFile">The led control ini file.</param>
        new public void Add(LedControlIniFile LedControlIniFile)
        {
            base.Add(LedControlIniFile);
            
        }

        /// <summary>
        /// Determines whether the list contains a entry for the specified LedWizNumber.
        /// </summary>
        /// <param name="LedWizNumber">The led wiz number.</param>
        /// <returns>
        ///   <c>true</c> if a entry exists for the LedWizNumber; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(int LedWizNumber)
        {
            foreach (LedControlIniFile F in this)
            {
                if (F.LedWizNumber == LedWizNumber) return true;
            }
            return false;

        }
 



    }
}
