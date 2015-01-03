using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TrickyLib.MachineLearning;

namespace TrickyLib.Relevance.Metric
{
    public class MAP : BaseRankMetric
    {
        public override double Calculate(IEnumerable<int> rates)
        {
            int numRelevant = 0;
            double avgPrecision = 0.0;
            for (int iPos = 0; iPos < rates.Count(); iPos++)
            {
                if (MetricUtility.RankPrecsionRelDic[rates.ElementAt(iPos)] == 1)
                {
                    ++numRelevant;
                    avgPrecision += numRelevant * 1.0 / (iPos + 1);
                }
            }
            if (numRelevant == 0)
                return 0.0;

            return avgPrecision / numRelevant;
        }

        public override double[] Calculate_TopN(IEnumerable<int> rates)
        {
            int numRelevant = 0;
            double avgPrecision = 0.0;
            double[] maps = new double[MetricUtility.MaxTopN];

            for (int iPos = 0; iPos < rates.Count() && iPos < MetricUtility.MaxTopN; iPos++)
            {
                if (MetricUtility.RankPrecsionRelDic[rates.ElementAt(iPos)] == 1)
                {
                    ++numRelevant;
                    avgPrecision += numRelevant * 1.0 / (iPos + 1);
                }

                if (numRelevant == 0)
                    maps[iPos] = 0;
                else
                    maps[iPos] = avgPrecision / numRelevant;
            }

            return maps;
        }
    }
}
