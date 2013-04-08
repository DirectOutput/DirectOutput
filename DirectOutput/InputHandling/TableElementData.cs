using System;

namespace DirectOutput.InputHandling
{
    /// <summary>
    /// Data received from Pinmame
    /// </summary>
    public class TableElementData
    {
        /// <summary>
        /// Gets or sets the type of the table element.
        /// </summary>
        /// <value>
        /// The type of the table element.
        /// </value>
        public TableElementTypeEnum TableElementType { get; set; }
        /// <summary>
        /// Gets or sets the number of the table element.
        /// </summary>
        /// <value>
        /// The number of the table element.
        /// </value>
        public int Number { get; set; }
        /// <summary>
        /// Gets or sets the value of the table element.
        /// </summary>
        /// <value>
        /// The value of the table element.
        /// </value>
        public int Value { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="TableElementData"/> class.
        /// </summary>
        public TableElementData() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableElementData"/> class.
        /// </summary>
        /// <param name="TableElementType">Type of the table element.</param>
        /// <param name="Number">The number of the table element.</param>
        /// <param name="Value">The value of the table element.</param>
        public TableElementData(TableElementTypeEnum TableElementType, int Number, int Value)
        {
            this.TableElementType = TableElementType;
            this.Number = Number;
            this.Value = Value;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TableElementData"/> class.
        /// </summary>
        /// <param name="TableElementTypeChar">Single character specifing the type of the table element. Valid values are L (Lamp), S (Solenoid), W (Switch), M (Mech), G (GI).</param>
        /// <param name="Number">The number of the table element.</param>
        /// <param name="Value">The value of the table element.</param>
        public TableElementData(Char TableElementTypeChar, int Number, int Value)
        {

            if(!Enum.IsDefined(typeof(TableElementTypeEnum),(int)TableElementTypeChar)) {
                throw new Exception("Undefined char \"{0}\" supplied for the TableElementTypeChar.".Build(TableElementTypeChar));
            }
            this.TableElementType = (TableElementTypeEnum)TableElementTypeChar;
            this.Number = Number;
            this.Value = Value;
        }



    }
}
