using System;
using System.Collections.Generic;
using DirectOutput.Table;

namespace DirectOutput.PinballSupport
{

    /// <summary>
    /// Simple queue of TableElementData objects.<br/>
    /// Used by the framework to separate data receiving and data processing.
    /// </summary>
    public class InputQueue : Queue<TableElementData>
    {
        private object QueueLocker = new object();


        /// <summary>
        /// Enqueues input data.
        /// </summary>
        /// <param name="TableElementTypeChar">Char specifing the TableElementType of the TableElement (see TableElementTypeEnum for valid values)</param>
        /// <param name="Number">The number of the TableElement.</param>
        /// <param name="Value">The value of the TableElement.</param>
        public void Enqueue(Char TableElementTypeChar, int Number, int Value)
        {
            Enqueue(new TableElementData(TableElementTypeChar, Number, Value));
        }


        /// <summary>
        /// Enqueues the specified TableElementData object.
        /// </summary>
        /// <param name="TableElementData">The table element data.</param>
        public new void Enqueue(TableElementData TableElementData)
        {
            lock (QueueLocker)
            {
                base.Enqueue(TableElementData);
            }
        }


        public void Enqueue(string TableElementName, int Value)
        {
            if (TableElementName.IsNullOrWhiteSpace()) return;

            Enqueue(new TableElementData(TableElementName, Value));





        }



        /// <summary>
        /// Dequeues the TableElementData object at the front of the queue.
        /// </summary>
        /// <returns></returns>
        public new TableElementData Dequeue()
        {
            lock (QueueLocker)
            {
                return base.Dequeue();
            }
        }

        /// <summary>
        /// Returns the TableElementData object at the front of the queue without dequeueing it.
        /// </summary>
        /// <returns></returns>
        public new TableElementData Peek()
        {
            lock (QueueLocker)
            {
                return base.Peek();
            }
        }


        /// <summary>
        /// Gets the of TableElementData objects in the queue..
        /// </summary>
        /// <value>
        /// The count of TableElementData object.
        /// </value>
        public new int Count
        {
            get
            {

                lock (QueueLocker)
                {
                    return base.Count;
                }
            }
        }


        /// <summary>
        /// Clears all elements from the queue.
        /// </summary>
        public new void Clear()
        {
            lock (QueueLocker)
            {
                base.Clear();
            }
        }



    }
}
