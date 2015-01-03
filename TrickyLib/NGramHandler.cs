using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib
{
    public class NGramHandler
    {
        private static NGramService.LookupServiceClient client;
        //private static NGramService2.WiabServiceClient client;
        private static SearchClass wikiSearcher;

        private static string _userToken = "5c56fe98-d1b5-4999-84f5-d5cf7f2ca572";
        //private static string _userToken = "hangli001";
        private static string _specialQuery = "thiswordisprobablynotinthemodel1234567890";
        private static string _ngramModel = "urn:ngram:bing-body:apr10:5";
        private static long _ngramCount = 1478154044558;

        static NGramHandler()
        {
            try
            {
                client = new NGramService.LookupServiceClient();
                //client = new NGramService2.WiabServiceClient();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to construct client. You need to copy the app.config to fix the problem.");
            }

            try
            {
                wikiSearcher = new SearchClass(@"E:\v-haowu\QuerySegmentation\Data\UnSplittedWikipedia");
            }
            catch (Exception e)
            {
                throw new Exception(@"path of E:\v-haowu\QuerySegmentation\Data\UnSplittedWikipedia does not exist");
            }
        }

        public static long GetTotalCount()
        {
            return _ngramCount;
        }

        public static float GetProbability(string query)
        {
            return client.GetProbability(_userToken, _ngramModel, query);
        }
        public static float[] GetProbabilities(string[] queries)
        {
            return client.GetProbabilities(_userToken, _ngramModel, queries);
        }

        public static float GetConditionalProbability(string query)
        {
            return client.GetConditionalProbability(_userToken, _ngramModel, query);
        }
        public static float[] GetConditionalProbabilities(string[] queries)
        {
            return client.GetConditionalProbabilities(_userToken, _ngramModel, queries);
        }

        public static long GetFrequency(string query)
        {
            float prob = client.GetProbability(_userToken, _ngramModel, query);
            long frequency = (long)(Math.Pow(10, prob) * _ngramCount);

            return frequency;
        }
        public static long[] GetFrequencies(string[] queries)
        {
            long[] freqs = new long[queries.Length];
            float[] probs = client.GetProbabilities(_userToken, _ngramModel, queries);

            for(int i = 0; i < freqs.Length; i++)
                probs[i] = (long)(Math.Pow(10, probs[i]) * _ngramCount);

            return freqs;
        }

        public static bool IsWikiTitle(string query)
        {
            List<string> searchResults = wikiSearcher.SearchQuery(query);
            if (searchResults != null && searchResults.Count > 0)
                return true;
            else
                return false;
        }
    }
}
