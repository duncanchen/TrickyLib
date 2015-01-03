using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenNLP.Tools.Tokenize;
using OpenNLP.Tools.SentenceDetect;
using OpenNLP.Tools.Parser;
using OpenNLP.Tools.NameFind;
using OpenNLP.Tools.Chunker;
using OpenNLP.Tools.PosTagger;
using System.IO;
using System.Text.RegularExpressions;
using TrickyLib.Extension;
using SF.Snowball.Ext;

namespace TrickyLib.Parser
{
    public class NLParser
    {
        private EnglishMaximumEntropyTokenizer s_tokenizer = null;
        private EnglishMaximumEntropyPosTagger s_postagger = null;
        private MaximumEntropyChunker s_chunker = null;
        private EnglishNameFinder s_NER = null;
        private EnglishTreebankParser s_parser = null;
        private EnglishMaximumEntropySentenceDetector s_sentenceDetector = null;
        private EnglishStemmer s_stemmer = null;

        public string[] NameTypes = new string[] { "person", "organization", "location", "date", "time", "percentage", "money" };

        private string s_modelPath = @"E:\v-haowu\Program\CSharp\TrickyLib\TrickyLib\Res\SharpNLP\Models";
        private string s_tokenPath;
        private string s_posPath;
        private string s_chunkPath;
        private string s_NEPath;
        private string s_sentencePath;

        public NLParser()
        {
            s_tokenPath = Path.Combine(s_modelPath, "EnglishTok.nbin");
            s_posPath = Path.Combine(s_modelPath, "EnglishPOS.nbin");
            s_chunkPath = Path.Combine(s_modelPath, "EnglishChunk.nbin");
            s_NEPath = Path.Combine(s_modelPath, "NameFind");
            s_sentencePath = Path.Combine(s_modelPath, "EnglishSD.nbin");
        }

        public string[] SentenceSeg(string s)
        {
            try
            {
                if (s_sentenceDetector == null)
                    s_sentenceDetector = new EnglishMaximumEntropySentenceDetector(s_sentencePath);

                return s_sentenceDetector.SentenceDetect(s);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public string[] Tokenize(string s)
        {
            if (s_tokenizer == null)
                s_tokenizer = new EnglishMaximumEntropyTokenizer(s_tokenPath);

            return s_tokenizer.Tokenize(s);
        }

        public string[] PosTagging(string[] s)
        {
            if (s_postagger == null)
                s_postagger = new EnglishMaximumEntropyPosTagger(s_posPath);

            return s_postagger.Tag(s);
        }

        public Parse Parsing(string s)
        {
            if (s_parser == null)
                s_parser = new EnglishTreebankParser(s_modelPath, true, false);

            return s_parser.DoParse(s);
        }

        public string PosTaggingSentence(string s)
        {
            if (s_postagger == null)
                s_postagger = new EnglishMaximumEntropyPosTagger(s_posPath);

            return s_postagger.TagSentence(s);
        }

        public string[] Chunking(string s)
        {
            return Chunking(Tokenize(s));
        }

        public string[] Chunking(string[] s)
        {
            if (s_chunker == null)
                s_chunker = new EnglishTreebankChunker(s_chunkPath);

            return s_chunker.Chunk(s, PosTagging(s));
        }

        public string NER(string s)
        {
            if (s_NER == null)
                s_NER = new EnglishNameFinder(s_NEPath);

            return s_NER.GetNames(NameTypes, Parsing(s));
        }

        public string Stem(string s)
        {
            if (s_stemmer == null)
                s_stemmer = new EnglishStemmer();

            return s_stemmer.GetStemWord(s);
        }
    }
}
