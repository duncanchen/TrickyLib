using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpassistLib.Struct;
using System.Collections;

namespace ExpassistLib.Extension
{
    public static class ListExtension
    {
        //public static IEnumerable<T> ToTension<T>(this IEnumerable<List<T>> input)
        //{
        //    List<T> output = new List<T>();
        //    foreach (var list in input)
        //        output.AddRange(list);

        //    return output;
        //}

        //public static IEnumerable<T> ToTension<T>(this IEnumerable<T[]> input)
        //{
        //    List<T> output = new List<T>();
        //    foreach (var list in input)
        //        output.AddRange(list);

        //    return output;
        //}

        public static IEnumerable<T> ToTension<T>(this IEnumerable<IEnumerable<T>> input)
        {
            List<T> output = new List<T>();
            foreach (var list in input)
                output.AddRange(list);

            return output;
        }

        public static T[] ToArray<T>(this object[] input)
        {
            if (input == null)
                return null;

            T[] output=  new T[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (T)(input[i]);
            }

            return output;
        }

        public static string ToRowString<T>(this IEnumerable<T> input)
        {
            return input.ToRowString("\t");
        }

        public static string ToRowString<T>(this IEnumerable<T> input, string separator)
        {
            string outputLine = null;

            if (input != null && input.Count() > 0)
            {
                foreach (var item in input)
                {
                    if (outputLine == null)
                    {
                        if (item != null)
                            outputLine = item.ToString();
                        else
                            outputLine = string.Empty;
                    }
                    else
                    {
                        if (item != null)
                            outputLine += separator + item.ToString();
                        else
                            outputLine += separator;
                    }
                }
            }

            return outputLine;
        }

        public static string ToColumnString<T>(this IEnumerable<T> input)
        {
            string outputLine = null;

            if (input != null && input.Count() > 0)
            {
                foreach (var item in input)
                {
                    if (outputLine == null)
                    {
                        if (item != null)
                            outputLine = item.ToString();
                        else
                            outputLine = string.Empty;
                    }
                    else
                    {
                        if (item != null)
                            outputLine += "\n" + item.ToString();
                        else
                            outputLine += "\n";
                    }
                }
            }

            return outputLine;
        }

        public static int FindIndex<T>(this IEnumerable<T> input, int startIndex, int count, Predicate<T> match)
        {
            if (startIndex > input.Count())
            {
                throw new ArgumentOutOfRangeException("startIndex", "out of range");
            }
            if (count < 0 || startIndex > input.Count() - count)
            {
                throw new ArgumentOutOfRangeException("count + startIndex", "out of range");
            }
            if (match == null)
            {
                throw new ArgumentNullException("Predicate<T> match is null");
            }
            int num = startIndex + count;
            for (int i = startIndex; i < num; i++)
            {
                if (match(input.ElementAt(i)))
                {
                    return i;
                }
            }
            return -1;
        }

        public static int FindIndex<T>(this IEnumerable<T> input, int startIndex, Predicate<T> match)
        {
            return input.FindIndex(startIndex, input.Count() - startIndex, match);
        }

        public static int FindIndex<T>(this IEnumerable<T> input, Predicate<T> match)
        {
            return input.FindIndex(0, input.Count(), match);
        }

        public static T Find<T>(this IEnumerable<T> input, int startIndex, int count, Predicate<T> match)
        {
            if (startIndex > input.Count())
            {
                throw new ArgumentOutOfRangeException("startIndex", "out of range");
            }
            if (count < 0 || startIndex > input.Count() - count)
            {
                throw new ArgumentOutOfRangeException("count + startIndex", "out of range");
            }
            if (match == null)
            {
                throw new ArgumentNullException("Predicate<T> match is null");
            }
            int num = startIndex + count;
            for (int i = startIndex; i < num; i++)
            {
                if (match(input.ElementAt(i)))
                {
                    return input.ElementAt(i);
                }
            }
            return default(T);
        }

        public static T Find<T>(this IEnumerable<T> input, int startIndex, Predicate<T> match)
        {
            return input.Find(startIndex, input.Count() - startIndex, match);
        }

        public static T Find<T>(this IEnumerable<T> input, Predicate<T> match)
        {
            return input.Find(0, input.Count(), match);
        }

        public static int IndexOf<T>(this IEnumerable<T> input, T item, int index = 0, int count = int.MaxValue)
        {
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException("index and count should not be less than zero");

            if (count == int.MaxValue)
                count = input.Count();

            for (int i = index; i < input.Count() && i < index + count; i++)
                if (input.ElementAt(i).Equals(item))
                    return i;

            return -1;
        }

        public static int LastIndexOf<T>(this IEnumerable<T> input, T item, int index = int.MaxValue, int count = int.MaxValue)
        {
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException("index and count should not be less than zero");

            if (index == int.MaxValue)
                index = input.Count();
            
            if (count == int.MaxValue)
                count = input.Count();

            for (int i = index; i >= 0 && i > index - count; i--)
                if (input.ElementAt(i).Equals(item))
                    return i;

            return -1;
        }

