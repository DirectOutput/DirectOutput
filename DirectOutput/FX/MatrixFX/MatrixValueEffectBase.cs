using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.General.Color;
using DirectOutput.Cab.Toys.Layer;

namespace DirectOutput.FX.MatrixFX
{
    /// <summary>
    /// Base class for effects setting all elements a specified area of a matrix toy to the same specific value.
    /// </summary>
    public abstract class MatrixValueEffectBase<MatrixElementType> : MatrixEffectBase<MatrixElementType>
    {

        /// <summary>
        /// Triggers the effect with the given TableElementData.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (MatrixLayer != null)
            {

                int V = TableElementData.Value.Limit(0, 255);
                if (V > 0 && FadeMode == FadeModeEnum.OnOff) { V = 255; }

                MatrixElementType D = GetEffectValue(V); 


                for (int x = AreaLeft; x <= AreaRight; x++)
                {
                    for (int y = AreaTop; y <= AreaBottom; y++)
                    {
                        MatrixLayer[x, y] = D;
                    }
                }
            }
        }


        /// <summary>
        /// Gets the value which is to be applied to all elements of the matrix area controlled by the effect.
        /// This methed must be overwritten.
        /// </summary>
        /// <param name="TriggerValue">The trigger value.</param>
        /// <returns>Returns the value which is to be applied to all elements of the matrix area controlled by the effect.</returns>
        protected abstract MatrixElementType GetEffectValue(int TriggerValue);





    }
}
