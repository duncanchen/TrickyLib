using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib.Statistics
{
    public class GetVarianceFromList
    {
        public static double GetSampleVariance(double[] list) {
            double mean = GetMeanFromList.GetSampleMean(list);
            double var = 0.0;
            for (int i = 0; i < list.Length; i++) {
                if (list[i] != double.NaN && (!double.IsNegativeInfinity(list[i])) && (!double.IsPositiveInfinity(list[i])))
                    var += (mean - list[i] + 0.0) * (mean - list[i] + 0.0);
            }
            if (list.Length > 1)
                return var / ((double)list.Length - 1);
            else
                return var;
        }

        public static double GetSumOfSquares(double[] list) {
            double mean = GetMeanFromList.GetSampleMean(list);
            double var = 0.0;
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != double.NaN)
                    var += (mean - list[i] + 0.0) * (mean - list[i] + 0.0);
            }
            return var;
        }
    }
}
