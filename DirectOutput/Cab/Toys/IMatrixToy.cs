using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.Cab.Toys
{
    /// <summary>
    /// Interface for toys having a matrix of elements (e.g. ledstrips)
    /// </summary>
    /// <typeparam name="MatrixElementType">The type of the matrix element type.</typeparam>
    public interface IMatrixToy<MatrixElementType>:IToy
    {
        /// <summary>
        /// Gets the specified layer of MatrixElementTypes of the toy.
        /// </summary>
        /// <param name="LayerNr">The layer nr.</param>
        /// <returns></returns>
        MatrixElementType[,] GetLayer(int LayerNr);

        /// <summary>
        /// Gets the height resp. the y dimension of the toys matrix.
        /// </summary>
        /// <value>
        /// The height resp. the y dimension of the toys matrix.
        /// </value>
        int Height { get; }

        /// <summary>
        /// Gets the width resp. the x dimension of the toys matrix.
        /// </summary>
        /// <value>
        /// The width resp. the x dimension of the toys matrix.
        /// </value>
        int Width { get; }
    }
}

