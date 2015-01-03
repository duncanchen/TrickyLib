using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using TrickyLib.IO;
using TrickyLib.Struct;
using TrickyLib.Extension;
using TrickyLib.Service;

namespace TrickyLib.Service.QuerySegmentation
{
    public class QSinfo
    {
        public static int MaxSegWord = 6;
        public static int MaxSeg = 10;

        public string Query { get; set; }
        public string Segmentation
        {
            get
            {
                if (this.Segments == null || this.Segments.Length == 0)
                    return null;
                else
                    return this.Segments.Select(s => /*s.GetWordCount() <= 1 ? s :*/ s.Bracket("\"")).ConnectWords();
            }
        }

        //public bool IsMatch { get; set; }
        public long Score { get; set; }
        public int Rank { get; set; }
        public int NewRank { get; set; }
        //public double CondSepProb { get; set; }

        public string[] Words;
        public int WordCount
        {
            get
            {
                if (this.Words != null)
                    return this.Words.Length;
                else
                    return 0;
            }
        }

        private List<int> _breaks;
        public List<int> Breaks
        {
            get
            {
                if (this._breaks == null)
                {
                    this._breaks = new List<int>();
                    foreach (var segment in this.Segments)
                    {
                        if (this._breaks.Count == 0)
                            this._breaks.Add(segment.GetWordCount());
                        else
                            this._breaks.Add(segment.GetWordCount() + this._breaks.Last());
                    }
                }

                return this._breaks;
            }
            set
            {
                this._breaks = value;
            }
        }
        public string[] Segments;

        public double SVMScore { get; set; }

        public int SegmentCount
        {
            get
            {
                if (this.Segments != null)
                    return this.Segments.Length;
                else
                    return 0;
            }
        }

        public Dictionary<string, long> SubQueryFreqDic;
        public Dictionary<string, long> SegmentConDic;
        public HashSet<string> WikiTitles;
        //public Dictionary<string, double> SubQueryCondProbDic;
        public Dictionary<string, long> SubQueryIntentCountDic;
        public IEnumerable<QSinfo> GroupPointer;

        //Constructor
        public QSinfo(Segmentation segmentation,  IEnumerable<QSinfo> groupPointer, Dictionary<string, long> subQueryFreqDic, HashSet<string> wikiTitles, Dictionary<string, long> subQueryIntentCountDic)
        {
            //this.Segmentation = segmentation;
            this.Query = segmentation.CleanedQuery;
            //this.IsMatch = isMatch;
            this.Score = segmentation.Score;
            this.Rank = segmentation.Rank;
            this.Words = this.Query.GetWords();
            this.GroupPointer = groupPointer;

            //SetBreaks();
            this.Breaks = segmentation.SegmentIndexList;
            SetSegments();

            //SetQueryFreqDic();
            this.SubQueryFreqDic = subQueryFreqDic;
            //SetIsWikiTitleDic();
            this.WikiTitles = wikiTitles;
            SetSegmentConDic();
            //SetQueryCondProbDic();
            //SetCondSepProb();
            //SetQueryIntentCountDic();
            this.SubQueryIntentCountDic = subQueryIntentCountDic;
        }

        public QSinfo() { }
        #region set members
        //private void SetBreaks()
        //{
        //    int currentIndex = 0;
        //    this.Breaks = new List<int>();

        //    foreach (string segment in GetSegments(this.Segmentation))
        //    {
        //        if (segment.Trim() != string.Empty)
        //        {
        //            currentIndex += segment.GetWordCount();
        //            this.Breaks.Add(currentIndex);
        //        }
        //    }
        //}

        private void SetSegments()
        {
            int startIndex = 0;
            List<string> segments = new List<string>();

            foreach (var breakIndex in this.Breaks)
            {
                int length = breakIndex - startIndex;
                segments.Add(this.Query.GetSubWords(startIndex, length));

                startIndex = breakIndex;
            }
            this.Segments = segments.ToArray();
        }

        //private void SetQueryFreqDic()
        //{
        //    List<string[]> ngramValues = TotalQueryFreqDic[this.Query];
        //    this.SubQueryFreqDic = new Dictionary<string, long>();

        //    foreach (string[] ngramValue in ngramValues)
        //    {
        //        string ngram = ngramValue[0];
        //        long value = long.Parse(ngramValue[1]);

        //        if(!this.SubQueryFreqDic.ContainsKey(ngram))
        //            this.SubQueryFreqDic.Add(ngram, value);
        //    }
        //}

