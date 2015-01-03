using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Index.Lucene.Documents;
using TrickyLib.Index.Lucene.Analysis;
using TrickyLib.Extension;
using Version = TrickyLib.Index.Lucene.Util.Version;


namespace TrickyLib.Index.QA_Index
{
    public class QueryLog_LuceneIndexBuilder : BaseLuceneIndexBuilder<QueryLog_SearchResult>
    {
        public static HashSet<string> UselessStartWords = new HashSet<string>(new string[] { "how tall", "how old", "who", "where", "how", "why", "what", "when" });
        public static HashSet<string> UselessStartAndEndWords = new HashSet<string>(new string[] { "where|live", "where|lives", "where|living", "where|lived", "where|from", "where|come from", "where|comes from", "where|came from", "where|born", "where|locate", "where|located", "when|born" });
        public static HashSet<string> UselessAuxiliaryWords = new HashSet<string>(new string[] { "is", "was", "were", "do", "did", "does" });
        public static HashSet<string> UselessWords = new HashSet<string>(new string[] { "a", "an", "the" });
        public static char[] Punctions = new char[] { ',', '.', '?', '!', ';', ':' };

        public QueryLog_LuceneIndexBuilder(Version version, Analyzer analyzer) : base(version, analyzer) { }

        protected override bool IsNeedToBeIndexedFile(string file)
        {
            return file.Contains("SubmitCount") && file.Contains("part");
        }

        protected override Document ConvertLineToDocument(string line)
        {
            var lineArray = line.Split('\t');

            if (lineArray.Length != 2)
                return null;

            Document doc = new Document();
            doc.Add(new Field("Question", lineArray[0], Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("SubmitCount", lineArray[1], Field.Store.YES, Field.Index.NO));
            return doc;
        }

        protected override QueryLog_SearchResult[] PostProcessSearchResults(QueryLog_SearchResult[] searchResults)
        {
            Dictionary<string, QueryLog_SearchResult> searchResultDic = new Dictionary<string, QueryLog_SearchResult>();

            foreach (var searchResult in searchResults)
            {
                var key = GetUsefulQuestionAfterFiltering(searchResult);
                if (!string.IsNullOrEmpty(key))
                {
                    if (searchResultDic.ContainsKey(key))
                        searchResultDic[key].SubmitCount += searchResult.SubmitCount;
                    else
                        searchResultDic.Add(key, searchResult);
                }
            }

            var orderedSearchResults = searchResultDic.Values.OrderByDescending(v => v.SubmitCount).ToArray();
            return orderedSearchResults;
        }

        public string GetUsefulQuestionAfterFiltering(QueryLog_SearchResult searchResult)
        {
            var questionLower = searchResult.Question.ToLower().TrimEnd(Punctions).RemoveMultiSpace();
            var words = questionLower.GetWords();
            int usefulWordCount = words.Length;

            foreach (var startWord in UselessStartWords)
            {
                if (questionLower.StartsWith(startWord))
                {
                    var startWordCount = startWord.GetWordCount();
                    usefulWordCount -= startWordCount;

                    if (words.Length > startWordCount && UselessAuxiliaryWords.Contains(words[startWordCount]))
                        --usefulWordCount;

                    break;
                }
            }

            foreach (var startAndEnd in UselessStartAndEndWords)
            {
                var sAe = startAndEnd.Split('|');
                if (questionLower.StartsWith(sAe[0]) && questionLower.EndsWith(sAe[1]))
                {
                    usefulWordCount -= sAe.Sum(w => w.GetWordCount());
                    var startWordCount = sAe[0].GetWordCount();

                    if (words.Length > startWordCount && UselessAuxiliaryWords.Contains(words[startWordCount]))
                        --usefulWordCount;

                    break;
                }
            }

            string useFullQuestion = "";

            foreach (var word in words)
            {
                if (UselessWords.Contains(word))
                    --usefulWordCount;
                else
                    useFullQuestion += word + " ";
            }

            if (usefulWordCount < 3)
                return "";
            else
                return useFullQuestion;
        }
    }

    public class QueryLog_SearchResult : LuceneSearchResult
    {
        public string Question { get; set; }
        public ulong SubmitCount { get; set; }
    }
}
