using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TrickyLib
{
    public static class Extensions
    {
        #region string
        public static int GetWordCount(this string input)
        {
            return input.Split(' ').Length;
        }

        public static string[] GetWords(this string input)
        {
            return input.Split(' ');
        }

        public static bool ContainsWord(this string input, string word)
        {
            string[] intputWords = input.ToLower().Split(' ');
            string wordLower = word.ToLower();

            foreach (var inputWord in intputWords)
            {
                if (inputWord == wordLower)
                    return true;
            }

            return false;
        }

        public static string GetSubWords(this string input, int start, int length)
        {
            string[] words = input.GetWords();
            return words.GetSubWords(start, length);
        }

        public static string GetSubWords(this string[] input, int start, int length)
        {
            string output = input[start];

            for (int i = start + 1; i < start + length; i++)
                output +=" " + input[i];

            return output;
        }

        public static MatchCollection GetMatches(this string input, string pattern)
        {
            Regex reg = new Regex(pattern);
            return reg.Matches(input);
        }

        public static bool IsMatch(this string input, string pattern)
        {
            Regex reg = new Regex(pattern);
            return reg.IsMatch(input);
        }
        #endregion

        #region List
        public static IEnumerable<T> GetTension<T>(this IEnumerable<List<T>> input)
        {
            List<T> output = new List<T>();
            foreach (var list in input)
                output.AddRange(list);

            return output;
        }

        public static IEnumerable<T> GetTension<T>(this IEnumerable<T[]> input)
        {
            List<T> output = new List<T>();
            foreach (var list in input)
                output.AddRange(list);

            return output;
        }
        #endregion
    }
}
