using System;
using System.Collections.Generic;
using System.Linq;
using DirectOutput.Cab;
using DirectOutput.Cab.Out;
using DirectOutput.Cab.Toys;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.Cab.Toys.LWEquivalent;
using DirectOutput.FX;
using DirectOutput.FX.AnalogToyFX;
using DirectOutput.FX.ConditionFX;
using DirectOutput.FX.MatrixFX;
using DirectOutput.FX.RGBAFX;
using DirectOutput.FX.TimmedFX;
using DirectOutput.FX.ValueFX;
using DirectOutput.General;
using DirectOutput.General.Analog;
using DirectOutput.General.Color;
using DirectOutput.LedControl.Loader;
using DirectOutput.Table;

namespace DirectOutput.LedControl.Setup
{
    /// <summary>
    /// Configures the system based on data from ini files (either directoutputconfig.ini or ledcontrol.ini files)
    /// </summary>
    public class Configurator
    {
        /// <summary>
        /// The min duration for effects targeting a single output.
        /// </summary>
        public int EffectMinDurationMs = 60;
        /// <summary>
        /// The min duration for effects targeting rgbleds.
        /// </summary>
        public int EffectRGBMinDurationMs = 120;

        /// <summary>
        /// Configures the system based on the data loaded from ini files.
        /// </summary>
        /// <param name="LedControlConfigList">The list of loaded led control config data.</param>
        /// <param name="Table">The table object to be configured.</param>
        /// <param name="Cabinet">The cabinet object to be configured.</param>
        /// <param name="RomName">Name of the rom to be used for the setup.</param>
        public void Setup(LedControlConfigList LedControlConfigList, DirectOutput.Table.Table Table, Cabinet Cabinet, string RomName)
        {
            Dictionary<int, TableConfig> TableConfigDict = LedControlConfigList.GetTableConfigDictionary(RomName);
            Dictionary<int, Dictionary<int, IToy>> ToyAssignments = SetupCabinet(TableConfigDict, Cabinet);

			if (LedControlConfigList.Count > 0 && LedControlConfigList[0].LedControlIniFile != null)
			{
				string IniFilePath = LedControlConfigList[0].LedControlIniFile.Directory.FullName;
				SetupTable(Table, TableConfigDict, ToyAssignments, IniFilePath);
			}
        }

