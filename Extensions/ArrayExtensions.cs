using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// Extends the Array object with additional functionality.
/// </summary>
    public static class ArrayExtensions
    {

        /// <summary>
        /// Compares the array with the specified array.
        /// </summary>
        /// <typeparam name="T">Type of the array.</typeparam>
        /// <param name="CurrentArray">The current array.</param>
        /// <param name="CompareWith">The array to compare with.</param>
        /// <returns>True if both arrays are equal (size and content), otherwise false.</returns>
        public static bool CompareContents<T>(this T[] CurrentArray, T[] CompareWith)
        {
            if (CurrentArray.Length != CompareWith.Length)
            {
                return false;
            }

            for (int i = 0; i < CurrentArray.Length; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(CurrentArray[i], CompareWith[i])) return false;
            }
            return true;


        }

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

        /// <summary>
        /// Fills the array with the specified value.
        /// </summary>
        /// <typeparam name="T">Type of the objects in the array.</typeparam>
        /// <param name="x">The source array.</param>
        /// <param name="FillValue">The fill value.</param>
        /// <param name="StartPos">The start pos.</param>
        public static void Fill<T>(this T[] x, T FillValue, int StartPos)
        {
            for (int i = 0; StartPos < x.Length; i++)
            {
                x[i] = FillValue;
            }

        }
    }
