using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DirectOutput.FX.TimmedFX
{
    /// <summary>
    /// Blink effect which triggers a TargetEffect at specified intervalls with active (org value of TableElementData used in Trigger method is used to trigger the TargetEffect) and inactive (uses 0 as the Value of the TableElementData to trigger the TargetEffect) values.<br/>
    /// \image html FX_Blink.png "Blink effect"
    /// </summary>
    public class BlinkEffect : EffectEffectBase
    {
        private int _ActiveValue = -1;

        /// <summary>
        /// Gets or sets the high value for the blinking.
        /// </summary>
        /// <value>
        /// The high value for the blinking. Values between 0 and 255 define the actual values which have to be output during the on phase of the blinking. A value of -1 defines that the value which has been received by the trigger event is used.
        /// </value>
        public int HighValue
        {
            get { return _ActiveValue; }
            set { _ActiveValue = value.Limit(-1, 255); }
        }

        private int _LowValue = 0;

        /// <summary>
        /// Gets or sets the low value for the blinking.
        /// </summary>
        /// <value>
        /// The low value for the blinking (0-255).
        /// </value>
        public int LowValue
        {
            get { return _LowValue; }
            set { _LowValue = value.Limit(0, 255); }
        }






        private int _DurationActiveMs = 500;

        /// <summary>
        /// Gets or sets the active duration for the blinking in milliseconds.
        /// </summary>
        /// <value>
        /// The active duration of the blinking in milliseconds.
        /// </value>
        public int DurationActiveMs
        {
            get { return _DurationActiveMs; }
            set { _DurationActiveMs = value.Limit(1, int.MaxValue); }
        }


        private int _DurationInactiveMs = 500;

        /// <summary>
        /// Gets or sets the inactive duration for the blinking in milliseconds.
        /// </summary>
        /// <value>
        /// The inactive duration of the blinking in milliseconds.
        /// </value>
        public int DurationInactiveMs
        {
            get { return _DurationInactiveMs; }
            set { _DurationInactiveMs = value.Limit(1, int.MaxValue); }
        }


        private BlinkEffectUntriggerBehaviourEnum _UntriggerBehaviour=BlinkEffectUntriggerBehaviourEnum.Immediate;

        /// <summary>
        /// Gets or sets the untrigger behaviour which defines how the blinking stops.
        /// </summary>
        /// <value>
        /// The untrigger behaviour defines how the blinking stops.
        /// </value>
        public BlinkEffectUntriggerBehaviourEnum UntriggerBehaviour
        {
            get { return _UntriggerBehaviour; }
            set { _UntriggerBehaviour = value; }
        }
        


        /// <summary>
        /// Gets a value indicating whether this <see cref="BlinkEffect"/> is currently active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise <c>false</c>.
        /// </value>
        [XmlIgnoreAttribute]
        public bool Active { get; private set; }


        private bool BlinkEnabled = false;
        private bool BlinkState = false;
        private int BlinkOrgTableElementDataValue = 1;
        private Table.TableElementData BlinkTableElementData;

        private void StartBlinking(Table.TableElementData TableElementData)
        {
            BlinkTableElementData = TableElementData;
            BlinkOrgTableElementDataValue = BlinkTableElementData.Value;

            if (!BlinkEnabled)
            {
                BlinkEnabled = true;
                BlinkState = false;
                DoBlink();
            }
            else
            {
                if (BlinkState)
                {
                    BlinkTableElementData.Value = (HighValue >= 0 ? HighValue : BlinkOrgTableElementDataValue);
                }
            }
        }

        private void StopBlinking()
        {
            BlinkEnabled = false;
            if (UntriggerBehaviour == BlinkEffectUntriggerBehaviourEnum.Immediate)
            {
                Table.Pinball.Alarms.UnregisterAlarm(DoBlink);
                BlinkTableElementData.Value = 0;
            };
        }

        private void DoBlink()
        {
            BlinkState = !BlinkState;
            if (BlinkState)
            {
                BlinkTableElementData.Value = (HighValue >= 0 ? HighValue : BlinkOrgTableElementDataValue);
                Table.Pinball.Alarms.RegisterAlarm(DurationActiveMs, DoBlink);
            }
            else
            {
                if (BlinkEnabled)
                {
                    BlinkTableElementData.Value = LowValue;
                    Table.Pinball.Alarms.RegisterAlarm(DurationInactiveMs, DoBlink);
                }
                else
                {
                    BlinkTableElementData.Value = 0;
                }
            }
            TargetEffect.Trigger(BlinkTableElementData);

        }



        /// <summary>
        /// Triggers the BlinkEffect with the given TableElementData.<br/>
        /// If the Value property of the TableElementData is >0, the blinking gets started. If the TableElementData Value property is 0, the blinking is stopped.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (TargetEffect != null)
            {
                if (TableElementData.Value != 0)
                {
                    StartBlinking(TableElementData);
                }
                else
                {
                    StopBlinking();

                }
            }
        }



        /// <summary>
        /// Finishes the BlinkEffect.
        /// </summary>
        public override void Finish()
        {
            try
            {
                Table.Pinball.Alarms.UnregisterAlarm(DoBlink);
            }
            catch { }
            base.Finish();
        }

    }
}
