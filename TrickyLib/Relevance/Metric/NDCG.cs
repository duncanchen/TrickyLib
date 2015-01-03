using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TrickyLib.MachineLearning;

namespace TrickyLib.Relevance.Metric
{
    public class NDCG : BaseRankMetric
    {
        public override double Calculate(IEnumerable<int> rates)
        {
            var dcg = CalcDCG(rates);
            var idcg = CalcDCG(rates.OrderByDescending(r => r));
            if (idcg == 0)
                return 0;
            else
                return dcg / idcg;
        }

        public override double[] Calculate_TopN(IEnumerable<int> rates)
        {
            var dcg = CalcDCG_TopN(rates);
            if (dcg == null)
                return new double[MetricUtility.MaxTopN];

            var bestDcg = CalcDCG_TopN(rates.OrderByDescending(r => r));
            double[] ndcg = new double[MetricUtility.MaxTopN];

            for (int iPos = 0; iPos < MetricUtility.MaxTopN && iPos < rates.Count(); ++iPos)
                if (bestDcg[iPos] != 0)
                    ndcg[iPos] = dcg[iPos] / bestDcg[iPos];

            return ndcg;
        }

        public double CalcDCG(IEnumerable<int> rates)
        {
            if (rates == null || rates.Count() <= 0)
                return 0;

            double dcg = MetricUtility.NdcgRelDic[rates.ElementAt(0)];

            for (int iPos = 1; iPos < MetricUtility.MaxTopN && iPos < rates.Count(); ++iPos)
            {
                if (iPos < 2)
                    dcg += MetricUtility.NdcgRelDic[rates.ElementAt(iPos)];
                else
                    dcg += MetricUtility.NdcgRelDic[rates.ElementAt(iPos)] / Math.Log(iPos + 1.0, 2);
            }

            return dcg;
        }

        public double[] CalcDCG_TopN(IEnumerable<int> rates)
        {
            if (rates == null || rates.Count() <= 0)
                return null;

            double[] dcg = new double[MetricUtility.MaxTopN];
            dcg[0] = MetricUtility.NdcgRelDic[rates.ElementAt(0)];

            for (int iPos = 1; iPos < MetricUtility.MaxTopN && iPos < rates.Count(); ++iPos)
            {
                if (iPos < 2)
                    dcg[iPos] = dcg[iPos - 1] + MetricUtility.NdcgRelDic[rates.ElementAt(iPos)];
                else
                    dcg[iPos] = dcg[iPos - 1] + MetricUtility.NdcgRelDic[rates.ElementAt(iPos)] / Math.Log(iPos + 1.0, 2);
            }

            return dcg;
        }
    }
}
