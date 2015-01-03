using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Reflection.Emit;
using TrickyLib.MachineLearning.Tools.SvmLight;
using TrickyLib.MachineLearning.Tools.TLC;
using System.Threading;
using TrickyLib.Extension;
using TrickyLib.IO;
using TrickyLib.Threading;
using TrickyLib.CustomControl.PropertyGrid.FileBrowser;

namespace TrickyLib.MachineLearning.Tools
{
    [Serializable]
    public class BaseExperimentSetting
    {
        private const string _textFileFilterString = @"Txt File|*.txt|All Files|*.*";
        private const string _featureFileFilterString = @"Feature File|*.feature|BingFastRank File|*.bingFastRank|All Files|*.*";
        private const string _modelFileFilterString = @"Model File|*.model|All Files|*.*";
        private const string _performanceFileString = @"Performance File|*.prf|All Files|*.*";

        [field: NonSerialized]
        private event ExpPropertyChangedHandler _propertyChanged;
        public event ExpPropertyChangedHandler PropertyChanged
        {
            add
            {
                if (_propertyChanged == null || !_propertyChanged.GetInvocationList().Contains(value))
                    _propertyChanged += value;
            }
            remove
            {
                _propertyChanged -= value;
            }
        }
        public delegate void ExpPropertyChangedHandler(BaseExperimentSetting expSetting);

        [field: NonSerialized]
        private event BaseThreadPool.ThreadsFinishedHandler _experimentFinished;
        public event BaseThreadPool.ThreadsFinishedHandler ExperimentFinished
        {
            add
            {
                if (_experimentFinished == null || !_experimentFinished.GetInvocationList().Contains(value))
                    _experimentFinished += value;
            }
            remove
            {
                _experimentFinished -= value;
            }
        }

        public BaseExperimentSetting()
        {
            _parameterSettingDic = new Dictionary<string, BaseParameterSetting>();
            _toolAPI_TypeDic = new Dictionary<string, Type>();

            //Initialize all the ParameterSettings
            SvmLightParameterSetting svmLightParaSet = new SvmLightParameterSetting();
            TlcParameterSetting bingFastRankParaSet = new TlcParameterSetting();

            //Classification, SvmLight
            _parameterSettingDic.Add(MachineLearningTask.BinaryClassification + _task_learner_connector + Utility.BinaryClassificationLearners[0], svmLightParaSet);
            _toolAPI_TypeDic.Add(MachineLearningTask.BinaryClassification + _task_learner_connector + Utility.BinaryClassificationLearners[0], typeof(SvmLightAPI));

            //Ranking, SvmLight RankSvm
            _parameterSettingDic.Add(MachineLearningTask.Ranking + _task_learner_connector + Utility.RankLeaners[0], svmLightParaSet);
            _toolAPI_TypeDic.Add(MachineLearningTask.Ranking + _task_learner_connector + Utility.RankLeaners[0], typeof(SvmLightAPI));

            //Ranking, SvmLight RankSvm Fast
            _parameterSettingDic.Add(MachineLearningTask.Ranking + _task_learner_connector + Utility.RankLeaners[1], svmLightParaSet);
            _toolAPI_TypeDic.Add(MachineLearningTask.Ranking + _task_learner_connector + Utility.RankLeaners[1], typeof(SvmLightAPI));

            //Ranking, Bing FastRank
            _parameterSettingDic.Add(MachineLearningTask.Ranking + _task_learner_connector + Utility.RankLeaners[2], bingFastRankParaSet);
            _toolAPI_TypeDic.Add(MachineLearningTask.Ranking + _task_learner_connector + Utility.RankLeaners[2], typeof(TlcAPI));
        }

        #region Task Setting

