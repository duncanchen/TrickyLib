using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TrickyLib.MachineLearning;

namespace TrickyLib.Relevance.Metric
{
    public abstract class BaseRankMetric
    {
        public abstract double Calculate(IEnumerable<int> rates);
        public abstract double[] Calculate_TopN(IEnumerable<int> rates);

        public TrainTestResult GetResult(string featureFile, string outputFile)
        {
            using (StreamReader featureSr = new StreamReader(featureFile))
            using (StreamReader outputSr = new StreamReader(outputFile))
            {
                List<KeyValuePair<int, double>> labelScores = new List<KeyValuePair<int, double>>();
                int qid = -1;
                int qCount = 0;
                double totalScore = 0;

                while (!featureSr.EndOfStream && !outputSr.EndOfStream)
                {
                    string line = featureSr.ReadLine().TrimStart();
                    if (!line.StartsWith("#"))
                    {
                        double outputScore = double.Parse(outputSr.ReadLine());
                        string labelStr = Regex.Match(line, MetricUtility.LabelRegStr).Value;
                        int label = int.Parse(labelStr.Trim());

                        FeatureLine fLine = new FeatureLine(line, false, false, false);
                        if (qid != fLine.Qid)
                        {
                            if (qid != -1)
                            {
                                totalScore += Calculate(labelScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key));
                                labelScores.Clear();
                            }

                            ++qCount;
                            qid = fLine.Qid;
                        }

                        labelScores.Add(new KeyValuePair<int, double>(label, outputScore));
                    }
                }

                if (labelScores.Count > 0)
                    totalScore += Calculate(labelScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key));

                return new TrainTestResult(this.GetType().Name, totalScore / qCount);
            }
        }
        public TrainTestResult GetResult_TopN(string featureFile, string outputFile)
        {
            using (StreamReader featureSr = new StreamReader(featureFile))
            using (StreamReader outputSr = new StreamReader(outputFile))
            {
                List<KeyValuePair<int, double>> labelScores = new List<KeyValuePair<int, double>>();
                int qid = -1;
                int qCount = 0;
                double[] totalScores = new double[MetricUtility.MaxTopN];

                while (!featureSr.EndOfStream && !outputSr.EndOfStream)
                {
                    string line = featureSr.ReadLine().TrimStart();
                    if (!line.StartsWith("#"))
                    {
                        double outputScore = double.Parse(outputSr.ReadLine());
                        string labelStr = Regex.Match(line, MetricUtility.LabelRegStr).Value;
                        int label = int.Parse(labelStr.Trim());

                        //SvmLight format
                        FeatureLine fLine = new FeatureLine(line, false, false, false);
                        if (qid != fLine.Qid)
                        {
                            if (qid != -1)
                            {
                                var scores = Calculate_TopN(labelScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key));
                                for (int i = 0; i < MetricUtility.MaxTopN; ++i)
                                    totalScores[i] += scores[i];

                                labelScores.Clear();
                            }

                            ++qCount;
                            qid = fLine.Qid;
                        }

                        labelScores.Add(new KeyValuePair<int, double>(label, outputScore));
                    }
                }

                if (labelScores.Count > 0)
                {
                    var scores = Calculate_TopN(labelScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key));
                    for (int i = 0; i < MetricUtility.MaxTopN; ++i)
                        totalScores[i] += scores[i];
                }

                TrainTestResult result = new TrainTestResult();
                for (int i = 0; i < MetricUtility.MaxTopN; ++i)
                    result.Add(this.GetType().Name + "@" + (i + 1).ToString(), totalScores[i] / qCount);

                return result;
            }
        }

        public double[] GetResultArray(string featureFile, string outputFile)
        {
            using (StreamReader featureSr = new StreamReader(featureFile))
            using (StreamReader outputSr = new StreamReader(outputFile))
            {
                List<KeyValuePair<int, double>> labelScores = new List<KeyValuePair<int, double>>();
                int qid = -1;
                int qCount = 0;
                List<double> totalScores = new List<double>();

                while (!featureSr.EndOfStream && !outputSr.EndOfStream)
                {
                    string line = featureSr.ReadLine().TrimStart();
                    if (!line.StartsWith("#"))
                    {
                        double outputScore = double.Parse(outputSr.ReadLine());
                        string labelStr = Regex.Match(line, MetricUtility.LabelRegStr).Value;
                        int label = int.Parse(labelStr.Trim());

                        FeatureLine fLine = new FeatureLine(line, false, false, false);
                        if (qid != fLine.Qid)
                        {
                            if (qid != -1)
                            {
                                totalScores.Add(Calculate(labelScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key)));
                                labelScores.Clear();
                            }

                            ++qCount;
                            qid = fLine.Qid;
                        }

                        labelScores.Add(new KeyValuePair<int, double>(label, outputScore));
                    }
                }

                if (labelScores.Count > 0)
                    totalScores.Add(Calculate(labelScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key)));

                return totalScores.ToArray();
            }
        }
        public double[][] GetResultArray_TopN(string featureFile, string outputFile)
        {
            using (StreamReader featureSr = new StreamReader(featureFile))
            using (StreamReader outputSr = new StreamReader(outputFile))
            {
                List<KeyValuePair<int, double>> labelScores = new List<KeyValuePair<int, double>>();
                int qid = -1;
                int qCount = 0;
                List<double[]> totalScoresList = new List<double[]>();

                while (!featureSr.EndOfStream && !outputSr.EndOfStream)
                {
                    string line = featureSr.ReadLine().TrimStart();
                    if (!line.StartsWith("#"))
                    {
                        double outputScore = double.Parse(outputSr.ReadLine());
                        string labelStr = Regex.Match(line, MetricUtility.LabelRegStr).Value;
                        int label = int.Parse(labelStr.Trim());

                        //SvmLight format
                        FeatureLine fLine = new FeatureLine(line, false, false, false);
                        if (qid != fLine.Qid)
                        {
                            if (qid != -1)
                            {
                                var scores = Calculate_TopN(labelScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key));
                                totalScoresList.Add(scores);

                                labelScores.Clear();
                            }

                            ++qCount;
                            qid = fLine.Qid;
                        }

                        labelScores.Add(new KeyValuePair<int, double>(label, outputScore));
                    }
                }

                if (labelScores.Count > 0)
                {
                    var scores = Calculate_TopN(labelScores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key));
                    totalScoresList.Add(scores);
                }

                return totalScoresList.ToArray();
            }
        }
    }
}
