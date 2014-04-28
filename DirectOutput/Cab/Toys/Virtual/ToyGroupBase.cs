using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.Cab.Toys.Virtual
{
    /// <summary>
    /// This toys allows the grouping of several toys  into a matrix, which can be controlled by matrix effects.
    /// 
    /// \note Be sure to define this toy in the cabinet config file before the toys, which are listed in the ToyNames array.
    /// </summary>
    public abstract class ToyGroupBase<MatrixElementType> : ToyBaseUpdatable, IToy, IMatrixToy<MatrixElementType>
        where MatrixElementType:new()
    {
        private List<List<string>> _ToyNames = new List<List<string>>();

        /// <summary>
        /// Gets or sets the 2-dimensional array of rgba toy names.
        /// </summary>
        /// <value>
        /// The 2 dimensional array of RGBA toy names.
        /// </value>
        [XmlArrayItem("Row")]
        [XmlArrayItem("Column",NestingLevel=1)]
        public List<List<string>> ToyNames
        {
            get { return _ToyNames; }
            set { _ToyNames = value; }
        }

        [XmlIgnore]
        private ILayerToy<MatrixElementType>[,] Toys = new ILayerToy<MatrixElementType>[0, 0];

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
            foreach (KeyValuePair<int, MatrixElementType[,]> Layer in Layers)
            {
                int LayerNr = Layer.Key+LayerOffset;

                int Height = Toys.GetUpperBound(1) + 1;
                int  Width=Toys.GetUpperBound(0) + 1;

                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        if (Toys[x, y] != null)
                        {
                            Toys[x, y].Layers[LayerNr] = Layer.Value[x, y];
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
            int ColCnt = 0;
            int RowCnt = ToyNames.Count;
            if (RowCnt > 0)
            {
                ColCnt = ToyNames.Max(X => X.Count).Limit(1, int.MaxValue);
            }
            else
            {
                ToyNames.Add(new List<string>(new []{""}));
                RowCnt = 1;
                ColCnt = 1;
            }

            Toys = new ILayerToy<MatrixElementType>[ColCnt, RowCnt];

            for (int y = 0; y < RowCnt ; y++)
            {
                for (int x = 0; x < ColCnt ; x++)
                {
                    if (ToyNames[y].Count > x)
                    {
                        if (Cabinet.Toys.Contains(ToyNames[y][x]) && Cabinet.Toys[ToyNames[y][x]] is ILayerToy<MatrixElementType>)
                        {
                            Toys[x, y] = (ILayerToy<MatrixElementType>)Cabinet.Toys[ToyNames[y][x]];
                        }
                        else
                        {
                            Toys[x, y] = null;
                        }
                    }
                }
            }

            Layers = new MatrixDictionaryBase<MatrixElementType>() { Width = Width, Height = Height };

        }

        /// <summary>
        /// Resets the state of the IToy to its default state.
        /// </summary>
        public override void Reset()
        {
            Layers = new MatrixDictionaryBase<MatrixElementType>() { Width = Width, Height = Height };
        }

        /// <summary>
        /// Finished the toy, releases references to toys and layers.
        /// </summary>
        public override void Finish()
        {
            base.Finish();
            Layers = null;
            Toys = null;
        }



        /// <summary>
        /// Gets the layers dictionary of the toy.
        /// </summary>
        /// <value>
        /// The layers dictionary of the toy.
        /// </value>
        [XmlIgnore]
        public MatrixDictionaryBase<MatrixElementType> Layers { get; private set; }


        #region IMatrix Member

        /// <summary>
        /// Gets the 2 dimensional data array for the specified layer.
        /// 
        /// Dimension 0 of the array represents the x resp. horizontal direction. Dimension 1 of the array repersent the y resp. vertical direction.
        /// Position 0,0 is the upper left corner of the array.
        /// </summary>
        /// <param name="LayerNr">The layer nr.</param>
        /// <returns>The data array for the specified layer.</returns>
        public MatrixElementType[,] GetLayer(int LayerNr)
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
                return Toys.GetUpperBound(1) + 1;
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
                return Toys.GetUpperBound(0) + 1;
            }
         }

        #endregion
    }
}
