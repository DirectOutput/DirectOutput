using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Extends the Array object with additional functionality.
/// </summary>
    public static class ArrayExtensions
    {
  
        public static T[] Concat<T>(this T[] x, T[] y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            int oldLen = x.Length;
            Array.Resize<T>(ref x, x.Length + y.Length);
            Array.Copy(y, 0, x, oldLen, y.Length);
            return x;
        }


        /// <summary>
        /// Fills the array with the specified value.
        /// </summary>
        /// <typeparam name="T">Type of the objects in the array.</typeparam>
        /// <param name="x">The source array.</param>
        /// <param name="FillValue">The fill value.</param>
        public static void Fill<T>(this T[] x, T FillValue)
        {
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = FillValue;
            }

        }
    }
