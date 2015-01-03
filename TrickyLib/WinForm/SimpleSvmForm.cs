using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using TrickyLib.Struct;
using TrickyLib.MachineLearning;
using TrickyLib.IO;
using TrickyLib.Threading;
using System.Reflection;
using TrickyLib.Reflection;
using TrickyLib.Extension;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TrickyLib.Service;
using Microsoft.Office.Interop.Excel;
using TrickyLib.WinControl;

namespace TrickyLib.WinForm
{
    public partial class SimpleSvmForm : Form
    {
        public static string ConfigPath = "SimpleSvmForm.LastConfig.txt";

        public string AdditionalFile
        {
            get
            {
                return this.AdditionalFileTextBox.FilePath;
            }
            set
            {
                this.AdditionalFileTextBox.FilePath = value;
            }
        }
        public string RawTrainFile
        {
            get
            {
                return this.RawTrainOpenFileTextBox.FilePath;
            }
            set
            {
                this.RawTrainOpenFileTextBox.FilePath = value;
            }
        }
        public string RawDevFile
        {
            get
            {
                return this.RawDevOpenFileTextBox.FilePath;
            }
            set
            {
                this.RawDevOpenFileTextBox.FilePath = value;
            }
        }
        public string RawTestFile
        {
            get
            {
                return this.RawTestOpenFileTextBox.FilePath;
            }
            set
            {
                this.RawTestOpenFileTextBox.FilePath = value;
            }
        }
        public string FeatureTrainFile
        {
            get
            {
                return this.FeatureTrainOpenFileTextBox.FilePath;
            }
            set
            {
                this.FeatureTrainOpenFileTextBox.FilePath = value;
                this.CrossValidationFile = value;
            }
        }
        public string FeatureDevFile
        {
            get 
            {
                return this.FeatureDevOpenFileTextBox.FilePath;
            }
            set
            {
                this.FeatureDevOpenFileTextBox.FilePath = value;
            }
        }
        public string FeatureTestFile
        {
            get
            {
                return this.FeatureTestOpenFileTextBox.FilePath;
            }
            set
            {
                this.FeatureTestOpenFileTextBox.FilePath = value;
            }
        }
        public string ModelFile
        {
            get
            {
                return this.ModelOpenFileTextBox.FilePath;
            }
            set
            {
                this.ModelOpenFileTextBox.FilePath = value;
            }
        }
        public string CrossValidationFile
        {
            get
            {
                return this.CrossValidationOpenFileTextBox.FilePath;
            }
            set
            {
                this.CrossValidationOpenFileTextBox.FilePath = value;
            }
        }
        public string Argument
        {
            get
            {
                return this.ArguTextBox.Text;
            }
            set
            {
                this.ArguTextBox.Text = value;
            }
        }
        public string ConsoleText
        {
            get
            {
                return this.ConsoleWinTextBox.Text.Replace("\r\n", " ##N## ").Replace("\t", " ##TAB## ");
            }
            set
            {
                this.ConsoleWinTextBox.Text = value.Replace(" ##N## ", "\r\n").Replace(" ##TAB## ", "\t");
            }
        }
        public string CaseQuery
        {
            get
            {
                return this.CaseQueryTextBox.Text;   
            }
            set
            {
                this.CaseQueryTextBox.Text = value;
            }
        }
        public string CaseSet
        {
            get
            {
                return this.CaseSetComboBox.Text;
            }
            set
            {
                this.CaseSetComboBox.Text = value;
            }
        }

        public bool IsDevIdenticalWithTrain
        {
            get
            {
                return this.DevIdenticalCheckBox.Checked;
            }
            set
            {
                this.DevIdenticalCheckBox.Checked = value;
            }
        }
        public bool IsRankSvm
        {
            get
            {
                return this.RankSvmCheckBox.Checked;
            }
            set
            {
                this.RankSvmCheckBox.Checked = value;
            }
        }
        public bool UseFastRankLearn
        {
            get
            {
                return this.FastRankLearnCheckBox.Checked;
            }
            set
            {
                this.FastRankLearnCheckBox.Checked = value;
            }
        }
        public bool IsNormalized
        {
            get
            {
                return this.NormalizeCheckBox.Checked;
            }
            set
            {
                this.NormalizeCheckBox.Checked = value;
            }
        }
        public bool InSeries
        {
            get
            {
                return !this.CrossSeriesButton.Enabled || !this.SeriesTrainButton.Enabled;
            }
            set
            { 
            
            }
        }
        public string ValueOperation
        {
            get
            {
                return this.ValueOperationComboBox.Text;
            }
            set
            {
                this.ValueOperationComboBox.Text = value;
            }
        }

        public decimal MinJ
        {
            get
            {
                return this.MinJNumeric.Value;
            }
            set
            {
                this.MinJNumeric.Value = value;
            }
        }
        public decimal MaxJ
        {
            get
            {
                return this.MaxJNumeric.Value;
            }
            set
            {
                this.MaxJNumeric.Value = value;
            }
        }
        public decimal StepJ
        {
            get
            {
                return this.StepJNumeric.Value;
            }
            set
            {
                this.StepJNumeric.Value = value;
            }
        }
        public decimal MinC
        {
            get
            {
                return this.MinCNumeric.Value;
            }
            set
            {
                this.MinCNumeric.Value = value;
            }
        }
        public decimal MaxC
        {
            get
            {
                return this.MaxCNumeric.Value;
            }
            set
            {
                this.MaxCNumeric.Value = value;
            }
        }
        public decimal StepC
        {
            get
            {
                return this.StepCNumeric.Value;
            }
            set
            {
                this.StepCNumeric.Value = value;
            }
        }
        public bool IsPowerC
        {
            get
            {
                return this.IsPowercCheckBox.Checked;
            }
            set
            {
                this.IsPowercCheckBox.Checked = value;
            }
        }

        public int ThreadCount 
        {
            get
            {
                return Convert.ToInt32(this.ThreadCountNumeric.Value);
            }
            set
            {
                this.ThreadCountNumeric.Value = value;
            }
        }
        public int MaxCrossValidationFolderCount
        {
            get
            {
                return Convert.ToInt32(this.MaxFolderNumeric.Value);
            }
            set
            {
                this.MaxFolderNumeric.Value = value;
            }
        }
        public int MinCrossValidationFolderCount
        {
            get
            {
                return Convert.ToInt32(this.MinFolderNumeric.Value);
            }
            set
            {
                this.MinFolderNumeric.Value = value;
            }
        }
        public int CrossValidationFolderCount
        {
            get
            {
                return Convert.ToInt32(this.FolderCountNumeric.Value);
            }
            set
            {
                this.FolderCountNumeric.Value = value;
            }
        }
        public double ProgressPercentage
        {
            get
            {
                return this.ProgressLBar.ProgressPercentage;
            }
            set
            {
                this.ProgressLBar.ProgressPercentage = value;
            }
        }
        public string ProcessingText
        {
            get
            {
                return this.ProgressLBar.ProcessingText;
            }
            set
            {
                this.ProgressLBar.ProcessingText = value;
            }
        }


        public bool IsSimpleTrainOrTest = false;

        private Train_ThreadPool trainThreadPool;

