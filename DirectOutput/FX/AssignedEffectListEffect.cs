using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX
{
    public class AssignedEffectOrder:AssignedEffect
    {
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


                #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AssignedEffectOrder"/> class.
        /// </summary>
        public AssignedEffectOrder()
        {
            EffectNameChanged += new EventHandler<EventArgs>(TableElementEffect_EffectNameChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignedEffectOrder"/> class for the specified <paramref name="EffectName"/>.
        /// </summary>
        /// <param name="EffectName">Name of the effect.</param>
        public AssignedEffectOrder(string EffectName)
            : this()
        {
            this.EffectName = EffectName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignedEffectOrder"/> class for the specified <paramref name="EffectName"/> with a specific order number.
        /// </summary>
        /// <param name="EffectName">Name of the effect.</param>
        /// <param name="Order">The order of the effect.</param>
        public AssignedEffectOrder(string EffectName, int Order)
            : this(EffectName)
        {
            this.Order = Order;
        }

        #endregion
    }
}
