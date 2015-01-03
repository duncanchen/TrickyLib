using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Index.Lucene.Analysis.Snowball;
using TrickyLib.Extension;

namespace TrickyLib.Relevance
{
    public class TFIDF
    {
        public static SnowballAnalyzer _tokenizer = new SnowballAnalyzer(Index.Lucene.Util.Version.LUCENE_30, "Lingo", true);

        public static Dictionary<string, double[]> CalcTFIDF(string[] docs)
        {
            if (docs == null || docs.Length <= 0)
                return null;

            Dictionary<string, double[]> wordTfidfDic = new Dictionary<string, double[]>();
            List<Dictionary<string, int>> docWordCountDicList = new List<Dictionary<string, int>>();
            Dictionary<string, int> totalDocFreqDic = new Dictionary<string, int>();

            foreach (var doc in docs)
            {
                //words and dic
                var docWords = _tokenizer.Tokenize(doc).ToArray();
                var wordCountDic = docWords.GetWordCountDic();

                //word count dic
                docWordCountDicList.Add(wordCountDic);

                //initial word tf-idf
                foreach (var docWord in docWords)
                    if(!wordTfidfDic.ContainsKey(docWord))
                        wordTfidfDic.Add(docWord, new double[docs.Length]);

                //doc freq dic
                foreach (var word in wordCountDic.Keys)
                {
                    if (totalDocFreqDic.ContainsKey(word))
                        ++totalDocFreqDic[word];
                    else
                        totalDocFreqDic.Add(word, 1);
                }
            }

            for (int i = 0; i < docWordCountDicList.Count; ++i)
            {
                //doc length
                int docLength = docWordCountDicList[i].Sum(kv => kv.Value);
                if (docLength <= 0)
                    continue;

                //for each word, sum tf-idf across all the docs
                foreach (var wordKv in docWordCountDicList[i])
                {
                    double tf = wordKv.Value * 1.0 / docLength;
                    double idf = Math.Log(docs.Length / (1 + totalDocFreqDic[wordKv.Key]));
                    double tfidf = tf * idf;

                    wordTfidfDic[wordKv.Key][i] = tfidf;
                }
            }

            return wordTfidfDic;
        }
    }
}