        public SimpleSvmForm()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;
            this.RawTrainOpenFileTextBox.Filter = "Text file|*.txt|All File|*.*";
            this.RawDevOpenFileTextBox.Filter = "Text file|*.txt|All File|*.*";
            this.RawTestOpenFileTextBox.Filter = "Text file|*.txt|All File|*.*";
            this.FeatureTrainOpenFileTextBox.Filter = "Feature file|*.feature|All File|*.*";
            this.FeatureDevOpenFileTextBox.Filter = "Feature file|*.feature|All File|*.*";
            this.FeatureTestOpenFileTextBox.Filter = "Feature file|*.feature|All File|*.*";
            this.ModelOpenFileTextBox.Filter = "Model file|*.model|All File|*.*";
            this.CrossValidationOpenFileTextBox.Filter = "Feature file|*.feature|All File|*.*";
            this.ConfigTextBox.Filter = "Config file|*.config|All File|*.*";
            this.ConfigTextBox.InitialDirectory = System.Environment.CurrentDirectory;
            this.FeatureTrainOpenFileTextBox.FilePathChanged += new FileBrowserTextBox.FilePathChangedHandler(FeatureTrainOpenFileTextBox_FilePathChanged);
            this.ConfigTextBox.FilePathChanged += new FileBrowserTextBox.FilePathChangedHandler(ConfigTextBox_FilePathChanged);
            this.ProgressLBar.ProgressPercentageChanged += new ProgressLabelBar.ProgressPercentageChangedHandler(ProgressLBar_ProgressPercentageChanged);
            this.Load +=new EventHandler(SimpleSvmForm_Load);
        }

        void ProgressLBar_ProgressPercentageChanged(object sender, string labelText)
        {
            this.Text = Path.GetFileNameWithoutExtension(this.ConfigTextBox.FilePath) + " " + this.ProgressLBar.ProgressPercentageText;
        }

        void ConfigTextBox_FilePathChanged(object sender, EventArgs e)
        {
            this.Text = Path.GetFileNameWithoutExtension(this.ConfigTextBox.FilePath) + " " + this.ProgressLBar.ProgressPercentageText;
        }

        void FeatureTrainOpenFileTextBox_FilePathChanged(object sender, EventArgs e)
        {
            this.CrossValidationFile = this.FeatureTrainFile;
        }

        void trainThreadPool_FinishedItemCountChanged(object sender, EventArgs e)
        {
            if (this.trainThreadPool.Items != null && this.trainThreadPool.Items.Count > 0)
            {
                double percentage = this.trainThreadPool.FinishedItemCount * 1.0 / this.trainThreadPool.Items.Count;
                if (percentage >= 1 && this.trainThreadPool.FinishedThreadCount < this.trainThreadPool.ThreadCount)
                    percentage -= 0.001;
                this.ProgressPercentage = percentage;
            }
            else
            {
                this.ProgressPercentage = 0;
                this.SeriesTrainButton.Enabled = true;
                this.CrossSeriesButton.Enabled = true;
                this.OpenPerformanceButton.Enabled = true;
            }
        }

