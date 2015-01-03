/*
 * This code is based on the Sho Statistics package originally written by Erin Renshaw in MSR. 
 */

using System;
using System.Collections;
using System.Collections.Generic;


namespace TrickyLib.Statistics
{
      /// <summary>Utility functions for statistics</summary>
    public static class Utils
    {
        private readonly static double[] lftab = new double[] { 0, 0, Math.Log(2), Math.Log(6),
                                           Math.Log(24), Math.Log(120), Math.Log(720),
                                           Math.Log(5040), Math.Log(40320), Math.Log(362880) };

        private readonly static double[] facttab = new double[] { 1, 1, 2, 6, 24, 120, 720,
                                                         5040, 40320, 362880 };

        private readonly static double constantForLogGamma = 0.5 * Math.Log(2 * Math.PI);
        private readonly static double[] coeffsForLogGamma = { 12.0, -360.0, 1260.0, -1680.0, 1188.0 };

        #region Gamma and LogGamma

        // Use GalenA's implementation of LogGamma - it's faster!
        /// <summary>Returns the log of the gamma function</summary>
        /// <param name="x">Argument of function</param>
        /// <returns>Log Gamma(x)</returns>
        /// <remarks>Accurate to eight digits for all x.</remarks>
        public static double LogGamma(double x)
        {
            if (x <= 0.0)
                throw new ArgumentOutOfRangeException("LogGamma arg must be > 0.");

            double res = 0;
            if (x < 6)
            {
                int toAdd = (int)Math.Floor(7 - x);
                double v2 = 1;
                for (int i = 0; i < toAdd; i++)
                {
                    v2 *= (x + i);
                }
                res = -Math.Log(v2);
                x += toAdd;
            }
            x = x - 1;

            res += constantForLogGamma + (x + 0.5) * Math.Log(x) - x;

            // correction terms
            double xSquared = x * x;
            double pow = x;
            foreach (double coeff in coeffsForLogGamma)
            {
                double newRes = res + 1.0 / (coeff * pow);
                if (newRes == res)
                {
                    return res;
                }
                res = newRes;
                pow *= xSquared;
            }

            return res;
        }

        /// <summary>Computes the gamma function</summary>
        public static double GammaFunc(double x)
        {
            return Math.Exp(LogGamma(x));
        }

        #endregion

        #region Beta and LogBeta

        /// <summary>Computes the log beta function</summary>
        public static double LogBeta(double x, double y)
        {
            if (x <= 0.0 || y <= 0.0) throw new ArgumentOutOfRangeException("LogBeta args must be > 0.");
            return Utils.LogGamma(x) + Utils.LogGamma(y) - Utils.LogGamma(x + y);
        }

        /// <summary>Computes the beta function</summary>
        public static double BetaFunc(double x, double y)
        {
            if (x <= 0.0 || y <= 0.0)
                throw new ArgumentOutOfRangeException("Beta args must be > 0.");
            return Math.Exp(Utils.LogGamma(x) + Utils.LogGamma(y) - Utils.LogGamma(x + y));
        }

        #endregion

        #region Factorial and Log Factorial

        /// <summary>Computes the factorial for a value</summary>
        public static double Factorial(int k)
        {
            if (k <= 9)
            {
                return facttab[k];
            }
            else
            {
                return Math.Exp(LogGamma(k + 1));
            }
        }

        private static double FactorialD(double kd)
        {
            int k = (int)Math.Floor(kd);
            if (kd != (int)Math.Floor(kd) || kd < 0)
                throw new ArgumentException("Factorial parameter k be a positive integer.");
            return Factorial(k);
        }

        /// <summary>Computes the log(factorial)</summary>
        public static double LogFactorial(int k)
        {
            if (k <= 9)
            {
                return lftab[k];
            }
            else
            {
                return LogGamma(k + 1);
            }
        }

        private static double LogFactorialD(double kd)
        {
            int k = (int)Math.Floor(kd);

            if (kd != (int)Math.Floor(kd) || kd < 0)
                throw new ArgumentException("factln parameter k be a positive integer");

            return LogFactorial(k);
        }

        #endregion

        #region Fisher transformation

        /// <summary>Fisher transformation</summary>
        public static double FisherTrans(double x)
        {
            if (x <= -1 || x >= 1)
                throw new ArgumentException("FisherTrans parameter x  must be >= -1 and < 1.");

            return 0.5 * Math.Log((1 + x) / (1 - x));
        }

        /// <summary>Inverse Fisher transformation</summary>
        public static double FisherInv(double y)
        {
            double z = Math.Exp(2 * y);
            return (z - 1) / (z + 1);
        }

        private static double linterp(double x1, double y1, double x2, double y2, double x)
        {
            return y1 + ((x - x1) * (y2 - y1)) / (x2 - x1);
        }

        #endregion

        #region Error Functions

        private static readonly double SQRTPI = Math.Sqrt(Math.PI);

        /// <summary>Computes the error function.</summary>
        /// <param name="x">Value for which the erf is computed.</param>
        /// <returns>Error function</returns>
        public static double Erf(double x)
        {
            if (x == 0)
            {
                return 0;
            }

            return 1 - Erfc(x);
        }

        /// <summary>Computes the complementary error function.</summary>
        /// <param name="x">Value for which the erfc is computed.</param>
        /// <returns>Complementary error function</returns>
        public static double Erfc(double x)
        {
            //special cases
            if (Double.IsNegativeInfinity(x))
            {
                return 2;
            }
            if (Double.IsPositiveInfinity(x))
            {
                return 0;
            }

            double absx = Math.Abs(x);
            double val;

            double expx = Math.Exp(-absx * absx);

            if (absx > 3.0)
            {
                //Asymptotic expansion
                double xsq = absx * absx;
                double x4 = xsq * xsq;
                double x6 = x4 * xsq;
                double numerator = (1.0 - 1.0 / (2.0 * xsq) + 3.0 / (4.0 * x4) - 5.0 / (6.0 * x6));

                val = numerator * expx / (absx * SQRTPI);
            }
            else
            {
                //Algorithm 7.1.26 from Abramowitz & Stegun with |error| <= 1.5 x 10-7
                const double a1 = 0.254829592;
                const double a2 = -0.284496736;
                const double a3 = 1.421413741;
                const double a4 = -1.453152027;
                const double a5 = 1.061405429;
                const double p = 0.3275911;

                double t = 1.0 / (1.0 + (p * absx));
                double f = (((((a5 * t + a4) * t + a3) * t) + a2) * t + a1) * t;

                val = f * expx;
            }

            if (x < 0)
            {
                //The above only covers the positive case
                return 2.0 - val;
            }
            else
            {
                return val;
            }
        }

        #endregion
    }
}
