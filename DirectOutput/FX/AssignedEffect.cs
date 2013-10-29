using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.FX;
using DirectOutput.Table;
using System.Xml.Serialization;

namespace DirectOutput.FX
{
    //TODO: Add conditions (expressions that evluate to true or false) for assigned effects. CSScript makes this easy, but unlopading of changed effects will not be possible without restarting the framework.
    /// <summary>
    /// Handles the assignemt of a effect to a AssignedEffectList.
    /// </summary>
    public class AssignedEffect
    {


        #region EffectName
        private string _EffectName;
        /// <summary>
        /// Name of the AssignedEffect.<br />
        /// Triggers EffectNameChanged if value is changed.
        /// </summary>
        /// <value>
        /// The name of the assigned effect.
        /// </value>
        public string EffectName
        {
            get { return _EffectName; }
            set
            {
                if (_EffectName != value)
                {
                    _EffectName = value;
                    if (EffectNameChanged != null)
                    {
                        EffectNameChanged(this, new EventArgs());
                    }
                    
                }
            }
        }
        /// <summary>
        /// Event is fired if the value of the property EffectName is changed.
        /// </summary>
        public event EventHandler<EventArgs> EffectNameChanged;

        /// <summary>
        /// Handles the EffectNameChanged event of the TableElementEffect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void TableElementEffect_EffectNameChanged(object sender, EventArgs e)
        {
            _Effect = null;
        }

        #endregion


        #region Effect
        private IEffect _Effect;
        /// <summary>
        /// Effect for the AssignedEffect (ReadOnly).<br/>
        /// The property is resolved from the EffectName. If EffectName is empty or unknown this property will return null.
        /// </summary>
        [XmlIgnoreAttribute]
        public IEffect Effect
        {
            get
            {
                return _Effect;
            }
            private set
            {
                _Effect = value;
            }
        }

        private void ResolveEffectName(Table.Table Table)
        {
            if (!EffectName.IsNullOrWhiteSpace() && Table.Effects.Contains(EffectName))
            {
                Effect = Table.Effects[EffectName];
            };

        }

        #endregion


        /// <summary>
        /// Triggers the assigned Effect.
        /// <remarks> If the assigned effect throws a exception the effect will be deactivated.</remarks>
        /// </summary>
        /// <param name="TableElementData">The table element data.</param>
        public void Trigger(TableElementData TableElementData)
        {
            if (Effect != null)
            {
                try
                {
                    Effect.Trigger(TableElementData);
                }
                catch (Exception E)
                {
                    if (TableElementData != null)
                    {
                        Log.Exception("A exception occured when triggering effect {0} for table element {1} {2} with value {3}. Effect assignement will be deactivated.".Build(new object[] { Effect.Name, TableElementData.TableElementType, TableElementData.Number, TableElementData.Value }), E);
                    }
                    else
                    {
                        Log.Exception("A exception occured when triggering effect {0} as a static effect.Effect assignement will be deactivated.".Build(new object[] { Effect.Name}), E);
                    }
                    Effect = null;
                }
            }
        }


        /// <summary>
        /// Initializes the AssignedEffect.
        /// </summary>
        /// <param name="Table">The table which contains the AssignedEffect.</param>
        public void Init(Table.Table Table)
        {

            ResolveEffectName(Table);
        }

        /// <summary>
        /// Finishes this instance of the AssignedEffect object.
        /// </summary>
        public void Finish()
        {
            Effect = null;
        }


        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AssignedEffect"/> class.
        /// </summary>
        public AssignedEffect()
        {
            EffectNameChanged += new EventHandler<EventArgs>(TableElementEffect_EffectNameChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignedEffect"/> class for the specified <paramref name="EffectName"/>.
        /// </summary>
        /// <param name="EffectName">Name of the effect.</param>
        public AssignedEffect(string EffectName)
            : this()
        {
            this.EffectName = EffectName;
        }



        #endregion


    }
}
