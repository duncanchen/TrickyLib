using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Struct;
using TrickyLib.Extension;

namespace TrickyLib.Relevance
{
    public class DependencyModel
    {
        public static IEnumerable<double> CalcDependencyModelFeatures(
            string[] queryWords,
            List<string[]> docWordsList,
            long totalCollectionLength,
            Dictionary<string, long> collectionConceptFreqDic,
            Dictionary<string, DependencyModelWeights> dependencyModelFeaturesDic,
            int collectionIndex)
        {
            try
            {
                int windowSize = 8;
                int docLength = docWordsList.Sum(array => array.Length);

                //Initialize the ngram count dic
                Dictionary<string, int> unigramFreqDic = new Dictionary<string, int>();
                Dictionary<string, int> bigramFreqDic = new Dictionary<string, int>();
                Dictionary<string, int> bigramWinFreqDic = new Dictionary<string, int>();

                for (int i = 0; i < queryWords.Length; i++)
                {
                    if (!unigramFreqDic.ContainsKey(queryWords[i]))
                        unigramFreqDic.Add(queryWords[i], 0);

                    if (i < queryWords.Length - 1)
                    {
                        string bigram = queryWords[i] + " " + queryWords[i + 1];
                        if (!bigramFreqDic.ContainsKey(bigram))
                            bigramFreqDic.Add(bigram, 0);

                        string bigramWin = queryWords[i].CompareTo(queryWords[i + 1]) < 0 ? (queryWords[i] + " | " + queryWords[i + 1]) : (queryWords[i + 1] + " | " + queryWords[i]);
                        if (!bigramWinFreqDic.ContainsKey(bigramWin))
                            bigramWinFreqDic.Add(bigramWin, 0);
                    }
                }

                //Record the position index of each word
                Dictionary<string, HashSet<int>> wordPosDic = new Dictionary<string, HashSet<int>>();
                foreach (var word in queryWords)
                    if (!wordPosDic.ContainsKey(word))
                        wordPosDic.Add(word, new HashSet<int>());

                foreach (var docWords in docWordsList)
                {
                    foreach (var kv in wordPosDic)
                        kv.Value.Clear();

                    for (int i = 0; i < docWords.Length; i++)
                        if (wordPosDic.ContainsKey(docWords[i]))
                            wordPosDic[docWords[i]].Add(i);

                    //Sum unigram
                    foreach (var word in queryWords)
                        unigramFreqDic[word] += wordPosDic[word].Count;

                    for (int i = 0; i < queryWords.Length - 1; i++)
                    {
                        string bigram = queryWords[i] + " " + queryWords[i + 1];
                        string bigramWin = queryWords[i].CompareTo(queryWords[i + 1]) < 0 ? (queryWords[i] + " | " + queryWords[i + 1]) : (queryWords[i + 1] + " | " + queryWords[i]);

                        foreach (var posWord1 in wordPosDic[queryWords[i]])
                        {
                            //Sum bigram
                            if (wordPosDic[queryWords[i + 1]].Contains(posWord1 + 1))
                                bigramFreqDic[bigram]++;

                            //Sum bigramWin
                            foreach (var posWord2 in wordPosDic[queryWords[i + 1]])
                                if (posWord1 != posWord2 && (posWord2 >= posWord1 - windowSize + 1) && (posWord2 <= posWord1 + windowSize - 1))
                                    bigramWinFreqDic[bigramWin]++;
                        }
                    }
                }

                //Calc unigram dependency features
                double[] unigramDependencyFeatures = new double[7];
                foreach (var unigramKv in unigramFreqDic)
                {
                    if (dependencyModelFeaturesDic.ContainsKey(unigramKv.Key))
                    {
                        var weightedFeatures = dependencyModelFeaturesDic[unigramKv.Key].CalcWeightedFeatures(
                            unigramKv.Value,
                            collectionConceptFreqDic.ContainsKey(unigramKv.Key) ? collectionConceptFreqDic[unigramKv.Key] : 0,
                            docLength,
                            totalCollectionLength,
                            collectionIndex);

                        for (int i = 0; i < 7; ++i)
                            unigramDependencyFeatures[i] += weightedFeatures[i];
                    }
                }

                //Calc bigram dependency features
                double[] bigramDependencyFeatures = new double[7];
                foreach (var bigramKv in bigramFreqDic)
                {
                    if (dependencyModelFeaturesDic.ContainsKey(bigramKv.Key))
                    {
                        var weightedFeatures = dependencyModelFeaturesDic[bigramKv.Key].CalcWeightedFeatures(
                            bigramKv.Value,
                            collectionConceptFreqDic.ContainsKey(bigramKv.Key) ? collectionConceptFreqDic[bigramKv.Key] : 0,
                            docLength,
                            totalCollectionLength,
                            collectionIndex);

                        for (int i = 0; i < 7; ++i)
                            bigramDependencyFeatures[i] += weightedFeatures[i];
                    }
                }

                //Calc bigram in window size dependency features
                double[] bigramWinDependencyFeatures = new double[7];
                for (int j = 0; j < queryWords.Length - 1; ++j)
                {
                    string bigram = queryWords[j] + " " + queryWords[j + 1];
                    string bigramWin = queryWords[j].CompareTo(queryWords[j + 1]) < 0 ? (queryWords[j] + " | " + queryWords[j + 1]) : (queryWords[j + 1] + " | " + queryWords[j]);

                    if (dependencyModelFeaturesDic.ContainsKey(bigram))
                    {
                        var weightedFeatures = dependencyModelFeaturesDic[bigram].CalcWeightedFeatures(
                            bigramWinFreqDic[bigramWin],
                            collectionConceptFreqDic.ContainsKey(bigramWin) ? collectionConceptFreqDic[bigramWin] : 0,
                            docLength,
                            totalCollectionLength,
                            collectionIndex);

                        for (int i = 0; i < 7; ++i)
                            bigramWinDependencyFeatures[i] += weightedFeatures[i];
                    }
                }

                return unigramDependencyFeatures.Concat(bigramDependencyFeatures).Concat(bigramWinDependencyFeatures);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ". Query: " + queryWords.ConnectWords() + ". First doc: " + docWordsList[0].ConnectWords());
            }
        }

