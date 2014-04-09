using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Out;
using DirectOutput.General.Color;
using DirectOutput.General;

namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// Thie RGBAToy controls RGB leds and other gadgets displaying RGB colors.<br/><br/>
    /// The RGBAToy has multilayer support with alpha channels. This allows the effects targeting RGBAToys to send their data to different layers. 
    /// Values in a layer do also have a alpha/transparency channel which will allow us to blend the colors/values in the various layers (e.g. if  a bottom layer is blue and top is a semi transparent red, you will get some mix of both or if one of the two blinks you get changing colors).<br/>
    /// The following picture might give you a clearer idea how the layers with their alpha channels work:
    /// 
    /// \image html LayersRGBA.png "RGBA Layers"
    /// </summary>
    public class RGBAToy : ToyBaseUpdatable, IRGBOutputToy,IRGBAToy
    {

        #region Layers
        /// <summary>
        /// Gets the dictionary of RGBALayers.
        /// </summary>
        /// <value>
        /// The layers dictionary.
        /// </value>
        [System.Xml.Serialization.XmlIgnore]
        public LayerDictionary<RGBAColor> Layers { get; private set; }



        /// <summary>
        /// Get the RGBColor resulting from the colors and alpha values in the layers.
        /// </summary>
        /// <returns></returns>
        public  RGBColor GetResultingData()
        {
            if (Layers.Count > 0)
            {
                float Red = 0;
                float Green = 0;
                float Blue = 0;
                foreach (KeyValuePair<int, RGBAColor> KV in Layers)
                {
                    int Alpha = KV.Value.Alpha;
                    if (Alpha != 0)
                    {
                        int NegAlpha = 255 - Alpha;
                        Red = AlphaMappingTable.AlphaMapping[NegAlpha, (int)Red] + AlphaMappingTable.AlphaMapping[Alpha, KV.Value.Red];
                        Green = AlphaMappingTable.AlphaMapping[NegAlpha, (int)Green] + AlphaMappingTable.AlphaMapping[Alpha, KV.Value.Green];
                        Blue = AlphaMappingTable.AlphaMapping[NegAlpha, (int)Blue] + AlphaMappingTable.AlphaMapping[Alpha, KV.Value.Blue];
                    }
                }

                return new RGBColor((int)Red, (int)Green, (int)Blue);
            }
            else
            {
                return new RGBColor(0, 0, 0);
            }
        }



        #endregion



        #region Outputs
        private IOutput _OutputRed;

        /// <summary>
        /// Name of the IOutput for red.
        /// </summary>
        public string OutputNameRed { get; set; }

        private IOutput _OutputGreen;

        /// <summary>
        /// Name of the IOutput for green.
        /// </summary>
        public string OutputNameGreen { get; set; }

        private IOutput _OutputBlue;

        /// <summary>
        /// Name of the IOutput for blue.
        /// </summary>
        public string OutputNameBlue { get; set; }

        #endregion


        #region Fading curve
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
        #endregion



        private Cabinet _Cabinet;
        #region Init
        /// <summary>
        /// Initializes the RGBALed toy.
        /// </summary>
        /// <param name="Cabinet"><see cref="Cabinet"/> object to which the <see cref="RGBAToy"/> belongs.</param>
        public override void Init(Cabinet Cabinet)
        {
            _Cabinet = Cabinet;
            InitFadingCurve(Cabinet);
            InitOutputs(Cabinet);
        }

        private void InitOutputs(Cabinet Cabinet)
        {
            if (Cabinet.Outputs.Contains(OutputNameRed))
            {
                _OutputRed = Cabinet.Outputs[OutputNameRed];
            }
            else
            {
                _OutputRed = null;
            }
            if (Cabinet.Outputs.Contains(OutputNameGreen))
            {
                _OutputGreen = Cabinet.Outputs[OutputNameGreen];
            }
            else
            {
                _OutputGreen = null;
            }
            if (Cabinet.Outputs.Contains(OutputNameBlue))
            {
                _OutputBlue = Cabinet.Outputs[OutputNameBlue];
            }
            else
            {
                _OutputBlue = null;
            }
        }
        #endregion

        #region Finish

        /// <summary>
        /// Finishes the RGBALed toy.<br/>
        /// Resets the the toy and releases all references.
        /// </summary>
        public override void Finish()
        {
            Reset();
            _Cabinet = null;
            _OutputRed = null;
            _OutputGreen = null;
            _OutputBlue = null;
        }
        #endregion




        /// <summary>
        /// Updates the outputs of the RGBAToy.
        /// </summary>
        public override void UpdateOutputs()
        {
            RGBColor RGB = GetResultingData();

            if (_OutputRed != null)
            {
                _OutputRed.Value = FadingCurve.MapValue(RGB.Red);
            }
            if (_OutputGreen != null)
            {
                _OutputGreen.Value =  FadingCurve.MapValue(RGB.Green);
            }
            if (_OutputBlue != null)
            {
                _OutputBlue.Value =  FadingCurve.MapValue(RGB.Blue);
            }
        }


        /// <summary>
        /// Clears all layers and sets all outputs to 0 (off).
        /// </summary>
        public override void Reset()
        {
            Layers.Clear();
            _OutputRed.Value = 0;
            _OutputGreen.Value = 0;
            _OutputBlue.Value = 0;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RGBAToy"/> class.
        /// </summary>
        public RGBAToy()
        {
            Layers = new LayerDictionary<RGBAColor>();
        }





    }
}
