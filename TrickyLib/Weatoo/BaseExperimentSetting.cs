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

namespace TrickyLib.MachineLearning.Tools
{
    class BaseExperimentSetting
    {
        public BaseExperimentSetting()
        {
            _parameterSettingDic = new Dictionary<string, BaseParameterSetting>();

            //Initialize all the ParameterSettings
            SvmLightParameterSetting svmLightParaSet = new SvmLightParameterSetting();
            TlcParameterSetting bingFastRankParaSet = new TlcParameterSetting();

            //Initialize all the ToolAPI
            SvmLightAPI svmlightAPI = new SvmLightAPI();
            TlcAPI tlcAPI = new TlcAPI();

            //Classification, SvmLight
            _parameterSettingDic.Add(MachineLearningTask.BinaryClassification + _task_learner_connector + Utility.BinaryClassificationLearners[0], svmLightParaSet);
            _machineLearningToolDic.Add(MachineLearningTask.BinaryClassification + _task_learner_connector + Utility.BinaryClassificationLearners[0], svmlightAPI);

            //Ranking, SvmLight RankSvm
            _parameterSettingDic.Add(MachineLearningTask.Ranking + _task_learner_connector + Utility.RankLeaners[0], svmLightParaSet);
            _machineLearningToolDic.Add(MachineLearningTask.Ranking + _task_learner_connector + Utility.RankLeaners[0], svmlightAPI);

            //Ranking, SvmLight RankSvm Fast
            _parameterSettingDic.Add(MachineLearningTask.Ranking + _task_learner_connector + Utility.RankLeaners[1], svmLightParaSet);
            _machineLearningToolDic.Add(MachineLearningTask.Ranking + _task_learner_connector + Utility.RankLeaners[1], svmlightAPI);

            //Ranking, Bing FastRank
            _parameterSettingDic.Add(MachineLearningTask.Ranking + _task_learner_connector + Utility.RankLeaners[2], bingFastRankParaSet);
            _machineLearningToolDic.Add(MachineLearningTask.Ranking + _task_learner_connector + Utility.RankLeaners[2], tlcAPI);
        }

        #region Task Setting

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
            }
        }

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
                if (_task != value)
                {
                    _task = value;
                    if (value == MachineLearningTask.BinaryClassification)
                        _learners = Utility.BinaryClassificationLearners;
                    else if (value == MachineLearningTask.MultiClassification)
                        _learners = Utility.MultiClassificationLeaners;
                    else if (value == MachineLearningTask.Ranking)
                        _learners = Utility.RankLeaners;
                    else if (value == MachineLearningTask.Regression)
                        _learners = Utility.RegressionLeaners;
                    else
                        throw new NotImplementedException();

                    if (_learners != null && _learners.Length > 0)
                        Learner = _learners[0];

                    if (PropertyVisiblilityChanged != null)
                        PropertyVisiblilityChanged(this, null);
                }
                else
                {
                    _task = value;
                }
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
                ParameterSetting = _parameterSettingDic[Task + _task_learner_connector + value];
                MachineLearningTool = _machineLearningToolDic[Task + _task_learner_connector + value];
            }
        }

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
        [Browsable(true)]
        [Category("2: Experiment Option"), Description("The exe path of the corresponding learning method"), Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string ExePath { get; set; }

        private bool _useThread = true;
        [Browsable(true)]
        [Category("2: Experiment Option"), Description("Use thread or not for training")]
        public bool UseThread
        {
            get
            {
                return _useThread;
            }
            set
            {
                _useThread = value;
                SetPropertyVisibility(this, "ThreadCount", value);
            }
        }

        private int _threadCount = 5;
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
                _threadCount = 5;
            }
        }
        #endregion

        #region Dataset Option
        private string _trainFile = "Unspecified";
        [Browsable(true)]
        [Category("3: Dataset Option"), Description("The training dataset"), Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string TrainFile
        {
            get
            {
                return _trainFile;
            }
            set
            {
                _trainFile = value;
            }
        }

        private int _crossValidationFolder = 4;
        [Browsable(true)]
        [Category("3: Dataset Option")]
        public int CrossValidationFolder
        {
            get
            {
                return _crossValidationFolder;
            }
            set
            {
                _crossValidationFolder = value;
            }
        }

        private string _validationFile = "Unspecified";
        [Browsable(true)]
        [Category("3: Dataset Option"), Description("The training dataset"), Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string ValidationFile
        {
            get
            {
                return _validationFile;
            }
            set
            {
                _validationFile = value;
            }
        }

        private string _testFile = "Unspecified";
        [Browsable(true)]
        [Category("3: Dataset Option"), Description("The training dataset"), Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public string TestFile
        {
            get
            {
                return _testFile;
            }
            set
            {
                _testFile = value;
            }
        }

        #endregion

        #region ParameterSetting

        public delegate void ParameterSettingChangedHander(object sender, EventArgs e);
        public event ParameterSettingChangedHander ParameterSettingChanged;

        private const string _task_learner_connector = " ### ";
        private BaseParameterSetting _parameterSetting;
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
                value.ChangeVisibility(Task, Learner);

                if (ParameterSettingChanged != null)
                    ParameterSettingChanged(this, null);
            }
        }
        protected Dictionary<string, BaseParameterSetting> _parameterSettingDic;

        #endregion

        #region ToolAPI Setting
        protected Dictionary<string, BaseMachineLearningToolAPI> _machineLearningToolDic;

        [Browsable(false)]
        public BaseMachineLearningToolAPI MachineLearningTool { get; set; }
        #endregion

        #region Assistant methods

        public delegate void PropertyVisiblilityChangedHander(object sender, EventArgs e);
        public event PropertyVisiblilityChangedHander PropertyVisiblilityChanged;
        public void SetPropertyVisibility(object obj, string propertyName, bool visible)
        {
            try
            {
                Type type = typeof(BrowsableAttribute);
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
                AttributeCollection attrs = props[propertyName].Attributes;
                FieldInfo fld = type.GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);
                fld.SetValue(attrs[type], visible);

                if (PropertyVisiblilityChanged != null)
                    PropertyVisiblilityChanged(this, null);
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
    }
}
