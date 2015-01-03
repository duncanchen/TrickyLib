using System;
using System.Collections;
using System.Collections.Generic;

namespace TrickyLib.Statistics
{

    /// <summary>Beta distribution</summary>
    public static class Beta
    {
        /// <summary>Cumulative distribution function</summary>
        /// <param name="x">Value at which to compute the cdf</param>
        /// <param name="a">Shape parameter (alpha)</param>
        /// <param name="b">Shape parameter (beta)</param>
        public static double Cdf(double x, double a, double b)
        {
            if (a <= 0 || b <= 0)
                throw new ArgumentException("Beta.Cdf parameters, a and b, must be > 0");
            if (x < 0 || x > 1)
                throw new ArgumentOutOfRangeException("Beta.Cdf parameter x must be between 0 and 1");

            return BetaInc(x, a, b);
        }

        /// <summary>Probability distribution function</summary>
        /// <param name="x">Value at which to compute the pdf</param>
        /// <param name="a">Shape parameter (alpha)</param>
        /// <param name="b">Shape parameter (beta)</param>
        public static double Pdf(double x, double a, double b)
        {
            if (a <= 0 || b <= 0)
                throw new ArgumentException("Beta.Pdf parameters, a and b, must be > 0");

            if (x > 1) return 0;
            if (x < 0) return 0;

            double lnb = Utils.LogBeta(a, b);
            return Math.Exp((a - 1) * Math.Log(x) + (b - 1) * Math.Log(1 - x) - lnb);
        }


        /// <summary>Mean and variance</summary>
        /// <param name="a">Shape parameter (alpha)</param>
        /// <param name="b">Shape parameter (beta)</param>
        /// <param name="var">Output: variance</param>
        /// <returns>Mean</returns>
        public static double Stats(double a, double b, out double var)
        {
            if (a <= 0 || b <= 0)
                throw new ArgumentException("Beta.Stats parameters, a and b, must be > 0");

            double mean = a / (a + b);
            double ab = a + b;
            var = (a * b) / (ab * ab * (ab + 1));
            return mean;
        }


        /// <summary>Inverse beta cumulative probability distribution</summary>
        /// <param name="p">Probability at which to compute the inverse cdf</param>
        /// <param name="a">Shape parameter (alpha)</param>
        /// <param name="b">Shape parameter (beta)</param>
        public static double Inv(double p, double a, double b)
        {
            // We use an iterative search to find x - this is Excel's algorithm (I think)

            if (p < 0 || p > 1)
                throw new ArgumentException("Beta.Inv parameter p must be between 0 and 1.");
            if (a <= 0 || b <= 0)
                throw new ArgumentException("Beta.Inv parameters a and b must be greater than 0.");

            if (p == 0) return 0;
            if (p == 1) return 1;

            // We know that x is between 0 and 1
            double guesslo = 0;
            double guesshi = 1;

            double guess = 0;
            for (int i = 0; i < MAXITER; ++i)
            {
                // Make a guess half way between the lo and hi
                guess = (guesslo + guesshi) / 2;
                if (BetaInc(guess, a, b) > p)
                    guesshi = guess;    // we guessed too high
                else
                    guesslo = guess;    // we guessed too low

                // Convergence!
                if (guesshi - guesslo < 1e-10)
                    break;
            }

            return guess;
        }


        internal static double BetaInc(double x, double a, double b)
        {
            if (x == 0 || x == 1)
            {
                return x;
            }
            else
            {
                double c = Math.Exp(Utils.LogGamma(a + b) - Utils.LogGamma(a) - Utils.LogGamma(b) +
                           a * Math.Log(x) + b * Math.Log(1 - x));
                double p;

                if (x < (a + 1) / (a + b + 2))
                {
                    double cf = BetaCF(x, a, b);
                    p = c * cf / a;
                }
                else
                {
                    // Use symmetry relation
                    double cf = BetaCF(1 - x, b, a);
                    p = 1 - c * cf / b;
                }

                if (p >= 1.0) p = 1.0;
                return p;
            }
        }


        const int MAXITER = 400;
        const double TINY = 1.0e-30;
        const double EPS = 2.2204e-016;

        // Using the modified Lentz's method, described in section 5.2 of 
        // Numerical Recipes
        private static double BetaCF(double x, double a, double b)
        {
            double ap1 = a + 1;
            double am1 = a - 1;
            double m2 = 2;
            double ab = a + b;

            double d = 1 - ab * x / ap1;
            if (Math.Abs(d) < TINY) d = TINY;
            double c = 1;
            d = 1.0 / d;
            double cf = d;

            for (int m = 1; m <= MAXITER; ++m)
            {
                double aj = m * (b - m) * x / ((am1 + m2) * (a + m2));
                d = 1.0 + aj * d;
                if (Math.Abs(d) < TINY) d = TINY;
                c = 1 + aj / c;
                if (Math.Abs(c) < TINY) c = TINY;
                d = 1 / d;
                double delta = c * d;
                cf *= delta;

                aj = -(a + m) * (ab + m) * x / ((ap1 + m2) * (a + m2));
                d = 1.0 + aj * d;
                if (Math.Abs(d) < TINY) d = TINY;
                c = 1 + aj / c;
                if (Math.Abs(c) < TINY) c = TINY;
                d = 1 / d;
                delta = c * d;
                cf *= delta;

                if (Math.Abs(delta - 1) < EPS)
                    break;

                m2 += 2;
            }

            return cf;
        }

