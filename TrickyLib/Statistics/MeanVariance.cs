using System;
using System.Collections.Generic;

namespace TrickyLib.Statistics
{
    /// <summary>
    /// Class for representing a dataset via the sufficient statistics of its mean and variance. 
    /// </summary>
    public class MeanVariance : ICloneable
    {
        /// <summary>
        /// Constructor for a mean and variance summary of a dataset. 
        /// </summary>
        /// <param name="mean">Mean of data</param>
        /// <param name="sumOfSquares">Sum Of Squares</param>
        /// <param name="count">Sample size of data</param>
        public MeanVariance(double mean, double sumOfSquares, int count)
        {
            _mean = mean;
            _m2 = sumOfSquares;
            _count = count;
        }

        /// <summary>
        /// Returns the sample variance (i.e. the sum of differnces from mean divided by n-1). 
        /// </summary>
        public double Variance
        {
            get { return _m2 / (_count - 1); }
        }

        /// <summary>
        /// Returns the mean. 
        /// </summary>
        public double Mean
        {
            get { return _mean; }
        }

        /// <summary>
        /// Returns the size of the sample for which the mean and variance was computed. 
        /// </summary>
        public double SumOfSquares
        {
            get { return _m2; }
        }

        /// <summary>
        /// Returns the size of the sample for which the mean and variance was computed. 
        /// </summary>
        /// <remarks>
        /// Note that the use of a real (<c>double</c>) representation here is intentional
        /// to allow weighted samples. 
        /// </remarks>
        public int Count
        {
            get { return _count; }
        }

        public override string ToString()
        {
            return String.Format("MeanVariance(mean={0}, sumofsq={1}, count={2})",
                this.Mean, this.SumOfSquares, this.Count);
        }

        /// <summary>
        /// Returns a mean and variance summary from sufficients statistics expressed as a combination
        /// of a sum_x, sum_x^2, and count.
        /// </summary>
        /// <param name="sum">Sum of sample data points</param>
        /// <param name="sumOfSq">Sum of each sample data point squared (i..e sum_xi^2</param>
        /// <param name="count"></param>
        /// <returns>Representation of sample in term of mean, variance, and count</returns>
        public static MeanVariance FromSummations(double sum, double sumOfSquares, int count)
        {
            double mean = sum / count;
            double ss = sumOfSquares - sum * sum / count;

            return new MeanVariance(mean, ss, count);
        }

        /// <summary>
        /// Returns a mean variance summary from a combination of mean, variance, and count. 
        /// </summary>
        /// <param name="mean">Value of sample mean</param>
        /// <param name="variance">Value of sample variance (with n-1 in the denominator</param>
        /// <param name="count">Sample size</param>
        /// <returns></returns>
        public static MeanVariance FromMeanAndVariance(double mean, double variance, int count)
        {
            double ss = variance * (count - 1);

            return new MeanVariance(mean, ss, count);
        }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }

