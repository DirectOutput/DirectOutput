using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// Sorted dictionary of layers for the matrix toys.
    /// </summary>
    public class MatrixDictionaryBase<MatrixElementType> : SortedDictionary<int, MatrixElementType[,]>
        where MatrixElementType:struct
    {

        /// <summary>
        /// Gets or sets the data array for the specified layernr.
        /// Dimension 0 of the array represents the x resp. horizontal direction. Dimension 1 of the array represents the y resp. vertical direction.
        /// Position 0,0 is the upper left corner of the array.
        /// </summary>
        /// <value>
        /// The data array for the specified layer.
        /// </value>
        /// <param name="LayerNr">The number of the layer.</param>
        public new MatrixElementType[,] this[int LayerNr]
        {
            get
            {
                try
                {
                    return base[LayerNr];
                }
                catch
                {
                    MatrixElementType[,] L = new MatrixElementType[Width, Height];

                    Add(LayerNr, L);
                    return L;
                }
            }
            //TODO: Check if set should be private
            set
            {
                if (value.GetUpperBound(0) == Width - 1 && value.GetUpperBound(1) == Height - 1)
                {
                    base[LayerNr] = value;
                }
                else
                {
                    throw new Exception("Supplied array has a illegal width ({0}) or height ({1}). Expecting {2} for width and {3} for height.".Build(new object[] {value.GetUpperBound(0)+1,value.GetUpperBound(1)+1,Width,Height}));
                }
            }
        }

        private int _Width=1;

        /// <summary>
        /// Gets or sets the width of the layers.
        /// </summary>
        /// <value>
        /// The width of the layers (Min.: 1).
        /// </value>
        public int Width
        {
            get { return _Width; }
            set { _Width = value.Limit(1,int.MaxValue); }
        }


        private int _Height=1;

        /// <summary>
        /// Gets or sets the height of the layers.
        /// </summary>
        /// <value>
        /// The height of the layers (Min.: 1).
        /// </value>
        public int Height
        {
            get { return _Height; }
            set { _Height = value.Limit(1,int.MaxValue); }
        }
        


    }
}
