using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ExpassistLib.Struct
{
    [Serializable, DataContract]
    public class Pair<Type_A, Type_B>
    {
        [DataMember]
        public Type_A First { get; set; }

        [DataMember]
        public Type_B Second { get; set; }

        public Pair() { }

        public Pair(Type_A first, Type_B second)
        {
            this.First = first;
            this.Second = second;
        }

        public override string ToString()
        {
            return "(" + this.First.ToString() + ", " + this.Second.ToString() + ")";
        }
    }

    [Serializable, DataContract]
    public class Triple<Type_A, Type_B, Type_C>
    {
        [DataMember]
        public Type_A First { get; set; }

        [DataMember]
        public Type_B Second { get; set; }

        [DataMember]
        public Type_C Third { get; set; }

        public Triple() { }

        public Triple(Type_A first, Type_B second, Type_C third)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
        }

        public override string ToString()
        {
            return "(" + this.First.ToString() + ", " + this.Second.ToString() + ", " + this.Third.ToString() + ")";
        }

        public string ToRowString()
        {
            return this.First.ToString() + "\t" + this.Second.ToString() + "\t" + this.Third.ToString();
        }
    }

    [Serializable, DataContract]
    public class Quad<Type_A, Type_B, Type_C, Type_D>
    {
        [DataMember]
        public Type_A First { get; set; }

        [DataMember]
        public Type_B Second { get; set; }

        [DataMember]
        public Type_C Third { get; set; }

        [DataMember]
        public Type_D Forth { get; set; }

        public Quad() { }

        public Quad(Type_A first, Type_B second, Type_C third, Type_D forth)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
            this.Forth = forth;
        }

        public override string ToString()
        {
            return "(" + this.First.ToString() + ", " + this.Second.ToString() + ", " + this.Third.ToString() + ", " + this.Forth.ToString() + ")";
        }

        public string ToRowString()
        {
            return this.First.ToString() + "\t" + this.Second.ToString() + "\t" + this.Third.ToString() + "\t" + this.Forth.ToString();
        }
    }

    [Serializable, DataContract]
    public class Five<Type_A, Type_B, Type_C, Type_D, Type_E>
    {
        [DataMember]
        public Type_A First { get; set; }

        [DataMember]
        public Type_B Second { get; set; }

        [DataMember]
        public Type_C Third { get; set; }

        [DataMember]
        public Type_D Forth { get; set; }

        [DataMember]
        public Type_E Fifth { get; set; }

        public Five() { }

        public Five(Type_A first, Type_B second, Type_C third, Type_D forth, Type_E fifth)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
            this.Forth = forth;
            this.Fifth = fifth;
        }

        public override string ToString()
        {
            return "(" + this.First.ToString() + ", " + this.Second.ToString() + ", " + this.Third.ToString() + ", " + this.Forth.ToString() + ", " + this.Fifth.ToString() + ")";
        }
    }

    [Serializable, DataContract]
    public class Hex<Type_A, Type_B, Type_C, Type_D, Type_E, Type_F>
    {
        [DataMember]
        public Type_A First { get; set; }

        [DataMember]
        public Type_B Second { get; set; }

        [DataMember]
        public Type_C Third { get; set; }

        [DataMember]
        public Type_D Forth { get; set; }

        [DataMember]
        public Type_E Fifth { get; set; }

        [DataMember]
        public Type_F Sixth{get;set;}

        public Hex() { }

        public Hex(Type_A first, Type_B second, Type_C third, Type_D forth, Type_E fifth, Type_F sixth)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
            this.Forth = forth;
            this.Fifth = fifth;
            this.Sixth = sixth;
        }

        public override string ToString()
        {
            return "(" + this.First.ToString() + ", " + this.Second.ToString() + ", " + this.Third.ToString() + ", " + this.Forth.ToString() + ", " + this.Fifth.ToString() + ", " + this.Sixth.ToString() + ")";
        }
    }

}
