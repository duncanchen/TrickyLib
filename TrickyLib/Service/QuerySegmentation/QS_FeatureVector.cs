using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib;
using System.Reflection;
using TrickyLib.IO;
using TrickyLib.MachineLearning;
using TrickyLib.Extension;
using TrickyLib.Service;

namespace TrickyLib.Service.QuerySegmentation
{
    public class QS_FeatureVector : FeatureVector
    {
        public static HashSet<string> WordAppearFullySplitWords = new HashSet<string>(FileReader.ReadRows(@"E:\v-haowu\QuerySegmentation\Data\OtherFiles\WordAppearFullySplitWords.txt"));
        public static HashSet<string> EndWithNotLastWords = new HashSet<string>(FileReader.ReadRows(@"E:\v-haowu\QuerySegmentation\Data\OtherFiles\EndWithNotLastWords.txt"));
        public static HashSet<string> EndWithAndLastWords = new HashSet<string>(FileReader.ReadRows(@"E:\v-haowu\QuerySegmentation\Data\OtherFiles\EndWithAndLastWords.txt"));
        public static HashSet<string> StartWords = new HashSet<string>(FileReader.ReadRows(@"E:\v-haowu\QuerySegmentation\Data\OtherFiles\StartWords.txt"));
        public static HashSet<string> EndWords = new HashSet<string>(FileReader.ReadRows(@"E:\v-haowu\QuerySegmentation\Data\OtherFiles\EndWords.txt"));
        public static HashSet<string> IndependentWords = new HashSet<string>(FileReader.ReadRows(@"E:\v-haowu\QuerySegmentation\Data\OtherFiles\IndependentWords.txt"));
        public static HashSet<string> NotIndependentWords = new HashSet<string>(FileReader.ReadRows(@"E:\v-haowu\QuerySegmentation\Data\OtherFiles\NotIndependentWords.txt"));
        public static HashSet<string> PrepWords = new HashSet<string>(FileReader.ReadRows(@"E:\v-haowu\QuerySegmentation\Data\OtherFiles\PrepWords.txt"));
        public static HashSet<string> MonthWords = new HashSet<string>(FileReader.ReadRows(@"E:\v-haowu\QuerySegmentation\Data\OtherFiles\MonthWords.txt"));
        public static HashSet<string> PlaceWords = new HashSet<string>(FileReader.ReadRows(@"E:\v-haowu\QuerySegmentation\Data\OtherFiles\PlaceWords.txt"));
        public static Dictionary<string, List<string>> ConditionSingleSegWords = FileReader.ReadToDic(@"E:\v-haowu\QuerySegmentation\Data\OtherFiles\ConditionSingleSegWords.txt", 0);

        public static int MaxSeg = 10;
        public static int MaxSegWord = 6;
        //public static long TotalWebNGramCount = nGramHandler.GetTotalCount();
        public static long TotalWebNGramCount = 1478154044558;

        public QSinfo Target { get; set; }
        public string HumanLabeledSegmentation { get; set; }
        public QS_FeatureVector(object category, int qid, QSinfo target, string humanLabeledSegmentation = "")
            : base(category, qid)
        {
            this.Target = target;
            this.HumanLabeledSegmentation = humanLabeledSegmentation;
        }

        public override string ToString()
        {
            string baseVector = base.ToString();
            string[] lqvc = FeatureFileProcessor.SplitLabelQidVectorComment(baseVector);

            baseVector += " ##TAB## " + this.Target.Query + " ##TAB## " + this.Target.Segmentation + " ##TAB## " + this.HumanLabeledSegmentation;
            return baseVector;
        }

        public Segmentation ConvertToSegmentation()
        {
            List<int> segmentIndexList = new List<int>();
            foreach (var segment in this.Target.Segments)
                if (segmentIndexList.Count == 0)
                    segmentIndexList.Add(segment.GetWordCount());
                else
                    segmentIndexList.Add(segment.GetWordCount() + segmentIndexList.Last());
            return new Segmentation()
            {
                CleanedQuery = this.Target.Query,
                Rank = this.Target.NewRank,
                Score = this.Target.Score,
                SegmentIndexList = segmentIndexList
            };
        }

        #region Priority
        public Feature GetWikiModelFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool | ValueTypes.Double);

            bool scoreNegtive = false;
            bool hasLittle = false;
            double[] wikiArray = new double[MaxSegWord];
            double[] nonWikiArray = new double[MaxSegWord];
            long top1Score = this.Target.GroupPointer.First().Score;

            if (this.Target.Score < 0)
                scoreNegtive = true;
            else
                scoreNegtive = false;

            if (top1Score > 0)
            {
                foreach (var seg in this.Target.Segments)
                {
                    int wordCount = seg.GetWordCount();
                    if (this.Target.WikiTitles.Contains(seg))
                    {
                        if (wordCount >= MaxSegWord)
                            wikiArray[MaxSegWord - 1] += this.Target.SegmentConDic[seg] * 1.0 / top1Score;
                        else
                            wikiArray[wordCount - 1] += this.Target.SegmentConDic[seg] * 1.0 / top1Score;
                    }
                    else
                    {
                        if (wordCount >= MaxSegWord)
                            nonWikiArray[MaxSegWord - 1] += this.Target.SegmentConDic[seg] * 1.0 / top1Score;
                        else
                            nonWikiArray[wordCount - 1] += this.Target.SegmentConDic[seg] * 1.0 / top1Score;
                    }
                }
            }


            for (int i = 0; i < wikiArray.Length; i++)
            {
                if (Math.Abs(wikiArray[i]) < 0.001 && wikiArray[i] != 0)
                {
                    wikiArray[i] = 0;
                    hasLittle = true;
                }
                if (Math.Abs(nonWikiArray[i]) < 0.001 && wikiArray[i] != 0)
                {
                    nonWikiArray[i] = 0;
                    hasLittle = true;
                }
            }

            feature.ValueList.Add(scoreNegtive);
            feature.ValueList.Add(hasLittle);
            foreach (var score in wikiArray)
                feature.ValueList.Add(score);
            foreach (var score in nonWikiArray)
                feature.ValueList.Add(score);

