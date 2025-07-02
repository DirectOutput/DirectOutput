using DirectOutput.FX.MatrixFX;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DirectOutput.LedControl.Loader
{
    /// <summary>
    /// A single setting from a LedControl.ini file.
    /// </summary>
    public class TableConfigSetting
    {


        /// <summary>
        /// Defines the control mode for a output. It can be constantly on, off or it can be controlled by a element of a pinball table.
        /// </summary>
        /// <value>
        /// The output control enum value.
        /// </value>
        public OutputControlEnum OutputControl { get; set; }

        /// <summary>
        /// Gets or sets the name of the color of the setting.<br/>
        /// This should only be set for RGB outputs.
        /// </summary>
        /// <value>
        /// The name of the color as specified in the color section of the Ledcontrol.ini file.
        /// </value>
        public string ColorName { get; set; }



        /// <summary>
        /// Gets or sets the color config.
        /// </summary>
        /// <value>
        /// The color config.
        /// </value>
        public ColorConfig ColorConfig { get; set; }


        /// <summary>
        /// The table element triggering the effect (if available)
        /// </summary>
        public string TableElement = null;


        /// <summary>
        /// The condition if available.
        /// </summary>
        public string Condition = null;


        /// <summary>
        /// Gets the type of the output.<br/>
        /// The value of this property depends on the value of the ColorName property.
        /// </summary>
        /// <value>
        /// The type of the output.
        /// </value>
        public OutputTypeEnum OutputType
        {
            get
            {
                return (!ColorName.IsNullOrWhiteSpace() ? OutputTypeEnum.RGBOutput : OutputTypeEnum.AnalogOutput);
            }
        }

        /// <summary>
        /// Gets or sets the duration in milliseconds.
        /// </summary>
        /// <value>
        /// The duration in milliseconds.
        /// </value>
        public int DurationMs { get; set; }

        private int _MinDurationMs = 0;

        /// <summary>
        /// Gets or sets the minimum duration in milliseconds.
        /// </summary>
        /// <value>
        /// The minimum duration in milliseconds.
        /// </value>
        public int MinDurationMs
        {
            get { return _MinDurationMs; }
            set { _MinDurationMs = value; }
        }


        /// <summary>
        /// Gets or sets the max duration for the effect in milliseconds.
        /// </summary>
        /// <value>
        /// The max duration of the effect in milliseconds.
        /// </value>
        public int MaxDurationMs { get; set; }

        /// <summary>
        /// Gets or sets the extended duration for the effect in milliseconds.
        /// </summary>
        /// <value>
        /// The extended duration of the effect in milliseconds.
        /// </value>
        public int ExtDurationMs { get; set; }

        private int _Intensity;
        /// <summary>
        /// Gets or sets the intensity.<br/>
        /// If the property <see cref="ColorName"/> is set, this property will always return -1.
        /// </summary>
        /// <value>
        /// The intensity.
        /// </value>
        public int Intensity
        {

            get { return (ColorName.IsNullOrWhiteSpace() ? _Intensity : -1); }
            set { _Intensity = value; }
        }

        private int _FadingDurationUpMs = 0;

        /// <summary>
        /// Gets or sets the duration for fading up in milliseconds.
        /// </summary>
        /// <value>
        /// The duration of the fading in milliseconds.
        /// </value>
        public int FadingUpDurationMs
        {
            get { return _FadingDurationUpMs; }
            set { _FadingDurationUpMs = value; }
        }

        private int _FadingDownDurationMs = 0;

        /// <summary>
        /// Gets or sets the duration for fading down in milliseconds.
        /// </summary>
        /// <value>
        /// The duration of the fading in milliseconds.
        /// </value>
        public int FadingDownDurationMs
        {
            get { return _FadingDownDurationMs; }
            set { _FadingDownDurationMs = value; }
        }

        /// <summary>
        /// Gets or sets the number blinks.
        /// </summary>
        /// <value>
        /// The number of blinks. -1 means infinite number of blinks.
        /// </value>
        public int Blink { get; set; }

        /// <summary>
        /// Gets or sets the blink interval in milliseconds.
        /// </summary>
        /// <value>
        /// The blink interval in  milliseconds.
        /// </value>
        public int BlinkIntervalMs { get; set; }


        /// <summary>
        /// Gets or sets the blink interval in milliseconds for nested blinking.
        /// </summary>
        /// <value>
        /// The blink interval in milliseconds for nested blinking.
        /// </value>
        public int BlinkIntervalMsNested { get; set; }

        private int _BlinkPulseWidthNested = 50;

        /// <summary>
        /// Gets or sets the width of the blink pulse for nested blinking.
        /// Value must be between 1 and 99 (defaults to 50).
        /// </summary>
        /// <value>
        /// The width of the blink pulse for nested blinking.
        /// </value>
        public int BlinkPulseWidthNested
        {
            get { return _BlinkPulseWidthNested; }
            set { _BlinkPulseWidthNested = value.Limit(1, 99); }
        }

        private int _BlinkPulseWidth = 50;

        /// <summary>
        /// Gets or sets the width of the blink pulse.
        /// Value must be between 1 and 99 (defaults to 50).
        /// </summary>
        /// <value>
        /// The width of the blink pulse.
        /// </value>
        public int BlinkPulseWidth
        {
            get { return _BlinkPulseWidth; }
            set { _BlinkPulseWidth = value.Limit(1, 99); }
        }


        public int BlinkLow;

        /// <summary>
        /// Gets or sets a value indicating whether the trigger value for the effect is inverted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if invert; otherwise, <c>false</c>.
        /// </value>
        public bool Invert { get; set; }

        /// <summary>
        /// Indicates the the trigger value of the effect is not to be treated as a boolean value resp. that the value should not be mapped to 0 or 255 (255 for all values which are not 0).
        /// </summary>
        /// <value>
        ///   <c>true</c> if [no bool]; otherwise, <c>false</c>.
        /// </value>
        public bool NoBool { get; set; }


        /// <summary>
        /// Gets or sets the wait duration before the effect is triggered.
        /// </summary>
        /// <value>
        /// The wait duration in milliseconds
        /// </value>
        public int WaitDurationMs { get; set; }


        /// <summary>
        /// Gets or sets the layer for the settings.
        /// </summary>
        /// <value>
        /// The layer for the settings.
        /// </value>
        public int? Layer { get; set; }


        public int AreaLeft = 0;
        public int AreaTop = 0;
        public int AreaWidth = 100;
        public int AreaHeight = 100;
        public int AreaSpeed = 100;
        public int AreaAcceleration = 0;
        public int AreaFlickerDensity = 0;
        public int AreaFlickerMinDurationMs = 0;
        public int AreaFlickerMaxDurationMs = 0;
        public int AreaFlickerFadeDurationMs = 0;
        public MatrixShiftDirectionEnum? AreaDirection = null;
        public bool IsArea = false;

        public bool IsBitmap = false;
        public int AreaBitmapTop = 0;
        public int AreaBitmapLeft = 0;
        public int AreaBitmapWidth = -1;
        public int AreaBitmapHeight = -1;
        public int AreaBitmapFrame = 0;

        public int AreaBitmapAnimationStepSize = 1;
        public int AreaBitmapAnimationStepCount = 0;
        public int AreaBitmapAnimationFrameDuration = 30;
        public MatrixAnimationStepDirectionEnum AreaBitmapAnimationDirection = MatrixAnimationStepDirectionEnum.Frame;
        public AnimationBehaviourEnum AreaBitmapAnimationBehaviour = AnimationBehaviourEnum.Loop;

        public string ShapeName=null;


        public bool IsPlasma = false;
        public int PlasmaSpeed = 100;
        public int PlasmaDensity = 100;
        public string ColorName2 = "";
        public ColorConfig ColorConfig2 = null;


        //public int PlasmaScale = 100;


        /// <summary>
        /// Parses the setting data. <br />
        /// </summary>
        /// <param name="SettingData">The setting data.</param>
        /// <exception cref="System.Exception">
        /// No data to parse.
        /// or
        /// Cant parse the part {0} of the ledcontrol table config setting {1}..
        /// </exception>
        public void ParseSettingData(string SettingData)
        {
            string S = SettingData.Trim();

            if (S.StartsWith("("))
            {
                //It is a condition

                int BracketCnt = 1;
                int ClosingBracketPos = -1;
                for (int i = 1; i < S.Length; i++)
                {
                    if (S[i] == '(') { BracketCnt++; }
                    else if (S[i] == ')') { BracketCnt--; }

                    if (BracketCnt == 0)
                    {
                        ClosingBracketPos = i;
                        break;
                    }
                }


                if (ClosingBracketPos > 0)
                {
                    Condition = S.Substring(0, ClosingBracketPos + 1).ToUpper();
                    OutputControl = OutputControlEnum.Condition;
                    S = S.Substring(Condition.Length).Trim();
                    //TODO: Maybe add a check for the condition validity

                }
                else
                {
                    Log.Warning("No closing bracket found for condition in setting {0}.".Build(S));

                    throw new Exception("No closing bracket found for condition in setting {0}.".Build(S));
                }




            }
            else
            {
                //not a condition

                if (S.Length == 0)
                {
                    Log.Warning("No data to parse.");

                    throw new Exception("No data to parse.");
                }

                int TriggerEndPos = -1;
                char LastChar = (char)0;
                for (int i = 0; i < S.Length - 1; i++)
                {
                    if (S[i] == ' ' && LastChar != '|' && LastChar != (char)0)
                    {
                        TriggerEndPos = i;
                        break;
                    }
                    if (S[i] != ' ')
                    {
                        LastChar = S[i];
                    }
                }
                if (TriggerEndPos == -1) TriggerEndPos = S.Length;

                string Trigger = S.Substring(0, TriggerEndPos).ToUpper().Trim();



                //Get output state and table element (if applicable)
                bool ParseOK = true;
                switch (Trigger)
                {
                    case "ON":
                    case "1":
                        OutputControl = OutputControlEnum.FixedOn;
                        break;
                    case "OFF":
                    case "0":
                        OutputControl = OutputControlEnum.FixedOff;
                        break;
                    case "B":
                    case "BLINK":
                        OutputControl = OutputControlEnum.FixedOn;
                        Blink = -1;
                        BlinkIntervalMs = 1000;
                        break;
                    default:
                        string[] ATE = Trigger.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(Tr => Tr.Trim()).ToArray();
                        foreach (string E in ATE)
                        {
                            if (E.Length > 1)
                            {
                                if (E[0] == (char)TableElementTypeEnum.NamedElement && E.Substring(1).All(C => char.IsLetterOrDigit(C) || C == '_'))
                                {
                                    //Named element
                                }
                                else if (Enum.IsDefined(typeof(TableElementTypeEnum), (int)E[0]) && E.Substring(1).IsInteger())
                                {
                                    //Normal table element
                                }
                                else
                                {
                                    Log.Error("Failed: " + E);
                                    ParseOK = false;
                                    break;
                                }
                            }
                            else
                            {
                                ParseOK = false;
                                break;
                            }


                        }
                        if (ParseOK)
                        {
                            OutputControl = OutputControlEnum.Controlled;
                            TableElement = Trigger;
                        }



                        break;
                }
                if (!ParseOK)
                {
                    Log.Warning("Cant parse the trigger part {0} of the ledcontrol table config setting {1}.".Build(Trigger, SettingData));

                    throw new Exception("Cant parse the part {0} of the ledcontrol table config setting {1}.".Build(Trigger, SettingData));
                }

                //Remove first part
                S = S.Substring(Trigger.Length).Trim();

            }


            string[] Parts = S.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


            int IntegerCnt = 0;
            int PartNr = 0;

            while (Parts.Length > PartNr)
            {

                if (Parts[PartNr].ToUpper() == "BLINK")
                {
                    Blink = -1;
                    BlinkIntervalMs = 1000;
                }
                else if (Parts[PartNr].ToUpper() == "INVERT")
                {
                    Invert = true;
                }
                else if (Parts[PartNr].ToUpper() == "NOBOOL")
                {
                    NoBool = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "APS" && Parts[PartNr].Substring(3).IsInteger())
                {
                    PlasmaSpeed = Parts[PartNr].Substring(3).ToInteger();
                    IsPlasma = true;
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "APD" && Parts[PartNr].Substring(3).IsInteger())
                {
                    PlasmaDensity = Parts[PartNr].Substring(3).ToInteger();
                    IsPlasma = true;
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "APC")
                {
                    ColorName2 = Parts[PartNr].Substring(3).ToUpper();
                    IsPlasma = true;
                    IsArea = true;
                }
                //else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "APC" && Parts[PartNr].Substring(3).IsInteger())
                //{
                //    PlasmaScale = Parts[PartNr].Substring(3).ToInteger();
                //    IsPlasma = true;
                //    IsArea = true;
                //}


                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "SHP" )
                {
                    ShapeName = Parts[PartNr].Substring(3).Trim().ToUpper();
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ABT" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapTop = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ABL" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapLeft = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ABW" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapWidth = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ABH" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapHeight = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ABF" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapFrame = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "AAS" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapAnimationStepSize = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "AAC" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapAnimationStepCount = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "AAF" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaBitmapAnimationFrameDuration = 1000 / Parts[PartNr].Substring(3).ToInteger().Limit(10, int.MaxValue);
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length == 4 && Parts[PartNr].Substring(0, 3).ToUpper() == "AAD" && Enum.IsDefined(typeof(MatrixAnimationStepDirectionEnum), (int)Parts[PartNr].Substring(3, 1).ToUpper()[0]))
                {

                    AreaBitmapAnimationDirection = (MatrixAnimationStepDirectionEnum)Parts[PartNr].Substring(3, 1).ToUpper()[0];
                    IsArea = true;
                    IsBitmap = true;
                }
                else if (Parts[PartNr].Length == 4 && Parts[PartNr].Substring(0, 3).ToUpper() == "AAB" && Enum.IsDefined(typeof(AnimationBehaviourEnum), (int)Parts[PartNr].Substring(3, 1).ToUpper()[0]))
                {

                    AreaBitmapAnimationBehaviour = (AnimationBehaviourEnum)Parts[PartNr].Substring(3, 1).ToUpper()[0];
                    IsArea = true;
                    IsBitmap = true;
                }

                else if (Parts[PartNr].Length > 5 && Parts[PartNr].Substring(0, 5).ToUpper() == "AFDEN" && Parts[PartNr].Substring(5).IsInteger())
                {
                    AreaFlickerDensity = Parts[PartNr].Substring(5).ToInteger();
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 5 && Parts[PartNr].Substring(0, 5).ToUpper() == "AFMIN" && Parts[PartNr].Substring(5).IsInteger())
                {
                    AreaFlickerMinDurationMs = Parts[PartNr].Substring(5).ToInteger();
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 5 && Parts[PartNr].Substring(0, 5).ToUpper() == "AFMAX" && Parts[PartNr].Substring(5).IsInteger())
                {
                    AreaFlickerMaxDurationMs = Parts[PartNr].Substring(5).ToInteger();
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 6 && Parts[PartNr].Substring(0, 6).ToUpper() == "AFFADE" && Parts[PartNr].Substring(6).IsInteger())
                {
                    AreaFlickerFadeDurationMs = Parts[PartNr].Substring(6).ToInteger();

                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].Substring(0, 2).ToUpper() == "AT" && Parts[PartNr].Substring(2).IsInteger())
                {
                    AreaTop = Parts[PartNr].Substring(2).ToInteger().Limit(0, 100);
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].Substring(0, 2).ToUpper() == "AL" && Parts[PartNr].Substring(2).IsInteger())
                {
                    AreaLeft = Parts[PartNr].Substring(2).ToInteger().Limit(0, 100);
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].Substring(0, 2).ToUpper() == "AW" && Parts[PartNr].Substring(2).IsInteger())
                {
                    AreaWidth = Parts[PartNr].Substring(2).ToInteger().Limit(0, 100);
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].Substring(0, 2).ToUpper() == "AH" && Parts[PartNr].Substring(2).IsInteger())
                {
                    AreaHeight = Parts[PartNr].Substring(2).ToInteger().Limit(0, 100);
                    IsArea = true;
                }
                //TODO: Remove parameter AA
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].Substring(0, 2).ToUpper() == "AA" && Parts[PartNr].Substring(2).IsInteger())
                {
                    AreaAcceleration = Parts[PartNr].Substring(2).ToInteger();
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ASA" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaAcceleration = Parts[PartNr].Substring(3).ToInteger();
                    IsArea = true;
                }

                    //TODO:Remove AS para
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].Substring(0, 2).ToUpper() == "AS" && Parts[PartNr].Substring(2).IsInteger())
                {
                    AreaSpeed = Parts[PartNr].Substring(2).ToInteger().Limit(1, 10000);
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].Substring(0, 3).ToUpper() == "ASS" && Parts[PartNr].Substring(3).IsInteger())
                {
                    AreaSpeed = Parts[PartNr].Substring(3).ToInteger().Limit(1, 10000);
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 5 && Parts[PartNr].Substring(0, 3).ToUpper() == "ASS" && Parts[PartNr].ToUpper().Right(2) == "MS" && Parts[PartNr].Substring(3, Parts[PartNr].Length - 5).IsInteger())
                {
                    AreaSpeed = (int)((double)100000 / Parts[PartNr].Substring(3, Parts[PartNr].Length - 5).ToInteger()).Limit(10, 100000);
                    IsArea = true;
                }

                    //TODO:Remove AD para
                else if (Parts[PartNr].Length == 3 && Parts[PartNr].Substring(0, 2).ToUpper() == "AD" && Enum.IsDefined(typeof(MatrixShiftDirectionEnum), (int)Parts[PartNr].Substring(2, 1).ToUpper()[0]))
                {

                    AreaDirection = (MatrixShiftDirectionEnum)Parts[PartNr].Substring(2, 1).ToUpper()[0];
                    IsArea = true;
                }
                else if (Parts[PartNr].Length == 4 && Parts[PartNr].Substring(0, 3).ToUpper() == "ASD" && Enum.IsDefined(typeof(MatrixShiftDirectionEnum), (int)Parts[PartNr].Substring(3, 1).ToUpper()[0]))
                {

                    AreaDirection = (MatrixShiftDirectionEnum)Parts[PartNr].Substring(3, 1).ToUpper()[0];
                    IsArea = true;
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].ToUpper().Substring(0, 3) == "MAX" && Parts[PartNr].Substring(3).IsInteger())
                {
                    MaxDurationMs = Parts[PartNr].Substring(3).ToInteger().Limit(0, int.MaxValue);
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].ToUpper().Substring(0, 3) == "BNP" && Parts[PartNr].Substring(3).IsInteger())
                {
                    BlinkIntervalMsNested = Parts[PartNr].Substring(3).ToInteger().Limit(0, int.MaxValue);
                }
                else if (Parts[PartNr].Length > 4 && Parts[PartNr].ToUpper().Substring(0, 4) == "BNPW" && Parts[PartNr].Substring(4).IsInteger())
                {
                    BlinkPulseWidthNested = Parts[PartNr].Substring(4).ToInteger().Limit(1, 99);
                }

                else if (Parts[PartNr].Length > 3 && Parts[PartNr].ToUpper().Substring(0, 3) == "BPW" && Parts[PartNr].Substring(3).IsInteger())
                {
                    BlinkPulseWidth = Parts[PartNr].Substring(3).ToInteger().Limit(1, 99);
                }
                else if (Parts[PartNr].Length > 3 && Parts[PartNr].ToUpper().Substring(0, 3) == "BL#" && Parts[PartNr].Substring(3).IsHexString())
                {

                    BlinkLow = Parts[PartNr].Substring(3).HexToInt().Limit(0, 255);
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "BL" && Parts[PartNr].Substring(1).IsInteger())
                {

                    BlinkLow = (int)(((double)Parts[PartNr].Substring(2).ToInteger().Limit(0, 48)) * 5.3125);
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "E" && Parts[PartNr].Substring(1).IsInteger())
                {

                    ExtDurationMs = Parts[PartNr].Substring(1).ToInteger().Limit(0, int.MaxValue);
                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].ToUpper().Substring(0, 2) == "I#" && Parts[PartNr].Substring(2).IsHexString())
                {
                    //Intensity setting
                    Intensity = Parts[PartNr].Substring(2).HexToInt().Limit(0, 255);
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "I" && Parts[PartNr].Substring(1).IsInteger())
                {
                    //Intensity setting
                    Intensity = (int)(((double)Parts[PartNr].Substring(1).ToInteger().Limit(0, 48)) * 5.3125);
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "L" && Parts[PartNr].Substring(1).IsInteger())
                {
                    //Layer setting
                    Layer = Parts[PartNr].Substring(1).ToInteger();
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "W" && Parts[PartNr].Substring(1).IsInteger())
                {
                    //WaitDuration setting
                    WaitDurationMs = Parts[PartNr].Substring(1).ToInteger().Limit(0, int.MaxValue);
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "M" && Parts[PartNr].Substring(1).IsInteger())
                {
                    //MinimumDuration setting
                    MinDurationMs = Parts[PartNr].Substring(1).ToInteger().Limit(0, int.MaxValue);
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "F" && Parts[PartNr].Substring(1).IsInteger())
                {

                    //Dimming duration for up and down
                    FadingUpDurationMs = Parts[PartNr].Substring(1).ToInteger().Limit(0, int.MaxValue);
                    FadingDownDurationMs = FadingUpDurationMs;
                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].ToUpper().Substring(0, 2) == "FU" && Parts[PartNr].Substring(2).IsInteger())
                {

                    //Dimming up duration
                    FadingUpDurationMs = Parts[PartNr].Substring(2).ToInteger().Limit(0, int.MaxValue);

                }
                else if (Parts[PartNr].Length > 2 && Parts[PartNr].ToUpper().Substring(0, 2) == "FD" && Parts[PartNr].Substring(2).IsInteger())
                {

                    //Dimming down duration
                    FadingDownDurationMs = Parts[PartNr].Substring(2).ToInteger().Limit(0, int.MaxValue);
                }
                else if (Parts[PartNr].IsInteger())
                {
                    switch (IntegerCnt)
                    {
                        case 0:
                            if (Blink == -1)
                            {
                                //Its a blink interval
                                BlinkIntervalMs = (Parts[PartNr].ToInteger()).Limit(1, int.MaxValue);
                                DurationMs = 0;
                            }
                            else
                            {
                                //Its a duration

                                DurationMs = Parts[PartNr].ToInteger().Limit(1, int.MaxValue);
                            }
                            break;
                        case 1:
                            if (Blink != -1)
                            {
                                Blink = Parts[PartNr].ToInteger().Limit(1, int.MaxValue);
                                if (DurationMs > 0 & Blink >= 1)
                                {
                                    BlinkIntervalMs = (DurationMs / Blink).Limit(1, int.MaxValue);
                                    DurationMs = 0;

                                }
                            }
                            break;
                        default:
                            Log.Warning("The ledcontrol table config setting {0} contains more than 2 numeric values without a type definition.".Build(SettingData));
                            throw new Exception("The ledcontrol table config setting {0} contains more than 2 numeric values without a type definition.".Build(SettingData));

                    }
                    IntegerCnt++;
                }
                // if Parts[PartNr] starts with # and a HTML-style hex color value we assume a color.
                else if (Regex.IsMatch(Parts[PartNr], @"^#"))
                {
                    if (!Regex.IsMatch(Parts[PartNr], @"^#[0-9A-Fa-f]{6,8}$"))
                    {
                        Log.Warning("Invalid '#' HTML-style color code \"{0}\", #rrggbb or #rrggbbaa required".Build(Parts[PartNr]));
                        throw new Exception("Invalid '#' HTML-style color code \"{0}\", #rrggbb or #rrggbbaa required".Build(Parts[PartNr]));
                    }
                    // This should be a HTML-style hex color string
                    ColorName = Parts[PartNr].ToUpper();
                }
                // if Parts[PartNr] contains only capital and small caps letters or underscore we assume a color
                else if (Parts[PartNr].Length > 2 && Regex.IsMatch(Parts[PartNr], @"^[A-Za-z_]+$"))
                {
                    ColorName = Parts[PartNr].ToUpper();
                }
                else
                {
                    Log.Warning("Cant parse the part {0} of the ledcontrol table config setting {1}".Build(Parts[PartNr], SettingData));

                    throw new Exception("Cant parse the part {0} of the ledcontrol table config setting {1}".Build(Parts[PartNr], SettingData));
                }
                PartNr++;
            }




        }




        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfigSetting"/> class.
        /// Parses the setting data. <br/>
        /// </summary>
        /// <param name="SettingData">The setting data.</param>
        /// <exception cref="System.Exception">
        /// No data to parse.<br/>
        /// or <br/>
        /// Cant parse the part {0} of the ledcontrol table config setting {1}.
        /// </exception>
        public TableConfigSetting(string SettingData)
            : this()
        {
            ParseSettingData(SettingData);
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfigSetting"/> class.
        /// </summary>
        public TableConfigSetting()
        {
            this.Intensity = 255;
            this.Blink = 0;
            this.DurationMs = -1;

        }







    }
}
