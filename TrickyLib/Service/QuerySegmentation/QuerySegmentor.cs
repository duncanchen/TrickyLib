using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TrickyLib.Extension;
using TrickyLib;
using TrickyLib.MachineLearning;
using TrickyLib.Index;
using TrickyLib.IO;
using TrickyLib.Struct;
using System.IO;
using System.Text.RegularExpressions;
using TrickyLib.Parser;
using TrickyLib.Threading;
using System.Reflection;

namespace TrickyLib.Service.QuerySegmentation
{
    public class QuerySegmentor
    {
        public string ModelFile { get; set; }

        public SVMModel SvmModel;
        public NGramHandler NGramHandler;
        //public NLParser NlpParser;

        private WikipediaSubQueryScore _wikiScore;
        private NaiveSubQueryScore _naiveScore;

        private SubQueryScore _subScore;
        private Dictionary<string, List<Segmentation>> _formerSegDic;

        private string _query;
        public string Query
        {
            get
            {
                return _query;
            }
            set
            {
                _query = value;
                this._subScore.QueryArray = value.GetWords();
            }
        }

        private int _topN;
        public int TopN
        {
            get
            {
                return _topN;
            }
            set
            {
                _topN = value;
            }
        }

        public int ThreadCount { get; set; }

        public SegMethodType SegType { get; set; }

        public Dictionary<string, long> NGramFreqDic
        {
            get
            {
                return this._subScore.SubQueryFreqDic;
            }
            set
            {
                this._subScore.SubQueryFreqDic = value;
            }
        }
        public HashSet<string> WikiTitles
        {
            get
            {
                return this._subScore.WikiTitles;
            }
            set
            {
                this._subScore.WikiTitles = value;
            }
        }
        public Dictionary<string, long> IntentFreqDic { get; set; }
        public Triple<Dictionary<string, long>, HashSet<string>, Dictionary<string, long>> NGramFreqWikiIntentFreqTriple
        {
            get
            {
                return new Triple<Dictionary<string, long>, HashSet<string>, Dictionary<string, long>>()
                {
                    First = this.NGramFreqDic,
                    Second = this.WikiTitles,
                    Third = this.IntentFreqDic
                };
            }
            set
            {
                this.NGramFreqDic = value.First;
                this.WikiTitles = value.Second;
                this.IntentFreqDic = value.Third;
            }
        }

        public QuerySegmentor()
        {
            InitializeComponent();

            this.IntentFreqDic = new Dictionary<string, long>();
            this.NGramFreqDic = new Dictionary<string, long>();
            this.WikiTitles = new HashSet<string>();
            this.ThreadCount = 1;
        }

        public QuerySegmentor(string modelFile = "")
        {
            InitializeComponent();

            if (modelFile != string.Empty)
            {
                this.ModelFile = modelFile;
                SvmModel.ReadSVMModel(ModelFile);
            }
            else
            {
                this.ModelFile = @"TrickyLib.Resources.SegmentBaseline.model";
                Assembly mAssembly = Assembly.GetExecutingAssembly();
                using (Stream stream = mAssembly.GetManifestResourceStream(this.ModelFile))
                {
                    SvmModel.ReadSVMModel(stream);
                }
            }
            this._subScore = _wikiScore;
            this.IntentFreqDic = new Dictionary<string, long>();
            this.NGramFreqDic = new Dictionary<string, long>();
            this.WikiTitles = new HashSet<string>();
            this.ThreadCount = 1;
        }

        public QuerySegmentor(SegMethodType segMethodType)
        {
            InitializeComponent();
            this.SegType = segMethodType;
            if (segMethodType == SegMethodType.BwcTrain)
                this.ModelFile = @"TrickyLib.Resource.SegmentBaseline.model";
            else if (segMethodType == SegMethodType.Fine)
                this.ModelFile = @"TrickyLib.Resource.Fine.Vote.Svm.NoIntent.model";
            else if (segMethodType == SegMethodType.Coarse)
                this.ModelFile = @"TrickyLib.Resource.Coarse.Vote.Svm.NoIntent.model";
            Assembly mAssembly = Assembly.GetExecutingAssembly();
            using (Stream stream = mAssembly.GetManifestResourceStream(this.ModelFile))
            {
                SvmModel.ReadSVMModel(stream);
            }

            this._subScore = _wikiScore;
            this.IntentFreqDic = new Dictionary<string, long>();
            this.NGramFreqDic = new Dictionary<string, long>();
            this.WikiTitles = new HashSet<string>();
            this.ThreadCount = 1;
        }

        public void InitializeComponent()
        {
            try
            {
                _wikiScore = new WikipediaSubQueryScore();
                _naiveScore = new NaiveSubQueryScore();
                _formerSegDic = new Dictionary<string, List<Segmentation>>();
                this._subScore = _wikiScore;

                SvmModel = new SVMModel();
                NGramHandler = new NGramHandler();
                //NlpParser = new NLParser();
            }
            catch (Exception ex)
            { 
            
            }
        }

        protected List<Segmentation> Uk(int k)
        {
            List<Segmentation> topNList = new List<Segmentation>();
            for (int i = 0; i < k; i++)
            {
                Segmentation seg = new Segmentation() { CleanedQuery = this.Query };
                seg.SegmentIndexList.Add(k);
                seg.Score = this._subScore.GetSubScore(i + 1, k);

                List<Segmentation> formerSegList = null;
                string formerSubQuery = _subScore.GetSubString(1, i);

                if (i > 0 && _formerSegDic.Keys.Contains(formerSubQuery))
                    formerSegList = _formerSegDic[formerSubQuery];
                else if (i > 0)
                {
                    formerSegList = Uk(i);
                    _formerSegDic.Add(formerSubQuery, formerSegList);
                }

                if (formerSegList != null)
                {
                    foreach (Segmentation formerSeg in formerSegList)
                    {
                        if (topNList.Count >= this.TopN)
                        {
                            if (seg.Score + formerSeg.Score >= topNList.First().Score)
                            {
                                Segmentation segClone = seg.Clone();
                                segClone.SegmentIndexList.AddRange(formerSeg.SegmentIndexList);

                                if (segClone.Score >= 0 && formerSeg.Score >= 0)
                                    segClone.Score += formerSeg.Score;
                                else
                                    segClone.Score = -1;

                                topNList.Add(segClone);
                                if (topNList.Count > this.TopN)
                                    topNList.RemoveAt(0);

                                topNList = topNList.OrderBy(s => s.Score).ToList();
                            }
                        }
                        else
                        {
                            Segmentation segClone = seg.Clone();
                            segClone.SegmentIndexList.AddRange(formerSeg.SegmentIndexList);

                            if (segClone.Score >= 0 && formerSeg.Score >= 0)
                                segClone.Score += formerSeg.Score;
                            else
                                segClone.Score = -1;

                            topNList.Add(segClone);
                            topNList = topNList.OrderBy(s => s.Score).ToList();
                        }
                    }
                }
                else
                {
                    if (seg.Score < 0)
                        seg.Score = -1;

                    topNList.Add(seg);
                }
            }
            return topNList;
        }

