using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.General
{
    public class SimpleStatistics<ValueType> where ValueType : IComparable
    {
        public string Name { get; set; }

        

        List<ValueType> _MaxValues = new List<ValueType>();

        public ValueType MaxValue
        {
            get
            {
                return MaxValues.Max();
            }
        }

        public List<ValueType> MaxValues
        {
            get { return _MaxValues; }
            private set { _MaxValues = value; }
        }


        List<ValueType> _MinValues = new List<ValueType>();

        public ValueType MinValue
        {
            get
            {
                return MaxValues.Min();
            }
        }


        public List<ValueType> MinValues
        {
            get { return _MinValues; }
            private set { _MinValues = value; }
        }

        long _ValuesCount = 0;

        public long ValuesCount
        {
            get { return _ValuesCount; }
        }


        private ValueType MaxBoundary;
        private ValueType MinBoundary;
        private bool MinMaxBufferFull = false;
        public void AddValue(ValueType Value)
        {
            _ValuesCount++;
            if (!MinMaxBufferFull)
            {
                MaxValues.Add(Value);
                MinValues.Add(Value);
                if (MaxValues.Count >9)
                {
                    MaxBoundary = MaxValues.Min();
                    MinBoundary = MinValues.Max();
                    MinMaxBufferFull = true;
                }
                return;
            }


            if (Value.CompareTo(MaxBoundary)>0)
            {
                MaxValues.Remove(MaxBoundary);
                MaxValues.Add(Value);
                MaxBoundary = MaxValues.Min();
            }
            if (Value.CompareTo(MinBoundary) <0)
            {
                MinValues.Remove(MinBoundary);
                MinValues.Add(Value);
                MinBoundary = MinValues.Max();
            }
       
        


        }







    }
}
