using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.FX.MatrixFX.BitmapShapes;
using DirectOutput.General.BitmapHandling;
using DirectOutput.FX.MatrixFX;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            ShapeDefinitions D = new ShapeDefinitions();

            D.BitmapFilePattern = new DirectOutput.General.FilePattern("{DllDir}\\shapes.png");

            Shape S = new Shape() { Name = "Round", BitmapTop = 10, BitmapLeft = 20, BitmapHeight = 50, BitmapWidth = 15, BitmapFrameNumber = 0, DataExtractMode = FastBitmapDataExtractModeEnum.BlendPixels };
            D.Shapes.Add(S);

            S = new ShapeAnimated() { Name = "Pulse", BitmapTop = 10, BitmapLeft = 20, BitmapHeight = 50, BitmapWidth = 15, BitmapFrameNumber = 0, DataExtractMode = FastBitmapDataExtractModeEnum.BlendPixels, AnimationBehaviour = AnimationBehaviourEnum.Loop, AnimationFrameCount = 15, AnimationStepDirection = MatrixAnimationStepDirectionEnum.Right, AnimationFrameDurationMs = 30, AnimationStepSize = 15 };
            D.Shapes.Add(S);

            D.SaveShapeDefinitionsXmlFile(@"C:\Users\tom\Documents\Github\DirectOutput\DirectOutput\DirectOutputShapes.xml");

        }
    }
}
