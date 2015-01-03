using System;
using System.Collections;
using System.Collections.Generic;

// Chi-square distribution
// See http://en.wikipedia.org/wiki/Chi-square_distribution
// It's just a special case of the gamma distribution with a = v/2 and b = 2
namespace TrickyLib.Statistics
{
    /// <summary>Chi-square distribution</summary>
    public static class Chi2
    {
        /// <summary>Cumulative distribution function</summary>
        /// <param name="x">Value at which to compute the cdf</param>
        /// <param name="v">Degrees of freedom</param>
        public static double Cdf(double x, double v)
        {
            if (v <= 0) throw new ArgumentException("Chi2.Cdf parameter v must be > 0.");
            return Gamma.Cdf(x, v / 2, 2);
        }

        /// <summary>Probability distribution function</summary>
        /// <param name="x">Value at which to compute the pdf</param>
        /// <param name="v">Degrees of freedom</param>
        public static double Pdf(double x, double v)
        {
            if (v <= 0) throw new ArgumentException("Chi2.Pdf parameter v must be > 0.");
            return Gamma.Pdf(x, v / 2, 2);
        }

        /// <summary>Inverse cdf</summary>
        public static double Inv(double p, double v)
        {
            if (v <= 0)
                throw new ArgumentException("Chi2.Inv parameter v must be > 0.");

            if (p < 0 || p > 1)
                throw new ArgumentException("Chi2.Inv parameter p must be between 0 and 1.");

            return Gamma.Inv(p, v / 2, 2);
        }

        /// <summary>Mean and variance</summary>
        public static double Stats(double v, out double var)
        {
            if (v <= 0)
                throw new ArgumentException("Chi2.Stats parameter v must be > 0.");
            var = 2 * v;
            return v;
        }
    }
}
