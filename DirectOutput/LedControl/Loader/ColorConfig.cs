using System;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Color;


namespace DirectOutput.LedControl.Loader
{

    /// <summary>
    /// Color configuration from a LedControl file.
    /// </summary>
    public class ColorConfig
    {
        /// <summary>
        /// Gets a cabinet RGBAColor object representing the values in the ColorConfig object-.
        /// </summary>
        /// <returns>RGBAColorNamed object for the content of the ColorConfig.</returns>
        public RGBAColorNamed GetCabinetColor()
        {
            return new RGBAColorNamed(Name,Red, Green,Blue,Alpha);
        }

        /// <summary>
        /// Gets or sets the name of the color.
        /// </summary>
        /// <value>
        /// The name of the color.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the red part of the color.
        /// </summary>
        /// <value>
        /// The red value.
        /// </value>
        public int Red { get; set; }
        /// <summary>
        /// Gets or sets the green part of the color.
        /// </summary>
        /// <value>
        /// The green part of the color.
        /// </value>
        public int Green { get; set; }
        /// <summary>
        /// Gets or sets the blue part of the color.
        /// </summary>
        /// <value>
        /// The blue part of the color.
        /// </value>
        public int Blue { get; set; }



        private int _Alpha;

        /// <summary>
        /// Gets or sets the alpha part of the color.
        /// </summary>
        /// <value>
        /// The alpha part of the color.
        /// </value>
        public int Alpha
        {
            get { return _Alpha; }
            set { _Alpha = value; }
        }
        
        /// <summary>
        /// Parses the ledcontrol data for a color definition.
        /// </summary>
        /// <param name="ColorConfigDataLine">The color config data line.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> [throw exceptions].</param>
        /// <exception cref="System.Exception">Line {0} has a unknown structure or contains wrong data.</exception>
        public void ParseLedcontrolData(string ColorConfigDataLine, bool ThrowExceptions = false)
        {
            //Black=5,5,5

            //Split Name and Value portion of line apart.
            string[] NameValues = ColorConfigDataLine.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
            if (NameValues.Length == 2)
            {
                string[] Values = NameValues[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (Values.Length == 3 && Values[0].IsInteger() && Values[1].IsInteger() && Values[2].IsInteger())
                {
                    Name = NameValues[0];
                    Red = (int)(Values[0].ToInteger().Limit(0, 48) * 5.3125);
                    Green = (int)(Values[1].ToInteger().Limit(0, 48) * 5.3125);
                    Blue = (int)(Values[2].ToInteger().Limit(0, 48) * 5.3125);
                    Alpha = (Red + Green + Blue == 0 ? 0 : 255);
                    return;
                }
                else if (Values.Length == 4 && Values[0].IsInteger() && Values[1].IsInteger() && Values[2].IsInteger() && Values[3].IsInteger())
                {
                    Name = NameValues[0];
                    Red = (int)(Values[0].ToInteger().Limit(0, 48)* 5.3125);
                    Green = (int)(Values[1].ToInteger().Limit(0, 48)* 5.3125);
                    Blue = (int)(Values[2].ToInteger().Limit(0, 48)* 5.3125);
                    Alpha = (int)(Values[2].ToInteger().Limit(0, 48) * 5.3125);
                    return;

                }
                else if (NameValues[1].StartsWith("#") && (NameValues[1].Length==7 || NameValues[1].Length==9) && NameValues[1].Substring(1).IsHexString())
                {
                    RGBAColor C = new RGBAColor();
                    if(C.SetColor(NameValues[1])) {
                        Red=C.Red;
                        Green=C.Green;
                        Blue=C.Blue;
                        Alpha=C.Alpha;
                        return;
                    } 
                    C=null;

                }
            }
            Log.Warning("Line {0} has a unknown structure or contains wrong data.".Build(ColorConfigDataLine));
            if (ThrowExceptions)
            {
                throw new Exception("Line {0} has a unknown structure or contains wrong data.".Build(ColorConfigDataLine));
            }
            return;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ColorConfig"/> class.
        /// </summary>
        public ColorConfig() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorConfig"/> class.
        /// Parses the ledcontrol data for a color definition.
        /// </summary>
        /// <param name="ColorConfigDataLine">The color config data line.</param>
        /// <param name="ThrowExceptions">If set to <c>true</c> [throw exceptions].</param>
        /// <exception cref="System.Exception">Line {0} has a unknown structure or contains wrong data.</exception>       
        public ColorConfig(string ColorConfigDataLine, bool ThrowExceptions = false)
        {
            ParseLedcontrolData(ColorConfigDataLine, ThrowExceptions);

        }
    }
}
