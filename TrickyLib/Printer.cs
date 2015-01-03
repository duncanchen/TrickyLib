using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TrickyLib
{
    public class Printer
    {
        #region return line
        public static string GetArrayLine(IEnumerable<object> input)
        {
            string outputLine = null;

            if (input != null && input.Count() > 0)
            {
                foreach (var item in input)
                {
                    if (outputLine == null)
                    {
                        if (item != null)
                            outputLine = item.ToString();
                        else
                            outputLine = string.Empty;
                    }
                    else
                    {
                        if (item != null)
                            outputLine += "\t" + item.ToString();
                        else
                            outputLine += "\t";
                    }
                }
            }

            return outputLine;
        }
        #endregion

        #region print to file
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
                    sw.WriteLine(GetArrayLine(list));   
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
        public static void PrintCollectionProperties(string file, IEnumerable<object> input)
        {
            StreamWriter sw = null;
            try
            {
                if (input != null && input.Count() <= 0)
                    throw new Exception("There is no data in input");

                sw = new StreamWriter(file);
                sw.WriteLine(GetArrayLine(ReflectionHandler.GetPropertyNames(input.ElementAt(0).GetType())));

                foreach (var item in input)
                {
                    sw.WriteLine(GetArrayLine(ReflectionHandler.GetProperties(item)));
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
        #endregion


        #region console print
        private static bool started = false;
        private static bool bugprinted = false;
        public static void ConsolePrintPercentage(int finisedCount, int totalCount)
        {
            if (!started && finisedCount <= totalCount)
            {
                Console.WriteLine("Process started at: " + DateTime.Now.ToString());
                Console.WriteLine();
                
                started = true;
                bugprinted = false;
            }

            decimal finishedPercentage = Convert.ToDecimal(finisedCount) / Convert.ToDecimal(totalCount);

            if (Console.CursorTop > 0)
                Console.SetCursorPosition(0, Console.CursorTop - 1);

            Console.WriteLine((finishedPercentage * 100).ToString("f1") + "%");

            if (finisedCount == totalCount)
            {
                Console.WriteLine("Process finished at: " + DateTime.Now.ToString());
                started = false;
            }
            else if (finisedCount > totalCount && !bugprinted)
            {
                Console.WriteLine("Bug: the finished count is larger than the total count");
                Console.WriteLine();

                started = false;
                bugprinted = true;
            }
        }
        #endregion
    }
}
