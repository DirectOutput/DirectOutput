using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.FX;
using DirectOutput.Table;
using System.Xml.Serialization;

namespace DirectOutput.FX
{
    /// <summary>
    /// Handles the assignemt of a effect to a AssignedEffectList.
    /// </summary>
    public class AssignedEffect
    {


        #region EffectName
        private string _EffectName;
        /// <summary>
        /// Name of the AssignedEffect.<br/>
        /// Triggers EffectNameChanged if value is changed.
        /// </summary>    
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

        private void TableElementEffect_EffectNameChanged(object sender, EventArgs e)
        {
            _Effect = null;
        }

        #endregion

        #region Order
        private int _Order;
        /// <summary>
        /// Order of type int.<br/>
        /// Triggers OrderChanged if value is changed.
        /// </summary>    
        public int Order
        {
            get { return _Order; }
            set
            {
                if (_Order != value)
                {
                    _Order = value;
                    if (OrderChanged != null)
                    {
                        OrderChanged(this, new EventArgs());
                    }
                }
            }
        }
        /// <summary>
        /// Event is fired if the value of the property Order is changed.
        /// </summary>
        public event EventHandler<EventArgs> OrderChanged;
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

        private void ResolveEffectName(Pinball Pinball)
        {
            if (!EffectName.IsNullOrWhiteSpace() && Pinball.Effects.Contains(EffectName))
            {
                Effect = Pinball.Effects[EffectName];
            };

        }

        #endregion


        /// <summary>
        /// Triggers the assigned Effect.
        /// \remark
        /// If the assigned effect throws a exception the effect will be deactivated.
        /// </summary>
        public void Trigger(TableElement TableElement)
        {
            if (Effect != null)
            {
                try
                {
                    Effect.Trigger(TableElement);
                }
                catch (Exception E)
                {
                    Log.Exception("A exception occured when triggering effect {0} for table element {1} {2} with value {3}. Effect assignement will be deactivated.".Build(new object[] {Effect.Name,TableElement.TableElementType,TableElement.Number,TableElement.Value}),E);
             
                    Effect = null;
                }
            }
        }


        /// <summary>
        /// Initializes the AssignedEffect.
        /// </summary>
        /// <param name="Pinball">The Pinball object to which the effect belongs.</param>
        public void Init(Pinball Pinball)
        {

            ResolveEffectName(Pinball);
        }

        /// <summary>
        /// Finishes this instance of the AssignedEffect object.
        /// </summary>
        public void Finish()
        {

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

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignedEffect"/> class for the specified <paramref name="EffectName"/> with a specific order number.
        /// </summary>
        /// <param name="EffectName">Name of the effect.</param>
        /// <param name="Order">The order of the effect.</param>
        public AssignedEffect(string EffectName, int Order)
            : this(EffectName)
        {
            this.Order = Order;
        }

        #endregion


    }
}
