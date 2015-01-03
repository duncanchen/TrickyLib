using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Extension;

namespace TrickyLib.Relevance
{
    public class BM25
    {
        public static double CalcBM25(long QF, long TF, long DF, long DocLen, long TotalLen, long DocCount, bool penaltyDF = true)
        {
            double k1 = 1.2;
            double b = 0.75;
            double k2 = 1;
            double idf = Math.Log((DocCount - (penaltyDF ? DF : 0) + 0.5) / (DF + 0.5), 2);
            double factor2 = (double)TF * (k1 + 1) / (TF + k1 * (1 - b + b * DocLen * DocCount / TotalLen));
            double factor3 = (double)QF * (k2 + 1) / (QF + k2);
            return idf * factor2 * factor3;
        }

        public static double CalcBM25(Dictionary<string, int> querWordCountDic, Dictionary<string, int> docWordCountDic, Dictionary<string, long> termDocCountDic, long TotalLen, long DocCount, bool penaltyDF= true)
        {
            double bm25 = 0;
            long dl = docWordCountDic.Sum(kv => kv.Value);

            foreach (var queryWordKv in querWordCountDic)
            {
                var qf = queryWordKv.Value;
                var tf = docWordCountDic.ContainsKey(queryWordKv.Key) ? docWordCountDic[queryWordKv.Key] : 0;
                var df = termDocCountDic.ContainsKey(queryWordKv.Key) ? termDocCountDic[queryWordKv.Key] : 0;
                bm25 += CalcBM25(qf, tf, df, dl, TotalLen, DocCount, penaltyDF);
            }

            return bm25;
        }

        public static Dictionary<string, double[]> CalcBM25(string[] docs, bool penaltyDF = true)
        {
            if (docs == null || docs.Length <= 0)
                return null;

            Dictionary<string, double[]> wordbm25Dic = new Dictionary<string, double[]>();
            List<Dictionary<string, int>> docWordCountDicList = new List<Dictionary<string, int>>();
            Dictionary<string, int> totalDocFreqDic = new Dictionary<string, int>();

            foreach (var doc in docs)
            {
                //words and dic
                var docWords = TotalUtility.Tokenizer.Tokenize(doc).ToArray();
                var wordCountDic = docWords.GetWordCountDic();

                //word count dic
                docWordCountDicList.Add(wordCountDic);

                //initial word bm25
                foreach (var docWord in docWords)
                    if (!wordbm25Dic.ContainsKey(docWord))
                        wordbm25Dic.Add(docWord, new double[docs.Length]);

                //doc freq dic
                foreach (var word in wordCountDic.Keys)
                {
                    if (totalDocFreqDic.ContainsKey(word))
                        ++totalDocFreqDic[word];
                    else
                        totalDocFreqDic.Add(word, 1);
                }
            }

            long QF = 1;
            long TF;
            long DF;
            long DocLen;
            long TotalLen = docWordCountDicList.Sum(d => d.Sum(kv => kv.Value));
            long DocCount = docs.Length;

            for (int i = 0; i < docWordCountDicList.Count; ++i)
            {
                //doc length
                int docLength = docWordCountDicList[i].Sum(kv => kv.Value);
                if (docLength <= 0)
                    continue;

                DocLen = docLength;

                //for each word, sum bm25 across all the docs
                foreach (var wordKv in docWordCountDicList[i])
                {
                    TF = wordKv.Value;
                    DF = totalDocFreqDic[wordKv.Key];
                    wordbm25Dic[wordKv.Key][i] = CalcBM25(QF, TF, DF, DocLen, TotalLen, DocCount, penaltyDF);
                }
            }

            return wordbm25Dic;
        }
    }
}
