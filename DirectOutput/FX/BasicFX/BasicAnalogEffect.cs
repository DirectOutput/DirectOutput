using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys;
using DirectOutput.Table;
using DirectOutput.Cab.Toys.Basic;

namespace DirectOutput.FX.BasicFX
{
    /// <summary>
    /// The <cref="BasicAnalogEffect"/> is used to set the state of toys implementing <see cref="IAnalogToy"/> based on the value of a <see cref="TableElement"/>.<br/>
    /// </summary>
    public class BasicAnalogEffect : EffectBase, IEffect
    {
        

        private string _AnalogToyName;
 
        /// <summary>
        /// Name of the <see cref="IAnalogToy"/>.
        /// </summary>
        /// <value>Name of the <see cref="IAnalogToy"/>.</value>
        public string AnalogToyName
        {
            get { return _AnalogToyName; }
            set
            {
                if (_AnalogToyName != value)
                {
                    _AnalogToyName = value;
                    _AnalogToy = null;
                }
            }
        }

        private void ResolveName(Table.Table Table)
        {
            if (!AnalogToyName.IsNullOrWhiteSpace() && Table.Pinball.Cabinet.Toys.Contains(AnalogToyName))
            {
                if (Table.Pinball.Cabinet.Toys[AnalogToyName] is IAnalogToy)
                {
                    _AnalogToy = (IAnalogToy)Table.Pinball.Cabinet.Toys[AnalogToyName];
                }
            }
        }

        private IAnalogToy _AnalogToy;

        /// <summary>
        /// Reference to the <see cref="IAnalogToy"/> specified in the <paramref name="AnalogToyName"/> property.<br/>
        /// If the  <paramref name="AnalogToyName"/> property is empty or contains a unknown name or the name of a toy which does not implement <see cref="IAnalogToy"/> this property will return null.
        /// </summary>
        public IAnalogToy AnalogToy
        {
            get
            {
                return _AnalogToy;
            }
        }


        /// <summary>
        /// Triggers the effect.<br/>
        /// If the value of the TableElement is 0 the value for the <see cref="IAnalogToy"/> will be set to the value of the property ValueOff, if the value of the TableElement is greater than 0 the value of the property ValueOn will be used.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param> 
        public override void Trigger(TableElementData TableElementData)
        {
            if (AnalogToy != null)
            {

                    AnalogToy.SetValue (TableElementData.Value == 0 ? ValueOff : ValueOn);

            }
        }

        private int _ValueOn = 255;

        /// <summary>
        /// Gets or sets the value which will be set for the IAnalogToy if the value of the TableElement supplied to the trigger method is >0.
        /// </summary>
        /// <value>
        /// The value which will be set for the IAnalogToy if the value of the TableElement supplied to the trigger method is >0.
        /// </value>
        public int ValueOn
        {
            get { return _ValueOn; }
            set { _ValueOn = value.Limit(0,255); }
        }

        private int _ValueOff = 0;

        /// <summary>
        /// Gets or sets the value which will be set for the IAnalogToy if the value of the TableElement supplied to the trigger method is 0.
        /// </summary>
        /// <value>
        /// The value which will be set for the IAnalogToy if the value of the TableElement supplied to the trigger method is 0.
        /// </value>
        public int ValueOff
        {
            get { return _ValueOff; }
            set { _ValueOff = value.Limit(0,255); }
        }



        /// <summary>
        /// Initializes the <see cref="IAnalogToy" />.
        /// </summary>
        public override void Init(Table.Table Table)
        {

            ResolveName(Table);

        }
        /// <summary>
        /// Finishes the <see cref="IAnalogToy"/>.
        /// </summary>
        public override void Finish()
        {
            if (AnalogToy != null) AnalogToy.Reset();
  
            _AnalogToy = null;
            
        }

    }
}
