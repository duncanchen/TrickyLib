/* 
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using SF.Snowball.Ext;
using Version = Lucene.Net.Util.Version;
using System.Reflection;

namespace TrickyLib.Index.Lucene.Analysis.Snowball
{

    /// <summary>Filters <see cref="StandardTokenizer"/> with <see cref="StandardFilter"/>, {@link
    /// LowerCaseFilter}, <see cref="StopFilter"/> and <see cref="SnowballFilter"/>.
    /// 
    /// Available stemmers are listed in <see cref="SF.Snowball.Ext"/>.  The stemmerName of a
    /// stemmer is the part of the class name before "Stemmer", e.g., the stemmer in
    /// <see cref="EnglishStemmer"/> is named "English".
    /// 
    /// <p><b>NOTE:</b> This class uses the same <see cref="Version"/>
    /// dependent settings as <see cref="StandardAnalyzer"/></p>
    /// </summary>
    public class SnowballAnalyzer : Analyzer
    {
        public string WordNetFilePath { get; set; }
        private System.String stemmerName;
        private ISet<string> StopWords;
        public bool UseStopWords { get; set; }
        private readonly Version matchVersion;

        /// <summary>Builds the named analyzer with no stop words. </summary>
        public SnowballAnalyzer(Version matchVersion, System.String stemmerName, bool useStopWords, string wordNetFilePath)
        {
            this.stemmerName = stemmerName;
            SetOverridesTokenStreamMethod<SnowballAnalyzer>();
            this.matchVersion = matchVersion;
            this.UseStopWords = useStopWords;
            this.WordNetFilePath = wordNetFilePath;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Lucene.Net.Analysis.Snowball.Resource.stopword.txt"))
            using (StreamReader sr = new StreamReader(stream))
            {
                HashSet<string> stopWords = new HashSet<string>();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    if (!string.IsNullOrEmpty(line))
                        stopWords.Add(line);
                }

                StopWords = stopWords;
            }
        }

        /// <summary>Builds the named analyzer with the given stop words. </summary>
        [Obsolete("Use SnowballAnalyzer(Version, string, ISet) instead.")]
        public SnowballAnalyzer(Version matchVersion, System.String stemmerName, System.String[] stopWords, string wordNetFilePath)
            : this(matchVersion, stemmerName, true, wordNetFilePath)
        {
            StopWords = StopFilter.MakeStopSet(stopWords);
        }

        /// <summary>
        /// Builds the named analyzer with the given stop words.
        /// </summary>
        public SnowballAnalyzer(Version matchVersion, string stemmerName, ISet<string> stopWords, string wordNetFilePath)
            : this(matchVersion, stemmerName, true, wordNetFilePath)
        {
            StopWords = CharArraySet.UnmodifiableSet(CharArraySet.Copy(stopWords));
        }

        /// <summary>Constructs a <see cref="StandardTokenizer"/> filtered by a {@link
        /// StandardFilter}, a <see cref="LowerCaseFilter"/> and a <see cref="StopFilter"/>. 
        /// </summary>
        public override TokenStream TokenStream(System.String fieldName, System.IO.TextReader reader)
        {
            TokenStream result = new StandardTokenizer(matchVersion, reader);
            result = new StandardFilter(result);
            result = new LowerCaseFilter(result);
            if (UseStopWords && StopWords != null && StopWords.Count > 0)
                result = new StopFilter(StopFilter.GetEnablePositionIncrementsVersionDefault(matchVersion),
                                        result, StopWords);
            result = new SnowballFilter(result, stemmerName, WordNetFilePath);
            return result;
        }

        private class SavedStreams
        {
            internal Tokenizer source;
            internal TokenStream result;
        };

        /* Returns a (possibly reused) {@link StandardTokenizer} filtered by a 
         * {@link StandardFilter}, a {@link LowerCaseFilter}, 
         * a {@link StopFilter}, and a {@link SnowballFilter} */

        public override TokenStream ReusableTokenStream(String fieldName, TextReader reader)
        {
            if (overridesTokenStreamMethod)
            {
                // LUCENE-1678: force fallback to tokenStream() if we
                // have been subclassed and that subclass overrides
                // tokenStream but not reusableTokenStream
                return TokenStream(fieldName, reader);
            }

            SavedStreams streams = (SavedStreams)PreviousTokenStream;
            if (streams == null)
            {
                streams = new SavedStreams();
                streams.source = new StandardTokenizer(matchVersion, reader);
                streams.result = new StandardFilter(streams.source);
                streams.result = new LowerCaseFilter(streams.result);
                if (StopWords != null)
                    streams.result = new StopFilter(StopFilter.GetEnablePositionIncrementsVersionDefault(matchVersion),
                                                    streams.result, StopWords);
                streams.result = new SnowballFilter(streams.result, stemmerName, WordNetFilePath);
                PreviousTokenStream = streams;
            }
            else
            {
                streams.source.Reset(reader);
            }
            return streams.result;
        }
    }
}