using ExpassistLib.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExpassistLib.Extension
{
    public static class StringExtention
    {
        public static int GetWordCount(this string input)
        {
            if (input.Trim() == string.Empty)
                return 0;
            else
                return input.Trim().Split(' ').Length;
        }

        public static string[] GetWords(this string input)
        {
            if (input.Trim() == string.Empty)
                return null;
            else
                return input.Trim().Split(' ');
        }

        public static string RemoveMultiSpace(this string input, string str = " ")
        {
            return Regex.Replace(input.Trim(), str + "{2,}", str);
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

        public static string TrimStart(this string input, string trimString)
        {
            string inputCopy = input;
            while (inputCopy.StartsWith(trimString, StringComparison.InvariantCultureIgnoreCase))
                inputCopy = inputCopy.Substring(trimString.Length);

            return inputCopy;
        }

        public static string TrimEnd(this string input, string trimString)
        {
            string inputCopy = input;
            while (inputCopy.EndsWith(trimString, StringComparison.InvariantCultureIgnoreCase))
                inputCopy = inputCopy.Substring(0, inputCopy.Length - trimString.Length);

            return inputCopy;
        }

        public static string Trim(this string input, string trimString)
        {
            string trimStart = input.TrimStart(trimString);
            string trimEnd = trimStart.TrimEnd(trimString);

            return trimEnd;
        }

        public static string[] Split(this string input, params string[] separators)
        {
            string pattern = RegularExpressions.NormalizeRegexPattern(separators[0]);
            for (int i = 1; i < separators.Length; i++)
                pattern += "|" + RegularExpressions.NormalizeRegexPattern(separators[i]);

            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            return reg.Split(input);
        }

        public static string GetSubWords(this string[] input, int start, int length)
        {
            if (start < 0 || start > input.Length - 1 || start + length > input.Length || length < 0)
                throw new Exception("string.GetSubWords(): index is wrong, out of the range");
            else if (length == 0)
                return string.Empty;

            string output = input[start];

            for (int i = start + 1; i < start + length && i < input.Length; i++)
                output += " " + input[i];

            return output;
        }

        public static Dictionary<string, int> GetWordCountDic(this IEnumerable<string> words)
        {
            if (words == null)
                return new Dictionary<string, int>();

            Dictionary<string, int> wordDic = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (wordDic.ContainsKey(word))
                    ++wordDic[word];
                else
                    wordDic.Add(word, 1);
            }

            return wordDic;
        }

        public static string[] Split(this string[] words, IEnumerable<string> separateWords, bool ignoreCase = true)
        {
            List<string> output = new List<string>();
            string currentChunk = string.Empty;

            foreach (var word in words)
            {
                if (separateWords.Contains(ignoreCase ? word.ToLowerInvariant() : word))
                {
                    if (currentChunk != string.Empty)
                        output.Add(currentChunk);

                    output.Add(word);
                    currentChunk = string.Empty;
                }
                else
                {
                    if (currentChunk != string.Empty)
                        currentChunk += " " + word;
                    else
                        currentChunk = word;
                }
            }

            if (currentChunk != string.Empty)
                output.Add(currentChunk);

            return output.ToArray();
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

        //public static string ConnectWords(this IEnumerable<string> words, string conn = " ")
        //{
        //    if (words == null || words.Count() == 0)
        //        return string.Empty;
        //    else
        //    {
        //        int startIndex = 0;
        //        while (string.IsNullOrEmpty(words.ElementAt(startIndex)))
        //            startIndex++;

        //        string outputString = words.ElementAt(startIndex);
        //        for (int i = startIndex + 1; i < words.Count(); i++)
        //            if (!string.IsNullOrEmpty(words.ElementAt(i)))
        //                outputString += conn + words.ElementAt(i);

        //        return outputString;
        //    }
        //}

        public static string ConnectWords<T>(this IEnumerable<T> words, string connector = " ")
        {
            if (words == null || words.Count() == 0)
                return string.Empty;
            else
            {
                string outputString = words.ElementAt(0).ToString();
                for (int i = 1; i < words.Count(); i++)
                    outputString += connector + words.ElementAt(i).ToString();

                return outputString;
            }
        }

        public static string BracketLeft(this string input, char c)
        {
            return c + input;
        }

        public static string BracketRight(this string input, char c)
        {
            return input + c;
        }

        public static string Bracket(this string input, char c)
        {
            return c + input + c;
        }

        public static string BracketLeft(this string input, string c)
        {
            return c + input;
        }

        public static string BracketRight(this string input, string c)
        {
            return input + c;
        }

        public static string Bracket(this string input, string c)
        {
            return c + input + c;
        }

        public static int IndexOfNth(this string input, char c, int n)
        {
            for (int i = 0, count = 0; i < input.Length; i++)
            {
                if (input[i] == c)
                {
                    if (++count == n)
                        return i;
                }
            }

            return -1;
        }

        public static string ReplaceTRN(this string input)
        {
            return input.Replace("\t", " ###TAB### ").Replace("\r", " ###R### ").Replace("\n", " ###N### ");
        }

        public static string RecoverTRN(this string input)
        {
            return input.Replace(" ###TAB### ", "\t").Replace(" ###R### ", "\r").Replace(" ###N### ", "\r");
        }
    }
}
