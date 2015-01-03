using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TrickyLib.IO;
using TrickyLib.MachineLearning;
using TrickyLib.MachineLearning.Tools;
using TrickyLib.Reflection;
using TrickyLib.Threading;
using TrickyLib.Extension;

namespace TrickyLib.Weatoo
{
    public partial class WeatooForm : Form
    {
        protected BaseExperimentSetting _experimentSetting;
        private readonly string _configDir = Path.Combine(Application.StartupPath, "config");
        private readonly string _lastConfigFile = Path.Combine(Path.Combine(Application.StartupPath, "config"), "lastConfig.txt");
        private string _configFile = "";
        private bool _runExpNextStep = false;
        public string ConfigFile
        {
            get
            {
                return _configFile.Trim();
            }
            private set
            {
                _configFile = value;
            }
        }

        public WeatooForm()
        {
            InitializeComponent();
            if (File.Exists(_lastConfigFile))
            {
                var lines = FileReader.ReadRows(_lastConfigFile);
                if(lines.Count > 0)
                    ConfigFile = lines[0];
            }
            if (!TryLoadLastConfig(ConfigFile.Trim()))
            {
                Text = "Untitled";
                ConfigFile = _configDir + "\\" + Text + ".config";

                _experimentSetting = new BaseExperimentSetting();
                _experimentSetting.Mode = RunMode.Train;
            }
        }

        public WeatooForm(BaseExperimentSetting expSetting)
        {
            InitializeComponent();
            _experimentSetting = expSetting;
        }

        #region Initialize
        void InitializeComponent_2()
        {
            //Add Adopt column in datagridview
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn() { HeaderText = "Adopt", Name = "AdoptButtonColumn", Text = "Adopt" };
            buttonColumn.UseColumnTextForButtonValue = true;
            ResultsDataGridView.Columns.Add(buttonColumn);

            //Add some events
            ResultsDataGridView.CellClick += ResultsDataGridView_CellClick;
            PercentStatusStrip.PropertyChanged += PercentStatusStrip_PercentageChanged;
            this.Load += WeatooForm_Load;
        }

        void SetFormEnabled(bool enabled)
        {
            this.MainSplitContainer.Enabled = enabled;
        }

        void InitializeMember()
        {
            //Initialize the property grid
            ExpOptionPropertyGrid.SelectedObject = _experimentSetting;
            MetricPropertyGrid.SelectedObject = _experimentSetting.Metric;
            ParameterPropertyGrid.SelectedObject = _experimentSetting.ParameterSetting;

            //Events
            _experimentSetting.PropertyChanged += ExperimentSetting_PropertyChanged;
            _experimentSetting.ExperimentFinished += ExperimentSetting_ExperimentFinished;
            BaseExperimentSetting.MessageReceived += new BaseExperimentSetting.MessageReceivedHander(ExperimentSetting_MessageReceived);
            _experimentSetting.FinishedPercentageChanged += ExperimentSetting_FinishedPercentageChanged;
            _experimentSetting.ProcessingTextChanged += ExperimentSetting_ProcessingTextChanged;
            _experimentSetting.Finished10MorePercentage += ExperimentSetting_Finished10MorePercentage;
            _experimentSetting.TransferFeatureFileFormatFinished += ExperimentSetting_TransferFeatureFileFormatFinished;

            //Initialize the appearance
            _experimentSetting.InInitialize = true;
            _experimentSetting.SetApparenceFromTask();
            _experimentSetting.SetApparenceFromMode();
            _experimentSetting.SetApparenceFromLeaner();
            _experimentSetting.SetApparenceFromIsGenerate();
            _experimentSetting.InInitialize = false;
        }

        public static bool MyInterfaceFilter(Type typeObj, Object criteriaObj)
        {
            if (typeObj.ToString() == criteriaObj.ToString())
                return true;
            else
                return false;
        }
        #endregion

