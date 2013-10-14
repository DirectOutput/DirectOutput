using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DirectOutput.FX.TimmedFX
{
    /// <summary>
    /// Duration effect which triggers a specified target effect for a specified duration.<br/>
    /// When this effect is triggered it triggers the target effect immediately with the same data it has received. After the specified duration it calls trigger on the target effect again with data for the same table elmenet, but with the value changed to 0.<br/>
    /// \image html FX_Duration.png "Duration effect"
    /// </summary>
    public class DurationEffect : EffectEffectBase
    {
        private RetriggerBehaviourEnum _RetriggerBehaviour;

        /// <summary>
        /// Gets or sets the retrigger behaviour.<br/>
        /// The setting defines the behaviour of the effect if it is retriggered while it is still active. <br/>
        /// This settings is only relevant, if the effect can be called from more than one table element.
        /// </summary>
        /// <value>
        /// Valid values are RestartEffect (Restarts the duration) or IgnoreRetrigger (keeps the org duration).
        /// </value>
        public RetriggerBehaviourEnum RetriggerBehaviour
        {
            get { return _RetriggerBehaviour; }
            set { _RetriggerBehaviour = value; }
        }

        private int _DurationMs = 500;

        /// <summary>
        /// Gets or sets the duration for the effect in milliseconds.
        /// </summary>
        /// <value>
        /// The effect duration in milliseconds.
        /// </value>
        public int DurationMs
        {
            get { return _DurationMs; }
            set { _DurationMs = value; }
        }


        /// <summary>
        /// Gets a value indicating whether this <see cref="DurationEffect"/> is currently active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise <c>false</c>.
        /// </value>
        [XmlIgnoreAttribute]
        public bool Active { get; private set; }

        /// <summary>
        /// Triggers the DurationEffect with the given TableElementData.<br/>
        /// The duration is started, if the value portion of the TableElementData parameter is !=0. 
        /// Trigger calls with a TableElement value=0 have no effect.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (TargetEffect != null)
            {
                if (!Active || RetriggerBehaviour == RetriggerBehaviourEnum.RestartEffect)
                {
                    if (TableElementData.Value != 0)
                    {
                        TargetEffect.Trigger(TableElementData);
                        Table.Pinball.Alarms.RegisterAlarm(DurationMs, DurationEnd, TableElementData.Clone() );
                    }
                    Active = true;
                }
            }
        }


        private void DurationEnd(object TableElementData)
        {
            
            Table.TableElementData TED = (Table.TableElementData)TableElementData;
            TED.Value = 0;
            TargetEffect.Trigger(TED);
            Active = false;
        }

        /// <summary>
        /// Finishes the DurationEffect.
        /// </summary>
        public override void Finish()
        {
            Table.Pinball.Alarms.UnregisterAlarm(DurationEnd);
            Active = false;
            base.Finish();
        }

    }
}
