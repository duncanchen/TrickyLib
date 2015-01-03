using System;
using System.Collections;
using System.Collections.Generic;
//using MS.Internal.LiveSearch.MathLib.Matrix;

// Poisson distribution
// http://en.wikipedia.org/wiki/Poisson_distribution
namespace TrickyLib.Statistics
{
    /// <summary>Poisson distribution</summary>
    public static class Poisson
    {
        /// <summary>Cumulative distribution function</summary>
        /// <param name="x">Value at which to compute the cdf</param>
        /// <param name="lambda">lambda parameter: expected number of occurrences in a given interval</param>
        public static double Cdf(double x, double lambda)
        {
            ArgumentValidator.GreaterThanOrEqualToZero("lambda", lambda);

            if (lambda == 0)
            {
                if (x == 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            
            int fx = (int)Math.Floor(x);
            return CdfFast(fx, lambda);
        }       

        private static double CdfFast(int x, double lambda)
        {
            // Galen Andrew's suggestion: use the incomplete gamma function to compute 
            // the cdf (http://en.wikipedia.org/wiki/Incomplete_Gamma_function), as explained in the reference above 
            // & Numerical Recipes (sec 6.2 on page p 214 http://www.mpi-hd.mpg.de/astrophysik/HEA/internal/Numerical_Recipes/f6-2.pdf.)
            return Gamma.GammaQ(lambda, x + 1);
        }

        //private static double CdfSlow(double x, double lambda)
        //{
        //    double sum = 0;
        //    int fx = (int)Math.Floor(x);

        //    for (int i = 0; i <= fx; ++i)
        //        sum += Poisson.Pdf(i, lambda);
        //    if (sum > 1) sum = 1;
        //    return sum;
        //}

        /// <summary>Probability distribution function</summary>
        /// <param name="x">Value at which to compute the pdf</param>
        /// <param name="lambda">lambda parameter: expected number of occurrences in a given interval</param>
        public static double Pdf(double x, double lambda)
        {
            ArgumentValidator.GreaterThanOrEqualToZero("lambda", lambda);

            if (lambda == 0)
            {
                if (x == 0) 
                { 
                    return 1; 
                }
                else 
                { 
                    return 0; 
                }
            }

            // Return 0 for non-integers
            int n = (int) Math.Floor(x);
            if (x != n)
            {
                return 0;
            }

            // Use n! = Gamma(n+1).
            return Math.Exp(-lambda - Utils.LogGamma(x + 1) + n * Math.Log(lambda));
        }

        /// <summary>Inverse cumulative distribution function</summary>
        /// <param name="p">Probability at which to compute the inverse cdf</param>
        /// <param name="lambda">lambda parameter: expected number of occurrences in a given interval</param>
        public static double Inv(double p, double lambda)
        {
            ArgumentValidator.InRange("p", p, 0, 1, true);
            ArgumentValidator.GreaterThanOrEqualToZero("lambda", lambda);

            if (lambda == 0)
            {
                return 0;
            }

            if (p == 1)
            {
                return Double.PositiveInfinity;
            }

            // Try an iterative approach to cut down on the number of function calls??
            int x = 0;
            double sum = 0;
            for (int i = 0; ; ++i)
            {
                // Find the value of x for which the cdf is >= p
                // Instead of calling cdf directly, we can just accumulate the sum of the pdf
                sum += Poisson.Pdf(i, lambda);
                if (sum >= p)
                {
                    x = i;
                    break;
                }
            }
            return x;
        }
    }
}
