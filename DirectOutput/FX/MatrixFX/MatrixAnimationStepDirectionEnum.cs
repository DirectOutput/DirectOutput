using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.FX.MatrixFX
{
    public enum MatrixAnimationStepDirectionEnum
    {

        /// <summary>
        /// Animation steps though frames of the source image (this mainly for animated gifs).
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
