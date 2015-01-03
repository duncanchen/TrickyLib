using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TrickyLib.IO;
using TrickyLib.Struct;

namespace TrickyLib.Index
{
    public class IndexBuilder
    {
        public string SourceFile { get; set; }
        public string IndexFile { get; set; }

        public const int BufferSize = 4  * 1024;
        public const int MaxRecords = int.MaxValue;

        public List<KeyValuePair<string, Pair<long, long>>> IndexList { get; set; }

        public IndexBuilder(string sourceFile)
        {
            this.SourceFile = sourceFile;
            this.IndexFile = FilePath.GetFilePathWithoutExtenstion(sourceFile) + ".index";
            this.IndexList = new List<KeyValuePair<string, Pair<long, long>>>();
        }
        public void BuildIndex(int bufferSize = 4096)
        {
            if (!File.Exists(this.SourceFile))
                throw new Exception("File does not exists: " + this.SourceFile);

            try
            {
                #region Build index file from txt file
                ConsoleWriter.WriteTime();
                Console.WriteLine("Start build buffer index");

                using (StreamReader sr = new StreamReader(this.SourceFile, Encoding.UTF8))
                {
                    string Context = string.Empty;
                    BinaryReader br = new BinaryReader(sr.BaseStream);
                    this.IndexList.Clear();

                    Console.WriteLine("Reading file...");

                    //Search in the text                                       
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        byte[] BufferSpace = new byte[bufferSize];
                        long StartPos = br.BaseStream.Position;

                        int ReadSize = br.Read(BufferSpace, 0, bufferSize);
                        int back_position = 0;
                        for (int j = ReadSize - 1; j >= 0; --j)
                        {
                            if (BufferSpace[j] == '\n')
                            {
                                back_position = j + 1;
                                break;
                            }
                        }
                        //System.Text.UnicodeEncoding enc = new System.Text.UnicodeEncoding();
                        System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                        Context = enc.GetString(BufferSpace);
                        if (back_position < Context.Length)
                        {
                            Context = Context.Substring(0, back_position);
                            br.BaseStream.Seek((StartPos + back_position), SeekOrigin.Begin);
                        }

                        string[] Separator_Type = { "\r\n" };
                        string[] lines = Context.Split(Separator_Type, StringSplitOptions.RemoveEmptyEntries);
                        if (lines.Length == 1)
                        {
                            throw new Exception("buffer size too small");
                        }
                        string query = string.Empty;
                        foreach (string var in lines)
                        {
                            string[] part = var.Split('\t');
                            //if (part.Length != 2)
                            //    throw new Exception("inverse log format wrong");
                            query = part[0].Trim();
                            break;
                        }

                        if (this.IndexList.Count > 0 && this.IndexList.Last().Key == query)
                            this.IndexList.Last().Value.Second = StartPos + back_position;
                        else
                            this.IndexList.Add(new KeyValuePair<string, Pair<long, long>>(query, new Pair<long, long>(StartPos, StartPos + back_position)));

                        ConsoleWriter.WritePercentage(br.BaseStream.Position, br.BaseStream.Length);
                    }
                    br.Close();
                    sr.Close();
                }

                #endregion

                Console.WriteLine("Printing index...");
                StreamWriter sw = new StreamWriter(this.IndexFile);
                foreach (var item in this.IndexList)
                {
                    sw.WriteLine(item.Key + "\t" + item.Value.First + "\t" + item.Value.Second);
                }
                sw.Close();

                Console.WriteLine("Finished build buffer index");
                ConsoleWriter.WriteTime();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }

            ReadIndex();
        }

