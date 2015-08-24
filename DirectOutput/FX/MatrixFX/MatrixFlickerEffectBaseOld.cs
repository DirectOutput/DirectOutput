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
    public abstract class MatrixFlickerEffectBaseOld<MatrixElementType> : MatrixEffectBase<MatrixElementType>
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

        /// <summary>
        /// Gets a value indicating whether this <see cref="MatrixFlickerEffectBase"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        private bool Active { get; set; }


        private SortedDictionary<int, List<System.Drawing.Point>> ElementDictionary = new SortedDictionary<int, List<System.Drawing.Point>>();
        private int CurrentStep = 0;
        private int CurrentValue = 0;
        private int CurrentFlickerElements = 0;

        private Random R = new Random();

        private void DoFlicker()
        {
            MatrixElementType D;
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

                D = GetEffectValue(V);

                int NumberOfLeds = AreaWidth * AreaHeight;
                int FlickerLeds = ((int)((double)NumberOfLeds / 100 * Density)).Limit(1, NumberOfLeds);

                int Min = MinFlickerDurationMs;
                int Max = MaxFlickerDurationMs;
                if (Max < Min)
                {
                    int Tmp = Min; Min = Max; Max = Tmp;
                }
                while (CurrentFlickerElements < FlickerLeds)
                {
                    int S = CurrentStep + (int)((float)(R.Next(Min ,Max)) / RefreshIntervalMs);
                    if (!ElementDictionary.ContainsKey(S))
                    {
                        ElementDictionary.Add(S, new List<System.Drawing.Point>());
                    }
                    ElementDictionary[S].Add(new System.Drawing.Point(R.Next(AreaLeft, AreaRight+1), R.Next(AreaTop,AreaBottom+1)));
                    CurrentFlickerElements++;
                }



                List<int> DropKeys = new List<int>();

                foreach (KeyValuePair<int, List<System.Drawing.Point>> KV in ElementDictionary)
                {
                    if (KV.Key < CurrentStep)
                    {
                        foreach (System.Drawing.Point P in KV.Value)
                        {
                            MatrixLayer[P.X, P.Y] = I;
                            CurrentFlickerElements--;
                        }
                        DropKeys.Add(KV.Key);
                    }
                    else
                    {
                        foreach (System.Drawing.Point P in KV.Value)
                        {
                            MatrixLayer[P.X, P.Y] = D;
                        }
                    }
                }


                foreach (int S in DropKeys)
                {
                    ElementDictionary.Remove(S);
                }

                CurrentStep++;



            }
            else
            {
                //Deactivate effect (V=0)

                foreach (KeyValuePair<int, List<System.Drawing.Point>> KV in ElementDictionary)
                {
                    foreach (System.Drawing.Point P in KV.Value)
                    {
                        MatrixLayer[P.X, P.Y] = I;
                    }
                }
                ElementDictionary.Clear();
                CurrentStep = 0;
                CurrentFlickerElements = 0;
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
        /// Gets the value which is to be applied to all elements of the matrix area controlled by the effect.
        /// This methed must be overwritten.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <returns>Returns a value which is to be applied to one or several matrix elements.</returns>
        protected abstract MatrixElementType GetEffectValue(int TriggerValue);

    }
}
