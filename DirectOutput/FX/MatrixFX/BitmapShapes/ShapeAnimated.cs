using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.BitmapHandling;

namespace DirectOutput.FX.MatrixFX.BitmapShapes
{
    public class ShapeAnimated : Shape
    {
        private MatrixAnimationStepDirectionEnum _AnimationStepDirection = MatrixAnimationStepDirectionEnum.Frame;

        /// <summary>
        /// Gets or sets the animation direction.
        /// </summary>
        /// <value>
        /// The direction in which the effect will step formward through the source image to get the next frame of the animation. 
        /// </value>
        public MatrixAnimationStepDirectionEnum AnimationStepDirection
        {
            get { return _AnimationStepDirection; }
            set { _AnimationStepDirection = value; }
        }

        private int _AnimationStepSize = 1;

        /// <summary>
        /// Gets or sets the size of the step in pixels or frames (depending on the \ref AnimationStepDirection) to the next frame of the animation.
        /// </summary>
        /// <value>
        /// The size of the step in pixels or frames (depending on the \ref AnimationStepDirection) to the next frame of the animation.
        /// </value>
        public int AnimationStepSize
        {
            get { return _AnimationStepSize; }
            set { _AnimationStepSize = value; }
        }


        private int _AnimationFrameCount = 1;

        /// <summary>
        /// Gets or sets the number of frames for the whole animation.
        /// </summary>
        /// <value>
        /// The number of frames for the whole animation.
        /// </value>
        public int AnimationFrameCount
        {
            get { return _AnimationFrameCount; }
            set { _AnimationFrameCount = value.Limit(1, int.MaxValue); }
        }

        private AnimationBehaviourEnum _AnimationBehaviour = AnimationBehaviourEnum.Loop;

        /// <summary>
        /// Gets or sets the animation behaviour.
        /// </summary>
        /// <value>
        /// The animation behaviour defines if a animation should run only once, run in a loop or continue at its last position when triggered.
        /// </value>
        public AnimationBehaviourEnum AnimationBehaviour
        {
            get { return _AnimationBehaviour; }
            set { _AnimationBehaviour = value; }
        }


        private int _AnimationFrameDurationMs = 30;

        /// <summary>
        /// Gets or sets the animation frame duration in ms.
        /// </summary>
        /// <value>
        /// The animation frame duration in miliseconds. Defaults to 30ms if not set.
        /// </value>
        public int AnimationFrameDurationMs
        {
            get { return _AnimationFrameDurationMs; }
            set { _AnimationFrameDurationMs = value.Limit(1, int.MaxValue); }
        }





    }
}