        public void SetNaiveSubScore()
        {
            if (!(this._subScore is NaiveSubQueryScore))
                this._subScore = _naiveScore;
        }
        public void SetWikiSubScore()
        {
            if (!(this._subScore is WikipediaSubQueryScore))
                this._subScore = _wikiScore;
        }

        public Segmentation[] GetBaselineSegmentations(string query, bool isDicReady = true, double topN = 6, bool use_MS_N_gram = true)
        {
            if (!isDicReady)
                SetFreqWikiIntentFreqDics(query);

            query = query.RemoveMultiSpace();
            string[] words = query.GetWords();

            if (string.IsNullOrEmpty(query) || words == null || words.Length <= 0)
            {
                List<Segmentation> outputs = new List<Segmentation>();
                outputs.Add(new Segmentation());
                return outputs.ToArray();
            }
            else if (words.Length == 1)
            {
                List<Segmentation> outputs = new List<Segmentation>();
                Segmentation segmentation = new Segmentation() { CleanedQuery = query };
                segmentation.SegmentIndexList.Add(1);

                outputs.Add(segmentation);
                return outputs.ToArray();
            }

            this.Query = query;
            this.TopN = topN >= 1 ? (int)topN : (int)(Math.Pow(2, words.Length - 1) * topN);
            this._formerSegDic.Clear();

            this._subScore.use_MS_N_gram = use_MS_N_gram;
            this._subScore.QueryArray = query.GetWords();

            List<Segmentation> segList = Uk(words.Length);
            //if (!segList.Exists(seg => seg.SegmentIndexList.Count == words.Length))
            //{
            //    Segmentation segmentation = new Segmentation() { CleanedQuery = query, Score = 0, SegmentIndexList = new List<int>() };
            //    for (int i = 1; i <= words.Length; ++i)
            //        segmentation.SegmentIndexList.Add(i);

            //    segList.Add(segmentation);
            //}

            segList = segList.OrderByDescending(seg => seg.Score).ToList();
            for (int i = 0; i < segList.Count; i++)
            {
                segList[i].Rank = i;
                segList[i].SegmentIndexList.Sort();
            }

            return segList.ToArray();
        }
        public List<Segmentation[]> GetBaselineSegmentations(string[] queries, bool isDicReady = true, double topN = 6)
        {
            if (!isDicReady)
                SetFreqWikiIntentFreqDics(queries);

            List<Segmentation[]> outputs = new List<Segmentation[]>();
            for (int i = 0; i < queries.Length; i++)
                outputs.Add(GetBaselineSegmentations(queries[i], true, topN));

            return outputs;
        }

        public string[] Split(string query)
        {
            if (this.SegType == SegMethodType.WT)
                return GetWikipediaTitleSegments(query);

            else if (this.SegType == SegMethodType.WBN)
                return GetBaselineSegments(query);

            else
                return GetSvmSegments(query);
        }

        public string[] GetBaselineSegments(string query)
        {
            return GetBaselineSegmentations(query)[0].GetSegments();
        }
        public List<string[]> GetBaselineSegments(string[] queries, bool isDicReady = true)
        {
            if (!isDicReady)
                SetFreqWikiIntentFreqDics(queries);

            List<string[]> output = new List<string[]>();
            foreach (var query in queries)
                output.Add(GetBaselineSegments(query));

            return output;
        }

        public string[] GetWikipediaTitleSegments(string query)
        {
            List<Triple<int, int, long>> wikiTitleSegments = new List<Triple<int, int, long>>();
            List<Triple<int, int, long>> remainedSegments = new List<Triple<int, int, long>>();
            string[] words = query.Split(' ');
            this.Query = query;
            this._subScore.QueryArray = words;
            this._subScore.use_MS_N_gram = true;

            for (int i = 0; i < words.Length; ++i)
            { 
                for (int j = i + 1; j < words.Length; ++j)
                {
                    long freq = GetSubScore(i, j - i + 1, true);
                    if (freq > 0)
                        wikiTitleSegments.Add(new Triple<int, int, long>(i, j, freq));
                }
            }

            wikiTitleSegments = wikiTitleSegments.OrderByDescending(x => x.Third).ToList();
            List<int> breaks = new List<int>();
            while (wikiTitleSegments.Count > 0)
            {
                remainedSegments.Add(wikiTitleSegments[0]);

                List<int> removedIndice = new List<int>();
                for (int i = 0; i < wikiTitleSegments.Count; ++i)
                {
                    if (!(wikiTitleSegments[i].Second < wikiTitleSegments[0].First || wikiTitleSegments[i].First > wikiTitleSegments[0].Second))
                        removedIndice.Add(i);
                }

                for (int i = removedIndice.Count - 1; i >= 0; --i)
                    wikiTitleSegments.RemoveAt(removedIndice[i]);
            }

            remainedSegments = remainedSegments.OrderBy(x => x.First).ToList();
            int startIndex = 0;
            List<string> segments = new List<string>();
            foreach (var remainedSegment in remainedSegments)
            {
                while (startIndex < remainedSegment.First)
                {
                    segments.Add(words.GetSubWords(startIndex, 1));
                    ++startIndex;
                }

                segments.Add(words.GetSubWords(startIndex, remainedSegment.Second - startIndex + 1));
                startIndex = remainedSegment.Second + 1;
            }

            while (startIndex < words.Length)
                segments.Add(words.GetSubWords(startIndex++, 1));

            return segments.ToArray();
        }
        public List<string[]> GetWikipediaTitleSegments(string[] queries, bool isDicReady = true)
        {
            if (!isDicReady)
                SetFreqWikiIntentFreqDics(queries);

            List<string[]> output = new List<string[]>();
            foreach (var query in queries)
                output.Add(GetWikipediaTitleSegments(query));

            return output;
        }

        //public string[] GetNlpSegments(string query)
        //{
        //    Regex punctuationRegex = new Regex(@"'(?!(s|t|re|m)( |$))|\.$|\.{2,}|©|`|~|!|@|#|\$|%|\^|\*|\(|\)|(^|[^\w])-+|-+($|[^\w])|_|=|\+|\[|\]|\{|\}|<|>|\\|\||/|;|:|""|•|–|,|\?|×|！|·|…|—|（|）|、|：|；|‘|’|“|”|《|》|，|。|？");
        //    string[] segments = new string[0];
        //    foreach (var nChunk in this.NlpParser.SentenceSeg(query))
        //        segments = segments.Concat(punctuationRegex.Split(nChunk).Where(seg => seg != string.Empty)).ToArray();

