using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ExpassistLib.Extension
{
    public static class StreamIO
    {
        public static void WriteArray<T>(this StreamWriter sw, IEnumerable<T> array)
        {
            foreach (var item in array)
                sw.WriteLine(item.ToString());
        }
    }
}
