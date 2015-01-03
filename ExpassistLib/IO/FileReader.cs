using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using ExpassistLib.Reflection;

namespace ExpassistLib.IO
{
    public class FileReader
    {
        public static Dictionary<string, List<string>> ReadToDic(string filePath, int keyIndex)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);
                Dictionary<string, List<string>> outputDic = new Dictionary<string, List<string>>();

                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split('\t');
                    string key = lineArray[keyIndex];

                    List<string> valueList = new List<string>();
                    for (int i = keyIndex + 1; i < lineArray.Length; i++)
                        valueList.Add(lineArray[i]);

                    if (!outputDic.ContainsKey(key))
                        outputDic.Add(key, valueList);
                    else
                        outputDic[key].AddRange(valueList);
                }

                sr.Close();
                return outputDic;
            }
            catch (Exception e)
            {
                if (sr != null)
                    sr.Close();

                throw e;
            }
        }

        public static Dictionary<string, List<string[]>> ReadToUnitDic(string filePath, int keyIndex, int unitLength)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);
                Dictionary<string, List<string[]>> outputDic = new Dictionary<string, List<string[]>>();

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    if (line != string.Empty)
                    {
                        string[] lineArray = line.Split('\t');
                        string key = lineArray[keyIndex];

                        List<string[]> valueList = new List<string[]>();
                        for (int i = keyIndex + 1; i < lineArray.Length; i = i + unitLength)
                        {
                            List<string> unit = new List<string>();
                            for (int j = 0; j < unitLength; j++)
                                unit.Add(lineArray[i + j]);

                            valueList.Add(unit.ToArray());
                        }
                        if (!outputDic.ContainsKey(key))
                            outputDic.Add(key, valueList);
                        else
                            outputDic[key].AddRange(valueList);
                    }
                }
                sr.Close();
                return outputDic;
            }
            catch (Exception e)
            {
                if (sr != null)
                    sr.Close();

                throw e;
            }
        }

        public static Dictionary<string, List<string>> ReduceToDic(string filePath, int keyIndex)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);
                Dictionary<string, List<string>> outputDic = new Dictionary<string, List<string>>();

                string currentString = null;
                List<string> currentValues = new List<string>();

                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split('\t');
                    if (currentString == null || currentString != lineArray[keyIndex])
                    {
                        if (currentString != null)
                        {
                            if (!outputDic.ContainsKey(currentString))
                                outputDic.Add(currentString, currentValues);
                            else
                                outputDic[currentString].AddRange(currentValues);
                        }

                        currentString = lineArray[keyIndex];
                        currentValues.Clear();
                    }

                    for (int i = keyIndex + 1; i < lineArray.Length; i++)
                        currentValues.Add(lineArray[i]);
                }

                if (currentString != null)
                {
                    if (!outputDic.ContainsKey(currentString))
                        outputDic.Add(currentString, currentValues);
                    else
                        outputDic[currentString].AddRange(currentValues);
                }

                sr.Close();
                return outputDic;
            }
            catch (Exception e)
            {
                if (sr != null)
                    sr.Close();

                throw e;
            }
        }

        public static Dictionary<string, List<string[]>> ReduceToUnitDic(string filePath, int keyIndex, int unitLength)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);
                Dictionary<string, List<string[]>> outputDic = new Dictionary<string, List<string[]>>();

                string currentString = null;
                List<string[]> currentValues = new List<string[]>();

                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split('\t');
                    if (currentString == null || currentString != lineArray[keyIndex])
                    {
                        if (currentString != null)
                        {
                            if (!outputDic.ContainsKey(currentString))
                                outputDic.Add(currentString, currentValues);
                            else
                                outputDic[currentString].AddRange(currentValues);
                        }

                        currentString = lineArray[keyIndex];
                        currentValues.Clear();
                    }

                    List<string> value = new List<string>();
                    for (int i = keyIndex + 1; i < lineArray.Length; i++)
                        value.Add(lineArray[i]);

                    currentValues.Add(value.ToArray());
                }

                if (currentString != null)
                {
                    if (!outputDic.ContainsKey(currentString))
                        outputDic.Add(currentString, currentValues);
                    else
                        outputDic[currentString].AddRange(currentValues);
                }
                sr.Close();
                return outputDic;
            }
            catch (Exception e)
            {
                if (sr != null)
                    sr.Close();

                throw e;
            }
        }

        public static Dictionary<S, T> ReadToDic<S, T>(string filePath)
        {
            Dictionary<S, T> outputDic = new Dictionary<S, T>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split('\t');
                    if (lineArray.Length >= 2)
                    {
                        S key = (S)Convert.ChangeType(lineArray[0], typeof(S));
                        if (!outputDic.ContainsKey(key))
                        {
                            T value = (T)Convert.ChangeType(lineArray[1], typeof(T));
                            outputDic.Add(key, value);
                        }
                    }
                }
            }

            return outputDic;
        }

        public static List<string[]> ReadToArrayList(string filePath)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);
                List<string[]> outputList = new List<string[]>();

                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split('\t');
                    outputList.Add(lineArray);
                }

                sr.Close();
                return outputList;
            }
            catch (Exception e)
            {
                if (sr != null)
                    sr.Close();

                throw e;
            }
        }

        public static string[] ReadColumn(string filePath, int columnIndex, ItemOperator op)
        {
            return ReadColumn(filePath, columnIndex, op, '\t');
        }

        public static string[] ReadColumn(string filePath, int columnIndex, ItemOperator op, char separator)
        {
            //ConsoleWriter.WriteCurrentMethodStarted();
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);
                List<string> output = new List<string>();

                if (op.HasFlag(ItemOperator.IgnoreHeader))
                    sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split(separator);

                    if (lineArray.Length > columnIndex)
                    {
                        string item = lineArray[columnIndex];
                        if (!op.HasFlag(ItemOperator.RemoveEmptyItem) || item.Trim() != string.Empty)
                            output.Add(item);
                    }
                    else if (!op.HasFlag(ItemOperator.RemoveEmptyItem))
                        output.Add(string.Empty);
                }

                sr.Close();
                //ConsoleWriter.WriteCurrentMethodFinished();
                return output.ToArray();
            }
            catch (Exception e)
            {
                if (sr != null)
                    sr.Close();

                //ConsoleWriter.WriteCurrentMethodFinished();
                throw e;
            }
        }

        public static List<string> ReadRows(string filePath, string startLine, string endLine, bool removeCover = false)
        {
            List<string> outputs = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                bool start = false;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line == startLine)
                        start = true;

                    if(start)
                        outputs.Add(line);

                    if (line == endLine)
                        break;
                }

                if (removeCover)
                {
                    if (outputs.Last() == startLine)
                        outputs.RemoveAt(outputs.Count - 1);

                    outputs.RemoveAt(0);
                }
            }

            return outputs;
        }

        public static List<string> ReadRows(string filePath, string startLine, bool removeCover = false)
        {
            List<string> outputs = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                bool start = false;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line == startLine)
                        start = true;

                    if (start)
                        outputs.Add(line);
                }

                if (removeCover)
                    outputs.RemoveAt(0);
            }

            return outputs;
        }

        public static List<string> ReadRows(string filePath, int start, int rows)
        {
            try
            {
                List<string> outputs = new List<string>();
                long endIndex = Convert.ToInt64(start) + Convert.ToInt64(rows);

                using (StreamReader sr = new StreamReader(filePath))
                {
                    int currentIndex = 0;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        if (currentIndex >= start && currentIndex < endIndex)
                            outputs.Add(line);
                        else if (currentIndex >= endIndex)
                            break;

                        currentIndex++;
                    }
                }

                return outputs;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public static List<string> ReadRows(string filePath, int start)
        {
            return ReadRows(filePath, start, int.MaxValue);
        }

        public static List<string> ReadRows(string filePath)
        {
            return ReadRows(filePath, 0, int.MaxValue);
        }

        public static string ReadRow(string filePath, int rowIndex)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                int currentIndex = 0;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (currentIndex++ == rowIndex)
                        return line;
                }
            }

            throw new ArgumentOutOfRangeException("rowIndex");
        }

        public static string ReadLastRow(string filePath, int lastRowIndex = 0, ItemOperator itemOperator = ItemOperator.RemoveEmptyItem)
        {
            Queue<string> lastRowsQueue = new Queue<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    if (line == string.Empty && itemOperator.HasFlag(ItemOperator.RemoveEmptyItem))
                        continue;

                    if (lastRowsQueue.Count >= lastRowIndex + 1)
                        lastRowsQueue.Dequeue();

                    lastRowsQueue.Enqueue(line);
                }
            }

            if (lastRowsQueue.Count <= lastRowIndex)
                throw new Exception("No enough rows");
            else
                return lastRowsQueue.Dequeue();
        }

        public static T GetColumn<T>(string filePath, int columnIndex, ItemOperator op, char separator) where T : ICollection<string>, new()
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);
                T output = new T();

                if (op.HasFlag(ItemOperator.IgnoreHeader))
                    sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split(separator);

                    if (lineArray.Length > columnIndex)
                    {
                        string item = lineArray[columnIndex];
                        if (!op.HasFlag(ItemOperator.RemoveEmptyItem) || item.Trim() != string.Empty)
                            output.Add(item);
                    }
                    else if (!op.HasFlag(ItemOperator.RemoveEmptyItem))
                        output.Add(string.Empty);
                }

                sr.Close();
                return output;
            }
            catch (Exception e)
            {
                if (sr != null)
                    sr.Close();

                throw e;
            }
        }

        public static int GetColumnCount(string filePath)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);
                int output = -1;

                string[] lineArray = sr.ReadLine().Split('\t');
                output = lineArray.Length;

                sr.Close();
                return output;
            }
            catch (Exception e)
            {
                if (sr != null)
                    sr.Close();

                throw e;
            }
        }

        public static long GetRowCount(string filePath, ItemOperator op)
        {
            //ConsoleWriter.WriteCurrentMethodStarted();
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);

                if (op.HasFlag(ItemOperator.IgnoreHeader))
                    sr.ReadLine();

                long output = 0;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (!op.HasFlag(ItemOperator.RemoveEmptyItem) || line.Trim() != string.Empty)
                        output++;
                }

                sr.Close();
                Console.WriteLine("TotalLine: [{0}] of file {1}", output, filePath);
                //ConsoleWriter.WriteCurrentMethodFinished();
                return output;
            }
            catch (Exception e)
            {
                if (sr != null)
                    sr.Close();

                //ConsoleWriter.WriteCurrentMethodFinished();
                throw e;
            }
        }

        public static string ReadSpecifiedRow(string filePath, long num, ItemOperator op)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);

                if (op.HasFlag(ItemOperator.IgnoreHeader))
                    sr.ReadLine();

                long curIndex = 0;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (!op.HasFlag(ItemOperator.RemoveEmptyItem) || line.Trim() != string.Empty)
                    {
                        curIndex++;
                        if (curIndex == num)
                            return line;
                    }
                }

                sr.Close();
                return null;
            }
            catch (Exception e)
            {
                if (sr != null)
                    sr.Close();

                throw e;
            }
        }

        public static IEnumerable<T> ReadToList<T>(string filePath, ItemOperator op)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);

                if (op.HasFlag(ItemOperator.IgnoreHeader))
                    sr.ReadLine();

                var propertyNames = ReflectionHandler.GetPropertyNames(typeof(T));
                List<T> output = new List<T>();

                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split('\t');
                    if (propertyNames.Count() != lineArray.Length)
                        throw new Exception("The file does not have the same columns of type");

                    Dictionary<string, object> propertyDic = new Dictionary<string, object>();
                    for (int i = 0; i < lineArray.Length; i++)
                        propertyDic.Add(propertyNames.ElementAt(i), lineArray[i]);

                    output.Add(ReflectionHandler.CreateInstanceWithProperty<T>(propertyDic));
                }

                sr.Close();
                return output;
            }
            catch (Exception e)
            {
                if (sr != null)
                    sr.Close();

                throw e;
            }
        }

        public static string ReadToEnd(string filePath)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);
                string output = sr.ReadToEnd();

                sr.Close();
                return output;
            }
            catch (Exception e)
            {
                if (sr != null)
                    sr.Close();

                throw e;
            }
        }

        public static string ReadSubString(string filePath, long start, int length)
        {
            FileInfo fi = new FileInfo(filePath);
            using (FileStream fs = fi.OpenRead())
            {
                fs.Seek(start, SeekOrigin.Begin);
                byte[] str = new byte[length];

                fs.Read(str, 0, length);
                string subString = Encoding.Default.GetString(str);

                return subString;
            }
        }

        public static T Deserialize<T>(string inputFile)
        {
            //ConsoleWriter.WriteCurrentMethodStarted();
            FileStream fs = new FileStream(inputFile, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            
            var output = (T)(Convert.ChangeType(bf.Deserialize(fs), typeof(T)));
            fs.Close();
            //ConsoleWriter.WriteCurrentMethodFinished();

            return output;
        }

        public static Stream ReadAssemblyFile(string fileName)
        {
            string exeNamespace = Assembly.GetCallingAssembly().GetName().Name;
            if (!fileName.StartsWith(exeNamespace + "."))
                fileName = exeNamespace + "." + fileName;

            Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(fileName);
            return stream;
        }

        #region ReadToHashSet
        public static HashSet<string> ReadToHashSet(string filePath, int columnIndex, ItemOperator op, char separator)
        {
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(filePath);
                HashSet<string> output = new HashSet<string>();

                if (op.HasFlag(ItemOperator.IgnoreHeader))
                    sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split(separator);

                    if (lineArray.Length > columnIndex)
                    {
                        string item = lineArray[columnIndex].Trim();
                        if (item != string.Empty && !output.Contains(item))
                            output.Add(item);
                    }
                }

                sr.Close();
                return output;
            }
            catch (Exception e)
            {
                if (sr != null)
                    sr.Close();

                throw e;
            }
        }

        public static HashSet<string> ReadToHashSet(string filePath, int columnIndex, ItemOperator op)
        {
            return ReadToHashSet(filePath, columnIndex, op, '\t');
        }

        public static HashSet<string> ReadToHashSet(string filePath, int columnIndex)
        {
            return ReadToHashSet(filePath, columnIndex, ItemOperator.None, '\t');
        }

        public static HashSet<string> ReadToHashSet(string filePath)
        {
            return ReadToHashSet(filePath, 0, ItemOperator.None, '\t');
        }
        #endregion 
    }

    public enum ItemOperator
    {
        None = 1,
        IgnoreHeader = 2,
        RemoveEmptyItem = 4
    }
}
