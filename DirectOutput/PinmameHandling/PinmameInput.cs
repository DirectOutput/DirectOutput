using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.PinmameHandling
{
    public class PinmameInput
    {
        private TableElementTypeEnum _TableElementType;

        private object[,] _Input;

        /// <summary>
        /// Data received from Pinmame 
        /// </summary>
        public object[,] Input
        {
            get { return _Input; }
            set { _Input = value; }
        }

            
        /// <summary>
        /// Number of items in received data
        /// </summary>
        public int Count
        {
            get { return _Input.GetLength(0); }
        }


        /// <summary>
        /// Convert PinmameInput into PinmameData 
        /// </summary>
        public PinmameData GetPinmameData(int Index)
        {

            try
            {
                return new PinmameData(TableElementType, Convert.ToInt32(Input[Index, 0]), Convert.ToInt32(Input[Index, 1]));
            }
            catch (Exception e)
            {
                throw new Exception("Could not convert PinmameInput into PinmameData",e);
            }
        }


        /// <summary>
        /// TablkeElementType of the Pinmameinput
        /// </summary>
        public TableElementTypeEnum TableElementType
        {
            get { return _TableElementType; }
            set { _TableElementType = value; }
        }


        public PinmameInput(TableElementTypeEnum TableElementType, object[,] Input)
        {
            this.Input = Input;
            this.TableElementType = TableElementType;
        }

        public PinmameInput()
        {
        }
        
    }
}
