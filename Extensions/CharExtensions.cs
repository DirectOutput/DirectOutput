using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public static class CharExtensions
    {
        /// <summary>
        /// Indicates whether the value of the char is within a specified range.
        /// </summary>
        /// <param name="MinValue">Minimum Value</param>
        /// <param name="MaxValue">>Maximum Value</param>
        /// <returns>true if the char is between <paramref name="MinValue"/> and <paramref name="MaxValue"/>, otherwise false.</returns>
        public static bool IsBetween(this char i, char MinValue, char MaxValue)
        {
            return (i >= MinValue && i <= MaxValue);
        }


        /// <summary>
        /// Converts the value of the char to its uppercase equivalent.
        /// </summary>
        /// <param name="i">The char.</param>
        /// <returns>Upper case equivalent of char.</returns>
        public static char ToUpper(this char i) {
            return char.ToUpper(i);
        }

        /// <summary>
        /// Converts the value of a Unicode character to its uppercase equivalent using the casing rules of the invariant culture.
        /// </summary>
        /// <param name="i">The char.</param>
        /// <returns>Upper case equivalent of the char.</returns>
        public static char ToUpperInvariant(this char i)
        {
            return char.ToUpperInvariant(i);
        }

        /// <summary>
        /// Converts the value of the char to its lowercase equivalent.
        /// </summary>
        /// <param name="i">The char.</param>
        /// <returns>Lower case equivalent of char.</returns>
        public static char ToLower(this char i)
        {
            return char.ToLower(i);
        }
        /// <summary>
        /// Converts the value of a Unicode character to its lowercase equivalent using the casing rules of the invariant culture.
        /// </summary>
        /// <param name="i">The char.</param>
        /// <returns>Lower case equivalent of the char.</returns>
        public static char ToLowerInvariant(this char i)
        {
            return char.ToLowerInvariant(i);
        
        }
    }

