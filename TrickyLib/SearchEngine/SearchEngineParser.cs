using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Parser;
using System.Web;
using System.Runtime.Serialization;
using System.Threading;
using TrickyLib.Extension;

namespace TrickyLib.SearchEngine
{
    public class SearchEngineParser
    {
        public static Random RandomWait = new Random(DateTime.Now.Millisecond);

        public static string BuildUrl(string query, SearchEngines engine)
        {
            string queryEncoded = HttpUtility.UrlEncode(query);
            string url = string.Empty;

            if (engine == SearchEngines.Google)
                url = "http://www.google.com/search?hl=en&q=" + queryEncoded;
            else if (engine == SearchEngines.Bing)
                url = "http://www.bing.com/search?mkt=en-us&q=" + queryEncoded;
            else if (engine == SearchEngines.Baidu)
                url = "http://www.baidu.com/s?wd=" + queryEncoded;
            else if (engine == SearchEngines.Yahoo)
                url = "http://search.yahoo.com/search?q=" + queryEncoded;

            return url;
        }

        public static List<SearchResult> GetSearchResults(string query, SearchEngines engine, int topN = int.MaxValue, bool randomWait = false)
        {
            if (randomWait)
                Thread.Sleep(RandomWait.Next(5, 10) * 70);

            string url = BuildUrl(query, engine);
            string html = HtmlParser.DownloadHtml(url);
            html = HtmlParser.RecoverFontTag(HtmlParser.ReplaceFontTag(HtmlParser.RemoveCSS(html)));

            string tagName = string.Empty;
            Dictionary<string, string> resultAttributeDic = new Dictionary<string, string>();
            if (engine == SearchEngines.Google)
            {
                tagName = "div";
                resultAttributeDic.Add("id", "ires");
            }
            else if (engine == SearchEngines.Bing)
            {
                tagName = "ul";
                resultAttributeDic.Add("class", "sb_results");
            }
            else if (engine == SearchEngines.Baidu)
            {
                tagName = "div";
                resultAttributeDic.Add("id", "container");            
            }
            else if (engine == SearchEngines.Yahoo)
            {
                tagName = "ol";
                resultAttributeDic.Add("start", "1");
            }

            int start = 0;
            string specificTag = HtmlParser.GetSpecificTag(html, tagName, resultAttributeDic, ref start);

            if (specificTag == null)
            {
                SearchResult result = new SearchResult()
                {
                    Query = query,
                    Rating = "NoResult"
                };

                List<SearchResult> noResultOutput = new List<SearchResult>();
                noResultOutput.Add(result);
                return noResultOutput;
            }

            start -= specificTag.Length;
            string tagCoveredString = HtmlParser.GetTagCoveredString(html, start);

            TagTreeNode root = TagTreeNode.BuildTagTree(tagCoveredString);
            List<SearchResult> searchResults = new List<SearchResult>();

            Dictionary<string, string> searchResultAttributeDic = new Dictionary<string, string>();
            Dictionary<string, string> snippetAttributeDic = new Dictionary<string, string>();

            string searchResultTagName = string.Empty;
            string titleTagName = string.Empty;
            string snippetTagName = string.Empty;
            string hostUrl = string.Empty;

            if (engine == SearchEngines.Google)
            {
                hostUrl = "http://www.google.com";

                searchResultTagName = "li";
                searchResultAttributeDic.Add("class", "g");

                titleTagName = "h3";

                snippetTagName = "div";                
                snippetAttributeDic.Add("class", "s");
            }
            else if (engine == SearchEngines.Bing)
            {
                hostUrl = "http://www.bing.com";

                searchResultTagName = "li";
                titleTagName = "h3";
                snippetTagName = "p";
            }
            else if (engine == SearchEngines.Baidu)
            {
                hostUrl = "http://www.baidu.com";

                searchResultTagName = "table";
                titleTagName = "h3";

                snippetTagName = "td";
                snippetAttributeDic.Add("class", "f");
            }
            else if (engine == SearchEngines.Yahoo)
            {
                searchResultTagName = "li";
                titleTagName = "h3";

                snippetTagName = "div";
                snippetAttributeDic.Add("class", "f");
            }

            List<TagTreeNode> searchResultTags = root.WideSearchNodes(searchResultTagName, searchResultAttributeDic);
            foreach(var searchResultTag in searchResultTags)
            {
                TagTreeNode header3 = searchResultTag.DeepSearchNode(titleTagName, new Dictionary<string, string>());
                if (header3 != null)
                {
                    SearchResult searchResult = new SearchResult()
                    {
                        Query = query,
                        Title = header3.Text,
                        Rank = searchResults.Count + 1
                    };

                    //Url
                    string headerHtml = HtmlParser.RecoverFontTag(HtmlParser.RecoverFontTag(header3.Text));
                    Tag urlTag = new Tag(string.Empty);
                    int urlStart = 0;
                    while (urlTag.TagName != "a" && urlStart < headerHtml.Length)
                        urlTag = new Tag(HtmlParser.GetNextTag(headerHtml, ref urlStart));
                    if (urlTag.TagName == "a" && urlTag.AttributeDic.ContainsKey("href"))
                    {
                        string titleUrl = urlTag.AttributeDic["href"];
                        if (urlTag.AttributeDic["href"].StartsWith("/"))
                        {
                            if (engine == SearchEngines.Google)
                            {
                                int httpIndex = titleUrl.IndexOf("http://");
                                if (httpIndex >= 0)
                                {
                                    int endIndex = titleUrl.IndexOf('&', httpIndex);
                                    if (endIndex >= 0)
                                        titleUrl = titleUrl.Substring(httpIndex, endIndex - httpIndex);
                                    else
                                        titleUrl = titleUrl.Substring(httpIndex);
                                }
                                else
                                    titleUrl = "http://www.google.com" + titleUrl;
                            }
                            else if (engine == SearchEngines.Bing)
                                titleUrl = "http://www.bing.com" + titleUrl;
                            else if (engine == SearchEngines.Baidu)
                                titleUrl = "http://www.baidu.com" + titleUrl;
                            else if (engine == SearchEngines.Yahoo)
                                titleUrl = HttpUtility.UrlDecode(titleUrl.Split(new string[] { "**" }, StringSplitOptions.RemoveEmptyEntries)[1]);
                        }
                        searchResult.Url = titleUrl;
                    }

                    //Snippet
                    TagTreeNode snippet = searchResultTag.DeepSearchNode(snippetTagName, snippetAttributeDic);
                    if (snippet != null)
                        searchResult.Snippet = snippet.Text;

                    searchResults.Add(searchResult);
                    if (searchResults.Count >= topN)
                        break;
                }
            }

            return searchResults;
        }

