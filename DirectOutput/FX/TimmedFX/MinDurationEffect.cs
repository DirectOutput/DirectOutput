using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.Table;

namespace DirectOutput.FX.TimmedFX
{
    /// <summary>
    /// This effect enforces a minimum duration on the effect calls.<br/>
    /// Calls which are setting a effect to active (having a trigger value which is not equal 0 or null) are forwarded directly to the TargetEffect.<br/>
    /// Calls setting the effect to inactive (having a trigger value of 0) are only forwarded to the TargetEffect after the specified minimum duration has expired.<br/>
    /// \image html FX_MinDuration.png "MinDuration effect"
    /// </summary>
    public class MinDurationEffect : EffectEffectBase
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
        /// Gets or sets the minimal duration for the effect in milliseconds.
        /// </summary>
        /// <value>
        /// The minimal effect duration in milliseconds.
        /// </value>
        public int MinDurationMs
        {
            get { return _DurationMs; }
            set { _DurationMs = value; }
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
        /// Triggers the MinDurationEffect with the given TableElementData.<br/>
        /// The minimal duration is started, if the value portion of the TableElementData parameter is !=0. 
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(TableElementData TableElementData)
        {
            if (TargetEffect != null)
            {
                if (TableElementData.Value != 0)
                {
                    if (!Active)
                    {
                        DurationStart = DateTime.Now;
                        TriggerTargetEffect(TableElementData);
                        Active = true;
                    } else if(RetriggerBehaviour==RetriggerBehaviourEnum.Restart) {
                        DurationStart = DateTime.Now;
                    }
                }
                else
                {
                    if (Active && TableElementData.TableElementType == UntriggerData.TableElementType && TableElementData.Number == UntriggerData.Number)
                    {
                        if ((DateTime.Now - DurationStart).TotalMilliseconds >= MinDurationMs)
                        {
                            MinDurationEnd();
                        }
                        else
                        {
                            Table.Pinball.Alarms.RegisterAlarm(MinDurationMs - (int)(DateTime.Now - DurationStart).TotalMilliseconds, MinDurationEnd);
                        }
                    }

                }

            }
        }



        private void MinDurationEnd()
        {
            if (Active)
            {
                TableElementData D = UntriggerData;
                D.Value = 0;
                TriggerTargetEffect(D);
            }
            Active = false;
        }



        /// <summary>
        /// Finishes the DurationEffect.
        /// </summary>
        public override void Finish()
        {

            try
            {
                Table.Pinball.Alarms.UnregisterAlarm(MinDurationEnd);
            }
            catch { }
            //MinDurationEnd();
            Active = false;
            base.Finish();
        }



    }
}
