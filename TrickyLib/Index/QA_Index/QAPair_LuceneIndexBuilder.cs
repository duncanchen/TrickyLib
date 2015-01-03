using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Index;
using TrickyLib.Index.Lucene.Analysis;
using Version = TrickyLib.Index.Lucene.Util.Version;
using TrickyLib.Index.Lucene.Analysis.Snowball;
using TrickyLib.Index.Lucene.Documents;

namespace TrickyLib.Index.QA_Index
{
    public class QAPair_LuceneIndexBuilder : BaseLuceneIndexBuilder<QAPair_SearchResult>
    {
        public QAPair_LuceneIndexBuilder(Version version, Analyzer analyzer) : base(version, analyzer) { }

        public QAPair_LuceneIndexBuilder()
            : this(Version.LUCENE_30, new SnowballAnalyzer(TrickyLib.Index.Lucene.Util.Version.LUCENE_30, "Lingo", true, @"F:\users\v-haowu\Programs\CSharp\TrickyLib\TrickyLib\Resource\wordnet.dict.txt"))
        {

        }

        protected override bool IsNeedToBeIndexedFile(string file)
        {
            return file.Contains("Heilman.QAP.txt");
        }

        protected override Document ConvertLineToDocument(string line)
        {
            try
            {
                var lineArray = line.Split('\t');

                if (lineArray.Length != 6)
                    return null;

                Document doc = new Document();
                int i = 0;

                doc.Add(new Field("Entity", lineArray[i++], Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("Question", lineArray[i++], Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("Answer", lineArray[i++], Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("DifficultyFromQuestioner", lineArray[i++], Field.Store.YES, Field.Index.NO));
                doc.Add(new Field("DifficultyFromAnswerer", lineArray[i++], Field.Store.YES, Field.Index.NO));
                doc.Add(new Field("SourceDataset", "Wikipedia", Field.Store.YES, Field.Index.NO));
                doc.Add(new Field("SourceDocument", lineArray[i++], Field.Store.YES, Field.Index.NO));

                return doc;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }

    public class QAPair_SearchResult : LuceneSearchResult
    {
        public string Entity { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string DifficultyFromQuestioner { get; set; }
        public string DifficultyFromAnswerer { get; set; }
        public string SourceDataset { get; set; }
        public string SourceDocument { get; set; }
    }
}
