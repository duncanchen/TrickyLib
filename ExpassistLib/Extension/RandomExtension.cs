using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpassistLib.Extension
{
    public static class RandomExtension
    {
        public static T NextItem<T>(this Random rd, IEnumerable<T> items)
        {
            if (items == null || items.Count() <= 0)
                throw new ArgumentNullException("items");

            return items.ElementAt(rd.Next(items.Count()));
        }

        public static IEnumerable<T> Sample<T>(this IEnumerable<T> items, double countOrRatio)
        {
            //If larger than the items' count
            if (countOrRatio >= items.Count())
                return items;
            
            //Set the sample count
            int sampleCount = 0;
            if (countOrRatio > 0 && countOrRatio < 1)
                sampleCount = Convert.ToInt32(items.Count() * countOrRatio);
            else if (countOrRatio >= 1)
                sampleCount = Convert.ToInt32(countOrRatio);
            else
                throw new ArgumentOutOfRangeException("countOrRatio", "Should be larger than 0");

            //Save the index of the all sample
            HashSet<int> usedIndice = new HashSet<int>();
            Random rd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < sampleCount; i++)
            {
                var index = rd.Next(0, items.Count() - 1);
                while(usedIndice.Contains(index))
                    index = rd.Next(0, items.Count() - 1);
                usedIndice.Add(index);
            }

            //Save sample
            List<T> output = new List<T>();
            foreach (var index in usedIndice)
                output.Add(items.ElementAt(index));

            return output;
        }

        public static List<T> ToRandomList<T>(this List<T> inputList)
        {
            //Copy to a array
            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);

            //Add range
            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);

            //Set outputList and random
            List<T> outputList = new List<T>();
            Random rd = new Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0)
            {
                //Select an index and item
                int rdIndex = rd.Next(0, copyList.Count - 1);
                T remove = copyList[rdIndex];

                //remove it from copyList and add it to output
                copyList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }
    }
}
