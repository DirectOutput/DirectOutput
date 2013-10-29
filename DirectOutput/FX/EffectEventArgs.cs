using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX
{

    /// <summary>
    /// The EffectEventArgs class is used for events triggered by IEffect objects
    /// </summary>
    public class EffectEventArgs: EventArgs
        {
            /// <summary>
            /// IEffect object which has triggered the event.
            /// </summary>
            public IEffect Effect { get; set; }
            public EffectEventArgs() { }
            public EffectEventArgs(IEffect Effect)
            {
                this.Effect = Effect;
            }
    }


    /// <summary>
    /// EventArgs for the BeforeEffectNameChanged event
    /// </summary>
    public class BeforeEffectNameChangeAventArgs : EffectEventArgs
    {

        /// <summary>
        /// New name for the Effect 
        /// </summary>
        public string NewName { get; set; }
        /// <summary>
        /// If CancelNameChanges is set to true, the Name of the Effect will not be changed and a exception is thrown.
        /// </summary>
        public bool CancelNameChange { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeEffectNameChangeAventArgs"/> class.
        /// </summary>
        public BeforeEffectNameChangeAventArgs() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeEffectNameChangeAventArgs"/> class.
        /// </summary>
        /// <param name="Effect">The effect.</param>
        /// <param name="NewName">The new name.</param>
        public BeforeEffectNameChangeAventArgs(IEffect Effect, string NewName)
            : base(Effect)
        {
            this.NewName = NewName;
        }



    }

}
