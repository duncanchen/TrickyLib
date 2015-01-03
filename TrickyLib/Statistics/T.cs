using System;
using System.Collections;

// Student's t-distribution
// http://en.wikipedia.org/wiki/Student's_t-distribution
namespace TrickyLib.Statistics
{
    /// <summary>Student's t-distribution</summary>
    public static class T
    {
        /// <summary>Cumulative distribution function</summary>
        /// <param name="x">Value at which to compute the cdf</param>
        /// <param name="v">Degrees of freedom parameter</param>
        public static double Cdf(double x, double v)
        {
            if (v <= 0)
                throw new ArgumentException("T.Cdf parameter v must be > 0");

            double t2 = Math.Sqrt(x * x + v);
            double val = (x + t2) / (2 * t2);
            double vhalf = v / 2;

            return Beta.BetaInc(val, vhalf, vhalf);
        }

        /// <summary>Probability distribution function</summary>
        /// <param name="x">Values at which to compute the pdf</param>
        /// <param name="v">Degrees of freedom parameter</param>
        public static double Pdf(double x, double v)
        {
            if (v <= 0)
                throw new ArgumentException("T.Pdf parameter v must be > 0");

            double v1half = (v + 1) / 2.0;
            double vhalf = v / 2.0;
            return Math.Exp(Utils.LogGamma(v1half) - v1half * Math.Log(1 + x * x / v) - Utils.LogGamma(vhalf) - Math.Log(Math.Sqrt(v * Math.PI)));
        }

 
        // Reference: "New Methods for Managing Student's T Distribution", William Shaw, April 2006
        /// <summary>Inverse cumulative distribution function</summary>
        /// <param name="p">Probability at which to compute the inverse cdf</param>
        /// <param name="v">Degrees of freedom parameter</param>
        public static double Inv(double p, double v)
        {
            if (p == 0) return Double.NegativeInfinity;
            if (p == 1) return Double.PositiveInfinity;

            if (p < 0 || p > 1)
                throw new ArgumentException("T.Inv parameter p must be between 0 and 1.");

            if (v <= 0)
                throw new ArgumentException("T.Inv parameter v must be greater than 0.");

            if (v == 1)
            {
                return Math.Tan((p - 0.5) * Math.PI);
            }
            else if (v == 2)
            {
                return (2 * p - 1) / Math.Sqrt(2 * p * (1 - p));
            }
            else if (v == 4)
            {
                double alpha = 4 * p * (1 - p);
                double sqrta = Math.Sqrt(alpha);
                double q = 4.0* Math.Cos(Math.Acos(sqrta) / 3.0) / sqrta;
                double sign = ((p - 0.5) >= 0) ? 1 : -1;
                return sign*Math.Sqrt(q - 4);
            }
            else
            {
                double sign;
                double invb;
                if (p < 0.5)
                {
                    invb = Beta.Inv(2 * p, v / 2, 0.5);
                    sign = -1;
                }
                else
                {
                    invb = Beta.Inv(2 - 2 * p, v / 2, 0.5);
                    sign = 1;
                }

                return sign * Math.Sqrt(v * (1.0 / invb - 1.0));
            }
        }

        /// <summary>Mean and variance</summary>
        /// <param name="v">Degrees of freedom parameter</param>
        ///<param name="var">Output: variance</param>
        /// <returns>Mean</returns>
        public static double Stats(double v, out double var)
        {
            // variance is not defined for v <= 2
            if (v > 2)
                var = v / (v - 2);
            else
                var = Double.NaN;

            // Mean is not defined for v <= 1
            if (v > 1)
                return 0;
            else
                return Double.NaN;
        }

    }

}
