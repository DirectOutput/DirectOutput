using System;
using DirectOutput.Cab.Toys;
using DirectOutput.Cab.Toys.LWEquivalent;

namespace DirectOutput.FX.LedControlFX
{

    /// <summary>
    /// The LedControlEffect is used when LedControl.ini files are parsed for this framework.<br/>
    /// It is recommended not to use this effect, for other purposes. Use specific effects instead.
    /// </summary>
    public class LedControlEffect : EffectBase, IEffect
    {
        private int MinimumEffectDurationMs = 60;
        private int MinimumRGBEffectDurationMs = 120;

        private DirectOutput.PinballSupport.AlarmHandler AlarmHandler;
        #region Properties
        private string _LedWizEquivalentName;

        /// <summary>
        /// Gets or sets the name of the LedWizEquivalent used for the effect output.
        /// </summary>
        /// <value>
        /// The name of the LedWizEquivalent used for the effect output.
        /// </value>
        public string LedWizEquivalentName
        {
            get { return _LedWizEquivalentName; }
            set { _LedWizEquivalentName = value; }
        }

        private LedWizEquivalent LedWizEquivalent;

        private void ResolveName(Pinball Pinball)
        {
            if (!LedWizEquivalentName.IsNullOrWhiteSpace() && Pinball.Cabinet.Toys.Contains(LedWizEquivalentName))
            {
                IToy T = Pinball.Cabinet.Toys[LedWizEquivalentName];
                if (T is LedWizEquivalent)
                {
                    LedWizEquivalent = (LedWizEquivalent)T;
                }
            }

        }

        private int _FirstOutputNumber = 0;
        /// <summary>
        /// Gets or sets the number of the first output for this effect.
        /// </summary>
        /// <value>
        /// The number of the first output for this effect (1-32).
        /// </value>
        /// <exception cref="System.ArgumentOutOfRangeException">The supplied value {0} for FirstOutputNumber is out of range (1-32).</exception>
        public int FirstOutputNumber
        {
            get { return _FirstOutputNumber; }
            set
            {
                if (!value.IsBetween(1, 32))
                {
                    throw new ArgumentOutOfRangeException("The supplied value {0} for FirstOutputNumber is out of range (1-32).".Build(value));
                }
                _FirstOutputNumber = value;
            }
        }



        private int _Intensity = 48;
        /// <summary>
        /// Gets or sets the intensity.
        /// </summary>
        /// <value>
        /// The intensity (0-48).
        /// </value>
        public int Intensity
        {
            get { return _Intensity; }
            set
            {

                if (!value.IsBetween(0, 48))
                {
                    throw new ArgumentOutOfRangeException("The supplied value {0} for Intensity is out of range (0-48).".Build(value));
                }
                _Intensity = value;
            }
        }

        private int[] _RGBColor = new int[3] { -1, -1, -1 };
        /// <summary>
        /// Gets or sets a array of color parts (Red, Green, Blue).
        /// </summary>
        /// <value>
        /// The array of color parts (Red, Green, Blue).
        /// </value>
        public int[] RGBColor
        {
            get { return _RGBColor; }
            set { _RGBColor = value; }
        }


        private bool IsRGBEffect
        {
            get
            {
                return RGBColor[0] >= 0;
            }
        }

        private int _Blink = 0;
        /// <summary>
        /// Gets or sets the blink configuration for the effect.<br/>
        /// 0 means do not blink. -1 means blink infinitely, positive number indicates number of blinks.
        /// </summary>
        /// <value>
        /// The blink configuration for the effect.
        /// </value>
        public int Blink
        {
            get { return _Blink; }
            set { _Blink = value; }
        }

        private int _BlinkInterval = 500;
        /// <summary>
        /// Gets or sets the blink interval in milliseconds.<br/>
        /// Defaults to 500ms.
        /// </summary>
        /// <value>
        /// The blink interval in milliseconds.
        /// </value>
        public int BlinkInterval
        {
            get { return _BlinkInterval; }
            set { _BlinkInterval = value.Limit(0, int.MaxValue); }
        }