        public class DependencyModelWeights
        {
            public string Concept { get; set; }

            public long WebNGramFreq { get; set; }
            public long ExactQueryFreq { get; set; }
            public long QueryNGramFreq { get; set; }

            public long IsWikiTitle { get; set; }
            public long WikiNGramFreq { get; set; }

            public List<Pair<long, long>> ColAndDocFreqOnCollections { get; set; }

            public double[] CalcWeightedFeatures(int nGramFreq, long nGramColFreq, long docLength, long colLength, int collectionIndex)
            {
                double u = 2500;
                double numerator = nGramFreq + u * nGramColFreq / colLength;
                double denominator = docLength + u;
                double weight = denominator == 0 || numerator == 0 ? 0 : Math.Log(numerator / denominator, 2);

                double[] features = new double[7];
                int i = 2;
                //features[i++] = collectionIndex < ColAndDocFreqOnCollections.Count ? weight * ColAndDocFreqOnCollections[collectionIndex].First : 0;
                //features[i++] = collectionIndex < ColAndDocFreqOnCollections.Count ? weight * ColAndDocFreqOnCollections[collectionIndex].Second : 0;
                features[i++] = weight * nGramFreq;
                //features[i++] = weight * ExactQueryFreq;
                //features[i++] = weight * QueryNGramFreq;
                //features[i++] = weight * IsWikiTitle;
                //features[i++] = weight * WikiNGramFreq;

                return features;
            }

            public string ToRowString()
            {
                string output = Concept +
                    "\t" + WebNGramFreq +
                    "\t" + ExactQueryFreq +
                    "\t" + QueryNGramFreq +
                    "\t" + IsWikiTitle +
                    "\t" + WikiNGramFreq;

                foreach (var colAndDocFreq in ColAndDocFreqOnCollections)
                    output += "\t" + colAndDocFreq.First + "\t" + colAndDocFreq.Second;

                return output;
            }

            public static DependencyModelWeights Parse(string str)
            {
                try
                {
                    var lineArray = str.Split('\t');
                    int i = 0;
                    DependencyModelWeights output = new DependencyModelWeights()
                    {
                        Concept = lineArray[i++],
                        WebNGramFreq = long.Parse(lineArray[i++]),
                        ExactQueryFreq = long.Parse(lineArray[i++]),
                        QueryNGramFreq = long.Parse(lineArray[i++]),
                        IsWikiTitle = long.Parse(lineArray[i++]),
                        WikiNGramFreq = long.Parse(lineArray[i++]),
                        ColAndDocFreqOnCollections = new List<Pair<long, long>>()
                    };

                    while (i < lineArray.Length)
                        output.ColAndDocFreqOnCollections.Add(new Pair<long, long>(long.Parse(lineArray[i++]), long.Parse(lineArray[i++])));

                    return output;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            }
        }
    }

}