        //Property: Mode
        private RunMode _mode = RunMode.Train;
        [Browsable(true)]
        [Category("1: Task Setting")]
        public RunMode Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
                SetApparenceFromMode();
            }
        }

        //Property: CrossValidationFolder
        private int _crossValidationFolder = 4;
        [Browsable(true)]
        [Category("1: Task Setting")]
        public int CrossValidationFolder
        {
            get
            {
                return _crossValidationFolder;
            }
            set
            {
                if (value < 2 || value > 20)
                    MessageBox.Show("Cross folder count out of range. It must between [2, 20]");
                else
                    _crossValidationFolder = value;

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }

        //Property: Task
        private MachineLearningTask _task = MachineLearningTask.BinaryClassification;
        [Browsable(true)]
        [Category("1: Task Setting")]
        public MachineLearningTask Task
        {
            get
            {
                return _task;
            }
            set
            {
                _task = value;
                SetApparenceFromTask();
            }
        }

        private static string[] __learners;
        protected static string[] _learners
        {
            get
            {
                return __learners;
            }
            set
            {
                __learners = value;
            }
        }

        //Property: Learner
        private string _learner = "";
        [Browsable(true)]
        [TypeConverter(typeof(LearnerConverter))]
        [Category("1: Task Setting")]
        public string Learner
        {
            get
            {
                return _learner;
            }
            set
            {
                _learner = value;
                SetApparenceFromLeaner();
            }
        }

        public bool InInitialize;

        class LearnerConverter : TypeConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                StandardValuesCollection cols = new StandardValuesCollection(__learners);
                return cols;
            }
        }

        #endregion

        #region Experiment Option

        //Property: ExePath
        [Browsable(true)]
        [Category("2: Experiment Option"), Description("The exe path of the corresponding learning method"), Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string TrainExePath
        {
            get
            {
                return _trainExePath;
            }
            set
            {
                _trainExePath = value;

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }
        private string _trainExePath;

        //Property: ExePath
        [Browsable(true)]
        [Category("2: Experiment Option"), Description("The exe path of the corresponding learning method"), Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string ClassifyExePath
        {
            get
            {
                return _classifyExePath;
            }
            set
            {
                _classifyExePath = value;

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }
        private string _classifyExePath;

        //Property: ThreadCount
        private int _threadCount = 1;
        [Browsable(true)]
        [Category("2: Experiment Option"), Description("The thread count for multi-thread training")]
        public int ThreadCount
        {
            get
            {
                return _threadCount;
            }
            set
            {
                if (value < 1 || value > Environment.ProcessorCount)
                    MessageBox.Show(string.Format("ThreadCount out of range, it must between [1, {0}]", Environment.ProcessorCount));
                else
                    _threadCount = value;

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }
        #endregion

        #region Dataset Option
        private bool _IsGenerate = false;
        [Browsable(true)]
        [Category("3: Dataset Option"), Description("The training dataset")]
        public bool IsGenerate
        {
            get
            {
                return _IsGenerate;
            }
            set
            {
                _IsGenerate = value;
                SetApparenceFromIsGenerate();
            }
        }

        //Property TrainRawFile
        [Browsable(true)]
        [Category("3: Dataset Option"), Description("The training dataset"), Editor(typeof(CustomFileNameEditor), typeof(UITypeEditor)), Filter(_textFileFilterString)]
        public string TrainRawFile
        {
            get
            {
                return _trainRawFile;
            }
            set
            {
                _trainRawFile = value;

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }
        private string _trainRawFile = "";

        //Property TrainFile
        [Browsable(true)]
        [Category("3: Dataset Option"), Description("The training dataset"), Editor(typeof(CustomFileNameEditor), typeof(UITypeEditor)), Filter(_featureFileFilterString)]
        public string TrainFile
        {
            get
            {
                return _trainFile;
            }
            set
            {
                _trainFile = value;

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }
        private string _trainFile = "";

        //Property ValidationRawFile
        [Browsable(true)]
        [Category("3: Dataset Option"), Description("The training dataset"), Editor(typeof(CustomFileNameEditor), typeof(UITypeEditor)), Filter(_textFileFilterString)]
        public string ValidationRawFile
        {
            get
            {
                return _validationRawFile;
            }
            set
            {
                _validationRawFile = value;

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }
        private string _validationRawFile = "";

        //Property ValidationFile
        private string _validationFile = "";
        [Browsable(true)]
        [Category("3: Dataset Option"), Description("The training dataset"), Editor(typeof(CustomFileNameEditor), typeof(UITypeEditor)), Filter(_featureFileFilterString)]
        public string ValidationFile
        {
            get
            {
                return _validationFile;
            }
            set
            {
                _validationFile = value;

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }

        //Property TestFiles
        [Browsable(true)]
        [Category("3: Dataset Option"), Description("The training dataset"), Editor(typeof(CustomFileNameEditor), typeof(UITypeEditor)), Filter(_textFileFilterString), MultiSelect(true)]
        public string TestRawFiles
        {
            get
            {
                return _testRawFiles;
            }
            set
            {
                _testRawFiles = value;

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }
        private string _testRawFiles = "";

        //Property TestFiles
        [Browsable(true)]
        [Category("3: Dataset Option"), Description("The training dataset"), Editor(typeof(CustomFileNameEditor), typeof(UITypeEditor)), Filter(_featureFileFilterString), MultiSelect(true)]
        public string TestFiles
        {
            get
            {
                return _testFiles;
            }
            set
            {
                _testFiles = value;

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }
        private string _testFiles = "";

        [Browsable(true)]
        [Category("3: Dataset Option"), Description("The training dataset"), Editor(typeof(CustomFileNameEditor), typeof(UITypeEditor)), Filter(_modelFileFilterString)]
        public string ModelFile
        {
            get
            {
                return _modelFile;
            }
            set
            {
                _modelFile = value;

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }
        private string _modelFile = "";

        //Property AdditionalFiles
        [Browsable(true)]
        [Category("3: Dataset Option"), Description("The training dataset"), Editor(typeof(CustomFileNameEditor), typeof(UITypeEditor)), MultiSelect(true)]
        public string AdditionalFiles
        {
            get
            {
                return _additionalFiles;
            }
            set
            {
                _additionalFiles = value;

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }
        private string _additionalFiles = "";

        [Browsable(true)]
        [Category("3: Dataset Option"), Description("The training dataset"), Editor(typeof(CustomFileNameEditor), typeof(UITypeEditor)), Filter(_performanceFileString)]
        public string PerformanceFile
        {
            get
            {
                return _performanceFile;
            }
            set
            {
                _performanceFile = value;

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }
        private string _performanceFile = "";

        [field: NonSerialized]
        private event TransferFeatureFileFormatFinishedHandler _transferFeatureFileFormatFinished;
        public event TransferFeatureFileFormatFinishedHandler TransferFeatureFileFormatFinished
        {
            add
            {
                if (_transferFeatureFileFormatFinished == null || !_transferFeatureFileFormatFinished.GetInvocationList().Contains(value))
                    _transferFeatureFileFormatFinished += value;
            }
            remove
            {
                _transferFeatureFileFormatFinished -= value;
            }
        }
        public delegate void TransferFeatureFileFormatFinishedHandler(BaseExperimentSetting sender);

        public void TransferFeatureFileFormat()
        {
            ReceiveMessage("******************");
            ReceiveMessage("Transfering data format...");

            var targetType = Utility.GetFeatureFileFormat(Task, Learner);
            int dimentionCount = -1;
            if (CheckAllTargetFormatFileExists())
                dimentionCount = GetDimentionCountForAll();

            if (Mode == RunMode.CrossValidation || Mode == RunMode.Train)
            {
                if (File.Exists(TrainFile))
                {
                    var sourceType = FeatureFileProcessor.GetFeatureFileFormat(TrainFile);
                    if (sourceType != targetType)
                    {
                        TrainRawFile = TrainFile;
                        TrainFile = FeatureFileProcessor.TransferFeatureFileFormat(TrainFile, sourceType, targetType, false, dimentionCount);
                    }
                }
            }

            if (Mode == RunMode.Train || Mode == RunMode.Test)
            {
                if (File.Exists(ValidationFile))
                {
                    var sourceType = FeatureFileProcessor.GetFeatureFileFormat(ValidationFile);

                    if (sourceType != targetType)
                    {
                        ValidationRawFile = ValidationFile;
                        ValidationFile = FeatureFileProcessor.TransferFeatureFileFormat(ValidationFile, sourceType, targetType, false, dimentionCount);
                    }
                }

                if (TestFiles != string.Empty)
                {
                    List<string> testFilesTransfered = new List<string>();
                    List<string> testFilesValid = new List<string>();

                    foreach (var testFile in TestFiles.Split(';'))
                    {
                        var sourceType = FeatureFileProcessor.GetFeatureFileFormat(testFile);
                        if (sourceType != targetType)
                        {
                            string testFileTransfered = FeatureFileProcessor.TransferFeatureFileFormat(testFile, FeatureFileFormat.SvmLight, FeatureFileFormat.BingFastRank, false, dimentionCount);
                            testFilesTransfered.Add(testFileTransfered);
                            testFilesValid.Add(testFile);
                        }
                    }

                    if (testFilesTransfered.Count > 0)
                    {
                        TestRawFiles = testFilesValid.ConnectWords(";");
                        TestFiles = testFilesTransfered.ConnectWords(";");
                    }
                }
            }

            ReceiveMessage("Finished transfering data format");
            ReceiveMessage("******************");

            if (_transferFeatureFileFormatFinished != null)
                _transferFeatureFileFormatFinished(this);
        }
        private int GetDimentionCountForAll()
        {
            var targetType = Utility.GetFeatureFileFormat(Task, Learner);
            int dimentionCount = -1;

            if (Mode == RunMode.CrossValidation || Mode == RunMode.Train)
            {
                if (File.Exists(TrainFile))
                {
                    var sourceType = FeatureFileProcessor.GetFeatureFileFormat(TrainFile);
                    if (sourceType != targetType && targetType == FeatureFileFormat.BingFastRank)
                    {
                        int trainDimentionCount = FeatureFileProcessor.GetDimentionCount(TrainFile);
                        if (dimentionCount < trainDimentionCount)
                            dimentionCount = trainDimentionCount;
                    }
                }
            }

            if (Mode == RunMode.Train || Mode == RunMode.Test)
            {
                if (File.Exists(ValidationFile))
                {
                    var sourceType = FeatureFileProcessor.GetFeatureFileFormat(ValidationFile);

                    if (sourceType != targetType && targetType == FeatureFileFormat.BingFastRank)
                    {
                        int validationDimentionCount = FeatureFileProcessor.GetDimentionCount(ValidationFile);
                        if (dimentionCount < validationDimentionCount)
                            dimentionCount = validationDimentionCount;
                    }
                }

                if (TestFiles != string.Empty)
                {
                    List<string> testFilesTransfered = new List<string>();
                    List<string> testFilesValid = new List<string>();

                    foreach (var testFile in TestFiles.Split(';'))
                    {
                        var sourceType = FeatureFileProcessor.GetFeatureFileFormat(testFile);
                        if (sourceType != targetType && targetType == FeatureFileFormat.BingFastRank)
                        {
                            int testDimentionCount = FeatureFileProcessor.GetDimentionCount(testFile);
                            if (dimentionCount < testDimentionCount)
                                dimentionCount = testDimentionCount;
                        }
                    }
                }
            }

            return dimentionCount;
        }
        public bool CheckWhetherNeedToTransfer()
        {
            var targetType = Utility.GetFeatureFileFormat(Task, Learner);

            if (Mode == RunMode.CrossValidation || Mode == RunMode.Train)
            {
                if (File.Exists(TrainFile))
                {
                    var sourceType = FeatureFileProcessor.GetFeatureFileFormat(TrainFile);
                    if (sourceType != targetType)
                        return true;
                }
            }

            if (Mode == RunMode.Train || Mode == RunMode.Test)
            {
                if (File.Exists(ValidationFile))
                {
                    var sourceType = FeatureFileProcessor.GetFeatureFileFormat(ValidationFile);
                    if (sourceType != targetType)
                        return true;
                }

                if (TestFiles != string.Empty)
                {
                    foreach (var testFile in TestFiles.Split(';'))
                    {
                        var sourceType = FeatureFileProcessor.GetFeatureFileFormat(testFile);
                        if (sourceType != targetType)
                            return true;
                    }
                }
            }

            return false;
        }
        private bool CheckAllTargetFormatFileExists()
        {
            var targetType = Utility.GetFeatureFileFormat(Task, Learner);
            if (Mode == RunMode.CrossValidation || Mode == RunMode.Train)
            {
                if (File.Exists(TrainFile))
                {
                    var sourceType = FeatureFileProcessor.GetFeatureFileFormat(TrainFile);
                    if (sourceType != targetType)
                    {
                        string targetFile = FilePath.ChangeExtension(TrainFile, targetType.ToString());
                        if (!File.Exists(targetFile))
                            return true;
                    }
                }
            }

            if (Mode == RunMode.Train || Mode == RunMode.Test)
            {
                if (File.Exists(ValidationFile))
                {
                    var sourceType = FeatureFileProcessor.GetFeatureFileFormat(ValidationFile);

                    if (sourceType != targetType)
                    {
                        string targetFile = FilePath.ChangeExtension(ValidationFile, targetType.ToString());
                        if (!File.Exists(targetFile))
                            return true;
                    }
                }

                if (TestFiles != string.Empty)
                {
                    foreach (var testFile in TestFiles.Split(';'))
                    {
                        var sourceType = FeatureFileProcessor.GetFeatureFileFormat(testFile);
                        if (sourceType != targetType)
                        {
                            string targetFile = FilePath.ChangeExtension(testFile, targetType.ToString());
                            if (!File.Exists(targetFile))
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region ParameterSetting
        private const string _task_learner_connector = " ### ";
        [Browsable(false)]
        public BaseParameterSetting ParameterSetting
        {
            get
            {
                return _parameterSetting;
            }
            set
            {
                _parameterSetting = value;
                ParameterSetting.ChangeVisibility(Task, Learner);

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }
        private BaseParameterSetting _parameterSetting;

        protected Dictionary<string, BaseParameterSetting> _parameterSettingDic;

        #endregion

        #region MetricSetting

        [Browsable(false)]
        public MetricSetting Metric
        {
            get
            {
                return _metric;
            }
            set
            {
                _metric = value;
                value.ChangeVisibility(Task, Learner);

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
        }
        private MetricSetting _metric = new MetricSetting();

        #endregion

        #region ToolAPI Setting
        private Dictionary<string, Type> _toolAPI_TypeDic;

        [Browsable(false)]
        private Type _toolAPI_Type;

        [NonSerialized]
        private API_ThreadPool _tool_ThreadPool_Running;
        [Browsable(false)]
        public API_ThreadPool Tool_ThreadPool_Running
        {
            get
            {
                return _tool_ThreadPool_Running;
            }
            set
            {
                _tool_ThreadPool_Running = value;
            }
        }

        public void RunExperiment()
        {
            if (Tool_ThreadPool_Running == null || !Tool_ThreadPool_Running.IsRunning)
            {
                Tool_ThreadPool_Running = new API_ThreadPool(this, _toolAPI_Type);
                if (_toolAPI_Type.Equals(typeof(SvmLightAPI)))
                {
                    //Initial train exe path
                    if (string.IsNullOrEmpty(TrainExePath))
                        TrainExePath = SvmLightAPI.TrainExePath;
                    else
                        SvmLightAPI.TrainExePath = TrainExePath;

                    //Initial test exe path
                    if (string.IsNullOrEmpty(ClassifyExePath))
                        ClassifyExePath = SvmLightAPI.TestExePath;
                    else
                        SvmLightAPI.TestExePath = ClassifyExePath;

                    //if not exists train exe
                    if (!File.Exists(SvmLightAPI.TrainExePath))
                        throw new FileNotFoundException(SvmLightAPI.TrainExePath);

                    //if not exists test exe
                    if (!File.Exists(SvmLightAPI.TestExePath))
                        throw new FileNotFoundException(SvmLightAPI.TestExePath);
                }
                else if (_toolAPI_Type.Equals(typeof(TlcAPI)))
                {
                    //Initial train exe path
                    if (string.IsNullOrEmpty(TrainExePath))
                        TrainExePath = TlcAPI.ExePath;
                    else
                        TlcAPI.ExePath = TrainExePath;

                    //if not exists train exe
                    if (!File.Exists(TlcAPI.ExePath))
                        throw new FileNotFoundException(TlcAPI.ExePath);
                }

                Tool_ThreadPool_Running.FinishedPercentageChanged += ToolAPI_ThreadPool_FinishedPercentageChanged;
                Tool_ThreadPool_Running.ThreadsFinished += ToolAPI_ThreadPool_ThreadsFinished;
                Tool_ThreadPool_Running.Finished10MorePercentage += Tool_ThreadPool_Running_Finished10MorePercentage;

                ReceiveMessage("******************");
                ReceiveMessage("Experiment started");
                Tool_ThreadPool_Running.Run();
            }
            else
            {
                MessageBox.Show("There is a running job. You can stop it first");
            }
        }

        public void StopExperiment()
        {
            if (Tool_ThreadPool_Running != null && Tool_ThreadPool_Running.IsRunning)
            {
                Tool_ThreadPool_Running.StopWork(true);
                ReceiveMessage("Experiment stopped");
                ReceiveMessage("******************");
            }
        }

        public void PauseExperiment()
        {
            if (Tool_ThreadPool_Running != null && Tool_ThreadPool_Running.IsRunning)
            {
                Tool_ThreadPool_Running.PauseWork(true);
                ReceiveMessage("Experiment paused");
            }
        }

        public void ResumeExperiment()
        {
            if (Tool_ThreadPool_Running != null && Tool_ThreadPool_Running.IsRunning)
            {
                Tool_ThreadPool_Running.ResumeWork();
                ReceiveMessage("Experiment resumed");
            }
        }

        #region Percentage and Text
        [field: NonSerialized]
        private event BaseThreadPool.FinishedPercentageChangedHandler _finishedPercentageChanged;
        public event BaseThreadPool.FinishedPercentageChangedHandler FinishedPercentageChanged
        {
            add
            {
                if (_finishedPercentageChanged == null || !_finishedPercentageChanged.GetInvocationList().Contains(value))
                    _finishedPercentageChanged += value;
            }
            remove
            {
                _finishedPercentageChanged -= value;
            }
        }

        [field: NonSerialized]
        private event BaseThreadPool.Finished10MorePercentageHandler _finished10MorePercentage;
        public event BaseThreadPool.Finished10MorePercentageHandler Finished10MorePercentage
        {
            add
            {
                if (_finished10MorePercentage == null || !_finished10MorePercentage.GetInvocationList().Contains(value))
                    _finished10MorePercentage += value;
            }
            remove
            {
                _finished10MorePercentage -= value;
            }
        }

        [field: NonSerialized]
        private event ProcessingTextChangedHandler _processingTextChanged;
        public event ProcessingTextChangedHandler ProcessingTextChanged
        {
            add
            {
                if (_processingTextChanged == null || !_processingTextChanged.GetInvocationList().Contains(value))
                    _processingTextChanged += value;
            }
            remove
            {
                _processingTextChanged -= value;
            }
        }
        public delegate void ProcessingTextChangedHandler(string text);

        void ToolAPI_ThreadPool_FinishedPercentageChanged(object sender, double percentage)
        {
            if (_finishedPercentageChanged != null)
            {
                API_ThreadPool toolAPI = sender as API_ThreadPool;
                _finishedPercentageChanged(toolAPI, percentage);
            }
        }
        void ToolAPI_ThreadPool_ThreadsFinished(BaseThreadPool sender)
        {
            if (_experimentFinished != null)
            {
                var toolAPI_ThreadPool = (API_ThreadPool)sender;

                //set model file if it is RunMode.Train and the threadcount is only 1
                if (toolAPI_ThreadPool.Threads.Count == 1 && toolAPI_ThreadPool.Mode == RunMode.Train && toolAPI_ThreadPool.Results.Count == 1)
                {
                    var thread = toolAPI_ThreadPool.Threads[0] as ToolAPI_Thread;
                    if (File.Exists(thread.FileSetting.ModelFile))
                        ModelFile = thread.FileSetting.ModelFile;
                }

                _experimentFinished(toolAPI_ThreadPool);

                if (sender.FinishedThreadCount == sender.ThreadCount)
                {
                    ReceiveMessage("Experiment finished");
                    ReceiveMessage("******************");
                }
            }
        }
        void Tool_ThreadPool_Running_Finished10MorePercentage(BaseThreadPool sender)
        {
            if (_finished10MorePercentage != null)
                _finished10MorePercentage(sender);
        }
        public void SetPercentage(double percentage)
        {
            if (_finishedPercentageChanged != null)
                _finishedPercentageChanged(Tool_ThreadPool_Running, percentage);
        }
        public void SetProcessingText(string text)
        {
            if (_processingTextChanged != null)
                _processingTextChanged(text);
        }
        #endregion
        #endregion

        #region Assistant methods
        private void SetPropertyVisibility(object obj, string propertyName, bool visible)
        {
            try
            {
                Type type = typeof(BrowsableAttribute);
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
                AttributeCollection attrs = props[propertyName].Attributes;
                FieldInfo fld = type.GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);
                fld.SetValue(attrs[type], visible);
            }
            catch (Exception ex)
            {
            }
        }

        [field: NonSerialized]
        private static event MessageReceivedHander _messageReceived;
        public static event MessageReceivedHander MessageReceived
        {
            add
            {
                if (_messageReceived == null || !_messageReceived.GetInvocationList().Contains(value))
                {
                    _messageReceived += value;
                }
            }
            remove
            {
                _messageReceived -= value;
            }
        }
        public delegate void MessageReceivedHander(string message);

        public static void ReceiveMessage(string message)
        {
            if (_messageReceived != null)
                _messageReceived(string.Format("[{0}]: {1}", DateTime.Now.ToLocalTime(), message));
        }

        public void ChangeDisk(char disk)
        {
            if (Mode == RunMode.CrossValidation || Mode == RunMode.Train)
            {
                if (!string.IsNullOrEmpty(TrainRawFile.Trim()) && TrainRawFile.Length > 2 && TrainRawFile[1] == ':')
                    TrainRawFile = disk + TrainRawFile.Substring(1);

                if (!string.IsNullOrEmpty(TrainFile.Trim()) && TrainFile.Length > 2 && TrainFile[1] == ':')
                    TrainFile = disk + TrainFile.Substring(1);

                if (!string.IsNullOrEmpty(PerformanceFile.Trim()) && PerformanceFile.Length > 2 && PerformanceFile[1] == ':')
                    PerformanceFile = disk + PerformanceFile.Substring(1);
            }

            if (Mode == RunMode.Train || Mode == RunMode.Test)
            {
                //validation raw file
                if (!string.IsNullOrEmpty(ValidationRawFile.Trim()) && ValidationRawFile.Length > 2 && ValidationRawFile[1] == ':')
                    ValidationRawFile = disk + ValidationRawFile.Substring(1);

                //validation file
                if (!string.IsNullOrEmpty(ValidationFile.Trim()) && ValidationFile.Length > 2 && ValidationFile[1] == ':')
                    ValidationFile = disk + ValidationFile.Substring(1);

                //test raw files
                if (TestRawFiles != string.Empty)
                {
                    List<string> newTestRawFiles = new List<string>();
                    foreach (var testRawFile in TestRawFiles.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(testRawFile.Trim()))
                        {
                            if (testRawFile.Length > 2 && testRawFile[1] == ':')
                                newTestRawFiles.Add(disk + testRawFile.Substring(1));
                            else
                                newTestRawFiles.Add(testRawFile);
                        }
                    }

                    TestRawFiles = newTestRawFiles.ConnectWords(";");
                }

                //test files
                if (TestFiles != string.Empty)
                {
                    List<string> newTestFiles = new List<string>();
                    foreach (var testFile in TestFiles.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(testFile.Trim()))
                        {
                            if (testFile.Length > 2 && testFile[1] == ':')
                                newTestFiles.Add(disk + testFile.Substring(1));
                            else
                                newTestFiles.Add(testFile);
                        }
                    }

                    TestFiles = newTestFiles.ConnectWords(";");
                }
            }

            if (!string.IsNullOrEmpty(ModelFile.Trim()) && ModelFile.Length > 2 && ModelFile[1] == ':')
                ModelFile = disk + ModelFile.Substring(1);
        }
        #endregion

        #region virtual methods

        public void GenerateFeatureFiles()
        {
            if (FeatureFileGenerator == null)
                return;

            if (Mode == RunMode.CrossValidation || Mode == RunMode.Train)
            {
                if (TrainRawFile != string.Empty)
                {
                    if (!File.Exists(TrainRawFile))
                        throw new FileNotFoundException("File does not exists", TrainRawFile);

                    TrainFile = FeatureFileGenerator(TrainRawFile, AdditionalFiles.Split(';'));
                }
            }
            if (Mode != RunMode.CrossValidation)
            {
                if (ValidationRawFile != string.Empty)
                {
                    if (!File.Exists(ValidationRawFile))
                        throw new FileNotFoundException("File does not exists", ValidationRawFile);

                    ValidationFile = FeatureFileGenerator(ValidationRawFile, AdditionalFiles.Split(';'));
                }
            }
            else if (Mode == RunMode.Train || Mode == RunMode.Test)
            {
                if (TestRawFiles != string.Empty)
                {
                    List<string> testFiles = new List<string>();

                    foreach (var testRawFile in TestRawFiles.Split(';'))
                    {
                        if (!File.Exists(testRawFile))
                            throw new FileNotFoundException("File does not exists", testRawFile);

                        testFiles.Add(FeatureFileGenerator(testRawFile, AdditionalFiles.Split(';')));
                    }
                    TestFiles = testFiles.ConnectWords(";");
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        [NonSerialized]
        public GenerateFeatureFileHander FeatureFileGenerator;
        public delegate string GenerateFeatureFileHander(string rawFile, string[] additionalFiles);

        [NonSerialized]
        public BaseAPI.GetCustomizePerformanceHandler CustomizePerformanceGenerator;
        #endregion

        #region Set apparence
        public void SetApparenceFromLeaner()
        {
            try
            {
                ParameterSetting = _parameterSettingDic[Task + _task_learner_connector + Learner];
                _toolAPI_Type = _toolAPI_TypeDic[Task + _task_learner_connector + Learner];
                SetPropertyVisibility(this, "ClassifyExePath", Utility.GetFeatureFileFormat(Task, Learner) != FeatureFileFormat.BingFastRank);

                if (ParameterSetting is SvmLightParameterSetting)
                {
                    TrainExePath = SvmLightAPI.TrainExePath;
                    ClassifyExePath = SvmLightAPI.TestExePath;
                }
                else if (ParameterSetting is TlcParameterSetting)
                {
                    TrainExePath = TlcAPI.ExePath;
                }
                else
                {
                    throw new NotImplementedException();
                }

                if (_propertyChanged != null)
                    _propertyChanged(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("The learner is not implemented currently. Please wait for update.");
            }
        }
        public void SetApparenceFromTask()
        {
            if (Task == MachineLearningTask.BinaryClassification)
                _learners = Utility.BinaryClassificationLearners;
            else if (Task == MachineLearningTask.MultiClassification)
                _learners = Utility.MultiClassificationLeaners;
            else if (Task == MachineLearningTask.Ranking)
                _learners = Utility.RankLeaners;
            else if (Task == MachineLearningTask.Regression)
                _learners = Utility.RegressionLeaners;
            else
                throw new NotImplementedException();

            if (!string.IsNullOrEmpty(_learner) && _learners.Length > 0 && !InInitialize)
                Learner = _learners[0];

            Metric.ChangeVisibility(Task, Learner);
            if (_propertyChanged != null)
                _propertyChanged(this);
        }
        public void SetApparenceFromMode()
        {
            SetPropertyVisibility(this, "CrossValidationFolder", Mode == RunMode.CrossValidation);
            SetPropertyVisibility(this, "ThreadCount", Mode == RunMode.CrossValidation || Mode == RunMode.Train);
            SetPropertyVisibility(this, "TrainFile", Mode == RunMode.CrossValidation || Mode == RunMode.Train);
            SetPropertyVisibility(this, "ValidationFile", Mode != RunMode.CrossValidation);
            SetPropertyVisibility(this, "TestFiles", Mode == RunMode.Train || Mode == RunMode.Test);
            SetPropertyVisibility(this, "TrainRawFile", IsGenerate && (Mode == RunMode.CrossValidation || Mode == RunMode.Train));
            SetPropertyVisibility(this, "ValidationRawFile", IsGenerate && Mode != RunMode.CrossValidation);
            SetPropertyVisibility(this, "TestRawFiles", IsGenerate && (Mode == RunMode.Train || Mode == RunMode.Test));
            SetPropertyVisibility(this, "ModelFile", Mode == RunMode.Train || Mode == RunMode.Test);
            SetPropertyVisibility(this, "PerformanceFile", Mode == RunMode.CrossValidation || Mode == RunMode.Train);

            if (_propertyChanged != null)
                _propertyChanged(this);
        }
        public void SetApparenceFromIsGenerate()
        {
            SetPropertyVisibility(this, "TrainRawFile", IsGenerate && (Mode == RunMode.CrossValidation || Mode == RunMode.Train));
            SetPropertyVisibility(this, "ValidationRawFile", IsGenerate && Mode != RunMode.CrossValidation);
            SetPropertyVisibility(this, "TestRawFiles", IsGenerate && (Mode == RunMode.Train || Mode == RunMode.Test));

            if (_propertyChanged != null)
                _propertyChanged(this);
        }
        #endregion

    }
}