        public static string[] QueryToSegments(string query)
        {
            string[] words = query.Trim().GetWords();
            List<string> segments = new List<string>();
            string segment = string.Empty;
            bool waitEndColon = false;

            foreach (var word in words)
            {
                if (word.StartsWith("\""))
                    waitEndColon = true;

                segment += " " + word.Trim('\"');

                if (word.EndsWith("\""))
                    waitEndColon = false;

                if (!waitEndColon)
                {
                    segments.Add(segment.Trim());
                    segment = string.Empty;
                }
            }

            if (segment.Trim() != string.Empty)
                segments.Add(segment.Trim());

            return segments.ToArray();
        }

        public static string SegmentsToQuery(IEnumerable<string> segments)
        {
            return segments.Select(s => /*s.GetWordCount() <= 1 ? s :*/ s.Bracket("\"")).ConnectWords();
        }

        public static int RatingToInt(string rating)
        { 
            if (rating.ToLowerInvariant() == "perfect")
                return 5;
            else if (rating.ToLowerInvariant() == "excellent")
                return 4;
            else if (rating.ToLowerInvariant() == "good")
                return 3;
            else if (rating.ToLowerInvariant() == "fair")
                return 2;
            else //if (rating.ToLowerInvariant() == "bad")
                return 1;
            //else
            //    return 0;
        }
    }

    [DataContract]
    public class SearchResult
    {
        [DataMember]
        public string Query { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string TitlePure
        {
            get
            {
                if (this.Title != null)
                    return HtmlParser.HtmlDecode(HtmlParser.RemoveFontReplacement(HtmlParser.RecoverFontTag(this.Title)));
                else
                    return string.Empty;
            }
        }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string Snippet { get; set; }
        [DataMember]
        public string SnippetPure
        {
            get
            {
                if (this.Snippet != null)
                    return HtmlParser.HtmlDecode(HtmlParser.RemoveFontReplacement(HtmlParser.RecoverFontTag(this.Snippet)));
                else
                    return string.Empty;
            }
        }
        [DataMember]
        public int Rank { get; set; }
        [DataMember]
        public string Rating { get; set; }

        public override string ToString()
        {
            string output = this.Query
                + ", " + this.TitlePure.Replace("\t", " ##TAB## ").Replace("\r\n", " ##N## ").Replace("\r", " ##N## ").Replace("\n", " ##N## ")
                + ", " + this.Url.Replace("\t", " ##TAB## ").Replace("\r\n", " ##N## ").Replace("\r", " ##N## ").Replace("\n", " ##N## ")
                + ", " + this.SnippetPure.Replace("\t", " ##TAB## ").Replace("\r\n", " ##N## ").Replace("\r", " ##N## ").Replace("\n", " ##N## ")
                + ", " + this.Rank
                + ", " + this.Rating;

            return output;
        }

        public string GetPrintLine()
        {
            string output = this.Query
                + "\t" + this.TitlePure.Replace("\t", " ##TAB## ").Replace("\r\n", " ##N## ").Replace("\r", " ##N## ").Replace("\n", " ##N## ")
                + "\t" + this.Title.Replace("\t", " ##TAB## ").Replace("\r\n", " ##N## ").Replace("\r", " ##N## ").Replace("\n", " ##N## ")
                + "\t" + this.Url.Replace("\t", " ##TAB## ").Replace("\r\n", " ##N## ").Replace("\r", " ##N## ").Replace("\n", " ##N## ")
                + "\t" + this.SnippetPure.Replace("\t", " ##TAB## ").Replace("\r\n", " ##N## ").Replace("\r", " ##N## ").Replace("\n", " ##N## ")
                + "\t" + this.Snippet.Replace("\t", " ##TAB## ").Replace("\r\n", " ##N## ").Replace("\r", " ##N## ").Replace("\n", " ##N## ")
                + "\t" + this.Rank
                + "\t" + this.Rating;

            return output;
        }

        public SearchResult()
        {
            this.Query = string.Empty;
            this.Title = string.Empty;
            this.Url = string.Empty;
            this.Snippet = string.Empty;
            this.Rank = -1;
            this.Rating = string.Empty;
        }
    }

    [DataContract]
    public enum SearchEngines
    { 
        [EnumMember]
        Google,
        [EnumMember]
        Bing,
        [EnumMember]
        Baidu,
        [EnumMember]
        Yahoo
    }
}
