using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Color;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.RGBAMatrixFX
{
    /// <summary>
    /// Does create random flickering with a defineable density, durations and color within the spefied area of a ledstrip.
    /// </summary>
    public class RGBAMatrixColorFlickerEffect : RGBAMatrixEffectBase
    {
        private const int RefreshIntervalMs = 30;

        private RGBAColor _ActiveColor = new RGBAColor(0xff, 0xff, 0xff, 0xff);

        /// <summary>
        /// Gets or sets the active color.
        /// The FadeMode property defines how this value is used.
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
        /// The FadeMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The inactive color.
        /// </value>
        public RGBAColor InactiveColor
        {
            get { return _InactiveColor; }
            set { _InactiveColor = value; }
        }


        private FadeModeEnum _FadeMode = FadeModeEnum.Fade;

        /// <summary>
        /// Gets or sets the fade mode.
        /// </summary>
        /// <value>
        /// Fade (active and inactive color will fade depending on trigger value) or OnOff (actvice color is used for triger values >0, otherwise inactive color will be used).
        /// </value>
        public FadeModeEnum FadeMode
        {
            get { return _FadeMode; }
            set { _FadeMode = value; }
        }


        private int _Density = 10;

        /// <summary>
        /// Gets or sets the density of the flickering in percent.
        /// For 0 no leds of the defined area will will flicker, for 50 half of the leds will flicker, for 100 all leds will flicker.
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
        /// Gets or sets the min duration in milliseconds for a single flicker/blink of a led. 
        /// </summary>
        /// <value>
        /// The min duration in milliseconds for a single flicker/blink of a led.
        /// </value>
        public int MinFlickerDurationMs
        {
            get { return _MinFlickerDurationMs; }
            set { _MinFlickerDurationMs = value.Limit(1, int.MaxValue); }
        }

        private int _MaxFlickerDurationMs = 150;

        /// <summary>
        /// Gets or sets the max duration in milliseconds for a single flicker/blink of a led. 
        /// </summary>
        /// <value>
        /// The max duration in milliseconds for a single flicker/blink of a led.
        /// </value>
        public int MaxFlickerDurationMs
        {
            get { return _MaxFlickerDurationMs; }
            set { _MaxFlickerDurationMs = value.Limit(1, int.MaxValue); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="RGBAMatrixColorFlickerEffect"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get; private set; }


        private SortedDictionary<int, List<System.Drawing.Point>> PixelDictionary = new SortedDictionary<int, List<System.Drawing.Point>>();
        private int CurrentStep = 0;
        private int CurrentValue = 0;
        private int CurrentFlickerLeds = 0;

        private Random R = new Random();

        public void DoFlicker()
        {
            RGBAData D;
            RGBAData I = new RGBAData();
            I.Set(InactiveColor);

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

                D.Red = InactiveColor.Red + (int)((float)(ActiveColor.Red - InactiveColor.Red) * V / 255).Limit(0, 255);
                D.Green = InactiveColor.Green + (int)((float)(ActiveColor.Green - InactiveColor.Green) * V / 255).Limit(0, 255);
                D.Blue = InactiveColor.Blue + (int)((float)(ActiveColor.Blue - InactiveColor.Blue) * V / 255).Limit(0, 255);
                D.Alpha = InactiveColor.Alpha + (int)((float)(ActiveColor.Alpha - InactiveColor.Alpha) * V / 255).Limit(0, 255);

                int NumberOfLeds = AreaWidth * AreaHeight;
                int FlickerLeds = ((int)((double)NumberOfLeds / 100 * Density)).Limit(1, NumberOfLeds);

                while (CurrentFlickerLeds < FlickerLeds)
                {
                    int S = CurrentStep + (int)((float)(MinFlickerDurationMs + R.Next(MaxFlickerDurationMs - MinFlickerDurationMs)) / RefreshIntervalMs);
                    if (!PixelDictionary.ContainsKey(S))
                    {
                        PixelDictionary.Add(S, new List<System.Drawing.Point>());
                    }
                    PixelDictionary[S].Add(new System.Drawing.Point(AreaLeft + R.Next(AreaWidth), AreaTop + R.Next(AreaHeight)));
                    CurrentFlickerLeds++;
                }



                List<int> DropKeys = new List<int>();

                foreach (KeyValuePair<int, List<System.Drawing.Point>> KV in PixelDictionary)
                {
                    if (KV.Key < CurrentStep)
                    {
                        foreach (System.Drawing.Point P in KV.Value)
                        {
                            RGBAMatrixLayer[P.X, P.Y] = I;
                            CurrentFlickerLeds--;
                        }
                        DropKeys.Add(KV.Key);
                    }
                    else
                    {
                        foreach (System.Drawing.Point P in KV.Value)
                        {
                            RGBAMatrixLayer[P.X, P.Y] = D;
                        }
                    }
                }


                foreach (int S in DropKeys)
                {
                    PixelDictionary.Remove(S);
                }

                CurrentStep++;



            }
            else
            {
                //Deactivate effect (V=0)

                foreach (KeyValuePair<int, List<System.Drawing.Point>> KV in PixelDictionary)
                {
                    foreach (System.Drawing.Point P in KV.Value)
                    {
                        RGBAMatrixLayer[P.X, P.Y] = I;
                    }
                }
                PixelDictionary.Clear();
                CurrentStep = 0;
                CurrentFlickerLeds = 0;
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
            if (RGBAMatrixLayer != null)
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
        public override void Init(Table.Table Table)
        {
            base.Init(Table);

            if (RGBAMatrix != null)
            {


            }
        }

        /// <summary>
        /// Finishes the effect.
        /// </summary>
        public override void Finish()
        {
            base.Finish();
        }


    }
}
