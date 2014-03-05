using System;
namespace DirectOutput.Cab.Toys.Layer
{
    /// <summary>
    /// Common interface for toys controlling a matrix of RGB components (e.g. led strips)
    /// </summary>
    public interface IRGBAMatrix:IToy
    {
        /// <summary>
        /// Gets the specified layer of RGBA data of the toy.
        /// </summary>
        /// <param name="LayerNr">The layer nr.</param>
        /// <returns></returns>
        RGBAData[,] GetLayer(int LayerNr);

        /// <summary>
        /// Gets the height resp. the y dimension of the toys matrix.
        /// </summary>
        /// <value>
        /// The height resp. the y dimension of the toys matrix.
        /// </value>
        int Height { get;  }

        /// <summary>
        /// Gets the width resp. the x dimension of the toys matrix.
        /// </summary>
        /// <value>
        /// The width resp. the x dimension of the toys matrix.
        /// </value>
        int Width { get;  }
    }
}
