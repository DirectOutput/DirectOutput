using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Out;
using System.Xml.Serialization;
using DirectOutput.General;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Color;
using DirectOutput.Cab.Schedules;
using DirectOutput.Cab.Toys.LWEquivalent;
using DirectOutput.Cab.Overrides;

namespace DirectOutput.Cab.Toys.Hardware
{
    /// <summary>
    /// Represents a adressable led strip. 
    /// 
    /// The toy supports several layers and supports transparency/alpha channels for every single led.
    /// </summary>
    public class LedStrip : ToyBaseUpdatable, IToy, IMatrixToy<RGBAColor>
    {
        #region Config properties
        private int _Width = 1;

        /// <summary>
        /// Gets or sets the width resp. number of leds in horizontal direction of the led stripe. 
        /// </summary>
        /// <value>
        /// The width of the led stripe.
        /// </value>
        public int Width
        {
            get { return _Width; }
            set { _Width = value.Limit(0, int.MaxValue); }
        }

        private int _Height = 1;

        /// <summary>
        /// Gets or sets the height resp. the number of leds in vertical direction of the led strip.
        /// </summary>
        /// <value>
        /// The height of the led stripe.
        /// </value>
        public int Height
        {
            get { return _Height; }
            set { _Height = value.Limit(0, int.MaxValue); }
        }


        /// <summary>
        /// Gets the number of leds of the led stripe.
        /// </summary>
        /// <value>
        /// The number of leds of the led stripe.
        /// </value>
        public int NumberOfLeds
        {
            get { return Width * Height; }
        }


        /// <summary>
        /// Gets the number of outputs required for the ledstrip.
        /// </summary>
        /// <value>
        /// The number of outputs of the ledstrip.
        /// </value>
        public int NumberOfOutputs
        {
            get { return NumberOfLeds * 3; }
        }


        private LedStripArrangementEnum _LedStripAranggement = LedStripArrangementEnum.LeftRightTopDown;

        /// <summary>
        /// Gets or sets the strip arrangement.
        /// The following image explains the meaining of the different values.
        ///  \image html LedStripArrangementEnum.jpg Supported led string arrangements
        /// </summary>
        /// <value>
        /// The strip arrangement value as defined in the LedStripArrangementEnum.
        ///  \image html LedStripArrangementEnum.jpg Supported led string arrangements
        ///  </value>
        public LedStripArrangementEnum LedStripArrangement
        {
            get { return _LedStripAranggement; }
            set { _LedStripAranggement = value; }
        }



        private RGBOrderEnum _ColorOrder = RGBOrderEnum.RBG;

        /// <summary>
        /// Gets or sets the order of the colors for the leds of the led strip.
        /// Usually colors are represented in RGB (Red - Green - Blue) order, but depending on the type of the used strip the color order might be different (e.g. WS2812 led chips have green - red - blue as their color order).
        /// </summary>
        /// <value>
        /// The color order of the leds on the strip.
        /// </value>
        public RGBOrderEnum ColorOrder
        {
            get { return _ColorOrder; }
            set { _ColorOrder = value; }
        }


        private int _FirstLedNumber = 1;
        /// <summary>
        /// Gets or sets the number of the first led of the strip.
        /// </summary>
        /// <value>
        /// The number of the first led of the strip.
        /// </value>
        public int FirstLedNumber
        {
            get { return _FirstLedNumber; }
            set { _FirstLedNumber = value.Limit(1, int.MaxValue); }
        }

        private string _FadingCurveName = "Linear";
        private Curve FadingCurve = null;

        /// <summary>
        /// Gets or sets the name of the fading curve as defined in the Curves list of the cabinet object.
        /// This curve can be used to adjust the brightness values for the led to the brightness perception of the human eye.
        /// </summary>
        /// <value>
        /// The name of the fading curve.
        /// </value>
        public string FadingCurveName
        {
            get { return _FadingCurveName; }
            set { _FadingCurveName = value; }
        }

