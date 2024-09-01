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

        private List<string> GetVariablesInternal()
        {
            List<string> Variables = new List<string>();
            string C = Condition;
            if (C.IsNullOrWhiteSpace()) return Variables;


            ExpressionContext Context = new ExpressionContext();
            Context.Options.ParseCulture = System.Globalization.CultureInfo.InvariantCulture;
            Context.Imports.AddType(typeof(Math));

            VariableCollection VC = Context.Variables;
            VC.ResolveVariableType += new EventHandler<ResolveVariableTypeEventArgs>(GetVariablesInternal_ResolveVariableType);
            VC.ResolveVariableValue += new EventHandler<ResolveVariableValueEventArgs>(GetVariablesInternal_ResolveVariableValue);

            IGenericExpression<bool> Exp = null;
            try
            {
                Exp = Context.CompileGeneric<bool>(Condition.Replace(((char)TableElementTypeEnum.NamedElement).ToString(),"NamedElement_"));
            }
            catch {
              Log.Warning("Cant extract variables from condition {0}.".Build(Condition));
              Exp = null;
            }
            VC.ResolveVariableType -= new EventHandler<ResolveVariableTypeEventArgs>(GetVariablesInternal_ResolveVariableType);
            VC.ResolveVariableValue -= new EventHandler<ResolveVariableValueEventArgs>(GetVariablesInternal_ResolveVariableValue);


            if (Exp != null && Exp.Info != null)
            {
                Variables = Exp.Info.GetReferencedVariables().ToList();
            }

            return Variables;
        }

        void GetVariablesInternal_ResolveVariableValue(object sender, ResolveVariableValueEventArgs e)
        {
            e.VariableValue = 0;
        }

        void GetVariablesInternal_ResolveVariableType(object sender, ResolveVariableTypeEventArgs e)
        {
            e.VariableType = typeof(double);
        }


        /// <summary>
        /// Gets a list of all variables in the condition.
        /// </summary>
        /// <returns></returns>
        public List<string> GetVariables()
        {
            return GetVariablesInternal().Select(Va => (Va.StartsWith("NamedElement_") ? ((char)TableElementTypeEnum.NamedElement).ToString() + Va.Substring("NamedElement_".Length) : Va)).ToList();
        }



        private void InitCondition()
        {
            string C = Condition.Replace(((char)TableElementTypeEnum.NamedElement).ToString(), "NamedElement_");
            ConditionExpression = null;

            if (C.IsNullOrWhiteSpace())
            {
                Log.Warning("No condition has been set for {0} named {1}.".Build(this.GetType().Name, Name));
                return;
            };

            List<string> Variables = GetVariablesInternal();

            ExpressionContext Context = new ExpressionContext();
            Context.Options.ParseCulture = System.Globalization.CultureInfo.InvariantCulture;
            try
            {
                Context.Imports.AddType(typeof(Math));

                foreach (string V in Variables)
                {
                    int P = 0;
                    while (P < C.Length && P >= 0)
                    {
                        P = C.IndexOf(V, P + 1, StringComparison.OrdinalIgnoreCase);
                        if (P < 0) break;
                        if (!C.Substring(P, V.Length + 1).Equals(V + ".", StringComparison.OrdinalIgnoreCase))
                        {
                            C = C.Substring(0, P) + "{0}.Value".Build(V) + C.Substring(P + V.Length);
                        }
                    }
                    //                    C = C.Replace(V, "{0}.Value".Build(V), StringComparison.OrdinalIgnoreCase);

                    TableElement TE = null;

                    if (V.StartsWith("NamedElement_"))
                    {
                        Table.TableElements.UpdateState(new TableElementData(V.Substring("NamedElement_".Length),0));
                        TE = Table.TableElements[V.Substring("NamedElement_".Length)];
                    }
                    else
                    {

                        Table.TableElements.UpdateState(new TableElementData((TableElementTypeEnum)V[0], V.Substring(1).ToInteger(), 0));
                        TE = Table.TableElements[(TableElementTypeEnum)V[0], V.Substring(1).ToInteger()];
                    }


                    Context.Variables[V] = TE;
                }
            }
            catch (Exception E)
            {
                Log.Exception("A exception has occurred while setting up the variables for condition {0} of effect {1}.".Build(Condition, Name), E);
                return;

            }

            try
            {
                ConditionExpression = Context.CompileGeneric<bool>(C);

            }
            catch (Exception E)
            {
                Log.Exception("A exception has occurred while compiling the condition {0} (internally translated to {2}) of effect {1}.".Build(Condition, Name, C), E);
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

                    Log.Exception("A exception occurred when evaluating the expression {0} of effect {1}. Effect will be deactivated.".Build(ConditionExpression.Text, Name), E);
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
