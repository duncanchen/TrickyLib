using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib.Relevance.Metric
{
    public class MetricUtility
    {
        public static Dictionary<int, int> NdcgRelDic;
        public static Dictionary<int, int> RankPrecsionRelDic;
        public static Dictionary<int, int> RwaRelDic;
        public static int MaxTopN = 20;
        public static double PositiveThreshold = 0;
        public const string LabelRegStr = @"^[\+-]?\d+(\s|$)";


        static MetricUtility()
        {
            NdcgRelDic = new Dictionary<int, int>();
            NdcgRelDic.Add(4, 15);
            NdcgRelDic.Add(3, 7);
            NdcgRelDic.Add(2, 3);
            NdcgRelDic.Add(1, 1);
            NdcgRelDic.Add(0, 0);

            RankPrecsionRelDic = new Dictionary<int, int>();
            RankPrecsionRelDic.Add(4, 1);
            RankPrecsionRelDic.Add(3, 1);
            RankPrecsionRelDic.Add(2, 1);
            RankPrecsionRelDic.Add(1, 0);
            RankPrecsionRelDic.Add(0, 0);

            RwaRelDic = new Dictionary<int, int>();
            RwaRelDic.Add(4, 3);
            RwaRelDic.Add(3, 2);
            RwaRelDic.Add(2, 1);
            RwaRelDic.Add(1, 0);
            RwaRelDic.Add(0, -1);
        }
    }
}
