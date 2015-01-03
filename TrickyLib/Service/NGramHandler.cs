using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Index;
using TrickyLib.Extension;
using TrickyLib.Threading;
using System.Configuration;
using TrickyLib.IO;

namespace TrickyLib.Service
{
    public class NGramHandler
    {
        private NGramService.LookupServiceClient client;
        //private NGramService2.WiabServiceClient client;
        private SearchClass wikiSearcher;
        private SearchClass localNGramSearcher;
        private SearchClass intentQueryCountSearcher;
        private SearchClass localQueryCountSearcher;

        private string _userToken = ConfigurationManager.AppSettings.Get("userToken");
        //private string _userToken = "hangli001";
        private string _specialQuery = "thiswordisprobablynotinthemodel1234567890";
        private string _ngramModel = ConfigurationManager.AppSettings.Get("ngramModel");
        private long _ngramCount = 1478154044558;
        private int maxLength = short.MaxValue;
        private int maxCount;
        //private IndexBuilder localNGram;
        

        //public HashSet<string> AllSubQueries = new HashSet<string>();
        //public Dictionary<string, long> AllSubQueryDic = new Dictionary<string, long>();

        public NGramHandler()
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

            wikiSearcher = new SearchClass(@"E:\v-haowu\QuerySegmentation\Data\UnSplittedWikipedia");
            localNGramSearcher = new SearchClass(@"E:\v-haowu\QuerySegmentation\Data\QueryIntentCount\NGramFreq");
            intentQueryCountSearcher = new SearchClass(@"E:\v-haowu\QuerySegmentation\Data\QueryIntentCount\IntentQueryCount");
            localQueryCountSearcher = new SearchClass(@"E:\v-haowu\QuerySegmentation\Data\QueryIntentCount\Query_Count");

            maxCount = maxLength / (sizeof(float) * 8);
        }

        public long GetTotalCount()
        {
            return _ngramCount;
        }

        public float GetProbability(string query)
        {
            return client.GetProbability(_userToken, _ngramModel, query);
        }
        public float[] GetProbabilities(string[] queries)
        {
            long total = queries.Length;
            long finished = 0;

            List<float> probs = new List<float>(); 
            List<string> buffer = new List<string>();
            int sumLength = 0;

            foreach (var query in queries) 
            {
                if (query.Length <= maxLength) 
                {
                    if (query.Length + sumLength > maxLength || buffer.Count >= maxCount) 
                    { 
                        probs.AddRange(client.GetProbabilities(_userToken, _ngramModel, buffer.ToArray())); 
                        buffer.Clear(); 
                        
                        buffer.Add(query); 
                        sumLength = query.Length;

                        ConsoleWriter.WritePercentage(finished, total);
                    } 
                    else 
                    { 
                        buffer.Add(query); 
                        sumLength += query.Length; 
                    } 
                } 
                else 
                { 
                    if (buffer.Count > 0) 
                    { 
                        probs.AddRange(client.GetProbabilities(_userToken, _ngramModel, buffer.ToArray()));
                        sumLength = 0; 
                        
                        buffer.Clear();
                        ConsoleWriter.WritePercentage(finished, total);
                    } 
                    
                    probs.Add(0); 
                }

                ++finished;
            }
            if (buffer.Count > 0)
            {
                probs.AddRange(client.GetProbabilities(_userToken, _ngramModel, buffer.ToArray()));
                ConsoleWriter.WritePercentage(finished, total);
            }

            return probs.ToArray();
        }
        public float[] GetProbabilities(string[] queries, int threadCount)
        {
            NGramThreadPool nGramThreadPool = new NGramThreadPool(queries, SearchType.Probability, threadCount);
            nGramThreadPool.StartWork();
            return nGramThreadPool.Probabilities;
        }

        public float GetConditionalProbability(string query)
        {
            return client.GetConditionalProbability(_userToken, _ngramModel, query);
        }
        public float[] GetConditionalProbabilities(string[] queries)
        {
            List<float> probs = new List<float>();
            List<string> buffer = new List<string>();
            int sumLength = 0;

            foreach (var query in queries)
            {
                if (query.Length <= maxLength)
                {
                    if (query.Length + sumLength > maxLength || buffer.Count >= maxCount)
                    {
                        probs.AddRange(client.GetConditionalProbabilities(_userToken, _ngramModel, buffer.ToArray()));
                        buffer.Clear();

                        buffer.Add(query);
                        sumLength = query.Length;
                    }
                    else
                    {
                        buffer.Add(query);
                        sumLength += query.Length;
                    }
                }
                else
                {
                    if (buffer.Count > 0)
                    {
                        probs.AddRange(client.GetConditionalProbabilities(_userToken, _ngramModel, buffer.ToArray()));
                        sumLength = 0;

                        buffer.Clear();
                    }

                    probs.Add(0);
                }
            }
            if (buffer.Count > 0)
                probs.AddRange(client.GetConditionalProbabilities(_userToken, _ngramModel, buffer.ToArray()));

            return probs.ToArray();
        }
        public float[] GetConditionalProbabilities(string[] queries, int threadCount)
        {
            NGramThreadPool nGramThreadPool = new NGramThreadPool(queries, SearchType.ConditionProbability, threadCount);
            nGramThreadPool.StartWork();
            return nGramThreadPool.Probabilities;
        }

