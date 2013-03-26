using System;

namespace DirectOutput.Table
{
    public class TableElementValueChangedEventArgs : EventArgs
    {
        public TableElement TableElement { get; set; }
        public int Number { get { return TableElement.Number; } }
        public int Value { get { return TableElement.Value; } }
        public string Name { get { return TableElement.Name; } }

        public TableElementValueChangedEventArgs(TableElement TableElement)
        {
            this.TableElement = TableElement;
        }

        public TableElementValueChangedEventArgs() { }
    }
}