        double _m2;
        double _mean;
        int _count;
    }

    /// <summary>
    /// Classs for accumulating observations using a numerically stable algorithm that 
    /// accumulates the mean 
    /// </summary>
    /// <seealso>
    /// http://research.microsoft.com/en-us/um/cambridge/projects/infernet/codedoc/html/T_MicrosoftResearch_Infer_Maths_MeanVarianceAccumulator.htm
    /// </seealso>
    /// <remarks>
    /// The algorithm here is based on referneces found from: 
    /// http://en.wikipedia.org/wiki/Algorithms_for_calculating_variance
    /// including: 
    /// Chan, Tony F.; Golub, Gene H.; LeVeque, Randall J. (1979), "Updating Formulae and a Pairwise Algorithm for Computing Sample Variances.", Technical Report STAN-CS-79-773, Department of Computer Science, Stanford University 
    /// ftp://reports.stanford.edu/pub/cstr/reports/cs/tr/79/773/CS-TR-79-773.pdf
    /// and 
    /// Pébay, Philippe (2008), "Formulas for Robust, One-Pass Parallel Computation of Covariances and Arbitrary-Order Statistical Moments", Technical Report SAND2008-6212, Sandia National Laboratories  
    /// http://prod.sandia.gov/techlib/access-control.cgi/2008/086212.pdf
    /// </remarks>
    /// 
    public class MeanVarianceAccumulator
    {
        /// <summary>
        /// Constructs an instance of a mean variance accumulator set to an empty state 
        /// </summary>
        public MeanVarianceAccumulator()
        {
            this._mean = 0.0;
            this._m2 = 0.0;
            this._count = 0;
        }
        /// <summary>
        /// Returns the sample variance (estimated with a factor of n-1)
        /// </summary>
        public double Variance
        {
            get
            {
                return this.Count <= 1 ? 0.0 : this.SumOfSquares / (this.Count - 1);
            }
        }

        /// <summary>
        /// Returns the mean. 
        /// </summary>
        public double Mean
        {
            get { return _mean; }
        }

        /// <summary>
        /// Returns the size of the sample for which the mean and variance was computed. 
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// Returns the size of the sample for which the mean and variance was computed. 
        /// </summary>
        public double SumOfSquares
        {
            get { return _m2; }
        }

        public MeanVariance Summary
        {
            get
            {
                return new MeanVariance(this.Mean, this.SumOfSquares, this.Count);
            }
        }

        public void Clear()
        {
            this._mean = 0;
            this._m2 = 0.0;
            this._count = 0;
        }

        public override string ToString()
        {
            return this.Summary.ToString();
        }

        /// <summary>
        /// Adds all of the values into the accumulator. 
        /// </summary>
        /// <param name="collection">Enumerable collection of values to accumulate</param>
        public void Add(IEnumerable<double> collection)
        {
            // TODO: Can optimize for special cases and use recursion (the pairwise algorithm)
            // to compute the sums if know 

            foreach (double value in collection)
            {
                this.Add(value);
            }
        }

        /// <summary>
        /// Adds the value in the accumulator. 
        /// </summary>
        /// <param name="value">Value to accumulate</param>
        public void Add(double value)
        {
            // This formula is from Pebay technical report refernced above.
            double delta = value - this._mean;

            this._count++;
            this._mean += delta / this._count; // NOTE: Must be the new value of the count 
            this._m2 += delta * (value - this._mean); // Critical to use the new mean here. delta is old mean.
        }

        /// <summary>
        /// Combines two accumulators into a single accumulator. The effects is as if
        /// the combined observations seen by both accumulators had been seen by a single 
        /// accumulator. 
        /// </summary>
        /// <param name="accumulator"></param>
        public void Add(MeanVarianceAccumulator accumulator)
        {
            // Dont' allow an accumualtor to accumulator into itself
            if (accumulator == this)
            {
                throw new InvalidOperationException("Cannot accumualtor an accumualtor into itself");
            }

            // Handle empty accmulator as special case where we just fold the 
            // other accumulator into this one as as.
            if (this.Count == 0)
            {
                this._count = accumulator.Count;
                this._mean = accumulator.Mean;
                this._m2 = accumulator.SumOfSquares;
            }
            else
            {
                // This formula is from Pebay technical report refernced above.
                int n1 = this.Count;
                int n2 = accumulator.Count;

                double delta = accumulator.Mean - this.Mean;

                this._count += n2;
                this._mean += n2 * delta / this._count;
                this._m2 += accumulator.SumOfSquares + n1 * n2 * delta*delta / this._count; // _count = n1 + n2 at this point 
            }
        }

        /// <summary>
        /// Sample size accumulated so far
        /// </summary>
        private int _count = 0;

        /// <summary>
        /// Mean accumulated so 
        /// </summary>
        private double _mean = 0.0;

        /// <summary>
        /// M2 is the sum of squares so far. That is, sigma(i, (xi - mean)**2. The nototation comes 
        /// from Wikipedia. M2 is also called the second-order central moment. 
        /// </summary>
        private double _m2 = 0.0;
    }
}
