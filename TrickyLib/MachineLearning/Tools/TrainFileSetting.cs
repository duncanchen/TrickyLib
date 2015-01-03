using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.IO;

namespace TrickyLib.MachineLearning.Tools
{
    [Serializable]
    public class TrainFileSetting
    {
        private string _guid = "";
        public string GUID
        {
            get
            {
                return _guid;
            }
            set
            {
                _guid = value;
            }
        }

        public string TrainRawFile { get; set; }
        public string ValidationRawFile { get; set; }
        public string[] TestRawFiles { get; set; }

        public string TrainFile { get; set; }
        public string ValidationFile { get; set; }
        public string[] TestFiles { get; set; }

        public string[] AdditionalFiles { get; set; }

        public string ModelFile
        {
            get
            {
                if (GUID != "" && !RegularExpressions.GuidRegex_Contain.IsMatch(TrainFile))
                    return FilePath.ChangeExtension(FilePath.AddExtention(TrainFile, GUID, 1), "model");
                else
                    return FilePath.ChangeExtension(TrainFile, "model");
            }
        }
        public string ValidationOutputFile
        {
            get
            {
                if (GUID != "" && !RegularExpressions.GuidRegex_Contain.IsMatch(ValidationFile))
                    return FilePath.ChangeExtension(FilePath.AddExtention(ValidationFile, GUID, 1), "output");
                else
                    return FilePath.ChangeExtension(ValidationFile, "output");
            }
        }
        public string[] TestOutputFiles
        {
            get
            {
                return TestFiles.Select(f =>
                        (GUID != "" && !RegularExpressions.GuidRegex_Contain.IsMatch(f)) ?
                        FilePath.ChangeExtension(FilePath.AddExtention(f, GUID, 1), "output")
                        :
                        FilePath.ChangeExtension(f, "output")
                        ).ToArray();
            }
        }
    }
}
