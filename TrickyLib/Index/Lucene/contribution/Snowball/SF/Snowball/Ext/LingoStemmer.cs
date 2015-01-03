using MSRA.NLC.Lingo.NLP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SnowballProgram = SF.Snowball.SnowballProgram;

namespace SF.Snowball.Ext
{
    public class LingoStemmer : SnowballProgram
    {
        private static BasicWordStemmer _lingoWordStemmer;
        public string WordNetFilePath { get; set; }
        public LingoStemmer()
        {
            
        }

        public override bool Stem()
        {
            if (_lingoWordStemmer == null)
            {
                if (!File.Exists(WordNetFilePath))
                    throw new FileNotFoundException(WordNetFilePath);

                _lingoWordStemmer = new BasicWordStemmer();
                _lingoWordStemmer.Init(WordNetFilePath);
            }

            try
            {
                var stemmedWord = _lingoWordStemmer.GetBaseForm(current.ToString());
                current.Remove(0, current.Length);
                current.Append(stemmedWord);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
