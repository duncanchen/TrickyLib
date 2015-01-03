using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpassistLib.Extension
{
    public static class ComparisonExtension
    {
        public static IComparer<T> AsComparer<T>(this Comparison<T> @this)
        {
            if (@this == null)
                throw new System.ArgumentNullException("Comparison<T> @this");
            return new ComparisonComparer<T>(@this);
        }

        public static IComparer<T> AsComparer<T>(this Func<T, T, int> @this)
        {
            if (@this == null)
                throw new System.ArgumentNullException("Func<T, T, int> @this");
            return new ComparisonComparer<T>((x, y) => @this(x, y));
        }

        private class ComparisonComparer<T> : IComparer<T>
        {
            public ComparisonComparer(Comparison<T> comparison)
            {
                if (comparison == null)
                    throw new System.ArgumentNullException("comparison");
                this.Comparison = comparison;
            }

            public int Compare(T x, T y)
            {
                return this.Comparison(x, y);
            }

            public Comparison<T> Comparison { get; private set; }
        }
    }
}
