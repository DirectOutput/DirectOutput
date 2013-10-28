using System;

namespace DirectOutput.Table
{
    /// <summary>
    /// EventArgs object for TableElementValueChanged events.
    /// </summary>
    public class TableElementValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the table element which has triggered the event.
        /// </summary>
        /// <value>
        /// The table element which has triggered the event.
        /// </value>
        public TableElement TableElement { get; set; }
        /// <summary>
        /// Gets the number of the table element which has triggered the event.
        /// </summary>
        /// <value>
        /// The number of the table element.
        /// </value>
        public int Number { get { return TableElement.Number; } }
        /// <summary>
        /// Gets the value of the table element which has triggered the event.
        /// </summary>
        /// <value>
        /// The value of the table element.
        /// </value>
        public int Value { get { return TableElement.Value; } }
        /// <summary>
        /// Gets the name of the table element which has triggered the event.
        /// </summary>
        /// <value>
        /// The name of the table element.
        /// </value>
        public string Name { get { return TableElement.Name; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableElementValueChangedEventArgs"/> class.
        /// </summary>
        /// <param name="TableElement">The table element which has triggered the event.</param>
        public TableElementValueChangedEventArgs(TableElement TableElement)
        {
            this.TableElement = TableElement;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableElementValueChangedEventArgs"/> class.
        /// </summary>
        public TableElementValueChangedEventArgs() { }
    }
}
