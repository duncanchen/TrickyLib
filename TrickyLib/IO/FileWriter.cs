using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TrickyLib.Extension;
using TrickyLib.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
namespace TrickyLib.IO
{
    public class FileWriter
    {
        public static void PrintCollection(string file, IEnumerable<object> input)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(file);

                foreach (var obj in input)
                {
                    if (obj != null)
                        sw.WriteLine(obj.ToString());
                    else
                        sw.WriteLine();
                }

                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }

                throw e;
            }
        }
        public static void PrintDoubleCollection(string file, IEnumerable<IEnumerable<object>> input)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(file);

                foreach (var list in input)
                {
                    sw.WriteLine(list.ToRowString());
                }

                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }

                throw e;
            }
        }
        public static void PrintCollectionProperties(string file, IEnumerable<object> input, bool printHeader = true)
        {
            StreamWriter sw = null;
            try
            {
                if (input != null && input.Count() <= 0)
                    throw new Exception("There is no data in input");

                sw = new StreamWriter(file);

                if(printHeader)
                    sw.WriteLine(ReflectionHandler.GetPropertyNames(input.ElementAt(0).GetType()).ToRowString());

                foreach (var item in input)
                {
                    sw.WriteLine(ReflectionHandler.GetProperties(item).ToRowString());
                }

                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }

                throw e;
            }
        }
        public static void MergeFiles(string mergePath, params string[] filePaths)
        {
            int b;
            int lastB = -1;
            int n = filePaths.Length;
            FileStream[] fileIn = new FileStream[n];
            using (FileStream fileOut = new FileStream(mergePath, FileMode.Create))
            {
                for (int i = 0; i < n; i++)
                {
                    try
                    {
                        fileIn[i] = new FileStream(filePaths[i], FileMode.Open);
                        while ((b = fileIn[i].ReadByte()) != -1)
                        {
                            fileOut.WriteByte((byte)b);
                            lastB = b;
                        }
                        if (lastB != '\n' && lastB != -1)
                            fileOut.WriteByte((byte)'\n');
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        fileIn[i].Close();
                    }
                }
            }
        }
        public static void SplitFile(string file, uint count)
        {
            ConsoleWriter.WriteCurrentMethodStarted();
            if (count <= 1)
            {
                Console.WriteLine("The split parts is less than 2, stop split file: " + file);
                return;
            }

            var countDigit = count.ToString().Length;
            var totalLineCount = FileReader.GetRowCount(file, ItemOperator.None);
            var splitCount = Math.Min(count, Convert.ToInt32(totalLineCount));
            var lineCountPerFile = totalLineCount / splitCount;

            int finished = 0;
            long currentLineNum = 0;
            StreamWriter sw = null;

            using (StreamReader sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    if (currentLineNum % lineCountPerFile == 0 && finished < splitCount)
                    {
                        if (sw != null)
                        {
                            sw.Flush();
                            sw.Close();
                        }

                        ++finished;
                        ConsoleWriter.WritePercentage(finished - 1, splitCount);
                        string partFile = FilePath.ChangeExtension(file, "part" + finished.AddZeroBefore(countDigit) + Path.GetExtension(file));
                        sw = new StreamWriter(partFile);
                    }

                    sw.WriteLine(sr.ReadLine());
                    ++currentLineNum;
                }
            }

            if (sw != null)
            {
                sw.Flush();
                sw.Close();
            }
            ConsoleWriter.WritePercentage(finished, splitCount);
        }
        public static void SelectTopLines(int lineNum, string inputFile)
        {
            if (!File.Exists(inputFile))
                throw new FileNotFoundException("File not found", inputFile);

            if (lineNum <= 0)
                throw new ArgumentOutOfRangeException("Error: lineNum <= 0");

            string outputFile = FilePath.GetFilePathWithoutExtenstion(inputFile) + "_top" + lineNum + Path.GetExtension(inputFile);
            using (StreamReader sr = new StreamReader(inputFile))
            {
                using (StreamWriter sw = new StreamWriter(outputFile))
                {
                    int i = 0;
                    while (!sr.EndOfStream && i < lineNum)
                    {
                        string line = sr.ReadLine();
                        sw.WriteLine(line);

                        i++;
                    }
                }
            }
        }
        public static void Serialize(object obj, string outputFile)
        {
            ConsoleWriter.WriteCurrentMethodStarted();
            FileStream fs = new FileStream(outputFile, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(fs, obj);
            fs.Close();
            ConsoleWriter.WriteCurrentMethodFinished();
        }

        /// <summary>
        /// We only support distinct the first column. This function will first sort the file, and then distinct
        /// </summary>
        /// <param name="file"></param>
        public static void DistinctFile(string sourceFile, string outputFile)
        {
            ConsoleWriter.WriteCurrentMethodStarted();

            string tempFile = FilePath.ChangeExtension(sourceFile, "sorted.temp");
            Sort.Sorter.SortFileWithBucketSorting(sourceFile, tempFile);

            long total = FileReader.GetRowCount(sourceFile, ItemOperator.None);
            long finished = 0;

            using (StreamReader sr = new StreamReader(tempFile))
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                string last = null;
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var lineArray = line.Split('\t');

                    if (last != lineArray[0])
                    {
                        last = lineArray[0];
                        sw.WriteLine(line);
                    }

                    ConsoleWriter.WritePercentage(++finished, total);
                }
            }

            File.Delete(tempFile);
            ConsoleWriter.WriteCurrentMethodFinished();
        }
    }
}
