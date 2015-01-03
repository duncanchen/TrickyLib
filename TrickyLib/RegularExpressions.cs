using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TrickyLib
{
    public class RegularExpressions
    { 
        public readonly static Regex UselessPunctionRegex = new Regex(@"'(?!(s|t|re|m)( |$))|\.$|\. |\.{2,}|©|`|~|!|@|#|\$|%|\^|\*|\(|\)|(^|[^\w])-+|-+($|[^\w])|_|=|\+|\[|\]|\{|\}|<|>|\\|\||/|;|:|""|•|–|,|\?|×|！|·|…|—|（|）|、|：|；|‘|’|“|”|《|》|，|。|？");
        public readonly static Regex HTML_TagRegex = new Regex(@"</?\w+(.|\n)*?>", RegexOptions.IgnoreCase);
        public readonly static Regex HTML_SelfClosureTagRegex = new Regex(@"<(!|/)?(br|hr|col|img|area|base|link|meta|frame|input|param|doctype|isindex|basefont)(.|\n)*?>", RegexOptions.IgnoreCase);
        public readonly static Regex HTML_FontRegex = new Regex(@"</?(b|font|i|em|big|strong|small|sup|sub|bdo|u|a)( (.|\n)*?)?>", RegexOptions.IgnoreCase);
        public readonly static Regex NonWordCharRegex = new Regex(@"[^A-Za-z0-9 ]", RegexOptions.IgnoreCase);
        public readonly static Regex SvmQidRegex = new Regex(@"qid:\d+");
        public readonly static Regex NumberRegex = new Regex(@"\d+");
        public readonly static Regex GuidRegex_Full = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
        public readonly static Regex GuidRegex_Contain = new Regex(@"(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})");

        public static string NormalizeRegexPattern(string input)
        {
            string output = string.Empty;
            foreach (var c in input)
            {
                switch (c)
                {
                    case '.':
                    case '*':
                    case '+':
                    case '?':
                    case '|':
                    case '(':
                    case ')':
                    case '{':
                    case '}':
                    case '^':
                    case '$':
                        output += "\\" + c;
                        break;
                    default:
                        output += c;
                        break;
                }
            }

            return output;
        }

        public static string NormalizeRegexPattern(IEnumerable<string> inputs)
        {
            string output = string.Empty;
            foreach (var item in inputs)
            {
                if (output != string.Empty)
                    output += "|" + NormalizeRegexPattern(item);
                else
                    output += NormalizeRegexPattern(item);
            }

            return output;
        }
    }
}