        #region Event Methods
        public static void AppThreadException(object source, System.Threading.ThreadExceptionEventArgs e)
        {
            BaseExperimentSetting.ReceiveMessage(e.Exception.Message);
            DialogResult result = MessageBox.Show(e.Exception.Message, "Application Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);

            //如果点击“中止”则退出程序
            if (result == DialogResult.Abort)
            {
                Application.Exit();
            }
        }
        void ResultsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore clicks that are not on button cells.  
            if (e.RowIndex < 0
                || e.ColumnIndex != ResultsDataGridView.Columns["AdoptButtonColumn"].Index)
                return;

            string parameter = ResultsDataGridView.Rows[e.RowIndex].Cells["Argument"].Value.ToString();
            _experimentSetting.ParameterSetting.ReadParameterString(parameter);
            ParameterPropertyGrid.Refresh();
        }
        void WeatooForm_Load(object sender, EventArgs e)
        {
            //Set the title
            Text = Path.GetFileNameWithoutExtension(ConfigFile);

            //Set the percentage status strip unvisible
            this.PercentStatusStrip.SetVisibility_Percentage(false);
            this.PercentStatusStrip.SetVisibility_ProcessingText(false);
            this.DiskComboBox.Text = Environment.CurrentDirectory[0].ToString();

            //Initialize member
            InitializeMember();
            InitializeComponent_2();
        }
        void PercentStatusStrip_PercentageChanged(object sender, string labelText)
        {
            lock (this)
            {
                try
                {
                    BeginInvoke(new Action(() =>
                    {
                        this.Text = Path.GetFileNameWithoutExtension(ConfigFile) + " " + labelText;
                    }));
                }
                catch (Exception ex)
                {

                }
            }
        }
        void ExperimentSetting_MessageReceived(string message)
        {
            lock (RunTextBox)
            {
                try
                {
                    BeginInvoke(new Action(() =>
                        {
                            RunTextBox.Text += message + "\n";
                            RunTextBox.Focus();
                            RunTextBox.Select(RunTextBox.TextLength, 0);
                            RunTextBox.ScrollToCaret();
                        }));
                }
                catch (Exception ex)
                {

                }
            }
        }
        void ExperimentSetting_ExperimentFinished(BaseThreadPool threadPool)
        {
            var performances = threadPool.Results.Select(kv => kv.Value as TrainTestResult);
            if (performances == null || performances.Count() <= 0)
            {
                if(threadPool != null && !threadPool.IsRunning)
                    MessageBox.Show("Experiment finished, but there is not performance results produced");

                return;
            }

            ExperimentSetting_Finished10MorePercentage(threadPool);

            if (threadPool != null && !threadPool.IsRunning && threadPool.FinishedThreadCount == threadPool.ThreadCount)
            {
                PercentStatusStrip.ProcessingText = "Job Finished";
                MessageBox.Show("Congratulations! The job is finished.");
            }
        }
        void ExperimentSetting_FinishedPercentageChanged(object sender, double percentage)
        {
            this.PercentStatusStrip.ProgressPercentage = percentage;
        }
        void ExperimentSetting_Finished10MorePercentage(BaseThreadPool threadPool)
        {
            lock (ResultsDataGridView)
            {
                var performances = threadPool.Results.Select(kv => kv.Value as TrainTestResult);
                if (performances == null || performances.Count() <= 0)
                    return;

                var dataTable = ConvertPerformancesToDataTable(performances);
                BeginInvoke(new Action(() =>
                {
                    ResultsDataGridView.DataSource = dataTable;
                    ResultsDataGridView.Refresh();
                }));
            }
        }
        void ExperimentSetting_PropertyChanged(BaseExperimentSetting expSetting)
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    this.ParameterPropertyGrid.SelectedObject = _experimentSetting.ParameterSetting;
                    this.ParameterPropertyGrid.Refresh();
                    this.ExpOptionPropertyGrid.Refresh();
                    this.MetricPropertyGrid.Refresh();
                    this.GenerateButton.Enabled = _experimentSetting.IsGenerate;
                }));
            }
            catch (Exception ex)
            {

            }
        }
        void ExperimentSetting_ProcessingTextChanged(string text)
        {
            PercentStatusStrip.ProcessingText = text;
        }
        void AdoptCustomParaButton_Click(object sender, EventArgs e)
        {
            try
            {
                _experimentSetting.ParameterSetting.ReadParameterString(CustomParaTextBox.Text);
                ParameterPropertyGrid.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void ChangeDiskButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(DiskComboBox.Text))
                _experimentSetting.ChangeDisk(DiskComboBox.Text[0]);
        }
        void ExperimentSetting_TransferFeatureFileFormatFinished(BaseExperimentSetting sender)
        {
            PercentStatusStrip.SetIsBusy(false, "Finished transfering data format");
            BeginInvoke(new Action(() => MainSplitContainer.Enabled = true));
            if(_runExpNextStep)
                RunButton_Click(sender, null);
        }
        void WeatooForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_experimentSetting.Tool_ThreadPool_Running != null && _experimentSetting.Tool_ThreadPool_Running.IsRunning)
            {
                if (MessageBox.Show("There is a job in running. Do you really to exit? Click YES to save the progress and exit", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    StopButton_Click(null, null);
                else
                {
                    e.Cancel = true;
                }
            }
        }
        #endregion

        #region Experiment
        private void RunButton_Click(object sender, EventArgs e)
        {
            if (_experimentSetting.Tool_ThreadPool_Running == null || !_experimentSetting.Tool_ThreadPool_Running.IsRunning)
            {
                if (_experimentSetting.CheckWhetherNeedToTransfer())
                {
                    BeginInvoke(new Action(() => MainSplitContainer.Enabled = false));

                    PercentStatusStrip.SetIsBusy(true, "Transfering data format...");
                    Thread thread = new Thread(_experimentSetting.TransferFeatureFileFormat);
                    thread.Start();
                    thread.Join(0);

                    _runExpNextStep = true;
                    return;
                }

                _runExpNextStep = false;
                BeginInvoke(new Action(() => MainTabControl.SelectedIndex = 0));
                _experimentSetting.RunExperiment();
                PercentStatusStrip.ProgressPercentage = 0;
                PercentStatusStrip.ProcessingText = "Training...";
                PauseButton.Text = "Pause";
                CleanPerformanceButton_Click(null, null);
            }
            else if (MessageBox.Show("There is a job in running. Do you want to stop it?", "Warning", MessageBoxButtons.YesNo)
                    == System.Windows.Forms.DialogResult.Yes)
            {
                StopButton_Click(sender, e);
                RunButton_Click(sender, e);
            }
        }
        private void StopButton_Click(object sender, EventArgs e)
        {
            if (_experimentSetting.Tool_ThreadPool_Running != null && _experimentSetting.Tool_ThreadPool_Running.IsRunning)
            {
                _experimentSetting.StopExperiment();
                PercentStatusStrip.ProcessingText = "Stopped";
                PauseButton.Text = "Pause";
            }
        }
        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (_experimentSetting.Tool_ThreadPool_Running != null && _experimentSetting.Tool_ThreadPool_Running.IsRunning)
            {
                if (PauseButton.Text == "Pause")
                {
                    _experimentSetting.PauseExperiment();
                    PauseButton.Text = "Resume";
                    PercentStatusStrip.ProcessingText = "Paused";
                }
                else
                {
                    _experimentSetting.ResumeExperiment();
                    PauseButton.Text = "Pause";
                    PercentStatusStrip.ProcessingText = "Training...";
                }
            }
        }
        private void CleanButton_Click(object sender, EventArgs e)
        {
            RunTextBox.Text = "";
        }
        #endregion

        #region Config file
        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(_configDir))
                Directory.CreateDirectory(_configDir);

            OpenFileDialog ofd = new OpenFileDialog() { Filter = "Config File|*.config|All Files|*.*", InitialDirectory = _configDir };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!TryLoadLastConfig(ofd.FileName))
                    MessageBox.Show("Error when loading config file: " + ofd.FileName);
            }
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(_configDir))
                Directory.CreateDirectory(_configDir);

            if (!File.Exists(ConfigFile))
            {
                SaveFileDialog sfd = new SaveFileDialog() { Filter = "Config File|*.config|All Files|*.*", InitialDirectory = _configDir };
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Parser.SerializeParser.Serialize(_experimentSetting, sfd.FileName);
                    ConfigFile = sfd.FileName;
                    Text = Path.GetFileNameWithoutExtension(ConfigFile);
                    SaveLastConfig();
                }
            }
            else
            {
                Parser.SerializeParser.Serialize(_experimentSetting, ConfigFile);
                Text = Path.GetFileNameWithoutExtension(ConfigFile);
                SaveLastConfig();
            }
        }
        private void NewButton_Click(object sender, EventArgs e)
        {
            Text = "Untitled";
            ConfigFile = _configDir + "\\" +  Text + ".config";

            _experimentSetting = new BaseExperimentSetting();
            InitializeMember();
            Refresh();
        }
        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(_configDir))
                Directory.CreateDirectory(_configDir);

            SaveFileDialog sfd = new SaveFileDialog() { Filter = "Config File|*.config|All Files|*.*", InitialDirectory = _configDir };
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Parser.SerializeParser.Serialize(_experimentSetting, sfd.FileName);
                ConfigFile = sfd.FileName;
                Text = Path.GetFileNameWithoutExtension(ConfigFile);
                SaveLastConfig();
            }
        }
        private bool TryLoadLastConfig(string filePath)
        {
            try
            {
                //load config
                _experimentSetting = Parser.SerializeParser.Deserialize(filePath.Trim()) as BaseExperimentSetting;
                Text = Path.GetFileNameWithoutExtension(filePath);
                InitializeMember();
                Refresh();

                //save config
                ConfigFile = filePath;
                SaveLastConfig();

                //success
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void SaveLastConfig()
        {
            if (!Directory.Exists(_configDir))
                Directory.CreateDirectory(_configDir);

            using (StreamWriter sw = new StreamWriter(_lastConfigFile))
            {
                if (this.ConfigFile.StartsWith(Application.StartupPath))
                    sw.WriteLine(this.ConfigFile.Substring(Application.StartupPath.Length + 1));
                else
                    sw.WriteLine(this.ConfigFile);
            }
        }
        #endregion

        #region virtual functions
        protected virtual TrainTestResult GetCustomizePerformance(string featureFile, string outputFile, string rawFile, string[] additionalFiles)
        {
            return null;
        }
        #endregion

        #region performance
        private void CleanPerformanceButton_Click(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                DataTable dataTable = new DataTable();
                ResultsDataGridView.DataSource = dataTable;
                ResultsDataGridView.Refresh();
            }));
        }
        private void LoadPerformanceConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "Performance File|*.prf|All Files|*.*", InitialDirectory = Path.GetDirectoryName(_experimentSetting.TrainFile) };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    var performances = API_ThreadPool.ReadPerformanceFile(ofd.FileName);
                    var dataTable = ConvertPerformancesToDataTable(performances);

                    BeginInvoke(new Action(() =>
                    {
                        ResultsDataGridView.DataSource = dataTable;
                        ResultsDataGridView.Refresh();
                    }));
                }
                catch (Exception ex)
                { 
                
                }
            }
        }
        private DataTable ConvertPerformancesToDataTable(IEnumerable<TrainTestResult> performances)
        {
            //Add propertyNames as new column headers
            bool isCrossValidation = performances.Exists(p => p.FolderCount > 0);
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Argument");
            if (isCrossValidation)
                dataTable.Columns.Add("Folder");
            foreach (var propertyName in performances.ElementAt(0).PropertyNames)
                dataTable.Columns.Add(propertyName);

            //Add each performance as a row
            foreach (var performance in performances)
            {
                DataRow row = dataTable.NewRow();
                row["Argument"] = performance.Argu;
                if (isCrossValidation)
                    row["Folder"] = performance.FolderCount;

                for (int i = 0; i < performance.Properties.Count; ++i)
                    row[performance.PropertyNames[i]] = performance.Properties[i];

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        #endregion

    }
}