        //    return segments;
        //}
        //public List<string[]> GetNlpSegments(string[] queries)
        //{
        //    ConsoleWriter.WriteCurrentMethodStarted();
        //    Regex punctuationRegex = new Regex(@"'(?!(s|t|re|m)( |$))|\.$|\.{2,}|©|`|~|!|@|#|\$|%|\^|\*|\(|\)|(^|[^\w])-+|-+($|[^\w])|_|=|\+|\[|\]|\{|\}|<|>|\\|\||/|;|:|""|•|–|,|\?|×|！|·|…|—|（|）|、|：|；|‘|’|“|”|《|》|，|。|？");
        //    List<string[]> segmentsList = new List<string[]>();
        //    foreach (var query in queries)
        //    {
        //        List<string> segments = new List<string>();
        //        foreach (var nChunk in this.NlpParser.SentenceSeg(query))
        //            foreach (var pChunk in punctuationRegex.Split(nChunk))
        //                if (pChunk.Trim() != string.Empty)
        //                    segments.Add(pChunk.Trim());

        //        segmentsList.Add(segments.ToArray());
        //    }

        //    return segmentsList;
        //}

        //public string GetConcatNlpSegments(string query)
        //{
        //    return GetNlpSegments(query).ConnectWords(" ").RemoveMultiSpace();
        //}
        //public string[] GetConcatNlpSegments(string[] queries)
        //{
        //    return queries.Select(q => GetConcatNlpSegments(q)).ToArray();
        //}

        public string[] GetSvmSegments(string query, bool intentRead = true, bool isDicReady = true)
        {
            QS_FeatureVector[] qsFvs = GetFeatureVectors(query, intentRead, isDicReady);
            var svmScores = SvmModel.GetSvmScores(qsFvs.Select(qsFv => qsFv.FullVector).ToArray());
            double max = svmScores.Max();

            int maxIndex = svmScores.FindIndex(value => value == max);
            return qsFvs[maxIndex].Target.Segments;
        }
        public List<string[]> GetSvmSegments(string[] queries, bool intentRead = true, bool isDicReady = true)
        {
            if (!isDicReady)
                SetFreqWikiIntentFreqDics(queries, intentRead);

            List<string[]> svmSegmentsList = new List<string[]>();
            foreach (var query in queries)
            {
                var removed = query.RemoveMultiSpace();
                if (removed != string.Empty)
                    svmSegmentsList.Add(GetSvmSegments(removed, intentRead, true));
            }
            return svmSegmentsList;
        }

        public QS_FeatureVector GetSvmMaxQS_FeatureVector(string query, bool useFilter = false, bool intentRead = true, bool isDicReady = true)
        {
            QS_FeatureVector[] qsFvs = GetFeatureVectors(query, intentRead, isDicReady);
            //if (useFilter)
            //    qsFvs = qsFvs.Where(fv => !fv.IsFiltered()).ToArray();

            if (qsFvs.Length == 0)
                return null;

            var svmScores = SvmModel.GetSvmScores(qsFvs.Select(qsFv => qsFv.FullVector).ToArray());
            double max = svmScores.Max();

            int maxIndex = svmScores.FindIndex(value => value == max);
            return qsFvs[maxIndex];
        }
        public List<QS_FeatureVector> GetSvmMaxQS_FeatureVectors(string[] queries, bool intentRead = true, bool isDicReady = true)
        {
            if (!isDicReady)
                SetFreqWikiIntentFreqDics(queries, intentRead);

            List<QS_FeatureVector> maxQS_FeatureVectorList = new List<QS_FeatureVector>();
            foreach (var query in queries)
                maxQS_FeatureVectorList.Add(GetSvmMaxQS_FeatureVector(query, intentRead, true));

            return maxQS_FeatureVectorList;
        }

        //public string[] GetNlpSvmSegments(string query, bool intentRead = true, bool isDicReady = true)
        //{
        //    var segments = GetNlpSegments(query);
        //    return GetSvmSegments(segments, intentRead, isDicReady).ToTension().ToArray();
        //}
        //public List<string[]> GetNlpSvmSegments(string[] queries, bool intentRead = true, bool isDicReady = true)
        //{
        //    var segmentsList = GetNlpSegments(queries);

        //    if (!isDicReady)
        //        SetFreqWikiIntentFreqDics(segmentsList.ToTension().ToArray(), intentRead);

        //    List<string[]> outputSegments = new List<string[]>();
        //    foreach (var segments in segmentsList)
        //        outputSegments.Add(GetSvmSegments(segments, intentRead).ToTension().ToArray());

        //    return outputSegments;
        //}

        //public string[] GetConcatNlpSvmSegments(string query, bool intentRead = true, bool isDicReady = true)
        //{
        //    string concatQuery = GetConcatNlpSegments(query);
        //    return GetSvmSegments(concatQuery, intentRead, isDicReady);
        //}
        //public List<string[]> GetConcatNlpSvmSegments(string[] queries, bool intentRead = true, bool isDicReady = true)
        //{
        //    var concatQueries = GetConcatNlpSegments(queries);

        //    if (!isDicReady)
        //        SetFreqWikiIntentFreqDics(concatQueries, intentRead);

        //    List<string[]> outputSegments = new List<string[]>();
        //    foreach (var concatQuery in concatQueries)
        //        outputSegments.Add(GetSvmSegments(concatQuery, intentRead));

        //    return outputSegments;
        //}

