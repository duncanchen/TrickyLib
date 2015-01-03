using System;
using System.Collections.Generic;
using System.Text;

namespace TrickyLib.Statistics
{
    /// <summary>
    /// Helper class to do argument validations
    /// </summary>
    public static class ArgumentValidator
    {
        /// <summary>
        /// Throw ArgumentOutOfRangeException if data is not greater than 0
        /// </summary>
        /// <param name="paramName">Argument parameter name</param>
        /// <param name="data">Argument value</param>
        public static void GreaterThanZero(string paramName, int data)
        {
            if (data <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, "Must be greater than 0.");
            }
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException if data is not greater than 0
        /// </summary>
        /// <param name="paramName">Argument parameter name</param>
        /// <param name="data">Argument value</param>
        public static void GreaterThanZero(string paramName, double data)
        {
            if (data <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, "Must be greater than 0.");
            }
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException if data is not greater than or equal to 0
        /// </summary>
        /// <param name="paramName">Argument parameter name</param>
        /// <param name="data">Argument value</param>
        public static void GreaterThanOrEqualToZero(string paramName, int data)
        {
            if (data < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, "Must be greater than or equal to 0.");
            }
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException if data is not greater than or equal to 0
        /// </summary>
        /// <param name="paramName">Argument parameter name</param>
        /// <param name="data">Argument value</param>
        public static void GreaterThanOrEqualToZero(string paramName, double data)
        {
            if (data < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, "Must be greater than or equal to 0.");
            }
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException if data is 0
        /// </summary>
        /// <param name="paramName">Argument parameter name</param>
        /// <param name="data">Argument value</param>
        public static void NonZero(string paramName, int data)
        {
            if (data == 0)
            {
                throw new ArgumentOutOfRangeException(paramName, "Cannot be zero.");
            }
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException if data is 0
        /// </summary>
        /// <param name="paramName">Argument parameter name</param>
        /// <param name="data">Argument value</param>
        public static void NonZero(string paramName, double data)
        {
            if (data == 0)
            {
                throw new ArgumentOutOfRangeException(paramName, "Cannot be zero.");
            }
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException if not in specific range
        /// </summary>
        /// <param name="paramName">Argument parameter name</param>
        /// <param name="data">Argument value</param>
        /// <param name="lowerBound">Lower bound value</param>
        /// <param name="upperBound">Upper bound value</param>
        /// <param name="inclusive">Whether the range is inclusive or not; Inclusive means both lower and upper bounds are valid values, exclusive otherwise</param>
        public static void InRange(string paramName, double data, double lowerBound, double upperBound, bool inclusive)
        {
            if (inclusive && (data < lowerBound || data > upperBound))
            {
                throw new ArgumentOutOfRangeException(paramName, string.Format("Must be between {0} and {1}.", lowerBound, upperBound));
            }

            if (!inclusive && (data <= lowerBound || data >= upperBound))
            {
                throw new ArgumentOutOfRangeException(paramName, string.Format("Must be greater than {0} and less than {1}.", lowerBound, upperBound));
            }
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException if not in specific range
        /// </summary>
        /// <param name="paramName">Argument parameter name</param>
        /// <param name="data">Argument value</param>
        /// <param name="lowerBound">Lower bound value</param>
        /// <param name="upperBound">Upper bound value</param>
        /// <param name="inclusive">Whether the range is inclusive or not; Inclusive means both lower and upper bounds are valid values, exclusive otherwise</param>
        public static void InRange(string paramName, int data, int lowerBound, int upperBound, bool inclusive)
        {
            if (inclusive && (data < lowerBound || data > upperBound))
            {
                throw new ArgumentOutOfRangeException(paramName, string.Format("Must be between {0} and {1}.", lowerBound, upperBound));
            }

            if (!inclusive && (data <= lowerBound || data >= upperBound))
            {
                throw new ArgumentOutOfRangeException(paramName, string.Format("Must be greater than {0} and less than {1}.", lowerBound, upperBound));
            }
        }
    }
}