        public long GetFrequency(string query)
        {
            float prob = client.GetProbability(_userToken, _ngramModel, query);
            long frequency = (long)(Math.Pow(10, prob) * _ngramCount);

            return frequency;
        }
        public long[] GetFrequencies(string[] queries)
        {
            long[] freqs = new long[queries.Length];
            float[] probs = GetProbabilities(queries);

            for (int i = 0; i < freqs.Length; i++)
                freqs[i] = (long)(Math.Pow(10, probs[i]) * _ngramCount);

            Dictionary<string, long> freqDic = new Dictionary<string, long>();

            return freqs;
        }
        public long[] GetFrequencies(string[] queries, int threadCount)
        {
            NGramThreadPool nGramThreadPool = new NGramThreadPool(queries, SearchType.Frequency, threadCount);
            nGramThreadPool.StartWork();
            return nGramThreadPool.Frequencies;
        }

        public long GetLocalNGramFrequency(string query)
        {
            List<string> searchResult = localNGramSearcher.SearchQuery(query);
            
            if (searchResult != null && searchResult.Count > 0)
                return long.Parse(searchResult[0].Split('\t')[1]);
            else
                return 0;
        }
        public long[] GetLocalNGramFrequencies(string[] queries)
        {
            long[] freqs = new long[queries.Length];
            for (int i = 0; i < queries.Length; i++)
            {
                var searchResult = localNGramSearcher.SearchQuery(queries[i]);

                if (searchResult != null && searchResult.Count > 0)
                    freqs[i] = long.Parse(searchResult[0].Split('\t')[1]);
            }

            return freqs;
        }
        public long[] GetLocalNGramFrequencies(string[] queries, int threadCount)
        {
            NGramThreadPool nGramThreadPool = new NGramThreadPool(queries, SearchType.LocalNGramFrequency, threadCount);
            nGramThreadPool.StartWork();
            return nGramThreadPool.Frequencies;
        }

        public long GetQueryFrequency(string query)
        {
            long freq = 0;
            foreach (var item in localQueryCountSearcher.SearchQuery(query))
            {
                freq += long.Parse(item.Split('\t')[1]);
            }

            return freq;
        }
        public long[] GetQueryFrequencies(string[] queries)
        {
            long[] freqs = new long[queries.Length];
            for (int i = 0; i < queries.Length; i++)
            {
                var searchResult = localQueryCountSearcher.SearchQuery(queries[i]);
                if (searchResult != null && searchResult.Count > 0)
                    foreach(var item in searchResult[0].Trim().Split('\n'))
                        freqs[i] += long.Parse(item.Trim().Split('\t')[1]);
            }

            return freqs;
        }
        public long[] GetQueryFrequencies(string[] queries, int threadCount)
        {
            NGramThreadPool nGramThreadPool = new NGramThreadPool(queries, SearchType.QueryFrequency, threadCount);
            nGramThreadPool.StartWork();
            return nGramThreadPool.Frequencies;
        }

        public long GetIntentFrequency(string query)
        {
            var searchResult = intentQueryCountSearcher.SearchQuery(query);

            if (searchResult != null && searchResult.Count > 0)
                return long.Parse(searchResult[0].Split("\r\n")[0].Split('\t')[1]);
            else
                return 0;
        }
        public long[] GetIntentFrequencies(string[] queries)
        {
            long[] freqs = new long[queries.Length];
            for (int i = 0; i < queries.Length; i++)
            {
                var searchResult = intentQueryCountSearcher.SearchQuery(queries[i]);
                if (searchResult != null && searchResult.Count > 0)
                {
                    freqs[i] = long.Parse(searchResult[0].Split("\r\n")[0].Split('\t')[1]);
                }
                //       foreach (var result in searchResult)
                //foreach (var kv in result.Trim().Split("\r\n"))
                //  freqs[i] += long.Parse(kv.Split('\t')[1]);
            }

            return freqs;
        }
        public long[] GetIntentFrequencies(string[] queries, int threadCount)
        {
            NGramThreadPool nGramThreadPool = new NGramThreadPool(queries, SearchType.IntentFrequency, threadCount);
            nGramThreadPool.StartWork();
            return nGramThreadPool.Frequencies;
        }
        
