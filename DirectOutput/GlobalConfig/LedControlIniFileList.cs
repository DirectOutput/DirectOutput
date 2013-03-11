using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;
using System.ComponentModel;

namespace DirectOutput.GlobalConfig
{
    public class LedControlIniFileList : BindingList<LedControlIniFile>
    {
        public void Add(string Filename)
        {
            LedControlIniFile I = new LedControlIniFile(Filename);
            Add(I);
        }

        new public void Add(LedControlIniFile LedControlIniFile)
        {
            base.Add(LedControlIniFile);
            Renumber();
        }

        public void Renumber()
        {
            int Number=1;
            foreach (LedControlIniFile I in this)
            {
                I.LedWizNumber = Number;
                Number++;
            }
        }



    }
}
