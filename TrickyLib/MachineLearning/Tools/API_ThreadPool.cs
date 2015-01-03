using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Threading;
using TrickyLib.Reflection;
using System.IO;
using TrickyLib.IO;
using System.Text.RegularExpressions;
using TrickyLib.Extension;
using TrickyLib.MachineLearning.Tools.SvmLight;
using TrickyLib.MachineLearning.Tools.TLC;

namespace TrickyLib.MachineLearning.Tools
{
    [Serializable]
    public class API_ThreadPool : BaseThreadPool
    {
        public bool NeedToCleanTempFiles
        {
            get
            {
                return this.Items.Count > 1 || Mode == RunMode.CrossValidation;
            }
        }
        public string PerformanceFile
        {
            get
            {
                if (Mode == RunMode.CrossValidation)
                    return FilePath.ChangeExtension(ExpSetting.TrainFile, "CrossValidation.prf");
                else if (Mode == RunMode.Train)
                    return FilePath.ChangeExtension(ExpSetting.TrainFile, "NormalValidation.prf");
                else
                    return "";
            }
        }
        private BaseExperimentSetting _expSetting;
        public BaseExperimentSetting ExpSetting
        {
            get
            {
                return _expSetting;
            }
            set
            {
                _expSetting = value;
                Mode = value.Mode;
                Metric = value.Metric;
                ParameterSetting = value.ParameterSetting;
                Task = value.Task;
                FolderCount = value.CrossValidationFolder;
            }
        }
        public RunMode Mode { get; private set; }
        public MetricSetting Metric { get; private set; }
        public BaseParameterSetting ParameterSetting { get; private set; }
        public BaseAPI ToolAPI { get; protected set; }
        public MachineLearningTask Task { get; protected set; }
        public int FolderCount { get; protected set; }
        private List<TrainTestResult> _existPerformances;

        public API_ThreadPool(BaseExperimentSetting expSetting, Type toolAPI_Type)
            : base(expSetting.Mode == RunMode.CrossValidation || expSetting.Mode == RunMode.Train ? expSetting.ThreadCount : 1)
        {
            if (!toolAPI_Type.IsSubclassOf(typeof(BaseAPI)))
            {
                throw new ArgumentException("toolAPI_Type should be the type of BaseAPI");
            }
            ToolAPI = Activator.CreateInstance(toolAPI_Type) as BaseAPI;
            ExpSetting = expSetting;
        }

        public void Run()
        {
            ThreadCount = ExpSetting.ThreadCount;

            if (Items == null)
                Items = new List<object>();
            else
                Items.Clear();

            HashSet<string> existArguments = new HashSet<string>();

            if (File.Exists(ExpSetting.PerformanceFile))
            {
                _existPerformances = ReadPerformanceFile(ExpSetting.PerformanceFile).ToList();
                existArguments.AddRange(_existPerformances.Select(r => r.Argu));
            }

            foreach (var paraCom in ParameterSetting.GetParameterCombinations(ExpSetting.Task, ExpSetting.Learner))
                if(!existArguments.Contains(paraCom.GetParameterString()))
                    Items.Add(paraCom);

            StartWork(true, 0);
        }

        protected override void FinishWork(bool isPause)
        {
            base.FinishWork(isPause);

            //add the existed performance
            if (_existPerformances != null)
                this.Results.AddRange(_existPerformances.Select(p => new KeyValuePair<int, object>(-1, p)));

            if (this.Results.Count > 0)
            {
                if (!string.IsNullOrEmpty(PerformanceFile))
                {
                    var headers = ((TrainTestResult)this.Results[0].Value).GetPropertyNames();
                    using (StreamWriter sw = new StreamWriter(PerformanceFile))
                    {
                        sw.WriteLine(headers.ToRowString());
                        foreach (var result in this.Results.Select(kv => kv.Value as TrainTestResult))
                            sw.WriteLine(result.ToString());
                    }
                }
            }

            if (!isPause && !this.IsRunning && NeedToCleanTempFiles)
                CleanTempFiles();
        }

        public void CleanTempFiles()
        {
            List<string> directoryNames = new List<string>();

            if (Mode == RunMode.CrossValidation && !string.IsNullOrEmpty(ExpSetting.TrainFile))
                directoryNames.Add(Path.GetDirectoryName(ExpSetting.TrainFile));

            if ((Mode == RunMode.Train || Mode == RunMode.Test) && !string.IsNullOrEmpty(ExpSetting.ValidationFile))
                directoryNames.Add(Path.GetDirectoryName(ExpSetting.ValidationFile));

            if ((Mode == RunMode.Train || Mode == RunMode.Test) && !string.IsNullOrEmpty(ExpSetting.TestFiles))
                directoryNames.Add(Path.GetDirectoryName(ExpSetting.TestFiles));

            HashSet<string> directories = new HashSet<string>(directoryNames);
            HashSet<string> guids = new HashSet<string>();

            foreach (var thread in Threads)
                if (!guids.Contains(thread.ThreadGUID))
                    guids.Add(thread.ThreadGUID);

            foreach (var directory in directories)
            {
                if (Directory.Exists(directory))
                {
                    foreach (var file in Directory.GetFiles(directory))
                    {
                        foreach (var guid in guids)
                        {
                            if(file.ToLower().Contains(guid.ToLower()))
                            {
                                try
                                {
                                    File.Delete(file);
                                }
                                catch (Exception ex)
                                {

                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        protected override Threading.BaseThread CreateBaseThread(object[] items)
        {
            return new ToolAPI_Thread(this, items);
        }

        public static IEnumerable<TrainTestResult> ReadPerformanceFile(string file)
        {
            string[] headers = FileReader.ReadRow(file, 0).Split('\t');
            var propertyNames = headers.GetRange(2).ToArray();

            List<TrainTestResult> performances = new List<TrainTestResult>();
            foreach (var performanceLine in FileReader.ReadRows(file, 1))
            {
                var items = performanceLine.Split('\t');
                var argument = items[0];
                var folderCount = int.Parse(items[1]);
                var properties = items.GetRange(2).Select(i => double.Parse(i)).ToArray();

                TrainTestResult result = new TrainTestResult(propertyNames, properties) { Argu = argument, FolderCount = folderCount };
                performances.Add(result);
            }

            return performances;
        }
    }
}
