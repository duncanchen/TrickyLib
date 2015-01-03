using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpassistLib.Extension
{
    public static class Number
    {
        public static string AddZeroBefore(this short number, int bitCount)
        {
            int numberBit = number.ToString().Length;
            int needZero = bitCount - numberBit;

            string output = "";
            for (int i = 0; i < needZero; ++i)
                output += "0";

            output += number.ToString();
            return output;
        }

        public static string AddZeroBefore(this int number, int bitCount)
        {
            int numberBit = number.ToString().Length;
            int needZero = bitCount - numberBit;

            string output = "";
            for (int i = 0; i < needZero; ++i)
                output += "0";

            output += number.ToString();
            return output;
        }

        public static string AddZeroBefore(this long number, int bitCount)
        {
            int numberBit = number.ToString().Length;
            int needZero = bitCount - numberBit;

            string output = "";
            for (int i = 0; i < needZero; ++i)
                output += "0";

            output += number.ToString();
            return output;
        }
    }
}
