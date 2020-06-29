using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.LedControl.Loader
{
    public class TableVariablesDictionary : Dictionary<string, VariablesDictionary>
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
                    if (!ContainsKey(TableName))
                    {
                        Add(TableName, new VariablesDictionary());
                    }
                    string VD = D.Substring(TP + 1).Trim();

                    int Pos = 0;
                    int LastPos = 0;
                    while (Pos < VD.Length)
                    {
                        Pos = VD.IndexOf('=', LastPos);
                        if (Pos < 0) break;
                        if (Pos - LastPos < 1)
                        {
                            Log.Warning("Will skip some variable definitions due to missing variable name before = in line {0}".Build(D));
                            break;
                        }
                        string VarName = VD.Substring(LastPos, Pos - LastPos).Trim();
                        string Value = "";

                        Pos++;
                        if (Pos < VD.Length)
                        {
                            LastPos = Pos;

                            if (VD[Pos] == '{')
                            {
                                Pos = VD.IndexOf('}', LastPos);
                                if (Pos < 0)
                                {
                                    Log.Warning("Will skip some variable definitions due to missing closing } bracket in line {0}".Build(D));
                                    break;
                                }
                                Value = VD.Substring(LastPos + 1, Pos - LastPos - 1);

                                Pos++;
                                while (Pos < VD.Length && VD[Pos] == ' ')
                                {
                                    Pos++;
                                }
                                if (Pos < VD.Length)
                                {
                                    if (VD[Pos] != '/')
                                    {
                                        
                                        Log.Warning("Will skip some variable definitions due to missing / after } in line {0}".Build(D));
                                        break;
                                    }

                                }
                            }
                            else
                            {
                                Pos = VD.IndexOf('/', LastPos);
                                if (Pos < 0)
                                {
                                    Pos = VD.Length ;
                                }
                                Value = VD.Substring(LastPos, Pos - LastPos).Trim();

                            }
                        }
                        if (!this[TableName].ContainsKey(VarName))
                        {
                            this[TableName].Add(VarName, Value);
                        }
                        else
                        {
                            Log.Warning("Variable {0} has been defined more than once in line {1}.".Build(VarName, D));
                        }

                        LastPos = Pos + 1;

                    };



                }
                else
                {
                    Log.Warning("Could not find comma in TableVariables section line {0}.".Build(D));
                }
            }


        }


    }
}
