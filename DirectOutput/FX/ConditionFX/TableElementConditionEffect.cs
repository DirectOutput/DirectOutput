using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Table;
using Ciloci.Flee;
using System.Xml.Serialization;

namespace DirectOutput.FX.ConditionFX
{
    /// <summary>
    /// This effect evaluates the condition specified in the Condition property. 
    /// </summary>
    public class TableElementConditionEffect : EffectEffectBase
    {
        private string _Condition;

        /// <summary>
        /// Gets or sets the condition. 
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        public string Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }

        [XmlIgnore]
        IGenericExpression<bool> ConditionExpression = null;


        public List<string> GetVariables()
        {
            List<string> Variables = new List<string>();

            string C = Condition;
            if (C.IsNullOrWhiteSpace()) return Variables;

            try
            {

                int VariableStart = -1;
                for (int i = 0; i < C.Length - 1; i++)
                {
                    if (Enum.IsDefined(typeof(TableElementTypeEnum), (int)C[i]))
                    {
                        //Found a possible variable start letter
                        if (VariableStart >= 0)
                        {
                            //We're already inside a variable. A second letter is not allowed here.
                            VariableStart = -1;
                        }
                        else
                        {
                            //Register the start pos of the variable
                            VariableStart = i;
                        }
                    }
                    else if (VariableStart >= 0)
                    {
                        if (C.Substring(i, 1).IsInteger())
                        {
                            //Still inside the variable
                        }
                        else if (C.Substring(i, 1) == "-" && (i - VariableStart) == 1)
                        {
                            //Variable has a negative number
                        }
                        else if ((i - VariableStart) > 1 && C.Substring(VariableStart + 1, i - VariableStart - 1).IsInteger())
                        {
                            //Outside the variable and variable is ok
                            if (!Variables.Contains(C.Substring(VariableStart, i - VariableStart).ToUpper()))
                            {
                                Variables.Add(C.Substring(VariableStart, i - VariableStart).ToUpper());
                            }
                            VariableStart = -1;
                        }
                        else
                        {
                            //outside the variable and no valid variable spec found
                            VariableStart = -1;
                        }


                    }
                    else
                    {
                        //This is not variable content
                        VariableStart = -1;
                    }
                }
            }
            catch (Exception E)
            {
                Log.Exception("A exception occured while trying to extract variable names from the condition {0}.".Build(Condition), E);
            }

            return Variables;
        }


        private void InitCondition()
        {
            List<string> Variables = GetVariables();
            string C = Condition;
            ConditionExpression = null;

            if (C.IsNullOrWhiteSpace())
            {
                Log.Warning("No condition has been set for {0} named {1}.".Build(this.GetType().Name, Name));
                return;
            };


            ExpressionContext Context = new ExpressionContext();

            try
            {
                Context.Imports.AddType(typeof(Math));

                foreach (string V in Variables)
                {
                    int P = 0;
                    while (P<C.Length && P>=0)
                    {
                        P = C.IndexOf(V, P + 1, StringComparison.OrdinalIgnoreCase);
                        if (P < 0) break;
                        if (!C.Substring(P, V.Length + 1).Equals(V + ".", StringComparison.OrdinalIgnoreCase))
                        {
                            C = C.Substring(0, P) + "{0}.Value".Build(V) + C.Substring(P + V.Length);
                        }
                    }
//                    C = C.Replace(V, "{0}.Value".Build(V), StringComparison.OrdinalIgnoreCase);


                    if (Table.TableElements.Contains((TableElementTypeEnum)V[0], V.Substring(1).ToInteger()))
                    {
                        Table.TableElements.UpdateState((TableElementTypeEnum)V[0], V.Substring(1).ToInteger(), 0);
                    }


                    TableElement TE = Table.TableElements[(TableElementTypeEnum)V[0], V.Substring(1).ToInteger()];

                    Context.Variables[V] = TE;
                }
            }
            catch (Exception E)
            {
                Log.Exception("A exception has occured while setting up the variables for condition {0} of effect {1}.".Build(Condition, Name), E);
                return;

            }

            try
            {
                ConditionExpression = Context.CompileGeneric<bool>(C);

            }
            catch (Exception E)
            {
                Log.Exception("A exception has occured while compiling the condition {0} (internaly translated to {2}) of effect {1}.".Build(Condition, Name, C), E);
                return;
            }




        }



        /// <summary>
        /// Triggers the effect with the given TableElementData.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(TableElementData TableElementData)
        {
            if (ConditionExpression != null)
            {
                try
                {
                    if (ConditionExpression.Evaluate())
                    {
                        TableElementData.Value = 255;
                    }
                    else
                    {
                        TableElementData.Value = 0;
                    }
                    TriggerTargetEffect(TableElementData);
                }
                catch (Exception E)
                {

                    Log.Exception("A exception occured when evaluating the expression {0} of effect {1}. Effect will be deactivated.".Build(ConditionExpression.Text, Name), E);
                    ConditionExpression = null;

                }
            }
        }





        /// <summary>
        /// Initializes the Effect. <br />
        /// Resolves the name of the TargetEffect and initializes the condition context.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public override void Init(Table.Table Table)
        {
            base.Init(Table);
            InitCondition();
        }


        /// <summary>
        /// Finishes the Effect.<br />
        /// Releases the references to the target effect, the table object and the expression context.
        /// </summary>
        public override void Finish()
        {
            ConditionExpression = null;
            base.Finish();
        }


    }
}
