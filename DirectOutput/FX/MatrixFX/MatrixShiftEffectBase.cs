using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Color;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// Base class for effects shift values through a matrix of elements.
    /// </summary>
    /// <typeparam name="MatrixElementType">The type of the atrix element type.</typeparam>
    public abstract class MatrixShiftEffectBase<MatrixElementType> : MatrixEffectBase<MatrixElementType>
    {
        private const int RefreshIntervalMs = 30;


        private MatrixShiftDirectionEnum _ShiftDirection = MatrixShiftDirectionEnum.Right;

        /// <summary>
        /// Gets or sets the shift direction resp. the direction in which the color moves.
        /// </summary>
        /// <value>
        /// The shift direction (Left, Right, Up, Down).
        /// </value>
        public MatrixShiftDirectionEnum ShiftDirection
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


        private float _ShiftAcceleration = 0;
        /// <summary>
        /// Gets or sets the acceleration for the shift speed in percent of the effect area per second.
        /// Acceleration can be zero, positive or negative. Positive values will increase the shift speed. Speed will be increased up to a max value of 10000. Negative values will decrease the shift speed. Speed will be decreased down to a minimum speed of 1.
        /// </summary>
        /// <value>
        /// The acceleration for the shift speed in percent of the effect area per second.
        /// </value>
        public float ShiftAcceleration
        {
            get { return _ShiftAcceleration; }
            set { _ShiftAcceleration = value; }
        }



        private void BuildStep2ElementTable()
        {
            List<float> L = new List<float>();

            float NumberOfElements = (ShiftDirection == MatrixShiftDirectionEnum.Left || ShiftDirection == MatrixShiftDirectionEnum.Right ? AreaWidth : AreaHeight);
            float Position = 0;
            float Speed = NumberOfElements / 100 * (ShiftSpeed / (1000 / RefreshIntervalMs));
            float Acceleration = NumberOfElements / 100 * (ShiftAcceleration / (1000 / RefreshIntervalMs));
            while (Position <= NumberOfElements)
            {
                L.Add(Position.Limit(0, NumberOfElements));
                Position += Speed;
                Speed = (Speed + Acceleration).Limit(NumberOfElements / 100 * (1 / (1000 / RefreshIntervalMs)), 10000);
            }
            L.Add(Position.Limit(0, NumberOfElements));


            Step2Element = L.ToArray();
        }

        float[] Step2Element = null;

        private void DoStep()
        {
            if (!Active)
            {
                Table.Pinball.Alarms.RegisterIntervalAlarm(RefreshIntervalMs, DoStep);
                Active = true;
            }


            int NumberOfElements = (ShiftDirection == MatrixShiftDirectionEnum.Left || ShiftDirection == MatrixShiftDirectionEnum.Right ? AreaWidth : AreaHeight);


            float FromElementNr = NumberOfElements;
            float ToElementNr = 0;
            float[] Value = new float[NumberOfElements + 1];

            int LastValue = LastDiscardedValue;

            int ToNr;
            foreach (KeyValuePair<int, int> KV in TriggerValueBuffer)
            {
                ToElementNr = Step2Element[(CurrentStep - KV.Key)];

                if (FromElementNr.Floor() == ToElementNr.Floor())
                {
                    Value[(int)FromElementNr.Floor()] += (FromElementNr - ToElementNr) * LastValue;
                }
                else
                {
                    if (!FromElementNr.IsIntegral())
                    {
                        Value[(int)FromElementNr.Floor()] += (FromElementNr - FromElementNr.Floor()) * LastValue;
                    }

                    ToNr = (int)(ToElementNr.Ceiling());
                    for (int i = (int)FromElementNr.Floor() - 1; i >= ToNr; i--)
                    {
                        Value[i] = LastValue;
                    }
                    if (!ToElementNr.IsIntegral())
                    {
                        Value[(int)ToElementNr.Floor()] += (ToElementNr.Ceiling() - ToElementNr) * LastValue;
                    }

                }
                FromElementNr = ToElementNr;
                LastValue = KV.Value;
            }
            ToElementNr = 0;
            if (FromElementNr != ToElementNr)
            {
                if (!FromElementNr.IsIntegral() && FromElementNr.Floor() < Width - 1)
                {
                    Value[(int)FromElementNr.Floor()] += (FromElementNr - FromElementNr.Floor()) * LastValue;
                }

                ToNr = (int)(ToElementNr.Ceiling()).Limit(0, int.MaxValue);
                for (int i = (int)FromElementNr.Floor() - 1; i >= ToNr; i--)
                {
                    Value[i] = LastValue;
                }
                if (!ToElementNr.IsIntegral())
                {
                    Value[(int)ToElementNr.Floor()] += (ToElementNr.Ceiling() - ToElementNr) * LastValue;
                }
            }


            #region Data ouput
            switch (ShiftDirection)
            {
                case MatrixShiftDirectionEnum.Right:
                    for (int i = 0; i < NumberOfElements; i++)
                    {
                        int V = ((int)Value[i]).Limit(0, 255);
                        if (V > 0 && FadeMode == FadeModeEnum.OnOff) { V = 255; }
                        MatrixElementType D = GetEffectValue(V);

                        for (int y = AreaTop; y <= AreaBottom; y++)
                        {
                            MatrixLayer[AreaLeft + i, y] = D;
                        }
                    }
                    break;
                case MatrixShiftDirectionEnum.Down:
                    for (int i = 0; i < NumberOfElements; i++)
                    {
                        int V = ((int)Value[i]).Limit(0, 255);
                        if (V > 0 && FadeMode == FadeModeEnum.OnOff) { V = 255; }
                        MatrixElementType D = GetEffectValue(V);

                        for (int x = AreaLeft; x <= AreaRight; x++)
                        {
                            MatrixLayer[x, AreaTop + i] = D;
                        }
                    }
                    break;
                case MatrixShiftDirectionEnum.Up:
                    for (int i = 0; i < NumberOfElements; i++)
                    {
                        int V = ((int)Value[i]).Limit(0, 255);
                        if (V > 0 && FadeMode == FadeModeEnum.OnOff) { V = 255; }
                        MatrixElementType D = GetEffectValue(V);

                        for (int x = AreaLeft; x <= AreaRight; x++)
                        {
                            MatrixLayer[x, AreaBottom - i] = D;
                        }
                    }
                    break;
                case MatrixShiftDirectionEnum.Left:
                default:
                    for (int i = 0; i < NumberOfElements; i++)
                    {
                        int V = ((int)Value[i]).Limit(0, 255);
                        if (V > 0 && FadeMode == FadeModeEnum.OnOff) { V = 255; }
                        MatrixElementType D = GetEffectValue(V);

                        for (int y = AreaTop; y <= AreaBottom; y++)
                        {
                            MatrixLayer[AreaRight - i, y] = D;
                        }
                    }
                    break;
            }
            #endregion





            int DropKey = CurrentStep - (Step2Element.Length - 1);
            if (TriggerValueBuffer.ContainsKey(DropKey))
            {
                LastDiscardedValue = TriggerValueBuffer[DropKey];
                TriggerValueBuffer.Remove(DropKey);
            };

            if (TriggerValueBuffer.Count > 0 || LastDiscardedValue != 0)
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

        SortedDictionary<int, int> TriggerValueBuffer = new SortedDictionary<int, int>();
        int LastTriggerValue = 0;


        int CurrentStep = 0;



        /// <summary>
        /// Gets the value which is to be applied to all elements of the matrix area controlled by the effect.
        /// This methed must be overwritten.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <returns>Returns a value which is to be applied to one or several matrix elements.</returns>
        protected abstract MatrixElementType GetEffectValue(int TriggerValue);


        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (LastTriggerValue != TableElementData.Value && MatrixLayer != null)
            {
                LastTriggerValue = TableElementData.Value;


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
            BuildStep2ElementTable();
        }

        public override void Finish()
        {
            try
            {
                Table.Pinball.Alarms.UnregisterIntervalAlarm(DoStep);
            }
            catch { };
            base.Finish();
        }

    }
}
