using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.IO;
using TrickyLib.Resource;
using TrickyLib.Extension;

namespace TrickyLib.Relevance
{
    public class MatchingFeature
    {
        static HashSet<string> SpecialWordsSet = new HashSet<string>(new string[]{ "http", "www", "com", "org", "gov", "php", "asp", "htm", "html", "jsp", "aspx", "shtml", "https", "cfm", "amp", "nbsp", "quot", "lt", "gt" });
        static HashSet<string> StopWordsSet = new HashSet<string>(ResourceReader.GetStopWords());

        static long RemoveStopWord(List<string> str, int left, int right)
        {
            long cnt = 0;
            int cur = left;
            while (cur < right)
            {
                int start = cur;
                while ((cur < right) && (ShouldBeDeleted(str[cur]))) ++cur;
                cnt += (long)(cur - start + 1) * (cur - start) / 2;
                ++cur;
            }
            return cnt;
        }

        static int GetExactMatch(List<string> str1, List<string> str2)
        {
            int cnt = 0;
            int[] pre = new int[str2.Count];
            int cur = -1;
            pre[0] = -1;
            for (int i = 1; i < str2.Count; ++i)
            {
                while ((cur != -1) && (str2[i] != str2[cur + 1]))
                    cur = pre[cur];

                if (str2[i] == str2[cur + 1])
                    pre[i] = ++cur;
                else
                    pre[i] = -1;
            }
            cur = -1;
            for (int i = 0; i < str1.Count; ++i)
            {
                while ((cur != -1) && (str1[i] != str2[cur + 1]))
                    cur = pre[cur];

                if (str1[i] == str2[cur + 1])
                    ++cur;

                if (cur + 1 == str2.Count)
                {
                    ++cnt;
                    cur = pre[cur];
                }
            }
            return cnt;
        }

        static long GetOrderedMatch(List<string> str1, List<string> str2)
        {
            Dictionary<string, List<int>> QuerySet = new Dictionary<string, List<int>>();
            for (int i = 0; i < str2.Count; ++i)
            {
                if (!QuerySet.ContainsKey(str2[i]))
                    QuerySet.Add(str2[i], new List<int>());

                QuerySet[str2[i]].Add(i);
            }
            int cur = 0;
            long cnt = 0;
            while (cur < str1.Count)
            {
                int start = cur;
                if (QuerySet.ContainsKey(str1[start]))
                {
                    for (int i = 0; i < QuerySet[str1[start]].Count(); ++i)
                    {
                        int pos1 = start;
                        int pos2 = QuerySet[str1[start]][i];
                        while ((pos1 < str1.Count) && (pos2 < str2.Count) && (str1[pos1] == str2[pos2]))
                        {
                            ++pos1;
                            ++pos2;
                        }
                        if (pos1 > cur) cur = pos1;
                    }
                }
                cnt += (long)(cur - start + 1) * (cur - start) / 2;
                cnt -= RemoveStopWord(str1, start, cur);
                if (start == cur) ++cur;
            }
            return cnt;
        }

        static long GetPartiallyMatch(List<string> str1, List<string> str2)
        {
            HashSet<string> QuerySet = new HashSet<string>();
            for (int i = 0; i < str2.Count; ++i)
            {
                QuerySet.Add(str2[i]);
            }
            int cur = 0;
            long cnt = 0;
            while (cur < str1.Count)
            {
                int start = cur;
                while ((cur < str1.Count) && (QuerySet.Contains(str1[cur]))) ++cur;
                cnt += (long)(cur - start + 1) * (cur - start) / 2;
                cnt -= RemoveStopWord(str1, start, cur);
                ++cur;
            }
            return cnt;
        }

        static int GetWordsFound(List<string> str1, List<string> str2)
        {
            Dictionary<string, int> QuerySet = new Dictionary<string, int>();
            for (int i = 0; i < str2.Count; ++i)
            {
                if (!QuerySet.ContainsKey(str2[i])) QuerySet.Add(str2[i], 0);
            }
            for (int i = 0; i < str1.Count; ++i)
            {
                if (QuerySet.ContainsKey(str1[i])) QuerySet[str1[i]]++;
            }
            int cnt = 0;
            for (int i = 0; i < str2.Count; ++i)
            {
                if (QuerySet[str2[i]] > 0) ++cnt;
            }
            return cnt;
        }

        static bool ShouldBeDeleted(string word)
        {
            if (word.Length <= 1)
            {
                return true;
            }
            if (StopWordsSet.Contains(word.ToLowerInvariant()))
            {
                return true;
            }
            if (SpecialWordsSet.Contains(word.ToLowerInvariant()))
            {
                return true;
            }
            if (word.StartsWith("&"))
            {
                return true;
            }
            return false;
        }

        public static IEnumerable<double> GetMatchingFeatures(List<string> queryWords, List<string> docSegments)
        {
            long ExactMatch = GetExactMatch(docSegments, queryWords);
            long OrderedMatch = GetOrderedMatch(docSegments, queryWords);
            long PartiallyMatch = 0; // GetPartiallyMatch(docSegments, queryWords);
            long WordsFound = GetWordsFound(docSegments, queryWords);

            //FeatureName.Add("ExactMatch");
            yield return ExactMatch;

            //FeatureName.Add("NormalizedExactMatch");
            if (docSegments.Count == 0)
                yield return 0;
            else
                yield return (double)ExactMatch / docSegments.Count;

            //FeatureName.Add("OrderedMatch");
            yield return OrderedMatch;

            //FeatureName.Add("NormalizedOrderedMatch");
            if (docSegments.Count == 0) 
                yield return 0;
            else
                yield return (double)OrderedMatch / docSegments.Count;

            //FeatureName.Add("PartiallyMatch");
            yield return PartiallyMatch;

            //FeatureName.Add("NormalizedPartiallyMatch");
            if (docSegments.Count == 0)
                yield return 0;
            else
                yield return (double)PartiallyMatch / docSegments.Count;

            //FeatureName.Add("WordsFound");
            yield return WordsFound;

            //FeatureName.Add("NormalizedWordsFound");
            if (queryWords.Count == 0)
                yield return 0;
            else
                yield return (double)WordsFound / queryWords.Count;
        }
    }
}
