using DirectOutput.Cab.Toys.Layer;
using System.Xml.Serialization;
using DirectOutput.Cab.Toys;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// Base class for effects targeting a matrix of toys (e.g. addressable ledstrip)
    /// </summary>
    public abstract class MatrixEffectBase<MatrixElementType> : EffectBase, DirectOutput.FX.MatrixFX.IMatrixEffect
    {
        #region Config properties

        private string _ToyName;

        /// <summary>
        /// Gets or sets the name of the toy which is to be controlled by the effect.
        /// </summary>
        /// <value>
        /// The name of the toy which is controlled by the effect.
        /// </value>
        public string ToyName
        {
            get { return _ToyName; }
            set
            {
                if (_ToyName != value)
                {
                    _ToyName = value;
                    Matrix = null;
                    MatrixLayer = null;
                }
            }
        }


        /// <summary>
        /// Gets the matrix toy which is referenced by the ToyName property.
        /// This property is initialized by the Init method.
        /// </summary>
        /// <value>
        /// The matrix toy which is referenced by the ToyName property.
        /// </value>
        protected IMatrixToy<MatrixElementType> Matrix { get; private set; }




        private float _Width = 100;

        /// <summary>
        /// Gets or sets the width in percent of target area of the ledstrip which is controlled by the effect.
        /// </summary>
        /// <value>
        /// The width in percent of the target area for the effect (0-100).
        /// </value>
        public float Width
        {
            get { return _Width; }
            set { _Width = value.Limit(0, 100); }
        }

        private float _Height = 100;

        /// <summary>
        /// Gets or sets the height in percent of target area of the matrix which is controlled by the effect.
        /// </summary>
        /// <value>
        /// The height in percent of the target area for the effect (0-100).
        /// </value>
        public float Height
        {
            get { return _Height; }
            set { _Height = value.Limit(0, 100); }
        }

        private float _Left = 0;

        /// <summary>
        /// Gets or sets the left resp. X positon of the upper left corner in percent of the target area of the matrix which is controlled by the effect.
        /// </summary>
        /// <value>
        /// The left resp. X position of the upper left corner in percent of the target area for the effect (0-100).
        /// </value>
        public float Left
        {
            get { return _Left; }
            set { _Left = value.Limit(0, 100); }
        }

        private float _Top = 0;

        /// <summary>
        /// Gets or sets the top resp. Y positon of the upper left corner in percent of the target area of the ledstrip which is controlled by the effect.
        /// </summary>
        /// <value>
        /// The top resp. Y position of the upper left corner in percent of the target area for the effect (0-100).
        /// </value>
        public float Top
        {
            get { return _Top; }
            set { _Top = value.Limit(0, 100); }
        }




        private int _LayerNr = 0;

        /// <summary>
        /// Gets or sets the number of the layer which is targeted by the effect.
        /// </summary>
        /// <value>
        /// The number of the target layer for the effect.
        /// </value>
        public int LayerNr
        {
            get { return _LayerNr; }
            set { _LayerNr = value; }
        }


        private FadeModeEnum _FadeMode = FadeModeEnum.Fade;

        /// <summary>
        /// Gets or sets the fade mode.
        /// </summary>
        /// <value>
        /// Fade (active and inactive values/color will fade depending on trigger value) or OnOff (actvice value/color is used for trigger values >0, otherwise inactive value/color will be used).
        /// </value>
        public FadeModeEnum FadeMode
        {
            get { return _FadeMode; }
            set { _FadeMode = value; }
        }


        #endregion

        /// <summary>
        /// The gets the X position of the led in the upper left corner of the effect area.
        /// </summary>
        [XmlIgnoreAttribute]
        protected int AreaLeft = 0;
        /// <summary>
        /// The gets the Y position of the led in the upper left corner of the effect area.
        /// </summary>
        [XmlIgnoreAttribute]
        protected int AreaTop = 0;
        /// <summary>
        /// The gets the X position of the led in the lower right corner of the effect area.
        /// </summary>
        [XmlIgnoreAttribute]
        protected int AreaRight = 0;
        /// <summary>
        /// The gets the Y position of the led in the lower right corner of the effect area.
        /// </summary>
        [XmlIgnoreAttribute]
        protected int AreaBottom = 0;

        /// <summary>
        /// Gets the number of leds on horizontal direction of the area for the effect.
        /// </summary>
        /// <value>
        /// The number of leds on horizontal direction of the area for the effect.
        /// </value>
        [XmlIgnoreAttribute]
        protected int AreaWidth
        {
            get { return (AreaRight - AreaLeft) + 1; }
        }

        /// <summary>
        /// Gets the number of leds on vertical direction of the area for the effect.
        /// </summary>
        /// <value>
        /// The number of leds on vertical direction of the area for the effect.
        /// </value>
        [XmlIgnoreAttribute]
        protected int AreaHeight
        {
            get { return (AreaBottom - AreaTop) + 1; }
        }

        /// <summary>
        /// Gets the table object which was specified during initialisation of the effect.
        /// </summary>
        /// <value>
        /// The table object which was specified during initialisation of the effect..
        /// </value>
        [XmlIgnoreAttribute]
        protected Table.Table Table { get; private set; }




        /// <summary>
        /// The layer array of a IRGBAMatrix object as specified by the ToyName and the LayerNr.
        /// This reference is initialized by the Init method.
        /// </summary>
        /// <value>
        /// A IRGBAMatrix object layer array.
        /// </value>
        [XmlIgnoreAttribute]
        protected MatrixElementType[,] MatrixLayer;


        /// <summary>
        /// Initializes the effect.
        /// Resolves object references.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public override void Init(Table.Table Table)
        {
            if (!ToyName.IsNullOrWhiteSpace() && Table.Pinball.Cabinet.Toys.Contains(ToyName) && Table.Pinball.Cabinet.Toys[ToyName] is IMatrixToy<MatrixElementType>)
            {
                Matrix = (IMatrixToy<MatrixElementType>)Table.Pinball.Cabinet.Toys[ToyName];
                MatrixLayer = Matrix.GetLayer(LayerNr);

                AreaLeft = (int)((float)Matrix.Width / 100 * Left).Floor().Limit(0, Matrix.Width - 1);
                AreaTop = (int)((float)Matrix.Height / 100 * Top).Floor().Limit(0, Matrix.Height - 1);
                AreaRight = (int)((float)Matrix.Width / 100 * (Left + Width).Limit(0, 100)).Floor().Limit(0, Matrix.Width - 1);
                AreaBottom = (int)((float)Matrix.Height / 100 * (Top + Height).Limit(0, 100)).Floor().Limit(0, Matrix.Height - 1);

                int Tmp;
                if (AreaLeft > AreaRight) { Tmp = AreaRight; AreaRight = AreaLeft; AreaLeft = AreaRight; }
                if (AreaTop > AreaBottom) { Tmp = AreaBottom; AreaBottom = AreaTop; AreaTop = Tmp; }

                Log.Instrumentation("MX", "MatrixBase for {12}. Calculated area size: AreaDef(L:{0}, T:{1}, W:{2}, H:{3}), Matrix(W:{4}, H:{5}), ResultArea(Left: {6}, Top:{7}, Right:{8}, Bottom:{9}, Width:{10}, Height:{11})".Build(new object[] { Left, Top, Width, Height, Matrix.Height, Matrix.Width, AreaLeft, AreaTop, AreaRight, AreaBottom, AreaWidth, AreaHeight, this.GetType().Name }));

            }

            this.Table = Table;
        }



        /// <summary>
        /// Finishes the effect and releases object references
        /// </summary>
        public override void Finish()
        {
            MatrixLayer = null;
            Matrix = null;
            Table = null;
            base.Finish();
        }
    }
}