        public List<QS_FeatureVector[]> GetQueryProbeFeatureVectors(string[] queries, string fwifFile = null, bool useFilter = false, bool intentRead = true, int topN = 6, string[] humanSegmentation = null)
        {
            var normalizedQueries = queries.Select(q => RegularExpressions.UselessPunctionRegex.Replace(q, " ").RemoveMultiSpace()).ToArray();

            //Initialize the QuerySegmentor and the freqWikiIntentDic
            List<QuerySegmentor> segmentors = new List<QuerySegmentor>();
            segmentors.Add(new QuerySegmentor(SegMethodType.BwcTrain));
            segmentors.Add(new QuerySegmentor(SegMethodType.Fine));
            segmentors.Add(new QuerySegmentor(SegMethodType.Coarse));

            if (fwifFile == null || !File.Exists(fwifFile))
            {
                if (fwifFile != string.Empty)
                    segmentors[0].SaveFreqWikiIntentFreqDicsToFile(normalizedQueries, fwifFile);
                else
                    segmentors[0].SetFreqWikiIntentFreqDics(normalizedQueries);
                for (int i = 1; i < segmentors.Count; i++)
                    segmentors[i].NGramFreqWikiIntentFreqTriple = segmentors[0].NGramFreqWikiIntentFreqTriple;
            }
            else
                foreach (var segmentor in segmentors)
                    segmentor.LoadFreqWikiIntentFreqDicsFromFile(fwifFile);

            List<QS_FeatureVector[]> output = new List<QS_FeatureVector[]>();
            Console.WriteLine("Total " + queries.Length + " queries");
            for (int qid = 1; qid <= queries.Length; qid++)
            {
                string normalizedQuery = normalizedQueries[qid - 1];

                var segments = normalizedQuery.GetWords().Split(Parameters.StopWords_NoArticle);
                //var segmentationsList = segmentors[0].GetBaselineSegmentations(segments, true, this.TopN);
                var segmentationsList = GetSegmentationsList(segments, segmentors);

                if (segmentationsList == null || segmentationsList.Count == 0)
                    continue;

                //Get the topN segmentations
                List<Segmentation> topNSegmentations = segmentationsList[0].ToList();
                for (int i = 1; i < segmentationsList.Count; i++)
                    topNSegmentations = GetTopNSegmentations(topNSegmentations, segmentationsList[i], (int)topN);

                //Add the fully segmentation
                if (!topNSegmentations.Exists(s => s.SegmentIndexList.Count == s.CleanedQuery.GetWords().Length))
                {
                    topNSegmentations.Add(new Segmentation()
                    {
                        CleanedQuery = topNSegmentations[0].CleanedQuery,
                        Score = 0,
                    });

                    for (int i = 1; i <= topNSegmentations.Last().CleanedQuery.GetWords().Length; i++)
                        topNSegmentations.Last().SegmentIndexList.Add(i);
                }

                //Sorted and assign rank
                var sortedTopNSegmentations = topNSegmentations.OrderByDescending(s => s.Score);
                int rank = 1;
                foreach (var segmentation in sortedTopNSegmentations)
                    segmentation.Rank = rank++;

                List<QSinfo> qsInfos = new List<QSinfo>();
                List<QS_FeatureVector> qsFvs = new List<QS_FeatureVector>();

                foreach (var segmentation in sortedTopNSegmentations)
                {
                    int label = 0;

                    QSinfo qsInfo = new QSinfo(segmentation, qsInfos, segmentors[0].NGramFreqDic, segmentors[0].WikiTitles, segmentors[0].IntentFreqDic);
                    QS_FeatureVector qsFv = new QS_FeatureVector(label, qid, qsInfo) { HumanLabeledSegmentation = queries[qid - 1] };

                    qsInfos.Add(qsInfo);
                    qsFvs.Add(qsFv);
                }

                if (qsFvs.Exists(fv => fv.Comment == string.Empty))
                    throw new Exception("There is a QsFv with empty comment");

                //if (useFilter)
                //    qsFvs.RemoveAll(fv => fv.IsFiltered());

                output.Add(qsFvs.ToArray());
                ConsoleWriter.WriteStep(qid, 1);
            }
            return output;
        }

        public void SetFreqWikiIntentFreqDics(string query, bool intentRead = true)
        {
            HashSet<string> subQueries = new HashSet<string>();
            string multiSpaceRemoved = query.RemoveMultiSpace();
            string[] words = multiSpaceRemoved.GetWords();

            for (int length = 1; length <= words.Length; length++)
            {
                for (int start = 0; start <= words.Length - length; start++)
                {
                    string subQuery = words.GetSubWords(start, length);
                    if (!subQueries.Contains(subQuery))
                        subQueries.Add(subQuery);
                }
            }
            for (int i = 0; i < words.Length - 1; i++)
            {
                string collapseWord = words[i] + words[i + 1];

                if (!subQueries.Contains(collapseWord))
                    subQueries.Add(collapseWord);
            }

            var subQueryArray = subQueries.ToArray();
            long[] freqs = NGramHandler.GetFrequencies(subQueryArray);
            this.NGramFreqDic.Clear();

            for (int i = 0; i < subQueryArray.Length; i++)
                if (!this.NGramFreqDic.ContainsKey(subQueryArray[i]))
                    this.NGramFreqDic.Add(subQueryArray[i], freqs[i]);

            var isWikiTitles = NGramHandler.IsWikiTitles(subQueries.ToArray());
            this.WikiTitles.Clear();

            for (int i = 0; i < subQueryArray.Length; i++)
                if (isWikiTitles[i] && !this.WikiTitles.Contains(subQueryArray[i]))
                    this.WikiTitles.Add(subQueryArray[i]);

            if (intentRead)
            {
                var intentFreqs = NGramHandler.GetIntentFrequencies(subQueries.ToArray());
                this.IntentFreqDic.Clear();

                for (int i = 0; i < subQueryArray.Length; i++)
                    if (!this.IntentFreqDic.ContainsKey(subQueryArray[i]))
                        this.IntentFreqDic.Add(subQueryArray[i], intentFreqs[i]);
            }
        }
        public void SetFreqWikiIntentFreqDics(string[] queries, bool intentRead = true)
        {
            ConsoleWriter.WriteCurrentMethodStarted();
            HashSet<string> subQueries = new HashSet<string>();
            foreach (var query in queries)
            {
                string multiSpaceRemoved = query.RemoveMultiSpace();
                string[] words = multiSpaceRemoved.GetWords();

                if (words == null || words.Length <= 0)
                    continue;

                for (int length = 1; length <= words.Length; length++)
                {
                    for (int start = 0; start <= words.Length - length; start++)
                    {
                        string subQuery = words.GetSubWords(start, length);
                        if (!subQueries.Contains(subQuery))
                            subQueries.Add(subQuery);
                    }
                }
                for (int i = 0; i < words.Length - 1; i++)
                {
                    string collapseWord = words[i] + words[i + 1];

                    if (!subQueries.Contains(collapseWord))
                        subQueries.Add(collapseWord);
                }
            }

            var subQueryArray = subQueries.ToArray();
            Console.WriteLine("Start get frequencies");
            long[] freqs = NGramHandler.GetFrequencies(subQueryArray, this.ThreadCount);
            this.NGramFreqDic.Clear();

            for (int i = 0; i < subQueryArray.Length; i++)
                if (!this.NGramFreqDic.ContainsKey(subQueryArray[i]))
                    this.NGramFreqDic.Add(subQueryArray[i], freqs[i]);

            Console.WriteLine("Start get wikiTitles");
            var isWikiTitles = NGramHandler.IsWikiTitles(subQueries.ToArray(), this.ThreadCount);
            this.WikiTitles.Clear();

            for (int i = 0; i < subQueryArray.Length; i++)
                if (isWikiTitles[i] && !this.WikiTitles.Contains(subQueryArray[i]))
                    this.WikiTitles.Add(subQueryArray[i]);

            if (intentRead)
            {
                Console.WriteLine("Start get intent frequencies");
                var intentFreqs = NGramHandler.GetIntentFrequencies(subQueries.ToArray(), this.ThreadCount);
                this.IntentFreqDic.Clear();

                for (int i = 0; i < subQueryArray.Length; i++)
                    if (!this.IntentFreqDic.ContainsKey(subQueryArray[i]))
                        this.IntentFreqDic.Add(subQueryArray[i], intentFreqs[i]);
            }

            ConsoleWriter.WriteCurrentMethodFinished();
        }

