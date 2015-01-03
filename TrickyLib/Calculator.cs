using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Struct;

namespace TrickyLib
{
    public class Calculator
    {
        public static double SignTest(int win, int loss)
        {
            if (win <= 0 || loss <= 0)
                throw new Exception("Win and loss should not less than 0");

            int N = win + loss;
            var min = Math.Min(win, loss);

            double sum = 0;
            for (int i = 0; i <= min; i++)
                sum += Factorial(N) / Factorial(i) / Factorial(N - i) / 4;

            return 2 * sum;
        }

        public static Pair<int, int> SignTest(double[] array1, double[] array2, double tolerance = 0)
        {
            if (array1.Length != array2.Length)
                throw new Exception("The arrays length do not match");

            int win = 0;
            int loss = 0;

            for (int i = 0; i < array1.Length; i++)
            {
                double diff = array1[i] - array2[i];
                if (Math.Abs(diff) > Math.Abs(tolerance))
                {
                    if (diff > 0)
                        win++;
                    else
                        loss++;
                }
            }

            return new Pair<int, int>(win, loss);
        }

        public static double Factorial(int N)
        {
            if (N < 0)
                throw new Exception("Cannot calculate factorial for non-positive number");

            long result = 1;
            for (int i = 1; i <= N; i++)
                result *= i;

            return result;
        }

        public static double Normalize(double number, double min, double max)
        {
            if (min == max)
            {
                if (number == 0)
                    return 0;
                else if (number > 0)
                    return 1;
                else
                    return -1;
            }
            if (min > 0)
                min = 0;

            return (number - min) / (max - min);
        }
    }
}
