using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.LedControl.Loader
{
    public class TableVariablesDictionary: Dictionary<string,VariablesDictionary>
    {

        public TableVariablesDictionary() { }

        public TableVariablesDictionary(List<string> DataToParse)
        {
            foreach (string D in DataToParse)
            {
                int TP = D.IndexOf(",");
                if (TP > 0)
                {
                    string TableName = D.Substring(0, TP).Trim();
                    string VD = D.Substring(TP + 1).Trim();

                    int P = VD.IndexOf("=");
                    if (P > 0)
                    {
                        if (!ContainsKey(TableName))
                        {
                            Add(TableName, new VariablesDictionary());
                        }
                        this[TableName].Add(VD.Substring(0, P).Trim(), VD.Substring(P + 1).Trim());
                    }
                    else
                    {
                        Log.Warning("Could not find = in variables section line {0}.".Build(D));
                    }
                }
                else
                {
                    Log.Warning("Could not find comma in TableVariables section line {0}.".Build(D));
                }
            }


        }


    }
}
