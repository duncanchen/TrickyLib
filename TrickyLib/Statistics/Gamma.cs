using System;
using System.Collections;
using System.Collections.Generic;

// Gamma distribution
// [1] http://en.wikipedia.org/wiki/Gamma_distribution
// [2] "Handbook of Mathematical Functions", Abramowitz and Stegun
namespace TrickyLib.Statistics
{
    /// <summary>Gamma distribution</summary>
    public static class Gamma
    {
        /// <summary>Cumulative distribution function</summary>
        /// <param name="x">Value at which to compute the cdf</param>
        /// <param name="k">Shape parameter</param>
        /// <param name="theta">Scale parameter</param>
        public static double Cdf(double x, double k, double theta)
        {
            if (x < 0) throw new ArgumentException("Gamma.Cdf argument x must be >= 0");
            if (k <= 0 || theta <= 0) throw new ArgumentException("Gamma.Cdf arguments must be > 0");
            return GammaP(x / theta, k);
        }


        /// <summary>Probability distribution function</summary>
        /// <param name="x">Value at which to compute the cdf</param>
        /// <param name="k">Shape parameter</param>
        /// <param name="theta">Scale parameter</param>
        public static double Pdf(double x, double k, double theta)
        {
            if (x < 0)  throw new ArgumentException("Gamma.Cdf argument x must be >= 0");
            if (k <= 0 || theta <= 0) throw new ArgumentException("Gamma.Pdf arguments must be > 0");

            if (x == 0)
            {
                if (k == 1)
                    return 1 / theta;
                else
                    return 0;
            }

            // ErinRen: taking the log, then the exp is better numerically
            return Math.Exp((k - 1) * Math.Log(x) - x / theta - Utils.LogGamma(k) - k * Math.Log(theta));
        }

        // Incomplete gamma functions

        /// <summary>p-gamma function</summary>
        /// <remarks>This gammainc(x,a,'lower') or gammainc(x,a) in Matlab</remarks>
        public static double GammaP(double x, double a)
        {
            if (x < a + 1)
            {
                return GammaIncLower(x, a);
            }
            else
            {
                return 1 - GammaIncUpper(x, a);
            }
        }


        // This is gammainc(x,a,'upper') in Matlab
        /// <summary>q-gamma function</summary>
        /// <remarks>This gammainc(x,a,'upper')in Matlab</remarks>
         public static double GammaQ(double x, double a)
        {
            if (x < a + 1)
            {
                return 1 - GammaIncLower(x, a);
            }
            else
            {
                return GammaIncUpper(x, a);
            }
        }

        const int MAXITER = 400;
        const double TINY = 1.0e-30;
        const double EPS = 2.2204e-016;

        // P(a,x) using series expansion
        // See 6.5.4 and 6.5.29:
        // y(a,x) = x^a * exp(-x) * sum (Gamma(a) * z^n / Gamma(a+n+1))
        // where sum goes from 0 to infinity
        // We use 6.1.15 & 6.1.16 to compute Gamma(a)/Gamma(a+n)
        // Gamma(a) = (a-1)!
        // Gamma(a+n) = (n-1+a)*(n-2+a)...(1+a)*a! = (n-1+a)*(n-2+a)...(1+a)*a*(a-1)! 
        // Gamma(a)/Gamma(a+n) = (n-1+a)*(n-2+a)...(1+a)*a


        // This converges quickly for x < a + 1
        /// <summary>Lower incomplete gamma function</summary>
        private static double GammaIncLower(double x, double a)
        {
            double sum = 1.0 / a;
            double frac = sum;
            double an = a;

            for (int i = 1; i <= MAXITER; ++i)
            {
                an++;
                frac = x * frac / an;
                sum += frac;

                // Stopping criterion: sum won't change
                if (Math.Abs(frac) < Math.Abs(sum * EPS))
                    break;
            }

            return sum * Math.Exp(-x - Utils.LogGamma(a) + a * Math.Log(x));
        }

        // Q(a,x) using continued fractions
        // This converges quickly for x > a + 1
        // See 6.5.31 in ref [2]
        // Using the modified Lentz's method, described in section 5.2 of 
        // Numerical Recipes
         /// <summary>Upper incomplete gamma function</summary>
        private static double GammaIncUpper(double x, double a)
        {
            double f = TINY;
            double c = f;
            double d = 0;
            double aj = 1;
            double bj = x + 1 - a;
            
            for (int i = 1; i <= MAXITER; ++i)
            {
                d = bj + aj * d;
                if (Math.Abs(d) < TINY) d = TINY;
                c = bj + aj / c;
                if (Math.Abs(c) < TINY) c = TINY;
                d = 1 / d;
                double delta = c * d;
                f *= delta;

                if (Math.Abs(delta - 1) < EPS)
                    break;

                bj += 2;
                aj = -i * (i - a);
            }

            return f * Math.Exp(-x - Utils.LogGamma(a) + a * Math.Log(x));
        }

