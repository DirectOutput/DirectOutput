using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.FX.MatrixFX.BitmapShapes;
using DirectOutput.General.Color;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// Displays a shape on a RGBA matrix (typically a ledstrip array). The color of the displayed shape is controlled by the effect.
    /// </summary>
    public class RGBAMatrixColorScaleShapeEffect : MatrixEffectBase<RGBAColor>
    {

        private RGBAColor _ActiveColor = new RGBAColor(0xff, 0xff, 0xff, 0xff);

        /// <summary>
        /// Gets or sets the active color.
        /// The FadeMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The active color.
        /// </value>
        public RGBAColor ActiveColor
        {
            get { return _ActiveColor; }
            set { _ActiveColor = value; }
        }

        private RGBAColor _InactiveColor = new RGBAColor(0, 0, 0, 0);

        /// <summary>
        /// Gets or sets the inactive color.
        /// The FadeMode property defines how this value is used.
        /// </summary>
        /// <value>
        /// The inactive color.
        /// </value>
        public RGBAColor InactiveColor
        {
            get { return _InactiveColor; }
            set { _InactiveColor = value; }
        }




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

                    FX = new RGBAMatrixColorScaleBitmapAnimationEffect();

                    RGBAMatrixColorScaleBitmapAnimationEffect FXA = (RGBAMatrixColorScaleBitmapAnimationEffect)FX;

                    FXA.AnimationBehaviour = DefAnim.AnimationBehaviour;
                    FXA.AnimationFrameCount = DefAnim.AnimationFrameCount;
                    FXA.AnimationFrameDurationMs = DefAnim.AnimationFrameDurationMs;
                    FXA.AnimationStepDirection = DefAnim.AnimationStepDirection;
                    FXA.AnimationStepSize = DefAnim.AnimationStepSize;

                    ((RGBAMatrixColorScaleBitmapAnimationEffect)FX).ActiveColor = this.ActiveColor;
                    ((RGBAMatrixColorScaleBitmapAnimationEffect)FX).InactiveColor = this.InactiveColor;


                }
                else
                {
                    FX = new RGBAMatrixColorScaleBitmapEffect();
                    ((RGBAMatrixColorScaleBitmapEffect)FX).ActiveColor = this.ActiveColor;
                    ((RGBAMatrixColorScaleBitmapEffect)FX).InactiveColor = this.InactiveColor;
                }
                
                FX.BitmapFilePattern = Table.ShapeDefinitions.BitmapFilePattern;
                FX.BitmapFrameNumber = Def.BitmapFrameNumber;
                FX.BitmapHeight = Def.BitmapHeight;
                FX.BitmapWidth = Def.BitmapWidth;
                FX.BitmapTop = Def.BitmapTop;
                FX.BitmapLeft = Def.BitmapLeft;
                FX.DataExtractMode = Def.DataExtractMode;
                FX.ToyName = this.ToyName;
              
                FX.FadeMode = this.FadeMode;
                FX.Name = this.Name + " Target";
                FX.LayerNr = this.LayerNr;
                FX.FadeMode = this.FadeMode;
                FX.Left = this.Left;
                FX.Top = this.Top;
                FX.Width = this.Width;
                FX.Height = this.Height;
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