        private int _Duration = -1;
        /// <summary>
        /// Gets or sets the duration of the effect.<br/>
        /// If duration&lt;=0 the effect will last for a infinite duration resp. until the triggering table element changes its value to 0, if duration>0 the effect will last the specified number of milliseconds. 
        /// </summary>
        /// <value>
        /// The duration of the effect.
        /// </value>
        public int Duration
        {
            get { return _Duration; }
            set { _Duration = value; }
        }

        private int _DimUpDurationMs = 0;

        /// <summary>
        /// Gets or sets the dim up duration in milliseconds.
        /// </summary>
        /// <value>
        /// The dim duration up in miliseconds.
        /// </value>
        public int DimUpDurationMs
        {
            get { return _DimUpDurationMs; }
            set { _DimUpDurationMs = value.Limit(0, int.MaxValue); }
        }

        private int _DimDownDurationMs = 0;

        /// <summary>
        /// Gets or sets the dim down duration in milliseconds.
        /// </summary>
        /// <value>
        /// The dim duration down in miliseconds.
        /// </value>
        public int DimDownDurationMs
        {
            get { return _DimDownDurationMs; }
            set { _DimDownDurationMs = value.Limit(0, int.MaxValue); }
        }

        #endregion


        #region Set/Unset
        private void Set()
        {
            if (IsRGBEffect)
            {
                SetOutputColor();
            }
            else
            {
                SetAnalogOutput();
            }
        }

        private void Unset()
        {
            if (IsRGBEffect)
            {
                UnsetOutputColor();
            }
            else
            {
                UnsetAnalogOutput();
            }

        }

        private void SetOutputColor()
        {

            if (RGBColor[0] >= 0)
            {
                if (DimUpDurationMs > 0 || DimDownDurationMs > 0)
                {
                    DimColor(true);
                }
                else
                {
                    SetOutputValue(FirstOutputNumber, RGBColor[0]);
                    SetOutputValue(FirstOutputNumber + 1, RGBColor[1]);
                    SetOutputValue(FirstOutputNumber + 2, RGBColor[2]);
                }
            }
        }
        private void UnsetOutputColor()
        {
            if (DimUpDurationMs > 0 || DimDownDurationMs > 0)
            {
                DimColor(false);
            }
            else
            {
                SetOutputValue(FirstOutputNumber, 0);
                SetOutputValue(FirstOutputNumber + 1, 0);
                SetOutputValue(FirstOutputNumber + 2, 0);
            }
        }

        private double[] ColorDimStep = new double[3] { 0, 0, 0 };
        private double[] ColorCurrent = new double[3] { 0, 0, 0 };
        private double[] ColorTarget = new double[3] { 0, 0, 0 };
        private void DimColor(bool LedState)
        {
            AlarmHandler.UnregisterAlarm(DimColorAlarmHandler);
            int Duration = (LedState ? DimUpDurationMs : DimDownDurationMs);
            if (Duration > 0)
            {
                bool ColorMatch = true;
                for (int i = 0; i < 3; i++)
                {
                    ColorCurrent[i] = LedWizEquivalent.GetOutputValue((FirstOutputNumber + i).Limit(1, 32));
                    ColorTarget[i] = (LedState ? RGBColor[i] : 0);
                    ColorMatch &= (ColorCurrent[i] == ColorTarget[i]);

                    ColorDimStep[i] = (ColorTarget[i] - ColorCurrent[i]) / (Duration / 30);

                }
                if (!ColorMatch)
                {
                    DimColorAlarmHandler();
                    //                    AlarmHandler.RegisterAlarm(30, DimColorAlarmHandler);
                }
            }
            else
            {

                SetOutputValue(FirstOutputNumber, (LedState ? RGBColor[0] : 0));
                SetOutputValue(FirstOutputNumber + 1, (LedState ? RGBColor[1] : 0));
                SetOutputValue(FirstOutputNumber + 2, (LedState ? RGBColor[2] : 0));
            }


        }