        private void SetupTable(Table.Table Table, Dictionary<int, TableConfig> TableConfigDict, Dictionary<int, Dictionary<int, IToy>> ToyAssignments, string IniFilePath)
        {
            foreach (KeyValuePair<int, TableConfig> KV in TableConfigDict)
            {
                int LedWizNr = KV.Key;
                Log.Write($"Setup table for LedWiz #{LedWizNr}");
                if (ToyAssignments.ContainsKey(LedWizNr))
                {
                    TableConfig TC = KV.Value;


                    foreach (TableConfigColumn TCC in TC.Columns)
                    {

                        if (ToyAssignments[LedWizNr].ContainsKey(TCC.Number))
                        {
                            IToy Toy = ToyAssignments[LedWizNr][TCC.Number];

                            int SettingNumber = 0;
                            foreach (TableConfigSetting TCS in TCC)
                            {
                                SettingNumber++;
                                IEffect Effect = null;

                                int Layer = (TCS.Layer.HasValue ? TCS.Layer.Value : SettingNumber);

                                if (Toy is IMatrixToy<RGBAColor> || Toy is IMatrixToy<AnalogAlpha>)
                                {

                                    if (!TCS.ShapeName.IsNullOrWhiteSpace())
                                    {
                                        if (Toy is IMatrixToy<RGBAColor>)
                                        {
                                            RGBAColor ActiveColor = null;
                                            if (TCS.ColorConfig != null)
                                            {
                                                ActiveColor = TCS.ColorConfig.GetCabinetColor().GetRGBAColor();
                                            }
                                            else
                                            {
                                                if (!TCS.ColorName.IsNullOrWhiteSpace())
                                                {
                                                    if (TCS.ColorName.StartsWith("#"))
                                                    {
                                                        ActiveColor = new RGBAColor();
                                                        if (!ActiveColor.SetColor(TCS.ColorName))
                                                        {
                                                            ActiveColor = null;
                                                        }
                                                    }
                                                }
                                            }

                                            Log.Instrumentation("MX", "Setting up shape effect for area. L: {0}, T: {1}, W: {2}, H: {3}, Name: {4}".Build(new object[] { TCS.AreaLeft, TCS.AreaTop, TCS.AreaWidth, TCS.AreaHeight, TCS.ShapeName }));
                                            if (ActiveColor != null)
                                            {
                                                RGBAColor InactiveColor = ActiveColor.Clone();
                                                InactiveColor.Alpha = 0;

                                                //Color defined. Use color scale effect

                                                Effect = new RGBAMatrixColorScaleShapeEffect() { ActiveColor = ActiveColor, InactiveColor = InactiveColor, LayerNr = Layer, ShapeName = TCS.ShapeName, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, ToyName = Toy.Name };
                                            }
                                            else
                                            {
                                                //No color defined. Use org color effects
                                                Effect = new RGBAMatrixShapeEffect() { LayerNr = Layer, ShapeName = TCS.ShapeName, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, ToyName = Toy.Name };

                                            }

                                        }

                                    }
                                    else if (TCS.IsBitmap)
                                    {
                                        FilePattern P = new FilePattern("{0}\\{1}.*".Build(IniFilePath, TC.ShortRomName));

                                        if (TCS.AreaBitmapAnimationStepCount > 1)
                                        {
                                            //it is a animation
                                            if (Toy is IMatrixToy<RGBAColor>)
                                            {

                                                Effect = new RGBAMatrixBitmapAnimationEffect() { BitmapFilePattern = P, BitmapLeft = TCS.AreaBitmapLeft, BitmapTop = TCS.AreaBitmapTop, BitmapHeight = TCS.AreaBitmapHeight, BitmapWidth = TCS.AreaBitmapWidth, BitmapFrameNumber = TCS.AreaBitmapFrame, AnimationStepDirection = TCS.AreaBitmapAnimationDirection, AnimationFrameDurationMs = TCS.AreaBitmapAnimationFrameDuration, AnimationFrameCount = TCS.AreaBitmapAnimationStepCount, AnimationStepSize = TCS.AreaBitmapAnimationStepSize, AnimationBehaviour = TCS.AreaBitmapAnimationBehaviour, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, LayerNr = Layer, ToyName = Toy.Name };
                                            }
                                            else
                                            {
                                                Effect = new AnalogAlphaMatrixBitmapAnimationEffect() { BitmapFilePattern = P, BitmapLeft = TCS.AreaBitmapLeft, BitmapTop = TCS.AreaBitmapTop, BitmapHeight = TCS.AreaBitmapHeight, BitmapWidth = TCS.AreaBitmapWidth, BitmapFrameNumber = TCS.AreaBitmapFrame, AnimationStepDirection = TCS.AreaBitmapAnimationDirection, AnimationFrameDurationMs = TCS.AreaBitmapAnimationFrameDuration, AnimationFrameCount = TCS.AreaBitmapAnimationStepCount, AnimationStepSize = TCS.AreaBitmapAnimationStepSize, AnimationBehaviour = TCS.AreaBitmapAnimationBehaviour, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, LayerNr = Layer, ToyName = Toy.Name };
                                            }
                                        }
                                        else
                                        {
                                            //its a static bitmap
                                            if (Toy is IMatrixToy<RGBAColor>)
                                            {
                                                Effect = new RGBAMatrixBitmapEffect() { BitmapFilePattern = P, BitmapLeft = TCS.AreaBitmapLeft, BitmapTop = TCS.AreaBitmapTop, BitmapHeight = TCS.AreaBitmapHeight, BitmapWidth = TCS.AreaBitmapWidth, BitmapFrameNumber = TCS.AreaBitmapFrame, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, LayerNr = Layer, ToyName = Toy.Name };
                                            }
                                            else
                                            {
                                                Effect = new AnalogAlphaMatrixBitmapEffect() { BitmapFilePattern = P, BitmapLeft = TCS.AreaBitmapLeft, BitmapTop = TCS.AreaBitmapTop, BitmapHeight = TCS.AreaBitmapHeight, BitmapWidth = TCS.AreaBitmapWidth, BitmapFrameNumber = TCS.AreaBitmapFrame, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, LayerNr = Layer, ToyName = Toy.Name };
                                            }
                                        }
                                    }
                                    else if (TCS.IsPlasma)
                                    {
                                        if (Toy is IMatrixToy<RGBAColor>)
                                        {
                                            RGBAColor InactiveColor = null;
                                            RGBAColor ActiveColor1 = null;
                                            RGBAColor ActiveColor2 = null;
                                            if (TCS.ColorConfig != null)
                                            {
                                                ActiveColor1 = TCS.ColorConfig.GetCabinetColor().GetRGBAColor();
                                            }
                                            else
                                            {
                                                if (!TCS.ColorName.IsNullOrWhiteSpace())
                                                {
                                                    if (TCS.ColorName.StartsWith("#"))
                                                    {
                                                        ActiveColor1 = new RGBAColor();
                                                        if (!ActiveColor1.SetColor(TCS.ColorName))
                                                        {
                                                            ActiveColor1 = null;
                                                        }
                                                    }
                                                }
                                            }

                                            if (TCS.ColorConfig2 != null)
                                            {
                                                ActiveColor2 = TCS.ColorConfig2.GetCabinetColor().GetRGBAColor();
                                            }
                                            else
                                            {
                                                if (!TCS.ColorName2.IsNullOrWhiteSpace())
                                                {
                                                    if (TCS.ColorName2.StartsWith("#"))
                                                    {
                                                        ActiveColor2 = new RGBAColor();
                                                        if (!ActiveColor2.SetColor(TCS.ColorName2))
                                                        {
                                                            ActiveColor2 = null;
                                                        }
                                                    }
                                                }
                                            }


                                            if (ActiveColor1 != null)
                                            {
                                                InactiveColor = ActiveColor1.Clone();
                                                InactiveColor.Alpha = 0;
                                            }
                                            else if (ActiveColor2 != null)
                                            {
                                                InactiveColor = ActiveColor2.Clone();
                                                InactiveColor.Alpha = 0;
                                            }

                                            if (ActiveColor1 == null)
                                            {
                                                ActiveColor1 = new RGBAColor(0xff, 0, 0, 0xff);
                                            }

                                            if (ActiveColor2 == null)
                                            {
                                                ActiveColor2 = new RGBAColor(0, 0xff, 0, 0xff);
                                            }


                                            Effect = new RGBAMatrixPlasmaEffect() { ActiveColor1 = ActiveColor1, ActiveColor2 = ActiveColor2, InactiveColor = InactiveColor, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, LayerNr = Layer, PlasmaSpeed = TCS.PlasmaSpeed, PlasmaDensity = TCS.PlasmaDensity, ToyName = Toy.Name };
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else
                                    {
                                        //Non bitmap area effects
                                        if (Toy is IMatrixToy<RGBAColor>)
                                        {
                                            //RGBAMatrix toy

                                            RGBAColor ActiveColor = null;
                                            if (TCS.ColorConfig != null)
                                            {
                                                ActiveColor = TCS.ColorConfig.GetCabinetColor().GetRGBAColor();
                                            }
                                            else
                                            {
                                                if (!TCS.ColorName.IsNullOrWhiteSpace())
                                                {
                                                    if (TCS.ColorName.StartsWith("#"))
                                                    {
                                                        ActiveColor = new RGBAColor();
                                                        if (!ActiveColor.SetColor(TCS.ColorName))
                                                        {
                                                            ActiveColor = null;
                                                        }
                                                    }
                                                }
                                            }

                                            if (ActiveColor != null)
                                            {
                                                RGBAColor InactiveColor = ActiveColor.Clone();
                                                InactiveColor.Alpha = 0;
                                                if (TCS.AreaDirection.HasValue)
                                                {
                                                    //shift effect
                                                    Effect = new RGBAMatrixShiftEffect() { ShiftDirection = TCS.AreaDirection.Value, ShiftAcceleration = TCS.AreaAcceleration, ActiveColor = ActiveColor, InactiveColor = InactiveColor, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, LayerNr = Layer, ToyName = Toy.Name };
                                                    if (TCS.AreaSpeed > 0)
                                                    {
                                                        ((RGBAMatrixShiftEffect)Effect).ShiftSpeed = TCS.AreaSpeed;
                                                    }

                                                }
                                                else if (TCS.AreaFlickerDensity > 0)
                                                {
                                                    //flicker effect
                                                    Effect = new RGBAMatrixFlickerEffect() { Density = TCS.AreaFlickerDensity.Limit(1, 99), ActiveColor = ActiveColor, InactiveColor = InactiveColor, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, LayerNr = Layer, ToyName = Toy.Name };
                                                    if (TCS.AreaFlickerMinDurationMs > 0)
                                                    {
                                                        ((RGBAMatrixFlickerEffect)Effect).MinFlickerDurationMs = TCS.AreaFlickerMinDurationMs;
                                                    }
                                                    if (TCS.AreaFlickerMaxDurationMs > 0)
                                                    {
                                                        ((RGBAMatrixFlickerEffect)Effect).MaxFlickerDurationMs = TCS.AreaFlickerMaxDurationMs;
                                                    }
                                                    if (TCS.AreaFlickerFadeDurationMs > 0)
                                                    {
                                                        ((RGBAMatrixFlickerEffect)Effect).FlickerFadeDownDurationMs = TCS.AreaFlickerFadeDurationMs;
                                                        ((RGBAMatrixFlickerEffect)Effect).FlickerFadeUpDurationMs = TCS.AreaFlickerFadeDurationMs;
                                                    }
                                                }
                                                else
                                                {
                                                    //Color effect
                                                    Effect = new RGBAMatrixColorEffect() { ActiveColor = ActiveColor, InactiveColor = InactiveColor, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, LayerNr = Layer, ToyName = Toy.Name };
                                                }

                                            }
                                            else
                                            {
                                                Log.Warning("Invalid color definition \"{0}\" found for area effect. Skipped setting {1} in column {2} for LedWizEqivalent number {3}.".Build(TCS.ColorName, SettingNumber, TCC.Number, LedWizNr));

                                            }
                                        }
                                        else if (Toy is IMatrixToy<AnalogAlpha>)
                                        {
                                            AnalogAlpha ActiveValue = new AnalogAlpha(TCS.Intensity.Limit(0, 255), 255);
                                            AnalogAlpha InactiveValue = ActiveValue.Clone();
                                            InactiveValue.Alpha = 0;


                                            if (TCS.AreaDirection.HasValue)
                                            {
                                                //shift effect
                                                Effect = new AnalogAlphaMatrixShiftEffect() { ShiftDirection = TCS.AreaDirection.Value, ShiftAcceleration = TCS.AreaAcceleration, ActiveValue = ActiveValue, InactiveValue = InactiveValue, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, LayerNr = Layer, ToyName = Toy.Name };
                                                if (TCS.AreaSpeed > 0)
                                                {
                                                    ((AnalogAlphaMatrixShiftEffect)Effect).ShiftSpeed = TCS.AreaSpeed;
                                                }

                                            }
                                            else if (TCS.AreaFlickerDensity > 0)
                                            {
                                                //flicker effect
                                                Effect = new AnalogAlphaMatrixFlickerEffect() { Density = TCS.AreaFlickerDensity.Limit(1, 99), ActiveValue = ActiveValue, InactiveValue = InactiveValue, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, LayerNr = Layer, ToyName = Toy.Name };
                                                if (TCS.AreaFlickerMinDurationMs > 0)
                                                {
                                                    ((AnalogAlphaMatrixFlickerEffect)Effect).MinFlickerDurationMs = TCS.AreaFlickerMinDurationMs;
                                                }
                                                if (TCS.AreaFlickerMaxDurationMs > 0)
                                                {
                                                    ((AnalogAlphaMatrixFlickerEffect)Effect).MaxFlickerDurationMs = TCS.AreaFlickerMaxDurationMs;
                                                }
                                                if (TCS.AreaFlickerFadeDurationMs > 0)
                                                {
                                                    ((AnalogAlphaMatrixFlickerEffect)Effect).FlickerFadeDownDurationMs = TCS.AreaFlickerFadeDurationMs;
                                                    ((AnalogAlphaMatrixFlickerEffect)Effect).FlickerFadeUpDurationMs = TCS.AreaFlickerFadeDurationMs;
                                                }

                                            }
                                            else
                                            {
                                                //Color effect
                                                Effect = new AnalogAlphaMatrixValueEffect() { ActiveValue = ActiveValue, InactiveValue = InactiveValue, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, LayerNr = Layer, ToyName = Toy.Name };
                                            }

                                        }
                                    }
                                }
                                else if (Toy is IRGBAToy)
                                {
                                    RGBAColor ActiveColor = null;
                                    if (TCS.ColorConfig != null)
                                    {
                                        ActiveColor = TCS.ColorConfig.GetCabinetColor().GetRGBAColor();
                                    }
                                    else
                                    {
                                        if (!TCS.ColorName.IsNullOrWhiteSpace())
                                        {
                                            if (TCS.ColorName.StartsWith("#"))
                                            {
                                                ActiveColor = new RGBAColor();
                                                if (!ActiveColor.SetColor(TCS.ColorName))
                                                {
                                                    ActiveColor = null;
                                                    Log.Warning("Skipped setting {0} in column {1} for LedWizEqivalent number {2} since {3} is not a valid color specification.".Build(new object[] { SettingNumber, TCC.Number, LedWizNr, TCS.ColorName }));
                                                }
                                            }
                                            else
                                            {
                                                Log.Warning("Skipped setting {0} in column {1} for LedWizEqivalent number {2} since {3} is not a valid color specification.".Build(new object[] { SettingNumber, TCC.Number, LedWizNr, TCS.ColorName }));
                                            }
                                        }
                                        else
                                        {
                                            Log.Warning("Skipped setting {0} in column {1} for LedWizEqivalent number {2} since it does not contain a color specification.".Build(SettingNumber, TCC.Number, LedWizNr));
                                        }
                                    }
                                    if (ActiveColor != null)
                                    {
                                        RGBAColor InactiveColor = ActiveColor.Clone();
                                        InactiveColor.Alpha = 0;
                                        Effect = new RGBAColorEffect() { ToyName = Toy.Name, LayerNr = Layer, ActiveColor = ActiveColor, InactiveColor = InactiveColor };
                                    }

                                }
                                else if (Toy is IAnalogAlphaToy)
                                {
                                    AnalogAlpha ActiveValue = new AnalogAlpha(TCS.Intensity.Limit(0, 255), 255);
                                    AnalogAlpha InactiveValue = ActiveValue.Clone();
                                    InactiveValue.Alpha = 0;
                                    Effect = new AnalogToyValueEffect() { ToyName = Toy.Name, LayerNr = Layer, ActiveValue = ActiveValue, InactiveValue = InactiveValue };

                                }
                                if (Effect != null)
                                {
                                    Effect.Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} {3}".Build(new object[] { LedWizNr, TCC.Number, SettingNumber, Effect.GetType().Name });
                                    MakeEffectNameUnique(Effect, Table);

                                    Table.Effects.Add(Effect);

                                    if (TCS.FadingUpDurationMs > 0 || TCS.FadingDownDurationMs > 0)
                                    {
                                        Effect = new FadeEffect() { Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} FadeEffect".Build(LedWizNr, TCC.Number, SettingNumber), TargetEffectName = Effect.Name, FadeDownDuration = TCS.FadingDownDurationMs, FadeUpDuration = TCS.FadingUpDurationMs };
                                        MakeEffectNameUnique(Effect, Table);
                                        Table.Effects.Add(Effect);
                                    }
                                    if (TCS.Blink != 0 && TCS.BlinkIntervalMsNested > 0)
                                    {
                                        Effect = new BlinkEffect() { Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} BlinkEffect Inner".Build(LedWizNr, TCC.Number, SettingNumber), TargetEffectName = Effect.Name, LowValue = TCS.BlinkLow, DurationActiveMs = (int)((double)TCS.BlinkIntervalMsNested * (double)TCS.BlinkPulseWidthNested / 100), DurationInactiveMs = (int)((double)TCS.BlinkIntervalMsNested * (100 - (double)TCS.BlinkPulseWidthNested) / 100) };
                                        MakeEffectNameUnique(Effect, Table);
                                        Table.Effects.Add(Effect);
                                    }


                                    if (TCS.Blink != 0)
                                    {
                                        Effect = new BlinkEffect() { Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} BlinkEffect".Build(LedWizNr, TCC.Number, SettingNumber), TargetEffectName = Effect.Name, DurationActiveMs = (int)((double)TCS.BlinkIntervalMs * (double)TCS.BlinkPulseWidth / 100), DurationInactiveMs = (int)((double)TCS.BlinkIntervalMs * (100 - (double)TCS.BlinkPulseWidth) / 100) };
                                        if (TCS.BlinkIntervalMsNested == 0)
                                        {
                                            ((BlinkEffect)Effect).LowValue = TCS.BlinkLow;
                                        }
                                        MakeEffectNameUnique(Effect, Table);
                                        Table.Effects.Add(Effect);
                                    }

                                    if (TCS.DurationMs > 0 || TCS.Blink > 0)
                                    {
                                        int Duration = (TCS.DurationMs > 0 ? TCS.DurationMs : (TCS.Blink * 2 - 1) * TCS.BlinkIntervalMs / 2 + 1);
                                        Effect = new DurationEffect() { Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} DurationEffect".Build(LedWizNr, TCC.Number, SettingNumber), TargetEffectName = Effect.Name, DurationMs = Duration, RetriggerBehaviour = RetriggerBehaviourEnum.Restart };
                                        MakeEffectNameUnique(Effect, Table);
                                        Table.Effects.Add(Effect);
                                    }
                                    if (TCS.MaxDurationMs > 0)
                                    {

                                        Effect = new MaxDurationEffect() { Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} MaxDurationEffect".Build(new object[] { LedWizNr, TCC.Number, SettingNumber }), TargetEffectName = Effect.Name, MaxDurationMs = TCS.MaxDurationMs };
                                        MakeEffectNameUnique(Effect, Table);
                                        Table.Effects.Add(Effect);
                                    }


                                    if (TCS.MinDurationMs > 0 || (Toy is IRGBAToy && EffectRGBMinDurationMs > 0) || (!(Toy is IRGBAToy) && EffectMinDurationMs > 0))
                                    {
                                        string N = (TCS.MinDurationMs > 0 ? "MinDuratonEffect" : "DefaultMinDurationEffect");
                                        int Min = (TCS.MinDurationMs > 0 ? TCS.MinDurationMs : (Toy is IRGBAToy ? EffectRGBMinDurationMs : EffectMinDurationMs));
                                        Effect = new MinDurationEffect() { Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} {3}".Build(new object[] { LedWizNr, TCC.Number, SettingNumber, N }), TargetEffectName = Effect.Name, MinDurationMs = Min };
                                        MakeEffectNameUnique(Effect, Table);
                                        Table.Effects.Add(Effect);
                                    }

                                    if (TCS.ExtDurationMs > 0)
                                    {
                                        Effect = new ExtendDurationEffect() { Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} ExtDurationEffect".Build(LedWizNr, TCC.Number, SettingNumber), TargetEffectName = Effect.Name, DurationMs = TCS.ExtDurationMs };
                                        MakeEffectNameUnique(Effect, Table);
                                        Table.Effects.Add(Effect);

                                    }

                                    if (TCS.WaitDurationMs > 0)
                                    {
                                        Effect = new DelayEffect() { Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} DelayEffect".Build(LedWizNr, TCC.Number, SettingNumber), TargetEffectName = Effect.Name, DelayMs = TCS.WaitDurationMs };
                                        MakeEffectNameUnique(Effect, Table);
                                        Table.Effects.Add(Effect);
                                    }

                                    if (TCS.Invert)
                                    {
                                        Effect = new ValueInvertEffect() { Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} ValueInvertEffect".Build(LedWizNr, TCC.Number, SettingNumber), TargetEffectName = Effect.Name };
                                        MakeEffectNameUnique(Effect, Table);
                                        Table.Effects.Add(Effect);
                                    }
                                    if (!TCS.NoBool)
                                    {

                                        Effect = new ValueMapFullRangeEffect() { Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} FullRangeEffect".Build(LedWizNr, TCC.Number, SettingNumber), TargetEffectName = Effect.Name };
                                        MakeEffectNameUnique(Effect, Table);
                                        Table.Effects.Add(Effect);


                                    }
                                    switch (TCS.OutputControl)
                                    {
                                        case OutputControlEnum.Condition:

                                            Effect = new TableElementConditionEffect() { Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} TableElementConditionEffect".Build(LedWizNr, TCC.Number, SettingNumber), TargetEffectName = Effect.Name, Condition = TCS.Condition };
                                            MakeEffectNameUnique(Effect, Table);
                                            Table.Effects.Add(Effect);

                                            AssignEffectToTableElements(Table, ((TableElementConditionEffect)Effect).GetVariables().ToArray(), Effect);

                                            break;


                                        case OutputControlEnum.FixedOn:
                                            Table.AssignedStaticEffects.Add(new AssignedEffect(Effect.Name));
                                            break;
                                        case OutputControlEnum.Controlled:

                                            string[] ATE = TCS.TableElement.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(A => A.Trim()).ToArray();
                                            AssignEffectToTableElements(Table, ATE, Effect);

                                            break;
                                        case OutputControlEnum.FixedOff:
                                        default:
                                            break;
                                    }
                                }
                            }


                        }
                    }
                }
            }
        }


        private void AssignEffectToTableElements(Table.Table Table, string[] TableElementDescriptors, IEffect Effect)
        {
            foreach (string D in TableElementDescriptors)
            {
                TableElement TE = null;
                if (D[0] == (char)TableElementTypeEnum.NamedElement)
                {
                    //Log.Write("Adding table element: " + D);
                    Table.TableElements.UpdateState(new Table.TableElementData(D.Substring(1), 0));
                    TE = Table.TableElements[D.Substring(1)];
                }
                else if (Enum.IsDefined(typeof(TableElementTypeEnum), (int)D[0]) && D.Substring(1).IsInteger())
                {
                    Table.TableElements.UpdateState(new Table.TableElementData((TableElementTypeEnum)D[0], D.Substring(1).ToInteger(), 0));
                    TE = Table.TableElements[(TableElementTypeEnum)D[0], D.Substring(1).ToInteger()];
                }
                TE.AssignedEffects.Add(new AssignedEffect(Effect.Name));
            }
        }



        private void MakeEffectNameUnique(IEffect Effect, Table.Table Table)
        {
            if (Table.Effects.Contains(Effect.Name))
            {
                int Cnt = 1;
                while (Table.Effects.Contains("{0} {1}".Build(Effect.Name, Cnt)))
                {
                    Cnt++;
                }
                Effect.Name = "{0} {1}".Build(Effect.Name, Cnt);
            }
        }





        private Dictionary<int, Dictionary<int, IToy>> SetupCabinet(Dictionary<int, TableConfig> TableConfigDict, Cabinet Cabinet)
        {
            Dictionary<int, Dictionary<int, IToy>> ToyAssignments = new Dictionary<int, Dictionary<int, IToy>>();

            Dictionary<int, LedWizEquivalent> LedWizEquivalentDict = new Dictionary<int, LedWizEquivalent>();
            foreach (IToy T in Cabinet.Toys.Where(Toy => Toy is LedWizEquivalent).ToList())
            {
                if (!LedWizEquivalentDict.Keys.Any(K => K == ((LedWizEquivalent)T).LedWizNumber))
                {
                    LedWizEquivalentDict.Add(((LedWizEquivalent)T).LedWizNumber, (LedWizEquivalent)T);
                }
                else
                {
                    Log.Warning("Found more than one ledwiz with number {0}.".Build(((LedWizEquivalent)T).LedWizNumber));

                }
            }

            foreach (KeyValuePair<int, TableConfig> KV in TableConfigDict)
            {
                int LedWizNr = KV.Key;
                ToyAssignments.Add(LedWizNr, new Dictionary<int, IToy>());

                TableConfig TC = KV.Value;
                if (LedWizEquivalentDict.ContainsKey(LedWizNr))
                {
                    LedWizEquivalent LWE = LedWizEquivalentDict[LedWizNr];

                    foreach (TableConfigColumn TCC in TC.Columns)
                    {
                        IToy TargetToy = null;

                        if (TCC.IsArea)
                        {
                            if (LWE.Outputs.Any(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber))
                            {
                                string OutputName = LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName;
                                if (Cabinet.Toys.Any(O => (O is IMatrixToy<RGBAColor> || O is IMatrixToy<AnalogAlpha>) && O.Name == OutputName))
                                {


                                    TargetToy = (IToy)Cabinet.Toys.FirstOrDefault(O => (O is IMatrixToy<RGBAColor> || O is IMatrixToy<AnalogAlpha>) && O.Name == OutputName);


                                }
                                else
                                {
                                    Log.Warning("Unknown toyname {0} defined for column {1} of LedwizEquivalent {2} (must be a matrix toy).".Build(OutputName, TCC.FirstOutputNumber, LWE.Name));
                                }
                            }
                        }
                        else
                        {

                            switch (TCC.RequiredOutputCount)
                            {
                                case 3:
                                    //RGB Led

                                    if (LWE.Outputs.Any(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber))
                                    {
                                        string OutputName = LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName;
                                        if (Cabinet.Toys.Any(O => O is IRGBAToy && O.Name == OutputName))
                                        {
                                            TargetToy = (IToy)Cabinet.Toys.FirstOrDefault(O => O is IRGBAToy && O.Name == OutputName);
                                        }
                                    }
                                    if (TargetToy == null)
                                    {
                                        if (LWE.Outputs.Any(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber) && LWE.Outputs.Any(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 1) && LWE.Outputs.Any(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 2))
                                        {
                                            //Try to get the toy 
                                            try
                                            {
                                                //Toy does already exist
                                                TargetToy = (IToy)Cabinet.Toys.First(Toy => Toy is IRGBOutputToy && ((IRGBOutputToy)Toy).OutputNameRed == LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName && ((IRGBOutputToy)Toy).OutputNameGreen == LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 1).OutputName && ((IRGBOutputToy)Toy).OutputNameBlue == LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 2).OutputName);

                                            }
                                            catch
                                            {
                                                //Toy does not exist. Create toyname and toy
                                                string ToyName = "LedWiz {0:00} Column {1:00}".Build(LedWizNr, TCC.Number);
                                                if (Cabinet.Toys.Contains(ToyName))
                                                {
                                                    int Cnt = 1;
                                                    while (Cabinet.Toys.Contains("{0} {1}".Build(ToyName, Cnt)))
                                                    {
                                                        Cnt++;
                                                    }
                                                    ToyName = "{0} {1}".Build(ToyName, Cnt);
                                                }
                                                //  if (Cabinet.Outputs.Contains(LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName) && Cabinet.Outputs.Contains(LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 1).OutputName) && Cabinet.Outputs.Contains(LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 2).OutputName))
                                                //  {
                                                TargetToy = (IToy)new RGBAToy() { Name = ToyName, OutputNameRed = LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName, OutputNameGreen = LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 1).OutputName, OutputNameBlue = LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 2).OutputName };
                                                Cabinet.Toys.Add(TargetToy);
                                                //  }
                                                //  else
                                                //  {
                                                //      Log.Warning("Unknown OutputName or ToyName defined for columns {0}-{1} (at least one of the 3) of LedWizEquivalent {2}.".Build(TCC.FirstOutputNumber, TCC.FirstOutputNumber + 3, LWE.Name));
                                                //  }
                                            }


                                        }
                                    }

                                    break;
                                case 1:
                                    //Single output

                                    //Analog output

                                    if (LWE.Outputs.Any(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber))
                                    {
                                        string OutputName = LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName;
                                        if (Cabinet.Toys.Any(O => O is IAnalogAlphaToy && O.Name == OutputName))
                                        {
                                            TargetToy = (IToy)Cabinet.Toys.FirstOrDefault(O => O is IAnalogAlphaToy && O.Name == OutputName);
                                        }
                                    }
                                    if (TargetToy == null)
                                    {


                                        if (LWE.Outputs.Any(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber))
                                        {
                                            try
                                            {
                                                TargetToy = Cabinet.Toys.First(Toy => Toy is ISingleOutputToy && ((ISingleOutputToy)Toy).OutputName == LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName);
                                            }
                                            catch
                                            {
                                                //Toy does not exist. Create toyname and toy
                                                string ToyName = "LedWiz {0:00} Column {1:00}".Build(LedWizNr, TCC.Number);

                                                if (Cabinet.Toys.Contains(ToyName))
                                                {
                                                    int Cnt = 1;
                                                    while (Cabinet.Toys.Contains("{0} {1}".Build(ToyName, Cnt)))
                                                    {
                                                        Cnt++;
                                                    }
                                                    ToyName = "{0} {1}".Build(ToyName, Cnt);
                                                }
                                                // if (Cabinet.Outputs.Contains(LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName))
                                                // {
                                                TargetToy = (IToy)new AnalogAlphaToy() { Name = ToyName, OutputName = LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName };
                                                Cabinet.Toys.Add(TargetToy);
                                                // }
                                                // else
                                                // {
                                                //     Log.Warning("Unknow ToyName or OutputName {0} defined for column {1} of LedwizEquivalent {2}.".Build(LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName, TCC.FirstOutputNumber, LWE.Name));
                                                // }
                                            }

                                        }

                                    }

                                    break;

                                default:
                                    //Unknow value
                                    Log.Warning("A illegal number ({0}) of required outputs has been found in a table config colum {0} for ledcontrol nr. {2}. Cant configure toy.".Build(TCC.RequiredOutputCount, TCC.Number, LedWizNr));
                                    break;
                            }
                        }

                        if (TargetToy != null)
                        {

                            ToyAssignments[LedWizNr].Add(TCC.Number, TargetToy);
                        }
                    }
                }
            }
            return ToyAssignments;
        }


    }
}
