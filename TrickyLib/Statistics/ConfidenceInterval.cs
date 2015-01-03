using System;
using System.Collections.Generic;
using System.Text;

namespace TrickyLib.Statistics
{
    /// <summary>
    /// Interface rerepresenting a simple univariante confidence interval with 
    /// lower and upper bounds bsaed on a confidence factor (<see>Alpha</see>
    /// </summary>
    public interface IConfidenceInteval
    {
        /// <summary>
        /// The lower bound of the confidence interval. 
        /// </summary>
        double Lower { get; }

        /// <summary>
        /// The upper bound of the confidence interval. 
        /// </summary>
        double Upper { get; }

        /// <summary>
        /// Returns the confience factor (alpha) value used to compute the confidence interval
        /// </summary>
        double Alpha { get; }
    }

    /// <summary>
    /// Simple container class to hold the information for a confidence interval.
    /// </summary>
    public class ConfidenceInterval : IConfidenceInteval
    {
        public ConfidenceInterval(double lower, double upper, double alpha)
        {
            this._lower = lower;
            this._upper = upper;
            this._alpha = alpha;
        }
        /// <summary>
        /// The lower bound of the confidence interval. 
        /// </summary>
        public double Lower 
        { 
            get
            {
                return _lower;
            }
        }

        /// <summary>
        /// The upper bound of the confidence interval. 
        /// </summary>
        public double Upper 
        { 
            get
            {
                return _upper;
            }
        }

        /// <summary>
        /// Returns the confience factor (alpha) value used to compute the confidence interval
        /// </summary>
        public double Alpha 
        { 
            get
            {
                return _alpha;
            }
        }

        private double _alpha;
        private double _lower;
        private double _upper;
    }
}
