using System;

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
        /// Gets or sets the type of the table element controlling a output.
        /// </summary>
        /// <value>
        /// The type of the table element.
        /// </value>
        public TableElementTypeEnum TableElementType { get; set; }
        /// <summary>
        /// Gets or sets the number of the table element controlling a output.
        /// </summary>
        /// <value>
        /// The table element number.
        /// </value>
        public int TableElementNumber { get; set; }



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

        private int _MinDurationMs=0;

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
        public int? Layer{get;set;}

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

            string[] Parts = SettingData.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (Parts.Length == 0)
            {
                Log.Warning("No data to parse.");

                throw new Exception("No data to parse.");

            }
            //Get output state and table element (if applicable)
            bool ParseOK = true;
            switch (Parts[0].ToUpper())
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
                    OutputControl = OutputControlEnum.FixedOn;
                    Blink = -1;
                    break;
                default:
                    if (Parts[0].Length > 1 && Parts[0].Substring(1).IsInteger())
                    {
                        OutputControl = OutputControlEnum.Controlled;
                        Char C = Parts[0].ToUpper().ToCharArray()[0];
                        if (Enum.IsDefined(typeof(TableElementTypeEnum), (int)C))
                        {
                            TableElementType = (TableElementTypeEnum)C;
                        }
                        else
                        {
                            ParseOK = false;
                        }

                        TableElementNumber = Parts[0].Substring(1).ToInteger();
                    }
                    else
                    {
                        ParseOK = false;
                    }

                    break;
            }
            if (!ParseOK)
            {
                Log.Warning("Cant parse the part {0} of the ledcontrol table config setting {1}.".Build(Parts[0], SettingData));

                throw new Exception("Cant parse the part {0} of the ledcontrol table config setting {1}.".Build(Parts[0], SettingData));

            }

            int IntegerCnt = 0;
            int PartNr = 1;
            while (Parts.Length > PartNr)
            {

                if (Parts[PartNr].ToUpper() == "BLINK")
                {
                    Blink = -1;
                    BlinkIntervalMs = 500;
                }
                else if (Parts[PartNr].Length > 1 && Parts[PartNr].ToUpper().Substring(0, 1) == "I" && Parts[PartNr].Substring(1).IsInteger())
                {
                    //Intensity setting
                    Intensity = Parts[PartNr].Substring(1).ToInteger().Limit(0, 48);
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
                else if (Parts[PartNr].Length>2 && Parts[PartNr].ToUpper().Substring(0, 2) == "FU" && Parts[PartNr].Substring(2).IsInteger())
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
                                BlinkIntervalMs = (Parts[PartNr].ToInteger()/2).Limit(1, int.MaxValue);
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
                                        BlinkIntervalMs = (DurationMs / Blink / 2).Limit(1, int.MaxValue);
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
                else if (PartNr == 1)
                {
                    //This should be a color
                    ColorName = Parts[PartNr];
                }
                else
                {
                    Log.Warning("Cant parse the part {0} of the ledcontrol table config setting {1}.".Build(Parts[PartNr], SettingData));

                    throw new Exception("Cant parse the part {0} of the ledcontrol table config setting {1}.".Build(Parts[PartNr], SettingData));
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
            this.Intensity = 48;
            this.Blink = 0;
            this.DurationMs = -1;

        }







    }
}
