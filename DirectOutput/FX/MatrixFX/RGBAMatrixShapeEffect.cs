using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.FX.MatrixFX.BitmapShapes;
using DirectOutput.General.Color;

namespace DirectOutput.FX.MatrixFX
{
    public class RGBAMatrixShapeEffect : MatrixEffectBase<RGBAColor>
    {
        private string _ShapeName;

        /// <summary>
        /// Gets or sets the name of the shape.
        /// </summary>
        /// <value>
        /// The name of the shape.
        /// </value>
        public string ShapeName
        {
            get { return _ShapeName; }
            set { _ShapeName = value; }
        }



        private IMatrixBitmapEffect TargetEffect = null;






        public override void Init(Table.Table Table)
        {
            base.Init(Table);

            Shape Def = Table.ShapeDefinitions.Shapes.FirstOrDefault(SH => SH.Name.Equals(ShapeName, StringComparison.InvariantCultureIgnoreCase));

            if (Def != null)
            {
                IMatrixBitmapEffect FX;
                if (Def.GetType() == typeof(ShapeAnimated))
                {
                    ShapeAnimated DefAnim = (ShapeAnimated)Def;

                    FX = new RGBAMatrixBitmapAnimationEffect();

                    RGBAMatrixBitmapAnimationEffect FXA = (RGBAMatrixBitmapAnimationEffect)FX;

                    FXA.AnimationBehaviour = DefAnim.AnimationBehaviour;
                    FXA.AnimationFrameCount = DefAnim.AnimationFrameCount;
                    FXA.AnimationFrameDurationMs = DefAnim.AnimationFrameDurationMs;
                    FXA.AnimationStepDirection = DefAnim.AnimationStepDirection;
                    FXA.AnimationStepSize = DefAnim.AnimationStepSize;


                }
                else
                {
                    FX = new RGBAMatrixBitmapEffect();
                }

                FX.BitmapFilePattern = Table.ShapeDefinitions.BitmapFilePattern;
                FX.BitmapFrameNumber = Def.BitmapFrameNumber;
                FX.BitmapHeight = Def.BitmapHeight;
                FX.BitmapWidth = Def.BitmapWidth;
                FX.BitmapTop = Def.BitmapTop;
                FX.BitmapLeft = Def.BitmapLeft;
                FX.DataExtractMode = Def.DataExtractMode;
                FX.ToyName = this.ToyName;
                FX.LayerNr = this.LayerNr;
                FX.FadeMode = this.FadeMode;
                FX.Left = this.Left;
                FX.Top = this.Top;
                FX.Width = this.Width;
                FX.Height = this.Height;
                
                FX.Name = this.Name + " Target";

                TargetEffect = FX;

                TargetEffect.Init(Table);
            }
            else
            {
                //Shape does not exist
                TargetEffect = null;
            }
        }

        public override void Finish()
        {
            base.Finish();

            if (TargetEffect != null)
            {
                TargetEffect.Finish();
                TargetEffect = null;
            }
        }

        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (TargetEffect != null)
            {
                TargetEffect.Trigger(TableElementData);
            }
        }


    }
}
