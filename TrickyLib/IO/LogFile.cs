using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TrickyLib.IO
{
    public class LogFileWriter
    {
        int mMaxLineNum = 10000;
        string mQueryLogFile = string.Empty;
        //List<string> mBufferText = new List<string>();
        StreamWriter mswLogDataWriter = null;
        Queue<string> mHistoryLines = new Queue<string>();

        public LogFileWriter()
        {
        }

        public void OpenDataWriter(string QueryLogFile)
        {
            this.mQueryLogFile = QueryLogFile;
            this.mswLogDataWriter = new StreamWriter(this.mQueryLogFile);
        }

        public void CloseDataWriter()
        {
            if (this.mHistoryLines.Count > 0)
            {
                SaveData();
            }
            this.mswLogDataWriter.Close();
        }

        public bool SaveLine(string LineText)
        {
            try
            {
                mHistoryLines.Enqueue(LineText);
                if (this.mHistoryLines.Count > mMaxLineNum)
                {
                    SaveData();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return false;
            }
            return true;
        }
        private void SaveData()
        {
            foreach (var item in this.mHistoryLines)
            {
                mswLogDataWriter.WriteLine(item);
            }
            this.mHistoryLines.Clear();
        }

        public bool WriteWholeListIntoFile(string OutputFile, List<string> ReadedLines)
        {
            try
            {
                #region Option 1. Merge lines and write out the whole string (Time: 18 seconds. Slower than option 2)

                //StringBuilder sb = new StringBuilder();
                //foreach (var item in ReadedLines)
                //{
                //    sb.Append(item);
                //    sb.Append("\n");
                //}
                //using (StreamWriter sw = new StreamWriter(OutputFile))
                //{
                //    sw.Write(sb.ToString());
                //}

                #endregion

                #region Option 2. write text line by line (Time: 13 seconds. Faster than option 1)

                using (StreamWriter sw = new StreamWriter(OutputFile))
                {
                    foreach (var item in ReadedLines)
                    {
                        sw.WriteLine(item);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return false;
            }
            return true;
        }
    }

    public class LogFileReader
    {
        int mMaxLineNum = 10000;
        string mQueryLogFile = string.Empty;
        string mPrevQueryWord = string.Empty;
        //QueryInfo mPrevQueryNode = null;
        StreamReader srLogDataReader = null;
        Queue<string> mHistoryLines = new Queue<string>();

        public LogFileReader()
        {

        }

        public void OpenDataReader(string QueryLogFile)
        {
            this.mQueryLogFile = QueryLogFile;
            this.srLogDataReader = new StreamReader(this.mQueryLogFile);
        }

        public void CloseDataReader()
        {
            this.srLogDataReader.Close();
        }

        //public bool QueryLogNormalization(string InputFile, string OutputFile)
        //{
        //    try
        //    {
        //        using (StreamReader sr = new StreamReader(InputFile))
        //        {
        //            using (StreamWriter sw = new StreamWriter(OutputFile))
        //            {
        //                while (!sr.EndOfStream)
        //                {
        //                    string TextLine = srLogDataReader.ReadLine();
        //                    string[] Tokens = TextLine.Split(Parameters.SEPARATOR_TAP, StringSplitOptions.RemoveEmptyEntries);
        //                    if (Tokens.Length == 3) //It's a whole query
        //                    {
        //                        Tokens[0] = Utils.NormalizeQuery(Tokens[0]);
        //                        string LineText = string.Join("\t", Tokens);
        //                        sw.WriteLine(LineText);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Console.WriteLine(ex);
        //        return false;
        //    }
        //    return true;
        //}

        //public bool GetNextLogBlockOfWord(out string QueryWord, out List<QueryInfo> QueryLogs)
        //{
        //    QueryWord = string.Empty;
        //    QueryLogs = new List<QueryInfo>();
        //    try
        //    {
        //        //Keep previous node if have
        //        if (mPrevQueryNode != null)
        //        {
        //            QueryWord = mPrevQueryWord;
        //            QueryLogs.Add(mPrevQueryNode);
        //        }
        //        if (srLogDataReader.EndOfStream)
        //        {
        //            if (mPrevQueryNode != null)//Perhaps in previous step we read the last node of the file
        //                return true;
        //            else
        //                return false;
        //        }

        //        while (!srLogDataReader.EndOfStream)
        //        {
        //            string TextLine = srLogDataReader.ReadLine();
        //            string[] Tokens = TextLine.Split(Parameters.SEPARATOR_TAP, StringSplitOptions.RemoveEmptyEntries);
        //            if (Tokens.Length != 3) //It's a whole query
        //                return false;
        //            string[] Words = Tokens[0].Split(Parameters.SEPARATOR_SPACE, StringSplitOptions.RemoveEmptyEntries);

        //            //If it's the first time to read data
        //            if (string.IsNullOrEmpty(mPrevQueryWord))
        //            {
        //                QueryWord = Words[0];

        //                QueryInfo node = new QueryInfo();
        //                node.Query = Tokens[0];
        //                node.ClickedURLs = Tokens[1]; //Original clicked urls
        //                node.ClickNum = System.Convert.ToInt32(Tokens[2]);
        //                QueryLogs.Add(node);

        //                mPrevQueryWord = Words[0];
        //            }
        //            else
        //            {
        //                if (string.CompareOrdinal(Words[0], mPrevQueryWord) != 0)
        //                {
        //                    //We need stop here and save current node
        //                    mPrevQueryNode = new QueryInfo();
        //                    mPrevQueryNode.Query = Tokens[0];
        //                    mPrevQueryNode.ClickedURLs = Tokens[1]; //Original clicked urls
        //                    mPrevQueryNode.ClickNum = System.Convert.ToInt32(Tokens[2]);

        //                    //Update the first word
        //                    mPrevQueryWord = Words[0];

        //                    //Break the thread
        //                    break;
        //                }
        //                else
        //                {
        //                    mPrevQueryNode = null;

        //                    QueryInfo node = new QueryInfo();
        //                    node.Query = Tokens[0];
        //                    node.ClickedURLs = Tokens[1]; //Original clicked urls
        //                    node.ClickNum = System.Convert.ToInt32(Tokens[2]);
        //                    QueryLogs.Add(node);

        //                    mPrevQueryWord = Words[0];
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        public bool GetNextNLines(int MaxLineNum, out List<string> ReadedLines, out int ReadLineNum)
        {
            ReadedLines = new List<string>();
            ReadLineNum = 0;
            try
            {
                if (srLogDataReader.EndOfStream)
                    return false;
                while (!srLogDataReader.EndOfStream)
                {
                    if (ReadLineNum > MaxLineNum)
                        break;

                    string TextLine = srLogDataReader.ReadLine();

                    #region Option 1. Read original data

                    ReadedLines.Add(TextLine);
                    ReadLineNum++;

                    #endregion

                    #region Option 2. Read query and normolize it

                    //string[] Tokens = TextLine.Split(Parameters.SEPARATOR_TAP, StringSplitOptions.RemoveEmptyEntries);
                    //if (Tokens.Length != 3) //It's a whole query
                    //    continue;

                    //string Query = Utils.NormalizeQuery(Tokens[0]).Trim();
                    //if (!string.IsNullOrEmpty(Query))
                    //{
                    //    Tokens[0] = Query;
                    //    string NewLine = string.Join("\t", Tokens).Trim(); ;

                    //    ReadedLines.Add(NewLine);
                    //    ReadLineNum++;
                    //}
                    //else
                    //{
                    //    int a = 0;
                    //}

                    #endregion
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get the next line. Null if no lines left.
        /// </summary>
        /// <param name="LineText"></param>
        /// <returns></returns>
        public bool GetNextLine(out string LineText)
        {
            LineText = string.Empty;

            #region Option 1. Read into a quene and distribute it out one by one (Time: 27 minutes to read 40G data)

            //if (mHistoryLines.Count > 0)
            //{
            //    LineText = mHistoryLines.Dequeue();
            //}
            //else
            //{
            //    if (srLogDataReader.EndOfStream)
            //    {
            //        return false;
            //    }
            //    else
            //    {                    
            //        while (!srLogDataReader.EndOfStream)
            //        {
            //            if (mHistoryLines.Count >= mMaxLineNum)
            //                break;

            //            string TextLine = srLogDataReader.ReadLine();

            //            #region Option 1. Read original data

            //            this.mHistoryLines.Enqueue(TextLine);

            //            #endregion

            //            #region Option 2. Read and normolize the query

            //            //string[] Tokens = TextLine.Split(Parameters.SEPARATOR_TAP, StringSplitOptions.RemoveEmptyEntries);
            //            //if (Tokens.Length != 3) //It's a whole query
            //            //    return false;
            //            //Tokens[0] = Utils.NormalizeQuery(Tokens[0]);
            //            //LineText = string.Join("\t", Tokens);

            //            #endregion
            //        }
            //        LineText = this.mHistoryLines.Dequeue();
            //    }
            //}

            #endregion

            #region Option 2. Read line directly (Time: 25 minutes to read 40G data)

            if (srLogDataReader.EndOfStream)
            {
                return false;
            }
            else
            {
                LineText = srLogDataReader.ReadLine();
            }

            #endregion

            return true;
        }

        public bool ReadWholeFileIntoList(string InputFile, out List<string> ReadedLines)
        {

            ReadedLines = new List<string>();
            try
            {
                #region Option 1. Read all lines and split them into blocks (Time: 30 = 9+21 seconds. Slower than Option 2)

                //System.Console.WriteLine(" Read start @ {0}", DateTime.Now);

                //string WholeText = string.Empty;
                //using (StreamReader sr = new StreamReader(InputFile))
                //{
                //    WholeText = sr.ReadToEnd();
                //}

                //System.Console.WriteLine(" Split start @ {0}", DateTime.Now);

                //char[] Separator = { '\r', '\n' };
                //int StartPos=0, EndPos=0;
                ////Block size has limited impact to the final speed. The test is conducted on 4M, 32M, 64M, and 128M.
                //int BlockSize = 4 * 1024 * 1024;
                //while (StartPos < WholeText.Length)
                //{
                //    EndPos = StartPos + BlockSize;
                //    if (EndPos > WholeText.Length)
                //    {
                //        EndPos = WholeText.Length - 1;
                //    }

                //    EndPos = WholeText.LastIndexOf("\n", EndPos);
                //    if (EndPos < 0)
                //    {
                //        EndPos = WholeText.Length;//Move to the end
                //    }

                //    string[] Lines = WholeText.Substring(StartPos, EndPos - StartPos).Split(Separator);
                //    ReadedLines.AddRange(Lines.ToList());
                //    StartPos = EndPos + 1;
                //}

                #endregion

                #region Option 2. Read text line by line (Time: 17 seconds. Faster than Option 1)

                //System.Console.WriteLine(" Read start @ {0}", DateTime.Now);
                using (StreamReader sr = new StreamReader(InputFile))
                {
                    while (!sr.EndOfStream)
                    {
                        ReadedLines.Add(sr.ReadLine());
                    }
                }

                #endregion

                //System.Console.WriteLine(" Sort start @ {0}", DateTime.Now);

                //ReadedLines.Sort(Sort_By_String);

                System.Console.WriteLine("End @ {0}", DateTime.Now);
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
}
