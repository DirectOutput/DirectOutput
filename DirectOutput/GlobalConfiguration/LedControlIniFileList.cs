using System.ComponentModel;

namespace DirectOutput.GlobalConfiguration
{
    public class LedControlIniFileList : BindingList<LedControlIniFile>
    {
        public void Add(string Filename, int LedWizNumber)
        {
            LedControlIniFile I = new LedControlIniFile(Filename,LedWizNumber);
            Add(I);
        }

        new public void Add(LedControlIniFile LedControlIniFile)
        {
            base.Add(LedControlIniFile);
            
        }

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