        public void LoadFreqWikiIntentFreqDicsFromFile(string filePath)
        {
            ConsoleWriter.WriteCurrentMethodStarted();
            //this.NGramFreqWikiIntentFreqTriple = FileReader.Deserialize<Triple<Dictionary<string, long>, HashSet<string>, Dictionary<string, long>>>(filePath);

            if (this.NGramFreqDic == null)
                this.NGramFreqDic = new Dictionary<string, long>();
            else
                this.NGramFreqDic.Clear();

            if (this.WikiTitles == null)
                this.WikiTitles = new HashSet<string>();
            else
                this.WikiTitles.Clear();

            if (this.IntentFreqDic == null)
                this.IntentFreqDic = new Dictionary<string, long>();
            else
                this.IntentFreqDic.Clear();

            AddFreqWikiintentFreqDicsFromFile(filePath);
        }
        public void SaveFreqWikiIntentFreqDicsToFile(string[] queries, string filePath)
        {
            ConsoleWriter.WriteCurrentMethodStarted();
            SetFreqWikiIntentFreqDics(queries, true);
            //FileWriter.Serialize(this.NGramFreqWikiIntentFreqTriple, filePath);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var kv in this.NGramFreqDic)
                {
                    string swLine = kv.Key + "\t" + kv.Value;

                    if (this.WikiTitles.Contains(kv.Key))
                        swLine += "\t" + true.ToString();
                    else
                        swLine += "\t" + false.ToString();

                    swLine += "\t" + this.IntentFreqDic[kv.Key];
                    sw.WriteLine(swLine);
                }
            }
            ConsoleWriter.WriteCurrentMethodFinished();
        }
        public void AddFreqWikiintentFreqDicsFromFile(string filePath)
        {
            if (this.NGramFreqDic == null)
                this.NGramFreqDic = new Dictionary<string, long>();

            if (this.WikiTitles == null)
                this.WikiTitles = new HashSet<string>();

            if (this.IntentFreqDic == null)
                this.IntentFreqDic = new Dictionary<string, long>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split('\t');
                    string nGram = lineArray[0];
                    long freq = long.Parse(lineArray[1]);
                    bool wiki = bool.Parse(lineArray[2]);
                    long intent = long.Parse(lineArray[3]);

                    if (!this.NGramFreqDic.ContainsKey(nGram))
                        this.NGramFreqDic.Add(nGram, freq);
                    if (wiki && !this.WikiTitles.Contains(nGram))
                        this.WikiTitles.Add(nGram);
                    if (!this.IntentFreqDic.ContainsKey(nGram))
                        this.IntentFreqDic.Add(nGram, intent);
                }
            }
        }

        public QS_FeatureVector[] GetFeatureVectors(string query, bool intentRead = true, bool isDicReady = true, double topN = 6, int qid = 1, string humanSegmentation = "")
        {
            if (!isDicReady)
                SetFreqWikiIntentFreqDics(query, intentRead);

            var segmentations = GetBaselineSegmentations(query, true, topN).ToList();
            QSinfo[] qsInfos = new QSinfo[segmentations.Count];
            QS_FeatureVector[] qsFvs = new QS_FeatureVector[segmentations.Count];

            for (int i = 0; i < segmentations.Count; i++)
            {
                qsInfos[i] = new QSinfo(segmentations[i], qsInfos, this.NGramFreqDic, this.WikiTitles, intentRead ? this.IntentFreqDic : new Dictionary<string, long>());
                qsFvs[i] = new QS_FeatureVector(qsInfos[i].Segmentation == humanSegmentation, qid, qsInfos[i]) { HumanLabeledSegmentation = humanSegmentation };
            }

            var vectors = qsFvs.Select(fv => fv.FullVector).ToList();
            var featureLines = vectors.Select(v => new FeatureLine(v, true)).ToList();
            
            Dictionary<int, Pair<double, double>> maxMinDic = new Dictionary<int, Pair<double, double>>();
            foreach (var featureLine in featureLines)
            {
                foreach (var kv in featureLine.ItemsDic)
                {
                    if (maxMinDic.ContainsKey(kv.Key))
                    {
                        if (maxMinDic[kv.Key].First < kv.Value)
                            maxMinDic[kv.Key].First = kv.Value;

                        if (maxMinDic[kv.Key].Second > kv.Value)
                            maxMinDic[kv.Key].Second = kv.Value;
                    }
                    else
                    {
                        maxMinDic.Add(kv.Key, new Pair<double, double>(kv.Value, kv.Value));
                    }
                }
            }

            foreach (var featureLine in featureLines)
                featureLine.Normalize(maxMinDic);

            for (int i = 0; i < qsFvs.Length; ++i)
                qsFvs[i].FullVector = featureLines[i].VectorString;

                return qsFvs;
        }
        public List<QS_FeatureVector[]> GetFeatureVectors(string[] queries, bool intentRead = true, bool isDicReady = true, double topN = 6, string[] segmentations = null)
        {
            ConsoleWriter.WriteCurrentMethodStarted();
            if (queries == null || segmentations != null && queries.Length != segmentations.Length)
                throw new Exception("queries == null || segmentations != null && queries.Length != segmentations.Length");

            if (!isDicReady)
                SetFreqWikiIntentFreqDics(queries, intentRead);

            List<QS_FeatureVector[]> featureVectors = new List<QS_FeatureVector[]>();
            for (int i = 0; i < queries.Length; ++i)
                featureVectors.Add(GetFeatureVectors(queries[i], intentRead, isDicReady, topN, i + 1, segmentations != null ? segmentations[i] : ""));
            //for (int j = 0; j < queries.Length; j++)
            //{
            //    var baseLineSegmentations = GetBaselineSegmentations(queries[j], true, topN);

            //    QSinfo[] qsInfos = new QSinfo[baseLineSegmentations.Length];
            //    QS2_FeatureVector[] qsFvs = new QS2_FeatureVector[baseLineSegmentations.Length];

            //    for (int i = 0; i < baseLineSegmentations.Length; i++)
            //    {
            //        qsInfos[i] = new QSinfo(baseLineSegmentations[i], qsInfos, this.NGramFreqDic, this.WikiTitles, intentRead ? this.IntentFreqDic : new Dictionary<string, long>());
            //        qsFvs[i] = new QS2_FeatureVector(segmentations != null && qsInfos[i].Segmentation == segmentations[j], j + 1, qsInfos[i], segmentations != null ? segmentations[j] : "");
            //    }

            //    featureVectors.Add(qsFvs);
            //}
            return featureVectors;
        }

        public long GetSubScore(int start, int length, bool onlyWikiTitle = false)
        {
            return this._subScore.GetSubScore(start + 1, start + length, onlyWikiTitle);
        }

        public List<Segmentation[]> GetSegmentationsList(string[] segments, IEnumerable<QuerySegmentor> segmentors)
        {
            List<Segmentation[]> segmentationsList = new List<Segmentation[]>();
            foreach (var segment in segments)
            {
                List<Segmentation> segmentations = new List<Segmentation>();
                foreach (var segmentor in segmentors)
                {
                    var maxQsFv = segmentor.GetSvmMaxQS_FeatureVector(segment);
                    if (maxQsFv == null)
                        return null;

                    var segmentation = maxQsFv.ConvertToSegmentation();
                    if (segmentations.Count == 0 || !segmentations.Exists(s => s.GetSegmentationString() == segmentation.GetSegmentationString()))
                        segmentations.Add(segmentation);
                }
                segmentationsList.Add(segmentations.ToArray());
            }
            return segmentationsList;
        }
        private List<Segmentation> GetTopNSegmentations(List<Segmentation> firstSegmentations, Segmentation[] secondSegmentations, int topN)
        {
            List<Segmentation> topNSegmentations = new List<Segmentation>();
            foreach (var first in firstSegmentations)
            {
                foreach (var second in secondSegmentations)
                {
                    Segmentation possibleSegmentation = new Segmentation()
                    {
                        CleanedQuery = first.CleanedQuery + " " + second.CleanedQuery,
                        Score = (first.Score < 0 || second.Score < 0) ? -1 : first.Score + second.Score,
                        SegmentIndexList = first.SegmentIndexList.Concat(second.SegmentIndexList.Select(index => index + first.CleanedQuery.GetWords().Length)).ToList()
                    };

                    topNSegmentations.Add(possibleSegmentation);
                }
            }

            var sortedTopNSegmentations = topNSegmentations.OrderByDescending(seg => seg.Score).ToList();
            var output = sortedTopNSegmentations.GetRange(0, (int)(Math.Min(topNSegmentations.Count, topN)));
            return output;
        }

        public static void GetSegmentationFeatureFile(string[] queries, string[] humanSegmentations, string outputFeatureFile, string fwifFile = null)
        {

            if (queries == null || humanSegmentations == null || queries.Length != humanSegmentations.Length)
                throw new Exception("Queries or HumanSegmentations is error, please check");

            try
            {
                QuerySegmentor segmentor = new QuerySegmentor();
                bool fwifExists = false;
                if (File.Exists(fwifFile))
                {
                    segmentor.LoadFreqWikiIntentFreqDicsFromFile(fwifFile);
                    fwifExists = true;
                }
                var fvsList = segmentor.GetFeatureVectors(queries, false, fwifExists, 6, humanSegmentations);
                using (StreamWriter sw = new StreamWriter(outputFeatureFile))
                {
                    foreach (var fvs in fvsList)
                    {
                        foreach (var fv in fvs)
                            sw.WriteLine(fv.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static SegmentAccuracy CalculateSegmentationAccuracy(string[] segmentations, string[] humanSegmentations)
        {
            SegmentAccuracy accuracy = new SegmentAccuracy();
            for (int i = 0; i < segmentations.Length; ++i)
                accuracy.AddNewSegmentAndReturnInstanceAccuracy(segmentations[i], humanSegmentations[i]);

            return accuracy;
        }

        public static string[] GetBreakFusionSegments(Dictionary<string, int> segmentationDic)
        {
            if (segmentationDic.Count <= 0)
                return null;

            //Generate the original query
            string query = segmentationDic.First().Key.Replace("\"", "").RemoveMultiSpace().Replace(" \" ", "\" ");
            string[] words = query.Split(' ');
            int[] breakCounts = new int[words.Length];

            int annotatorCount = segmentationDic.Sum(kv => kv.Value);
            breakCounts[words.Length - 1] = annotatorCount;

            //Record the break counts
            foreach(var kv in segmentationDic)
            {
                string segmentation = kv.Key.RemoveMultiSpace().Replace(" \" ", "\" ");
                string[] segWords = segmentation.Split(' ');
                bool enterSeg = false;

                for (int j = 0; j < segWords.Length - 1; j++)
                {
                    if (segWords[j].StartsWith("\""))
                        enterSeg = true;

                    if (segWords[j].EndsWith("\""))
                    {
                        breakCounts[j] += kv.Value;
                        enterSeg = false;
                    }
                    else if (!enterSeg)
                    {
                        breakCounts[j] += kv.Value;
                    }
                }
            }

            //Generate the final segments
            int start = 0;
            List<string> breakFusionSegments = new List<string>();
            for (int end = 0; end < breakCounts.Length; ++end)
            {
                if (breakCounts[end] >= (annotatorCount * 1.0 / 2))
                {
                    int length = end - start + 1;
                    breakFusionSegments.Add(words.GetSubWords(start, length));
                    start = end + 1;
                }
            }

            return breakFusionSegments.ToArray();
        }
        public static string[] GetBreakFusionSegments(string[] segmentations)
        {
            if (segmentations == null || segmentations.Length <= 0)
                return null;

            //Generate the original query
            string query = segmentations[0].Replace("\"", "").RemoveMultiSpace();
            string[] words = query.Split(' ');
            int[] breakCounts = new int[words.Length];

            int annotatorCount = segmentations.Length;
            breakCounts[words.Length - 1] = annotatorCount;

            //Record the break counts
            foreach (var segmentation in segmentations)
            {
                string[] segWords = segmentation.RemoveMultiSpace().Replace(" \" ", "\" ").Split(' ');
                bool enterSeg = false;

                for (int j = 0; j < segWords.Length - 1; j++)
                {
                    if (segWords[j].StartsWith("\""))
                        enterSeg = true;

                    if (segWords[j].EndsWith("\""))
                    {
                        breakCounts[j] += 1;
                        enterSeg = false;
                    }
                    else if (!enterSeg)
                    {
                        breakCounts[j] += 1;
                    }
                }
            }

            //Generate the final segments
            int start = 0;
            List<string> breakFusionSegments = new List<string>();
            for (int end = 0; end < breakCounts.Length; ++end)
            {
                if (breakCounts[end] >= (annotatorCount * 1.0 / 2))
                {
                    int length = end - start + 1;
                    breakFusionSegments.Add(words.GetSubWords(start, length));
                    start = end + 1;
                }
            }

            return breakFusionSegments.ToArray();
        }
    }

    public class SubQueryScore
    {
        public Dictionary<string, long> SubQueryFreqDic { get; set; }
        public HashSet<string> WikiTitles { get; set; }

        public bool use_MS_N_gram = true;

        public SubQueryScore()
        { 
        
        }

        protected SearchClass _searcher = new SearchClass(@"E:\v-haowu\QuerySegmentation\Data\SortedData");

        public string[] QueryArray { get; set; }

        public SubQueryScore(string[] queryArray)
        {
            this.QueryArray = queryArray;
        }

        public virtual long GetSubScore(int start, int end, bool onlyWikiTitle = false)
        {
            return -1;
        }

        protected long GetFreq(int start, int end)
        {
            try
            {
                if (start >= end)
                    return 0;

                string subQuery = this.QueryArray[start - 1];
                for (int i = start; i < end; i++)
                    subQuery += " " + this.QueryArray[i];

                long sumFrequency = 0;

                if (this.SubQueryFreqDic.ContainsKey(subQuery))
                {
                    //throw new KeyNotFoundException(string.Format("key:[{0}] not exists. Total keys:[{1}]. Query:[{2}]", subQuery, this.SubQueryFreqDic.Count, this.QueryArray.ConnectWords()));

                    if (!use_MS_N_gram)
                    {
                        List<string> searchResults = this._searcher.SearchQuery(subQuery);
                        foreach (string result in searchResults)
                        {
                            string[] resultArray = result.Split('\t');
                            int frequency = 0;
                            if (int.TryParse(resultArray[1], out frequency))
                                sumFrequency += frequency;
                        }
                    }
                    else
                        sumFrequency = this.SubQueryFreqDic[subQuery];
                }

                if (sumFrequency > 0)
                    return sumFrequency;
                else
                    return -1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetSubString(int start, int end)
        {
            if (start > end)
                return string.Empty;

            string subString = this.QueryArray[start - 1];
            for (int i = start; i < end; i++)
            {
                subString += " " + this.QueryArray[i];
            }
            return subString;
        }
    }

    public class NaiveSubQueryScore : SubQueryScore
    {
        public NaiveSubQueryScore()
        { 
        
        }

        public override long GetSubScore(int start, int end, bool onlyWikiTitle)
        {
            long subQueryScore = 0;
            long freq = GetFreq(start, end);
            if (freq < 0)
                subQueryScore = freq;
            else
                subQueryScore = Convert.ToInt64((Math.Pow(Convert.ToDouble(end - start + 1), Convert.ToDouble(end - start + 1)))) * freq;

            return subQueryScore;
        }
    }

    public class WikipediaSubQueryScore : SubQueryScore
    {
        public WikipediaSubQueryScore()
        { 
        
        }

        public override long GetSubScore(int start, int end, bool onlyWikiTitle)
        {
            if (start >= end)
                return 0;

            if (!onlyWikiTitle)
            {
                long subQueryScore = 0;
                string subQuery = GetSubString(start, end);

                if (this.WikiTitles.Contains(subQuery))
                {
                    long maxFreq = GetFreq(start, end);
                    for (int i = start; i < end; i++)
                    {
                        long conWordFreq = GetFreq(i, i + 1);
                        if (maxFreq < conWordFreq)
                            maxFreq = conWordFreq;
                    }
                    subQueryScore = end - start + 1 + maxFreq;
                }
                else
                    subQueryScore = GetFreq(start, end);

                if (subQueryScore > 0)
                    subQueryScore = (end - start + 1) * subQueryScore;
                else
                    subQueryScore = -1;

                return subQueryScore;
            }
            else
            {
                string subQuery = GetSubString(start, end);
                long subQueryScore = 0;
                if (this.WikiTitles.Contains(subQuery))
                {
                    for (int i = start; i < end; i++)
                    {
                        long conWordFreq = GetFreq(i, i + 1);
                        if (subQueryScore < conWordFreq)
                            subQueryScore = conWordFreq;
                    }
                }
                return subQueryScore *(end - start + 1);
            }
        }
    }

    [DataContract]
    public class Segmentation
    {
        public string CleanedQuery;

        [DataMember]
        public List<int> SegmentIndexList = new List<int>();

        [DataMember]
        public long Score = 0;

        public int Rank = -1;

        public Segmentation Clone()
        {
            List<int> cloneSegmentIndexList = new List<int>();
            foreach (int i in SegmentIndexList)
                cloneSegmentIndexList.Add(i);

            return new Segmentation() { SegmentIndexList = cloneSegmentIndexList, Score = this.Score, CleanedQuery = this.CleanedQuery };
        }

        public override string ToString()
        {
            return GetSegments().Select(s => /*s.GetWordCount() <= 1 ? s : */ s.Bracket("\"")).ConnectWords();
        }

        public string[] GetSegments()
        {
            string output = string.Empty;
            string[] words = this.CleanedQuery.GetWords();
            List<string> segments = new List<string>();
            int start = 0;

            for (int i = 0; i < SegmentIndexList.Count; i++)
            {
                segments.Add(words.GetSubWords(start, SegmentIndexList[i] - start));
                start = SegmentIndexList[i];
            }
            return segments.ToArray();
        }

        public string GetSegmentationString()
        {
            return SearchEngine.SearchEngineParser.SegmentsToQuery(this.GetSegments());
        }
    }

    public class QS_ThreadPool : BaseThreadPool
    {
        public SegmentationType SegType { get; set; }
        public List<string[]> ResultList { get; set; }
        public bool IntentRead { get; set; }
        public string ModelFile { get; set; }
        private QuerySegmentor tempSegmentor = new QuerySegmentor();
        public string FreqWikiIntentFreqFile { get; set; }
        public QS_ThreadPool(object[] items, SegmentationType segType, bool intentRead, int threadCount, string modelFile = "", bool isDicReady = false, string fwifFile = "Temp.Fwif")
            : base(threadCount)
        {
            this.FreqWikiIntentFreqFile = fwifFile;
            if (!isDicReady)
            {
                string[] queries = null;
                //if (segType == SegmentationType.NlpSvmSegmentation)
                //    queries = tempSegmentor.GetNlpSegments((string[])Convert.ChangeType(items, typeof(string[]))).ToTension().ToArray();
                //else if (segType == SegmentationType.ConcatNlpSvmSegmentation)
                //    queries = tempSegmentor.GetConcatNlpSegments((string[])Convert.ChangeType(items, typeof(string[])));
                //else 
                if (segType == SegmentationType.SvmSegmentation)
                    queries = (string[])Convert.ChangeType(items, typeof(string[]));
                else
                    throw new NotImplementedException();

                this.tempSegmentor.SaveFreqWikiIntentFreqDicsToFile(queries, this.FreqWikiIntentFreqFile);
            }

            this.ModelFile = modelFile;
            this.Items = items.ToList<object>();
            this.IntentRead = intentRead;
            SegType = segType;
            this.ResultList = new List<string[]>();
        }

        protected override BaseThread CreateBaseThread(object[] items)
        {
            return new QS_Thread(this, items);
        }

        protected override void FinishWork(bool isPause)
        {
            foreach (QS_Thread thread in this.Threads)
            {
                this.ResultList.AddRange(thread.ResultList);
            }
        }
    }

    public class QS_Thread : BaseThread
    {
        public QuerySegmentor Segmentor { get; set; }
        public SegmentationType SegType
        {
            get
            {
                return ((QS_ThreadPool)this.Parent).SegType;
            }
        }
        public List<string[]> ResultList { get; set; }
        public bool IntentRead
        {
            get
            {
                return ((QS_ThreadPool)this.Parent).IntentRead;
            }
        }
        public string ModelFile
        {
            get
            {
                return ((QS_ThreadPool)this.Parent).ModelFile;
            }
        }
        public string FreqWikiIntentFreqFile
        {
            get
            {
                return ((QS_ThreadPool)this.Parent).FreqWikiIntentFreqFile;
            }
        }
        public QS_Thread(QS_ThreadPool parent, object[] items)
            : base(parent, items)
        {
            this.Segmentor = new QuerySegmentor(this.ModelFile);
        }

        public override void Work()
        {
            this.Segmentor.LoadFreqWikiIntentFreqDicsFromFile(this.FreqWikiIntentFreqFile);

            if (this.SegType == SegmentationType.BaselineSegmentation)
                this.ResultList = this.Segmentor.GetBaselineSegments(this.Items.ToArray<string>());

            else if (this.SegType == SegmentationType.SvmSegmentation)
                this.ResultList = this.Segmentor.GetSvmSegments(this.Items.ToArray<string>());

            //else if (this.SegType == SegmentationType.NlpSvmSegmentation)
            //    this.ResultList = this.Segmentor.GetNlpSvmSegments(this.Items.ToArray<string>());

            //else if (this.SegType == SegmentationType.ConcatNlpSvmSegmentation)
            //    this.ResultList = this.Segmentor.GetConcatNlpSvmSegments(this.Items.ToArray<string>());

            base.Work();
        }
    }

    public enum SegmentationType
    { 
        BaselineSegmentation,
        SvmSegmentation,
        //NlpSvmSegmentation,
        //ConcatNlpSvmSegmentation,
        FineSegmentation,
        CoarseSegmentation
    }

    public enum SegMethodType
    {
        WBN,
        WT,
        BwcTrain,
        WebisTrain,
        Fine,
        Coarse
    }

    public class SegmentAccuracy
    {
        //The properties below use the example: query (“Michael Jordan machine learning”), computer segmentation(“michael jordan” “machine” “learning”) and human labeled segmentation (“Michael jordan” “machine learning”)
        public int QueryCount { get; set; }             //The total input queries/segmentations count. (i.e. sum 1 for each query)
        public int HumanSegmentCount { get; set; }      //The sum of segment count of all human segmentations. For example: there are 2 segments in human segmentation (“michael jordan” “machine learning”)
        public int SegmentCount { get; set; }           //The sum of segment count of all computer segmentations. For example: there are 3 segments in computer segmentation (“michael jordan” “machine” “learning”)
        public int BreakCount { get; set; }             //The sum of break decision count of all queries (Break decision means the break between a pair of two adjacent words, i.e. cut or connect). For example, there are 3 breaks in query ““michael Jordan machine learning”
        public int CorrectQueryCount { get; set; }      //The total exactly correct computer segmentation count compared with the human segmentations. For example: in the above example, computer segmentation != human segmentation (i.e. sum 0 in this query)
        public int CorrectSegmentCount { get; set; }    //The total correct segment count of the computer segmentation compared with the human segmentations. For example: in the above example, computer segmentation has a same segment with human segmentation (“Michael joran”) (i.e. sum 1 in this segmentation)
        public int CorrectBreakCount { get; set; }      //The total correct break count of computer segmentation compared with the human segmentations. For example: in the above example, the series of 3 breaks of computer segmentation is connect/cut/cut, and the series of 3 breaks in human segmentation is connect/cut/connect, so the correct break count in this query is 2. (i.e. sum 2 in this query)

        public SegmentAccuracy()
        {
            QueryCount = 0;
            HumanSegmentCount = 0;
            SegmentCount = 0;
            BreakCount = 0;

            CorrectQueryCount = 0;
            CorrectSegmentCount = 0;
            CorrectBreakCount = 0;
        }

        public static string[] GetHeaders()
        {
            return new string[] { "QueryAcc", "SegPrec", "SegRec", "SegF", "BreakAcc" };
        }

        /// <summary>
        /// Five performance measures: QueryAccuracy, SegmentPrecision, SegmentRecall, SegmentF-Measure and BreakAccuracy
        /// 
        /// QueryAccuracy = CorrectQueryCount / QueryCount (i.e. the accuracy of exactly the same computer segmentations with human segmentations)
        /// SegmentPrecision = CorrectSegmentCount / SegmentCount (i.e. the precision of the correct computer segments)
        /// SegmentRecall = CorrectSegmentCount / HumanSegmentCount (i.e. the recall of the correct computer segments)
        /// SegmentF-Measure = 2 * SegmentPrecision * SegmentRecall / (SegmentPrecision + SegmentRecall) (i.e. the F-measure of the correct computer segments)
        /// BreakAccuracy = CorrectBreakCount / BreakCount (i.e. the correct computer break accuracy)
        /// </summary>
        /// <returns></returns>
        public double[] GetAccuracies()
        {
            double queryAcc = CorrectQueryCount * 1.0 / QueryCount;
            double segPrec = CorrectSegmentCount * 1.0 / SegmentCount;
            double segRec = CorrectSegmentCount * 1.0 / HumanSegmentCount;
            double segF = (segPrec + segRec == 0) ? 0 : (2 * segPrec * segRec / (segPrec + segRec));
            double breakAccu = CorrectBreakCount * 1.0 / BreakCount;

            List<double> accuracies = new List<double>();
            accuracies.Add(queryAcc);
            accuracies.Add(segPrec);
            accuracies.Add(segRec);
            accuracies.Add(segF);
            accuracies.Add(breakAccu);

            return accuracies.ToArray();
        }

        public SegmentAccuracy AddNewSegmentAndReturnInstanceAccuracy(string segmentation, string humanSegmentation)
        {
            ++QueryCount;

            string query = segmentation.Replace("\"", "");
            int wordCount = query.Split(' ').Length;

            Dictionary<int, int> breakCountDic = new Dictionary<int, int>();
            breakCountDic.Add(0, 2);

            var segments = segmentation.Trim('\"').Split(new string[] { "\" \"" }, StringSplitOptions.None);
            var humanSegments = humanSegmentation.Trim('\"').Split(new string[] { "\" \"" }, StringSplitOptions.None);

            int segmentCount = segments.Length;
            int humanSegmentCount = humanSegments.Length;

            int lastBreak = 0;
            SegmentCount += segments.Length;
            foreach (var segment in segments)
            {
                lastBreak += segment.Split(' ').Length;
                if (!breakCountDic.ContainsKey(lastBreak))
                    breakCountDic.Add(lastBreak, 1);
                else
                    ++breakCountDic[lastBreak];
            }

            lastBreak = 0;
            HumanSegmentCount += humanSegments.Length;
            foreach (var segment in humanSegments)
            {
                lastBreak += segment.Split(' ').Length;
                if (!breakCountDic.ContainsKey(lastBreak))
                    breakCountDic.Add(lastBreak, 1);
                else
                    ++breakCountDic[lastBreak];
            }

            var breakCounts = breakCountDic.OrderBy(kv => kv.Key).Select(kv => kv.Value).ToArray();
            int correctSegmentCount = 0;
            for (int i = 1; i < breakCounts.Length; ++i)
            {
                if (breakCounts[i - 1] == 2 && breakCounts[i] == 2)
                {
                    ++correctSegmentCount;
                    ++CorrectSegmentCount;
                }
            }

            int breakCount = wordCount - 1;
            int correctBreakCount = breakCount - breakCounts.Count(v => v == 1);

            if (correctBreakCount == breakCount)
                ++CorrectQueryCount;

            BreakCount += breakCount;
            CorrectBreakCount += correctBreakCount;

            return new SegmentAccuracy() { QueryCount = 1, CorrectQueryCount = correctBreakCount == breakCount ? 1 : 0, SegmentCount = segmentCount, HumanSegmentCount = humanSegmentCount, CorrectSegmentCount = correctSegmentCount, BreakCount = breakCount, CorrectBreakCount = correctBreakCount };
        }
    }

}
