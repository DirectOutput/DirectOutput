using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DirectOutput.FX
{
    /// <summary>
    /// Base class for effects targeting another effect.
    /// </summary>
    public abstract class EffectEffectBase: EffectBase
    {
        #region EffectName
        private string _TargetEffectName;
        /// <summary>
        /// Name of the target effect.<br/>
        /// Triggers EffectNameChanged if value is changed.
        /// </summary>    
        public string TargetEffectName
        {
            get { return _TargetEffectName; }
            set
            {
                if (_TargetEffectName != value)
                {
                    _TargetEffectName = value;
                    TargetEffect = null;
                    
                }
            }
        }




        #endregion


        #region Effect
        private IEffect _TargetEffect;
        /// <summary>
        /// TargetEffect for the Effect (ReadOnly).<br/>
        /// The property is resolved from the TargetEffectName. If TargetEffectName is empty or unknown this property will return null.
        /// </summary>
        [XmlIgnoreAttribute]
        public IEffect TargetEffect
        {
            get
            {
                return _TargetEffect;
            }
            protected set
            {
                _TargetEffect = value;
            }
        }

        private void ResolveEffectName(Table.Table Table)
        {
            if (!TargetEffectName.IsNullOrWhiteSpace() && Table.Effects.Contains(TargetEffectName))
            {
                TargetEffect = Table.Effects[TargetEffectName];
            };

        }

        #endregion


        public override void Init(Table.Table Table)
        {
            ResolveEffectName(Table);
        }

        public override void Finish()
        {
            base.Finish();
        }
    
    }
}
