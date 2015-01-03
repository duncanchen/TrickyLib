using System;
using System.Collections;
using System.Collections.Generic;

namespace TrickyLib.Statistics
{
    // t-test for the significance between means of two
    // independent samples. The data are assumed to come from 
    // normal distributions with unknown but equal variances.
    // http://en.wikipedia.org/wiki/Student's_t-test
    /// <summary>Two sample T-test for the significance between means of two independent samples.</summary>
    /// <remarks>The data are assumed to come from normal distributions with unknown but equal variances.</remarks>
    /// <example>
    /// <code>
    ///        
    /// // ---
    /// // nx=558474 ny=278565
    /// // mx=   0.10863966899999999427 my=   0.11034326399999999646
    /// // vx=  0.069655138790760004475 vy=  0.070807436188508890429
    ///
    /// // Create summaries of the mean, variance and size of each group. x1 is the base 
    /// MeanVariance x1 = MeanVariance.FromMeanAndVariance(0.10863966899999999427, 0.069655138790760004475, 558474);
    /// MeanVariance y1 = MeanVariance.FromMeanAndVariance(0.11034326399999999646, 0.070807436188508890429, 278565);
    ///
    /// TwoSampleTTest test1 = new TwoSampleTTest(x1, y1);
    ///
    /// System.Format("The T statistics is {0} and the p-value is {1}", test1.Statistic, test1.Probability);
    /// 
    /// // Note: Use alpha (i.e. for 95% CI use alpha = 0.05
    /// IConfidenceInterval ci = test1.ConfidenceInterval(0.05)
    /// 
    /// 
    /// System.Format("95% confidence interval is [{0},{1}]", ci.Lower, ci.Upper);
    /// 
    /// </code>
    /// </example>
    public class TwoSampleTTest
    {
        /// <summary>Tests to see if the data in x and y have the same mean</summary>
        /// <param name="x">Sample data</param>
        /// <param name="y">Sample data</param>
        /// <returns>Probability; small values indicate that the two samples have significantly means.</returns>
        /// <exception cref="ArgumentException">Throws an invalid argument exception if either group
        /// has a size <= 0
        /// </exception>
        public TwoSampleTTest(MeanVariance x, MeanVariance y)
        {
            int nx = x.Count;
            int ny = y.Count;

            if (nx <= 0)
            {
                throw new ArgumentException(String.Format("The size of the first group is {0} and should be larger than zero", nx));
            }

            if (ny <= 0)
            {
                throw new ArgumentException(String.Format("The size of the second group is {0} and should be larger than zero", ny));
            }

            /// Clone a copy of the data summaries so that this doesn't have any dependencies on 
            /// the data the user supplied. 
            this._data1 = (MeanVariance)x.Clone();
            this._data2 = (MeanVariance)y.Clone();

            double mx = x.Mean;
            double my = y.Mean;

            double varx = x.Variance;
            double vary = y.Variance;

            this._df = nx + ny - 2;

            double invNx = 1.0 / nx;
            double invNy = 1.0 / ny;

            // Compute the standard error -- the variance estimated formed by 
            // combining (pooling) the variance estimate of the groups.
            this._stdErr = Math.Sqrt((((nx - 1) * varx + (ny - 1) * vary) / this._df) * (invNx + invNy));

            if (this._stdErr > 1.0e-10)
            {
                this._tStatistic = (my - mx) / this._stdErr;

                // Use the left tail and then double that probability 
                // TODO: Add support for one-tailed hypotheses
                this._p = 2 * T.Cdf(-Math.Abs(_tStatistic), this._df);
            }
            //Special handling when both variances are 0
            else
            {
                if (Math.Abs(mx - my) < 1.0e-10)
                {
                    this._tStatistic = 0;
                    this._p = 1;
                }
                else
                {
                    if (mx > my)
                        this._tStatistic = Double.PositiveInfinity;
                    else
                        this._tStatistic = Double.NegativeInfinity;
                    this._p = 0;
                }
            }
        }

        /// <summary>
        /// Returns the test staistics (the t statistics) for the signficant test.
        /// </summary>
        public double Statistic
        {
            get { return _tStatistic; }
        }

        /// <summary>
        /// Returns the probability of seeing a value as extreme or higher as the observed 
        /// test statistic.
        /// </summary>
        public double Probability
        {
            get { return _p; }
        }

        /// <summary>
        /// Returns the degress of freedom associated with the t-test.
        /// </summary>
        public double Df
        {
            get { return _df; }
        }

        /// <summary>
        /// Returns the standard error associated with the t-test.
        /// </summary>
        public double StandardError
        {
            get { return _stdErr; }
        }


        /// <summary>
        /// Returns a confidence for the difference between the group means
        /// </summary>
        /// <param name="alpha">The confidence factor</param>
        /// <returns>Confidence intervals for the group difference</returns>
        public IConfidenceInteval ConfidenceInterval(double alpha)
        {
            // Compute the width of the confidence interval by inverting the 
            // probabiliities. 
            double width = T.Inv(1 - alpha / 2, this.Df);

            return new ConfidenceInterval(
                (this.Statistic - width) * this.StandardError,
                (this.Statistic + width) * this.StandardError,
                alpha
                );
        }

        /// <summary>
        /// Sufficient statistics for first group.
        /// </summary>
        private MeanVariance _data1;

        /// <summary>
        /// Sufficient statistics for second group.
        /// </summary>
        private MeanVariance _data2;

        /// <summary>
        /// The probaility of observing the difference between the two groups.
        /// </summary>
        private double _p;

        /// <summary>
        /// Value of the t statistics for the observed difference and sample variances
        /// </summary>
        private double _tStatistic;

        /// <summary>
        /// Degrees of freedom associated with the test. 
        /// </summary>
        private int _df;

        /// <summary>
        /// The standard error estimated formed from the two groups.
        /// </summary>
        /// <remarks>Assumes equal variances</remarks>
        private double _stdErr;

        /// <summary>
        /// Get the TTest results
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static TwoSampleTTest GetTTest(double[] array1, double[] array2)
        {
            if (array1.Length != array2.Length)
                throw new Exception("Array length did not match in TTest");

            MeanVariance mv_1 = new MeanVariance(GetMeanFromList.GetSampleMean(array1),
                                                  GetVarianceFromList.GetSumOfSquares(array1),
                                                  array1.Length);

            MeanVariance mv_2 = new MeanVariance(GetMeanFromList.GetSampleMean(array2),
                                                              GetVarianceFromList.GetSumOfSquares(array2),
                                                              array2.Length);

            return new TwoSampleTTest(mv_1, mv_2);
        }
    }
}
