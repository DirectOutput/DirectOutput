using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Color;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// Does create random flickering with a defineable density, durations and value within the spefied area of a matrix toy.
    /// </summary>
    public abstract class MatrixFlickerEffectBase<MatrixElementType> : MatrixEffectBase<MatrixElementType>
    {
        private const int RefreshIntervalMs = 30;







        private int _Density = 10;

        /// <summary>
        /// Gets or sets the density of the flickering in percent.
        /// For 0 no elements of the defined area will will flicker, for 50 half of the elements will flicker, for 100 all elements will flicker.
        /// </summary>
        /// <value>
        /// The density if the flickering in percent.
        /// </value>
        public int Density
        {
            get { return _Density; }
            set { _Density = value.Limit(0, 100); }
        }


        private int _MinFlickerDurationMs = 60;

        /// <summary>
        /// Gets or sets the min duration in milliseconds for a single flicker/blink of a element. 
        /// </summary>
        /// <value>
        /// The min duration in milliseconds for a single flicker/blink of a element.
        /// </value>
        public int MinFlickerDurationMs
        {
            get { return _MinFlickerDurationMs; }
            set { _MinFlickerDurationMs = value.Limit(1, int.MaxValue); }
        }

        private int _MaxFlickerDurationMs = 150;

        /// <summary>
        /// Gets or sets the max duration in milliseconds for a single flicker/blink of a element. 
        /// </summary>
        /// <value>
        /// The max duration in milliseconds for a single flicker/blink of a element.
        /// </value>

        public int MaxFlickerDurationMs
        {
            get { return _MaxFlickerDurationMs; }
            set { _MaxFlickerDurationMs = value.Limit(1, int.MaxValue); }
        }

        private int _FlickerFadeUpDurationMs = 0;

        /// <summary>
        /// Gets or sets the fade up duration in milliseconds for a single flicker/blink of a element. 
        /// </summary>
        /// <value>
        /// The fade up duration in milliseconds for a single flicker/blink of a element.
        /// </value>

        public int FlickerFadeUpDurationMs
        {
            get { return _FlickerFadeUpDurationMs; }
            set { _FlickerFadeUpDurationMs = value.Limit(1, int.MaxValue); }
        }

        private int _FlickerFadeDownDurationMs = 0;

        /// <summary>
        /// Gets or sets the fade down duration in milliseconds for a single flicker/blink of a element. 
        /// </summary>
        /// <value>
        /// The fade down duration in milliseconds for a single flicker/blink of a element.
        /// </value>

        public int FlickerFadeDownDurationMs
        {
            get { return _FlickerFadeDownDurationMs; }
            set { _FlickerFadeDownDurationMs = value.Limit(1, int.MaxValue); }
        }


        /// <summary>
        /// Gets a value indicating whether this <see cref="MatrixFlickerEffectBase"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        private bool Active { get; set; }


        private int CurrentValue = 0;

        private Random R = new Random();

        private void DoFlicker()
        {

            MatrixElementType I = GetEffectValue(0);

            int V = CurrentValue.Limit(0, 255);
            if (V > 0)
            {

                if (!Active)
                {
                    Table.Pinball.Alarms.RegisterIntervalAlarm(RefreshIntervalMs, DoFlicker);
                    Active = true;
                  
                
                }

                //Effect is active (V>0)
                if (V > 0 && FadeMode == FadeModeEnum.OnOff) { V = 255; }

                int NumberOfLeds = AreaWidth * AreaHeight;
                int FlickerLeds = ((int)((double)NumberOfLeds / 100 * Density)).Limit(1, NumberOfLeds);


                int Min = MinFlickerDurationMs;
                int Max = MaxFlickerDurationMs;
                if (Max < Min)
                {
                    int Tmp = Min; Min = Max; Max = Tmp;
                }

                //if (Min != Max && ActiveFlickerObjects.Count.IsBetween(0,FlickerLeds-1))
                //{
                //    int Avg = (Min + Max / 2) - Min;
                //    int NewObjectsAvg = ((FlickerLeds * Avg) - ActiveFlickerObjects.Sum(FO => FO.DurationMs)) / (FlickerLeds - ActiveFlickerObjects.Count);
                //    int NewObjectsAvgChange = Avg - NewObjectsAvg;

                //    if (NewObjectsAvgChange < 0)
                //    {
                //        //Increase min
                //        Min = (Min + Math.Abs(NewObjectsAvgChange) * 2).Limit(Min, Max);

                //    }
                //    else
                //    {
                //        //Decrease max
                //        Max = (Max - NewObjectsAvgChange * 2).Limit(Min, Max);
                //    }
                //}

             
                while (ActiveFlickerObjects.Count < FlickerLeds && InactiveFlickerObjects.Count > 0)
                {
                    FlickerObject FO = InactiveFlickerObjects[R.Next(InactiveFlickerObjects.Count)];
                    InactiveFlickerObjects.Remove(FO);

                    FO.StartTimestamp = DateTime.Now;
                    FO.DurationMs = R.Next(Min, Max)+ FlickerFadeDownDurationMs;
                    ActiveFlickerObjects.Add(FO);
                }

                DateTime CurrentTimestamp = DateTime.Now;

                for (int i = ActiveFlickerObjects.Count - 1; i >= 0; i--)
                {
                    FlickerObject FO = ActiveFlickerObjects[i];

                    int FV;
                    int AgeMs = (int)(DateTime.Now - FO.StartTimestamp).TotalMilliseconds;
                    if (AgeMs > FO.DurationMs + FlickerFadeDownDurationMs)
                    {
                        

                        if (AgeMs > (FO.DurationMs + FlickerFadeDownDurationMs) * 2 || R.NextDouble()>.5)
                        {
                            //Remove element
                            ActiveFlickerObjects.Remove(FO);
                            InactiveFlickerObjects.Add(FO);
                        }
                        FV = 0;
                    }
                    else if (FlickerFadeUpDurationMs > 0 && AgeMs < FlickerFadeUpDurationMs && AgeMs < FO.DurationMs)
                    {
                        //Fade up
                        FV = (int)((double)V / FlickerFadeUpDurationMs * AgeMs);
                        //Log.Write("U: " + FV.ToString());
                       
                    }
                    else if (AgeMs > FO.DurationMs && FlickerFadeDownDurationMs > 0)
                    {
                        //Fade down
                        if (FO.DurationMs < FlickerFadeUpDurationMs)
                        {
                            FV = (int)((double)V / FlickerFadeUpDurationMs * FO.DurationMs);
                        }
                        else
                        {
                            FV = V;
                        }
                        FV = FV - (int)((double)FV / FlickerFadeDownDurationMs * (AgeMs - FO.DurationMs));
                        //Log.Write("D: " + FV.ToString());
                      
                    }
                    else
                    {
                        //Full on
                        FV = V;

                        //Log.Write("F: " + FV.ToString());

                    }
                    FV = FV.Limit(0, 255);

                    MatrixLayer[FO.X, FO.Y] = GetEffectValue(FV);
                }

            }
            else
            {
               
                foreach (FlickerObject FO in ActiveFlickerObjects)
                {
                    MatrixLayer[FO.X, FO.Y] = I;
                }
                InactiveFlickerObjects.AddRange(ActiveFlickerObjects);
                ActiveFlickerObjects.Clear();
                Table.Pinball.Alarms.UnregisterIntervalAlarm(DoFlicker);
                Active = false;

            }

        }

        



        /// <summary>
        /// Triggers the effect with the given TableElementData.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (MatrixLayer != null)
            {
                CurrentValue = TableElementData.Value;
                if (CurrentValue > 0 && !Active)
                {
                    DoFlicker();
                }
            }
        }

        /// <summary>
        /// Initializes the effect.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public override void Init(DirectOutput.Table.Table Table)
        {
            base.Init(Table);

            BuildFlickerObjects();
        }

        public override void Finish()
        {
            if (Active)
            {
                CurrentValue = 0;
                DoFlicker();
            }
            ActiveFlickerObjects.Clear();
            InactiveFlickerObjects.Clear();
            base.Finish();
        }

        private void BuildFlickerObjects()
        {
            ActiveFlickerObjects = new List<FlickerObject>();
            InactiveFlickerObjects = new List<FlickerObject>();

       
            for (int Y = AreaTop; Y <= AreaBottom; Y++)
            {
                for (int X = AreaLeft; X <= AreaRight; X++)
                {
                    InactiveFlickerObjects.Add(new FlickerObject() { X = X, Y = Y });
                }
            }

        }

        private List<FlickerObject> ActiveFlickerObjects = new List<FlickerObject>();

        private List<FlickerObject> InactiveFlickerObjects = new List<FlickerObject>();

        private class FlickerObject
        {

            public int X { get; set; }
            public int Y { get; set; }

            public int DurationMs { get; set; }
            public DateTime StartTimestamp { get; set; }

        }



        /// <summary>
        /// Gets the value which is to be applied to all elements of the matrix area controlled by the effect.
        /// This methed must be overwritten.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <returns>Returns a value which is to be applied to one or several matrix elements.</returns>
        protected abstract MatrixElementType GetEffectValue(int TriggerValue);

    }
}
