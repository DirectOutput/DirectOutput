using System;

namespace DirectOutput.Table
{
    /// <summary>
    /// Data representing the state of a table emlement
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

        public TableElementData Clone()
        {
            return new TableElementData(TableElementType, Number, Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableElementData"/> class.
        /// </summary>
        public TableElementData() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableElementData"/> class.
        /// </summary>
        /// <param name="TableElementType">Type of the table element as defined in TableElementTypeEnum.</param>
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
        /// <param name="TableElementTypeChar">Single character specifing the type of the table element. Valid values are the enum values in TableElementTypeEnum.</param>
        /// <param name="Number">The number of the table element.</param>
        /// <param name="Value">The value of the table element.</param>
        public TableElementData(Char TableElementTypeChar, int Number, int Value)
        {

            if (!Enum.IsDefined(typeof(TableElementTypeEnum), (int)TableElementTypeChar))
            {
                Log.Warning("Undefined char \"{0}\" supplied for the TableElementTypeChar.".Build(TableElementTypeChar));
                this.TableElementType = TableElementTypeEnum.Unknown;
            }
            else
            {
                this.TableElementType = (TableElementTypeEnum)TableElementTypeChar;
            }
            this.Number = Number;
            this.Value = Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableElementData"/> class from the data in a TableElement.
        /// </summary>
        /// <param name="TableElement">The table element containg the data.</param>
        public TableElementData(TableElement TableElement)
        {
            this.Number = TableElement.Number;
            this.TableElementType = TableElement.TableElementType;
            this.Value = TableElement.Value;
        }

    }
}
