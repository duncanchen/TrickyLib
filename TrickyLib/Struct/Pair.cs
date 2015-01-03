using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Parser;
using System.Runtime.Serialization;

namespace TrickyLib.Struct
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

        public static Pair<Type_A, Type_B> Parse(string input)
        {
            string[] items = input.Split('\t');
            if (items.Length != 2)
                throw new Exception("The input does not match 2 columns: " + input);

            Pair<Type_A, Type_B> output = new Pair<Type_A, Type_B>();
            output.First = TypeConverter.ConvertToType<Type_A>(items[0]);
            output.Second = TypeConverter.ConvertToType<Type_B>(items[1]);

            return output;
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

        public static Triple<Type_A, Type_B, Type_C> Parse(string input)
        {
            string[] items = input.Split('\t');
            if (items.Length != 3)
                throw new Exception("The input does not match 3 columns: " + input);

            Triple<Type_A, Type_B, Type_C> output = new Triple<Type_A, Type_B, Type_C>();
            output.First = TypeConverter.ConvertToType<Type_A>(items[0]);
            output.Second = TypeConverter.ConvertToType<Type_B>(items[1]);
            output.Third = TypeConverter.ConvertToType<Type_C>(items[2]);

            return output;
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

        public static Quad<Type_A, Type_B, Type_C, Type_D> Parse(string input)
        {
            string[] items = input.Split('\t');
            if (items.Length != 4)
                throw new Exception("The input does not match 4 columns: " + input);

            Quad<Type_A, Type_B, Type_C, Type_D> output = new Quad<Type_A, Type_B, Type_C, Type_D>();
            output.First = TypeConverter.ConvertToType<Type_A>(items[0]);
            output.Second = TypeConverter.ConvertToType<Type_B>(items[1]);
            output.Third = TypeConverter.ConvertToType<Type_C>(items[2]);
            output.Forth = TypeConverter.ConvertToType<Type_D>(items[3]);

            return output;
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

        public static Five<Type_A, Type_B, Type_C, Type_D, Type_E> Parse(string input)
        {
            string[] items = input.Split('\t');
            if (items.Length != 5)
                throw new Exception("The input does not match 5 columns: " + input);

            Five<Type_A, Type_B, Type_C, Type_D, Type_E> output = new Five<Type_A, Type_B, Type_C, Type_D, Type_E>();
            output.First = TypeConverter.ConvertToType<Type_A>(items[0]);
            output.Second = TypeConverter.ConvertToType<Type_B>(items[1]);
            output.Third = TypeConverter.ConvertToType<Type_C>(items[2]);
            output.Forth = TypeConverter.ConvertToType<Type_D>(items[3]);
            output.Fifth = TypeConverter.ConvertToType<Type_E>(items[4]);
            return output;
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

        public static Hex<Type_A, Type_B, Type_C, Type_D, Type_E, Type_F> Parse(string input)
        {
            string[] items = input.Split('\t');
            if (items.Length != 5)
                throw new Exception("The input does not match 5 columns: " + input);

            Hex<Type_A, Type_B, Type_C, Type_D, Type_E, Type_F> output = new Hex<Type_A, Type_B, Type_C, Type_D, Type_E, Type_F>();
            output.First = TypeConverter.ConvertToType<Type_A>(items[0]);
            output.Second = TypeConverter.ConvertToType<Type_B>(items[1]);
            output.Third = TypeConverter.ConvertToType<Type_C>(items[2]);
            output.Forth = TypeConverter.ConvertToType<Type_D>(items[3]);
            output.Fifth = TypeConverter.ConvertToType<Type_E>(items[4]);
            output.Sixth = TypeConverter.ConvertToType<Type_F>(items[5]);
            return output;
        }

        public override string ToString()
        {
            return "(" + this.First.ToString() + ", " + this.Second.ToString() + ", " + this.Third.ToString() + ", " + this.Forth.ToString() + ", " + this.Fifth.ToString() + ", " + this.Sixth.ToString() + ")";
        }
    }

}
