using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.Table;

namespace DirectOutput.FX.TimmedFX
{
    /// <summary>
    /// Limits the max duration of the effect to the specified number of milliseconds.
    /// </summary>
   public class MaxDurationEffect:EffectEffectBase
    {
        private RetriggerBehaviourEnum _RetriggerBehaviour = RetriggerBehaviourEnum.Ignore;

        /// <summary>
        /// Gets or sets the retrigger behaviour.<br/>
        /// The setting defines the behaviour of the effect if it is retriggered while it is still active. <br/>
        /// This settings is only relevant, if the effect can be called from more than one table element.
        /// </summary>
        /// <value>
        /// Valid values are Restart (restarts the minimal duration) or Ignore (keeps the org duration).
        /// </value>
        public RetriggerBehaviourEnum RetriggerBehaviour
        {
            get { return _RetriggerBehaviour; }
            set { _RetriggerBehaviour = value; }
        }

        private int _DurationMs = 500;

        /// <summary>
        /// Gets or sets the max duration for the effect in milliseconds.
        /// </summary>
        /// <value>
        /// The max effect duration in milliseconds.
        /// </value>
        public int MaxDurationMs
        {
            get { return _DurationMs; }
            set { _DurationMs = value.Limit(1,int.MaxValue); }
        }



        /// <summary>
        /// Gets a value indicating whether this <see cref="MinDurationEffect"/> is currently holding back trigger calls on the TargetEffect having a value of 0.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise <c>false</c>.
        /// </value>
        [XmlIgnoreAttribute]
        public bool Active { get; private set; }

        private TableElementData UntriggerData;
        private DateTime DurationStart = DateTime.MinValue;


        /// <summary>
        /// Triggers the effect with the given TableElementData.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (TableElementData.Value != 0)
            {
                
                if (!Active || RetriggerBehaviour == RetriggerBehaviourEnum.Restart)
                {
                    TriggerTargetEffect(TableElementData);
                    UntriggerData=TableElementData;
                    Table.Pinball.Alarms.RegisterAlarm(MaxDurationMs, DurationEnd);
                    Active = true;
                }

            }
            else
            {
                if (Active && UntriggerData.TableElementType == TableElementData.TableElementType && UntriggerData.Number == TableElementData.Number)
                {
                    TriggerTargetEffect(TableElementData);
                    Table.Pinball.Alarms.UnregisterAlarm(DurationEnd);
                    Active = false;
                }

            }

        }


        private void DurationEnd()
        {
            TriggerTargetEffect(UntriggerData);
            Active = false;

        }
    }
}
