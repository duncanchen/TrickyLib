using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Index.Lucene.Documents;
using Version = TrickyLib.Index.Lucene.Util.Version;
using TrickyLib.Index.Lucene.Analysis;
using TrickyLib.Parser.Json;
using TrickyLib.Index.Lucene.Analysis.Snowball;

namespace TrickyLib.Index.QA_Index
{
    public class Quora_LuceneIndexBuilder : BaseLuceneIndexBuilder<Quora_SearchResult>
    {
        public Quora_LuceneIndexBuilder(Version version, Analyzer analyzer) : base(version, analyzer) { }

        public Quora_LuceneIndexBuilder()
            : this(Version.LUCENE_30, new SnowballAnalyzer(TrickyLib.Index.Lucene.Util.Version.LUCENE_30, "Lingo", true, @"F:\users\v-haowu\Programs\CSharp\TrickyLib\TrickyLib\Resource\wordnet.dict.txt"))
        { 
            
        }

        protected override bool IsNeedToBeIndexedFile(string file)
        {
            return file.Contains("base") && file.Contains("MergeTopic") && file.Contains("part");
        }

        protected override Document ConvertLineToDocument(string line)
        {
            try
            {
                var lineArray = line.Split('\t');

                if (lineArray.Length != 2)
                    return null;

                var jsonObject = new JsonObject(lineArray[1]);
                Document doc = new Document();
                bool hasField = false;
                foreach (var jsonValue in jsonObject)
                {
                    if (_hashFields.Contains(jsonValue.Key))
                    {
                        if(!hasField)
                            hasField = true;

                        if (jsonValue.Key == "TopicArray")
                        { 
                            string topicString = "";
                            foreach (JsonObject topic in jsonValue.Value as JsonArray)
                                topicString += topic["TopicName"] + "; ";

                            doc.Add(new Field("TopicNames", topicString, Field.Store.YES, Field.Index.ANALYZED));
                            doc.Add(new Field("TopicArray", jsonValue.Value.ToString(), Field.Store.YES, Field.Index.NO));
                        }

                        else if (jsonValue.Key == "QuestionText" || jsonValue.Key == "AnswerText" || jsonValue.Key == "UserName")
                        {
                            doc.Add(new Field(jsonValue.Key, jsonValue.Value.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                        }

                        else
                        {
                            doc.Add(new Field(jsonValue.Key, jsonValue.Value.ToString(), Field.Store.YES, Field.Index.NO));
                        }
                    }
                }

                if (hasField)
                    return doc;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }

    public class Quora_SearchResult : LuceneSearchResult
    {
        public string QuestionID { get; set; }
        public string AnswerID { get; set; }
        public string QuestionText { get; set; }
        public string QuestionURL { get; set; }
        public string QuestionDetails { get; set; }
        public string TopicNames { get; set; }
        public string TopicArray { get; set; }
        //public string TopicURL { get; set; }
        public double AnswerScore { get; set; }
        public string AnswerText { get; set; }
        public long AnswerTime { get; set; }
        public string AnswerURL { get; set; }
        public int AnswerViewCount { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        //public string UserPhoto { get; set; }
        //public string UserURL { get; set; }
        //public string FBID { get; set; }
        //public string UserTwitterID { get; set; }
        //public string UserBio { get; set; }
        //public string UserTopicBio { get; set; }
        //public double UserTopicScore { get; set; }
        //public int TopicAnsweredQuestionCount { get; set; }
        //public int TopicExpertCount { get; set; }
        //public int TopicPositiveAnswerCount { get; set; }
        //public int TopicUserAnsweredQuestionCount { get; set; }
        public int QuestionAnswerCount { get; set; }
        //public int TopicQuestionCount { get; set; }
        public int QuestionTopicCount { get; set; }
    }
}
