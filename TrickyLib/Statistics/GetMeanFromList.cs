using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib.Statistics
{
    public class GetMeanFromList
    {
        public static double GetSampleMean(double[] input) {

            if (input.Length <= 0) { return 0; }

            double mean = 0.0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != double.NaN && (!double.IsNegativeInfinity(input[i])) && (!double.IsPositiveInfinity(input[i])))
                    mean += input[i];
                if (double.IsNegativeInfinity(mean)) {
                    Console.WriteLine();
                }
            }
            return mean / input.Length;
        }
    }
}