        static double Eval(double y, double lo, double hi, double s)
        {
            double a = lo;
            double b = hi;
            double fa = y - Gamma.Psi(a) + Gamma.Psi(a + s);
            if (fa == 0) return a;
            double fb = y - Gamma.Psi(b) + Gamma.Psi(b + s);
            if (fb == 0) return b;

            double c = a;
            double fc = fa;

            double tol = 1e-10;

            for (int i = 0; i < MAXITER; ++i)
            {
                double diff = b - a;
                if (Math.Abs(fc) < Math.Abs(fb))
                {
                    a = b;
                    b = c;
                    c = a;
                    fa = fb;
                    fb = fc;
                    fc = fa;
                }

                double tolact = 2 * EPS * Math.Abs(b) + tol / 2.0;
                double nextstep = (c - b) / 2;

                if (Math.Abs(nextstep) <= tolact || fb == 0.0)
                {
                    return b;
                }

                if (Math.Abs(diff) >= tolact && Math.Abs(fa) > Math.Abs(fb))
                {
                    double p, q;
                    double cb = c - b;
                    if (a == c)
                    {
                        double t1 = fa / fb;
                        p = cb * t1;
                        q = 1.0 - t1;
                    }
                    else
                    {
                        q = fa / fc;
                        double t1 = fb / fc;
                        double t2 = fb / fa;
                        p = t2 * (cb * q * (q - t1) - (b - a) * (t1 - 1.0));
                        q = (q - 1.0) * (t1 - 1.0) * (t2 - 1.0);
                    }

                    if (p > 0)
                        q = -q;
                    else
                        p = -p;

                    if (p < (0.75 * cb * q - Math.Abs(tolact * q) / 2) && p < Math.Abs(diff * q / 2))
                        nextstep = p / q;

                }

                if (Math.Abs(nextstep) < tolact)
                {
                    if (nextstep > 0)
                        nextstep = tolact;
                    else
                        nextstep = -tolact;
                }

                // Previous approximation
                a = b;
                fa = fb;
                b += nextstep;
                fb = y - Gamma.Psi(b) + Gamma.Psi(b + s);
                if ((fb > 0 && fc > 0) || (fb < 0 && fc < 0))
                {
                    c = a;
                    fc = fa;
                }

            }

            return b;
        }

        /*
              // Test case: [.9,.1,.3,.23]
              /// <summary>Maximum likely estimate</summary>
              /// <param name="dataIn">Data to fit</param>
              /// <param name="bhat">Shape estimate (beta)</param>
              /// <returns>Shape estimate (alpha)</returns>
              public static double MLE(IEnumerable dataIn, out double bhat)
              {
                  DoubleArray data = Matrix.ConvertToVector(dataIn);
                  if (data.Length < 2)
                      throw new ArgumentException("Beta.MLE parameter, data, must be of length > 1");

                  double meanlnp = 0;
                  double meanlp1 = 0;

                  for (int i = 0; i < data.Length; ++i)
                  {
                      if (data[i] <= 0 || data[i] >= 1)
                          throw new ArgumentException("Data for Beta.MLE must be > 0 and < 1.");

                      meanlnp += Math.Log(data[i]);
                      meanlp1 += Math.Log(1 - data[i]);
                  }

                  double cnt = (double)data.Length;
                  meanlnp /= cnt;
                  meanlp1 /= cnt;

                  double p = data.Mean();
                  double p1 = 1 - p;
                  double var = data.Var();
                  double pp = p * p1 / var - 1;

                  // Starting guesses
                  double ahat = pp * p;
                  bhat = pp * p1;

                  double toler = 1.0e-6;
            
                  double alo = 0.001;
                  double ahi = ahat * 2;
                  double blo = 0.001;
                  double bhi = bhat * 2;
            
                  for (int i = 0; i < MAXITER; ++i)
                  {
                      double ahatnew = Eval(meanlnp, alo, ahi, bhat);
                      double bhatnew = Eval(meanlp1, blo, bhi, ahat);

                      double adiff = ahatnew - ahat;
                      double bdiff = bhatnew - bhat;
                      double dist = Math.Sqrt(adiff * adiff + bdiff * bdiff);
                      ahat = ahatnew;
                      bhat = bhatnew;
                      if (dist <= toler)
                      {
                          bool fTryAgain = false;
                          // If we bump against the value of ahi or bhi, we increase it and
                          // try again. It's possible for the change in a or bhat to get very small but
                          // not have converged.
                          if (Math.Abs(ahat - ahi) <= toler)
                          {
                              ahi *= 2;
                              fTryAgain = true;
                          }
                          if (Math.Abs(bhat - bhi) <= toler)
                          {
                              bhi *= 2;
                              fTryAgain = true;
                          }

                          if (!fTryAgain)
                              break;
                      }
                  }

                  return ahat;
              }
         **/
    }

}
