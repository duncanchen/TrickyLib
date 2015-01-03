using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TrickyLib.MachineLearning;

namespace TrickyLib.Relevance.Metric
{
    public class RankPrecision : BaseRankMetric
    {
        public override double[] Calculate_TopN(IEnumerable<int> rates)
        {
            double[] precN = new double[MetricUtility.MaxTopN];
            int numRelevant = 0;
            for (int iPos = 0; iPos < MetricUtility.MaxTopN && iPos < rates.Count(); ++iPos)
            {
                if (MetricUtility.RankPrecsionRelDic[rates.ElementAt(iPos)] == 1)
                    ++numRelevant;

                precN[iPos] = numRelevant * 1.0 / (iPos + 1);
            }
            return precN;
        }

        public override double Calculate(IEnumerable<int> rates)
        {
            int numRelevant = 0;
            for (int iPos = 0; iPos < MetricUtility.MaxTopN && iPos < rates.Count(); ++iPos)
            {
                if (MetricUtility.RankPrecsionRelDic[rates.ElementAt(iPos)] == 1)
                    ++numRelevant;
            }

            return numRelevant * 1.0 / rates.Count();
        }
    }
}