        private void FeatureIdenticalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.FeatureDevOpenFileTextBox.Enabled = !this.DevIdenticalCheckBox.Checked;
            this.RawDevOpenFileTextBox.Enabled = !this.DevIdenticalCheckBox.Checked;
            this.RawDevProduceButton.Enabled = !this.DevIdenticalCheckBox.Checked && !this.NormalizeCheckBox.Checked;
        }

        private void StartTrainButton_Click(object sender, EventArgs e)
        {
            this.IsSimpleTrainOrTest = true;

            try
            {
                if (!File.Exists(this.FeatureTrainFile))
                    throw new Exception("Train file does not exists!");

                string devFeatureFile = string.Empty;
                if (!this.DevIdenticalCheckBox.Checked)
                {
                    devFeatureFile = this.FeatureDevFile;
                    if (!File.Exists(devFeatureFile))
                        throw new Exception("Develpment file does not exists!");
                }
                else
                    devFeatureFile = this.FeatureTrainFile;

                if (this.IsNormalized)
                {
                    List<string> featureFiles = new List<string>();
                    featureFiles.Add(this.FeatureTrainFile);
                    if (File.Exists(this.FeatureTestFile))
                        featureFiles.Add(this.FeatureTestFile);
                    if (!this.IsDevIdenticalWithTrain && File.Exists(this.FeatureDevFile))
                        featureFiles.Add(this.FeatureDevFile);

                    var minMaxDic = FeatureFileProcessor.GetNormalizeMinMaxDic(featureFiles.ToArray());
                    this.FeatureTrainFile = FeatureFileProcessor.NormalizeFeatureFile(this.FeatureTrainFile, minMaxDic);
                    if (File.Exists(this.FeatureTestFile))
                        this.FeatureTestFile = FeatureFileProcessor.NormalizeFeatureFile(this.FeatureTestFile, minMaxDic);
                    if (!this.IsDevIdenticalWithTrain && File.Exists(this.FeatureDevFile))
                        this.FeatureDevFile = FeatureFileProcessor.NormalizeFeatureFile(this.FeatureDevFile, minMaxDic);

                    this.IsNormalized = false;
                }

                this.Text = Path.GetFileNameWithoutExtension(this.ConfigTextBox.FilePath) + " (Running...)";
                TrainTestResult result = SvmLight.NormalValidation(this.Argument, this.FeatureTrainFile, this.FeatureTrainFile, devFeatureFile, this.CalculatePerformance, this.UseFastRankLearn && this.IsRankSvm);
                this.ModelFile = SvmLight.GetNormalValidationFiles(this.FeatureTrainFile, this.FeatureTrainFile, devFeatureFile, this.UseFastRankLearn && this.IsRankSvm)[2];

                this.ConsoleText = result.ToConsoleString("Development");
                this.Text = Path.GetFileNameWithoutExtension(this.ConfigTextBox.FilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.IsSimpleTrainOrTest = false;
        }

        private void StartTestButton_Click(object sender, EventArgs e)
        {
            this.IsSimpleTrainOrTest = true;
            try
            {
                if (!File.Exists(this.FeatureTestFile))
                    throw new Exception("Test file does not exists");

                if (!File.Exists(this.ModelFile))
                    throw new Exception("Model file does not exists");

                this.Text = Path.GetFileNameWithoutExtension(this.ConfigTextBox.FilePath) + " (Running...)";
                string testOutputFile = SvmLight.GetSvmFileName(FilePath.ChangeFile(FeatureTestFile, FilePath.MergeDistinctFilePathPart(this.ModelFile, FeatureTestFile) + "."), MLFileTypes.Output);
                TrainTestResult testResult = SvmLight.ClassifySVM(this.FeatureTestFile, this.ModelFile, testOutputFile, this.UseFastRankLearn && this.IsRankSvm);
                testResult.AddRange(CalculatePerformance(this.ModelFile, this.FeatureTestFile, testOutputFile, this.RawTestFile));

                this.ConsoleText = testResult.InsertHeader("Test").ToConsoleString("Test");
                this.Text = Path.GetFileNameWithoutExtension(this.ConfigTextBox.FilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.IsSimpleTrainOrTest = false;
        }

        private void StartTest2Button_Click(object sender, EventArgs e)
        {
            this.IsSimpleTrainOrTest = true;
            try
            {
                if (!File.Exists(this.ModelFile))
                    throw new Exception("Model file does not exists");

                string devFeatureFile = this.IsDevIdenticalWithTrain ? this.FeatureTrainFile : this.FeatureDevFile;
                string rawDevFile = this.IsDevIdenticalWithTrain ? this.RawTrainFile : this.RawDevFile;
                string resultHeader = this.IsDevIdenticalWithTrain ? "Train" : "Dev";

                if (!File.Exists(this.FeatureTestFile) || !File.Exists(devFeatureFile))
                    throw new Exception("Dev or Test file does not exists");

                this.Text = Path.GetFileNameWithoutExtension(this.ConfigTextBox.FilePath) + " (Running...)";

                string devOutputFile = SvmLight.GetSvmFileName(FilePath.ChangeFile(devFeatureFile, FilePath.MergeDistinctFilePathPart(this.ModelFile, devFeatureFile) + "."), MLFileTypes.Output);
                TrainTestResult devResult = SvmLight.ClassifySVM(devFeatureFile, this.ModelFile, devOutputFile, this.UseFastRankLearn && this.IsRankSvm);
                devResult.AddRange(CalculatePerformance(this.ModelFile, devFeatureFile, devOutputFile, rawDevFile));

                string testOutputFile = SvmLight.GetSvmFileName(FilePath.ChangeFile(this.FeatureTestFile, FilePath.MergeDistinctFilePathPart(this.ModelFile, this.FeatureTestFile) + "."), MLFileTypes.Output);
                TrainTestResult testResult = SvmLight.ClassifySVM(this.FeatureTestFile, this.ModelFile, testOutputFile, this.UseFastRankLearn && this.IsRankSvm);
                testResult.AddRange(CalculatePerformance(this.ModelFile, this.FeatureTestFile, testOutputFile, this.RawTestFile));

                this.ConsoleText = devResult.InsertHeader(resultHeader).ToConsoleString(resultHeader);
                this.ConsoleText += "\n\n" + testResult.InsertHeader("Test").ToConsoleString("Test");
                this.Text = Path.GetFileNameWithoutExtension(this.ConfigTextBox.FilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.IsSimpleTrainOrTest = false;
        }

        private void StartTrainAndTestButton_Click(object sender, EventArgs e)
        {
            this.IsSimpleTrainOrTest = true;
            try
            {
                if (!File.Exists(this.FeatureTrainFile))
                    throw new Exception("Train file does not exists!");

                string devFeatureFile = string.Empty;
                if (!this.DevIdenticalCheckBox.Checked)
                {
                    devFeatureFile = this.FeatureDevFile;
                    if (!File.Exists(devFeatureFile))
                        throw new Exception("Develpment file does not exists!");
                }
                else
                    devFeatureFile = this.FeatureTrainFile;

                if (this.IsNormalized)
                {
                    List<string> featureFiles = new List<string>();
                    featureFiles.Add(this.FeatureTrainFile);
                    if(File.Exists(this.FeatureTestFile))
                        featureFiles.Add(this.FeatureTestFile);
                    if (!this.IsDevIdenticalWithTrain && File.Exists(this.FeatureDevFile))
                        featureFiles.Add(this.FeatureDevFile);

                    var minMaxDic = FeatureFileProcessor.GetNormalizeMinMaxDic(featureFiles.ToArray());
                    this.FeatureTrainFile = FeatureFileProcessor.NormalizeFeatureFile(this.FeatureTrainFile, minMaxDic);
                    if(File.Exists(this.FeatureTestFile))
                    this.FeatureTestFile = FeatureFileProcessor.NormalizeFeatureFile(this.FeatureTestFile, minMaxDic);
                    if (!this.IsDevIdenticalWithTrain && File.Exists(this.FeatureDevFile))
                        this.FeatureDevFile = FeatureFileProcessor.NormalizeFeatureFile(this.FeatureDevFile, minMaxDic);

                    this.IsNormalized = false;
                }

                this.Text = Path.GetFileNameWithoutExtension(this.ConfigTextBox.FilePath) + " (Running...)";
                TrainTestResult result = SvmLight.NormalValidation(this.Argument, this.FeatureTrainFile, this.FeatureTrainFile, devFeatureFile, this.CalculatePerformance, this.UseFastRankLearn && this.IsRankSvm);
                this.ModelFile = SvmLight.GetNormalValidationFiles(this.FeatureTrainFile, this.FeatureTrainFile, devFeatureFile, this.UseFastRankLearn && this.IsRankSvm)[2];

                string testOutputFile = SvmLight.GetSvmFileName(FilePath.ChangeFile(this.FeatureTestFile, FilePath.MergeDistinctFilePathPart(this.ModelFile, this.FeatureTestFile) + "."), MLFileTypes.Output);
                TrainTestResult testResult = SvmLight.ClassifySVM(this.FeatureTestFile, this.ModelFile, testOutputFile, this.UseFastRankLearn && this.IsRankSvm);
                testResult.AddRange(CalculatePerformance(this.ModelFile, this.FeatureTestFile, testOutputFile, this.RawTestFile));

                this.ConsoleText = result.ToConsoleString("Development");
                this.ConsoleText += "\n\n" + testResult.InsertHeader("Test").ToConsoleString("Test");
                this.Text = Path.GetFileNameWithoutExtension(this.ConfigTextBox.FilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.IsSimpleTrainOrTest = false;
        }

        private void SaveConfigButton_Click(object sender, EventArgs e)
        {
            if (this.ConfigTextBox.FilePath.Trim() == string.Empty)
                MessageBox.Show("ConfigTextBox cannot be empty");
            else
            {
                int inheritLevel = 1;
                Type type = this.GetType();
                while (type.Name != typeof(SimpleSvmForm).Name)
                {
                    type = type.BaseType;
                    inheritLevel++;
                }
                ReflectionHandler.SavePropertiesConfig(this.ConfigTextBox.FilePath, this, inheritLevel);
                SaveLastConfig();
            }
        }

        private void SeriesTrainButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(this.FeatureTrainFile) || !File.Exists(this.FeatureTestFile) || !this.IsDevIdenticalWithTrain && !File.Exists(this.FeatureDevFile))
                MessageBox.Show("Please specify the correct train, dev and test file");
            else
            {
                try
                {
                    if (this.IsNormalized)
                    {
                        List<string> featureFiles = new List<string>();
                        featureFiles.Add(this.FeatureTrainFile);
                        if (File.Exists(this.FeatureTestFile))
                            featureFiles.Add(this.FeatureTestFile);
                        if (!this.IsDevIdenticalWithTrain && File.Exists(this.FeatureDevFile))
                            featureFiles.Add(this.FeatureDevFile);

                        var minMaxDic = FeatureFileProcessor.GetNormalizeMinMaxDic(featureFiles.ToArray());
                        this.FeatureTrainFile = FeatureFileProcessor.NormalizeFeatureFile(this.FeatureTrainFile, minMaxDic);
                        if (File.Exists(this.FeatureTestFile))
                            this.FeatureTestFile = FeatureFileProcessor.NormalizeFeatureFile(this.FeatureTestFile, minMaxDic);
                        if (!this.IsDevIdenticalWithTrain && File.Exists(this.FeatureDevFile))
                            this.FeatureDevFile = FeatureFileProcessor.NormalizeFeatureFile(this.FeatureDevFile, minMaxDic);

                        this.IsNormalized = false;
                    }
                    this.trainThreadPool = new Train_ThreadPool(this, this.ThreadCount, this.IsRankSvm, this.UseFastRankLearn,
                        this.MinJ, this.MaxJ, this.StepJ,
                        this.MinC, this.MaxC, this.StepC, this.IsPowerC,
                        this.FeatureTrainFile, this.IsDevIdenticalWithTrain ? this.FeatureTrainFile : this.FeatureDevFile, this.FeatureTestFile, this.RawTestFile);

                    trainThreadPool.FinishedItemCountChanged += new BaseThreadPool.FinishedItemCountChangedHandler(trainThreadPool_FinishedItemCountChanged);
                    trainThreadPool.ThreadsFinished += new BaseThreadPool.ThreadsFinishedHandler(trainThreadPool_ThreadsFinished);
                    trainThreadPool.StartWork(true, 0);

                    this.ProgressPercentage = 0;
                    this.SeriesTrainButton.Enabled = false;
                    this.CrossSeriesButton.Enabled = false;
                    this.OpenPerformanceButton.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.SeriesTrainButton.Enabled = true;
                    this.CrossSeriesButton.Enabled = true;
                    this.OpenPerformanceButton.Enabled = true;
                }
            }
        }

        private void CrossTrainButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(this.CrossValidationOpenFileTextBox.FilePath))
                MessageBox.Show("Cross validation file does not exists!");
            else
            {
                if (this.IsNormalized)
                {
                    var minMaxDic = FeatureFileProcessor.GetNormalizeMinMaxDic(this.CrossValidationFile);
                    this.CrossValidationFile = FeatureFileProcessor.NormalizeFeatureFile(this.FeatureTrainFile, minMaxDic);
                    this.IsNormalized = false;
                }

                TrainTestResult trainTestResult = SvmLight.CrossValidation(this.ArguTextBox.Text, this.CrossValidationOpenFileTextBox.FilePath, this.CrossValidationFolderCount, this.CalculatePerformance);
                this.ConsoleText = trainTestResult.ToConsoleString("CrossValidation");
            }
        }

        private void CrossSeriesButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(this.CrossValidationFile))
            {
                MessageBox.Show("Please specify the correct cross validation file");
                return;
            }
            try
            {
                if (this.IsNormalized)
                {
                    var minMaxDic = FeatureFileProcessor.GetNormalizeMinMaxDic(this.CrossValidationFile);
                    this.CrossValidationFile = FeatureFileProcessor.NormalizeFeatureFile(this.FeatureTrainFile, minMaxDic);
                    this.IsNormalized = false;
                }

                List<string> performanceList = new List<string>();
                performanceList.Add(ReflectionHandler.GetPropertyNames(typeof(TrainTestResult)).ToRowString());

                this.trainThreadPool = new Train_ThreadPool(this, this.ThreadCount, this.IsRankSvm, this.UseFastRankLearn,
                    this.MinJ, this.MaxJ, this.StepJ,
                    this.MinC, this.MaxC, this.StepC, this.IsPowerC,
                    this.CrossValidationOpenFileTextBox.FilePath, this.MinCrossValidationFolderCount, this.MaxCrossValidationFolderCount);

                trainThreadPool.FinishedItemCountChanged += new BaseThreadPool.FinishedItemCountChangedHandler(trainThreadPool_FinishedItemCountChanged);
                trainThreadPool.ThreadsFinished += new BaseThreadPool.ThreadsFinishedHandler(trainThreadPool_ThreadsFinished);
                trainThreadPool.StartWork(true, 0);

                this.ProgressPercentage = 0;
                this.SeriesTrainButton.Enabled = false;
                this.CrossSeriesButton.Enabled = false;
                this.OpenPerformanceButton.Enabled = false;
            }
            catch (EvaluateException ex)
            {
                MessageBox.Show(ex.Message);
                this.SeriesTrainButton.Enabled = true;
                this.CrossSeriesButton.Enabled = true;
                this.OpenPerformanceButton.Enabled = true;
            }
        }

        protected void LoadConfigButton_Click(object sender, EventArgs e)
        {
            if (this.ConfigTextBox.FilePath.Trim() == string.Empty)
            {
                MessageBox.Show("ConfigTextBox cannot be empty");
                return;
            }

            if (File.Exists(this.ConfigTextBox.FilePath.Trim()))
            {
                ReflectionHandler.LoadPropertiesConfig(this.ConfigTextBox.FilePath, this);
                SaveLastConfig();
                this.ProgressPercentage = 0;
            }
            else
                MessageBox.Show(string.Format("{0} does not exist", this.ConfigTextBox.FilePath.Trim()));
        }

        private void RawTrainProduceButton_Click(object sender, EventArgs e)
        {
            if (this.RawTrainFile.Trim() == string.Empty)
                MessageBox.Show("RawTrainFile cannot be empty");
            else if (!File.Exists(this.RawTrainFile.Trim()))
                MessageBox.Show("RawTrainFile does not exists");
            else
            {
                string featureFile = ProduceFeatureFile(this.RawTrainFile, this.AdditionalFile);
                if (featureFile != string.Empty)
                    this.FeatureTrainFile = featureFile;
            }
        }

        private void RawDevProduceButton_Click(object sender, EventArgs e)
        {
            if (this.RawDevFile.Trim() == string.Empty)
                MessageBox.Show("RawDevFile cannot be empty");
            else if (!File.Exists(this.RawDevFile.Trim()))
                MessageBox.Show("RawDevFile does not exists");
            else
            {
                string featureFile = ProduceFeatureFile(this.RawDevFile, this.AdditionalFile);
                if (featureFile != string.Empty)
                    this.FeatureDevFile = featureFile;
            }
        }

        private void RawTestProduceButton_Click(object sender, EventArgs e)
        {
            if (this.RawTestFile.Trim() == string.Empty)
                MessageBox.Show("RawTestFile cannot be empty");
            else if (!File.Exists(this.RawTestFile.Trim()))
                MessageBox.Show("RawTestFile does not exists");
            else
            {
                string featureFile = ProduceFeatureFile(this.RawTestFile, this.AdditionalFile);
                if (featureFile != string.Empty)
                    this.FeatureTestFile = featureFile;
            }
        }

        private void SaveLastConfig()
        {
            if (this.ConfigTextBox.FilePath.Trim() != string.Empty)
            {
                using (StreamWriter sw = new StreamWriter(ConfigPath))
                {
                    sw.WriteLine(this.ConfigTextBox.FilePath.Trim());
                }
            }
        }

        void trainThreadPool_ThreadsFinished(object sender, EventArgs e)
        {
            this.SeriesTrainButton.Enabled = true;
            this.CrossSeriesButton.Enabled = true;
            this.OpenPerformanceButton.Enabled = true;
            this.ProgressPercentage = 1;
        }
        
        #region virtual methods
        public virtual TrainTestResult CalculatePerformance(string modelFile, string featureFile, string outputFile, string rawFeatureFile)
        {
            return new TrainTestResult(new string[0]);
        }

        protected virtual string ProduceFeatureFile(string rawFile, string additionalFile)
        {
            return string.Empty;
        }

        protected virtual void SaveAdditionalButton_Click(object sender, EventArgs e)
        {

        }

        protected virtual void LoadAdditionalFileButton_Click(object sender, EventArgs e)
        {

        }

        #endregion

        private void StopThreadsButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.trainThreadPool != null)
                    this.trainThreadPool.StopWork(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.PauseResumeButton.Text = "Pause";
            this.SeriesTrainButton.Enabled = true;
            this.CrossSeriesButton.Enabled = true;
            this.OpenPerformanceButton.Enabled = true;
        }

        private void PauseResumeButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.PauseResumeButton.Text == "Pause")
                {
                    if (this.trainThreadPool != null && this.trainThreadPool.IsRunning)
                    {
                        this.trainThreadPool.PauseWork(true);
                        this.PauseResumeButton.Text = "Resume";
                    }
                }
                else if (this.PauseResumeButton.Text == "Resume")
                {
                    if (this.trainThreadPool != null && this.trainThreadPool.IsRunning)
                    {
                        this.trainThreadPool.ResumeWork();
                        this.PauseResumeButton.Text = "Pause";
                    }
                }
                else
                    this.PauseResumeButton.Text = "Pause";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.PauseResumeButton.Text = "Pause";
            }
        }

        private void RawProduceAllButton_Click(object sender, EventArgs e)
        {
            try
            {
                RawTrainProduceButton_Click(null, null);
                RawTestProduceButton_Click(null, null);

                if (!this.IsDevIdenticalWithTrain)
                    RawDevProduceButton_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RankCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsRankSvm && this.UseFastRankLearn)
            {
                Regex cReg = new Regex(@"-(j|b) \d*\.?\d*", RegexOptions.IgnoreCase);
                this.Argument = cReg.Replace(this.Argument.Replace("-z p", ""), " ").RemoveMultiSpace();

                if (!this.Argument.Contains("-c"))
                    this.Argument += ("-c 1 " + this.Argument).RemoveMultiSpace();
            }
            else if(this.IsRankSvm && !this.Argument.Contains("-z p"))
                this.Argument = ("-z p " + this.Argument).RemoveMultiSpace();
            else if (!this.RankSvmCheckBox.Checked && this.Argument.Contains("-z p"))
                this.Argument = this.Argument.Replace("-z p", "").RemoveMultiSpace();
        }

        private void OpenPerformanceButton_Click(object sender, EventArgs e)
        {
            if (this.trainThreadPool != null)
                if (!string.IsNullOrEmpty(this.trainThreadPool.PerformanceFile) && File.Exists(this.trainThreadPool.PerformanceFile))
                    SoftwareOperator.OpenSoftware(Softwares.NOTEPAD, this.trainThreadPool.PerformanceFile);
                else
                    MessageBox.Show("File [" + this.trainThreadPool.PerformanceFile + "] not found");
        }

        private void OpenConfigsButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(System.Environment.CurrentDirectory);
        }

        private void OpenPerformanceDirButton_Click(object sender, EventArgs e)
        {
            if (this.trainThreadPool != null)
                if (!string.IsNullOrEmpty(this.trainThreadPool.PerformanceFile) && Directory.Exists(Path.GetDirectoryName(this.trainThreadPool.PerformanceFile)))
                    System.Diagnostics.Process.Start(Path.GetDirectoryName(this.trainThreadPool.PerformanceFile));
                else
                    MessageBox.Show("Directory [" + Path.GetDirectoryName(this.trainThreadPool.PerformanceFile) + "] not found");
        }

        private void OpenCaseButton_Click(object sender, EventArgs e)
        {
            string rawFile = string.Empty;
            string featureFile = string.Empty;
            if (this.CaseSetComboBox.Text == "Train")
            {
                rawFile = this.RawTrainFile;
                featureFile = this.FeatureTrainFile;
            }
            else if (this.CaseSetComboBox.Text == "Develpment")
            {
                rawFile = this.IsDevIdenticalWithTrain ? this.RawTrainFile : this.RawDevFile;
                featureFile = this.IsDevIdenticalWithTrain ? this.FeatureTrainFile : this.FeatureDevFile;
            }
            else if (this.CaseSetComboBox.Text == "Test")
            {
                rawFile = this.RawTestFile;
                featureFile = this.FeatureTestFile;
            }

            string outputResultFile = FilePath.ChangeExtension(this.ModelFile, FilePath.ChangeExtension(featureFile, "output"));
            if (!File.Exists(outputResultFile))
                throw new Exception("outputFile does not exist: " + outputResultFile);

            var caseData = GetCaseData(this.CaseQueryTextBox.Text, rawFile, featureFile, outputResultFile);
            if (caseData != null && caseData.Length > 0)
            {
                Microsoft.Office.Interop.Excel.Application myExcel = new Microsoft.Office.Interop.Excel.Application();
                myExcel.Workbooks.Add(true);
                myExcel.DisplayAlerts = false;
                myExcel.Visible = true;

                Workbook myBook = myExcel.Workbooks[1];
                Worksheet mySheet = (Worksheet)myBook.Worksheets[1];
                
                mySheet.Name = this.CaseQueryTextBox.Text;
                var myRange = (Range)mySheet.get_Range((Range)myExcel.Cells[1, 1], (Range)myExcel.Cells[caseData.GetUpperBound(0), caseData.GetUpperBound(1)]);
                myRange.Value2 = caseData;
            }
            else
                MessageBox.Show("Could not find case: " + this.CaseQueryTextBox.Text);
        }

        protected virtual string[,] GetCaseData(string query, string rawFile, string featureFile, string outputFile)
        {
            throw new NotImplementedException();            
        }

        private void NormalizeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.RawTrainProduceButton.Enabled = !this.NormalizeCheckBox.Checked;
            this.RawDevProduceButton.Enabled = !this.DevIdenticalCheckBox.Checked && !this.NormalizeCheckBox.Checked;
            this.RawTestProduceButton.Enabled = !this.NormalizeCheckBox.Checked;
            this.ValueOperationComboBox.Enabled = !this.NormalizeCheckBox.Checked;

            if (this.NormalizeCheckBox.Checked)
                this.ValueOperationComboBox.Text = "None";
        }

        private void SimpleSvmForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(ConfigPath))
            {
                using (StreamReader sr = new StreamReader(ConfigPath))
                    this.ConfigTextBox.FilePath = sr.ReadLine().Trim();

                if (File.Exists(this.ConfigTextBox.FilePath))
                    LoadConfigButton_Click(null, null);
            }
        }

        private void ConvertFeatureFileButton_Click(object sender, EventArgs e)
        {
            if (this.TrainMethodComboBox.SelectedItem != null)
            {
                if (this.TrainMethodComboBox.SelectedItem.ToString() == "RankSVM")
                {
                    if (this.ZeroComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Please specify the zero operation");
                        return;
                    }
                    if (this.FeatureTrainFile != string.Empty && File.Exists(this.FeatureTrainFile))
                        this.FeatureTrainFile = FeatureFileProcessor.TransferFeatureFile_SvmToRankSvm(this.FeatureTrainFile, this.ZeroComboBox.SelectedItem.ToString(), this.LabelFineCoarseCheckBox.Checked);

                    if (!this.IsDevIdenticalWithTrain && this.FeatureDevFile != string.Empty && File.Exists(this.FeatureDevFile))
                        this.FeatureDevFile = FeatureFileProcessor.TransferFeatureFile_SvmToRankSvm(this.FeatureDevFile, this.ZeroComboBox.SelectedItem.ToString(), this.LabelFineCoarseCheckBox.Checked);

                    if (this.FeatureTestFile != string.Empty && File.Exists(this.FeatureTestFile))
                        this.FeatureTestFile = FeatureFileProcessor.TransferFeatureFile_SvmToRankSvm(this.FeatureTestFile, this.ZeroComboBox.SelectedItem.ToString(), this.LabelFineCoarseCheckBox.Checked);

                    this.IsRankSvm = true;
                }
                else if (this.TrainMethodComboBox.SelectedItem.ToString() == "SVM")
                {
                    if (this.ZeroComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Please specify the zero operation");
                        return;
                    }

                    if (this.FeatureTrainFile != string.Empty && File.Exists(this.FeatureTrainFile))
                        this.FeatureTrainFile = FeatureFileProcessor.TransferFeatureFile_RankSvmToSvm(this.FeatureTrainFile, Convert.ToDouble(this.SplitLabelNumeric.Value), this.ZeroComboBox.SelectedItem.ToString(), this.LabelFineCoarseCheckBox.Checked);

                    if (!this.IsDevIdenticalWithTrain && this.FeatureDevFile != string.Empty && File.Exists(this.FeatureDevFile))
                        this.FeatureDevFile = FeatureFileProcessor.TransferFeatureFile_RankSvmToSvm(this.FeatureDevFile, Convert.ToDouble(this.SplitLabelNumeric.Value), this.ZeroComboBox.SelectedItem.ToString(), this.LabelFineCoarseCheckBox.Checked);

                    if (this.FeatureTestFile != string.Empty && File.Exists(this.FeatureTestFile))
                        this.FeatureTestFile = FeatureFileProcessor.TransferFeatureFile_RankSvmToSvm(this.FeatureTestFile, Convert.ToDouble(this.SplitLabelNumeric.Value), this.ZeroComboBox.SelectedItem.ToString(), this.LabelFineCoarseCheckBox.Checked);

                    this.IsRankSvm = false;
                }
                else
                {
                    if (this.FeatureTrainFile != string.Empty && File.Exists(this.FeatureTrainFile))
                        this.FeatureTrainFile = FeatureFileProcessor.GetTransferFeatureFileName(this.FeatureTrainFile, TrainMethods.None);

                    if (!this.IsDevIdenticalWithTrain && this.FeatureDevFile != string.Empty && File.Exists(this.FeatureDevFile))
                        this.FeatureDevFile = FeatureFileProcessor.GetTransferFeatureFileName(this.FeatureDevFile, TrainMethods.None);

                    if (this.FeatureTestFile != string.Empty && File.Exists(this.FeatureTestFile))
                        this.FeatureTestFile = FeatureFileProcessor.GetTransferFeatureFileName(this.FeatureTestFile, TrainMethods.None);
                }
            }
            else
                MessageBox.Show("Please select the featureFile type");
        }

        private void TrainMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.TrainMethodComboBox.SelectedItem.ToString() == "RankSVM")
            {
                this.SplitLabelNumeric.Enabled = false;
                this.LabelFineCoarseCheckBox.Checked = false;
            }
            else if (this.TrainMethodComboBox.SelectedItem.ToString() == "SVM")
            {
                this.SplitLabelNumeric.Enabled = true;
                this.LabelFineCoarseCheckBox.Checked = true;
            }
        }

        private void MagnifyButton_Click(object sender, EventArgs e)
        {
            if (this.MagnifyTextBox.Text.Trim() != string.Empty)
            {
                List<int> selectedLabels = new List<int>();
                var labels = RegularExpressions.UselessPunctionRegex.Replace(this.MagnifyTextBox.Text, " ").RemoveMultiSpace().Split(' ');
                foreach (var label in labels)
                {
                    int labelInt = -1;
                    if (int.TryParse(label, out labelInt))
                        selectedLabels.Add(labelInt);
                }

                if (this.FeatureTrainFile != string.Empty && File.Exists(this.FeatureTrainFile))
                    FeatureFileProcessor.MagnifyExamples(this.FeatureTrainFile, Convert.ToInt32(this.MagnifyNumeric.Value), selectedLabels.ToArray());

                if (!this.IsDevIdenticalWithTrain && this.FeatureDevFile != string.Empty && File.Exists(this.FeatureDevFile))
                    FeatureFileProcessor.MagnifyExamples(this.FeatureDevFile, Convert.ToInt32(this.MagnifyNumeric.Value), selectedLabels.ToArray());

                if (this.FeatureTestFile != string.Empty && File.Exists(this.FeatureTestFile))
                    FeatureFileProcessor.MagnifyExamples(this.FeatureTestFile, Convert.ToInt32(this.MagnifyNumeric.Value), selectedLabels.ToArray());
            }
        }

        private void SplitButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(this.FeatureTrainFile))
            {
                if (this.SplitComboBox.SelectedItem == null || !this.SplitComboBox.SelectedItem.ToString().Contains(":"))
                {
                    MessageBox.Show("Please select ratio");
                    return;
                }

                List<List<string>> samples = FeatureFileProcessor.ReadFeatureFile(this.FeatureTrainFile);
                if (this.SplitRandomCheckBox.Checked)
                    samples = samples.ToRandomList();

                var ratios = this.SplitComboBox.SelectedItem.ToString().Split(':').Select(n => int.Parse(n)).ToArray();
                var sum = ratios.Sum();
                //if (ratios.Length == 2)
                //{
                //    this.IsDevIdenticalWithTrain = true;
                //    int trainCount = (int)(samples.Count * 1.0 * ratios[0] / sum);
                //    int testCount = samples.Count - trainCount;

                //    List<string> trainSet = new List<string>();
                //    List<string> testSet = new List<string>();

                //    trainSet.AddRange(samples.GetRange(0, trainCount).ToTension());
                //    testSet.AddRange(samples.GetRange(trainCount, testCount).ToTension());

                //    string splitTrainFeatureFile = FilePath.ChangeExtension(this.FeatureTrainFile, "SplitTrain.feature");
                //    string splitTestFeatureFile = FilePath.ChangeExtension(this.FeatureTrainFile, "SplitTest.feature");

                //    FileWriter.PrintCollection(splitTrainFeatureFile, trainSet);
                //    FileWriter.PrintCollection(splitTestFeatureFile, testSet);

                //    this.FeatureTrainFile = splitTrainFeatureFile;
                //    this.FeatureTestFile = splitTestFeatureFile;
                //}
                int trainCount = 0;
                int devCount = 0;
                int testCount = 0;

                if (ratios[0] == 0)
                {
                    devCount = (int)(samples.Count * 1.0 * ratios[1] / sum);
                    testCount = samples.Count - devCount;
                }
                else if (ratios[1] == 0)
                {
                    trainCount = (int)(samples.Count * 1.0 * ratios[0] / sum);
                    testCount = samples.Count - trainCount;
                }
                else if (ratios[2] == 0)
                {
                    trainCount = (int)(samples.Count * 1.0 * ratios[0] / sum);
                    devCount = samples.Count - trainCount;
                }
                else
                {
                    trainCount = (int)(samples.Count * 1.0 * ratios[0] / sum);
                    devCount = (int)(samples.Count * 1.0 * ratios[1] / sum);
                    testCount = samples.Count - trainCount - devCount;
                }

                if (ratios[1] == 0)
                    this.IsDevIdenticalWithTrain = true;
                else
                {
                    this.IsDevIdenticalWithTrain = false;
                    List<string> devSet = new List<string>();
                    devSet.AddRange(samples.GetRange(trainCount, devCount).ToTension());
                    string splitDevFeatureFile = FilePath.ChangeExtension(this.FeatureTrainFile, "SplitDev.feature");
                    FileWriter.PrintCollection(splitDevFeatureFile, devSet);
                    this.FeatureDevFile = splitDevFeatureFile;
                }

                if (ratios[2] > 0)
                {
                    List<string> testSet = new List<string>();
                    testSet.AddRange(samples.GetRange(trainCount + devCount, testCount).ToTension());
                    string splitTestFeatureFile = FilePath.ChangeExtension(this.FeatureTrainFile, "SplitTest.feature");
                    FileWriter.PrintCollection(splitTestFeatureFile, testSet);
                    this.FeatureTestFile = splitTestFeatureFile;
                }

                if (ratios[0] > 0)
                {
                    List<string> trainSet = new List<string>();
                    trainSet.AddRange(samples.GetRange(0, trainCount).ToTension());
                    string splitTrainFeatureFile = FilePath.ChangeExtension(this.FeatureTrainFile, "SplitTrain.feature");
                    FileWriter.PrintCollection(splitTrainFeatureFile, trainSet);
                    this.FeatureTrainFile = splitTrainFeatureFile;
                }
            }
            else
                MessageBox.Show("FeatureTrainFile not exists");
        }
    }

    public class Train_ThreadPool : BaseThreadPool
    {
        private SimpleSvmForm _svmForm;
        public string TrainFeatureFile { get; set; }
        public string DevFeatureFile { get; set; }
        public string TestFeatureFile { get; set; }
        public string AdditionalFile { get; set; }
        public string PerformanceFile
        {
            get
            {
                if (!this.IsCrossValidation)
                {
                    return FilePath.ChangeFile(this.TrainFeatureFile, FilePath.MergeDistinctFilePathPart(this.TrainFeatureFile, this.DevFeatureFile, this.TestFeatureFile) + string.Format(".{0}NormalValidation.performances.txt", this.IsRankSvm && this.UseFastRankLearn ? "FastRank." : ""));
                }
                else
                    return FilePath.ChangeExtension(this.TrainFeatureFile, string.Format("{0}CrossValidation.performances.txt", this.IsRankSvm && this.UseFastRankLearn ? "FastRank." : ""));
            }
        }
        public List<string> PerformanceList { get; set; }

        public int MaxFolderCount { get; set; }
        public int MinFolderCount { get; set; }

        public decimal MinJ { get; set; }
        public decimal MaxJ { get; set; }
        public decimal StepJ { get; set; }

        public decimal MinC { get; set; }
        public decimal MaxC { get; set; }
        public decimal StepC { get; set; }
        public bool IsPowerC { get; set; }

        public bool IsCrossValidation { get; set; }
        public bool IsRankSvm { get; set; }
        public bool UseFastRankLearn { get; set; }

        public string[] PerformancePropertyNames { get; set; }

        public double[] CNonPowGrades = new double[] { 1, 2, 5 };
        public int CMaxPowGrades = 4;

        private int lastPrintedPercentage = 0;

        public Train_ThreadPool(SimpleSvmForm svmFrom, int threadCount, bool rankSvm, bool useFastRankLearn,
            decimal minJ, decimal maxJ, decimal stepJ,
            decimal minPowerC, decimal maxPowerC, decimal stepPowerC, bool isPowerC,
            string trainFeatureFile, string devFeatureFile, string testFeatureFile, string additionalFile = ""
            ) : base(threadCount)
        {
            this._svmForm = svmFrom;
            this.IsRankSvm = rankSvm;
            this.UseFastRankLearn = useFastRankLearn;

            this.TrainFeatureFile = trainFeatureFile;
            this.DevFeatureFile = devFeatureFile;
            this.TestFeatureFile = testFeatureFile;
            this.AdditionalFile = additionalFile;
            this.PerformanceList = new List<string>();

            this.MinFolderCount = 1;
            this.MaxFolderCount = 1;

            this.MinJ = minJ;
            this.MaxJ = maxJ;
            this.StepJ = stepJ;

            this.MinC = minPowerC;
            this.MaxC = maxPowerC;
            this.StepC = stepPowerC;
            this.IsPowerC = isPowerC;

            this.IsCrossValidation = false;
            this.FinishedPercentageChanged += new FinishedPercentageChangedHandler(Train_ThreadPool_FinishedPercentageChanged);
        }


        public Train_ThreadPool(SimpleSvmForm svmFrom, int threadCount, bool rankSvm, bool useFastRankLearn,
            decimal minJ, decimal maxJ, decimal stepJ,
            decimal minPowerC, decimal maxPowerC, decimal stepPowerC, bool isPowerC,
            string trainFeatureFile, int minFolderCount, int maxFolderCount
            )
            : base(threadCount)
        {
            this._svmForm = svmFrom;
            this.IsRankSvm = rankSvm;
            this.UseFastRankLearn = useFastRankLearn;

            this.TrainFeatureFile = trainFeatureFile;
            this.DevFeatureFile = trainFeatureFile;
            this.TestFeatureFile = trainFeatureFile;
            this.PerformanceList = new List<string>();

            this.MinJ = minJ;
            this.MaxJ = maxJ;
            this.StepJ = stepJ;

            this.MinC = minPowerC;
            this.MaxC = maxPowerC;
            this.StepC = stepPowerC;
            this.IsPowerC = isPowerC;

            this.IsCrossValidation = true;
            this.MinFolderCount = minFolderCount;
            this.MaxFolderCount = maxFolderCount;

            this.FinishedPercentageChanged +=new FinishedPercentageChangedHandler(Train_ThreadPool_FinishedPercentageChanged);
        }

        protected override void AssignWork(bool useRandomItems)
        {
            List<double> cItems = new List<double>();
            double tempC = Convert.ToDouble(this.MinC);
            while (tempC <= Convert.ToDouble(this.MaxC))
            {
                if (this.IsPowerC)
                {
                    double c = Math.Pow(2, tempC);
                    cItems.Add(c);
                    tempC += Convert.ToDouble(this.StepC);
                }
                else
                {
                    int order = -3;
                    int index = 0;
                    for (order = -3; order <= 4; order++)
                    {
                        for (index = 0; index < this.CNonPowGrades.Length; index++)
                        {
                            if (this.CNonPowGrades[index] * Math.Pow(10, order) >= tempC)
                                break;
                        }

                        if (index < this.CNonPowGrades.Length)
                            break;
                    }

                    double c = this.CNonPowGrades[index] * Math.Pow(10, order);
                    cItems.Add(c);
                    tempC = c * 2;
                }
            }

            List<int> folders = new List<int>();
            if (this.IsCrossValidation)
                for (int folder = this.MinFolderCount; folder <= this.MaxFolderCount; folder++)
                    folders.Add(folder);
            else
                folders.Add(1);

            List<double> jItems = new List<double>();
            List<int> bItems = new List<int>();
            if (this.IsRankSvm && this.UseFastRankLearn)
            {
                jItems.Add(1);
                bItems.Add(1);
            }
            else
            {
                for (double j = Convert.ToDouble(this.MinJ); j <= Convert.ToDouble(this.MaxJ); j += Convert.ToDouble(this.StepJ))
                    jItems.Add(j);

                bItems.Add(0);
                bItems.Add(1);
            }

            foreach (var f in folders)
                foreach (var c in cItems)
                    foreach (var j in jItems)
                        foreach (var b in bItems)
                            this.Items.Add(new Quad<int, double, double, int>(f, c, j, b));

            base.AssignWork(useRandomItems);
        }

        protected override void FinishWork(bool isPause)
        {
            try
            {
                this.PerformanceList = new List<string>();

                foreach (var thread in this.Threads)
                {
                    lock (((Train_Thread)thread).PerformanceList)
                    {
                        if (((Train_Thread)thread).PerformancePropertyNames != null && (this.PerformancePropertyNames == null || this.PerformancePropertyNames.Length < ((Train_Thread)thread).PerformancePropertyNames.Length))
                            this.PerformancePropertyNames = ((Train_Thread)thread).PerformancePropertyNames;

                        this.PerformanceList.AddRange(((Train_Thread)thread).PerformanceList);
                    }
                }

                if (this.PerformanceList != null)
                {
                    if (this.PerformancePropertyNames != null)
                        this.PerformanceList.Insert(0, this.PerformancePropertyNames.ToRowString());

                    FileWriter.PrintCollection(this.PerformanceFile, this.PerformanceList);
                }

                if (!isPause)
                {
                    foreach (var thread in this.Threads)
                    {
                        string[] tempFiles =
                            this.IsCrossValidation ?
                            SvmLight.GetCrossValidationTempFiles(this.TrainFeatureFile, this.UseFastRankLearn && this.IsRankSvm, thread.ThreadGUID)
                            :
                            SvmLight.GetNormalValidationFiles(this.TrainFeatureFile, this.DevFeatureFile, this.TestFeatureFile, this.UseFastRankLearn && this.IsRankSvm, thread.ThreadGUID);

                        foreach (var tempFile in tempFiles)
                        {
                            try
                            {
                                if (File.Exists(tempFile))
                                    File.Delete(tempFile);
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        protected override BaseThread CreateBaseThread(object[] items)
        {
            return new Train_Thread(this, this._svmForm, items);
        }

        void Train_ThreadPool_FinishedPercentageChanged(object sender, int newPercentage)
        {
            if (newPercentage - this.lastPrintedPercentage >= 10)
            {
                this.lastPrintedPercentage = newPercentage;
                FinishWork(true);
            }
        }

    }

    public class Train_Thread : BaseThread
    {
        public SimpleSvmForm _svmForm;
        public string TrainFeatureFile
        {
            get
            {
                return ((Train_ThreadPool)this.Parent).TrainFeatureFile;
            }
        }
        public string DevFeatureFile
        {
            get
            {
                return ((Train_ThreadPool)this.Parent).DevFeatureFile;
            }
        }
        public string TestFeatureFile
        {
            get
            {
                return ((Train_ThreadPool)this.Parent).TestFeatureFile;
            }
        }
        public string AdditionalFile
        {
            get
            {
                return ((Train_ThreadPool)this.Parent).AdditionalFile;
            }
        }

        public List<string> PerformanceList { get; set; }

        public bool IsCrossValidation
        {
            get
            {
                return ((Train_ThreadPool)this.Parent).IsCrossValidation;
            }
        }
        public bool IsRankSvm
        {
            get
            {
                return ((Train_ThreadPool)this.Parent).IsRankSvm;
            }
        }
        public bool UseFastRankLearn
        {
            get
            {
                return ((Train_ThreadPool)this.Parent).UseFastRankLearn;
            }
        }
        public string[] PerformancePropertyNames { get; set; }

        public Train_Thread(BaseThreadPool parent, SimpleSvmForm svmForm, object[] items)
            : base(parent, items)
        {
            this._svmForm = svmForm;
            this.PerformanceList = new List<string>();
        }

        public override void Work()
        {
            while (this.HandledItemCount < this.Items.Length)
                ReceiveResults(DoItem(GetNextItem()));

            int unFinishedThreadID = -1;
            while ((unFinishedThreadID = this.Parent.Threads.FindIndex(t => !t.AllHandled)) >= 0)
                ((Train_Thread)this.Parent.Threads[unFinishedThreadID]).ReceiveResults(DoItem(((Train_Thread)this.Parent.Threads[unFinishedThreadID]).GetNextItem()));

            base.Work();
        }

        public object GetNextItem()
        {
            lock (this.Items)
            {
                if (this.HandledItemCount >= this.Items.Length)
                    return null;
                else
                {
                    object item = this.Items[this.HandledItemCount];
                    this.HandledItemCount++;
                    return item;
                }
            }
        }

        public string DoItem(object item)
        {
            if (item == null)
                return null;

            Quad<int, double, double, int> fcjb = (Quad<int, double, double, int>)item;
            int f = fcjb.First;
            double c = fcjb.Second;
            double j = fcjb.Third;
            int b = fcjb.Forth;

            string argument = this.IsRankSvm && this.UseFastRankLearn ?
                ("-c " + c) 
                : 
                ((IsRankSvm ? "-z p " : string.Empty) + "-c " + c + " -j " + j + " -b " + b);

            TrainTestResult trainTestResult = this.IsCrossValidation ?
                SvmLight.CrossValidation(argument, this.TrainFeatureFile, f, this._svmForm.CalculatePerformance, this.UseFastRankLearn && this.IsRankSvm, this.ThreadGUID)
                :
                SvmLight.NormalValidation(argument, this.TrainFeatureFile, this.DevFeatureFile, this.TestFeatureFile, this._svmForm.CalculatePerformance, this.UseFastRankLearn && this.IsRankSvm, this.ThreadGUID, this.AdditionalFile);

            if (this.PerformancePropertyNames == null || this.PerformancePropertyNames.Length < trainTestResult.GetPropertyNames().Length)
                this.PerformancePropertyNames = trainTestResult.GetPropertyNames();

            return trainTestResult.ToString();
        }

        public void ReceiveResults(string result)
        {
            if (result == null)
                return;

            lock (this.PerformanceList)
            {
                this.PerformanceList.Add(result);
            }
        }
    }
}
