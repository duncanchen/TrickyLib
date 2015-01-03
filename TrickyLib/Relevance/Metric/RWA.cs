using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TrickyLib.MachineLearning;

namespace TrickyLib.Relevance.Metric
{
    public class RWA : BaseRankMetric
    {
        public override double Calculate(IEnumerable<int> rates)
        {
            int rank = 1;
            double numerator = 0;
            double denominator = 0;

            foreach (var rate in rates)
            {
                var log = Math.Log(rank + 1, 2);
                ++rank;

                numerator += MetricUtility.RwaRelDic[rate] / log;
                denominator += 1 / log;
            }

            if (denominator > 0)
                return numerator / denominator;
            else
                return 0;
        }

        public override double[] Calculate_TopN(IEnumerable<int> rates)
        {
            int rank = 1;
            double numerator = 0;
            double denominator = 0;
            double[] rwas = new double[MetricUtility.MaxTopN];

            foreach (var rate in rates)
            {
                if (rank > MetricUtility.MaxTopN)
                    break;

                var log = Math.Log(rank + 1, 2);

                numerator += MetricUtility.RwaRelDic[rate] / log;
                denominator += 1 / log;

                if (denominator > 0)
                    rwas[rank - 1] = numerator / denominator;
                else
                    rwas[rank - 1] = 0;

                ++rank;
            }

            return rwas;
        }
    }
}