        private void InitFadingCurve(Cabinet Cabinet)
        {
            if (Cabinet.Curves.Contains(FadingCurveName))
            {
                FadingCurve = Cabinet.Curves[FadingCurveName];
            }
            else if (!FadingCurveName.IsNullOrWhiteSpace())
            {
                if (Enum.GetNames(typeof(Curve.CurveTypeEnum)).Contains(FadingCurveName))
                {
                    Curve.CurveTypeEnum T = Curve.CurveTypeEnum.Linear;
                    Enum.TryParse(FadingCurveName, out T);
                    FadingCurve = new Curve(T);
                }
                else
                {
                    FadingCurve = new Curve(Curve.CurveTypeEnum.Linear) { Name = FadingCurveName };
                    Cabinet.Curves.Add(FadingCurveName);
                }
            }
            else
            {
                FadingCurve = new Curve(Curve.CurveTypeEnum.Linear);
            }

        }

        /// <summary>
        /// The brightness ratio in percent of the Ledstrip, will be applied at the end of the Values calculation.
        /// Value range from 0 (nothing displayed) to 100 (output untouched)
        /// </summary>
        /// <remarks>
        /// Will allow users to adjust their led brightness regarding their cabinet setup without reflashing any controller firmware.
        /// </remarks>
        private float _Brightness = 1.0f;
        public int Brightness
        {
            get {
                return (int)((_Brightness * 100.0f) + 0.5f);
            }
            set {
                _Brightness = (value * 0.01f).Limit(0.0f, 1.0f);
            }
        }

        /// <summary>
        /// Gets or sets the name of the output controller to be used.
        /// </summary>
        /// <value>
        /// The name of the output controller.
        /// </value>
        public string OutputControllerName { get; set; }

        private ISupportsSetValues OutputController;

        #endregion


        /// <summary>
        /// Gets the layers dictionary of the toy.
        /// </summary>
        /// <value>
        /// The layers dictionary of the toy.
        /// </value>
        [XmlIgnore]
        public MatrixDictionaryBase<RGBAColor> Layers { get; private set; }

        
        #region IToy methods

        Cabinet Cabinet;
        /// <summary>
        /// Initializes the toy.
        /// </summary>
        /// <param name="Cabinet"><see cref="Cabinet" /> object  to which the <see cref="IToy" /> belongs.</param>
        public override void Init(Cabinet Cabinet)
        {
            this.Cabinet = Cabinet;

            if (Cabinet.OutputControllers.Contains(OutputControllerName) && Cabinet.OutputControllers[OutputControllerName] is ISupportsSetValues)
            {
                OutputController = (ISupportsSetValues)Cabinet.OutputControllers[OutputControllerName];
            }

            BuildMappingTables();
            OutputData = new byte[NumberOfOutputs];
            InitFadingCurve(Cabinet);

            Layers = new MatrixDictionaryBase<RGBAColor>() { Width = Width, Height = Height };
        }



        /// <summary>
        /// Resets the toy. Turns all outputs off.
        /// </summary>
        public override void Reset()
        {
            if (OutputController != null && NumberOfLeds > 0)
            {
                OutputController.SetValues((FirstLedNumber - 1) * 3, new byte[NumberOfLeds]);
            }
        }

        /// <summary>
        /// Updates the data of the assigned output controller
        /// </summary>
        public override void UpdateOutputs()
        {
            if (OutputController != null && Layers.Count > 0)
            {
                SetOutputData();
                OutputController.SetValues((FirstLedNumber-1)*3, OutputData);

            };
        }

        #endregion

        /// <summary>
        /// Gets the 2 dimensional RGBAColor array for the specified layer.
        /// 
        /// Dimension 0 of the array represents the x resp. horizontal direction. Dimension 1 of the array repersent the y resp. vertical direction.
        /// Position 0,0 is the upper left corner of the ledarray.
        /// 
        /// If the specified layer does not exist, it will be created as a fully transparent layer where all positions are set to transparent black.
        /// </summary>
        /// <param name="LayerNr">The layer nr.</param>
        /// <returns>The RGBAColor array for the specified layer.</returns>
        public RGBAColor[,] GetLayer(int LayerNr)
        {
            return Layers[LayerNr];
        }




