using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Generic;
using DirectOutput.Table;

namespace DirectOutput.FX
{

    /// <summary>
    /// Abstract base class for IEffect objects.
    /// This class does inherit IEffect.
    /// </summary>
    public abstract class EffectBase : NamedItemBase,IEffect
    {


        /// <summary>
        /// Triggers the effect for the given TableElement
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public abstract void Trigger(TableElementData TableElementData);


        /// <summary>
        /// Init must do all necessary initialization work after the IEffect object has been instanciated
        /// </summary>
        /// <param name="Pinball">Pinball object</param>
        public abstract void Init(Pinball Pinball);


        /// <summary>
        /// Finish must do all necessary cleanupwork before a IEffect object is discarded
        /// </summary>
        public virtual void Finish() {}


    }
}
