using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.Cab.Toys.Virtual
{
    /// <summary>
    /// This toys allows the grouping of several RGBA toys (e.g. <see cref="RGBAToy"/> or <see cref="RGBLed"/>) into a matrix, which can be controlled by the RGBAMatrix effects.
    /// 
    /// \note: Be sure to define this toy in the config file before the toys, which are listed in the RGBAToyNames array.
    /// </summary>
    public class RGBAToyGroup : ToyBaseUpdatable, IToy, IRGBAMatrix
    {
        private List<List<string>> _RGBAToyNames = new List<List<string>>();

        /// <summary>
        /// Gets or sets the 2-dimensional array of rgba toy names.
        /// </summary>
        /// <value>
        /// The 2 dimensional array of RGBA toy names.
        /// </value>
        [XmlArrayItem("Row")]
        [XmlArrayItem("Column",NestingLevel=1)]
        public List<List<string>> RGBAToyNames
        {
            get { return _RGBAToyNames; }
            set { _RGBAToyNames = value; }
        }

        [XmlIgnore]
        private IRGBAToy[,] RGBAToys = new IRGBAToy[0, 0];

        private int _LayerOffset;

        /// <summary>
        /// Gets or sets the layer offset.<br/>
        /// The layer offset defines a fixed positive or negative offset to the layers which are controlled on the target toy.
        /// </summary>
        /// <value>
        /// The layer offset which defines a fixed positive or negative offset to the layers which are controlled on the target toy..
        /// </value>
        public int LayerOffset
        {
            get { return _LayerOffset; }
            set { _LayerOffset = value; }
        }


        /// <summary>
        /// Updates the toys specified in the toy names array with the values from the layers of this toy.
        /// </summary>
        public override void UpdateOutputs()
        {
            foreach (KeyValuePair<int, RGBAData[,]> Layer in Layers)
            {
                int LayerNr = Layer.Key+LayerOffset;

                for (int y = 0; y < RGBAToys.GetUpperBound(1) + 1; y++)
                {
                    for (int x = 0; x < RGBAToys.GetUpperBound(0) + 1; x++)
                    {
                        if (RGBAToys[x, y] != null)
                        {
                            RGBAToys[x, y].Layers[LayerNr].Set(Layer.Value[x, y]);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Initializes the toy.
        /// </summary>
        /// <param name="Cabinet"><see cref="Cabinet" /> object  to which the <see cref="IToy" /> belongs.</param>
        public override void Init(Cabinet Cabinet)
        {
            int RowCnt = RGBAToyNames.Count.Limit(1,int.MaxValue);
            int ColCnt = RGBAToyNames.Max(X => X.Count).Limit(1, int.MaxValue);


            RGBAToys = new IRGBAToy[ColCnt, RowCnt];

            for (int y = 0; y < RowCnt ; y++)
            {
                for (int x = 0; x < ColCnt ; x++)
                {
                    if (RGBAToyNames[y].Count > x)
                    {
                        if (Cabinet.Toys.Contains(RGBAToyNames[y][x]) && Cabinet.Toys[RGBAToyNames[y][x]] is IRGBAToy)
                        {
                            RGBAToys[x, y] = (IRGBAToy)Cabinet.Toys[RGBAToyNames[y][x]];
                        }
                        else
                        {
                            RGBAToys[x, y] = null;
                        }
                    }
                }
            }

            Layers = new RGBAMatrixDictionary() { Width = Width, Height = Height };

        }

        /// <summary>
        /// Resets the state of the IToy to its default state.
        /// </summary>
        public override void Reset()
        {
            Layers = new RGBAMatrixDictionary() { Width = Width, Height = Height };
        }

        /// <summary>
        /// Finished the toy, releases references to toys and layers.
        /// </summary>
        public override void Finish()
        {
            base.Finish();
            Layers = null;
            RGBAToys = null;
        }



        /// <summary>
        /// Gets the layers dictionary of the toy.
        /// </summary>
        /// <value>
        /// The layers dictionary of the toy.
        /// </value>
        [XmlIgnore]
        public RGBAMatrixDictionary Layers { get; private set; }


        #region IRGBAMatrix Member

        /// <summary>
        /// Gets the 2 dimensional RGBAData array for the specified layer.
        /// 
        /// Dimension 0 of the array represents the x resp. horizontal direction. Dimension 1 of the array repersent the y resp. vertical direction.
        /// Position 0,0 is the upper left corner of the ledarray.
        /// 
        /// If the specified layer does not exist, it will be created as a fully transparent layer where all positions are set to transparent black.
        /// </summary>
        /// <param name="LayerNr">The layer nr.</param>
        /// <returns>The RGBAData array for the specified layer.</returns>
        public RGBAData[,] GetLayer(int LayerNr)
        {
            return Layers[LayerNr];
        }

        /// <summary>
        /// Gets the height resp. the y dimension of the toys matrix.
        /// </summary>
        /// <value>
        /// The height resp. the y dimension of the toys matrix.
        /// </value>
        public int Height
        {
            get
            {
                return RGBAToys.GetUpperBound(1) + 1;
            }

        }

        /// <summary>
        /// Gets the width resp. the x dimension of the toys matrix.
        /// </summary>
        /// <value>
        /// The width resp. the x dimension of the toys matrix.
        /// </value>
        public int Width
        {
            get
            {
                return RGBAToys.GetUpperBound(0) + 1;
            }
         }

        #endregion
    }
}