        //private int[,] LedMappingTable = new int[0, 0];
        private int[,] OutputMappingTable = new int[0, 0];

        private void BuildMappingTables()
        {
            //LedMappingTable = new int[Width, Height];
            OutputMappingTable = new int[Width, Height];
            bool FirstException = true;
            int LedNr = 0;

            for (int Y = 0; Y < Height; Y++)
            {
                for (int X = 0; X < Width; X++)
                {

                    switch (LedStripArrangement)
                    {
                        case LedStripArrangementEnum.LeftRightTopDown:
                            LedNr = (Y * Width) + X;
                            break;
                        case LedStripArrangementEnum.LeftRightBottomUp:
                            LedNr = ((Height - 1 - Y) * Width) + X;
                            break;
                        case LedStripArrangementEnum.RightLeftTopDown:
                            LedNr = (Y * Width) + (Width - 1 - X);
                            break;
                        case LedStripArrangementEnum.RightLeftBottomUp:
                            LedNr = ((Height - 1 - Y) * Width) + (Width - 1 - X);
                            break;
                        case LedStripArrangementEnum.TopDownLeftRight:
                            LedNr = X * Height + Y;
                            break;
                        case LedStripArrangementEnum.TopDownRightLeft:
                            LedNr = ((Width - 1 - X) * Height) + Y;
                            break;
                        case LedStripArrangementEnum.BottomUpLeftRight:
                            LedNr = (X * Height) + (Height - 1 - Y);
                            break;
                        case LedStripArrangementEnum.BottomUpRightLeft:
                            LedNr = ((Width - 1 - X) * Height) + (Height - 1 - Y);
                            break;
                        case LedStripArrangementEnum.LeftRightAlternateTopDown:
                            LedNr = (Width * Y) + ((Y & 1) == 0 ? X : (Width - 1 - X));
                            break;
                        case LedStripArrangementEnum.LeftRightAlternateBottomUp:
                            LedNr = (Width * (Height - 1 - Y)) + (((Height - 1 - Y) & 1) == 0 ? X : (Width - 1 - X));
                            break;
                        case LedStripArrangementEnum.RightLeftAlternateTopDown:
                            LedNr = (Width * Y) + ((Y & 1) == 1 ? X : (Width - 1 - X));
                            break;
                        case LedStripArrangementEnum.RightLeftAlternateBottomUp:
                            LedNr = (Width * (Height - 1 - Y)) + (((Height - 1 - Y) & 1) == 1 ? X : (Width - 1 - X));
                            break;
                        case LedStripArrangementEnum.TopDownAlternateLeftRight:
                            LedNr = (Height * X) + ((X & 1) == 0 ? Y : (Height - 1 - Y));
                            break;
                        case LedStripArrangementEnum.TopDownAlternateRightLeft:
                            LedNr = (Height * (Width - 1 - X)) + ((X & 1) == 1 ? Y : (Height - 1 - Y));
                            break;
                        case LedStripArrangementEnum.BottomUpAlternateLeftRight:
                            LedNr = (Height * X) + ((X & 1) == 1 ? Y : (Height - 1 - Y));
                            break;
                        case LedStripArrangementEnum.BottomUpAlternateRightLeft:
                            LedNr = (Height * (Width - 1 - X)) + ((X & 1) == 0 ? Y : (Height - 1 - Y));
                            break;
                        default:
                            if (FirstException)
                            {
                                Log.Exception("Unknow LedStripArrangement value ({0}) found. Will use LeftRightTopDown mapping as fallback.".Build(LedStripArrangement.ToString()));
                                FirstException = false;
                            };
                            LedNr = (Y * Width) + X;
                            break;
                    }
                    //LedMappingTable[X, Y] = LedNr;
                    OutputMappingTable[X, Y] = LedNr * 3;
                }
            }

        }

