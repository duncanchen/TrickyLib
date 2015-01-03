using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenNLP.Tools.Tokenize;
using OpenNLP.Tools.PosTagger;
using OpenNLP.Tools.Chunker;
using OpenNLP.Tools.NameFind;
using OpenNLP.Tools.Parser;
using OpenNLP.Tools.SentenceDetect;

namespace Paraphrase.Util
{
    class CSharpNLPUtil
    {

        private static EnglishMaximumEntropyTokenizer s_tokenizer = null;
        private static EnglishMaximumEntropyPosTagger s_postagger = null;
        private static MaximumEntropyChunker s_chunker = null;
        private static EnglishNameFinder s_NER = null;
        private static EnglishTreebankParser s_parser = null;
        private static EnglishMaximumEntropySentenceDetector s_sentenceDetector = null;

        public static string[] NameTypes = new string[] { "person", "organization", "location", "date", "time", "percentage", "money" };

        private static string s_tokenPath = @"D:\SharpNLP\Models\EnglishTok.nbin";
        private static string s_posPath = @"D:\SharpNLP\Models\EnglishPOS.nbin";
        private static string s_chunkPath = @"D:\SharpNLP\Models\EnglishChunk.nbin";
        private static string s_NEPath = @"D:\SharpNLP\Models\NameFind\";
        private static string s_modelPath = @"D:\SharpNLP\Models\";
        private static string s_sentencePath = @"D:\SharpNLP\Models\EnglishSD.nbin";

        public static string[] SentenceSeg(string  s)
        {
            if (s_sentenceDetector == null)
                s_sentenceDetector = new EnglishMaximumEntropySentenceDetector(s_sentencePath);

            return s_sentenceDetector.SentenceDetect(s);
        }

        public static string[] Tokenize(string s)
        {    
            if (s_tokenizer == null)
                s_tokenizer = new EnglishMaximumEntropyTokenizer(s_tokenPath);
            
            
            return s_tokenizer.Tokenize(s);
        }


        public static string[] PosTagging(string[] s)
        {
            if (s_postagger == null)
                s_postagger = new EnglishMaximumEntropyPosTagger(s_posPath);

            return s_postagger.Tag(s);
        }

        public static Parse Parsing(string s)
        {
            if (s_parser == null)
                s_parser = new EnglishTreebankParser(s_modelPath,true,false);

            return s_parser.DoParse(s);
        }

        public static string PosTaggingSentence(string s)
        {
            if (s_postagger == null)
                s_postagger = new EnglishMaximumEntropyPosTagger(s_posPath);
            return s_postagger.TagSentence(s);
        }

        public static string[] Chunking(string s)
        {
          
            string[] ts = Tokenize(s);

            return Chunking(ts);
        }

        public static string[] Chunking(string[] s)
        {
            if (s_chunker == null)
                s_chunker = new EnglishTreebankChunker(s_chunkPath);

      
            return s_chunker.Chunk(s, PosTagging(s));
        }

        public static string NER(string s)
        {
            if (s_NER == null)
                s_NER = new EnglishNameFinder(s_NEPath);


            return s_NER.GetNames(NameTypes,Parsing(s));
        }
    }
}
