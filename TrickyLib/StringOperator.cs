using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Extension;

namespace TrickyLib
{
    public class StringOperator
    {
        /// <summary>
        /// GetSegmentations("a b c") = {"a b c", "\"a b\" c", "a \"b c\"", "\"a b c\""}
        /// </summary>
        /// <param name="str"></param>
        public static string[] GetSegmentations(string str)
        {
            //Normalize query
            string cleanedQuery = str.RemoveMultiSpace();
            if (cleanedQuery == string.Empty)
                return new string[] { cleanedQuery };

            //Initial all the possible segmented queries
            string[] words = cleanedQuery.GetWords();
            int currentWordIndex = 1;
            List<string> queries = new List<string>();
            queries.Add(words[0]);
            while (currentWordIndex < words.Length)
            {
                int currentQueryCount = queries.Count;
                for (int i = 0; i < currentQueryCount; i++)
                {
                    //currentWord attached
                    if (queries[i].EndsWith("\"")) //last word of queries[i] is attached
                        queries.Add(queries[i].TrimEnd("\"") + " " + words[currentWordIndex] + "\"");
                    else //last word of queries[i] is independent
                    {
                        string[] queriesI_Words = queries[i].GetWords();
                        queriesI_Words[queriesI_Words.Length - 1] = "\"" + queriesI_Words.Last();
                        var newQueriesI_Words = queriesI_Words.Concat(new string[] { words[currentWordIndex] + "\"" });
                        queries.Add(newQueriesI_Words.ConnectWords());
                    }

                    //currentWord independent
                    queries[i] += " " + words[currentWordIndex];
                }

                currentWordIndex++;
            }

            return queries.ToArray();
        }
    }
}
