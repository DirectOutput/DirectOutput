using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectOutput.General
{
    public class SimpleStatistics<T> where T : IComparable
    {
        List<T> _MaxValues = new List<T>();

        public T MaxValue
        {
            get
            {
                return MaxValues.Max();
            }
        }

        public List<T> MaxValues
        {
            get { return _MaxValues; }
            private set { _MaxValues = value; }
        }


        List<T> _MinValues = new List<T>();

        public T MinValue
        {
            get
            {
                return MaxValues.Min();
            }
        }


        public List<T> MinValues
        {
            get { return _MinValues; }
            private set { _MinValues = value; }
        }

        long _ValuesCount = 0;

        public long ValuesCount
        {
            get { return _ValuesCount; }
        }


        private T MaxBoundary;
        private T MinBoundary;
        private bool MinMaxBufferFull = false;
        public void AddValue(T Value)
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
