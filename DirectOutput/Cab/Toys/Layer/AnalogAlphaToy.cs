using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Out;
using DirectOutput.General;
using System.Xml.Serialization;
using DirectOutput.General.Analog;

namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// This toy handles analog values (0-255) in a layer structure including alpha value (0=completely transparent, 255=fully opaque) and outputs the belended result of the layers on a single output.
    /// </summary>
    public class AnalogAlphaToy : ToyBaseUpdatable, IAnalogAlphaToy, ISingleOutputToy
    {

        /// <summary>
        /// Gets the layers dictionary.
        /// </summary>
        /// <value>
        /// The layers dictionary.
        /// </value>
        [XmlIgnore]
        public LayerDictionary<AnalogAlpha> Layers { get; private set; }

        /// <summary>
        /// Gets the analog value which results from the analog values and alpha values in the dirctionary.
        /// </summary>
        /// <returns>A analog value.</returns>
        protected int GetResultingValue()
        {
            if (Layers.Count > 0)
            {
                float Value = 0;

                foreach (KeyValuePair<int, AnalogAlpha> KV in Layers)
                {
                    int Alpha = KV.Value.Alpha;
                    if (Alpha != 0)
                    {
                        Value = AlphaMappingTable.AlphaMapping[255 - Alpha, (int)Value] + AlphaMappingTable.AlphaMapping[Alpha, KV.Value.Value];
                    }
                }

                return (int)Value;
            }
            else
            {
                return 0;
            }
        }


        #region Outputs

        [XmlIgnore]
        protected IOutput Output;


        /// <summary>
        /// Gets or sets the name of the IOutput object of the toy.
        /// </summary>
        /// <value>
        /// The name of the output.
        /// </value>
        public string OutputName { get; set; }


        #endregion

        #region Fading curve
        private string _FadingCurveName = "Linear";
        [XmlIgnore]
        protected Curve FadingCurve = null;

        /// <summary>
        /// Gets or sets the name of the fading curve as defined in the Curves list of the cabinet object.
        /// This curve can be used to adjust the output values of the toy to fit the behaviour of the toys hardware and/or human perception.
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



        [XmlIgnore]
        protected Cabinet Cabinet;


        /// <summary>
        /// Initializes the toy.
        /// </summary>
        /// <param name="Cabinet"><see cref="Cabinet" /> object  to which the <see cref="IToy" /> belongs.</param>
        public override void Init(Cabinet Cabinet)
        {
            this.Cabinet = Cabinet;
            InitOutputs(Cabinet);
            InitFadingCurve(Cabinet);
        }

        private void InitOutputs(Cabinet Cabinet)
        {
            if (Cabinet.Outputs.Contains(OutputName))
            {
                Output = Cabinet.Outputs[OutputName];
            }
            else
            {
                Output = null;
            }
        }




        /// <summary>
        /// Updates the output of the toy.
        /// </summary>
        public override void UpdateOutputs()
        {
            if (Output != null)
            {
       
                Output.Value = FadingCurve.MapValue(GetResultingValue());
            }
        }


        /// <summary>
        /// Resets the toy and releases all references
        /// </summary>
        public override void Finish()
        {
            Reset();
            Output = null;
            Cabinet = null;
            base.Finish();
        }

        /// <summary>
        /// Resets the toy.<br/>
        /// Clears the Layers object and turn off the output (if available).
        /// Method must be overwritten.
        /// </summary>
        public override void Reset()
        {
            Layers.Clear();
            if (Output != null)
            {
                Output.Value = 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogAlphaToy"/> class.
        /// </summary>
        public AnalogAlphaToy()
        {
            Layers = new LayerDictionary<AnalogAlpha>();
        }
    }
}
