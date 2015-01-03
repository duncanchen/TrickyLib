using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TrickyLib
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
                    string[] lineArray = sr.ReadLine().Split('\t');
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

        public static List<string> ReadToList(string filePath)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);
                List<string> outputList = new List<string>();

                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split('\t');
                    foreach (string item in lineArray)
                    {
                        outputList.Add(item.Trim());
                    }
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

        public static List<string> ReadColumn(string filePath, int columnIndex, ItemOperator op)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);
                List<string> output = new List<string>();

                if (op == ItemOperator.IgnoreHeader)
                    sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split('\t');

                    string item = lineArray[columnIndex];

                    if (op == ItemOperator.None)
                        output.Add(item);
                    else if (op == ItemOperator.RemoveEmptyItem && item.Trim() != string.Empty)
                        output.Add(item);
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

        public static IEnumerable<string> ReadRow(string filePath, int rowIndex, ItemOperator op)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);

                if (op == ItemOperator.IgnoreHeader)
                    sr.ReadLine();

                int currentIndex = 0;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (currentIndex++ == rowIndex)
                    {
                        string[] lineArray = line.Split('\t');
                        
                        sr.Close();
                        return lineArray.AsEnumerable();
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

        public static int GetRowCount(string filePath, ItemOperator op)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);

                if (op == ItemOperator.IgnoreHeader)
                    sr.ReadLine();

                int output = 0;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if(op != ItemOperator.RemoveEmptyItem || line.Trim() != string.Empty)
                        output++;
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

        public static IEnumerable<T> ReadToList<T>(string filePath, ItemOperator op)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);

                if (op == ItemOperator.IgnoreHeader)
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
                return output.AsEnumerable();
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
    }

    public enum ItemOperator
    { 
        None,
        RemoveEmptyItem,
        IgnoreHeader
    }
}