        //public static T[] GetRange<T>(this T[] input, int start, int length)
        //{
        //    if (input == null || input.Length <= 0)
        //        return null;

        //    if (start < 0 || start >= input.Length)
        //        throw new ArgumentOutOfRangeException("start index out of range");
        //    else if (length <= 0)
        //        throw new ArgumentOutOfRangeException("length should not be non-positive");

        //    List<T> output = new List<T>();
        //    for (int i = start; i < start + length && i < input.Length; ++i)
        //        output.Add(input[i]);

        //    return output.ToArray();
        //}

        //public static T[] GetRange<T>(this T[] input, int start)
        //{
        //    if (input == null || input.Length <= 0)
        //        return null;

        //    if (start < 0 || start >= input.Length)
        //        throw new ArgumentOutOfRangeException("start index out of range");

        //    List<T> output = new List<T>();
        //    for (int i = start; i < input.Length; ++i)
        //        output.Add(input[i]);

        //    return output.ToArray();
        //}

        //public static List<T> GetRange<T>(this List<T> input, int start)
        //{
        //    if (input == null || input.Count <= 0)
        //        return null;

        //    if (start < 0 || start >= input.Count)
        //        throw new ArgumentOutOfRangeException("start index out of range");

        //    List<T> output = new List<T>();
        //    for (int i = start; i < input.Count; ++i)
        //        output.Add(input[i]);

        //    return output;
        //}

        public static IEnumerable<T> GetRange<T>(this IEnumerable<T> input, int start, int length)
        {
            if (input == null || input.Count() <= 0)
                yield break;

            if (start < 0 || start >= input.Count())
                throw new ArgumentOutOfRangeException("start index out of range");
            else if (length <= 0)
                throw new ArgumentOutOfRangeException("length should not be non-positive");

            for (int i = start; i < start + length && i < input.Count(); ++i)
                yield return input.ElementAt(i);
        }

        public static IEnumerable<T> GetRange<T>(this IEnumerable<T> input, int start)
        {
            return GetRange(input, start, input.Count() - start);
        }

        public static bool ContainsAll<T>(this IEnumerable<T> input, IEnumerable<T> items)
        {
            if (input.Count() <= 0 || items.Count() <= 0)
                return false;

            foreach (var item in items)
                if (!input.Contains(item))
                    return false;

            return true;
        }

        public static void AddOrIncrease<T>(this Dictionary<T, short> input, T item)
        {
            input.AddOrIncrease(item, 1);
        }

        public static void AddOrIncrease<T>(this Dictionary<T, short> input, T item, short count)
        {
            if (input.ContainsKey(item))
                input[item] += count;
            else
                input.Add(item, count);
        }

        public static void AddOrIncrease<T>(this Dictionary<T, int> input, T item)
        {
            input.AddOrIncrease(item, 1);
        }

        public static void AddOrIncrease<T>(this Dictionary<T, int> input, T item, int count)
        {
            if (input.ContainsKey(item))
                input[item] += count;
            else
                input.Add(item, count);
        }

        public static void AddOrIncrease<T>(this Dictionary<T, long> input, T item)
        {
            input.AddOrIncrease(item, 1);
        }

        public static void AddOrIncrease<T>(this Dictionary<T, long> input, T item, long count)
        {
            if (input.ContainsKey(item))
                input[item] += count;
            else
                input.Add(item, count);
        }

        public static void AddRange<T>(this HashSet<T> input, IEnumerable<T> items)
        {
            foreach (var item in items)
                if (!input.Contains(item))
                    input.Add(item);
        }

        public static string[,] ToArray2D(this IEnumerable<string> input)
        {
            var splitInput = input.Select(line => line.Split('\t'));
            int max2DLength = splitInput.Max(a => a.Length);

            string[,] output = new string[splitInput.Count(), max2DLength];
            for (int i = 0; i < splitInput.Count(); i++)
            {
                for (int j = 0; j < splitInput.ElementAt(i).Length; j++)
                    output[i, j] = splitInput.ElementAt(i)[j];
            }

            return output;
        }

        public static void Move<T>(this List<T> input, int sourceIndex, int targetIndex)
        {
            if (!(sourceIndex >= 0 && sourceIndex < input.Count && targetIndex >= 0 && targetIndex < input.Count))
                throw new ArgumentOutOfRangeException("Index out of list range");

            if (sourceIndex != targetIndex)
            {
                var obj = input[sourceIndex];
                input.RemoveAt(sourceIndex);
                input.Insert(targetIndex, obj);
            }
        }

        public static void AddRange<S, T>(this Dictionary<S, T> input, IEnumerable<KeyValuePair<S, T>> addDic)
        {
            foreach (var kv in addDic)
                if (!input.ContainsKey(kv.Key))
                    input.Add(kv.Key, kv.Value);
        }

        public static bool Exists<T>(this IEnumerable<T> input, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("Predicate<T> match is null");
            }
            foreach(var item in input)
            {
                if (match(item))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Contains<T>(this IEnumerable<T> input, IEnumerable<T> target)
        {
            foreach (var t in target)
            {
                if (!input.Contains(t))
                    return false;
            }
            return true;
        }
    }
}