        public bool IsWikiTitle(string query)
        {
            List<string> searchResults = wikiSearcher.SearchQuery(query);
            if (searchResults != null && searchResults.Count > 0)
                return true;
            else
                return false;
        }
        public bool[] IsWikiTitles(string[] queries)
        {
            bool[] results = new bool[queries.Length];
            for (int i = 0; i < results.Length; i++)
            {
                List<string> searchResults = wikiSearcher.SearchQuery(queries[i]);
                results[i] = searchResults != null && searchResults.Count > 0;
            }

            return results;
        }
        public bool[] IsWikiTitles(string[] queries, int threadCount)
        {
            NGramThreadPool nGramThreadPool = new NGramThreadPool(queries, SearchType.IsWikiTitle, threadCount);
            nGramThreadPool.StartWork();
            return nGramThreadPool.IsWikiTitles;
        }

        public string[] GetNGramModels()
        {
            return client.GetModels();
        }
    }

    class NGramThreadPool : BaseThreadPool
    {
        public SearchType SType { get; set; }
        public float[] Probabilities { get; set; }
        public long[] Frequencies { get; set; }
        public bool[] IsWikiTitles { get; set; }

        public NGramThreadPool(string[] items, SearchType sType, int threadCount)
            : base(threadCount)
        {
            Console.WriteLine("Start multi thread NGramHandler, thread count = " + ThreadCount);
            this.SType = sType;
            this.Items = items.ToList<object>();
        }

        protected override BaseThread CreateBaseThread(object[] items)
        {
            return new NGramThread(this, items, this.SType);
        }

        protected override void FinishWork(bool isPause)
        {
            this.Probabilities = new float[0];
            this.Frequencies = new long[0];
            this.IsWikiTitles = new bool[0];

            foreach (var thread in this.Threads)
            {
                NGramThread nGramThread = thread as NGramThread;
                switch (this.SType)
                {
                    case SearchType.Probability:
                    case SearchType.ConditionProbability:
                        this.Probabilities = this.Probabilities.Concat(nGramThread.Probabilities).ToArray();
                        break;
                    case SearchType.Frequency:
                    case SearchType.LocalNGramFrequency:
                    case SearchType.IntentFrequency:
                        this.Frequencies = this.Frequencies.Concat(nGramThread.Frequencies).ToArray();
                        break;
                    case SearchType.IsWikiTitle:
                        this.IsWikiTitles = this.IsWikiTitles.Concat(nGramThread.IsWikiTitles).ToArray();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    class NGramThread : BaseThread
    {
        public SearchType SType { get; set; }
        public NGramHandler Searcher { get; set; }
        public float[] Probabilities { get; set; }
        public long[] Frequencies { get; set; }
        public bool[] IsWikiTitles { get; set; }
        public NGramThread(NGramThreadPool parent, object[] items, SearchType sType)
            : base(parent, items)
        {
            this.SType = sType;
            this.Searcher = new NGramHandler();
        }

        public override void Work()
        {
            switch (this.SType)
            { 
                case SearchType.Probability:
                    this.Probabilities = this.Searcher.GetProbabilities(this.Items.ToArray<string>());
                    break;
                case SearchType.ConditionProbability:
                    this.Probabilities = this.Searcher.GetConditionalProbabilities(this.Items.ToArray<string>());
                    break;
                case SearchType.Frequency:
                    this.Frequencies = this.Searcher.GetFrequencies(this.Items.ToArray<string>());
                    break;
                case SearchType.LocalNGramFrequency:
                    this.Frequencies = this.Searcher.GetLocalNGramFrequencies(this.Items.ToArray<string>());
                    break;
                case SearchType.QueryFrequency:
                    this.Frequencies = this.Searcher.GetQueryFrequencies(this.Items.ToArray<string>());
                    break;
                case SearchType.IntentFrequency:
                    this.Frequencies = this.Searcher.GetIntentFrequencies(this.Items.ToArray<string>());
                    break;
                case SearchType.IsWikiTitle:
                    this.IsWikiTitles = this.Searcher.IsWikiTitles(this.Items.ToArray<string>());
                    break;
                default:
                    break;
            }
            base.Work();
        }
    }

    public enum SearchType
    { 
        Probability,
        ConditionProbability,
        Frequency,
        LocalNGramFrequency,
        QueryFrequency,
        IntentFrequency,
        IsWikiTitle
    }
}
