using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TrickyLib.IO;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;

namespace TrickyLib.Sort
{
    public class Sorter
    {
        public static bool SortFileWithBucketSorting(string InputFile, string OutputFile)
        {

            int MaxFileNum = 240;
            int ShowResultLineNum = 10000000;
            Dictionary<string, int> DictStrIndex = new Dictionary<string, int>();
            try
            {
                int j = 0;
                if (File.Exists(OutputFile))
                    File.Delete(OutputFile);

                #region Step 1.1 Read data and get distribution of prefix

                System.Console.WriteLine("Get statistic info @ {0}", DateTime.Now);

                LogFileReader fo = new LogFileReader();
                fo.OpenDataReader(InputFile);
                string LineText = string.Empty;
                int LenPrefix = 2;
                string StrPrefix = string.Empty;
                string PreStrPrefix = string.Empty;
                while (fo.GetNextLine(out LineText))
                {
                    if (j++ % ShowResultLineNum == 0)
                    {
                        //System.Console.Write("\r {0} lines @ {1}", j, DateTime.Now);
                    }

                    if (LineText.Length > 2)
                        LenPrefix = 2;
                    else if (LineText.Length < 1)
                        continue;
                    else
                        LenPrefix = LineText.Length;

                    StrPrefix = LineText.Substring(0, LenPrefix);
                    if (StrPrefix == PreStrPrefix)
                    {
                        //if (!DictStrIndex.ContainsKey(StrPrefix))
                        //{
                        //    int a = 0;
                        //}
                        DictStrIndex[StrPrefix]++;
                    }
                    else
                    {
                        if (DictStrIndex.ContainsKey(StrPrefix))
                            DictStrIndex[StrPrefix]++;
                        else
                            DictStrIndex[StrPrefix] = 1;
                        //Update previous string
                        PreStrPrefix = StrPrefix;
                        //if (StrPrefix == "m")
                        //{
                        //    int b = 0;
                        //}
                    }
                }
                System.Console.WriteLine("\r {0} lines readed\t\t\t\t", j);
                fo.CloseDataReader();

                System.Console.WriteLine("Done! @ {0}", DateTime.Now);

                #endregion

                #region Step 1.2 Distribute prefix into different groups

                long TotalLineNum = 0;
                List<string> ListPrefix = new List<string>();
                foreach (var item in DictStrIndex)
                {
                    ListPrefix.Add(item.Key);
                    TotalLineNum += item.Value;
                }
                ListPrefix.Sort(Sort_By_String);
                int IndexNum = 0, CurFileLineNum = 0;
                double MaxValuePerFile = TotalLineNum / Convert.ToDouble(MaxFileNum);
                foreach (var item in ListPrefix)
                {
                    CurFileLineNum += DictStrIndex[item];
                    if (CurFileLineNum > MaxValuePerFile)
                    {
                        IndexNum++;
                        CurFileLineNum = DictStrIndex[item];
                    }
                    DictStrIndex[item] = IndexNum;
                }

                #endregion

                #region (Option 2) Step 2.1 Read all lines and assign them into different files

                System.Console.WriteLine("Assign text lines @ {0}", DateTime.Now);

                //Create and open temp files
                List<string> TmpFiles = new List<string>();
                List<LogFileWriter> listDataWriters = new List<LogFileWriter>();
                for (int i = 0; i <= IndexNum; i++)
                {
                    string TmpFile = string.Format("{0}.{1}.tmp", OutputFile, i);
                    TmpFiles.Add(TmpFile);
                    LogFileWriter lfwriter = new LogFileWriter();
                    lfwriter.OpenDataWriter(TmpFile);
                    listDataWriters.Add(lfwriter);
                }

                //Read data and assign them into different files
                fo.OpenDataReader(InputFile);
                j = 0;
                int FileIndexNum;
                while (fo.GetNextLine(out LineText))
                {
                    if (j++ % ShowResultLineNum == 0)
                    {
                        //System.Console.Write("\r {0} lines @ {1}", j, DateTime.Now);
                    }

                    if (LineText.Length > 2)
                        LenPrefix = 2;
                    else if (LineText.Length < 1)
                        continue;
                    else
                        LenPrefix = LineText.Length;

                    //StrPrefix = LineText.Substring(0, LenPrefix);
                    FileIndexNum = DictStrIndex[LineText.Substring(0, LenPrefix)];
                    listDataWriters[FileIndexNum].SaveLine(LineText);
                }
                System.Console.WriteLine("\r {0} lines readed\t\t\t\t", j);
                fo.CloseDataReader();

                //Close temp files
                for (int i = 0; i <= IndexNum; i++)
                {
                    listDataWriters[i].CloseDataWriter();
                }

                System.Console.WriteLine("Done! @ {0}", DateTime.Now);

                #endregion

                #region (Option 2) Step 2.2 Sort and merge each file (faster than first sort then merge)

                System.Console.WriteLine("Sort and merge files @ {0}", DateTime.Now);
                List<string> listOrigQueryLog = new List<string>();
                for (int i = 0; i < TmpFiles.Count; i++)
                {
                    System.Console.Write("\r Sort and merge file {0} @ {1}", i, DateTime.Now);
                    listOrigQueryLog.Clear();
                    using (StreamReader sr = new StreamReader(TmpFiles[i]))
                    {
                        while (!sr.EndOfStream)
                        {
                            listOrigQueryLog.Add(sr.ReadLine());
                        }
                    }
                    listOrigQueryLog.Sort(Sort_By_String);
                    //using (StreamWriter sw = new StreamWriter(TmpFiles[i]))
                    using (StreamWriter sw = new StreamWriter(OutputFile, true))
                    {
                        foreach (var item in listOrigQueryLog)
                        {
                            sw.WriteLine(item);
                        }
                    }
                    File.Delete(TmpFiles[i]);
                }
                System.Console.WriteLine();
                System.Console.WriteLine("Done! @ {0}", DateTime.Now);

                #endregion

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        private static int Sort_By_String(string Str_A, string Str_B)
        {
            return string.CompareOrdinal(Str_A, Str_B);
        }
    }

    public enum Directions
    { 
        Ascend = -1,
        Descend = 1
    }
}
