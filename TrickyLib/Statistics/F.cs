using System;
using System.Collections;
using System.Collections.Generic;

// F distribution
// See: http://en.wikipedia.org/wiki/F-distribution
namespace TrickyLib.Statistics
{
    /// <summary>F distribution</summary>
    public static class F
    {

        /// <summary>Cumulative distribution function</summary>
        /// <param name="x">Value at which to compute the cdf</param>
        /// <param name="v1">Degree of freedom parameter</param>
        /// <param name="v2">Degree of freedom parameter</param>
        public static double Cdf(double x, double v1, double v2)
        {
            if (x == 0) return 0;

            if (x < 0) throw new ArgumentException("F.Cdf parameter x must be > 0.");
            if (v1 <= 0 || v2 <= 0) throw new ArgumentException("F.Cdf parameters v1 and v2 must be > 0.");


            double vx = v1 * x;
            double k = vx / (vx + v2);
            return Beta.BetaInc(k, v1 / 2, v2 / 2);
        }


        /// <summary>Probability distribution function</summary>
        /// <param name="x">Value at which to compute the pdf</param>
        /// <param name="v1">Degree of freedom parameter</param>
        /// <param name="v2">Degree of freedom parameter</param>
        public static double Pdf(double x, double v1, double v2)
        {
            if (x == 0) return 0;

            if (x < 0) throw new ArgumentException("F.Pdf parameter x must be > 0.");
            if (v1 <= 0 || v2 <= 0) throw new ArgumentException("F.Pdf parameters v1 and v2 must be > 0.");

            double d1 = v1 * x;
            double d = d1/(d1+v2);

            double v1half = v1/2.0;
            double v2half = v2/2.0;

            return Math.Exp(v1half * Math.Log(d) + v2half * Math.Log(1 - d) - Math.Log(x) - Utils.LogBeta(v1half, v2half));
        }


        /// <summary>Inverse cumulative distribution function</summary>
        /// <param name="p">Probability at which to compute the inverse cdf</param>
        /// <param name="v1">Degree of freedom parameter</param>
        /// <param name="v2">Degree of freedom parameter</param>
        public static double Inv(double p, double v1, double v2)
        {
            if (p < 0 || p > 1)
                throw new ArgumentException("F.Inv parameter p must be between 0 and 1.");

            if (v1 <= 0 || v2 <= 0)
                throw new ArgumentException("F.Inv parameters v1 and v2 must be > 0.");

            if (p == 0) return 0;
            if (p == 1) return Double.PositiveInfinity;

            double x = Beta.Inv(1 - p, v2 / 2, v1 / 2);
            return (v2 - v2 * x) / (v1 * x);
        }

        /// <summary>Mean and variance</summary>
        /// <param name="v1">Degree of freedom parameter</param>
        /// <param name="v2">Degree of freedom parameter</param>
        /// <param name="var">Output: variance</param>
        /// <returns>Mean</returns>
        public static double Stats(double v1, double v2, out double var)
        {
            if (v2 <= 2) throw new ArgumentException("F.Stats parameter v2 must be > 2.");

            double mu = v2 / (v2 - 2);

            var = Double.NaN;
            if (v2 > 4)
                var = 2 * v2 * v2 * (v1 + v2 - 2) / (v1 * (v2 - 2) * (v2 - 2) * (v2 - 4));
            return mu;
        }
    }
}
