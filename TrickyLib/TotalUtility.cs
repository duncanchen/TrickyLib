using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Index.Lucene.Analysis.Snowball;

namespace TrickyLib
{
    public class TotalUtility
    {
        public static SnowballAnalyzer Tokenizer = new SnowballAnalyzer(Index.Lucene.Util.Version.LUCENE_30, "Lingo", true);
    }
}
