using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace TrickyLib
{
    public class HtmlParser
    {
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
            result = Regex.Replace(result, @"<!--[^-]*-->", "", RegexOptions.IgnoreCase);
            return result;
        }
        
        public static string RemoveStyle(string input)
        {
            string result = input;
            //remove all styles 
            result = Regex.Replace(result, @"<style[^>]*?>.*?</style>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return result;
        }
        
        public static string RemoveScript(string input)
        {
            string result = input;
            result = Regex.Replace(result, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            result = Regex.Replace(result, @"<noscript[^>]*?>.*?</noscript>", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return result;
        }

        public static string RemoveTags(string input)
        {
            string result = input;
            //result = result.Replace(" ", " ");
            //result = result.Replace(" ", "\""); 
            //result = result.Replace("<", "<");
            //result = result.Replace(">", ">");
            //result = result.Replace("&", "&");
            result = Regex.Replace(result, @"<[\s\S]*?>", " ", RegexOptions.IgnoreCase);
            return result;
        }

        public static string ReplaceLink(string input)
        {
            return Regex.Replace(input, @"</?a(.|\n)*?>", "###LINK###", RegexOptions.IgnoreCase);
        }

        public static string ReplaceImg(string input)
        {
            return Regex.Replace(input, @"</?img.*?>", "###IMG###", RegexOptions.IgnoreCase);
        }
    }
}