        private void DimColorAlarmHandler()
        {
            bool ContinueDimming = false;

            for (int i = 0; i < 3; i++)
            {
                ColorCurrent[i] += ColorDimStep[i];

                if (ColorDimStep[i] > 0)
                {
                    if (ColorCurrent[i] >= ColorTarget[i] || ColorCurrent[i] >= 48)
                    {
                        ColorDimStep[i] = 0;
                        ColorCurrent[i] = ColorTarget[i];
                    }
                    else
                    {
                        ContinueDimming = true;
                    }
                }
                else if (ColorDimStep[i] < 0)
                {
                    if (ColorCurrent[i] <= ColorTarget[i] || ColorCurrent[i] <= 0)
                    {
                        ColorDimStep[i] = 0;
                        ColorCurrent[i] = ColorTarget[i];
                    }
                    else
                    {
                        ContinueDimming = true;
                    }

                }
                SetOutputValue(FirstOutputNumber + i, ((int)ColorCurrent[i]).Limit(0, 48));
            }

            if (ContinueDimming)
            {
                AlarmHandler.RegisterAlarm(30, DimColorAlarmHandler);
            }
        }

        private void SetAnalogOutput()
        {
            if (RGBColor[0] < 0)
            {
                if (DimUpDurationMs > 0 || DimDownDurationMs > 0)
                {
                    DimAnalogOutput(true);
                }
                else
                {
                    SetOutputValue(FirstOutputNumber, Intensity);
                }
            }
        }

        private void UnsetAnalogOutput()
        {
            if (RGBColor[0] < 0)
            {
                if (DimUpDurationMs > 0 || DimDownDurationMs > 0)
                {
                    DimAnalogOutput(false);
                }
                else
                {
                    SetOutputValue(FirstOutputNumber, 0);
                }
            }
        }


        private double DimCurrent = 0;

        private double DimStep = 0;
        private double DimTarget = 0;


        private void DimAnalogOutput(bool OutputState)
        {
            AlarmHandler.UnregisterAlarm(DimAnalogOutputAlarmHandler);

            DimCurrent = LedWizEquivalent.GetOutputValue(FirstOutputNumber);
            DimTarget = (OutputState ? Intensity : 0);
            int Duration = (DimCurrent < DimTarget ? DimUpDurationMs : DimDownDurationMs);
            if (DimCurrent == DimTarget || Duration == 0)
            {
                LedWizEquivalent.SetOutputValue(FirstOutputNumber, Intensity);
            }
            else
            {

                DimStep = (DimTarget - DimCurrent) / (Duration / 30);


                if (DimStep != 0)
                {
                    DimAnalogOutputAlarmHandler();
                }
            }
        }


        private void DimAnalogOutputAlarmHandler()
        {
            DimCurrent += DimStep;
            if (DimStep > 0)
            {
                if (DimCurrent < DimTarget && DimCurrent < 48)
                {
                    AlarmHandler.RegisterAlarm(30, DimAnalogOutputAlarmHandler);
                }
                else
                {
                    DimCurrent = DimTarget;
                }
            }
            else if (DimStep < 0)
            {
                if (DimCurrent > DimTarget && DimCurrent > 0)
                {
                    AlarmHandler.RegisterAlarm(30, DimAnalogOutputAlarmHandler);
                }
                else
                {
                    DimCurrent = DimTarget;
                }
            }

            if (DimStep == 0 || DimCurrent >= 48 || DimCurrent <= 0)
            {
                AlarmHandler.UnregisterAlarm(DimAnalogOutputAlarmHandler);
            }


            LedWizEquivalent.SetOutputValue(FirstOutputNumber, ((int)DimCurrent).Limit(0, 48));
        }

        private void SetOutputValue(int OutputNumber, int Value)
        {
            if (LedWizEquivalent != null && OutputNumber.IsBetween(1, 32))
            {

                LedWizEquivalent.SetOutputValue(OutputNumber, Value);
            }
        }



        #endregion


