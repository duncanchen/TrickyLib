using System;


namespace TrickyLib.Statistics
{
    /// <summary>
    /// Interface representing a statistic for a statistical test
    /// </summary>
    interface ITestStatistic
    {
        /// <summary>
        /// Returns the probability of observing the given value of the test statistic
        /// </summary>
        double Probability { get; }

        /// <summary>
        /// Returns the value of the test statistic
        /// </summary>
        double Statistic { get; }
    }
}
