using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TrickyLib.Parser
{
    public static class SerializeParser
    {
        public static void Serialize(object obj, string outputFile)
        {
            FileStream fs = new FileStream(outputFile, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(fs, obj);
            fs.Flush();
            fs.Close();
        }

        public static object Deserialize(string inputFile)
        {
            FileStream fs = new FileStream(inputFile, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            var obj = bf.Deserialize(fs);
            fs.Close();

            return obj;
        }
    }
}
