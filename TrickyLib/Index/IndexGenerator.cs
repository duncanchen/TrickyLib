using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TrickyLib.Index
{
    public class IndexGenerator
    {
        public class SourceFileHandler
        {
            #region Member Variable
            public const int QueryColumnNum = 0;
            public const int MarketColumnNum = 1;
            public bool MarketNull;
            public string SourceFilePath;
            public StreamReader SR;
            public string Query;
            public string Market;
            public string QueryBefore;
            public string MarketBefore;
            public long StartOffsetBefore;
            public long EndOffsetBefore;
            public long StartOffset;
            public long EndOffset;
            public long SearchOffset;
            public long StartLine;
            public long EndLine;
            public bool EndOfStream;
            public string tempLine;
            #endregion End Member Variable
            public SourceFileHandler()
            {
                this.SourceFilePath = string.Empty;
                this.SR = null;
                this.Query = string.Empty;
                this.Market = string.Empty;
                this.QueryBefore = string.Empty;
                this.MarketBefore = string.Empty;
                this.tempLine = string.Empty;
                this.StartOffsetBefore = 0;
                this.EndOffsetBefore = 0;
                this.StartOffset = 0;
                this.EndOffset = 0;
                this.SearchOffset = 0;
                this.StartLine = 0;
                this.EndLine = 0;
                this.EndOfStream = false;
            }
            public SourceFileHandler(string fileName, bool marketNull)
            {
                if (File.Exists(fileName))
                    this.SourceFilePath = fileName;
                else
                    this.SourceFilePath = Path.Combine(IndexGenerator.Root, fileName);
                this.Query = string.Empty;
                this.Market = string.Empty;
                this.QueryBefore = string.Empty;
                this.MarketBefore = string.Empty;
                this.tempLine = string.Empty;
                this.StartOffsetBefore = 0;
                this.EndOffsetBefore = 0;
                this.StartOffset = 0;
                this.EndOffset = 0;
                this.SearchOffset = 0;
                this.StartLine = 0;
                this.EndLine = 0;
                this.EndOfStream = false;
                if (File.Exists(this.SourceFilePath))
                {
                    this.SR = new StreamReader(this.SourceFilePath);
                }
                else
                {
                    this.EndOfStream = true;
                }
                this.MarketNull = marketNull;
            }
            public void GetNextQuery()
            {
                int i;
                if (!this.EndOfStream)
                {
                    //if ((SR.EndOfStream || SR.Peek() == 13) &&
                    //    (string.Compare(this.QueryBefore, this.Query) != 0
                    //    || string.Compare(this.MarketBefore, this.Market) != 0))
                    //{
                    //    this.QueryBefore = this.Query;
                    //    this.MarketBefore = this.Market;
                    //    this.StartLine = this.EndLine;
                    //    this.EndLine++;
                    //    this.StartOffset = this.EndOffset;
                    //    this.EndOffset += (long)fileEncoding.GetByteCount(tempLine + "\r\n");
                    //    //this.StartOffsetBefore = this.StartOffset;
                    //    //this.EndOffsetBefore = this.EndOffset;
                    //}
                    //else if (this.QueryBefore != string.Empty 
                    //    && (this.MarketNull || this.MarketBefore != string.Empty) 
                    //    && (string.Compare(this.QueryBefore, this.Query) == 0
                    //    && string.Compare(this.MarketBefore, this.Market) == 0))
                    //{
                    //    this.EndOfStream = true;
                    //}
                    if (SR.EndOfStream || SR.Peek() == 13)
                    {
                        if ((string.Compare(this.QueryBefore, this.Query, StringComparison.Ordinal) != 0
                              || (!this.MarketNull && string.Compare(this.MarketBefore, this.Market, StringComparison.Ordinal) != 0)))
                        {
                            this.QueryBefore = this.Query;
                            this.MarketBefore = this.Market;
                            this.StartLine = this.EndLine;
                            this.EndLine++;
                            this.StartOffset = this.EndOffset;
                            this.EndOffset += (long)fileEncoding.GetByteCount(tempLine + "\r\n");
                        }
                        else
                        {
                            this.EndOfStream = true;
                        }
                    }
                    else
                    {
                        //this.StartOffsetBefore = this.StartOffset;
                        //this.EndOffsetBefore = this.EndOffset;
                        this.StartOffset = this.EndOffset;
                        this.StartLine = this.EndLine;
                        this.QueryBefore = this.Query;
                        this.MarketBefore = this.Market;
                        while (!SR.EndOfStream && SR.Peek() != 13
                                                && (string.Compare(this.Query, this.QueryBefore, StringComparison.Ordinal) == 0
                                                && string.Compare(this.Market, this.MarketBefore, StringComparison.Ordinal) == 0))
                        {
                            if (this.tempLine != "")
                                this.EndOffset += (long)fileEncoding.GetByteCount(this.tempLine + "\r\n");
                            tempLine = SR.ReadLine();
                            this.EndLine++;
                            string[] columns = tempLine.Split('\t');
                            this.Query = columns[SourceFileHandler.QueryColumnNum];
                            if (!this.MarketNull)
                            {
                                this.Market = columns[SourceFileHandler.MarketColumnNum];
                            }
                        }
                    }
                }
            }
            private int SumBottomIndexCountPerInfo()
            {
                StreamReader tempSR = new StreamReader(this.SourceFilePath);
                string tempLine = tempSR.ReadLine();
                string[] tempColumns = tempLine.Split('\t');
                return tempColumns.Length;
            }
            ~SourceFileHandler()
            {
                if (this.SR != null)
                {
                    this.SR.Close();
                }
            }
        }
        #region Member Variable
        private static int DirIndexCountPerInfo = 3;
        private static int BottonIndexOffsetCountPerFile = 2;
        private static int BottomIndexCountPerInfo;
        private static string IndexRootDir;    //Save position
        private static string Root;
        private string BottomFileName;    //The bottom index file name, for customize
        private string IndexDirName; //The directory name in the saving route, for customize
        private bool NeedComputeTotalQueryCount; //If need to count the query numbers
        private SourceFileHandler[] FilesHandler;     //Source files' handler
        private FileInfo[] SourceFiles;                   //Source files' name
        private int Levels;      //How many levels in need to save the index
        private string LevelIndexGuideTXT;   //The index name in directory, for customize
        private StreamWriter[] LevelIndexWriter;   //The index TXT writer, used in function CreateIndex()
        private StreamWriter BottomFileWriter;     //The bottom index TXT writer, LevelIndexWriter[lowest] = BottomFileWriter, used in CreateIndex()
        private bool[] NeedNew;      //Alert to create new directory or index file
        private long[] MaxCount;     //The max capacity for the level directory and BottomIndex
        private static Encoding fileEncoding = Encoding.UTF8;
        private bool DirectoryCreated = false;
        private long TotalRecords = 0;
        private long FinishedBiTreeRecords = 0;
        #endregion end Member Variable
        #region Constructors
        //public IndexGenerator(string dataRootDir, FileInfo[] sourceFiles,
        //                                        int levels, bool needComputeTotalQueryCount,
        //                                        bool marketNull)
        //{
        //    IndexGenerator.Root = dataRootDir;
        //    IndexGenerator.IndexRootDir = Path.Combine(Root, "TotalIndex");
        //    IndexGenerator.BottomIndexCountPerInfo = 2 + BottonIndexOffsetCountPerFile * sourceFiles.Length;
        //    this.IndexDirName = "-";
        //    this.BottomFileName = "IndexList";
        //    this.SourceFiles = sourceFiles;
        //    this.Levels = levels;
        //    this.NeedComputeTotalQueryCount = needComputeTotalQueryCount;
        //    this.LevelIndexGuideTXT = "Guide-Level.txt";
        //    this.LevelIndexWriter = new StreamWriter[this.Levels];
        //    this.MaxCount = new long[this.Levels];
        //    this.NeedNew = new bool[this.Levels];
        //    for (int i = 0; i < this.Levels; i++)
        //    {
        //        this.NeedNew[i] = true;
        //        if (i < this.Levels - 1)
        //        {
        //            this.MaxCount[i] = 200;        //Max file count
        //        }
        //        else
        //            this.MaxCount[i] = 200;   //Max index count
        //    }

        //    this.FilesHandler = new SourceFileHandler[this.SourceFiles.Length];
        //    for (int i = 0; i < this.SourceFiles.Length; i++)
        //    {
        //        this.FilesHandler[i] = new SourceFileHandler(this.SourceFiles[i].FullName, marketNull);
        //    }
        //}
        //public IndexGenerator(string dataRootDir, int levels)
        //    :this(dataRootDir, (new DirectoryInfo(dataRootDir)).GetFiles(), levels, false, true)
        //{
        //}
        public IndexGenerator(string dataRootDir, string[] files, int levels)
        {
            if (files == null || files.Length == 0 || dataRootDir == null || levels <= 0)
                throw new ArgumentNullException("dataRootDir, files or levels");

            FileInfo[] sourceFiles = new FileInfo[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                if (!File.Exists(files[i]))
                    throw new FileNotFoundException("Not found:", files[i]);

                sourceFiles[i] = new FileInfo(files[i]);
            }

            IndexGenerator.Root = dataRootDir;
            IndexGenerator.IndexRootDir = Path.Combine(Root, "TotalIndex");
            IndexGenerator.BottomIndexCountPerInfo = 2 + BottonIndexOffsetCountPerFile * sourceFiles.Length;
            this.IndexDirName = "-";
            this.BottomFileName = "IndexList";
            this.SourceFiles = sourceFiles;
            this.Levels = levels;
            this.NeedComputeTotalQueryCount = false;
            this.LevelIndexGuideTXT = "Guide-Level.txt";
            this.LevelIndexWriter = new StreamWriter[this.Levels];
            this.MaxCount = new long[this.Levels];
            this.NeedNew = new bool[this.Levels];
            for (int i = 0; i < this.Levels; i++)
            {
                this.NeedNew[i] = true;
                if (i < this.Levels - 1)
                {
                    this.MaxCount[i] = 200;        //Max file count
                }
                else
                    this.MaxCount[i] = 200;   //Max index count
            }

            this.FilesHandler = new SourceFileHandler[this.SourceFiles.Length];
            for (int i = 0; i < this.SourceFiles.Length; i++)
            {
                this.FilesHandler[i] = new SourceFileHandler(this.SourceFiles[i].FullName, true);
            }
        }

        public IndexGenerator(string dataRootDir, int levels)
            : this(dataRootDir, (new DirectoryInfo(dataRootDir)).GetFiles().Select(fi => fi.FullName).ToArray(), levels)
        {
        
        }

        #endregion Constructors
        #region Destructor
        ~IndexGenerator()
        {

            this.NeedNew = null;
            this.MaxCount = null;
            this.LevelIndexWriter = null;
            this.LevelIndexGuideTXT = null;
            this.BottomFileWriter = null;
        }
        #endregion Destructor
        #region Assist Member Function
        private string NewDirectory(string[] CurrentLevelPath, long[] CurrentLevelCount, int i)
        {
            if (i == 0)
                return IndexGenerator.IndexRootDir;
            else
                return Path.Combine(CurrentLevelPath[i - 1], "Level" + (i + 1).ToString() + this.IndexDirName + CurrentLevelCount[i - 1].ToString());
        }
        private string NewFileName(string[] CurrentLevelPath, long[] CurrentLevelCount)  // Bottom Index File
        {
            return Path.Combine(CurrentLevelPath[this.Levels - 2], "Level" + this.Levels.ToString() + this.BottomFileName + CurrentLevelCount[this.Levels - 2].ToString() + ".txt");
        }
        private string NewFileName(string[] CurrentLevelPath, int j)//Dir Index File
        {
            return Path.Combine(CurrentLevelPath[j], this.LevelIndexGuideTXT);
        }
        private bool CreateNewDirAndFile(string QueryBefore, string MarketBefore, ref string[] CurrentLevelPath, ref long[] CurrentLevelCount)
        {
            if (this.NeedNew[this.Levels - 1] == false)          //if the lowest level(BottomFileWriter level)
                return true;                                                         //needs not to create new file, then others should not create new dir
            int i = this.Levels - 1;
            while (i >= 0 && this.NeedNew[i] == true)       //to find the highest level who needs to create new dir
            {
                if (this.NeedNew[0] == false)
                {
                    if (i == this.Levels - 1)
                    {
                        string BTreePath = Path.Combine(Path.GetDirectoryName(CurrentLevelPath[i]),
                                                                                    "BiTree" + Path.GetFileName(CurrentLevelPath[i]));
                        this.LevelIndexWriter[i - 1].WriteLine(QueryBefore + "\t" + MarketBefore + "\t" + BTreePath);
                    }
                    else
                        this.LevelIndexWriter[i - 1].WriteLine(QueryBefore + "\t" + MarketBefore + "\t" + CurrentLevelPath[i]);
                }
                i--;
            }
            i++;     //compensate
            if (i > 0)               //There is no need to count for level 1, because it contains infinity
                CurrentLevelCount[i - 1]++;
            for (int j = i; j < this.Levels; j++)                       //create new file or dir from the highest level who needs to create new,
            {                                                                           //then all of the lower levels needs to create new dir and file
                if (j != this.Levels - 1)
                {
                    CurrentLevelCount[j] = 1;
                    CurrentLevelPath[j] = NewDirectory(CurrentLevelPath, CurrentLevelCount, j);
                    if (!Directory.Exists(CurrentLevelPath[j]))
                    {
                        Directory.CreateDirectory(CurrentLevelPath[j]);
                    }
                    if (this.DirectoryCreated == false)
                    {
                        this.DirectoryCreated = true;
                    }
                    if (this.LevelIndexWriter[j] != null)
                    {
                        this.LevelIndexWriter[j].Flush();
                        this.LevelIndexWriter[j].Close();
                    }
                    this.LevelIndexWriter[j] = new StreamWriter(NewFileName(CurrentLevelPath, j));
                    this.NeedNew[j] = false;
                }
                else
                //j = this.Levels - 1
                {
                    CurrentLevelCount[j] = 0;
                    CurrentLevelPath[j] = NewFileName(CurrentLevelPath, CurrentLevelCount);
                    if (BottomFileWriter != null)
                    {
                        BottomFileWriter.Flush();
                        BottomFileWriter.Close();
                    }
                    BottomFileWriter = new StreamWriter(CurrentLevelPath[j]);
                    this.LevelIndexWriter[j] = BottomFileWriter;
                    this.NeedNew[j] = false;
                }
            }
            return true;
        }
        private bool AddLineAndCheckForNew(string Query, string Market,
                                                                    string[] CurrentLevelPath, ref long[] CurrentLevelCount)
        {
            BottomFileWriter.Write(Query + "\t" + Market);
            foreach (SourceFileHandler Handler in FilesHandler)
            {
                if (!Handler.EndOfStream &&
                    Handler.QueryBefore == Query && (Handler.MarketNull || Handler.MarketBefore == Market))
                {
                    /*
                    BottomFileWriter.Write(
                        "\t" + Handler.StartOffset + "\t" + (Handler.EndOffset - 1) + "\t"
                               + Handler.StartLine + "\t" + Handler.EndLine + "\t" + Handler.SourceFilePath);
                     */
                    BottomFileWriter.Write("\t" + Handler.StartOffset + "\t" + (Handler.EndOffset - 1));
                    Handler.GetNextQuery();
                }
                else
                {
                    for (int i = 0; i < BottonIndexOffsetCountPerFile; i++)
                    {
                        BottomFileWriter.Write("\t");
                    }
                }
            }
            BottomFileWriter.WriteLine();
            CurrentLevelCount[this.Levels - 1]++;
            for (int i = 0; i < this.Levels; i++)
            {

                if (i > 0 && CurrentLevelCount[i] >= this.MaxCount[i])
                {
                    this.NeedNew[i] = true;
                    CurrentLevelCount[i] = 0;
                }
            }
            return true;
        }
        private bool AllUseless()
        {
            foreach (SourceFileHandler Handler in this.FilesHandler)
            {
                if (!Handler.EndOfStream)
                {
                    return false;
                }
            }
            return true;
        }
        private void GetNewQuery(out string Query, out string Market)
        {
            Query = string.Empty;
            Market = string.Empty;
            foreach (SourceFileHandler Handler in this.FilesHandler)
            {
                if (!Handler.EndOfStream)
                {
                    if ((Query == string.Empty || string.Compare(Query, Handler.QueryBefore, StringComparison.Ordinal) > 0)
                        || (!Handler.MarketNull && (Market == string.Empty || string.Compare(Market, Handler.MarketBefore, StringComparison.Ordinal) > 0)))
                    {
                        Query = Handler.QueryBefore;
                        Market = Handler.MarketBefore;
                    }
                }
            }
        }
        #endregion Assist Member Function
        #region CreateIndex
        private long CreateIndex()
        {
            #region Prepare
            if (IndexGenerator.Root == null
                    || fileEncoding == null)
            {
                return 0;
            }
            DateTime beginTime = System.DateTime.Now;
            Console.WriteLine("Build index started at:" + beginTime);
            long totalIndex = 0;

            long[] CurrentLevelCount = new long[this.Levels];
            for (int i = 0; i < this.Levels; i++)
            {
                CurrentLevelCount[i] = 0;
            }

            string[] CurrentLevelPath = new string[this.Levels];

            for (int i = 0; i < this.Levels - 1; i++)
            {
                CurrentLevelPath[i] = NewDirectory(CurrentLevelPath, CurrentLevelCount, i);
            }

            string Query = string.Empty;
            string Market = string.Empty;

            this.TotalRecords = 0;
            this.FinishedBiTreeRecords = 0;

            #endregion end Prepare
            #region Start Create Index

            try
            {
                foreach (SourceFileHandler Handler in this.FilesHandler)
                {
                    Handler.GetNextQuery();
                    Handler.GetNextQuery();
                }
                while (!AllUseless())
                {
                    CreateNewDirAndFile(Query, Market, ref CurrentLevelPath, ref CurrentLevelCount);
                    GetNewQuery(out Query, out Market);
                    if (Query != string.Empty)
                    {
                        AddLineAndCheckForNew(Query, Market,
                                               CurrentLevelPath, ref CurrentLevelCount);
                        totalIndex++;
                        if (totalIndex % 1000000 == 0)
                        {
                            Console.WriteLine("Built {0} M index.", totalIndex);
                        }
                    }
                }
                if (this.DirectoryCreated)
                {
                    for (int i = 0; i < this.Levels - 1; i++)
                    {
                        if (i == this.Levels - 2)
                        {
                            string BTreePath = Path.Combine(Path.GetDirectoryName(CurrentLevelPath[i + 1]),
                                                                                        "BiTree" + Path.GetFileName(CurrentLevelPath[i + 1]));
                            this.LevelIndexWriter[i].WriteLine(Query + "\t" + Market + "\t" + BTreePath);
                        }
                        else
                            this.LevelIndexWriter[i].WriteLine(Query + "\t" + Market + "\t" + CurrentLevelPath[i + 1]);
                    }
                }
            }
            #region catch
            catch (ArgumentException ex)
            {
                //Logger.WriteException(LogID.FILE_UDPCREATEINDEX,
                //                      typeof(UserDataIndexGenerator),
                //                      "CreateIndexFile Failed: Speicified file path is not a legal path.",
                //                      ex);
                throw new Exception("CreateIndexFile Failed: Speicified file path is not a legal path." + ex.ToString());
            }
            catch (IOException ex)
            {
                //Logger.WriteException(LogID.FILE_UDPCREATEINDEX,
                //                      typeof(UserDataIndexGenerator),
                //                      "CreateIndexFile Failed: File specified can not be found.",
                //                      ex);
                throw new Exception("CreateIndexFile Failed: File specified can not be found." + ex.ToString());
            }
            catch (UnauthorizedAccessException ex)
            {
                //Logger.WriteException(LogID.FILE_UDPCREATEINDEX,
                //                      typeof(UserDataIndexGenerator),
                //                      "CreateIndexFile Failed: Getting access to specified file is declined.",
                //                      ex);
                throw new Exception("CreateIndexFile Failed: Getting access to specified file is declined." + ex.ToString());
            }
            #endregion end catch
            finally
            {
                if (this.DirectoryCreated)
                {
                    for (int i = 0; i < this.Levels; i++)
                    {
                        this.LevelIndexWriter[i].Flush();
                        this.LevelIndexWriter[i].Close();
                    }
                }
            }

            #endregion end Create Index
            #region Write Config.txt
            if (this.DirectoryCreated)
            {
                StreamWriter ConfigSW = new StreamWriter(Path.Combine(IndexGenerator.IndexRootDir, "Config.txt"));
                ConfigSW.WriteLine("DirIndexCountPerInfo=" + "\t" + IndexGenerator.DirIndexCountPerInfo);
                ConfigSW.WriteLine("BottomIndexCountPerInfo=" + "\t" + IndexGenerator.BottomIndexCountPerInfo);
                ConfigSW.WriteLine("SourceFilesNum=" + "\t" + this.SourceFiles.Length);
                ConfigSW.WriteLine("SourceFiles:");
                foreach (FileInfo fileName in this.SourceFiles)
                {
                    ConfigSW.Write("\t" + fileName.FullName);
                }
                ConfigSW.Flush();
                ConfigSW.Close();
            }
            else
            {
                Console.WriteLine("There is no content in the sourcefile(s), or there is empty line in file, so no index built");
            }
            #endregion End Write Config.txt
            return totalIndex;
        }
        private bool RecursiveCreateBiTreeIndex(string CurrentLevelPath)
        {
            DirectoryInfo CurrentLevelDIR = new DirectoryInfo(CurrentLevelPath);


            DirectoryInfo[] ChildDIRs = CurrentLevelDIR.GetDirectories();
            if (ChildDIRs.Length == 0)
            {
                FileInfo[] ChildFiles = CurrentLevelDIR.GetFiles();
                foreach (FileInfo file in ChildFiles)
                {
                    if (file.FullName.Contains("IndexList"))
                    {
                        this.FinishedBiTreeRecords += CreateBiTreeIndex(file.FullName, IndexGenerator.BottomIndexCountPerInfo);
                        if (this.FinishedBiTreeRecords % 1000000 == 0 && this.TotalRecords > 0)
                        {
                            decimal finishedPercentage = Convert.ToDecimal(this.FinishedBiTreeRecords) / Convert.ToDecimal(this.TotalRecords);
                            Console.SetCursorPosition(0, Console.CursorTop - 1);
                            Console.WriteLine((finishedPercentage * 100).ToString("f1") + "%");
                        }
                    }
                    else
                    {
                        CreateBiTreeIndex(file.FullName, IndexGenerator.DirIndexCountPerInfo);
                    }
                }
            }
            else
            {
                string GuideTXT = Path.Combine(CurrentLevelDIR.FullName, this.LevelIndexGuideTXT);
                CreateBiTreeIndex(GuideTXT, IndexGenerator.DirIndexCountPerInfo);
                foreach (DirectoryInfo dir in ChildDIRs)
                {
                    RecursiveCreateBiTreeIndex(dir.FullName);
                }
            }
            return true;
        }
        private long CreateBiTreeIndex(string GuideTXT, int offset)
        {
            StreamReader GuideReader = new StreamReader(GuideTXT);
            string DIRpath = Path.GetDirectoryName(GuideTXT);
            string ABCGuideTXT = Path.Combine(DIRpath, "BiTree" + Path.GetFileName(GuideTXT));

            List<string> DivideNode = new List<string>();
            string QueryInfoLine;
            while (!GuideReader.EndOfStream)
            {
                QueryInfoLine = GuideReader.ReadLine();
                DivideNode.Add(QueryInfoLine);
            }
            BinaryTreeNode Root = new BinaryTreeNode(offset);
            Root = Root.RecursiveBuildTree(DivideNode, 0, DivideNode.Count - 1);
            Root.PrintToTXT(ABCGuideTXT);
            GuideReader.Close();
            File.Delete(GuideTXT);
            return DivideNode.Count;
        }
        public bool StartCreateIndex()
        {
            try
            {
                this.TotalRecords = this.CreateIndex();
                Console.WriteLine("Total index {0}", this.TotalRecords);
                Console.WriteLine("Start create BiTreeIndex, at: " + DateTime.Now.ToString());
                Console.WriteLine();
                if (this.DirectoryCreated)
                {
                    this.RecursiveCreateBiTreeIndex(IndexGenerator.IndexRootDir);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public class BinaryTreeNode
        {
            public BinaryTreeNode LNode, RNode, PNode;
            public string QueryInfo;
            public int Offset;
            public BinaryTreeNode(int offset)
            {
                this.Offset = offset;
            }
            public BinaryTreeNode RecursiveBuildTree(List<string> DivideNode, int start, int end)
            {
                BinaryTreeNode Node = null;
                if (start < end)
                {
                    Node = new BinaryTreeNode(this.Offset);
                    int mid = (int)Math.Ceiling(((double)start + (double)end) / 2);
                    Node.QueryInfo = DivideNode[mid];
                    BinaryTreeNode Left = RecursiveBuildTree(DivideNode, start, mid - 1);
                    BinaryTreeNode Right = RecursiveBuildTree(DivideNode, mid + 1, end);
                    Node.LNode = Left;
                    Node.RNode = Right;
                    if (Left != null)
                        Left.PNode = Node;
                    if (Right != null)
                        Right.PNode = Node;
                }
                else if (start == end)
                {
                    Node = new BinaryTreeNode(this.Offset);
                    Node.QueryInfo = DivideNode[start];
                }
                return Node;
            }
            public void PrintToTXT(string path)
            {
                StreamWriter SW = new StreamWriter(path);
                int serial = 0, level = 1;
                bool NullAppears = false;
                bool EnQueueLock = false;
                Queue<BinaryTreeNode> NodeQueue = new Queue<BinaryTreeNode>();
                NodeQueue.Enqueue(this);
                while (NodeQueue.Count > 0)
                {
                    BinaryTreeNode Node = NodeQueue.Dequeue();
                    serial++;
                    if (Node != null)
                    {
                        string[] ColumnsToChange = Node.QueryInfo.Split('\t');
                        if (ColumnsToChange.Length == IndexGenerator.DirIndexCountPerInfo)
                        {
                            ColumnsToChange[IndexGenerator.DirIndexCountPerInfo - 1] = Path.GetFileName(ColumnsToChange[IndexGenerator.DirIndexCountPerInfo - 1]);
                        }
                        //else
                        //{
                        //    for (int i = 1 + IndexGenerator.BottomIndexCountPerQuery;
                        //        i < IndexGenerator.BottomIndexCountPerInfo;
                        //        i += IndexGenerator.BottomIndexCountPerQuery)
                        //    {
                        //        if (ColumnsToChange[i] != string.Empty)
                        //        {
                        //            ColumnsToChange[i]
                        //            = Path.GetFileName(ColumnsToChange[i]);
                        //        }
                        //    }
                        //    //SW.Write(Node.QueryInfo + "\t");
                        //}
                        string WrittenStr = string.Empty;
                        for (int i = 0; i < ColumnsToChange.Length - 1; i++)
                        {
                            WrittenStr += ColumnsToChange[i] + "\t";
                        }
                        WrittenStr += ColumnsToChange[ColumnsToChange.Length - 1];
                        SW.WriteLine(WrittenStr);

                        if (Node.LNode == null || Node.RNode == null)
                            NullAppears = true;

                        if (!EnQueueLock)
                        {
                            NodeQueue.Enqueue(Node.LNode);
                            NodeQueue.Enqueue(Node.RNode);
                        }
                    }
                    else
                    {
                        SW.WriteLine();
                    }
                    if (serial == (int)Math.Pow(2, (double)level) - 1)
                    {
                        level++;
                        if (NullAppears == true)
                            EnQueueLock = true;
                    }
                }
                SW.Flush();
                SW.Close();
            }
        }

        #endregion
    }

    public enum LogType
    {
        INFO,
        ERROR
    }

    public class Helper
    {
        #region Log
        static string mainLogFile = null;

        public static void Log(LogType type, string message)
        {
            if (mainLogFile == null)
            {
                InitLogFile();
            }
            // open log file
            StreamWriter sw = new StreamWriter(mainLogFile, true);
            string currentTime = DateTime.Now.ToString();
            string tag;
            if (type == LogType.INFO)
            {
                tag = "[INFO]";
            }
            else
            {
                tag = "[ERROR]";
            }
            string line = "[" + currentTime + "]" + "\t" + tag + "\t" + message;
            sw.WriteLine(line);
            sw.Close();
            Console.WriteLine(message);
        }

        static void InitLogFile()
        {
            string directory = @"c:\data\logal\logs\";
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            mainLogFile = directory + "dataServerlog_" + currentDate + ".txt";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        #endregion
    }

    public class MergeHandler
    {
        class InfoContainer
        {
            public int Columns;
            private string SourcePath;
            public StreamReader SR;
            public string Query;
            public string Market;
            public string Tail;
            public bool EndofStream;
            public InfoContainer(string path)
            {
                this.SourcePath = path;
                this.Query = null;
                this.Market = null;
                this.Tail = null;
                this.Columns = -1;
                if (File.Exists(this.SourcePath))
                {
                    SR = new StreamReader(this.SourcePath);
                    this.EndofStream = false;
                }
                else
                {
                    SR = null;
                    this.EndofStream = true;
                }
            }
            private void HandleLine(string Line)
            {
                string[] columns = Line.Trim().Split('\t');
                if (this.Columns < 0)
                {
                    this.Columns = columns.Length;
                }
                this.Query = columns[MergeHandler.QueryColumn];
                if (!MergeHandler.MarketNull)
                {
                    this.Market = columns[MergeHandler.MarketColumn];
                }
                this.Tail = Line.TrimStart((this.Query + "\t" + this.Market).ToCharArray()).Trim();
            }
            public void GetNextQuery()
            {
                if (this.SR.EndOfStream || this.SR.Peek() == 13 || this.SR.Peek() == 9)
                {
                    this.EndofStream = true;
                    return;
                }
                HandleLine(this.SR.ReadLine());
            }
        }
        private InfoContainer[] Infos;
        public const int QueryColumn = 0;
        public const int MarketColumn = 1;
        public static bool MarketNull;
        private string[] SourceFiles;
        private StreamWriter SW;
        public MergeHandler(bool marketNull, string[] path)
        {
            this.SourceFiles = path;
            MergeHandler.MarketNull = marketNull;
            this.Infos = new InfoContainer[this.SourceFiles.Length];
            for (int i = 0; i < this.SourceFiles.Length; i++)
            {
                this.Infos[i] = new InfoContainer(this.SourceFiles[i]);
            }
        }
        public MergeHandler(string[] path)
            : this(true, path)
        {
        }
        ~MergeHandler()
        {
            this.Infos = null;
            this.SourceFiles = null;
        }
        private bool NoNeedProcess()
        {
            int total = 0;
            foreach (InfoContainer info in this.Infos)
            {
                if (!info.EndofStream)
                {
                    total++;
                    if (total >= 2)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void GetNewQuery(out string Query, out string Market)
        {
            Query = string.Empty;
            Market = string.Empty;
            foreach (InfoContainer info in this.Infos)
            {
                if (!info.EndofStream)
                {
                    if ((Query == string.Empty || string.Compare(Query, info.Query, StringComparison.Ordinal) > 0)
                        || (!MergeHandler.MarketNull && (Market == string.Empty || string.Compare(Market, info.Market, StringComparison.Ordinal) > 0)))
                    {
                        Query = info.Query;
                        Market = info.Market;
                    }
                }
            }
        }
        private void AddLine(string Query, string Market)
        {
            this.SW.Write(Query);
            if (!MergeHandler.MarketNull)
            {
                this.SW.Write("\t" + Market);
            }
            foreach (InfoContainer info in this.Infos)
            {
                if (info.EndofStream ||
                    string.Compare(info.Query, Query, StringComparison.Ordinal) != 0 ||
                    (!MergeHandler.MarketNull && string.Compare(info.Market, Market, StringComparison.Ordinal) != 0))
                {
                    int i = 0;
                    this.SW.Write("\t");
                    while (i < info.Columns - 2)
                    {
                        this.SW.Write("\t");
                        i++;
                    }
                }
                else
                {
                    this.SW.Write("\t" + info.Tail);
                    info.GetNextQuery();
                }
            }
            this.SW.WriteLine();
        }
        private void PrintLast()
        {
            int i;
            for (i = 0; i < this.Infos.Length; i++)
            {
                if (!this.Infos[i].EndofStream)
                {
                    break;
                }
            }
            if (i >= this.Infos.Length)
            {
                return;
            }
            while (!this.Infos[i].EndofStream)
            {
                this.SW.Write(this.Infos[i].Query);
                if (!MergeHandler.MarketNull)
                {
                    this.SW.Write("\t" + this.Infos[i].Market);
                }
                for (int j = 0; j < this.Infos.Length; j++)
                {
                    if (j != i)
                    {
                        this.SW.Write("\t");
                        int k = 0;
                        while (k < this.Infos[j].Columns - 2)
                        {
                            this.SW.Write("\t");
                            k++;
                        }
                    }
                    else
                    {
                        this.SW.Write("\t" + this.Infos[i].Tail);
                        this.Infos[i].GetNextQuery();
                    }
                }
                this.SW.WriteLine();
            }
        }
        public void Merge(string DesPath, string DesName)
        {
            string Query;
            string Market;
            if (NoNeedProcess())
            {
                Console.WriteLine("There are less than 2 files");
                SW = null;
                return;
            }
            SW = new StreamWriter(Path.Combine(DesPath, DesName));
            foreach (InfoContainer info in this.Infos)
            {
                info.GetNextQuery();

            }
            while (!NoNeedProcess())
            {
                GetNewQuery(out Query, out Market);
                AddLine(Query, Market);
            }
            PrintLast();
            SW.Flush();
            SW.Close();
        }
        public void Merge(string DesName)
        {
            string DesPath = Path.GetDirectoryName(this.SourceFiles[0]);
            Merge(DesPath, DesName);
        }
        public void Merge()
        {
            string DesPath = Path.GetDirectoryName(this.SourceFiles[0]);
            string DesName = "MergeFile.txt";
            Merge(DesPath, DesName);
        }
    }

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
            try
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
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
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
