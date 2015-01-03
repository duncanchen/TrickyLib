using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpassistLib.Extension
{
    public static class HashExtension
    {
        public static HashSet<int> Or(this HashSet<int> A, HashSet<int> B)
        {
            HashSet<int> output = new HashSet<int>();
            foreach (var a in A)
                if (!output.Contains(a))
                    output.Add(a);

            foreach (var b in B)
                if (!output.Contains(b))
                    output.Add(b);

            return output;
        }

        public static HashSet<int> Plus(this HashSet<int> A, int b)
        {
            HashSet<int> output = new HashSet<int>();
            foreach (var a in A)
            {
                int result = a + b;
                if (!output.Contains(result))
                    output.Add(result);
            }
            return output;
        }

        public static HashSet<int> And(this HashSet<int> A, HashSet<int> B)
        {
            HashSet<int> output = new HashSet<int>();
            foreach (var a in A)
                if (!output.Contains(a) && B.Contains(a))
                    output.Add(a);

            foreach (var b in B)
                if (!output.Contains(b) && A.Contains(b))
                    output.Add(b);

            return output;
        }

        public static HashSet<int> Multiply(this HashSet<int> A, int b)
        {
            HashSet<int> output = new HashSet<int>();
            foreach (var a in A)
            {
                int result = a * b;
                if (!output.Contains(result))
                    output.Add(result);
            }
            return output;
        }

        public static HashSet<int> Scale(this HashSet<int> A, HashSet<int> B, int scale)
        {
            HashSet<int> output = new HashSet<int>();
            foreach (var a in A)
            {
                foreach (var b in B)
                {
                    int result = a * scale + b;
                    if (!output.Contains(result))
                        output.Add(result);
                }
            }

            return output;
        }

        public static bool TryAdd<T>(this HashSet<T> hashSet, T obj)
        {
            if (!hashSet.Contains(obj))
            {
                hashSet.Add(obj);
                return true;
            }

            return false;
        }
    }
}
