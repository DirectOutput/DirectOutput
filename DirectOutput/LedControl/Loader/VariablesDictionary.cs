using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.LedControl.Loader
{
    public class VariablesDictionary: Dictionary<string,string>
    {



        public VariablesDictionary() { }

        public VariablesDictionary(List<string> DataToParse)
        {
            foreach (string D in DataToParse)
            {
                int P = D.IndexOf("=");
                if (P > 0)
                {
                    Add(D.Substring(0,P).Trim(),D.Substring(P+1).Trim());
                }
                else
                {
                    Log.Warning("Could not find = in variables section line {0}.".Build(D));
                }
            }
        }
    }
}
