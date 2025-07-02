/*	* * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * modified by Tyler Jensen from 
 * http://www.codeguru.com/vb/gen/vb_misc/algorithms/article.php/c13137__1/Fuzzy-Matching-Demo-in-Access.htm
 * Also see  http://www.berghel.net/publications/asm/asm.php 
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;

using System.Text;

namespace FuzzyStrings
{
	public static class FuzzyText
	{
		/// <summary>
		/// Levenshtein Distance algorithm with transposition. <br />
		/// A value of 1 or 2 is okay, 3 is iffy and greater than 4 is a poor match
		/// </summary>
		/// <param name="input"></param>
		/// <param name="comparedTo"></param>
		/// <param name="caseSensitive"></param>
		/// <returns></returns>
        public static double Levenshtein(string input, string comparedTo, bool caseSensitive = false)
		{
			if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(comparedTo)) return 999;
			if (!caseSensitive)
			{
				input = input.ToLower();
				comparedTo = comparedTo.ToLower();
			}
			int inputLen = input.Length;
			int comparedToLen = comparedTo.Length;

			int[,] matrix = new int[inputLen, comparedToLen];

			//initialize
			for (int i = 0; i < inputLen; i++) matrix[i, 0] = i;
			for (int i = 0; i < comparedToLen; i++) matrix[0, i] = i;

			//analyze
			for (int i = 1; i < inputLen; i++)
			{
				var si = input[i - 1];
				for (int j = 1; j < comparedToLen; j++)
				{
					var tj = comparedTo[j - 1];
					int cost = (si == tj) ? 0 : 1;

					var above = matrix[i - 1, j];
					var left = matrix[i, j - 1];
					var diag = matrix[i - 1, j - 1];
					var cell = FindMinimum(above + 1, left + 1, diag + cost);

					//transposition
					if (i > 1 && j > 1)
					{
						var trans = matrix[i - 2, j - 2] + 1;
						if (input[i - 2] != comparedTo[j - 1]) trans++;
						if (input[i - 1] != comparedTo[j - 2]) trans++;
						if (cell > trans) cell = trans;
					}
					matrix[i, j] = cell;
				}
			}
			return (double)1/(1+matrix[inputLen - 1, comparedToLen - 1]);
		}

		private static int FindMinimum(params int[] p)
		{
			if (null == p) return int.MinValue;
			int min = int.MaxValue;
			for (int i = 0; i < p.Length; i++)
			{
				if (min > p[i]) min = p[i];
			}
			return min;
		}



        /// <summary>
        /// Dice Coefficient based on bigrams. <br />
        /// A good value would be 0.33 or above, a value under 0.2 is not a good match, from 0.2 to 0.33 is iffy.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="comparedTo"></param>
        /// <returns></returns>
        public static double DiceCoefficient(string input, string comparedTo)
        {
            var ngrams = ToBiGrams(input);
            var compareToNgrams = ToBiGrams(comparedTo);
            return DiceCoefficient(ngrams,compareToNgrams);
        }

        /// <summary>
        /// Dice Coefficient used to compare nGrams arrays produced in advance.
        /// </summary>
        /// <param name="nGrams"></param>
        /// <param name="compareToNGrams"></param>
        /// <returns></returns>
        private static double DiceCoefficient( string[] nGrams, string[] compareToNGrams)
        {
            int matches = 0;
            foreach (var nGram in nGrams)
            {
                foreach (string x in compareToNGrams)
                {
                    if (x == nGram)
                    {
                        matches++;
                        break;
                    }
                }
            }
            if (matches == 0) return 0.0d;
            double totalBigrams = nGrams.Length + compareToNGrams.Length;
            return (2 * matches) / totalBigrams;
        }

        private static string[] ToBiGrams( string input)
        {
            // nLength == 2
            //   from Jackson, return %j ja ac ck ks so on n#
            //   from Main, return #m ma ai in n#
            input = SinglePercent + input + SinglePound;
            return ToNGrams(input, 2);
        }

        private static string[] ToTriGrams( string input)
        {
            // nLength == 3
            //   from Jackson, return %%j %ja jac ack cks kso son on# n##
            //   from Main, return ##m #ma mai ain in# n##
            input = DoublePercent + input + DoublePount;
            return ToNGrams(input, 3);
        }

        private static string[] ToNGrams(string input, int nLength)
        {
            int itemsCount = input.Length - 1;
            string[] ngrams = new string[input.Length - 1];
            for (int i = 0; i < itemsCount; i++) ngrams[i] = input.Substring(i, nLength);
            return ngrams;
        }

        private const string SinglePercent = "%";
        private const string SinglePound = "#";
        private const string DoublePercent = "&&";
        private const string DoublePount = "##";

	}

	public static class LongestCommonSubsequenceExtensions
	{
		/// <summary>
		/// Longest Common Subsequence. A good value is greater than 0.33.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="comparedTo"></param>
		/// <param name="caseSensitive"></param>
		/// <returns>Returns a Tuple of the sub sequence string and the match coefficient.</returns>
		public static Tuple<string, double> LongestCommonSubsequence(this string input, string comparedTo, bool caseSensitive = false)
		{
			if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(comparedTo)) return new Tuple<string, double>(string.Empty, 0.0d);
			if (!caseSensitive)
			{
				input = input.ToLower();
				comparedTo = comparedTo.ToLower();
			}

			int inputLen = input.Length;
			int comparedToLen = comparedTo.Length;

			int[,] lcs = new int[inputLen + 1, comparedToLen + 1];
			LcsDirection[,] tracks = new LcsDirection[inputLen + 1, comparedToLen + 1];
			int[,] w = new int[inputLen + 1, comparedToLen + 1];
			int i, j;

			for (i = 0; i <= inputLen; ++i)
			{
				lcs[i, 0] = 0;
				tracks[i, 0] = LcsDirection.North;

			}
			for (j = 0; j <= comparedToLen; ++j)
			{
				lcs[0, j] = 0;
				tracks[0, j] = LcsDirection.West;
			}

			for (i = 1; i <= inputLen; ++i)
			{
				for (j = 1; j <= comparedToLen; ++j)
				{
					if (input[i - 1].Equals(comparedTo[j - 1]))
					{
						int k = w[i - 1, j - 1];
						//lcs[i,j] = lcs[i-1,j-1] + 1;
						lcs[i, j] = lcs[i - 1, j - 1] + Square(k + 1) - Square(k);
						tracks[i, j] = LcsDirection.NorthWest;
						w[i, j] = k + 1;
					}
					else
					{
						lcs[i, j] = lcs[i - 1, j - 1];
						tracks[i, j] = LcsDirection.None;
					}

					if (lcs[i - 1, j] >= lcs[i, j])
					{
						lcs[i, j] = lcs[i - 1, j];
						tracks[i, j] = LcsDirection.North;
						w[i, j] = 0;
					}

					if (lcs[i, j - 1] >= lcs[i, j])
					{
						lcs[i, j] = lcs[i, j - 1];
						tracks[i, j] = LcsDirection.West;
						w[i, j] = 0;
					}
				}
			}

			i = inputLen;
			j = comparedToLen;

			string subseq = "";
			double p = lcs[i, j];

			//trace the backtracking matrix.
			while (i > 0 || j > 0)
			{
				if (tracks[i, j] == LcsDirection.NorthWest)
				{
					i--;
					j--;
					subseq = input[i] + subseq;
					//Trace.WriteLine(i + " " + input1[i] + " " + j);
				}

				else if (tracks[i, j] == LcsDirection.North)
				{
					i--;
				}

				else if (tracks[i, j] == LcsDirection.West)
				{
					j--;
				}
			}

			double coef = p / (inputLen * comparedToLen);

			Tuple<string, double> retval = new Tuple<string, double>(subseq, coef);
			return retval;
		}

		private static int Square(int p)
		{
			return p * p;
		}
	}

	internal enum LcsDirection
	{
		None,
		North,
		West,
		NorthWest
	}
}