        #region Blink
        private int BlinkCount = 0;
        private void BlinkInit()
        {
            BlinkState = true;
            BlinkCount = 0;
            if (Blink != 0)
            {
                AlarmHandler.RegisterIntervalAlarm(BlinkInterval, BlinkAlarmHandler);
            }
        }


        private void BlinkFinish()
        {
            AlarmHandler.UnregisterIntervalAlarm(BlinkAlarmHandler);
        }

        private bool BlinkState = true;
        private void BlinkAlarmHandler()
        {
            if (BlinkState)
            {
                Unset();
                BlinkCount++;
                if (Blink > 0 && BlinkCount >= Blink)
                {
                    AlarmHandler.UnregisterIntervalAlarm(BlinkAlarmHandler);
                }
                BlinkState = false;
            }
            else
            {
                Set();
                BlinkState = true;
            }

        }
        #endregion

        private void DurationAlarmHandler()
        {
            BlinkFinish();
            Unset();
        }


        private DateTime SetTime = DateTime.MinValue;

        /// <summary>
        /// Triggers the effect 
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect. Null for static effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {


            if (TableElementData == null)
            {
                //Static effect init

                Set();
                BlinkInit();
                if (Duration > 0)
                {
                    AlarmHandler.RegisterAlarm(Duration, DurationAlarmHandler);
                }
            }
            else
            {

                //Controlled effect call
                if (TableElementData.Value > 0)
                {
                    Set();
                    BlinkInit();
                    if (SetTime == DateTime.MinValue)
                    {
                        SetTime = DateTime.Now;
                    }
                    if (Duration > 0)
                    {
                        AlarmHandler.RegisterAlarm(Duration, DurationAlarmHandler);
                    }
                }
                else
                {
                    if (Duration <= 0 && Blink <= 0)
                    {
                        if (SetTime != DateTime.MinValue)
                        {

                            int D = (int)(DateTime.Now - SetTime).TotalMilliseconds;

                            if (D < (IsRGBEffect ? MinimumRGBEffectDurationMs : MinimumEffectDurationMs))
                            {
                                AlarmHandler.RegisterAlarm(((IsRGBEffect ? MinimumRGBEffectDurationMs : MinimumEffectDurationMs) - D).Limit(1, 10000), DelayedUnsetAlarmHandler);
                                return;
                            }
                        }
                        BlinkFinish();
                        Unset();
                        SetTime = DateTime.MinValue;
                    }
                }
            }
        }


        private void DelayedUnsetAlarmHandler()
        {
            BlinkFinish();
            Unset();
            SetTime = DateTime.MinValue;
        }

        /// <summary>
        /// Initializes the LedControlEffect.
        /// </summary>
        /// <param name="Pinball">Pinball object containing the effect.</param>
        public override void Init(Pinball Pinball)
        {
            ResolveName(Pinball);
            AlarmHandler = Pinball.Alarms;
            MinimumEffectDurationMs = Pinball.GlobalConfig.LedControlMinimumEffectDurationMs;
            MinimumRGBEffectDurationMs = Pinball.GlobalConfig.LedControlMinimumRGBEffectDurationMs;
        }

        /// <summary>
        /// Finishes the LedControlEffect.
        /// </summary>
        public override void Finish()
        {
            AlarmHandler.UnregisterAlarm(DurationAlarmHandler);
            AlarmHandler.UnregisterIntervalAlarm(BlinkAlarmHandler);
            AlarmHandler.UnregisterAlarm(DimAnalogOutputAlarmHandler);
            Unset();
            AlarmHandler = null;
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="LedControlEffect"/> class.
        /// </summary>
        public LedControlEffect() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LedControlEffect"/> class using the specified LedWizEquivalent and OutputNumber.
        /// </summary>
        /// <param name="LedWizEquivalentName">Name of the  LedWizEquivalent.</param>
        /// <param name="FirstOutputNumber">The number of the first output.</param>
        public LedControlEffect(string LedWizEquivalentName, int FirstOutputNumber)
            : this()
        {
            this.LedWizEquivalentName = LedWizEquivalentName;
            this.FirstOutputNumber = FirstOutputNumber;
        }

    }
}
