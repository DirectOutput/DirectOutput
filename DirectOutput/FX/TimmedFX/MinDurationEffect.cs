using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

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
        private RetriggerBehaviourEnum _RetriggerBehaviour;

        /// <summary>
        /// Gets or sets the retrigger behaviour.<br/>
        /// The setting defines the behaviour of the effect if it is retriggered while it is still active. <br/>
        /// This settings is only relevant, if the effect can be called from more than one table element.
        /// </summary>
        /// <value>
        /// Valid values are RestartEffect (Restarts the minimal duration) or IgnoreRetrigger (keeps the org duration).
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

        private Queue<Table.TableElementData> UntriggerData = new Queue<Table.TableElementData>();

        /// <summary>
        /// Triggers the MinDurationEffect with the given TableElementData.<br/>
        /// The minimal duration is started, if the value portion of the TableElementData parameter is !=0. 
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (TargetEffect != null)
            {
                Table.TableElementData TED =  TableElementData;
                if (TED.Value != 0)
                {
                    TargetEffect.Trigger(TED);
                    if (!Active || RetriggerBehaviour == RetriggerBehaviourEnum.RestartEffect)
                    {
                        Table.Pinball.Alarms.RegisterAlarm(MinDurationMs, MinDurationEnd);
                        Active = true;
                    }
                }
                else
                {
                    if (Active)
                    {
                        //Min duration is active, put data in queue
                        UntriggerData.Enqueue(TED.Clone());
                    }
                    else
                    {
                        //Min duration has ended call target effect directly
                        TargetEffect.Trigger(TED);
                    }

                }
            }
        }



        private void MinDurationEnd()
        {
            while (UntriggerData.Count>0)
            {
                TargetEffect.Trigger(UntriggerData.Dequeue());
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
            catch  {}
            //MinDurationEnd();
            Active = false;
            base.Finish();
        }



    }
}
