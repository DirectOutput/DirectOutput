using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.RGBAMatrixFX
{
    public enum RGBAMatrixAnimationDirection
    {

        /// <summary>
        /// Animation steps though frames of the source image.
        /// </summary>
        Frame='F',
        /// <summary>
        /// Animation steps from left to right through the source image
        /// </summary>
        Right='R',
        /// <summary>
        /// Animation steps from top to bottom through the source image
        /// </summary>
        Down='D'
    }
}