            return feature;
        }

        public Feature GetFirstSegWikiModelFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool | ValueTypes.Double);

            bool scoreNegtive = false;
            bool hasLittle = false;
            double[] wikiArray = new double[MaxSegWord];
            double[] nonWikiArray = new double[MaxSegWord];
            long top1Score = this.Target.GroupPointer.Select(i => i.SegmentConDic[i.Segments[0]]).Max();

            if (this.Target.SegmentConDic[this.Target.Segments[0]] < 0)
                scoreNegtive = true;

            else if (top1Score > 0)
                scoreNegtive = false;

            string seg = this.Target.Segments[0];
            int wordCount = seg.GetWordCount();

            if (top1Score > 0)
            {
                if (this.Target.WikiTitles.Contains(seg))
                {
                    if (wordCount >= MaxSegWord)
                        wikiArray[MaxSegWord - 1] += this.Target.SegmentConDic[seg] * 1.0 / top1Score;
                    else
                        wikiArray[wordCount - 1] += this.Target.SegmentConDic[seg] * 1.0 / top1Score;
                }
                else
                {
                    if (wordCount >= MaxSegWord)
                        nonWikiArray[MaxSegWord - 1] += this.Target.SegmentConDic[seg] * 1.0 / top1Score;
                    else
                        nonWikiArray[wordCount - 1] += this.Target.SegmentConDic[seg] * 1.0 / top1Score;
                }
            }


            for (int i = 0; i < wikiArray.Length; i++)
            {
                if (Math.Abs(wikiArray[i]) < 0.001 && wikiArray[i] != 0)
                {
                    wikiArray[i] = 0;
                    hasLittle = true;
                }
                if (Math.Abs(nonWikiArray[i]) < 0.001 && wikiArray[i] != 0)
                {
                    nonWikiArray[i] = 0;
                    hasLittle = true;
                }
            }

            feature.ValueList.Add(scoreNegtive);
            feature.ValueList.Add(hasLittle);
            foreach (var score in wikiArray)
                feature.ValueList.Add(score);
            foreach (var score in nonWikiArray)
                feature.ValueList.Add(score);

            return feature;
        }

        //public Feature GetLastSegWikiModelFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool | ValueTypes.Double);

        //    bool scoreNegtive = false;
        //    bool hasLittle = false;
        //    double[] wikiArray = new double[MaxSegWord];
        //    double[] nonWikiArray = new double[MaxSegWord];
        //    long top1Score = this.Target.GroupPointer.Select(i => i.SegmentConDic[i.Segments[i.Segments.Count - 1]]).Max();

        //    if (this.Target.SegmentConDic[this.Target.Segments[this.Target.Segments.Count - 1]] < 0)
        //        scoreNegtive = true;

        //    else if (top1Score > 0)
        //        scoreNegtive = false;

        //    string seg = this.Target.Segments[this.Target.Segments.Count - 1];
        //    int wordCount = seg.GetWordCount();

        //    if (top1Score > 0)
        //    {
        //        if (this.Target.IsWikiTitleDic[seg])
        //        {
        //            if (wordCount >= MaxSegWord)
        //                wikiArray[MaxSegWord - 1] += this.Target.SegmentConDic[seg] * 1.0 / top1Score;
        //            else
        //                wikiArray[wordCount - 1] += this.Target.SegmentConDic[seg] * 1.0 / top1Score;
        //        }
        //        else
        //        {
        //            if (wordCount >= MaxSegWord)
        //                nonWikiArray[MaxSegWord - 1] += this.Target.SegmentConDic[seg] * 1.0 / top1Score;
        //            else
        //                nonWikiArray[wordCount - 1] += this.Target.SegmentConDic[seg] * 1.0 / top1Score;
        //        }
        //    }


        //    for (int i = 0; i < wikiArray.Length; i++)
        //    {
        //        if (Math.Abs(wikiArray[i]) < 0.001 && wikiArray[i] != 0)
        //        {
        //            wikiArray[i] = 0;
        //            hasLittle = true;
        //        }
        //        if (Math.Abs(nonWikiArray[i]) < 0.001 && wikiArray[i] != 0)
        //        {
        //            nonWikiArray[i] = 0;
        //            hasLittle = true;
        //        }
        //    }

        //    feature.ValueList.Add(scoreNegtive);
        //    feature.ValueList.Add(hasLittle);
        //    foreach (var score in wikiArray)
        //        feature.ValueList.Add(score);
        //    foreach (var score in nonWikiArray)
        //        feature.ValueList.Add(score);

        //    return feature;
        //}

        #endregion

        #region First/Last segment

        /////// <summary>
        /////// The first seg words features, like starts with "how", etc.
        /////// </summary>
        /////// <returns></returns>
        ////public Feature GetFirstSegWordFeature()
        ////{
        ////    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);

        ////    string firstSeg = this.Target.Segments[0];
        ////    List<object> firstSegFeatures = new List<object>();

        ////    foreach (string firstword in FirstWords)
        ////        firstSegFeatures.Add(firstSeg.StartsWith(firstword, StringComparison.InvariantCultureIgnoreCase));

        ////    feature.ValueList.AddRange(firstSegFeatures);
        ////    return feature;
        ////}

        /////// <summary>
        /////// The last seg words features, like end with "free", etc.
        /////// </summary>
        /////// <returns></returns>
        ////public Feature GetlastSegWordFeature()
        ////{
        ////    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);

        ////    string lastSeg = this.Target.Segments[this.Target.Segments.Count - 1];
        ////    List<object> lastSegFeatures = new List<object>();

        ////    foreach (string lastword in LastWords)
        ////        lastSegFeatures.Add(lastSeg.EndsWith(lastword, StringComparison.InvariantCultureIgnoreCase));

        ////    feature.ValueList.AddRange(lastSegFeatures);
        ////    return feature;
        ////}

        ///// <summary>
        ///// The first segment score
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetFirstSegFreqFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);
        //    long firstSegScore = this.Target.SubQueryFreqDic[this.Target.Segments[0]];

        //    feature.ValueList.Add(firstSegScore);
        //    return feature;
        //}

        ///// <summary>
        ///// The last segment score
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetLastSegFreqFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);
        //    long lastSegScore = this.Target.SubQueryFreqDic[this.Target.Segments.Last()];

        //    feature.ValueList.Add(lastSegScore);
        //    return feature;
        //}

        /////// <summary>
        /////// The first segment score ratio
        /////// </summary>
        /////// <returns></returns>
        ////public Feature GetFirstSegScoreRatioFeature()
        ////{
        ////    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Double);
        ////    if (this.Target.Score != 0)
        ////    {
        ////        double firstSegScoreRatio = Convert.ToDouble(this.Target.SegmentConDic[this.Target.Segments[0]]) / this.Target.Score;
        ////        feature.ValueList.Add(firstSegScoreRatio);
        ////    }
        ////    else
        ////        feature.ValueList.Add(0);

        ////    return feature;
        ////}

        /////// <summary>
        /////// The last segment score ratio
        /////// </summary>
        /////// <returns></returns>
        ////public Feature GetLastSegScoreRatioFeature()
        ////{
        ////    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Double);

        ////    if (this.Target.Score != 0)
        ////    {
        ////        double lastSegScoreRatio = Convert.ToDouble(this.Target.SegmentConDic[this.Target.Segments.Last()]) / this.Target.Score;
        ////        feature.ValueList.Add(lastSegScoreRatio);
        ////    }
        ////    else
        ////        feature.ValueList.Add(0);

        ////    return feature;
        ////}

        ///// <summary>
        ///// The length of the first segment
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetFirstSegLengthFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Int);
        //    feature.ValueList.Add(this.Target.Segments[0].GetWords().Length);

        //    return feature;
        //}

        ///// <summary>
        ///// The length of the last segment
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetLastSegLengthFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Int);
        //    feature.ValueList.Add(this.Target.Segments[this.Target.Segments.Count - 1].GetWords().Length);

        //    return feature;
        //}
        #endregion

        #region Rank

        /// <summary>
        /// The rank feature
        /// </summary>
        /// <returns></returns>
        public Feature GetRankFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Int);
            feature.ValueList.Add(this.Target.Rank + 1);

            return feature;
        }
        #endregion

        #region Segments Frequency

        ///// <summary>
        ///// The freq array of the segments when length equals to 1,2,3,..., MaxSeg, MaxSeg + 1
        ///// This feature need to be consider use it or not
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetSegmentsFreqArrayFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);
        //    List<object> segFreqs = new List<object>();

        //    for (int i = 0; i < this.Target.SegmentCount; i++)
        //    {
        //        if (i <= MaxSeg)
        //            segFreqs.Add(this.Target.SubQueryFreqDic[this.Target.Segments[i]]);
        //        else
        //            segFreqs[segFreqs.Count - 1] = (long)(segFreqs[segFreqs.Count - 1]) + this.Target.SubQueryFreqDic[this.Target.Segments[i]];
        //    }

        //    for (int i = 1; i <= MaxSeg + 1; i++)
        //    {
        //        if (i == this.Target.SegmentCount || i == MaxSeg + 1 && this.Target.SegmentCount > MaxSeg)
        //            feature.ValueList.AddRange(segFreqs);

        //        else if (i != this.Target.SegmentCount)
        //            for (int j = 0; j < i; j++)
        //                feature.ValueList.Add(0);
        //    }

        //    return feature;
        //}

        ///// <summary>
        ///// The score contribution array of the segments when length equals to 1,2,3,...,MaxSeg, MaxSeg + 1
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetSegmentsConArrayFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);
        //    List<object> segCons = new List<object>();

        //    for (int i = 0; i < this.Target.SegmentCount; i++)
        //    {
        //        if (i <= MaxSeg)
        //            segCons.Add(this.Target.SegmentConDic[this.Target.Segments[i]]);
        //        else
        //            segCons[segCons.Count - 1] = (long)(segCons[segCons.Count - 1]) + this.Target.SegmentConDic[this.Target.Segments[i]];
        //    }

        //    for (int i = 1; i <= MaxSeg + 1; i++)
        //    {
        //        if (i == this.Target.SegmentCount || i == MaxSeg + 1 && this.Target.SegmentCount > MaxSeg)
        //            feature.ValueList.AddRange(segCons);

        //        else if (i != this.Target.SegmentCount)
        //            for (int j = 0; j < i; j++)
        //                feature.ValueList.Add(0);
        //    }

        //    return feature;
        //}

        //public Feature GetQueryCountFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);

        //    long sum = 0;

        //    foreach (var seg in this.Target.Segments)
        //    {
        //        long freq = this.Target.SubQueryIntentCountDic[seg].First;
        //        double weight = Math.Pow(seg.GetWordCount(), seg.GetWordCount());

        //        if (weight <= 1)
        //            continue;

        //        if (freq <= 0)
        //        {
        //            sum = 0;
        //            break;
        //        }
        //        else
        //            sum += Convert.ToInt64(freq * weight);
        //    }

        //    feature.ValueList.Add(sum);
        //    return feature;
        //}

        //public Feature GetQueryCountAppearanceFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Double);

        //    int count = 0;

        //    foreach (var seg in this.Target.Segments)
        //    {
        //        if (seg.GetWordCount() <= 1)
        //            continue;

        //        long freq = this.Target.SubQueryIntentCountDic[seg].First;

        //        if (freq == 0)
        //            count++;
        //    }

        //    double ratio = Convert.ToDouble(count) / this.Target.SegmentCount;
        //    feature.ValueList.Add(ratio);
        //    return feature;
        //}

        //public Feature GetFirstQueryFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);
        //    bool exists = false;
        //    string firstSeg = this.Target.Segments[0];
        //    if (this.Target.SubQueryIntentCountDic[firstSeg].Third > 0)
        //        exists = true;

        //    feature.ValueList.Add(exists);
        //    return feature;
        //}

        //public Feature GetLastQueryFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);
        //    bool exists = false;
        //    string LastSeg = this.Target.Segments.Last();
        //    if (this.Target.SubQueryIntentCountDic[LastSeg].Forth > 0)
        //        exists = true;

        //    feature.ValueList.Add(exists);
        //    return feature;
        //}

        //public Feature GetQueryCountAllAppearanceFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);

        //    bool allAppear = true;

        //    if (this.Target.WordCount == this.Target.SegmentCount)
        //        allAppear = false;
        //    else
        //    {
        //        foreach (var seg in this.Target.Segments)
        //        {
        //            if (seg.GetWordCount() <= 1)
        //                continue;

        //            long freq = this.Target.SubQueryIntentCountDic[seg].First;

        //            if (freq < 500)
        //            {
        //                allAppear = false;
        //                break;
        //            }
        //        }
        //    }

        //    feature.ValueList.Add(allAppear);
        //    return feature;
        //}


        //public Feature GetQueryIntentCountFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);

        //    long sum = 0;

        //    foreach (var seg in this.Target.Segments)
        //    {
        //        long freq = this.Target.SubQueryIntentCountDic[seg].Second;
        //        double weight = Math.Pow(seg.GetWordCount(), seg.GetWordCount());

        //        if (weight <= 1)
        //            continue;

        //        if (freq <= 0)
        //        {
        //            sum = 0;
        //            break;
        //        }
        //        else
        //            sum += Convert.ToInt64(freq * weight);
        //    }

        //    feature.ValueList.Add(sum);
        //    return feature;
        //}

        public Feature GetQueryIntentCountAppearanceFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Int);

            int count = 0;

            foreach (var seg in this.Target.Segments)
            {
                if (seg.GetWordCount() <= 1 || !this.Target.SubQueryIntentCountDic.ContainsKey(seg))
                    continue;

                long freq = this.Target.SubQueryIntentCountDic[seg];

                if (freq > 1000)
                    count++;
            }

            feature.ValueList.Add(count);
            return feature;
        }

        public Feature GetQueryIntentCountAllAppearanceFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);

            bool allAppear = true;

            if (this.Target.WordCount == this.Target.SegmentCount)
                allAppear = false;
            else
            {
                foreach (var seg in this.Target.Segments)
                {
                    if (seg.GetWordCount() <= 1 || !this.Target.SubQueryIntentCountDic.ContainsKey(seg))
                        continue;

                    long freq = this.Target.SubQueryIntentCountDic[seg];

                    if (freq < 500)
                    {
                        allAppear = false;
                        break;
                    }
                }
            }

            feature.ValueList.Add(allAppear);
            return feature;
        }


        //public Feature GetQueryBeforeAddCountFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);

        //    long sum = 0;

        //    foreach (var seg in this.Target.Segments)
        //    {
        //        long freq = this.Target.SubQueryIntentCountDic[seg].Third;
        //        double weight = Math.Pow(seg.GetWordCount(), seg.GetWordCount());

        //        if (weight <= 1)
        //            continue;

        //        if (freq <= 0)
        //        {
        //            sum = 0;
        //            break;
        //        }
        //        else
        //            sum += Convert.ToInt64(freq * weight);
        //    }

        //    feature.ValueList.Add(sum);
        //    return feature;
        //}

        //public Feature GetQueryBeforeAddCountAppearanceFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Int);

        //    int count = 0;

        //    foreach (var seg in this.Target.Segments)
        //    {
        //        if (seg.GetWordCount() <= 1)
        //            continue;

        //        long freq = this.Target.SubQueryIntentCountDic[seg].First;

        //        if (freq > 0)
        //            count++;
        //    }

        //    feature.ValueList.Add(count);
        //    return feature;
        //}

        //public Feature GetQueryAfterAddCountFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);

        //    long sum = 0;

        //    foreach (var seg in this.Target.Segments)
        //    {
        //        long freq = this.Target.SubQueryIntentCountDic[seg].Forth;
        //        double weight = Math.Pow(seg.GetWordCount(), seg.GetWordCount());

        //        if (weight <= 1)
        //            continue;

        //        if (freq <= 0)
        //        {
        //            sum = 0;
        //            break;
        //        }
        //        else
        //            sum += Convert.ToInt64(freq * weight);
        //    }

        //    feature.ValueList.Add(sum);
        //    return feature;
        //}

        //public Feature GetQueryAfterAddCountAppearanceFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Int);

        //    int count = 0;

        //    foreach (var seg in this.Target.Segments)
        //    {
        //        if (seg.GetWordCount() <= 1)
        //            continue;

        //        long freq = this.Target.SubQueryIntentCountDic[seg].Forth;

        //        if (freq > 0)
        //            count++;
        //    }

        //    feature.ValueList.Add(count);
        //    return feature;
        //}

        //public Feature GetQuery4AppearanceFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Int);

        //    int count = 0;

        //    foreach (var seg in this.Target.Segments)
        //    {
        //        if (seg.GetWordCount() <= 1)
        //            continue;

        //        long freq1 = this.Target.SubQueryIntentCountDic[seg].First;
        //        long freq2 = this.Target.SubQueryIntentCountDic[seg].Second;
        //        long freq3 = this.Target.SubQueryIntentCountDic[seg].Third;
        //        long freq4 = this.Target.SubQueryIntentCountDic[seg].Forth;

        //        if (freq1 > 1000)
        //            count++;
        //        else if (freq2 > 1000)
        //            count++;
        //        else if (freq3 > 0)
        //            count++;
        //        else if (freq4 > 0)
        //            count++;
        //    }

        //    feature.ValueList.Add(count);
        //    return feature;
        //}

        #endregion

        #region Score
        /// <summary>
        /// The value is the Score
        /// </summary>
        /// <returns></returns>
        public Feature GetScoreFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Long);
            feature.ValueList.Add(this.Target.Score);

            return feature;
        }

        /// <summary>
        /// The ratio of the score in the group
        /// </summary>
        /// <returns></returns>
        public Feature GetScoreRatioFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool | ValueTypes.Double);

            long sum = this.Target.GroupPointer.Sum(qs => qs.Score);
            double scoreRatio = 0;

            if (sum > 0)
                scoreRatio = Convert.ToDouble(this.Target.Score) / Convert.ToDouble(sum);
            else
                scoreRatio = 0;

            feature.ValueList.Add(scoreRatio);

            return feature;
        }

        /// <summary>
        /// The average score on each segment
        /// </summary>
        /// <returns></returns>
        public Feature GetAvgSegScoreFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Long);
            long avgSegScore = this.Target.Score / this.Target.SegmentCount;

            feature.ValueList.Add(avgSegScore);
            return feature;
        }

        /// <summary>
        /// The difference score to upper QS: upper.Score - this.Target.Score
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public Feature GetUpperDiffScoreFeature()
        {
            QSinfo upperQS = null;
            if (this.Target.Rank <= 0)
                upperQS = this.Target;
            else
                upperQS = this.Target.GroupPointer.ElementAt(this.Target.Rank - 1);

            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Long);
            long upperDiffScore = upperQS.Score - this.Target.Score;

            feature.ValueList.Add(upperDiffScore);
            return feature;
        }

        ///// <summary>
        ///// The difference score to lower QS: this.Target.Score - lower.Score
        ///// </summary>
        ///// <param name="group"></param>
        ///// <returns></returns>
        //public Feature GetLowerDiffScoreFeature()
        //{
        //    QSinfo lowerQS = null;
        //    if (this.Target.Rank >= this.Target.GroupPointer.Count - 1)
        //        lowerQS = this.Target;
        //    else
        //        lowerQS = this.Target.GroupPointer[this.Target.Rank + 1];

        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);
        //    long lowerDiffScore = this.Target.Score - lowerQS.Score;

        //    feature.ValueList.Add(lowerDiffScore);
        //    return feature;
        //}
        #endregion

        #region Segment Count
        /// <summary>
        /// The segment # feature
        /// </summary>
        /// <returns></returns>
        public Feature GetSegCountFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Int);
            feature.ValueList.Add(this.Target.SegmentCount);

            return feature;
        }

        ///// <summary>
        ///// The segment # array in different length of words
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetSegWordCountArrayFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Int);
        //    object[] SegWordCounts = new object[MaxSegWord + 1];

        //    for (int i = 0; i <= MaxSegWord; i++)
        //        SegWordCounts[i] = 0;

        //    foreach (string seg in this.Target.Segments)
        //    {
        //        int wordCount = seg.GetWords().Length;
        //        if (wordCount <= MaxSegWord)
        //            SegWordCounts[wordCount - 1] = (int)SegWordCounts[wordCount - 1] + 1;
        //        else
        //            SegWordCounts[MaxSegWord] = (int)SegWordCounts[MaxSegWord] + 1;
        //    }

        //    feature.ValueList.AddRange(SegWordCounts);
        //    return feature;
        //}

        /// <summary>
        /// The consecutive 1 word seg count in a row
        /// </summary>
        /// <returns></returns>
        public Feature GetOneWordSegRowCountFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Int);

            int maxLength = 0;
            int currentLength = 0;

            foreach (string seg in this.Target.Segments)
            {
                int wordCount = seg.GetWords().Length;
                if (wordCount > 1)
                {
                    if (maxLength < currentLength)
                        maxLength = currentLength;

                    currentLength = 0;
                }
                else
                    currentLength++;
            }

            if (maxLength < currentLength)
                maxLength = currentLength;

            feature.ValueList.Add(maxLength);
            return feature;
        }

        //public Feature GetThreeMoreSegCountFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);
        //    feature.ValueList.Add(this.Target.SegmentCount >= 3);

        //    return feature;
        //}

        public Feature GetOnlyOneSegFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            feature.ValueList.Add(this.Target.SegmentCount == 1);

            return feature;
        }

        #endregion

        #region Segment Length

        /// <summary>
        /// The avg segment length
        /// </summary>
        /// <returns></returns>
        public Feature GetAvgSegLengthFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Int);
            feature.ValueList.Add(this.Target.WordCount / this.Target.SegmentCount);

            return feature;
        }

        public Feature Get121Length4Feature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);

            bool isFour = this.Target.Query.GetWordCount() == 4;
            bool is121 = false;

            if (isFour && this.Target.SegmentCount == 3)
            {
                if (this.Target.Segments[0].GetWordCount() == 1
                    && this.Target.Segments[1].GetWordCount() == 2
                    && this.Target.Segments[2].GetWordCount() == 1)
                    is121 = true;
            }

            feature.ValueList.Add(isFour && is121);
            return feature;
        }

        public Feature Get22Length4Feature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);

            bool isFour = this.Target.Query.GetWordCount() == 4;
            bool is22 = false;

            if (isFour && this.Target.SegmentCount == 2)
            {
                if (this.Target.Segments[0].GetWordCount() == 2
                    && this.Target.Segments[1].GetWordCount() == 2)
                    is22 = true;
            }

            feature.ValueList.Add(isFour && is22);
            return feature;
        }

        //public Feature Get23Length5Feature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);

        //    bool isFour = this.Target.Query.GetWordCount() == 5;
        //    bool is23 = false;

        //    if (isFour && this.Target.SegmentCount == 2)
        //    {
        //        if ((this.Target.Segments[0].GetWordCount() == 2
        //            && this.Target.Segments[1].GetWordCount() == 3) 
        //            || this.Target.Segments[0].GetWordCount() == 3
        //            && this.Target.Segments[1].GetWordCount() == 2)
        //            is23 = true;
        //    }

        //    feature.ValueList.Add(isFour && is23);
        //    return feature;
        //}

        //public Feature GetLowUpLengthFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);

        //    bool lowUp = false;
        //    if (this.Target.SegmentCount >= 3)
        //    {
        //        for (int i = 1; i < this.Target.SegmentCount - 1; i++)
        //        {
        //            bool low = false;
        //            bool up = false;

        //            for (int j = 0; j < i; j++)
        //            {
        //                if (this.Target.Segments[j].GetWordCount() > this.Target.Segments[i].GetWordCount())
        //                {
        //                    low = true;
        //                    break;
        //                }
        //            }

        //            if (low)
        //            {
        //                for (int j = i + 1; j < this.Target.SegmentCount; j++)
        //                {
        //                    if (this.Target.Segments[j].GetWordCount() > this.Target.Segments[i].GetWordCount())
        //                    {
        //                        up = true;
        //                        break;
        //                    }
        //                }
        //            }

        //            lowUp = low && up;
        //            if (lowUp)
        //                break;
        //        }
        //    }

        //    feature.ValueList.Add(lowUp);
        //    return feature;
        //}

        //public Feature GetMaxNearLengthDiffFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Int);

        //    int maxNearDiff = 0;
        //    for (int i = 0; i < this.Target.SegmentCount - 1; i++)
        //    {
        //        int nearDiff = Math.Abs(this.Target.Segments[i + 1].GetWordCount() - this.Target.Segments[i].GetWordCount());
        //        if (maxNearDiff < nearDiff)
        //            maxNearDiff = nearDiff;
        //    }

        //    feature.ValueList.Add(maxNearDiff);
        //    return feature;
        //}
        //public Feature GetMaxLengthDiffFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Int);

        //    int max = 0;
        //    int min = int.MaxValue;

        //    foreach (var seg in this.Target.Segments)
        //    {
        //        int length = seg.GetWordCount();

        //        if (max < length)
        //            max = length;

        //        if (min > length)
        //            min = length;
        //    }

        //    feature.ValueList.Add(max - min);
        //    return feature;
        //}

        ///// <summary>
        ///// The segment length array
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetSegLengthArrayFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Int);
        //    List<object> segLengths = new List<object>();

        //    foreach (string seg in this.Target.Segments)
        //    {
        //        int wordCount = seg.GetWords().Length;

        //        if (segLengths.Count <= MaxSegWord)
        //            segLengths.Add(wordCount);
        //        else
        //            segLengths[segLengths.Count - 1] = (int)segLengths[segLengths.Count - 1] + wordCount;
        //    }

        //    for (int i = 1; i <= MaxSeg + 1; i++)
        //    {
        //        if (i == this.Target.SegmentCount || i == MaxSeg + 1 && this.Target.SegmentCount > MaxSeg)
        //            feature.ValueList.AddRange(segLengths);

        //        else if (i != this.Target.SegmentCount)
        //            for (int j = 0; j < i; j++)
        //                feature.ValueList.Add(0);
        //    }

        //    return feature;
        //}
        #endregion

        #region Special Frequency

        public Feature GetMaxSegMutualInfoFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Double);
            decimal maxSegMI = decimal.MinValue;

            if (this.Target.SegmentCount > 1)
            {
                for (int i = 0; i < this.Target.SegmentCount - 1; i++)
                {
                    double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i]]);
                    double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i + 1]]);
                    double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i] + " " + this.Target.Segments[i + 1]]);

                    double mi = 0;
                    if (freq1 != 0 && freq2 != 0 && freq12 != 0)
                        mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

                    if (maxSegMI < Convert.ToDecimal(mi))
                        maxSegMI = Convert.ToDecimal(mi);
                }
            }
            else
                maxSegMI = 0;

            if (maxSegMI > decimal.MinValue)
                feature.ValueList.Add(maxSegMI);
            else
                feature.ValueList.Add(Convert.ToDecimal(0));

            return feature;
        }
        //public Feature GetMaxSegMutualInfoFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Decimal);
        //    List<decimal> mutialInfos = new List<decimal>();

        //    if (this.Target.SegmentCount > 1)
        //    {
        //        for (int i = 0; i < this.Target.SegmentCount - 1; i++)
        //        {
        //            double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i]]);
        //            double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i + 1]]);
        //            double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i] + " " + this.Target.Segments[i + 1]]);

        //            if (freq1 != 0 && freq2 != 0 && freq12 != 0)
        //            {
        //                double mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

        //                if (i < MaxSeg || mutialInfos.Count <= 0)
        //                    mutialInfos.Add(Convert.ToDecimal(mi));
        //                else
        //                    mutialInfos[mutialInfos.Count - 1] = mutialInfos[mutialInfos.Count - 1] < Convert.ToDecimal(mi) ? Convert.ToDecimal(mi) : mutialInfos[mutialInfos.Count - 1];
        //            }
        //            else if (i < MaxSeg)
        //                mutialInfos.Add(Convert.ToDecimal(0));
        //        }
        //    }
        //    else
        //        mutialInfos.Add(Convert.ToDecimal(0));

        //    feature.ValueList.Add(mutialInfos.Max());

        //    return feature;
        //}

        public Feature GetMaxSepWordsMutualInfoFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Double);
            decimal maxSepWordMI = decimal.MinValue;

            if (this.Target.SegmentCount > 1)
            {
                for (int i = 0; i < this.Target.SegmentCount - 1; i++)
                {
                    string[] words1 = this.Target.Segments[i].GetWords();
                    string[] words2 = this.Target.Segments[i + 1].GetWords();

                    double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[words1[words1.Length - 1]]);
                    double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[words2[0]]);
                    double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[words1[words1.Length - 1] + " " + words2[0]]);

                    double mi = 0;
                    if (freq1 != 0 && freq2 != 0 && freq12 != 0)
                        mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

                    if (maxSepWordMI < Convert.ToDecimal(mi))
                        maxSepWordMI = Convert.ToDecimal(mi);
                }
            }

            if (maxSepWordMI > decimal.MinValue)
                feature.ValueList.Add(maxSepWordMI);
            else
                feature.ValueList.Add(Convert.ToDecimal(0));

            return feature;
        }
        //public Feature GetMaxSepWordsMutualInfoFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Decimal);
        //    List<decimal> mutialInfos = new List<decimal>();

        //    if (this.Target.SegmentCount > 1)
        //    {
        //        for (int i = 0; i < this.Target.SegmentCount - 1; i++)
        //        {
        //            string[] words1 = this.Target.Segments[i].GetWords();
        //            string[] words2 = this.Target.Segments[i + 1].GetWords();

        //            double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[words1[words1.Length - 1]]);
        //            double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[words2[0]]);
        //            double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[words1[words1.Length - 1] + " " + words2[0]]);

        //            if (freq1 != 0 && freq2 != 0 && freq12 != 0)
        //            {
        //                double mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

        //                if (i < MaxSeg || mutialInfos.Count <= 0)
        //                    mutialInfos.Add(Convert.ToDecimal(mi));
        //                else
        //                    mutialInfos[mutialInfos.Count - 1] = mutialInfos[mutialInfos.Count - 1] < Convert.ToDecimal(mi) ? Convert.ToDecimal(mi) : mutialInfos[mutialInfos.Count - 1];
        //            }
        //            else if (i < MaxSeg)
        //                mutialInfos.Add(Convert.ToDecimal(0));
        //        }
        //    }
        //    else
        //        mutialInfos.Add(Convert.ToDecimal(0));

        //    feature.ValueList.Add(mutialInfos.Max());
        //    return feature;
        //}

        //public Feature GetMinInnerMutualInfoFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Decimal);
        //    decimal minInnerMI = decimal.MaxValue;

        //    for (int i = 0; i < this.Target.SegmentCount; i++)
        //    {
        //        string[] words = this.Target.Segments[i].GetWords();

        //        if (words.Length > 1)
        //        {
        //            for (int j = 0; j < words.Length - 1; j++)
        //            {
        //                double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[words[j]]);
        //                double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[words[j + 1]]);
        //                double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[words[j] + " " + words[j + 1]]);

        //                double mi = 0;
        //                if (freq1 != 0 && freq2 != 0 && freq12 != 0)
        //                    mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

        //                if (minInnerMI > Convert.ToDecimal(mi))
        //                    minInnerMI = Convert.ToDecimal(mi);
        //            }
        //        }
        //    }

        //    if (minInnerMI < decimal.MaxValue)
        //        feature.ValueList.Add(minInnerMI);
        //    else
        //        feature.ValueList.Add(Convert.ToDecimal(0));

        //    return feature;
        //}

        public Feature GetMinInnerMutualInfoFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Double);
            List<decimal> innerMutialInfos = new List<decimal>();

            for (int i = 0; i < this.Target.SegmentCount; i++)
            {
                string[] words = this.Target.Segments[i].GetWords();
                double innerMinMI = int.MaxValue;

                if (words.Length <= 1)
                    continue;
                else
                    for (int j = 0; j < words.Length - 1; j++)
                    {
                        double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[words[j]]);
                        double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[words[j + 1]]);
                        double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[words[j] + " " + words[j + 1]]);

                        if (freq1 != 0 && freq2 != 0 && freq12 != 0)
                        {
                            double mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

                            if (innerMinMI > mi)
                                innerMinMI = mi;
                        }
                    }

                if (innerMinMI < int.MaxValue)
                {
                    if (i <= MaxSeg || innerMutialInfos.Count <= 0)
                        innerMutialInfos.Add(Convert.ToDecimal(innerMinMI));
                    else
                        innerMutialInfos[innerMutialInfos.Count - 1] = innerMutialInfos[innerMutialInfos.Count - 1] > Convert.ToDecimal(innerMinMI) ? Convert.ToDecimal(innerMinMI) : innerMutialInfos[innerMutialInfos.Count - 1];
                }
            }

            if (innerMutialInfos.Count <= 0)
                feature.ValueList.Add(Convert.ToDecimal(0));
            else
                feature.ValueList.Add(innerMutialInfos.Min());

            return feature;
        }

        public Feature GetMaxSepCollapsedWordsFreqFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Long);
            long maxSepCoWordFreq = 0;

            if (this.Target.SegmentCount > 1)
            {
                for (int i = 0; i < this.Target.SegmentCount - 1; i++)
                {
                    string[] words1 = this.Target.Segments[i].GetWords();
                    string[] words2 = this.Target.Segments[i + 1].GetWords();

                    string collapsedWord = words1[words1.Length - 1] + words2[0];
                    long freq = this.Target.SubQueryFreqDic[collapsedWord];

                    if (maxSepCoWordFreq < freq)
                        maxSepCoWordFreq = freq;
                }
            }

            feature.ValueList.Add(maxSepCoWordFreq);
            return feature;
        }
        //public Feature GetMaxSepCollapsedWordsFreqFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);
        //    List<long> freqs = new List<long>();

        //    if (this.Target.SegmentCount > 1)
        //    {
        //        for (int i = 0; i < this.Target.SegmentCount - 1; i++)
        //        {
        //            string[] words1 = this.Target.Segments[i].GetWords();
        //            string[] words2 = this.Target.Segments[i + 1].GetWords();

        //            string collapsedWord = words1[words1.Length - 1] + words2[0];
        //            long freq = this.Target.SubQueryFreqDic[collapsedWord];

        //            if (i < MaxSeg || freqs.Count <= 0)
        //                freqs.Add(freq);
        //            else
        //                freqs[freqs.Count - 1] = freqs[freqs.Count - 1] < freq ? freq : freqs[freqs.Count - 1];
        //        }
        //    }
        //    else
        //        freqs.Add(Convert.ToInt64(0));

        //    feature.ValueList.Add(freqs.Max());
        //    return feature;
        //}

        public Feature GetMinInnerCollaspedWordsFreqFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Long);
            long minInnerCoWordFreq = long.MaxValue;

            for (int i = 0; i < this.Target.SegmentCount; i++)
            {
                string[] words = this.Target.Segments[i].GetWords();

                if (words.Length > 1)
                {
                    for (int j = 0; j < words.Length - 1; j++)
                    {
                        string word1 = words[j];
                        string word2 = words[j + 1];

                        string collapsedWord = word1 + word2;
                        long freq = this.Target.SubQueryFreqDic[collapsedWord];

                        if (minInnerCoWordFreq > freq)
                            minInnerCoWordFreq = freq;
                    }
                }
            }

            if (minInnerCoWordFreq < long.MaxValue)
                feature.ValueList.Add(minInnerCoWordFreq);
            else
                feature.ValueList.Add(Convert.ToInt64(0));

            return feature;
        }
        //public Feature GetMinInnerCollaspedWordsFreqFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);
        //    List<long> innerMinFreqs = new List<long>();

        //    for (int i = 0; i < this.Target.SegmentCount; i++)
        //    {
        //        string[] words = this.Target.Segments[i].GetWords();
        //        long innerMaxFreq = long.MaxValue;

        //        if (words.Length <= 1)
        //            continue;
        //        else
        //            for (int j = 0; j < words.Length - 1; j++)
        //            {
        //                string word1 = words[j];
        //                string word2 = words[j + 1];

        //                string collapsedWord = word1 + word2;
        //                long freq = this.Target.SubQueryFreqDic[collapsedWord];

        //                if (innerMaxFreq > freq)
        //                    innerMaxFreq = freq;
        //            }

        //        if (innerMaxFreq < long.MaxValue)
        //        {
        //            if (i <= MaxSeg || innerMinFreqs.Count <= 0)
        //                innerMinFreqs.Add(innerMaxFreq);
        //            else
        //                innerMinFreqs[innerMinFreqs.Count - 1] = innerMinFreqs[innerMinFreqs.Count - 1] > innerMaxFreq ? innerMaxFreq : innerMinFreqs[innerMinFreqs.Count - 1];
        //        }
        //    }

        //    if (innerMinFreqs.Count <= 0)
        //        feature.ValueList.Add(Convert.ToInt64(0));
        //    else
        //        feature.ValueList.Add(innerMinFreqs.Min());

        //    return feature;
        //}

        ///// <summary>
        ///// The mutial information between segments
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetSegMutualInfoArrayFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Decimal);
        //    List<decimal> mutialInfos = new List<decimal>();

        //    if (this.Target.SegmentCount > 1)
        //    {
        //        for (int i = 0; i < this.Target.SegmentCount - 1; i++)
        //        {
        //            double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i]]);
        //            double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i + 1]]);
        //            double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i] + " " + this.Target.Segments[i + 1]]);

        //            if (freq1 != 0 && freq2 != 0 && freq12 != 0)
        //            {
        //                double mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

        //                if (i < MaxSeg)
        //                    mutialInfos.Add(Convert.ToDecimal(mi));
        //                else
        //                    mutialInfos[mutialInfos.Count - 1] = mutialInfos[mutialInfos.Count - 1] < Convert.ToDecimal(mi) ? Convert.ToDecimal(mi) : mutialInfos[mutialInfos.Count - 1];
        //            }
        //            else if (i < MaxSeg)
        //                mutialInfos.Add(Convert.ToDecimal(0));
        //        }
        //    }

        //    for (int i = 2; i <= MaxSeg; i++)
        //    {
        //        if (i == this.Target.SegmentCount || i == MaxSeg && this.Target.SegmentCount >= MaxSeg)
        //            foreach (decimal value in mutialInfos)
        //                feature.ValueList.Add(value);

        //        else if (i != this.Target.SegmentCount)
        //            for (int j = 0; j < i - 1; j++)
        //                feature.ValueList.Add(Convert.ToDecimal(0));
        //    }

        //    return feature;
        //}


        ///// <summary>
        ///// The mutual info of the words between segments
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetSepWordsMutualInfoArrayFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Decimal);
        //    List<decimal> mutialInfos = new List<decimal>();

        //    if (this.Target.SegmentCount > 1)
        //    {
        //        for (int i = 0; i < this.Target.SegmentCount - 1; i++)
        //        {
        //            string[] words1 = this.Target.Segments[i].GetWords();
        //            string[] words2 = this.Target.Segments[i + 1].GetWords();

        //            double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[words1[words1.Length - 1]]);
        //            double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[words2[0]]);
        //            double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[words1[words1.Length - 1] + " " + words2[0]]);

        //            if (freq1 != 0 && freq2 != 0 && freq12 != 0)
        //            {
        //                double mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

        //                if (i < MaxSeg)
        //                    mutialInfos.Add(Convert.ToDecimal(mi));
        //                else
        //                    mutialInfos[mutialInfos.Count - 1] = mutialInfos[mutialInfos.Count - 1] < Convert.ToDecimal(mi) ? Convert.ToDecimal(mi) : mutialInfos[mutialInfos.Count - 1];
        //            }
        //            else if (i < MaxSeg)
        //                mutialInfos.Add(Convert.ToDecimal(0));
        //        }
        //    }

        //    for (int i = 2; i <= MaxSeg; i++)
        //    {
        //        if (i == this.Target.SegmentCount || i == MaxSeg && this.Target.SegmentCount >= MaxSeg)
        //            foreach (decimal value in mutialInfos)
        //                feature.ValueList.Add(value);

        //        else if (i != this.Target.SegmentCount)
        //            for (int j = 0; j < i - 1; j++)
        //                feature.ValueList.Add(Convert.ToDecimal(0));
        //    }

        //    return feature;
        //}

        ///// <summary>
        ///// The max mutial information between words in segment.
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetInnerMaxMutualInfoArrayFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Decimal);
        //    List<decimal> innerMaxMutialInfos = new List<decimal>();

        //    for (int i = 0; i < this.Target.SegmentCount; i++)
        //    {
        //        string[] words = this.Target.Segments[i].GetWords();
        //        double innerMaxMI = -1;

        //        if (words.Length <= 1)
        //            innerMaxMI = 0;
        //        else
        //            for (int j = 0; j < words.Length - 1; j++)
        //            {
        //                double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[words[j]]);
        //                double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[words[j + 1]]);
        //                double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[words[j] + " " + words[j + 1]]);

        //                if (freq1 != 0 && freq2 != 0 && freq12 != 0)
        //                {
        //                    double mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

        //                    if (innerMaxMI < mi)
        //                        innerMaxMI = mi;
        //                }
        //            }

        //        if (i <= MaxSeg)
        //            innerMaxMutialInfos.Add(Convert.ToDecimal(innerMaxMI));
        //        else
        //            innerMaxMutialInfos[innerMaxMutialInfos.Count - 1] = innerMaxMutialInfos[innerMaxMutialInfos.Count - 1] < Convert.ToDecimal(innerMaxMI) ? Convert.ToDecimal(innerMaxMI) : innerMaxMutialInfos[innerMaxMutialInfos.Count - 1];
        //    }

        //    for (int i = 1; i <= MaxSeg + 1; i++)
        //    {
        //        if (i == this.Target.SegmentCount || i == MaxSeg + 1 && this.Target.SegmentCount > MaxSeg)
        //            foreach (decimal value in innerMaxMutialInfos)
        //                feature.ValueList.Add(value);

        //        else if (i != this.Target.SegmentCount)
        //            for (int j = 0; j < i; j++)
        //                feature.ValueList.Add(Convert.ToDecimal(0));
        //    }

        //    return feature;
        //}

        ///// <summary>
        ///// The freq of the collasped words between segs
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetSepCollapsedWordsFreqArrayFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);
        //    List<object> freqs = new List<object>();

        //    if (this.Target.SegmentCount > 1)
        //    {
        //        for (int i = 0; i < this.Target.SegmentCount - 1; i++)
        //        {
        //            string[] words1 = this.Target.Segments[i].GetWords();
        //            string[] words2 = this.Target.Segments[i + 1].GetWords();

        //            string collapsedWord = words1[words1.Length - 1] + words2[0];
        //            long freq = this.Target.SubQueryFreqDic[collapsedWord];

        //            if (i < MaxSeg)
        //                freqs.Add(freq);
        //            else
        //                freqs[freqs.Count - 1] = (long)freqs[freqs.Count - 1] < freq ? freq : freqs[freqs.Count - 1];
        //        }
        //    }

        //    for (int i = 2; i <= MaxSeg; i++)
        //    {
        //        if (i == this.Target.SegmentCount || i == MaxSeg && this.Target.SegmentCount >= MaxSeg)
        //            feature.ValueList.AddRange(freqs);

        //        else if (i != this.Target.SegmentCount)
        //            for (int j = 0; j < i - 1; j++)
        //                feature.ValueList.Add(Convert.ToInt64(0));
        //    }

        //    return feature;
        //}



        ///// <summary>
        ///// The max freq of the collapsed word in a segment
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetInnerCollaspedWordsMaxFreqArrayFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);
        //    List<object> innerMaxFreqs = new List<object>();

        //    for (int i = 0; i < this.Target.SegmentCount; i++)
        //    {
        //        string[] words = this.Target.Segments[i].GetWords();
        //        long innerMaxFreq = 0;

        //        if (words.Length <= 1)
        //            innerMaxFreq = 0;
        //        else
        //            for (int j = 0; j < words.Length - 1; j++)
        //            {
        //                string word1 = words[j];
        //                string word2 = words[j + 1];

        //                string collapsedWord = word1 + word2;
        //                long freq = this.Target.SubQueryFreqDic[collapsedWord];

        //                if (innerMaxFreq < freq)
        //                    innerMaxFreq = freq;
        //            }

        //        if (i <= MaxSeg)
        //            innerMaxFreqs.Add(innerMaxFreq);
        //        else
        //            innerMaxFreqs[innerMaxFreqs.Count - 1] = (long)innerMaxFreqs[innerMaxFreqs.Count - 1] < innerMaxFreq ? innerMaxFreq : innerMaxFreqs[innerMaxFreqs.Count - 1];
        //    }

        //    for (int i = 1; i <= MaxSeg + 1; i++)
        //    {
        //        if (i == this.Target.SegmentCount || i == MaxSeg + 1 && this.Target.SegmentCount > MaxSeg)
        //            feature.ValueList.AddRange(innerMaxFreqs);

        //        else if (i != this.Target.SegmentCount)
        //            for (int j = 0; j < i; j++)
        //                feature.ValueList.Add(Convert.ToInt64(0));
        //    }

        //    return feature;
        //}

        //public Feature GetSumSegMutualInfoFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Decimal);
        //    decimal sumSegMI = 0;

        //    if (this.Target.SegmentCount > 1)
        //    {
        //        for (int i = 0; i < this.Target.SegmentCount - 1; i++)
        //        {
        //            double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i]]);
        //            double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i + 1]]);
        //            double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i] + " " + this.Target.Segments[i + 1]]);

        //            double mi = 0;
        //            if (freq1 != 0 && freq2 != 0 && freq12 != 0)
        //                mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

        //            sumSegMI += Convert.ToDecimal(mi);
        //        }
        //    }

        //    feature.ValueList.Add(sumSegMI);

        //    return feature;
        //}

        //public Feature GetSumSepWordsMutualInfoFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Decimal);
        //    decimal sumSepWordMI = 0;

        //    if (this.Target.SegmentCount > 1)
        //    {
        //        for (int i = 0; i < this.Target.SegmentCount - 1; i++)
        //        {
        //            string[] words1 = this.Target.Segments[i].GetWords();
        //            string[] words2 = this.Target.Segments[i + 1].GetWords();

        //            double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[words1[words1.Length - 1]]);
        //            double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[words2[0]]);
        //            double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[words1[words1.Length - 1] + " " + words2[0]]);

        //            double mi = 0;
        //            if (freq1 != 0 && freq2 != 0 && freq12 != 0)
        //                mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

        //            sumSepWordMI += Convert.ToDecimal(mi);
        //        }
        //    }

        //    feature.ValueList.Add(sumSepWordMI);

        //    return feature;
        //}

        //public Feature GetSumMinInnerMutualInfoFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Decimal);
        //    decimal sumInnerMI = 0;

        //    for (int i = 0; i < this.Target.SegmentCount; i++)
        //    {
        //        string[] words = this.Target.Segments[i].GetWords();

        //        if (words.Length > 1)
        //        {
        //            for (int j = 0; j < words.Length - 1; j++)
        //            {
        //                double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[words[j]]);
        //                double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[words[j + 1]]);
        //                double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[words[j] + " " + words[j + 1]]);

        //                double mi = 0;
        //                if (freq1 != 0 && freq2 != 0 && freq12 != 0)
        //                    mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

        //                sumInnerMI += Convert.ToDecimal(mi);
        //            }
        //        }
        //    }

        //    feature.ValueList.Add(sumInnerMI);

        //    return feature;
        //}

        //public Feature GetSumSepCollapsedWordsFreqFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);
        //    long sumSepCoWordFreq = 0;

        //    if (this.Target.SegmentCount > 1)
        //    {
        //        for (int i = 0; i < this.Target.SegmentCount - 1; i++)
        //        {
        //            string[] words1 = this.Target.Segments[i].GetWords();
        //            string[] words2 = this.Target.Segments[i + 1].GetWords();

        //            string collapsedWord = words1[words1.Length - 1] + words2[0];
        //            long freq = this.Target.SubQueryFreqDic[collapsedWord];

        //            sumSepCoWordFreq += freq;
        //        }
        //    }

        //    feature.ValueList.Add(sumSepCoWordFreq);
        //    return feature;
        //}

        //public Feature GetSumInnerCollapsedWordsFreqFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Long);
        //    long sumInnerCoWordFreq = 0;

        //    for (int i = 0; i < this.Target.SegmentCount; i++)
        //    {
        //        string[] words = this.Target.Segments[i].GetWords();

        //        if (words.Length > 1)
        //        {
        //            for (int j = 0; j < words.Length - 1; j++)
        //            {
        //                string word1 = words[j];
        //                string word2 = words[j + 1];

        //                string collapsedWord = word1 + word2;
        //                long freq = this.Target.SubQueryFreqDic[collapsedWord];

        //                sumInnerCoWordFreq += freq;
        //            }
        //        }
        //    }

        //    feature.ValueList.Add(sumInnerCoWordFreq);

        //    return feature;
        //}

        //public Feature GetSumSegMutalInfoMatrixFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Decimal);
        //    decimal[,] sumSegMIMaxtirx = new decimal[MaxSegWord, MaxSegWord];

        //    if (this.Target.SegmentCount > 1)
        //    {
        //        for (int i = 0; i < this.Target.SegmentCount - 1; i++)
        //        {
        //            double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i]]);
        //            double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i + 1]]);
        //            double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[this.Target.Segments[i] + " " + this.Target.Segments[i + 1]]);

        //            double mi = 0;
        //            if (freq1 != 0 && freq2 != 0 && freq12 != 0)
        //                mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

        //            int length1 = this.Target.Segments[i].GetWordCount();
        //            int length2 = this.Target.Segments[i + 1].GetWordCount();

        //            int d1 = length1 >= MaxSegWord ? MaxSegWord - 1 : length1;
        //            int d2 = length2 >= MaxSegWord ? MaxSegWord - 1 : length2;

        //            sumSegMIMaxtirx[d1, d2] += Convert.ToDecimal(mi);
        //        }
        //    }

        //    for (int i = 0; i < MaxSegWord; i++)
        //        for (int j = 0; j < MaxSegWord; j++ )
        //            feature.ValueList.Add(sumSegMIMaxtirx[i, j]);

        //    return feature;

        //}

        //public Feature GetSumSepWordsMutualInfoMatrixFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Decimal);
        //    decimal[,] sumSegMIMaxtirx = new decimal[MaxSegWord, MaxSegWord];

        //    if (this.Target.SegmentCount > 1)
        //    {
        //        for (int i = 0; i < this.Target.SegmentCount - 1; i++)
        //        {
        //            string[] words1 = this.Target.Segments[i].GetWords();
        //            string[] words2 = this.Target.Segments[i + 1].GetWords();

        //            double freq1 = Convert.ToDouble(this.Target.SubQueryFreqDic[words1[words1.Length - 1]]);
        //            double freq2 = Convert.ToDouble(this.Target.SubQueryFreqDic[words2[0]]);
        //            double freq12 = Convert.ToDouble(this.Target.SubQueryFreqDic[words1[words1.Length - 1] + " " + words2[0]]);

        //            double mi = 0;
        //            if (freq1 != 0 && freq2 != 0 && freq12 != 0)
        //                mi = Math.Log(freq12 * TotalWebNGramCount / freq1 / freq2);

        //            int length1 = this.Target.Segments[i].GetWordCount();
        //            int length2 = this.Target.Segments[i + 1].GetWordCount();

        //            int d1 = length1 >= MaxSegWord ? MaxSegWord - 1 : length1;
        //            int d2 = length2 >= MaxSegWord ? MaxSegWord - 1 : length2;

        //            sumSegMIMaxtirx[d1, d2] += Convert.ToDecimal(mi);
        //        }
        //    }

        //    for (int i = 0; i < MaxSegWord; i++)
        //        for (int j = 0; j < MaxSegWord; j++)
        //            feature.ValueList.Add(sumSegMIMaxtirx[i, j]);

        //    return feature;
        //}

        //public Feature GetCondSepProbFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);
        //    bool isLowest = this.Target.GroupPointer.Min(qs => qs.CondSepProb) == this.Target.CondSepProb;

        //    feature.ValueList.Add(isLowest);
        //    return feature;
        //}

        //public Feature GetMaxCondSepProbSplitedFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);
        //    double max = this.Target.SubQueryCondProbDic.Where(kv => kv.Key.GetWordCount() > 1).Max(kv => kv.Value);
        //    bool notSplited = false;

        //    var kvPair = this.Target.SubQueryCondProbDic.First(kv => kv.Value == max);

        //    string[] maxSubQuries = kvPair.Key.GetWords();
        //    string[] words = this.Target.Query.GetWords();

        //    if (max > -0.5)
        //    {
        //        for (int i = 0; i < this.Target.WordCount - maxSubQuries.Length; i++)
        //        {
        //            bool match = true;
        //            for (int j = i, k = 0; k < maxSubQuries.Length; j++, k++)
        //            {
        //                if (words[j] != maxSubQuries[k])
        //                {
        //                    match = false;
        //                    break;
        //                }
        //            }

        //            if (match)
        //            {
        //                int lastIndex = i + maxSubQuries.Length - 1;
        //                notSplited = !this.Target.Breaks.Contains(lastIndex);
        //                break;
        //            }
        //        }
        //    }

        //    feature.ValueList.Add(notSplited);
        //    return feature;
        //}

        //public Feature GetMax2WordCondSepSplitedFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);
        //    bool Splited = false;
        //    bool exists = false;

        //    string[] words = this.Target.Words;

        //    for (int i = 0; i < this.Target.WordCount - 1; i++)
        //    {
        //        string subWord = words[i] + " " + words[i + 1];
        //        double condProb = this.Target.SubQueryCondProbDic[subWord];

        //        if (condProb > -1)
        //        {
        //            exists = true;
        //            if (this.Target.Breaks.Contains(i + 1))
        //                Splited = true;
        //        }
        //    }

        //    feature.ValueList.Add(exists);
        //    feature.ValueList.Add(Splited);
        //    return feature;
        //}

        //public Feature GetSegCondFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);

        //    bool exists = false;

        //    for (int i = 1; i < this.Target.SegmentCount; i++)
        //    {
        //        string seg = this.Target.Segments[i];
        //        string firstWord = seg.GetWords()[0];

        //        double maxCondProb = this.Target.SubQueryCondProbDic.Where(kv => kv.Key.EndsWith(firstWord)).Max(kv => kv.Value);
        //        if (maxCondProb > -0.5)
        //            exists = true;
        //    }

        //    feature.ValueList.Add(exists);
        //    return feature;
        //}

        #endregion

        #region Query Natural

        /// <summary>
        /// The query words count
        /// </summary>
        /// <returns></returns>
        public Feature GetQueryLengthFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Int);
            feature.ValueList.Add(this.Target.WordCount);

            return feature;
        }

        #endregion

        #region Pattern

        public Feature GetWordAppearFullySplitWordFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            var words = this.Target.Query.ToLower().GetWords();
            foreach (var wansWord in WordAppearFullySplitWords)
            {
                bool exists = words.Contains(wansWord);
                bool existsAndFullySeg = exists && words.Length == this.Target.SegmentCount;
                feature.ValueList.Add(exists);
                feature.ValueList.Add(existsAndFullySeg);
            }

            return feature;
        }

        public Feature GetEndWithNotLastWordFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            foreach (var ewnlWord in EndWithNotLastWords)
            {
                bool endWithAndLast = false;
                for (int i = 0; i < this.Target.SegmentCount - 1; i++)
                {
                    if (this.Target.Segments[i].ToLower().EndsWith(ewnlWord) && this.Target.Segments[i].GetWordCount() > 1)
                    {
                        endWithAndLast = true;
                        break;
                    }
                }
                feature.ValueList.Add(endWithAndLast);
            }

            return feature;
        }

        public Feature GetEndWithAndLastWordFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            foreach (var ewalWord in EndWithNotLastWords)
            {
                bool endWithAndLast = false;
                for (int i = 0; i < this.Target.SegmentCount - 1; i++)
                {
                    if (this.Target.Segments[i].ToLower().EndsWith(ewalWord) && this.Target.Segments[i].GetWordCount() > 1)
                    {
                        endWithAndLast = true;
                        break;
                    }
                }
                feature.ValueList.Add(endWithAndLast);
            }

            return feature;
        }

        //public Feature GetStartWordFeature()
        //{
        //    Feature feature = new Feature(this, GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);

        //    foreach (var startWord in StartWords)
        //    {
        //        bool isStart = false;
        //        for (int i = 1; i < this.Target.SegmentCount; i++)
        //        {
        //            var words = this.Target.Segments[i].ToLower().GetWords();
        //            if (words[0] == startWord && words.Length > 1)
        //            {
        //                isStart = true;
        //                break;
        //            }
        //        }
        //        feature.ValueList.Add(isStart);
        //    }
        //    return feature;
        //}

        //public Feature GetStartWordPunishFeature()
        //{
        //    Feature feature = new Feature(this, GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);
        //    var words = this.Target.Query.ToLower().GetWords();
        //    foreach (var startWord in StartWords)
        //    {
        //        int index = words.IndexOf(startWord);
        //        bool containAndNotStart = index > 0 && !this.Target.Breaks.Contains(index);
        //        feature.ValueList.Add(containAndNotStart);
        //    }
        //    return feature;
        //}

        public Feature GetEndWordFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);

            foreach (var endWord in EndWords)
            {
                bool isEnd = false;
                for (int i = 1; i < this.Target.SegmentCount; i++)
                {
                    var words = this.Target.Segments[i].ToLower().GetWords();
                    if (words.Last() == endWord && words.Length > 1)
                    {
                        isEnd = true;
                        break;
                    }
                }
                feature.ValueList.Add(isEnd);
            }
            return feature;
        }

        public Feature GetIndependentWordFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            foreach (var iWord in IndependentWords)
            {
                bool independent = this.Target.Segments.Select(s => s.ToLower()).Contains(iWord);
                feature.ValueList.Add(independent);
            }
            return feature;
        }

        public Feature GetIndependentWordPunishFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            var words = this.Target.Query.ToLower().GetWords();
            foreach (var iWord in IndependentWords)
            {
                int index = words.IndexOf(iWord);
                bool notIndependent = true;
                if (index == 0 && this.Target.Breaks.Contains(1))
                    notIndependent = false;
                else if (index == words.Length - 1 && this.Target.Breaks.Contains(words.Length - 1))
                    notIndependent = false;
                else if (index >= 0 && this.Target.Breaks.Contains(index) && this.Target.Breaks.Contains(index + iWord.GetWordCount()))
                    notIndependent = false;
                else if (index < 0)
                    notIndependent = false;
                feature.ValueList.Add(notIndependent);
            }
            return feature;
        }

        public Feature GetNotIndependentWordFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            var words = this.Target.Query.ToLower().GetWords();
            foreach (var niWord in NotIndependentWords)
            {
                int index = words.IndexOf(niWord);
                bool notIndependent = true;
                if (index == 0 && this.Target.Breaks.Contains(1))
                    notIndependent = false;
                else if (index == words.Length - 1 && this.Target.Breaks.Contains(words.Length - 1))
                    notIndependent = false;
                else if (index >= 0 && this.Target.Breaks.Contains(index) && this.Target.Breaks.Contains(index + niWord.GetWordCount()))
                    notIndependent = false;
                else if (index < 0)
                    notIndependent = false;
                feature.ValueList.Add(notIndependent);
            }
            return feature;
        }

        public Feature GetNotIndependentWordPunishFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            foreach (var niWord in NotIndependentWords)
            {
                bool independent = this.Target.Segments.Select(s => s.ToLower()).Contains(niWord);
                feature.ValueList.Add(independent);
            }
            return feature;

        }

        public Feature GetFirstTwoCombineFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            bool firstTwoCombine = this.Target.Segments[0].GetWordCount() == 2;
            feature.ValueList.Add(firstTwoCombine);
            return feature;
        }

        public Feature GetLastTwoCombineFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            bool lastTwoCombine = this.Target.Segments.Last().GetWordCount() == 2;
            feature.ValueList.Add(lastTwoCombine);
            return feature;
        }

        public Feature GetOnlyOneSegmentWithTwoWordFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            bool onlyOneSegWithTwo = this.Target.SegmentCount == this.Target.Query.GetWordCount() - 1;
            feature.ValueList.Add(onlyOneSegWithTwo);
            return feature;
        }

        public Feature GetPrepAdjacentTwoFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            foreach (var prep in PrepWords)
            {
                bool adjacentTwo = false;
                var index = this.Target.Segments.Select(seg => seg.ToLower()).IndexOf(prep);
                if (index >= 0)
                {
                    if (index >= 1 && this.Target.Segments[index].GetWordCount() == 2)
                        adjacentTwo = true;

                    if (!adjacentTwo && index < this.Target.SegmentCount - 1 && this.Target.Segments[index + 1].GetWordCount() == 2)
                        adjacentTwo = true;
                }
                feature.ValueList.Add(adjacentTwo);
            }

            return feature;
        }

        public Feature GetPrepAdjacentTwoPunishFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            var words = this.Target.Query.ToLower().GetWords();
            foreach (var prep in PrepWords)
            {
                bool notAdjacentTwo = words.Contains(prep);
                var index = this.Target.Segments.Select(seg => seg.ToLower()).IndexOf(prep);
                if (index >= 0)
                {
                    if (index >= 1 && this.Target.Segments[index].GetWordCount() == 2)
                        notAdjacentTwo = false;

                    if (notAdjacentTwo && index < this.Target.SegmentCount - 1 && this.Target.Segments[index + 1].GetWordCount() == 2)
                        notAdjacentTwo = false;
                }
                feature.ValueList.Add(notAdjacentTwo);
            }
            return feature;
        }

        public Feature GetSameWithPunctionSegmentationFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            bool isSame = false;
            int sameCount = 0;
            
            var punctionSegments = RegularExpressions.UselessPunctionRegex.Split(this.Target.Query.ToLower()).Where(seg => seg.Trim() != string.Empty).Select(seg => seg.ToLower().Trim());
            foreach (var pSeg in punctionSegments)
            {
                if (this.Target.Segments.Select(seg => seg.ToLower()).Contains(pSeg))
                    sameCount++;
            }

            isSame = sameCount == this.Target.SegmentCount;

            feature.ValueList.Add(isSame);
            return feature;
        }

        public Feature GetContinueCapitalFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            var words = this.Target.Query.GetWords();
            string capitalStr = string.Empty;
            bool startC = false;
            foreach (var word in words)
            {
                if (word[0] >= 'A' && word[0] <= 'Z')
                {
                    startC = true;
                    capitalStr += " " + word;
                }
                else if (startC)
                    break;
            }

            bool fully = false;
            bool independent = false;
            if (capitalStr.Trim() != string.Empty && capitalStr.Trim().GetWordCount() > 1)
            {
                fully = this.Target.Segments.Contains(capitalStr.Trim());
                independent = this.Target.Segments.Contains(capitalStr.Trim().GetWords());
            }
            feature.ValueList.Add(fully);
            feature.ValueList.Add(independent);
            return feature;
        }

        public Feature GetContinueCapitalPunishFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            var words = this.Target.Query.GetWords();
            string capitalStr = string.Empty;
            bool startC = false;
            foreach (var word in words)
            {
                if (word[0] >= 'A' && word[0] <= 'Z')
                {
                    startC = true;
                    capitalStr += " " + word;
                }
                else if (startC)
                    break;
            }

            bool notFullyAndIndependent = false;
            if (capitalStr.Trim() != string.Empty && capitalStr.Trim().GetWordCount() > 1)
                notFullyAndIndependent = !this.Target.Segments.Contains(capitalStr.Trim()) && !this.Target.Segments.Contains(capitalStr.Trim().GetWords());
            feature.ValueList.Add(notFullyAndIndependent);
            return feature;
        }

        public Feature GetMonthWordFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            var words = this.Target.Query.GetWords();
            bool monthSegExits = false;
            for (int i = 0; i < words.Length; i++)
            {
                if (MonthWords.Contains(words[i].ToLower()))
                {
                    string monthSeg = string.Empty;
                    int num = 0;
                    if (i > 0 && int.TryParse(words[i - 1].ToLower().Trim("stndrh".ToArray()), out num))
                        monthSeg = words[i - 1] + " " + words[i];
                    else if (i < words.Length - 1 && int.TryParse(words[i + 1].Trim("stndrh".ToArray()), out num))
                        monthSeg = words[i] + " " + words[i + 1];

                    if (monthSeg != string.Empty && this.Target.Segments.Contains(monthSeg))
                    {
                        monthSegExits = true;
                        break;
                    }
                }
            }
            feature.ValueList.Add(monthSegExits);
            return feature;
        }

        public Feature GetMonthWordPunishFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            var words = this.Target.Query.GetWords();
            bool notMonthSegExits = false;
            for (int i = 0; i < words.Length; i++)
            {
                if (MonthWords.Contains(words[i].ToLower()))
                {
                    string monthSeg = string.Empty;
                    int num = 0;
                    if (i > 0 && int.TryParse(words[i - 1].ToLower().Trim("stndrh".ToArray()), out num))
                        monthSeg = words[i - 1] + " " + words[i];
                    else if (i < words.Length - 1 && int.TryParse(words[i + 1].Trim("stndrh".ToArray()), out num))
                        monthSeg = words[i] + " " + words[i + 1];

                    if (monthSeg != string.Empty && !this.Target.Segments.Contains(monthSeg))
                    {
                        notMonthSegExits = true;
                        break;
                    }
                }
            }
            feature.ValueList.Add(notMonthSegExits);
            return feature;
        }

        //public Feature GetPlaceWordFeature()
        //{
        //    Feature feature = new Feature(this, GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);
        //    var words = this.Target.Query.GetWords();
        //    bool combinePlace = false;
        //    bool combineTwoPlace = false;
        //    for (int i = 0; i < words.Length - 1; i++)
        //    {
        //        string word1 = words[i];
        //        string word2 = words[i + 1];
        //        string combineWord = word1 + " " + word2;

        //        if(PlaceWords.Contains(combineWord.ToLower()) && this.Target.Segments.Contains(combineWord))
        //            combinePlace = true;
        //        if (PlaceWords.Contains(word1.ToLower()) && PlaceWords.Contains(word2.ToLower()) && this.Target.Segments.Contains(combineWord))
        //            combineTwoPlace = true;
        //    }

        //    feature.ValueList.Add(combinePlace);
        //    feature.ValueList.Add(combineTwoPlace);
        //    return feature;
        //}

        //public Feature GetPlaceWordPunishFeature()
        //{
        //    Feature feature = new Feature(this, GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);
        //    var words = this.Target.Query.GetWords();
        //    bool notCombinePlace = false;
        //    bool notCombineTwoPlace = false;
        //    for (int i = 0; i < words.Length - 1; i++)
        //    {
        //        string word1 = words[i];
        //        string word2 = words[i + 1];
        //        string combineWord = word1 + " " + word2;

        //        if (PlaceWords.Contains(combineWord.ToLower()) && !this.Target.Segments.Contains(combineWord))
        //            notCombinePlace = true;
        //        if (PlaceWords.Contains(word1.ToLower()) && PlaceWords.Contains(word2.ToLower()) && !this.Target.Segments.Contains(combineWord))
        //            notCombineTwoPlace = true;
        //    }

        //    feature.ValueList.Add(notCombinePlace);
        //    feature.ValueList.Add(notCombineTwoPlace);
        //    return feature;
        //}

        //public Feature GetNotLastWordFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);

        //    foreach (var notLastWord in NotLastWords)
        //    {
        //        bool exists = false;
        //        bool isLast = false;

        //        for (int i = this.Target.SegmentCount - 2; i >= 0; i--)
        //        {
        //            string segLower = this.Target.Segments[i].Trim().ToLower();
        //            if (segLower.ContainsWord(notLastWord))
        //            {
        //                exists = true;
        //                string[] segWords = segLower.Split(' ');

        //                if (segWords.Last() == notLastWord)
        //                    isLast = true;
        //            }
        //        }
        //        feature.ValueList.Add(exists);
        //        feature.ValueList.Add(isLast);
        //    }

        //    return feature;
        //}

        ///// <summary>
        ///// Exists and Contribution of "and"
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetAndBoolFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);

        //    bool independent = false;
        //    bool exists = false;

        //    foreach (string seg in this.Target.Segments)
        //    {
        //        if (seg.ToLower().Contains("and"))
        //        {
        //            exists = true;
        //            if (seg.GetWordCount() <= 1)
        //                independent = true;
        //        }
        //    }

        //    feature.ValueList.Add(exists);
        //    feature.ValueList.Add(independent);

        //    return feature;
        //}

        ///// <summary>
        ///// The exists and starts and ends and contribution of "free"
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetFreeBoolFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);

        //    bool independent = false;
        //    bool exists = false;

        //    foreach (string seg in this.Target.Segments)
        //    {
        //        if (seg.ToLower().Contains("free"))
        //        {
        //            exists = true;
        //            if (seg.GetWordCount() <= 1)
        //                independent = true;
        //        }
        //    }

        //    feature.ValueList.Add(exists);
        //    feature.ValueList.Add(independent);

        //    return feature;
        //}

        ///// <summary>
        ///// IsExist and contribution of "Vs" segment
        ///// </summary>
        ///// <returns></returns>
        //public Feature GetVsBoolFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);

        //    bool independent = false;
        //    bool exists = false;

        //    foreach (string seg in this.Target.Segments)
        //    {
        //        string segLower = seg.ToLower();
        //        if (segLower.Contains("vs") || segLower.Contains("versus") || segLower.Contains("compare"))
        //        {
        //            exists = true;
        //            if (segLower.GetWordCount() <= 1)
        //                independent = true;
        //        }
        //    }

        //    feature.ValueList.Add(exists);
        //    feature.ValueList.Add(independent);

        //    return feature;
        //}



        //public Feature GetConditionSingleSegWordFeature()
        //{
        //    Feature feature = new Feature(GetFeatureName(MethodBase.GetCurrentMethod().Name), ValueTypes.Bool);

        //    foreach (var kv in ConditionSingleSegWords)
        //    {
        //        if (kv.Key != string.Empty && !kv.Key.StartsWith("#"))
        //        {
        //            bool independent = false;
        //            bool exists = false;

        //            if (this.Target.Query.ContainsWord(kv.Key) && !this.Target.Query.ContainsWord(kv.Value[0]))
        //            {
        //                foreach (var seg in this.Target.Segments)
        //                {
        //                    if (seg.ContainsWord(kv.Key))
        //                    {
        //                        exists = true;
        //                        if (seg.GetWordCount() == 1)
        //                            independent = true;
        //                    }
        //                }
        //            }

        //            feature.ValueList.Add(exists && independent);
        //            feature.ValueList.Add(independent);
        //        }
        //    }


        //    return feature;
        //}

        #endregion

        #region Wiki Title

        public Feature GetLongestWikiTitleFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool | ValueTypes.Double);

            bool exist = false;
            bool splited = false;
            bool independent = false;
            double ratio = 0;

            int length, i = 0;
            List<int> wikiPos = new List<int>();
            for (length = this.Target.WordCount; length >= 2; length--)
            {
                for (i = 0; i <= this.Target.WordCount - length; i++)
                {
                    string subQuery = this.Target.GetSubQuery(i, length);
                    if (this.Target.WikiTitles.Contains(subQuery))
                    {
                        exist = true;
                        wikiPos.Add(i);
                    }
                }
                if (exist)
                    break;
            }

            if (exist)
            {
                int nonOverlapCount = 1;
                //for (int k = 1; k < wikiPos.Count; k++)
                //    if (wikiPos[k] - wikiPos[k - 1] >= length)
                //        nonOverlapCount++;

                ratio = Convert.ToDouble(length) * nonOverlapCount / this.Target.WordCount;

                foreach (var wp in wikiPos)
                {
                    int start = wp;
                    int end = wp + length;

                    if (!splited && this.Target.Breaks.Exists(b => b > start && b < end))
                        splited = true;
                    else if (splited && this.Target.Breaks.Exists(b => b > start && b < end))
                        splited = false;

                    if (!splited && !independent && (this.Target.Breaks.Contains(start) || start == 0) && this.Target.Breaks.Contains(end))
                        independent = true;
                    else if (!splited && independent && (!(this.Target.Breaks.Contains(start) || start == 0) || !this.Target.Breaks.Contains(end)))
                        independent = false;
                }
            }
            feature.ValueList.Add(exist);
            feature.ValueList.Add(ratio);

            feature.ValueList.Add(splited);
            feature.ValueList.Add(independent);


            return feature;
        }
        #endregion

        #region Difference Top1

        /// <summary>
        /// The different break count and same seg count
        /// </summary>
        /// <returns></returns>
        public Feature GetTop1DiffBreakAndSameSegCountFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Int);
            QSinfo top1QS = this.Target.GroupPointer.First();

            int u = 0, v = 0, errorBreak = 0, correctSeg = 0;
            bool correct = true;

            while (u < this.Target.Breaks.Count || v < top1QS.Breaks.Count)
            {
                if (this.Target.Breaks[u] < top1QS.Breaks[v])
                {
                    correct = false;
                    u++;
                    errorBreak++;
                }
                else if (this.Target.Breaks[u] > top1QS.Breaks[v])
                {
                    correct = false;
                    v++;
                    errorBreak++;
                }
                else
                {
                    u++;
                    v++;
                    if (correct)
                    {
                        correctSeg++;
                    }
                    correct = true;
                }
            }

            feature.ValueList.Add(errorBreak);
            feature.ValueList.Add(correctSeg);

            return feature;
        }

        /// <summary>
        /// bool of same / split / merge / move / wrong
        /// </summary>
        /// <returns></returns>
        public Feature GetTop1SameSplitMergeMoveWrongFeature()
        {
            NormalFeature feature = new NormalFeature(MethodBase.GetCurrentMethod().Name, ValueTypes.Bool);
            QSinfo top1QS = this.Target.GroupPointer.First();

            bool[] top1BreakArray = new bool[this.Target.WordCount + 1];
            bool[] thisBreakArray = new bool[this.Target.WordCount + 1];
            bool[] mergeBreakArray = new bool[this.Target.WordCount + 1];

            bool IsSame = false;
            bool IsSplit = false;
            bool IsMerge = false;
            bool IsMove = false;
            bool IsWrong = false;

            for (int i = 0; i <= this.Target.WordCount; i++)
            {
                top1BreakArray[i] = top1QS.Breaks.Contains(i);
                thisBreakArray[i] = this.Target.Breaks.Contains(i);
            }

            for (int i = 0; i <= this.Target.WordCount; i++)
                mergeBreakArray[i] = top1BreakArray[i] ^ thisBreakArray[i];

            int diffCount = mergeBreakArray.Count(b => b);

            if (diffCount == 0)
                IsSame = true;

            else if (diffCount == 1)
            {
                int i = 0;
                for (; i < mergeBreakArray.Length; i++)
                    if (mergeBreakArray[i])
                        break;

                if (top1BreakArray[i])
                    IsMerge = true;
                else
                    IsSplit = true;
            }

            else if (diffCount == 2)
            {
                int i = -1, j = -1;
                for (int k = 0; k < mergeBreakArray.Length; k++)
                {
                    if (mergeBreakArray[k])
                    {
                        if (i < 0)
                            i = k;
                        else
                            j = k;
                    }

                    if (j >= 0)
                        break;
                }
                if (Math.Abs(i - j) == 1 && (top1BreakArray[i] || top1BreakArray[j]))
                    IsMove = true;
                else
                    IsWrong = true;
            }

            else
                IsWrong = true;

            feature.ValueList.Add(IsSame);
            feature.ValueList.Add(IsSplit);
            feature.ValueList.Add(IsMerge);
            feature.ValueList.Add(IsMove);
            feature.ValueList.Add(IsWrong);

            return feature;
        }


        #endregion

        #region Filter

        public bool GetSegWordCountFilter()
        {
            return this.Target.Segments.Exists(seg => seg.GetWordCount() >= 4);
        }

        public bool GetWholeSegFilter()
        {
            return this.Target.SegmentCount == 1 && this.Target.Query.GetWordCount() >= 3;
        }

        public bool GetStartWordsFilter()
        {
            foreach (var startWord in StartWords)
            {
                if (this.Target.Segments.Exists(seg => (seg.ToLower().EndsWith(" " + startWord) || seg.ToLower() == startWord) && this.Target.Segments.Last() != seg))
                    return true;
            }

            return false;
        }

        public bool GetPlaceWordFilter()
        {
            var words = this.Target.Query.GetWords();
            bool notCombinePlace = false;
            bool notCombineTwoPlace = false;
            for (int i = 0; i < words.Length - 1; i++)
            {
                string word1 = words[i];
                string word2 = words[i + 1];
                string combineWord = word1 + " " + word2;

                if (PlaceWords.Contains(combineWord.ToLower()) && !this.Target.Segments.Contains(combineWord))
                    notCombinePlace = true;
                if (PlaceWords.Contains(word1.ToLower()) && PlaceWords.Contains(word2.ToLower()) && !this.Target.Segments.Contains(combineWord))
                    notCombineTwoPlace = true;
            }

            if (notCombinePlace && notCombineTwoPlace)
                return true;
            else
                return false;
        }

        public bool GetSameWithPunctionSegmentationFilter()
        {
            bool notSame = false;
            if (this.HumanLabeledSegmentation.ToLower() != this.Target.Query.ToLower())
            {
                int sameCount = 0;
                var punctionSegments = RegularExpressions.UselessPunctionRegex.Split(this.HumanLabeledSegmentation).Where(seg => seg.Trim() != string.Empty).Select(seg => seg.ToLower().Trim());
                foreach (var pSeg in punctionSegments)
                {
                    if (this.Target.Segments.Select(seg => seg.ToLower()).Contains(pSeg))
                        sameCount++;
                }

                notSame = sameCount != this.Target.SegmentCount;
            }
            return notSame;
        }

        //public bool GetContinueCapitalFilter()
        //{
        //    var words = this.Target.Query.GetWords();
        //    string capitalStr = string.Empty;
        //    bool startC = false;
        //    foreach (var word in words)
        //    {
        //        if (word[0] >= 'A' && word[0] <= 'Z')
        //        {
        //            startC = true;
        //            capitalStr += " " + word;
        //        }
        //        else if (startC)
        //            break;
        //    }

        //    bool notFullyAndIndependent = false;
        //    if (capitalStr.Trim() != string.Empty && capitalStr.Trim().GetWordCount() > 1)
        //        notFullyAndIndependent = !this.Target.Segments.Contains(capitalStr.Trim()) && !this.Target.Segments.Contains(capitalStr.Trim().GetWords());
        //    return notFullyAndIndependent;
        //}

        //public bool GetPrepAdjacentTwoFilter()
        //{
        //    var words = this.Target.Query.ToLower().GetWords();
        //    foreach (var prep in PrepWords)
        //    {
        //        bool notAdjacentTwo = words.Contains(prep);
        //        var index = this.Target.Segments.Select(seg => seg.ToLower()).IndexOf(prep);
        //        if (index >= 0)
        //        {
        //            if (index >= 1 && this.Target.Segments[index].GetWordCount() == 2)
        //                notAdjacentTwo = false;

        //            if (notAdjacentTwo && index < this.Target.SegmentCount - 1 && this.Target.Segments[index + 1].GetWordCount() == 2)
        //                notAdjacentTwo = false;
        //        }

        //        if (notAdjacentTwo)
        //            return true;
        //    }

        //    return false;
        //}

        //public bool GetIndependentFilter()
        //{
        //    var words = this.Target.Query.ToLower().GetWords();
        //    foreach (var iWord in IndependentWords)
        //    {
        //        int index = words.IndexOf(iWord);
        //        bool notIndependent = true;
        //        if (index == 0 && this.Target.Breaks.Contains(1))
        //            notIndependent = false;
        //        else if (index == words.Length - 1 && this.Target.Breaks.Contains(words.Length - 1))
        //            notIndependent = false;
        //        else if (index >= 0 && this.Target.Breaks.Contains(index) && this.Target.Breaks.Contains(index + iWord.GetWordCount()))
        //            notIndependent = false;
        //        else if (index < 0)
        //            notIndependent = false;

        //        if (notIndependent)
        //            return true;
        //    }
        //    return false;
        //}

        //public bool GetNotIndependentFilter()
        //{
        //    var words = this.Target.Query.ToLower().GetWords();
        //    foreach (var niWord in NotIndependentWords)
        //    {
        //        int index = words.IndexOf(niWord);
        //        bool notIndependent = true;
        //        if (index == 0 && this.Target.Breaks.Contains(1))
        //            notIndependent = false;
        //        else if (index == words.Length - 1 && this.Target.Breaks.Contains(words.Length - 1))
        //            notIndependent = false;
        //        else if (index >= 0 && this.Target.Breaks.Contains(index) && this.Target.Breaks.Contains(index + niWord.GetWordCount()))
        //            notIndependent = false;

        //        if (!notIndependent)
        //            return true;
        //    }
        //    return false;
        //}


        //public bool GetPrepAdjacentTwoFilter()
        //{
        //    var words = this.Target.Query.ToLower().GetWords();
        //    foreach (var prep in PrepWords)
        //    {
        //        bool notAdjacentTwo = words.Contains(prep);
        //        var index = this.Target.Segments.Select(seg => seg.ToLower()).IndexOf(prep);
        //        if (index >= 0)
        //        {
        //            if (index >= 1 && this.Target.Segments[index].GetWordCount() == 2)
        //                notAdjacentTwo = false;

        //            if (notAdjacentTwo && index < this.Target.SegmentCount - 1 && this.Target.Segments[index + 1].GetWordCount() == 2)
        //                notAdjacentTwo = false;
        //        }

        //        if (notAdjacentTwo)
        //            return true;
        //    }

        //    return false;
        //}

        //public bool GetMonthWordFilter()
        //{
        //    var words = this.Target.Query.GetWords();
        //    bool notMonthSegExits = false;
        //    for (int i = 0; i < words.Length; i++)
        //    {
        //        if (MonthWords.Contains(words[i].ToLower()))
        //        {
        //            string monthSeg = string.Empty;
        //            int num = 0;
        //            if (i > 0 && int.TryParse(words[i - 1].ToLower().Trim("stndrh".ToArray()), out num))
        //                monthSeg = words[i - 1] + " " + words[i];
        //            else if (i < words.Length - 1 && int.TryParse(words[i + 1].Trim("stndrh".ToArray()), out num))
        //                monthSeg = words[i] + " " + words[i + 1];

        //            if (monthSeg != string.Empty && !this.Target.Segments.Contains(monthSeg))
        //            {
        //                notMonthSegExits = true;
        //                break;
        //            }
        //        }
        //    }

        //    return notMonthSegExits;
        //}

        #endregion
    }
}
