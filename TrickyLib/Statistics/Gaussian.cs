using System;

namespace TrickyLib.Statistics
{
    /// <summary>
    /// Gaussian (Normal) distribution    
    /// TODO: Re-evaluate the approximation algorithms we used here. Wikipedia references a bunch of other work 
    /// (see http://en.wikipedia.org/wiki/Normal_distribution and http://www.jstatsoft.org/v11/i05/paper or http://www.wilmott.com/pdfs/090721_west.pdf in particular) 
    /// </summary>
    public static class Gaussian
    {
        /// <summary>Probability distribution function - assuming standard normal distribution (mean=0, stddev=1)</summary>
        /// <param name="x">Value at which to compute the pdf</param>
        /// <returns>Probability distribution function</returns>
        public static double Pdf(double x)
        {
            return Pdf(x, 0, 1);
        }

        /// <summary>Probability distribution function</summary>
        /// <param name="x">Value at which to compute the pdf</param>
        /// <param name="mean">Mean</param>
        /// <param name="stdDev">Standard Deviation</param>
        /// <returns>Probability distribution function</returns>
        public static double Pdf(double x, double mean, double stdDev)
        {
            ArgumentValidator.GreaterThanZero("stdDev", stdDev);

            return Math.Exp(-(Math.Pow((x - mean) / stdDev, 2.0) / 2.0)) / (SQRT2 * SQRTPI) / stdDev;
        }

        /// <summary>
        /// Cumulative distribution function
        /// </summary>
        /// <param name="x">Value at which to compute the cdf</param>
        /// <returns>Cumulative distribution function</returns>
        public static double Cdf(double x)
        {          
            return 0.5 * (Utils.Erfc(-x / SQRT2));        
        }

        /// <summary>
        /// Cumulative distribution function        
        /// </summary>
        /// <param name="x">Value at which to compute the cdf</param>
        /// <param name="mean">Mean</param>
        /// <param name="stdDev">Standard Deviation</param>
        /// <returns>Cumulative distribution function</returns>
        public static double Cdf(double x, double mean, double stdDev)
        {
            ArgumentValidator.GreaterThanZero("stdDev", stdDev);

            double z = (x - mean) / stdDev;
            return Cdf(z);
        }        

        /// <summary>
        /// Inverse of cumulative distribution function.
        /// Based on algorithm from Peter Acklam - http://home.online.no/~pjacklam/notes/invnorm/
        /// His algorithm is for the inverse normal cdf with mean=0 and stddev= 1
        /// The relative error of the approximation has absolute value less than 1.15 × 10−9
        /// </summary>
        /// <param name="p">Probability</param>
        /// <returns>Inverse of cumulative distribution function</returns>
        public static double Inv(double p)
        {
            ArgumentValidator.InRange("p", p, 0.0, 1.0, true);

            //handling of the -inf and +inf case
            if (p == 0)
            {
                return Double.NegativeInfinity;
            }
            if (p == 1)
            {
                return Double.PositiveInfinity;
            }

            return InvNorm(p);
        }

        /// <summary>
        /// Inverse of cumulative distribution function.
        /// Based on algorithm from Peter Acklam - http://home.online.no/~pjacklam/notes/invnorm/
        /// His algorithm is for the inverse normal cdf with mean=0 and stddev= 1
        /// The relative error of the approximation has absolute value less than 1.15 × 10−9
        /// </summary>
        /// <param name="p">Probability</param>
        /// <param name="mean">Mean</param>
        /// <param name="stdDev">Standard Deviation</param>
        /// <returns>Inverse of cumulative distribution function</returns>
        public static double Inv(double p, double mean, double stdDev)
        {
            ArgumentValidator.GreaterThanZero("stdDev", stdDev);
            
            double z = Inv(p);
            return z * stdDev + mean;
        }

        #region Private members

        private static readonly double SQRT2 = Math.Sqrt(2.0);
        private static readonly double SQRTPI = Math.Sqrt(Math.PI);

        /// <summary>
        /// Algorithm from Peter Acklam - http://home.online.no/~pjacklam/notes/invnorm/
        /// </summary>
        /// <param name="p">Probability</param>
        /// <returns>Inverse of cumulative distribution function</returns>
        private static double InvNorm(double p)
        {
            // Coefficients
            const double a1 = -3.969683028665376e+01;
            const double a2 =  2.209460984245205e+02;
            const double a3 = -2.759285104469687e+02;
            const double a4 =  1.383577518672690e+02;
            const double a5 = -3.066479806614716e+01;
            const double a6 =  2.506628277459239e+00;

            const double b1 = -5.447609879822406e+01;
            const double b2 =  1.615858368580409e+02;
            const double b3 = -1.556989798598866e+02;
            const double b4 =  6.680131188771972e+01;
            const double b5 = -1.328068155288572e+01;

            const double c1 = -7.784894002430293e-03;
            const double c2 = -3.223964580411365e-01;
            const double c3 = -2.400758277161838e+00;
            const double c4 = -2.549732539343734e+00;
            const double c5 =  4.374664141464968e+00;
            const double c6 =  2.938163982698783e+00;

            const double d1 =  7.784695709041462e-03;
            const double d2 =  3.224671290700398e-01;
            const double d3 =  2.445134137142996e+00;
            const double d4 =  3.754408661907416e+00;

            const double pLow  = 0.02425;
            const double pHigh = 0.97575;
           
            double x;

            if (p < pLow)
            {
                double q = Math.Sqrt(-2 * Math.Log(p));

                x = (((((c1 * q + c2) * q + c3) * q + c4) * q + c5) * q + c6) /
                     ((((d1 * q + d2) * q + d3) * q + d4) * q + 1);

            }
            else if (p >= pLow && p <= pHigh)
            {
                double q = p - 0.5;
                double r = q * q;
                x = (((((a1 * r + a2) * r + a3) * r + a4) * r + a5) * r + a6) * q /
                           (((((b1 * r + b2) * r + b3) * r + b4) * r + b5) * r + 1);
            }
            else
            {
                double q = Math.Sqrt(-2 * Math.Log(1 - p));
                x = -(((((c1 * q + c2) * q + c3) * q + c4) * q + c5) * q + c6) /
                      ((((d1 * q + d2) * q + d3) * q + d4) * q + 1);
            }

            //One iteration of Halley’s rational method (third order) gives full machine precision.
            double e = 0.5 * Utils.Erfc(-x / Math.Sqrt(2)) - p;
            double u = e * SQRT2 * SQRTPI * Math.Exp(x * x / 2);
            x = x - u / (1 + x * u / 2);

            return x;
        }

        #endregion
    }
}