        //Array for output data is not in GetResultingValues to avoid reinitiaslisation of the array
        byte[] OutputData = new byte[0];

        /// <summary>
        /// Returns calculated FadingTable using input percent 0-100.
        /// </summary>
        /// <param name="outputPercent"></param>
        /// <returns></returns>
        private byte[] getfadingtablefromPercent (int outputPercent) {
            byte[] fadingTable = FadingCurve.Data;

            if (outputPercent == 100) {
                //leave default running
            } else if (outputPercent >= 89) {
                fadingTable = new Curve(Curve.CurveTypeEnum.Linear0To224).Data;
            } else if (outputPercent >= 78) {
                fadingTable = new Curve(Curve.CurveTypeEnum.Linear0To192).Data;
            } else if (outputPercent >= 67) {
                fadingTable = new Curve(Curve.CurveTypeEnum.Linear0To160).Data;
            } else if (outputPercent >= 56) {
                fadingTable = new Curve(Curve.CurveTypeEnum.Linear0To128).Data;
            } else if (outputPercent >= 45) {
                fadingTable = new Curve(Curve.CurveTypeEnum.Linear0To96).Data;
            } else if (outputPercent >= 34) {
                fadingTable = new Curve(Curve.CurveTypeEnum.Linear0To64).Data;
            } else if (outputPercent >= 23) {
                fadingTable = new Curve(Curve.CurveTypeEnum.Linear0To32).Data;
            } else {
                fadingTable = new Curve(Curve.CurveTypeEnum.Linear0To16).Data;
            }

            return fadingTable;
        }

        /// <summary>
        /// Gets a array of bytes values re'presenting the data to be sent to the led strip.
        /// </summary>
        /// <returns></returns>
        private void SetOutputData()
        {
            if (Layers.Count > 0)
            {
                //Ledstrip brightness is zero, won't output anything
                if (_Brightness == 0.0f) {
                    OutputData.Fill<byte>(0);
                    return;
                }

                //Blend layers
                float[, ,] Value = new float[Width, Height, 3];

                foreach (KeyValuePair<int, RGBAColor[,]> KV in Layers)
                {
                    RGBAColor[,] D = KV.Value;

                    int Nr = 0;
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            int Alpha = D[x, y].Alpha.Limit(0, 255);
                            if (Alpha != 0)
                            {
                                Value[x, y, 0] = AlphaMappingTable.AlphaMapping[255 - Alpha, (int)Value[x, y, 0]] + AlphaMappingTable.AlphaMapping[Alpha, D[x, y].Red.Limit(0, 255)];
                                Value[x, y, 1] = AlphaMappingTable.AlphaMapping[255 - Alpha, (int)Value[x, y, 1]] + AlphaMappingTable.AlphaMapping[Alpha, D[x, y].Green.Limit(0, 255)];
                                Value[x, y, 2] = AlphaMappingTable.AlphaMapping[255 - Alpha, (int)Value[x, y, 2]] + AlphaMappingTable.AlphaMapping[Alpha, D[x, y].Blue.Limit(0, 255)];
                            }
                            Nr++;

           

                        }
                    }
                }

                //the following code mapps the led data to the outputs of the stripe
                //to affect output strength, modify fadingtable during overrides
                byte[] FadingTable = FadingCurve.Data;

                //wrapper object to specify output strength so we have something to modify
                Output newOutput = new Output();
                List<LedWizEquivalent> LWELedstripList = new List<LedWizEquivalent>(Cabinet.Toys.Where(OC => OC is LedWizEquivalent).Select(LW => ((LedWizEquivalent)LW)));
                LedWizEquivalent LWE = LWELedstripList[0];

                //wrap firstlednumber into an output number and channel strength 0-100 as output value
                newOutput.Number = FirstLedNumber;
                newOutput.Value = 100;

                //check for table overrides
                newOutput.Value = TableOverrideSettings.Instance.getnewrecalculatedOutput(newOutput, 30, LWE.LedWizNumber - 30).Value;

