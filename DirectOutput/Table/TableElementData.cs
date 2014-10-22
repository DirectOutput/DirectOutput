using System;

namespace DirectOutput.Table
{
    /// <summary>
    /// Data representing the state of a table emlement
    /// </summary>
    public struct TableElementData
    {

        /// <summary>
        /// The type of the table element.
        /// </summary>
        public TableElementTypeEnum TableElementType;

        /// <summary>
        /// The number of the table element.
        /// </summary>
        public int Number;

        /// <summary>
        /// The value of the table element.
        /// </summary>
        public int Value;

        /// <summary>
        /// The name of the table element
        /// </summary>
        public string Name;



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
            this.Name = null;
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
            this.Name = null;
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
            this.Name = TableElement.Name;
            this.Value = TableElement.Value;
        }

        public TableElementData(string TableElementName, int Value)
        {
            if (TableElementName.Length > 1 && Enum.IsDefined(typeof(TableElementTypeEnum), (int)TableElementName[0]) && TableElementName[0]!=(char)TableElementTypeEnum.NamedElement && TableElementName.Substring(1).IsInteger())
            {
                //It is a normal table element. 
                this.TableElementType = (TableElementTypeEnum)TableElementName[0];
                this.Number = TableElementName.Substring(1).ToInteger();
                this.Name = null;
            }
            else
            {
                //Named table element
                this.TableElementType = TableElementTypeEnum.NamedElement;
                this.Name = (TableElementName[0] != (char)TableElementTypeEnum.NamedElement?TableElementName:TableElementName.Substring(1));
                this.Number = int.MinValue;
            }

           
            this.Value = Value;
        }

    }
}
