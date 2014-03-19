using System.Collections.Generic;
using System.Linq;
using DirectOutput.Cab;
using DirectOutput.Cab.Out;
using DirectOutput.Cab.Toys;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.Cab.Toys.LWEquivalent;
using DirectOutput.FX;
using DirectOutput.FX.AnalogToyFX;
using DirectOutput.FX.RGBAFX;
using DirectOutput.FX.TimmedFX;
using DirectOutput.LedControl.Loader;
using DirectOutput.General.Color;
using DirectOutput.FX.ValueFX;
using DirectOutput.FX.RGBAMatrixFX;
using System;
using DirectOutput.FX.ConditionFX;

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
            Dictionary<int, TableConfig> TableConfigDict = LedControlConfigList.GetTableConfigDictonary(RomName);

            Dictionary<int, Dictionary<int, IToy>> ToyAssignments = SetupCabinet(TableConfigDict, Cabinet);

            SetupTable(Table, TableConfigDict, ToyAssignments);


        }

        private void SetupTable(Table.Table Table, Dictionary<int, TableConfig> TableConfigDict, Dictionary<int, Dictionary<int, IToy>> ToyAssignments)
        {
            foreach (KeyValuePair<int, TableConfig> KV in TableConfigDict)
            {
                int LedWizNr = KV.Key;
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

                                if (Toy is LedStrip)
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

                                    if (ActiveColor != null)
                                    {
                                        RGBAColor InactiveColor = ActiveColor.Clone();
                                        InactiveColor.Alpha = 0;
                                        if (TCS.AreaDirection.HasValue)
                                        {
                                            //shift effect
                                            Effect = new RGBAMatrixColorShiftEffect() { ShiftDirection = TCS.AreaDirection.Value, ShiftAcceleration=TCS.AreaAcceleration, ActiveColor = ActiveColor, InactiveColor = InactiveColor, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, LayerNr = Layer, ToyName = Toy.Name };
                                            if (TCS.AreaSpeed > 0)
                                            {
                                               ((RGBAMatrixColorShiftEffect)Effect).ShiftSpeed = TCS.AreaSpeed;
                                            }
                                        
                                        }
                                        else if (TCS.AreaFlickerDensity > 0)
                                        {
                                            //flicker effect
                                            Effect = new RGBAMatrixColorFlickerEffect() {Density=TCS.AreaFlickerDensity.Limit(1,99), ActiveColor = ActiveColor, InactiveColor = InactiveColor, Height = TCS.AreaHeight, Width = TCS.AreaWidth, Top = TCS.AreaTop, Left = TCS.AreaLeft, LayerNr = Layer, ToyName = Toy.Name };
                                            if (TCS.AreaFlickerMinDurationMs > 0)
                                            {
                                                ((RGBAMatrixColorFlickerEffect)Effect).MinFlickerDurationMs = TCS.AreaFlickerMinDurationMs;
                                            }
                                            if (TCS.AreaFlickerMaxDurationMs > 0)
                                            {
                                                ((RGBAMatrixColorFlickerEffect)Effect).MaxFlickerDurationMs = TCS.AreaFlickerMaxDurationMs;
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
                                        Log.Warning("No color valid color definition found for earea effect. Skipped setting {0} in column {1} for LedWizEqivalent number {2}.".Build(SettingNumber, TCC.Number, LedWizNr));

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
                                    AnalogAlphaValue ActiveValue = new AnalogAlphaValue(TCS.Intensity.Limit(0, 255), 255);
                                    AnalogAlphaValue InactiveValue = ActiveValue.Clone();
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
                                        Effect = new BlinkEffect() { Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} BlinkEffect Inner".Build(LedWizNr, TCC.Number, SettingNumber), TargetEffectName = Effect.Name, DurationActiveMs = (int)((double)TCS.BlinkIntervalMsNested * (double)TCS.BlinkPulseWidthNested / 100), DurationInactiveMs = (int)((double)TCS.BlinkIntervalMsNested * (100 - (double)TCS.BlinkPulseWidthNested) / 100) };
                                        MakeEffectNameUnique(Effect, Table);
                                        Table.Effects.Add(Effect);
                                    }


                                    if (TCS.Blink != 0)
                                    {
                                        Effect = new BlinkEffect() { Name = "Ledwiz {0:00} Column {1:00} Setting {2:00} BlinkEffect".Build(LedWizNr, TCC.Number, SettingNumber), TargetEffectName = Effect.Name, DurationActiveMs = (int)((double)TCS.BlinkIntervalMs * (double)TCS.BlinkPulseWidth / 100), DurationInactiveMs = (int)((double)TCS.BlinkIntervalMs * (100 - (double)TCS.BlinkPulseWidth) / 100) };
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

                                            foreach (string Variable in ((TableElementConditionEffect)Effect).GetVariables())
                                            {
                                                TableElementTypeEnum TET = (TableElementTypeEnum)Variable[0];
                                                int TEN = Variable.Substring(1).ToInteger();
                                                if (!Table.TableElements.Contains(TET, TEN))
                                                {
                                                    Table.TableElements.UpdateState(TET, TEN, 0);
                                                }
                                                Table.TableElements[TET, TEN].AssignedEffects.Add(new AssignedEffect(Effect.Name));
                                            }
                                            
                                            break;


                                        case OutputControlEnum.FixedOn:
                                            Table.AssignedStaticEffects.Add(new AssignedEffect(Effect.Name));
                                            break;
                                        case OutputControlEnum.Controlled:

                                            string[] ATE = TCS.TableElement.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (string TE in ATE)
                                            {
                                                TableElementTypeEnum TET = (TableElementTypeEnum)TE[0];
                                                int TEN = TE.Substring(1).ToInteger();
                                                if (!Table.TableElements.Contains(TET, TEN))
                                                {
                                                    Table.TableElements.UpdateState(TET, TEN, 0);
                                                }
                                                Table.TableElements[TET, TEN].AssignedEffects.Add(new AssignedEffect(Effect.Name));
                                            }


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
                LedWizEquivalentDict.Add(((LedWizEquivalent)T).LedWizNumber, (LedWizEquivalent)T);
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
                                if (Cabinet.Toys.Any(O => O is LedStrip && O.Name == OutputName))
                                {


                                    TargetToy = (IToy)Cabinet.Toys.FirstOrDefault(O => O is LedStrip && O.Name == OutputName);

                                    if (TargetToy != null)
                                    {
                                        ToyAssignments[LedWizNr].Add(TCC.Number, TargetToy);
                                    }

                                }



                            }
                        }
                        else
                        {
                            switch (TCC.RequiredOutputCount)
                            {
                                case 3:
                                    //RGB Led

                                    if (LWE.Outputs.Any(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber) && LWE.Outputs.Any(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 1) && LWE.Outputs.Any(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 2))
                                    {
                                        //Try to get the toy 
                                        try
                                        {
                                            //Toy does already exist
                                            TargetToy = (IToy)Cabinet.Toys.First(Toy => Toy is IRGBAToy && ((IRGBAToy)Toy).OutputNameRed == LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName && ((IRGBAToy)Toy).OutputNameGreen == LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 1).OutputName && ((IRGBAToy)Toy).OutputNameBlue == LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 2).OutputName);

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
                                            TargetToy = (IToy)new RGBAToy() { Name = ToyName, OutputNameRed = LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName, OutputNameGreen = LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 1).OutputName, OutputNameBlue = LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber + 2).OutputName };
                                            Cabinet.Toys.Add(TargetToy);
                                        }

                                        ToyAssignments[LedWizNr].Add(TCC.Number, TargetToy);
                                    }

                                    break;
                                case 1:
                                    //Single output

                                    //Analog output
                                    if (LWE.Outputs.Any(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber))
                                    {
                                        try
                                        {
                                            TargetToy = Cabinet.Toys.First(Toy => Toy is IAnalogAlphaToy && ((IAnalogAlphaToy)Toy).OutputName == LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName);
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
                                            TargetToy = (IToy)new AnalogAlphaToy() { Name = ToyName, OutputName = LWE.Outputs.First(Output => Output.LedWizEquivalentOutputNumber == TCC.FirstOutputNumber).OutputName };
                                            Cabinet.Toys.Add(TargetToy);
                                        }
                                        ToyAssignments[LedWizNr].Add(TCC.Number, TargetToy);
                                    }



                                    break;

                                default:
                                    //Unknow value
                                    Log.Warning("A illegal number ({0}) of required outputs has been found in a table config colum {0} for ledcontrol nr. {2}. Cant configure toy.".Build(TCC.RequiredOutputCount, TCC.Number, LedWizNr));
                                    break;
                            }
                        }
                    }
                }
            }
            return ToyAssignments;
        }


    }
}
