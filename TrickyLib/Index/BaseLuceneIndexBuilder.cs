using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Index.Lucene.Index;
using System.IO;
using System.Threading;
using TrickyLib.Index.Lucene.Documents;
using TrickyLib.Index.Lucene.Analysis;
using TrickyLib.Index.Lucene.QueryParsers;
using TrickyLib.Index.Lucene.Store;
using TrickyLib.Index.Lucene.Search;
using FSDirectory = TrickyLib.Index.Lucene.Store.FSDirectory;
using Version = TrickyLib.Index.Lucene.Util.Version;
using TrickyLib.Index.Lucene.Analysis.Standard;
using TrickyLib.Index.Lucene.Analysis.Snowball;
using TrickyLib.IO;
using TrickyLib.Reflection;
using TrickyLib.Extension;
using TrickyLib;

namespace TrickyLib.Index
{
    public abstract class BaseLuceneIndexBuilder<TSearchResult> where TSearchResult : LuceneSearchResult
    {
        public static int RamWriterStorageLines = 100000;
        public string TargetDir { get; set; }
        private string IndexDir
        {
            get
            {
                if (string.IsNullOrEmpty(TargetDir))
                    return null;
                else
                    return Path.Combine(TargetDir, "Index");
            }
        }

        public static int MaxThreadCount = 18;
        private int TotalThreadCount { get; set; }
        private int FinishedThreadCount { get; set; }
        private int TotalFileCount { get; set; }
        private int FinishedFileCount { get; set; }
        protected object ThreadLock = "I am a thread Lock";

        protected HashSet<string> _hashFields;
        public string[] Fields { get; set; }

        private IndexWriter _indexWriter;
        private IndexSearcher _indexReader;

        private Analyzer _analyzer;
        public Analyzer Analyzer
        {
            get
            {
                return _analyzer;
            }
            set
            {
                _analyzer = value;
            }
        }

        private Version _version;
        public Version Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }

        static BaseLuceneIndexBuilder()
        {
            MaxThreadCount = int.Parse(System.Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS")) * 3 / 4;
            if (MaxThreadCount <= 0)
                MaxThreadCount = 1;
        }

        public BaseLuceneIndexBuilder(Version version, Analyzer analyzer)
        {
            _version = version;
            _analyzer = analyzer;
        }
        ~BaseLuceneIndexBuilder()
        {
            if (_indexReader != null)
                _indexReader.Dispose();
        }

        #region Build Index
        public void BuildindexForDir(string targetDir)
        {
            //Start
            ConsoleWriter.WriteCurrentMethodStarted();
            Console.WriteLine("Lucene index building:");
            Console.WriteLine("Directory: " + targetDir);
            Console.WriteLine("Version: " + Version.ToString());
            Console.WriteLine("Analyzer: " + this.Analyzer.GetType().Name);
            DateTime startTime = DateTime.Now;

            //Get fieldNames
            var fields = ReflectionHandler.GetPropertyNames(typeof(TSearchResult)).ToArray();
            if (fields == null || fields.Length <= 0)
            {
                throw new Exception("The Fields is null or empty! Please specify the Fields in the construction function");
            }
            else
            {
                Fields = fields;
                _hashFields = new HashSet<string>(fields);
            }

            TargetDir = targetDir;

            //Initialize writer
            _indexWriter = new IndexWriter(FSDirectory.Open(IndexDir), _analyzer, true, new IndexWriter.MaxFieldLength(1000000));

            //Get all files
            var files = System.IO.Directory.GetFiles(TargetDir).Where(f => IsNeedToBeIndexedFile(f)).ToList();
            if (files.Count <= 0)
                throw new Exception("No files are selected to be indexed");
            else
                Console.WriteLine(files.Count + " files to be indexed");
            TotalFileCount = files.Count;
            FinishedFileCount = 0;

            //Initial thread count
            TotalThreadCount = Math.Min(files.Count, MaxThreadCount);
            FinishedThreadCount = 0;
            Console.WriteLine(TotalThreadCount + " threads to be paralleling");
            int filePerThread = files.Count / TotalThreadCount;
            int threadIndex = 1;

            //Multi-thread build index
            Console.WriteLine("Start building index...");
            ConsoleWriter.WritePercentage(0, TotalFileCount);

            for (int i = 0; i < files.Count; )
            {
                List<string> threadFiles = null;

                if (threadIndex < TotalThreadCount)
                {
                    threadFiles = files.GetRange(i, filePerThread).ToList();
                    i += filePerThread;
                }
                else
                {
                    threadFiles = files.GetRange(i, files.Count - i).ToList();
                    i = files.Count;
                }

                ++threadIndex;
                Thread thread = new Thread(BuildIndex_OneThread);
                thread.Start(threadFiles);
            }

            //If multi-thread finished
            while (FinishedThreadCount < TotalThreadCount)
                ;

            //Finish IndexWriter
            Console.WriteLine("Index building completed. Index optimizing..." + DateTime.Now.ToShortTimeString());
            _indexWriter.Optimize();
            _indexWriter.Dispose();

            //Write the column file
            string columnFile = Path.Combine(IndexDir, "ColumnNames.txt");
            FileWriter.PrintCollection(columnFile, Fields);

            ConsoleWriter.WriteCurrentMethodFinished();
            var timeUsed = DateTime.Now - startTime;
            Console.WriteLine("Total time for building index:" + timeUsed.TotalHours.ToString("0.00") + "h");
        }
        private void BuildIndex_OneThread(object files)
        {
            foreach (var file in files as List<string>)
                BuildIndex_OneFile(file);

            lock (ThreadLock)
            {
                ++FinishedThreadCount;
            }
        }
        protected virtual void BuildIndex_OneFile(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                uint finishedLines = 0;
                RAMDirectory ramDir = new RAMDirectory();
                IndexWriter ramWriter = new IndexWriter(ramDir, _analyzer, new IndexWriter.MaxFieldLength(1000000));

                while (!sr.EndOfStream)
                {
                    if (finishedLines > 0 && finishedLines % RamWriterStorageLines == 0)
                    {
                        lock (ThreadLock)
                        {
                            ramWriter.Dispose();
                            _indexWriter.AddIndexesNoOptimize(ramDir);
                        }

                        ramDir = new RAMDirectory();
                        ramWriter = new IndexWriter(ramDir, _analyzer, new IndexWriter.MaxFieldLength(1000000));
                        finishedLines = 0;
                    }

                    var doc = ConvertLineToDocument(sr.ReadLine());
                    if (doc != null)
                    {
                        ramWriter.AddDocument(doc);
                        ++finishedLines;
                    }
                }

                if (ramWriter != null && finishedLines > 0)
                {
                    lock (ThreadLock)
                    {
                        ramWriter.Close();
                        _indexWriter.AddIndexesNoOptimize(ramDir);
                    }
                }
            }

            lock (ThreadLock)
            {
                ++FinishedFileCount;
                ConsoleWriter.WritePercentage(FinishedFileCount, TotalFileCount);
            }
        }
        protected virtual bool IsNeedToBeIndexedFile(string file)
        {
            return true;
        }
        protected abstract Document ConvertLineToDocument(string line);
        #endregion

