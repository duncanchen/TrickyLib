using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace TrickyLib.Parser
{
    public class HtmlParser
    {
        public const string LeftBracketReplacement = " ###LEFT### ";
        public const string RightBracketReplacement = " %%%RIGHT%%% ";
        public const string ImageReplacement = " ###IMG### ";
        public const string LinkReplacement = " ###LINK### ";
        public const string EnterReplacement = " ###N### ";
        public const string TabReplacement = " ###TAB### ";

        public static Regex TagRegex = new Regex(@"</?\w+(.|\n)*?>");
        public static Regex SelfClosureTagRegex = new Regex(@"<(!|/)?(br|hr|col|img|area|base|link|meta|frame|input|param|doctype|isindex|basefont)(.|\n)*?>");

        public static string GetUrlHost(string url)
        {
            Uri uri = new Uri(url);
            return uri.Host;
        }

        public static string GetUrlPathAndQuery(string url)
        {
            Uri uri = new Uri(url);
            return uri.PathAndQuery;
        }

        public static string DownloadHtml(string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                ((HttpWebRequest)request).UserAgent = "HtmlParser";
                ((HttpWebRequest)request).Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                //AddProxyToRequest(request, "itgproxy");
                WebResponse response = request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                string HTML = sr.ReadToEnd();
                sr.Close();
                return HTML;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string DownloadHtml2(string url)
        {
            WebClient aWebClient = new WebClient();
            aWebClient.Encoding = Encoding.Default;
            string HTML = aWebClient.DownloadString(url);

            return HTML;
        }

        public static string ExtractText(string html)
        {
            string result = html;
            result = result.Replace("<br>", "\r\n");
            result = RemoveComment(result);
            result = RemoveScript(result);
            result = RemoveStyle(result);
            result = RemoveTags(result);
            return result.Trim();
        }

        public static string HtmlDecode(string input)
        {
            return HttpUtility.HtmlDecode(input);
        }

        public static string HtmlEncode(string input)
        {
            return HttpUtility.HtmlEncode(input);
        }

        public static string RemoveComment(string input)
        {
            string result = input;
            //remove comment 
            result = Regex.Replace(result, @"(<!--(.|\n)*?-->|/\*(.|\n)*?\*/)", "", RegexOptions.IgnoreCase);
            return result;
        }

        public static string RemoveStyle(string input)
        {
            string result = input;
            //remove all styles 
            result = Regex.Replace(result, @"<style(.|\n)*?>.*?</style>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return result;
        }

        public static string RemoveScript(string input)
        {
            string result = input;
            result = Regex.Replace(result, @"<script(.|\n)*?>.*?</script>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            result = Regex.Replace(result, @"<noscript(.|\n)*?>.*?</noscript>", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return result;
        }

        public static string RemoveCSS(string input)
        {
            return RemoveComment(RemoveStyle(RemoveScript(input)));
        }

        public static string RemoveFont(string input)
        {
            return Regex.Replace(input, @"</?(b|font|i|em|big|strong|small|sup|sub|bdo|u|a)( (.|\n)*?)?>", "", RegexOptions.IgnoreCase);
        }

        public static string RemoveTags(string input)
        {
            return Regex.Replace(input, @"</?[\s\S](.|\n)*?>", "", RegexOptions.IgnoreCase);
        }

        public static string RemoveFontReplacement(string input)
        {
            Regex fontRegex = new Regex(LeftBracketReplacement + "(.|\n)*?" + RightBracketReplacement);
            return fontRegex.Replace(input, "");
        }

        public static string ReplaceFontTag(string html)
        {
            Regex FontTagRegex = new Regex(@"</?(b|font|i|em|big|strong|small|sup|sub|bdo|u|a)( (.|\n)*?)?>");
            string replacedHtml = string.Empty;
            int currentIndex = 0;
            Match currentMatch = null;

            while ((currentMatch = FontTagRegex.Match(html, currentIndex)).Value != string.Empty)
            {
                int length = currentMatch.Index - currentIndex;
                replacedHtml += html.Substring(currentIndex, length);
                replacedHtml += LeftBracketReplacement + currentMatch.Value.Trim("<>".ToArray()) + RightBracketReplacement;

                currentIndex = currentMatch.Index + currentMatch.Length;
            }

            int lengthLeft = html.Length - currentIndex;
            if (lengthLeft > 0)
                replacedHtml += html.Substring(currentIndex, lengthLeft);

            return replacedHtml;
        }

        public static string RecoverFontTag(string replacedHtml)
        {
            return Regex.Replace(Regex.Replace(replacedHtml, LeftBracketReplacement.TrimStart(), "<"), RightBracketReplacement.TrimEnd(), ">");
        }

        public static string ReplaceNonFontTags(string input, string replacement = "\r\n")
        {
            return Regex.Replace(input, @"</?(?<!b|font|i|em|big|strong|small|sup|sub|bdo|u|a)( (.|\n)+?)?>", replacement, RegexOptions.IgnoreCase);
        }

        public static string ReplaceLink(string input, string replacement = LinkReplacement)
        {
            return Regex.Replace(input, @"</?a(.|\n)*?>", replacement, RegexOptions.IgnoreCase);
        }

        public static string ReplaceImg(string input, string replacement = ImageReplacement)
        {
            return Regex.Replace(input, @"</?img.*?>", replacement, RegexOptions.IgnoreCase);
        }

        public static string ReplaceTags(string input, string replacement = "\r\n")
        {
            return Regex.Replace(input, @"</?[\s\S]*?>", replacement, RegexOptions.IgnoreCase);
        }

        public static string RecoverHtml(string html)
        {
            return html.Replace(LeftBracketReplacement, "<").Replace(RightBracketReplacement, ">");
        }

        public static string ReplaceEnter(string html, string replacement = EnterReplacement)
        {
            return Regex.Replace(html, "\n", EnterReplacement, RegexOptions.IgnoreCase);
        }

        public static string ReplaceTab(string html, string replacement = TabReplacement)
        {
            return html.Replace("\t", TabReplacement);
        }

        public static bool IsSelfClosureTag(string tag)
        {
            return tag.TrimEnd().EndsWith("/>") || SelfClosureTagRegex.IsMatch(tag);
        }

        public static string GetNextTag(string html, ref int start)
        {
            if (start >= html.Length)
                return null;

            //html = html.Replace("###LEFT###", "<").Replace("%%%RIGHT%%%", ">");
            
            int tagStartIndex = html.IndexOf('<', start);
            if (tagStartIndex < 0)
                return null;

            int rightBracketCount = 0;
            int leftBracketCount = 1;
            int currentIndex = tagStartIndex + 1;
            while (currentIndex < html.Length && leftBracketCount != rightBracketCount)
            {
                if (html[currentIndex] == '<')
                    leftBracketCount++;
                else if (html[currentIndex] == '>')
                    rightBracketCount++;

                currentIndex++;
            }

            if (leftBracketCount == rightBracketCount)
            {
                string tagString = html.Substring(tagStartIndex, currentIndex - tagStartIndex);
                start = currentIndex;

                return tagString;
            }
            else
                return null;
        }

        private static void AddProxyToRequest(WebRequest request, string webProxy)
        {
            if (!String.IsNullOrEmpty(webProxy))
            {
                string proxyAddress = webProxy;
                int proxyPort = 80;
                if (webProxy.Contains(":"))
                {
                    string[] proxyParts = webProxy.Split(new char[] { ':' });
                    proxyAddress = proxyParts[0];
                    proxyPort = Int32.Parse(proxyParts[1]);
                }
                request.Proxy = new System.Net.WebProxy(proxyAddress, proxyPort);
            }
        }

        public static string GetSpecificTag(string html, string tagName, Dictionary<string, string> attributeDic, ref int start)
        {
            if (attributeDic.Count <= 0)
                throw new ArgumentException("properties.Length <= 0 || properties.Length % 2 != 0");

            string tagString = string.Empty;
            while (tagString != null)
            {
                Tag tag = new Tag(tagString);
                if (tag.TagName.ToLowerInvariant() == "div")
                { 
                
                }
                if (tag.HasAttributes(attributeDic) && tag.TagName.ToLowerInvariant() == tagName.ToLowerInvariant())
                    return tagString;

                tagString = GetNextTag(html, ref start);
            }

            return null;
        }

        public static string GetTagCoveredString(string html, int start)
        {
            Stack<Tag> tagStack = new Stack<Tag>();
            int newStart = start;

            Tag startTag = new Tag(GetNextTag(html, ref newStart));
            if (startTag.IsEnd || startTag.IsSelfClosure)
                return html.Substring(start, newStart);

            tagStack.Push(startTag);
            while (tagStack.Count > 0)
            {
                Tag tag = new Tag(GetNextTag(html, ref newStart));

                if (tag.IsEnd)
                    tagStack.Pop();
                else if (!tag.IsSelfClosure)
                    tagStack.Push(tag);
            }

            return html.Substring(start, newStart - start);
        }

        public static string NormalizeUrl(string url)
        {
            string normalizedUrl = url.ToLowerInvariant().Trim().TrimEnd('/').Trim();
            if (normalizedUrl != string.Empty)
                return normalizedUrl.StartsWith("http://") ? normalizedUrl : "http://" + normalizedUrl;
            else
                return string.Empty;
        }
    }

    public class Tag
    {
        public static Regex TagNameRegex = new Regex(@"<(!|/)?\w+");
        public static Regex AttributeRegex = new Regex("\\S+=\"(.|\n)*?\"");
        public static HashSet<string> HeaderTags = new HashSet<string>(new string[] { "h1", "h2", "h3", "h4", "h5", "h6" });

        public bool IsHeader { get; set; }
        public bool IsTitle { get; set; }
        public bool IsEnd { get; set; }
        public bool IsSelfClosure { get; set; }

        public string TagName { get; set; }

        public Dictionary<string, string> AttributeDic { get; set; }

        public Tag(string tagString)
        {
            this.IsEnd = tagString.StartsWith("</");
            this.IsSelfClosure = HtmlParser.IsSelfClosureTag(tagString);
            this.TagName = TagNameRegex.Match(tagString).Value.TrimStart("<!/".ToArray());
            this.IsHeader = HeaderTags.Contains(this.TagName);
            this.IsTitle = this.TagName == "title";

            foreach (var match in AttributeRegex.Matches(tagString, this.TagName.Length))
            {
                if (this.AttributeDic == null)
                    this.AttributeDic = new Dictionary<string, string>();

                string[] kv = match.ToString().Split('=');
                string key = kv[0].ToLower();
                string value = match.ToString().Substring(key.Length + 1).Trim('\"');

                if (!this.AttributeDic.ContainsKey(key))
                    this.AttributeDic.Add(key.ToLowerInvariant(), value);
            }
        }

        public override string ToString()
        {
            string output = "<";
            if (this.IsEnd)
                output += "/";

            output += this.TagName.ToUpper();

            if (this.AttributeDic != null)
            {
                foreach (var kv in this.AttributeDic)
                {
                    output += " " + kv.Key + "=\"" + kv.Value + "\"";
                }

                if (this.IsSelfClosure)
                    output += "/";
            }

            output += ">";
            return output;
        }

        public bool HasAttributes(Dictionary<string, string> attributeDic)
        {
            foreach (var kv in attributeDic)
            {
                if (this.AttributeDic == null)
                    return false;
                if (!this.AttributeDic.ContainsKey(kv.Key.ToLowerInvariant()))
                    return false;
                else if (this.AttributeDic[kv.Key] != kv.Value)
                    return false;
            }

            return true;
        }
    }

    public class TagTreeNode : Tag
    {
        public string Text { get; set; }
        public List<TagTreeNode> Children { get; set; }

        public TagTreeNode(string tagString)
            : base(tagString)
        {
            this.Children = new List<TagTreeNode>();
        }

        public static TagTreeNode BuildTagTree(string html)
        { 
            string cleanHtml = HtmlParser.ReplaceFontTag(HtmlParser.RemoveCSS(html));
            int start = 0;
            int lastEnd = 0;
            Stack<TagTreeNode> TagStack = new Stack<TagTreeNode>();

            TagTreeNode root = new TagTreeNode(string.Empty);
            TagStack.Push(root);

            while (start < cleanHtml.Length)
            {
                //Visible Text
                string tagString = HtmlParser.GetNextTag(cleanHtml, ref start);
                string fatherText = cleanHtml.Substring(lastEnd, start - tagString.Length - lastEnd).Trim();
                lastEnd = start;

                TagTreeNode fatherNode = TagStack.Peek();
                fatherNode.Text += (!string.IsNullOrEmpty(fatherNode.Text) && fatherText != string.Empty ? " " : "") + fatherText;

                TagTreeNode tag = new TagTreeNode(tagString);

                if (tag.IsEnd)  //Node end
                {
                    if (tag.TagName != TagStack.Peek().TagName)
                        throw new Exception("Bracket not match!");

                    TagStack.Pop();
                }
                else    //New Node
                {
                    //Add to father node
                    fatherNode.Children.Add(tag);

                    //Can have children
                    if (!tag.IsSelfClosure)
                        TagStack.Push(tag);
                }
            }

            return TagStack.Pop();
        }

        public TagTreeNode DeepSearchNode(string tagName, Dictionary<string, string> attributeDic)
        {
            if (this.TagName == tagName && this.HasAttributes(attributeDic))
                return this;
            else
                foreach (var child in this.Children)
                {
                    TagTreeNode matchNode = child.DeepSearchNode(tagName, attributeDic);
                    if (matchNode != null)
                        return matchNode;
                }

            return null;
        }

        public List<TagTreeNode> WideSearchNodes(string tagName, Dictionary<string, string> attributeDic)
        {
            List<TagTreeNode> matchResults = new List<TagTreeNode>();
            Queue<TagTreeNode> levelQueue = new Queue<TagTreeNode>();
            
            levelQueue.Enqueue(this);
            bool resultAppears = false;

            while (levelQueue.Count > 0)
            {
                foreach (var node in levelQueue)
                {
                    if (node.TagName == tagName && node.HasAttributes(attributeDic))
                    {
                        resultAppears = true;
                        matchResults.Add(node);
                    }
                }

                if (resultAppears)
                    break;
                else
                {
                    int levelCount = levelQueue.Count;
                    for (int i = 0; i < levelCount; i++)
                    {
                        TagTreeNode node = levelQueue.Dequeue();
                        foreach (var child in node.Children)
                            levelQueue.Enqueue(child);
                    }
                }
            }

            return matchResults;
        }
    }
}
