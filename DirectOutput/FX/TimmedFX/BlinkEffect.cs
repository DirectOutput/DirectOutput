using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.TimmedFX
{
    /// <summary>
    /// Blink effect which triggers a TargetEffect at specified intervalls with active (org value of TableElementData used in Trigger method is used to trigger the TargetEffect) and inactive (uses 0 as the Value of the TableElementData to trigger the TargetEffect) values.<br/>
    /// \image html FX_Blink.png "Blink effect"
    /// </summary>
    public class BlinkEffect : EffectEffectBase
    {

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

        /// <summary>
        /// Gets a value indicating whether this <see cref="BlinkEffect"/> is currently active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise <c>false</c>.
        /// </value>
        public bool Active { get; private set; }


        private bool BlinkEnabled = false;
        private bool BlinkState = false;
        private int BlinkOrgTableElementDataValue = 1;
        private Table.TableElementData BlinkTableElementData;

        private void StartBlinking(Table.TableElementData TableElementData)
        {
            if (!BlinkEnabled)
            {
                BlinkTableElementData = TableElementData.Clone();
                BlinkOrgTableElementDataValue = BlinkTableElementData.Value;
                BlinkEnabled = true;
                BlinkState = false;
                DoBlink();
            }
        }

        private void StopBlinking()
        {
            BlinkEnabled = false;
        }

        private void DoBlink()
        {
            BlinkState = !BlinkState;
            if (BlinkState)
            {
                BlinkTableElementData.Value = BlinkOrgTableElementDataValue;
                Table.Pinball.Alarms.RegisterAlarm(DurationActiveMs, DoBlink);
            }
            else
            {
                BlinkTableElementData.Value = 0;
                if (BlinkEnabled)
                {
                    Table.Pinball.Alarms.RegisterAlarm(DurationInactiveMs, DoBlink);
                }
            }
            TargetEffect.Trigger(BlinkTableElementData);
            
        }



        /// <summary>
        /// Triggers the BlinkEffect with the given TableElementData.<br/>
        /// If the Value property of the TableElementData is >0 or TableElementData is null (static effect), the blinking gets started. If the TableElementData Value property is 0, the blinking is stopped.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (TargetEffect != null)
            {
                if (TableElementData == null)
                {
                    StartBlinking(new Table.TableElementData(TableElementTypeEnum.Unknown,0,1));
                }
                else if (TableElementData.Value != 0)
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
            Table.Pinball.Alarms.UnregisterAlarm(DoBlink);
            base.Finish();
        }

    }
}