                //check for scheduled setting
                newOutput.Value = ScheduledSettings.Instance.getnewrecalculatedOutput(newOutput, 30, LWE.LedWizNumber - 30).Value;

                //get fadingtable from finale percent output
                FadingTable = getfadingtablefromPercent(newOutput.Value);

                switch (ColorOrder)
                {
                    case RGBOrderEnum.RBG:
                        for (int y = 0; y < Height; y++)
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                int OutputNumber = OutputMappingTable[x, y];
                                OutputData[OutputNumber] = FadingTable[(int)Value[x, y, 0]];
                                OutputData[OutputNumber + 2] = FadingTable[(int)Value[x, y, 1]];
                                OutputData[OutputNumber + 1] = FadingTable[(int)Value[x, y, 2]];
                            }
                        }
                        break;
                    case RGBOrderEnum.GRB:
                        for (int y = 0; y < Height; y++)
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                int OutputNumber = OutputMappingTable[x, y];
                                OutputData[OutputNumber + 1] = FadingTable[(int)Value[x, y, 0]];
                                OutputData[OutputNumber] = FadingTable[(int)Value[x, y, 1]];
                                OutputData[OutputNumber + 2] = FadingTable[(int)Value[x, y, 2]];

                 
                            }
                        }
                        break;
                    case RGBOrderEnum.GBR:
                        for (int y = 0; y < Height; y++)
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                int OutputNumber = OutputMappingTable[x, y];
                                OutputData[OutputNumber + 1] = FadingTable[(int)Value[x, y, 0]];
                                OutputData[OutputNumber + 2] = FadingTable[(int)Value[x, y, 1]];
                                OutputData[OutputNumber] = FadingTable[(int)Value[x, y, 2]];
                            }
                        }
                        break;
                    case RGBOrderEnum.BRG:
                        for (int y = 0; y < Height; y++)
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                int OutputNumber = OutputMappingTable[x, y];
                                OutputData[OutputNumber + 2] = FadingTable[(int)Value[x, y, 0]];
                                OutputData[OutputNumber] = FadingTable[(int)Value[x, y, 1]];
                                OutputData[OutputNumber + 1] = FadingTable[(int)Value[x, y, 2]];
                            }
                        }
                        break;
                    case RGBOrderEnum.BGR:
                        for (int y = 0; y < Height; y++)
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                int OutputNumber = OutputMappingTable[x, y];
                                OutputData[OutputNumber + 2] = FadingTable[(int)Value[x, y, 0]];
                                OutputData[OutputNumber + 1] = FadingTable[(int)Value[x, y, 1]];
                                OutputData[OutputNumber] = FadingTable[(int)Value[x, y, 2]];
                            }
                        }
                        break;
                    case RGBOrderEnum.RGB:
                    default:
                        for (int y = 0; y < Height; y++)
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                int OutputNumber = OutputMappingTable[x, y];
                                OutputData[OutputNumber] = FadingTable[(int)Value[x, y, 0]];
                                OutputData[OutputNumber + 1] = FadingTable[(int)Value[x, y, 1]];
                                OutputData[OutputNumber + 2] = FadingTable[(int)Value[x, y, 2]];
                            }
                        }
                        break;
                }

                //Apply ledstrip brightness if needed
                if (_Brightness < 1.0f) {
                    for (var num = 0; num < OutputData.Length; ++num) {
                        OutputData[num] = (byte)(OutputData[num] * _Brightness);
                    }
                }
            }



            //for (int i = 0; i < OutputData.Length-3; i+=3)
            //{
            //    if (OutputData[i] != OutputData[i + 1] || OutputData[i] != OutputData[i + 2])
            //    {
            //        Console.WriteLine("Stop");

            //    }


            //}



        }

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="LedStrip"/> class.
        /// </summary>
        public LedStrip()
        {
            Layers = new MatrixDictionaryBase<RGBAColor>();


        }
        #endregion

    }
}