        public bool ReadIndex()
        {
            try
            {
                StreamReader sr = new StreamReader(this.IndexFile);
                this.IndexList.Clear();

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] part = line.Split('\t');
                    if (part.Length != 3)
                        throw new Exception("index file format error");
                    long start = long.Parse(part[1]);
                    long end = long.Parse(part[2]);
                    this.IndexList.Add(new KeyValuePair<string, Pair<long, long>>(part[0], new Pair<long, long>(start, end)));
                }
                sr.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public List<string> SearchIndex(string word)
        {
            if (this.IndexList == null || this.IndexList.Count <= 0)
            {
                if (File.Exists(this.IndexFile))
                    ReadIndex();
                else
                    throw new FileNotFoundException(@"Could not find the index file: " + this.IndexFile);
            }

            List<string> outputs = new List<string>();
            int begin = 0;
            int end = this.IndexList.Count - 1;
            if (this.IndexList.Count == 0)
                return outputs;
            int key = -1;
            int compareResult = string.CompareOrdinal(word, this.IndexList[0].Key);
            //int compareResult = word.CompareTo(this.IndexList[0].Key);
            if(compareResult < 0)
                return outputs;
            if (compareResult == 0)
                key = 0;
            else
            {
                while (begin <= end)
                {
                    int mid = (begin + end) / 2;
                    int cmp = string.CompareOrdinal(this.IndexList[mid].Key, word);
                    //int cmp = this.IndexList[mid].Key.CompareTo(word);
                    if (cmp < 0)
                    {
                        key = mid;
                        begin = mid + 1;
                    }
                    else
                    {
                        end = mid - 1;
                    }
                }
            }


            #region get log records from txt file

            using (StreamReader sr = new StreamReader(this.SourceFile, Encoding.UTF8))
            {
                string Context = string.Empty;
                BinaryReader br = new BinaryReader(sr.BaseStream);
                br.BaseStream.Seek(this.IndexList[key].Value.First, SeekOrigin.Begin);
                for (int i = key; i < this.IndexList.Count; ++i)
                {
                    if (i == key + 1)
                    {
                        if (this.IndexList[i].Key.Count() < word.Length)
                            break;
                        if (this.IndexList[i].Key.Substring(0, word.Length).CompareTo(word) != 0)
                            break;
                    }

                    byte[] BufferSpace = new byte[(int)(this.IndexList[i].Value.Second - this.IndexList[i].Value.First)];
                    int ReadSize = br.Read(BufferSpace, 0, (int)(this.IndexList[i].Value.Second - this.IndexList[i].Value.First));
                    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                    Context = enc.GetString(BufferSpace);
                    string[] Separator_Type = { "\r\n" };

                    string[] lines = Context.Split(Separator_Type, StringSplitOptions.RemoveEmptyEntries);
                    int low = -1;
                    if(i == key && string.CompareOrdinal(this.IndexList[i].Key, word) < 0)
                    //if (i == key && this.IndexList[i].Key.CompareTo(word) < 0)
                    {
                        int begin1 = 0, end1 = lines.Length - 1;
                        while (begin1 <= end1)
                        {
                            int mid = (begin1 + end1) / 2;
                            string query = lines[mid].Split('\t')[0];
                            int cmp = string.CompareOrdinal(query, word);
                            //int cmp = query.CompareTo(word);
                            if (cmp >= 0)
                            {
                                low = mid;
                                end1 = mid - 1;
                            }
                            else
                            {
                                begin1 = mid + 1;
                            }
                        }
                    }
                    else
                    {
                        low = 0;
                    }
                    if (low == -1)
                        continue;
                    string[] tmp = word.Split(' ');
                    for (int ii = low; ii < lines.Count(); ii++)
                    {
                        string[] parts = lines[ii].Split('\t')[0].Split(' ');

                        if (parts.Length < tmp.Length)
                            continue;
                        bool is_include = true;
                        for (int j = 0; j < tmp.Length; ++j)
                        {
                            if (tmp[j].CompareTo(parts[j]) != 0)
                            {
                                is_include = false;
                                break;
                            }
                        }
                        if (is_include)
                            outputs.Add(lines[ii]);
                        if (outputs.Count >= MaxRecords)
                            break;
                        if (!is_include)
                            break;
                    }
                    if (outputs.Count >= MaxRecords)
                        break;
                }
                br.Close();
                sr.Close();
            }
            #endregion

            return outputs;
        }
    }
}