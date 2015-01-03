using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TrickyLib
{
    public class SearchClass
    {
        public class QueryContainer
        {
            public string Query;
            public string Market;
            public List<string> positions;

            public QueryContainer(string query, string market)
            {
                this.Query = query;
                this.Market = market;
            }

            public QueryContainer()
                : this(string.Empty, string.Empty)
            {

            }
        }
        private int FilesNum;
        private int BottomOffset;
        private int DirOffset;
        private string IndexRootDir;
        public string currentDirectory;
        private DirectoryInfo dir;
        private FileInfo[] FilesName;

        public SearchClass(string indexRootDir)
        {
            this.IndexRootDir = Path.Combine(indexRootDir, "TotalIndex");
            dir = new DirectoryInfo(indexRootDir);
            FilesName = dir.GetFiles();

            StreamReader SR = new StreamReader(Path.Combine(this.IndexRootDir, "Config.txt"));
            string[] tempLine = SR.ReadLine().Split('\t');
            this.DirOffset = int.Parse(tempLine[1]);
            tempLine = SR.ReadLine().Split('\t');
            this.BottomOffset = int.Parse(tempLine[1]);
        }

        private QueryContainer SearchQueryMain(string query, string Market, string DIRPath)
        {
            bool Bottom;
            int Offset;
            if (DIRPath.Equals(this.IndexRootDir))
            {
                currentDirectory = this.IndexRootDir;
            }
            else
            {
                DIRPath = currentDirectory + "\\" + DIRPath;
                currentDirectory = DIRPath;
            }

            FileInfo fileInfo = new FileInfo(DIRPath);
            StreamReader SR = null;
            string path;
            if ((fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                Bottom = false;
                SR = new StreamReader(Path.Combine(DIRPath, "BiTreeGuide-Level.txt"));
                Offset = this.DirOffset;
            }
            else
            {
                Bottom = true;
                SR = new StreamReader(DIRPath);
                Offset = this.BottomOffset;
            }
            string tempLine;
            if (!SR.EndOfStream)
            {
                tempLine = SR.ReadLine();
            }
            else
            {
                return null;
            }
            string[] Comparer = tempLine.Split('\t');
            string[] ComparerLeftTreeAncestor = null;
            long CurrentLineNum = 1;
            if (Bottom)
            {

            }
            while (true)
            {
                #region Bingo
                if (string.Compare(query, Comparer[0], StringComparison.OrdinalIgnoreCase) == 0 && (string.IsNullOrEmpty(Market) || string.Compare(Market, Comparer[1], StringComparison.OrdinalIgnoreCase) == 0))
                {
                    if (Bottom)
                    {
                        QueryContainer Result = new QueryContainer(Comparer[0], Comparer[1]);
                        List<string> positions = new List<string>();
                        for (int i = 2; i < this.BottomOffset; i += 2)
                        {
                            positions.Add(Comparer[i]);
                            positions.Add(Comparer[i + 1]);
                        }
                        Result.positions = positions;
                        return Result; ;
                    }
                    else
                    {
                        path = Comparer[Offset - 1];
                        return SearchQueryMain(query, Market, path);
                    }
                }
                #endregion End Bingo
                #region ToLeftTree
                else if (string.Compare(query, Comparer[0], StringComparison.OrdinalIgnoreCase) < 0
                         || string.Compare(query, Comparer[0], StringComparison.OrdinalIgnoreCase) == 0 && (string.IsNullOrEmpty(Market) || string.Compare(Market, Comparer[1], StringComparison.OrdinalIgnoreCase) < 0))
                {
                    for (long LineNum = CurrentLineNum; LineNum < CurrentLineNum * 2; LineNum++)
                    {
                        if (SR.EndOfStream)
                        {
                            tempLine = string.Empty;
                            break;
                        }
                        tempLine = SR.ReadLine();
                    }
                    CurrentLineNum *= 2;
                    if (tempLine.Trim() == string.Empty)
                    {
                        if (Bottom)
                        {
                            //Helper.Log(LogType.INFO, query + ", " + Market + " is not found");
                            return null;
                        }
                        else
                        {
                            path = Comparer[Offset - 1];
                            return SearchQueryMain(query, Market, path);
                        }
                    }
                    else
                    {
                        ComparerLeftTreeAncestor = Comparer;
                        Comparer = tempLine.Split('\t');
                    }
                }
                #endregion ToLeftTree
                #region ToRightTree
                else
                {
                    for (long LineNum = CurrentLineNum; LineNum < CurrentLineNum * 2 + 1; LineNum++)
                    {
                        if (SR.EndOfStream)
                        {
                            tempLine = string.Empty;
                            break;
                        }
                        tempLine = SR.ReadLine();
                    }
                    CurrentLineNum = 2 * CurrentLineNum + 1;
                    if (tempLine.Trim() == string.Empty)
                    {
                        if (Bottom || ComparerLeftTreeAncestor == null)
                        {
                            //Helper.Log(LogType.INFO, query + ", " + Market + " is not found");
                            return null;
                        }
                        else
                        {
                            path = ComparerLeftTreeAncestor[Offset - 1];
                            return SearchQueryMain(query, Market, path);
                        }
                    }
                    else
                    {
                        Comparer = tempLine.Split('\t');
                    }
                }
                #endregion ToRightTree
            }
        }

        private QueryContainer SearchQueryMain(string query)
        {
            return SearchQueryMain(query, string.Empty, this.IndexRootDir);
        }

        public List<string> SearchQuery(string query)
        {
            return SearchQuery(query.Trim(), string.Empty);
        }

        private List<string> SearchQuery(string query, string market)
        {
            List<string> resultList = new List<string>();
            QueryContainer qc = SearchQueryMain(query.Trim(), market, this.IndexRootDir);
            if (qc != null)
            {
                for (int i = 0; i < qc.positions.Count; i += 2)
                {
                    string filePath = FilesName[i / 2].FullName;

                    bool hasValidData = false;
                    long startOffset;

                    hasValidData = long.TryParse(qc.positions[i], out startOffset);
                    if (hasValidData)
                    {
                        long endOffset;
                        hasValidData = long.TryParse(qc.positions[i + 1], out endOffset);
                        if (hasValidData)
                        {
                            FileInfo fi = new FileInfo(filePath);
                            FileStream fs = fi.OpenRead();

                            fs.Seek(startOffset, SeekOrigin.Begin);
                            byte[] str = new byte[endOffset - startOffset + 1];

                            fs.Read(str, 0, str.Length);
                            string featureStr = Encoding.UTF8.GetString(str);

                            resultList.Add(featureStr);
                            //Console.WriteLine(query, ": found, file:" + filePath);
                            //Console.WriteLine(featureStr);
                        }
                    }
                }
            }
            return resultList;
        }
    }
}
