using System;

namespace TrickyLib.Statistics
{
    /// <summary>
    /// One- or Two-Tailed test
    /// </summary>
    public enum Tail
    {
        OneTailed,
        TwoTailed
    }

    /// <summary>
    /// This class performs power analysis using Normal Distribution
    /// Design Doc: http://sharepoint/sites/dmsq/Design%20Documents/Design%20Doc%20-%20Power%20Analysis%20for%20Experimentation.docx
    /// </summary>
    public static class PowerAnalysisNormalDist
    {
        /// <summary>
        /// Get sample size of group 2, assuming unbalanced data
        /// </summary>
        /// <param name="group1SampleSize">Group 1 sample size</param>
        /// <param name="relativeDelta">Relative delta</param>
        /// <param name="mean">Mean</param>
        /// <param name="stdDev">Standard Deviation</param>
        /// <param name="power">Power</param>
        /// <param name="sigLevel">Significant Level (alpha)</param>
        /// <param name="tail">One- or Two-tailed</param>
        /// <returns>Required sample size for group 2</returns>
        public static int GetRequiredSampleSize(int group1SampleSize, double relativeDelta, double mean, double stdDev, double power, double sigLevel, Tail tail)
        {
            // Argument validations
            ArgumentValidator.GreaterThanZero("group1SampleSize", group1SampleSize);
            ArgumentValidator.NonZero("relativeDelta", relativeDelta);
            ArgumentValidator.InRange("power", power, 0, 1, false);
            ArgumentValidator.NonZero("mean", mean);
            ArgumentValidator.GreaterThanZero("stdDev", stdDev);
            ArgumentValidator.InRange("sigLevel", sigLevel, 0, 1, false);

            // Calc n2
            double delta = Math.Abs(relativeDelta * mean);
            double a = (tail == Tail.OneTailed ? sigLevel : sigLevel / 2.0);
            double zb = Math.Abs((Gaussian.Inv(1.0 - power)));
            double za = Math.Abs(Gaussian.Inv(1.0 - a));
            double n2 = 1.0 / ( (Math.Pow(delta, 2.0) / (Math.Pow(stdDev * (zb + za), 2.0))) - 1.0 / group1SampleSize);

            // n2 must be > 0; if it is not, then n1 must have been too big, or delta being too small
            if (n2 <= 0)
            {
                throw new PowerAnalysisInvalidInputException(string.Format("Cannot compute group 2 sample size. Either delta (relativeDelta x mean) or group1SampleSize is too small."));
            }

            // round it up to int
            return (Int32) Math.Ceiling(n2);
        }

        /// <summary>
        /// Get relative delta for the given sample sizes and power
        /// </summary>
        /// <param name="group1SampleSize">Group 1 sample size</param>
        /// <param name="group2SampleSize">Group 2 sample size</param>
        /// <param name="mean">Mean</param>
        /// <param name="stdDev">Standard Deviation</param>
        /// <param name="power">Power</param>
        /// <param name="sigLevel">Significant Level (alpha)</param>
        /// <param name="tail">One- or Two-tailed</param>
        /// <returns>Relative Delta</returns>
        public static double GetRelativeDelta(int group1SampleSize, int group2SampleSize, double mean, double stdDev, double power, double sigLevel, Tail tail)
        {
            // Argument validations
            ArgumentValidator.GreaterThanZero("group1SampleSize", group1SampleSize);
            ArgumentValidator.GreaterThanZero("group2SampleSize", group2SampleSize);
            ArgumentValidator.InRange("power", power, 0, 1, false);
            ArgumentValidator.NonZero("mean", mean);
            ArgumentValidator.GreaterThanZero("stdDev", stdDev);
            ArgumentValidator.InRange("sigLevel", sigLevel, 0, 1, false);           

            // Calculate delta percentage
            double a = (tail == Tail.OneTailed ? sigLevel : sigLevel / 2.0);
            double zb = Math.Abs((Gaussian.Inv(1.0 - power)));
            double za = Math.Abs(Gaussian.Inv(1.0 - a));
            double delta = (za + zb) * stdDev * Math.Sqrt(1.0 / group1SampleSize + 1.0 / group2SampleSize);
            double relativeDelta = delta / mean;

            return relativeDelta;
        }

        /// <summary>
        /// Get power for the given sample sizes to detect delta
        /// </summary>
        /// <param name="group1SampleSize">Group 1 sample size</param>
        /// <param name="group2SampleSize">Group 2 sample size</param>
        /// <param name="relativeDelta">Relative delta</param>
        /// <param name="mean">Mean</param>
        /// <param name="stdDev">Standard Deviation</param>        
        /// <param name="sigLevel">Significant Level (alpha)</param>
        /// <param name="tail">One- or Two-tailed</param>
        /// <returns>Power</returns>
        public static double GetPower(int group1SampleSize, int group2SampleSize, double relativeDelta, double mean, double stdDev, double sigLevel, Tail tail)
        {
            // Argument validations
            ArgumentValidator.GreaterThanZero("group1SampleSize", group1SampleSize);
            ArgumentValidator.GreaterThanZero("group2SampleSize", group2SampleSize);
            ArgumentValidator.NonZero("relativeDelta", relativeDelta);
            ArgumentValidator.NonZero("mean", mean);
            ArgumentValidator.GreaterThanZero("stdDev", stdDev);
            ArgumentValidator.InRange("sigLevel", sigLevel, 0, 1, false);

            // Calculate Power
            double delta = relativeDelta * mean;
            double a = (tail == Tail.OneTailed ? sigLevel : sigLevel / 2.0);            
            double za = Math.Abs(Gaussian.Inv(1.0 - a));
            double zb = za - (delta / (stdDev * Math.Sqrt(1.0 / group1SampleSize + 1.0 / group2SampleSize)));
            double power = 1 - Gaussian.Cdf(zb);

            return power;
        }
    }

    [Serializable]
    public class PowerAnalysisInvalidInputException: Exception
    {
        public PowerAnalysisInvalidInputException(string message) 
            : base(message)
        {
        }

        public PowerAnalysisInvalidInputException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
