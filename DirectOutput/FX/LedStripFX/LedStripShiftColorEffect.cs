using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Color;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.LedStripFX
{
    public class LedStripShiftColorEffect : LedStripEffectBase
    {
        private const int RefreshIntervalMs = 30;

        private RGBAColor _ActiveColor = new RGBAColor(0xff, 0xff, 0xff, 0xff);

        /// <summary>
        /// Gets or sets the active color.
        /// The ColorSetMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The active color.
        /// </value>
        public RGBAColor ActiveColor
        {
            get { return _ActiveColor; }
            set { _ActiveColor = value; }
        }

        private RGBAColor _InactiveColor = new RGBAColor(0, 0, 0, 0);

        /// <summary>
        /// Gets or sets the inactive color.
        /// The ColorSetMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The inactive color.
        /// </value>
        public RGBAColor InactiveColor
        {
            get { return _InactiveColor; }
            set { _InactiveColor = value; }
        }


        private ColorSetModeEnum _ColorSetMode = ColorSetModeEnum.OnOff;

        /// <summary>
        /// Gets or sets the color set mode.
        /// </summary>
        /// <value>
        /// Fade (active and inactive color will fade depending on trigger value) or OnOff (actvice color is used for triger values >0, otherwise inactive color will be used).
        /// </value>
        public ColorSetModeEnum ColorSetMode
        {
            get { return _ColorSetMode; }
            set { _ColorSetMode = value; }
        }


        private ShiftDirectionEnum _ShiftDirection = ShiftDirectionEnum.Right;

        /// <summary>
        /// Gets or sets the shift direction resp. the direction in which the color moves.
        /// </summary>
        /// <value>
        /// The shift direction (Left, Right, Up, Down).
        /// </value>
        public ShiftDirectionEnum ShiftDirection
        {
            get { return _ShiftDirection; }
            set { _ShiftDirection = value; }
        }

        private float _ShiftSpeed = 200;

        /// <summary>
        /// Gets or sets the shift speed in percentage of the effect area per second.
        /// A speed setting of 100 will shift through the whole area of the effect in 1 second. 200 will shift through the effect area in .5 seconds. 33 will shift through the effect area in approx. 3 seconds. 
        /// Max. speed is 10000 (shift through the effect area in 1/100 seconds). Min. speed is 1 (shifts through the effect area in 100 seconds). 
        /// </summary>
        /// <value>
        /// The shift speed in percentage of the effect area (Left, Top, Width, Height properties) per second .
        /// </value>
        public float ShiftSpeed
        {
            get { return _ShiftSpeed; }
            set { _ShiftSpeed = value.Limit(1, 10000); }
        }



        private void DoStep()
        {
            if (!Active)
            {
                Table.Pinball.Alarms.RegisterIntervalAlarm(RefreshIntervalMs, DoStep);
                Active = true;
            }
            
            int TotalSteps = (int)((double)100000 / ShiftSpeed / RefreshIntervalMs);

            int NumberOfLeds = (ShiftDirection == ShiftDirectionEnum.Left || ShiftDirection == ShiftDirectionEnum.Right ? AreaWidth : AreaHeight);
            int StepsPerLed = TotalSteps / NumberOfLeds;


            float FromLedNr = NumberOfLeds - 1;
            float ToLedNr = 0;
            float[] Value = new float[NumberOfLeds];

            int LastValue = LastDiscardedValue;

            foreach (KeyValuePair<int, int> KV in TriggerValueBuffer)
            {
                ToLedNr = (KV.Key - CurrentStep) / StepsPerLed;

                if (FromLedNr.Floor() == ToLedNr.Floor())
                {
                    Value[(int)FromLedNr.Floor()] += (FromLedNr - ToLedNr) * LastValue;
                }
                else
                {
                    if (!FromLedNr.IsIntegral())
                    {
                        Value[(int)FromLedNr.Floor()] += (FromLedNr - FromLedNr.Floor()) * LastValue;
                    }

                    int ToNr = (int)ToLedNr.Ceiling();
                    for (int i = (int)FromLedNr.Floor(); i >= ToNr; i--)
                    {
                        Value[i] = LastValue;
                    }
                    if (!ToLedNr.IsIntegral())
                    {
                        Value[(int)ToLedNr.Floor()] += (ToLedNr.Ceiling() - ToLedNr) * LastValue;
                    }

                }
                FromLedNr=ToLedNr;
                LastValue = KV.Value;
            }


            #region Data ouput
            switch (ShiftDirection)
            {
                case ShiftDirectionEnum.Right:
                    for (int i = 0; i < NumberOfLeds; i++)
                    {
                        int V = ((int)Value[i]).Limit(0, 255);
                        if (V > 0 && ColorSetMode == ColorSetModeEnum.OnOff) { V = 255; }
                        RGBAData D;
                        D.Red = InactiveColor.Red + (int)((float)(ActiveColor.Red - InactiveColor.Red) * V / 255).Limit(0, 255);
                        D.Green = InactiveColor.Green + (int)((float)(ActiveColor.Green - InactiveColor.Green) * V / 255).Limit(0, 255);
                        D.Blue = InactiveColor.Blue + (int)((float)(ActiveColor.Blue - InactiveColor.Blue) * V / 255).Limit(0, 255);
                        D.Alpha = InactiveColor.Alpha + (int)((float)(ActiveColor.Alpha - InactiveColor.Alpha) * V / 255).Limit(0, 255);

                        for (int y = AreaTop; y <= AreaBottom; y++)
                        {
                            LedStripLayer[AreaLeft + i, y] = D;
                        }
                    }
                    break;
                case ShiftDirectionEnum.Up:
                    for (int i = 0; i < NumberOfLeds; i++)
                    {
                        int V = ((int)Value[i]).Limit(0, 255);
                        if (V > 0 && ColorSetMode == ColorSetModeEnum.OnOff) { V = 255; }
                        RGBAData D;
                        D.Red = InactiveColor.Red + (int)((float)(ActiveColor.Red - InactiveColor.Red) * V / 255).Limit(0, 255);
                        D.Green = InactiveColor.Green + (int)((float)(ActiveColor.Green - InactiveColor.Green) * V / 255).Limit(0, 255);
                        D.Blue = InactiveColor.Blue + (int)((float)(ActiveColor.Blue - InactiveColor.Blue) * V / 255).Limit(0, 255);
                        D.Alpha = InactiveColor.Alpha + (int)((float)(ActiveColor.Alpha - InactiveColor.Alpha) * V / 255).Limit(0, 255);

                        for (int x = AreaLeft; x <= AreaRight; x++)
                        {
                            LedStripLayer[x, AreaTop + i] = D;
                        }
                    }
                    break;
                case ShiftDirectionEnum.Down:
                    for (int i = 0; i < NumberOfLeds; i++)
                    {
                        int V = ((int)Value[i]).Limit(0, 255);
                        if (V > 0 && ColorSetMode == ColorSetModeEnum.OnOff) { V = 255; }
                        RGBAData D;
                        D.Red = InactiveColor.Red + (int)((float)(ActiveColor.Red - InactiveColor.Red) * V / 255).Limit(0, 255);
                        D.Green = InactiveColor.Green + (int)((float)(ActiveColor.Green - InactiveColor.Green) * V / 255).Limit(0, 255);
                        D.Blue = InactiveColor.Blue + (int)((float)(ActiveColor.Blue - InactiveColor.Blue) * V / 255).Limit(0, 255);
                        D.Alpha = InactiveColor.Alpha + (int)((float)(ActiveColor.Alpha - InactiveColor.Alpha) * V / 255).Limit(0, 255);

                        for (int x = AreaLeft; x <= AreaRight; x++)
                        {
                            LedStripLayer[x, AreaBottom - i] = D;
                        }
                    }
                    break;
                case ShiftDirectionEnum.Left:
                default:
                    for (int i = 0; i < NumberOfLeds; i++)
                    {
                        int V = ((int)Value[i]).Limit(0, 255);
                        if (V > 0 && ColorSetMode == ColorSetModeEnum.OnOff) { V = 255; }
                        RGBAData D;
                        D.Red = InactiveColor.Red + (int)((float)(ActiveColor.Red - InactiveColor.Red) * V / 255).Limit(0, 255);
                        D.Green = InactiveColor.Green + (int)((float)(ActiveColor.Green - InactiveColor.Green) * V / 255).Limit(0, 255);
                        D.Blue = InactiveColor.Blue + (int)((float)(ActiveColor.Blue - InactiveColor.Blue) * V / 255).Limit(0, 255);
                        D.Alpha = InactiveColor.Alpha + (int)((float)(ActiveColor.Alpha - InactiveColor.Alpha) * V / 255).Limit(0, 255);

                        for (int y = AreaTop; y <= AreaBottom; y++)
                        {
                            LedStripLayer[AreaRight - i, y] = D;
                        }
                    }
                    break;
            } 
            #endregion




            int DropKey = CurrentStep - TotalSteps;
            if (TriggerValueBuffer.ContainsKey(DropKey))
            {
                LastDiscardedValue = TriggerValueBuffer[DropKey];
                TriggerValueBuffer.Remove(DropKey);
            };

            if (TriggerValueBuffer.Count > 0)
            {
                CurrentStep++;
            }
            else
            {
                Table.Pinball.Alarms.UnregisterIntervalAlarm(DoStep);
                LastDiscardedValue = 0;
                CurrentStep = 0;
                Active = false;
            }
        }

        int LastDiscardedValue = 0;
        bool Active = false;

        Dictionary<int, int> TriggerValueBuffer = new Dictionary<int, int>(100);
        int LastTriggerValue = 0;

        int LastTriggerStep = -1;
        int CurrentStep = 0;
        

        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (LastTriggerValue != TableElementData.Value && LedStripLayer!=null)
            {
                LastTriggerValue = TableElementData.Value;
                LastTriggerStep = CurrentStep;

                if (TriggerValueBuffer.ContainsKey(CurrentStep))
                {
                    TriggerValueBuffer[CurrentStep] = LastTriggerValue;
                }
                else
                {
                    TriggerValueBuffer.Add(CurrentStep, LastTriggerValue);
                }

                if (!Active)
                {
                    DoStep();
                }

            }


        }





        public override void Init(Table.Table Table)
        {
            base.Init(Table);
        }

        public override void Finish()
        {
            Table.Pinball.Alarms.UnregisterIntervalAlarm(DoStep);
            base.Finish();
        }

    }
}
