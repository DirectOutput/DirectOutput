using System;
using DirectOutput.Cab.Color;
namespace DirectOutput.Cab.Toys.Basic
{
    /// <summary>
    /// This interface is to be used for toys outputing RGB colors (e.g. RGBLed toy).<br/>
    /// </summary>
    public interface IRGBToy:IToy
    {
        /// <summary>
        /// Gets the brightness of the blue color component.
        /// </summary>
        /// <value>
        /// The brightness of the blue color component.
        /// </value>
        int Blue { get;  }
        /// <summary>
        /// Gets the brightness of the green color component.
        /// </summary>
        /// <value>
        /// The brightness of the green color component.
        /// </value>
        int Green { get;  }
        /// <summary>
        /// Gets the brightness of the red color component.
        /// </summary>
        /// <value>
        /// The brightness of the red color component.
        /// </value>
        int Red { get;  }

        /// <summary>
        /// Sets the color if the rgb toy.
        /// </summary>
        /// <param name="Color">The color for the rgb toy.</param>
        void SetColor(IRGBColor Color);
        /// <summary>
        /// Sets the color for the rgb toy by specifying the red, green and blue color components.
        /// </summary>
        /// <param name="Red">The red color component.</param>
        /// <param name="Green">The green component.</param>
        /// <param name="Blue">The blue component.</param>
        void SetColor(int Red, int Green, int Blue);

        /// <summary>
        /// Sets the color by setting a colordefinition in a string.<br/>
        /// </summary>
        /// <param name="Color">Hexadecimal color (e.g. \#ff0000 for red), comma separated color (e.g. 0,255,0 for green) or color name as defined in Cabinet.Colors.</param>
        void SetColor(string Color);
    }
}