        /// <summary>Inverse cumulative distribution function</summary>
        /// <param name="p">Probability at which to compute the inverse cdf</param>
        /// <param name="k">Shape parameter</param>
        /// <param name="theta">Scale parameter</param>
        public static double Inv(double p, double k, double theta)
        {
            // We use an iterative search to find x - this is Excel's algorithm (I think)

            if (p == 0) return 0;
            if (p == 1) return Double.PositiveInfinity;

            if (p < 0 || p > 1)
                throw new ArgumentException("Gamma.Inv parameter p must be between 0 and 1.");
            if (k <= 0 || theta <= 0)
                throw new ArgumentException("Gamma.Inv parameters a and b must be greater than 0.");

            double guesslo = 0;
            double ab = k * theta;
            // What's a good initial guess when a*b < 1 ???
            double guesshi = ab > 1 ? 10 * ab : 10;

            double guess = 0;
            double pguess;
            for (int i = 0; i < 400; ++i)
            {
                guess = (guesslo + guesshi) / 2;
                pguess = Cdf(guess, k, theta);

                if (Math.Abs(p - pguess) < 1e-16)
                    break;
                
                if (pguess > p)
                    guesshi = guess;
                else
                    guesslo = guess;
            }

            return guess;
        }

         /// <summary>Mean and variance</summary>
        /// <param name="k">Shape parameter</param>
        /// <param name="theta">Scale parameter</param>
        /// <param name="var">Output: variance</param>
        /// <returns>Mean</returns>
        public static double Stats(double k, double theta, out double var)
        {
            if (k <= 0 || theta <= 0)
                throw new ArgumentException("Gamma.Stats arguments must be > 0");

            double mean = k * theta;
            var = k * theta * theta;
            return mean;
        }

        internal static double digamma(double k, out double digprime)
        {
            double dig = 0;

            if (k < 8)
            {
                dig = digamma(k + 1, out digprime);
                dig -= 1.0 / k;
                digprime += 1.0 / (k * k);
            }
            else
            {
                double ksq = k * k;
                double k3 = ksq * k;
                double k4 = ksq * ksq;
                double k5 = k4 * k;
                double k6 = ksq * k4;
                double k7 = k6 * k;

                dig =  Math.Log(k) - 1.0 / (2.0 * k) - 1.0 / (12.0 * ksq) + 1.0 / (120.0 * k4) - 1.0 / (252.0 * k6);
                digprime = 1.0 / k + 1.0 / (2.0 * ksq) + 1.0 / (6.0 * k3) - 1.0 / (30.0 * k5) + 1.0 / (42.0 * k7);
            }
            return dig;
        }

        /// <summary>Computes the psi (digamma) function</summary>
        public static double Psi(double k)
        {
            double dig = 0;

            if (k < 8)
            {
                dig = Psi(k + 1);
                dig -= 1.0 / k;
            }
            else
            {
                double ksq = k * k;
                double k3 = ksq * k;
                double k4 = ksq * ksq;
                double k5 = k4 * k;
                double k6 = ksq * k4;
                double k7 = k6 * k;

                dig = Math.Log(k) - 1.0 / (2.0 * k) - 1.0 / (12.0 * ksq) + 1.0 / (120.0 * k4) - 1.0 / (252.0 * k6);
            }
            return dig;
        }

/*
        /// <summary>Maximum likely estimate</summary>
        /// <param name="dataIn">Data to fit</param>
        /// <param name="thetahat">Output: theta estimate</param>
        /// <returns>k estimate</returns>
        public static double MLE(IEnumerable dataIn, out double thetahat)
        {
            DoubleArray data = Matrix.ConvertToVector(dataIn);
            if (data.Length < 2)
                throw new ArgumentException("Gauss.MLE parameter, data, must be of length > 1");

            double mean = data.Mean();
            double meanlog = Math.Log(mean);

            double barlogx = 0;
            for (int i = 0; i < data.Length; ++i)
            {
                barlogx += Math.Log(data[i]);
            }
            barlogx /= (double)data.Length;

            double s = meanlog - barlogx;
            double s3 = s - 3;

            // Starting guess for ahat
            double ahat = (-s3 + Math.Sqrt(s3 * s3 + 24 * s)) / (12 * s);

            for (int i = 0; i <= MAXITER; ++i)
            {
                // Update the guess
                double digprime;
                double dig = digamma(ahat, out digprime);

                double delta = (Math.Log(ahat) - dig - s) / ((1 / ahat) - digprime);

                ahat -= delta;

                if (Math.Abs(delta) < 1e-10)
                    break;
            }

            thetahat = mean / ahat;
            return ahat;
        }
 */
    }
}