        #region Load Index
        public void LoadIndex(string targetDir)
        {
            try
            {
                TargetDir = targetDir;
                Fields = ReflectionHandler.GetPropertyNames(typeof(TSearchResult)).ToArray();
                _indexReader = new IndexSearcher(FSDirectory.Open(IndexDir));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region Search
        public TSearchResult[] Search(string queryString, int queryN, QueryParser.Operator andOr, string[] requiredFields)
        {
            MultiFieldQueryParser queryParser = new MultiFieldQueryParser(_version, Fields, _analyzer);
            queryParser.DefaultOperator = andOr;

            var query = queryParser.Parse(RegularExpressions.UselessPunctionRegex.Replace(queryString, " ").Replace('?', ' ').Replace('*', ' ').Replace('-', ' ').RemoveMultiSpace());
            return PostProcessSearchResults(SearchQuery(query, queryN, requiredFields));
        }

        public TSearchResult[] Search(string queryString, int queryN, QueryParser.Operator andOr, string[] requiredFields, string searchedField)
        {
            QueryParser queryParser = new QueryParser(_version, searchedField, _analyzer);
            queryParser.DefaultOperator = andOr;

            var query = queryParser.Parse(RegularExpressions.UselessPunctionRegex.Replace(queryString, " ").Replace('?', ' ').Replace('*', ' ').Replace('-', ' ').RemoveMultiSpace());
            return PostProcessSearchResults(SearchQuery(query, queryN, requiredFields));
        }

        public TSearchResult[] Search(string queryString, int queryN, QueryParser.Operator andOr, string[] requiredFields, string[] searchedFields)
        {
            MultiFieldQueryParser queryParser = new MultiFieldQueryParser(_version, searchedFields, _analyzer);
            queryParser.DefaultOperator = andOr;

            var query = queryParser.Parse(RegularExpressions.UselessPunctionRegex.Replace(queryString, " ").Replace('?', ' ').Replace('*', ' ').Replace('-', ' ').RemoveMultiSpace());
            return PostProcessSearchResults(SearchQuery(query, queryN, requiredFields));
        }

        private TSearchResult[] SearchQuery(Query query, int queryN, string[] requiredFields)
        {
            int rank = 1;
            List<TSearchResult> tSearchResults = new List<TSearchResult>();

            foreach (var searchResult in _indexReader.Search(query, queryN).ScoreDocs)
            {
                var tSearchResult = ConvertDocumentToQuestion(_indexReader.Doc(searchResult.Doc), requiredFields);
                tSearchResult.LuceneRank = rank++;
                tSearchResult.LuceneScore = searchResult.Score;
                tSearchResults.Add(tSearchResult);
            }
            return tSearchResults.ToArray();
        }

        protected virtual TSearchResult[] PostProcessSearchResults(TSearchResult[] searchResults)
        {
            return searchResults;
        }

        private TSearchResult ConvertDocumentToQuestion(Document doc, string[] requiredFields)
        {
            TSearchResult searchResult = Activator.CreateInstance<TSearchResult>();
            foreach (var requiredField in requiredFields)
                ReflectionHandler.SetProperty(searchResult, requiredField, doc.Get(requiredField));

            return searchResult;
        }
        #endregion
    }

    public class LuceneSearchResult
    {
        public int LuceneRank { get; set; }
        public double LuceneScore { get; set; }
    }
}