        private void SetSegmentConDic()
        {
            this.SegmentConDic = new Dictionary<string, long>();

            for (int i = -1; i < Breaks.Count - 1; i++)
            {
                string segment = string.Empty;
                int length = 0;
                if (i < 0)
                {
                    segment = GetSubQuery(0, Breaks[0]);
                    length = Breaks[0];
                }
                else
                {
                    segment = GetSubQuery(Breaks[i], Breaks[i + 1] - Breaks[i]);
                    length = Breaks[i + 1] - Breaks[i];
                }

                if (!this.SegmentConDic.ContainsKey(segment))
                {
                    if (length >= 2 && this.SubQueryFreqDic[segment] <= 0)
                        this.SegmentConDic.Add(segment, -1);

                    else
                    {
                        string[] segWords = segment.GetWords();

                        if (length <= 1)
                            this.SegmentConDic.Add(segment, 0);

                        else if (!this.WikiTitles.Contains(segment))
                            this.SegmentConDic.Add(segment, this.SubQueryFreqDic[segment] * length);

                        else
                        {
                            long maxSubScore = 0;
                            for (int j = 0; j < segWords.Length - 1; j++)
                            {
                                string wordpair = segWords[j] + " " + segWords[j + 1];
                                if (maxSubScore < this.SubQueryFreqDic[wordpair])
                                    maxSubScore = this.SubQueryFreqDic[wordpair];
                            }

                            this.SegmentConDic.Add(segment, (maxSubScore + segWords.Length) * segWords.Length);
                        }
                    }
                }
            }
        }

        //private void SetIsWikiTitleDic()
        //{
        //    List<string[]> ngramValues = TotalQueryFreqDic[this.Query];
        //    this.IsWikiTitleDic = new Dictionary<string, bool>();

        //    foreach (string[] ngramValue in ngramValues)
        //    {
        //        string ngram = ngramValue[0];
        //        bool value = bool.Parse(ngramValue[2]);

        //        if(!this.IsWikiTitleDic.ContainsKey(ngram))
        //            this.IsWikiTitleDic.Add(ngram, value);
        //    }
        //}

        //private void SetQueryCondProbDic()
        //{ 
        //    List<string[]> ngramValues = TotalQueryCondProbDic[this.Query];
        //    this.SubQueryCondProbDic = new Dictionary<string, double>();

        //    foreach (string[] ngramValue in ngramValues)
        //    {
        //        string ngram = ngramValue[0];
        //        double value = double.Parse(ngramValue[1]);

        //        if (!this.SubQueryCondProbDic.ContainsKey(ngram))
        //            this.SubQueryCondProbDic.Add(ngram, value);
        //    }
        //}

        //private void SetQueryIntentCountDic()
        //{
        //    List<string[]> ngramValues = TotalQueryIntentCountDic[this.Query];
        //    this.SubQueryIntentCountDic = new Dictionary<string, Quad<long, long, long, long>>();

        //    foreach (string[] ngramValue in ngramValues)
        //    {
        //        string ngram = ngramValue[0];

        //        long value1 = long.Parse(ngramValue[1]);
        //        long value2 = long.Parse(ngramValue[2]);
        //        long value3 = long.Parse(ngramValue[3]);
        //        long value4 = long.Parse(ngramValue[4]);
        //        Quad<long, long, long, long> values = new Quad<long, long, long, long>() { First = value1, Second = value2, Third = value3, Forth = value4 };

        //        if (!this.SubQueryIntentCountDic.ContainsKey(ngram))
        //            this.SubQueryIntentCountDic.Add(ngram, values);
        //    }
        //}

        //private void SetCondSepProb()
        //{
        //    this.CondSepProb = 0;
        //    string[] words = this.Query.GetWords();

        //    for (int i = 1; i < words.Length; i++)
        //    {
        //        string word = words[i];
        //        double maxProb = this.SubQueryCondProbDic.Where(kv => kv.Key.EndsWith(word) && kv.Key.GetWordCount() > 1).Max(kv => kv.Value);

        //        int hasBreak = !this.Breaks.Contains(i) ? 1 : 0;
        //        this.CondSepProb += hasBreak * maxProb + (1 - hasBreak) * Math.Log10(1 - Math.Pow(10, maxProb));
        //    }
        //}
        #endregion

        public string GetSubQuery(int startIndex, int length)
        {
            string output = string.Empty;
            for (int i = startIndex, j = 0; j < length; j++)
            {
                if (output != string.Empty)
                    output += " ";

                output += Words[i + j];
            }
            return output;
        }

        public static string CleanQuery(string query)
        {
            return query.Replace("\"", "").Trim();
        }

        public static IEnumerable<string> GetSegments(string segmentation)
        {
            List<string> output = new List<string>();
            string[] candidates = segmentation.Split('\"');

            foreach (var item in candidates)
            {
                if (item.Trim() != string.Empty)
                    output.Add(item.Trim());
            }

            return output;
        }
    }

}