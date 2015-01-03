using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib
{
    public class Pair<Type_A, Type_B>
    {
        public Type_A First { get; set; }
        public Type_B Second { get; set; }
    }

    public class Triple<Type_A, Type_B, Type_C>
    {
        public Type_A First { get; set; }
        public Type_B Second { get; set; }
        public Type_C Third { get; set; }
    }

    public class Quad<Type_A, Type_B, Type_C, Type_D>
    {
        public Type_A First { get; set; }
        public Type_B Second { get; set; }
        public Type_C Third { get; set; }
        public Type_D Forth { get; set; }
    }
}
