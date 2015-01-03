using TrickyLib.WinControl;
using System.Windows.Forms;
namespace TrickyLib.WinForm
{
    partial class SelectSvmForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectSvmForm));
            this.FeatureCLBox = new System.Windows.Forms.CheckedListBox();
            this.InfoGainDGView = new System.Windows.Forms.DataGridView();
            this.Clear = new System.Windows.Forms.Button();
            this.Reverse = new System.Windows.Forms.Button();
            this.SelectAll = new System.Windows.Forms.Button();
            this.SelectBottom = new System.Windows.Forms.Button();
            this.FeatureBottom = new System.Windows.Forms.NumericUpDown();
            this.FeatureTop = new System.Windows.Forms.NumericUpDown();
            this.SelectTop = new System.Windows.Forms.Button();
            this.ReadModel = new System.Windows.Forms.Button();
            this.InfoGainButton = new System.Windows.Forms.Button();
            this.FilterCLBox = new System.Windows.Forms.CheckedListBox();
            this.ClearFilterButton = new System.Windows.Forms.Button();
            this.ReverseFilterButton = new System.Windows.Forms.Button();
            this.SelectAllFilterButton = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.ArguTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.StartTestButton = new System.Windows.Forms.Button();
            this.StartTrainButton = new System.Windows.Forms.Button();
            this.ConsoleWinTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.RawTrainProduceButton = new System.Windows.Forms.Button();
            this.RawDevProduceButton = new System.Windows.Forms.Button();
            this.RawTestProduceButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.DevIdenticalCheckBox = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SaveConfigButton = new System.Windows.Forms.Button();
            this.LoadConfigButton = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.SeriesTrainButton = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.CrossTrainButton = new System.Windows.Forms.Button();
            this.CrossSeriesButton = new System.Windows.Forms.Button();
            this.FolderCountNumeric = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.ThreadCountNumeric = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.MaxCNumeric = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.MinCNumeric = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.MaxJNumeric = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.MinJNumeric = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.StepCNumeric = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.StepJNumeric = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.MaxFolderNumeric = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.MinFolderNumeric = new System.Windows.Forms.NumericUpDown();
            this.StopThreadsButton = new System.Windows.Forms.Button();
            this.PauseResumeButton = new System.Windows.Forms.Button();
            this.RawProduceAllButton = new System.Windows.Forms.Button();
            this.IsPowercCheckBox = new System.Windows.Forms.CheckBox();
            this.OpenPerformanceButton = new System.Windows.Forms.Button();
            this.OpenConfigsButton = new System.Windows.Forms.Button();
            this.OpenPerformanceDirButton = new System.Windows.Forms.Button();
            this.CaseQueryTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CaseSetComboBox = new System.Windows.Forms.ComboBox();
            this.OpenCaseButton = new System.Windows.Forms.Button();
            this.ValueOperationComboBox = new System.Windows.Forms.ComboBox();
            this.StartTest2Button = new System.Windows.Forms.Button();
            this.StartTrainAndTestButton = new System.Windows.Forms.Button();
            this.TrainMethodComboBox = new System.Windows.Forms.ComboBox();
            this.SplitLabelNumeric = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.ConvertFeatureFileButton = new System.Windows.Forms.Button();
            this.ZeroComboBox = new System.Windows.Forms.ComboBox();
            this.LabelFineCoarseCheckBox = new System.Windows.Forms.CheckBox();
            this.ProgressLBar = new TrickyLib.WinControl.ProgressLabelBar();
            this.ModelOpenFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.FeatureDevOpenFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.ConfigTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.CrossValidationOpenFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.FeatureTrainOpenFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.RawDevOpenFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.AdditionalFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.RawTrainOpenFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.MagnifyNumeric = new System.Windows.Forms.NumericUpDown();
            this.MagnifyTextBox = new System.Windows.Forms.TextBox();
            this.MagnifyButton = new System.Windows.Forms.Button();
            this.SplitComboBox = new System.Windows.Forms.ComboBox();
            this.SplitButton = new System.Windows.Forms.Button();
            this.SplitRandomCheckBox = new System.Windows.Forms.CheckBox();
            this.FeatureTestOpenFileListBox = new TrickyLib.WinControl.FileBrowserListBox();
            this.RawTestOpenFileListBox = new TrickyLib.WinControl.FileBrowserListBox();
            this.ReadToMemoryCheckBox = new System.Windows.Forms.CheckBox();
            this.ChangeToDiskCheckBox = new System.Windows.Forms.CheckBox();
            this.DiskComboBox = new System.Windows.Forms.ComboBox();
            this.RawTrainReproduceButton = new System.Windows.Forms.Button();
            this.RawReproduceAllButton = new System.Windows.Forms.Button();
            this.RawDevReproduceButton = new System.Windows.Forms.Button();
            this.RawTestReproduceButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.InfoGainDGView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeatureBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeatureTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FolderCountNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadCountNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxCNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinCNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxJNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinJNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StepCNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StepJNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxFolderNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinFolderNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SplitLabelNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MagnifyNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // FeatureCLBox
            // 
            this.FeatureCLBox.CheckOnClick = true;
            this.FeatureCLBox.FormattingEnabled = true;
            this.FeatureCLBox.Location = new System.Drawing.Point(1105, 54);
            this.FeatureCLBox.Name = "FeatureCLBox";
            this.FeatureCLBox.Size = new System.Drawing.Size(222, 604);
            this.FeatureCLBox.TabIndex = 46;
            // 
            // InfoGainDGView
            // 
            this.InfoGainDGView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InfoGainDGView.Location = new System.Drawing.Point(1333, 428);
            this.InfoGainDGView.Name = "InfoGainDGView";
            this.InfoGainDGView.RowHeadersVisible = false;
            this.InfoGainDGView.Size = new System.Drawing.Size(235, 200);
            this.InfoGainDGView.TabIndex = 47;
            // 
            // Clear
            // 
            this.Clear.Location = new System.Drawing.Point(1284, 29);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(41, 22);
            this.Clear.TabIndex = 50;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // Reverse
            // 
            this.Reverse.Location = new System.Drawing.Point(1241, 29);
            this.Reverse.Name = "Reverse";
            this.Reverse.Size = new System.Drawing.Size(41, 22);
            this.Reverse.TabIndex = 49;
            this.Reverse.Text = "Rev";
            this.Reverse.UseVisualStyleBackColor = true;
            this.Reverse.Click += new System.EventHandler(this.Reverse_Click);
            // 
            // SelectAll
            // 
            this.SelectAll.Location = new System.Drawing.Point(1197, 29);
            this.SelectAll.Name = "SelectAll";
            this.SelectAll.Size = new System.Drawing.Size(41, 22);
            this.SelectAll.TabIndex = 48;
            this.SelectAll.Text = "All";
            this.SelectAll.UseVisualStyleBackColor = true;
            this.SelectAll.Click += new System.EventHandler(this.SelectAll_Click);
            // 
            // SelectBottom
            // 
            this.SelectBottom.Location = new System.Drawing.Point(1516, 635);
            this.SelectBottom.Name = "SelectBottom";
            this.SelectBottom.Size = new System.Drawing.Size(52, 22);
            this.SelectBottom.TabIndex = 54;
            this.SelectBottom.Text = "Bottom";
            this.SelectBottom.UseVisualStyleBackColor = true;
            this.SelectBottom.Click += new System.EventHandler(this.SelectBottom_Click);
            // 
            // FeatureBottom
            // 
            this.FeatureBottom.Location = new System.Drawing.Point(1459, 636);
            this.FeatureBottom.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.FeatureBottom.Name = "FeatureBottom";
            this.FeatureBottom.Size = new System.Drawing.Size(51, 20);
            this.FeatureBottom.TabIndex = 53;
            this.FeatureBottom.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // FeatureTop
            // 
            this.FeatureTop.Location = new System.Drawing.Point(1367, 637);
            this.FeatureTop.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.FeatureTop.Name = "FeatureTop";
            this.FeatureTop.Size = new System.Drawing.Size(46, 20);
            this.FeatureTop.TabIndex = 52;
            this.FeatureTop.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // SelectTop
            // 
            this.SelectTop.Location = new System.Drawing.Point(1419, 636);
            this.SelectTop.Name = "SelectTop";
            this.SelectTop.Size = new System.Drawing.Size(34, 22);
            this.SelectTop.TabIndex = 51;
            this.SelectTop.Text = "Top";
            this.SelectTop.UseVisualStyleBackColor = true;
            this.SelectTop.Click += new System.EventHandler(this.SelectTop_Click);
            // 
            // ReadModel
            // 
            this.ReadModel.Location = new System.Drawing.Point(1475, 399);
            this.ReadModel.Name = "ReadModel";
            this.ReadModel.Size = new System.Drawing.Size(93, 24);
            this.ReadModel.TabIndex = 55;
            this.ReadModel.Text = "Read Model";
            this.ReadModel.UseVisualStyleBackColor = true;
            this.ReadModel.Click += new System.EventHandler(this.ReadModel_Click);
            // 
            // InfoGainButton
            // 
            this.InfoGainButton.Location = new System.Drawing.Point(1384, 400);
            this.InfoGainButton.Name = "InfoGainButton";
            this.InfoGainButton.Size = new System.Drawing.Size(75, 23);
            this.InfoGainButton.TabIndex = 56;
            this.InfoGainButton.Text = "Info Gain";
            this.InfoGainButton.UseVisualStyleBackColor = true;
            this.InfoGainButton.Click += new System.EventHandler(this.InfoGainButton_Click);
            // 
            // FilterCLBox
            // 
            this.FilterCLBox.CheckOnClick = true;
            this.FilterCLBox.FormattingEnabled = true;
            this.FilterCLBox.Location = new System.Drawing.Point(1332, 54);
            this.FilterCLBox.Name = "FilterCLBox";
            this.FilterCLBox.Size = new System.Drawing.Size(236, 334);
            this.FilterCLBox.TabIndex = 57;
            // 
            // ClearFilterButton
            // 
            this.ClearFilterButton.Location = new System.Drawing.Point(1526, 29);
            this.ClearFilterButton.Name = "ClearFilterButton";
            this.ClearFilterButton.Size = new System.Drawing.Size(41, 22);
            this.ClearFilterButton.TabIndex = 60;
            this.ClearFilterButton.Text = "Clear";
            this.ClearFilterButton.UseVisualStyleBackColor = true;
            this.ClearFilterButton.Click += new System.EventHandler(this.ClearFilterButton_Click);
            // 
            // ReverseFilterButton
            // 
            this.ReverseFilterButton.Location = new System.Drawing.Point(1483, 29);
            this.ReverseFilterButton.Name = "ReverseFilterButton";
            this.ReverseFilterButton.Size = new System.Drawing.Size(41, 22);
            this.ReverseFilterButton.TabIndex = 59;
            this.ReverseFilterButton.Text = "Rev";
            this.ReverseFilterButton.UseVisualStyleBackColor = true;
            this.ReverseFilterButton.Click += new System.EventHandler(this.ReverseFilterButton_Click);
            // 
            // SelectAllFilterButton
            // 
            this.SelectAllFilterButton.Location = new System.Drawing.Point(1439, 29);
            this.SelectAllFilterButton.Name = "SelectAllFilterButton";
            this.SelectAllFilterButton.Size = new System.Drawing.Size(41, 22);
            this.SelectAllFilterButton.TabIndex = 58;
            this.SelectAllFilterButton.Text = "All";
            this.SelectAllFilterButton.UseVisualStyleBackColor = true;
            this.SelectAllFilterButton.Click += new System.EventHandler(this.SelectAllFilterButton_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(1105, 35);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(51, 13);
            this.label24.TabIndex = 61;
            this.label24.Text = "Features:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(1332, 36);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(34, 13);
            this.label25.TabIndex = 62;
            this.label25.Text = "Filters";
            // 
            // ArguTextBox
            // 
            this.ArguTextBox.Location = new System.Drawing.Point(92, 464);
            this.ArguTextBox.Name = "ArguTextBox";
            this.ArguTextBox.Size = new System.Drawing.Size(211, 20);
            this.ArguTextBox.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(49, 466);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Argu:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 281);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Test set:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 241);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Dev set:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Train set:";
            // 
            // StartTestButton
            // 
            this.StartTestButton.Location = new System.Drawing.Point(587, 278);
            this.StartTestButton.Name = "StartTestButton";
            this.StartTestButton.Size = new System.Drawing.Size(72, 19);
            this.StartTestButton.TabIndex = 11;
            this.StartTestButton.Text = "Test";
            this.StartTestButton.UseVisualStyleBackColor = true;
            this.StartTestButton.Click += new System.EventHandler(this.StartTestButton_Click);
            // 
            // StartTrainButton
            // 
            this.StartTrainButton.Location = new System.Drawing.Point(589, 193);
            this.StartTrainButton.Name = "StartTrainButton";
            this.StartTrainButton.Size = new System.Drawing.Size(42, 71);
            this.StartTrainButton.TabIndex = 10;
            this.StartTrainButton.Text = "Train";
            this.StartTrainButton.UseVisualStyleBackColor = true;
            this.StartTrainButton.Click += new System.EventHandler(this.StartTrainButton_Click);
            // 
            // ConsoleWinTextBox
            // 
            this.ConsoleWinTextBox.Location = new System.Drawing.Point(840, 54);
            this.ConsoleWinTextBox.Multiline = true;
            this.ConsoleWinTextBox.Name = "ConsoleWinTextBox";
            this.ConsoleWinTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ConsoleWinTextBox.Size = new System.Drawing.Size(259, 476);
            this.ConsoleWinTextBox.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(837, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Console Win";
            // 
            // RawTrainProduceButton
            // 
            this.RawTrainProduceButton.Location = new System.Drawing.Point(672, 70);
            this.RawTrainProduceButton.Name = "RawTrainProduceButton";
            this.RawTrainProduceButton.Size = new System.Drawing.Size(42, 19);
            this.RawTrainProduceButton.TabIndex = 9;
            this.RawTrainProduceButton.Text = "Pro";
            this.RawTrainProduceButton.UseVisualStyleBackColor = true;
            this.RawTrainProduceButton.Click += new System.EventHandler(this.RawTrainProduceButton_Click);
            // 
            // RawDevProduceButton
            // 
            this.RawDevProduceButton.Location = new System.Drawing.Point(672, 110);
            this.RawDevProduceButton.Name = "RawDevProduceButton";
            this.RawDevProduceButton.Size = new System.Drawing.Size(42, 19);
            this.RawDevProduceButton.TabIndex = 8;
            this.RawDevProduceButton.Text = "Pro";
            this.RawDevProduceButton.UseVisualStyleBackColor = true;
            this.RawDevProduceButton.Click += new System.EventHandler(this.RawDevProduceButton_Click);
            // 
            // RawTestProduceButton
            // 
            this.RawTestProduceButton.Location = new System.Drawing.Point(672, 150);
            this.RawTestProduceButton.Name = "RawTestProduceButton";
            this.RawTestProduceButton.Size = new System.Drawing.Size(42, 19);
            this.RawTestProduceButton.TabIndex = 7;
            this.RawTestProduceButton.Text = "Pro";
            this.RawTestProduceButton.UseVisualStyleBackColor = true;
            this.RawTestProduceButton.Click += new System.EventHandler(this.RawTestProduceButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Raw Train:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 114);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Raw Dev:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 154);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Raw Test:";
            // 
            // DevIdenticalCheckBox
            // 
            this.DevIdenticalCheckBox.AutoSize = true;
            this.DevIdenticalCheckBox.Location = new System.Drawing.Point(637, 228);
            this.DevIdenticalCheckBox.Name = "DevIdenticalCheckBox";
            this.DevIdenticalCheckBox.Size = new System.Drawing.Size(66, 17);
            this.DevIdenticalCheckBox.TabIndex = 18;
            this.DevIdenticalCheckBox.Text = "Identical";
            this.DevIdenticalCheckBox.UseVisualStyleBackColor = true;
            this.DevIdenticalCheckBox.CheckedChanged += new System.EventHandler(this.FeatureIdenticalCheckBox_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(42, 389);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Model:";
            // 
            // SaveConfigButton
            // 
            this.SaveConfigButton.Location = new System.Drawing.Point(636, 577);
            this.SaveConfigButton.Name = "SaveConfigButton";
            this.SaveConfigButton.Size = new System.Drawing.Size(53, 27);
            this.SaveConfigButton.TabIndex = 20;
            this.SaveConfigButton.Text = "Save";
            this.SaveConfigButton.UseVisualStyleBackColor = true;
            this.SaveConfigButton.Click += new System.EventHandler(this.SaveConfigButton_Click);
            // 
            // LoadConfigButton
            // 
            this.LoadConfigButton.Location = new System.Drawing.Point(703, 577);
            this.LoadConfigButton.Name = "LoadConfigButton";
            this.LoadConfigButton.Size = new System.Drawing.Size(53, 27);
            this.LoadConfigButton.TabIndex = 20;
            this.LoadConfigButton.Text = "Load";
            this.LoadConfigButton.UseVisualStyleBackColor = true;
            this.LoadConfigButton.Click += new System.EventHandler(this.LoadConfigButton_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 34);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Additional File:";
            // 
            // SeriesTrainButton
            // 
            this.SeriesTrainButton.Location = new System.Drawing.Point(638, 195);
            this.SeriesTrainButton.Name = "SeriesTrainButton";
            this.SeriesTrainButton.Size = new System.Drawing.Size(85, 19);
            this.SeriesTrainButton.TabIndex = 10;
            this.SeriesTrainButton.Text = "Train Series";
            this.SeriesTrainButton.UseVisualStyleBackColor = true;
            this.SeriesTrainButton.Click += new System.EventHandler(this.SeriesTrainButton_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(-1, 430);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 13);
            this.label12.TabIndex = 14;
            this.label12.Text = "CrossValidation:";
            // 
            // CrossTrainButton
            // 
            this.CrossTrainButton.Location = new System.Drawing.Point(587, 429);
            this.CrossTrainButton.Name = "CrossTrainButton";
            this.CrossTrainButton.Size = new System.Drawing.Size(72, 19);
            this.CrossTrainButton.TabIndex = 10;
            this.CrossTrainButton.Text = "Cross Train";
            this.CrossTrainButton.UseVisualStyleBackColor = true;
            this.CrossTrainButton.Click += new System.EventHandler(this.CrossTrainButton_Click);
            // 
            // CrossSeriesButton
            // 
            this.CrossSeriesButton.Location = new System.Drawing.Point(661, 429);
            this.CrossSeriesButton.Name = "CrossSeriesButton";
            this.CrossSeriesButton.Size = new System.Drawing.Size(83, 19);
            this.CrossSeriesButton.TabIndex = 10;
            this.CrossSeriesButton.Text = "Cross Series";
            this.CrossSeriesButton.UseVisualStyleBackColor = true;
            this.CrossSeriesButton.Click += new System.EventHandler(this.CrossSeriesButton_Click);
            // 
            // FolderCountNumeric
            // 
            this.FolderCountNumeric.Location = new System.Drawing.Point(89, 574);
            this.FolderCountNumeric.Name = "FolderCountNumeric";
            this.FolderCountNumeric.Size = new System.Drawing.Size(52, 20);
            this.FolderCountNumeric.TabIndex = 23;
            this.FolderCountNumeric.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 573);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 13);
            this.label13.TabIndex = 14;
            this.label13.Text = "Folder Count:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(524, 506);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 13);
            this.label14.TabIndex = 14;
            this.label14.Text = "Thread Count:";
            // 
            // ThreadCountNumeric
            // 
            this.ThreadCountNumeric.Location = new System.Drawing.Point(599, 502);
            this.ThreadCountNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ThreadCountNumeric.Name = "ThreadCountNumeric";
            this.ThreadCountNumeric.Size = new System.Drawing.Size(52, 20);
            this.ThreadCountNumeric.TabIndex = 23;
            this.ThreadCountNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(199, 506);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(37, 13);
            this.label15.TabIndex = 14;
            this.label15.Text = "MaxC:";
            // 
            // MaxCNumeric
            // 
            this.MaxCNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.MaxCNumeric.Location = new System.Drawing.Point(248, 503);
            this.MaxCNumeric.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.MaxCNumeric.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.MaxCNumeric.Name = "MaxCNumeric";
            this.MaxCNumeric.Size = new System.Drawing.Size(79, 20);
            this.MaxCNumeric.TabIndex = 23;
            this.MaxCNumeric.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(202, 542);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(34, 13);
            this.label16.TabIndex = 14;
            this.label16.Text = "MinC:";
            // 
            // MinCNumeric
            // 
            this.MinCNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.MinCNumeric.Location = new System.Drawing.Point(248, 539);
            this.MinCNumeric.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.MinCNumeric.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.MinCNumeric.Name = "MinCNumeric";
            this.MinCNumeric.Size = new System.Drawing.Size(79, 20);
            this.MinCNumeric.TabIndex = 23;
            this.MinCNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(372, 507);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(35, 13);
            this.label17.TabIndex = 14;
            this.label17.Text = "MaxJ:";
            // 
            // MaxJNumeric
            // 
            this.MaxJNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.MaxJNumeric.Location = new System.Drawing.Point(415, 504);
            this.MaxJNumeric.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.MaxJNumeric.Name = "MaxJNumeric";
            this.MaxJNumeric.Size = new System.Drawing.Size(81, 20);
            this.MaxJNumeric.TabIndex = 23;
            this.MaxJNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(375, 542);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(32, 13);
            this.label18.TabIndex = 14;
            this.label18.Text = "MinJ:";
            // 
            // MinJNumeric
            // 
            this.MinJNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.MinJNumeric.Location = new System.Drawing.Point(415, 539);
            this.MinJNumeric.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.MinJNumeric.Name = "MinJNumeric";
            this.MinJNumeric.Size = new System.Drawing.Size(81, 20);
            this.MinJNumeric.TabIndex = 23;
            this.MinJNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(197, 579);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(39, 13);
            this.label19.TabIndex = 14;
            this.label19.Text = "StepC:";
            // 
            // StepCNumeric
            // 
            this.StepCNumeric.DecimalPlaces = 2;
            this.StepCNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.StepCNumeric.Location = new System.Drawing.Point(248, 576);
            this.StepCNumeric.Name = "StepCNumeric";
            this.StepCNumeric.Size = new System.Drawing.Size(79, 20);
            this.StepCNumeric.TabIndex = 23;
            this.StepCNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(375, 579);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(37, 13);
            this.label20.TabIndex = 14;
            this.label20.Text = "StepJ:";
            // 
            // StepJNumeric
            // 
            this.StepJNumeric.DecimalPlaces = 2;
            this.StepJNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.StepJNumeric.Location = new System.Drawing.Point(415, 576);
            this.StepJNumeric.Name = "StepJNumeric";
            this.StepJNumeric.Size = new System.Drawing.Size(81, 20);
            this.StepJNumeric.TabIndex = 23;
            this.StepJNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(22, 501);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(59, 13);
            this.label21.TabIndex = 14;
            this.label21.Text = "MaxFolder:";
            // 
            // MaxFolderNumeric
            // 
            this.MaxFolderNumeric.Location = new System.Drawing.Point(89, 502);
            this.MaxFolderNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.MaxFolderNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.MaxFolderNumeric.Name = "MaxFolderNumeric";
            this.MaxFolderNumeric.Size = new System.Drawing.Size(52, 20);
            this.MaxFolderNumeric.TabIndex = 23;
            this.MaxFolderNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(25, 537);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(56, 13);
            this.label22.TabIndex = 14;
            this.label22.Text = "MinFolder:";
            // 
            // MinFolderNumeric
            // 
            this.MinFolderNumeric.Location = new System.Drawing.Point(89, 538);
            this.MinFolderNumeric.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.MinFolderNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.MinFolderNumeric.Name = "MinFolderNumeric";
            this.MinFolderNumeric.Size = new System.Drawing.Size(52, 20);
            this.MinFolderNumeric.TabIndex = 23;
            this.MinFolderNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // StopThreadsButton
            // 
            this.StopThreadsButton.Location = new System.Drawing.Point(657, 502);
            this.StopThreadsButton.Name = "StopThreadsButton";
            this.StopThreadsButton.Size = new System.Drawing.Size(47, 19);
            this.StopThreadsButton.TabIndex = 10;
            this.StopThreadsButton.Text = "Stop";
            this.StopThreadsButton.UseVisualStyleBackColor = true;
            this.StopThreadsButton.Click += new System.EventHandler(this.StopThreadsButton_Click);
            // 
            // PauseResumeButton
            // 
            this.PauseResumeButton.Location = new System.Drawing.Point(709, 502);
            this.PauseResumeButton.Name = "PauseResumeButton";
            this.PauseResumeButton.Size = new System.Drawing.Size(47, 19);
            this.PauseResumeButton.TabIndex = 10;
            this.PauseResumeButton.Text = "Pause";
            this.PauseResumeButton.UseVisualStyleBackColor = true;
            this.PauseResumeButton.Click += new System.EventHandler(this.PauseResumeButton_Click);
            // 
            // RawProduceAllButton
            // 
            this.RawProduceAllButton.Location = new System.Drawing.Point(717, 70);
            this.RawProduceAllButton.Name = "RawProduceAllButton";
            this.RawProduceAllButton.Size = new System.Drawing.Size(33, 100);
            this.RawProduceAllButton.TabIndex = 9;
            this.RawProduceAllButton.Text = "Pro All";
            this.RawProduceAllButton.UseVisualStyleBackColor = true;
            this.RawProduceAllButton.Click += new System.EventHandler(this.RawProduceAllButton_Click);
            // 
            // IsPowercCheckBox
            // 
            this.IsPowercCheckBox.AutoSize = true;
            this.IsPowercCheckBox.Checked = true;
            this.IsPowercCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsPowercCheckBox.Location = new System.Drawing.Point(203, 606);
            this.IsPowercCheckBox.Name = "IsPowercCheckBox";
            this.IsPowercCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.IsPowercCheckBox.Size = new System.Drawing.Size(59, 17);
            this.IsPowercCheckBox.TabIndex = 18;
            this.IsPowercCheckBox.Text = ":Power";
            this.IsPowercCheckBox.UseVisualStyleBackColor = true;
            this.IsPowercCheckBox.CheckedChanged += new System.EventHandler(this.IsPowercCheckBox_CheckedChanged);
            // 
            // OpenPerformanceButton
            // 
            this.OpenPerformanceButton.Location = new System.Drawing.Point(985, 24);
            this.OpenPerformanceButton.Name = "OpenPerformanceButton";
            this.OpenPerformanceButton.Size = new System.Drawing.Size(114, 23);
            this.OpenPerformanceButton.TabIndex = 24;
            this.OpenPerformanceButton.Text = "Open Performance";
            this.OpenPerformanceButton.UseVisualStyleBackColor = true;
            this.OpenPerformanceButton.Click += new System.EventHandler(this.OpenPerformanceButton_Click);
            // 
            // OpenConfigsButton
            // 
            this.OpenConfigsButton.Location = new System.Drawing.Point(527, 579);
            this.OpenConfigsButton.Name = "OpenConfigsButton";
            this.OpenConfigsButton.Size = new System.Drawing.Size(100, 23);
            this.OpenConfigsButton.TabIndex = 25;
            this.OpenConfigsButton.Text = "Open Configs";
            this.OpenConfigsButton.UseVisualStyleBackColor = true;
            this.OpenConfigsButton.Click += new System.EventHandler(this.OpenConfigsButton_Click);
            // 
            // OpenPerformanceDirButton
            // 
            this.OpenPerformanceDirButton.Location = new System.Drawing.Point(906, 25);
            this.OpenPerformanceDirButton.Name = "OpenPerformanceDirButton";
            this.OpenPerformanceDirButton.Size = new System.Drawing.Size(75, 23);
            this.OpenPerformanceDirButton.TabIndex = 26;
            this.OpenPerformanceDirButton.Text = "Open Dir";
            this.OpenPerformanceDirButton.UseVisualStyleBackColor = true;
            this.OpenPerformanceDirButton.Click += new System.EventHandler(this.OpenPerformanceDirButton_Click);
            // 
            // CaseQueryTextBox
            // 
            this.CaseQueryTextBox.Location = new System.Drawing.Point(89, 636);
            this.CaseQueryTextBox.Name = "CaseQueryTextBox";
            this.CaseQueryTextBox.Size = new System.Drawing.Size(123, 20);
            this.CaseQueryTextBox.TabIndex = 28;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 637);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Case Query:";
            // 
            // CaseSetComboBox
            // 
            this.CaseSetComboBox.FormattingEnabled = true;
            this.CaseSetComboBox.Items.AddRange(new object[] {
            "Train",
            "Develpment",
            "Test"});
            this.CaseSetComboBox.Location = new System.Drawing.Point(223, 636);
            this.CaseSetComboBox.Name = "CaseSetComboBox";
            this.CaseSetComboBox.Size = new System.Drawing.Size(121, 21);
            this.CaseSetComboBox.TabIndex = 29;
            // 
            // OpenCaseButton
            // 
            this.OpenCaseButton.Location = new System.Drawing.Point(354, 636);
            this.OpenCaseButton.Name = "OpenCaseButton";
            this.OpenCaseButton.Size = new System.Drawing.Size(75, 23);
            this.OpenCaseButton.TabIndex = 30;
            this.OpenCaseButton.Text = "Open Case";
            this.OpenCaseButton.UseVisualStyleBackColor = true;
            this.OpenCaseButton.Click += new System.EventHandler(this.OpenCaseButton_Click);
            // 
            // ValueOperationComboBox
            // 
            this.ValueOperationComboBox.FormattingEnabled = true;
            this.ValueOperationComboBox.Items.AddRange(new object[] {
            "None",
            "Discrete",
            "Normalize"});
            this.ValueOperationComboBox.Location = new System.Drawing.Point(587, 44);
            this.ValueOperationComboBox.Name = "ValueOperationComboBox";
            this.ValueOperationComboBox.Size = new System.Drawing.Size(69, 21);
            this.ValueOperationComboBox.TabIndex = 32;
            this.ValueOperationComboBox.Text = "None";
            // 
            // StartTest2Button
            // 
            this.StartTest2Button.Location = new System.Drawing.Point(705, 223);
            this.StartTest2Button.Name = "StartTest2Button";
            this.StartTest2Button.Size = new System.Drawing.Size(18, 76);
            this.StartTest2Button.TabIndex = 33;
            this.StartTest2Button.Text = "Test";
            this.StartTest2Button.UseVisualStyleBackColor = true;
            this.StartTest2Button.Click += new System.EventHandler(this.StartTest2Button_Click);
            // 
            // StartTrainAndTestButton
            // 
            this.StartTrainAndTestButton.Location = new System.Drawing.Point(728, 198);
            this.StartTrainAndTestButton.Name = "StartTrainAndTestButton";
            this.StartTrainAndTestButton.Size = new System.Drawing.Size(22, 101);
            this.StartTrainAndTestButton.TabIndex = 34;
            this.StartTrainAndTestButton.Text = "All";
            this.StartTrainAndTestButton.UseVisualStyleBackColor = true;
            this.StartTrainAndTestButton.Click += new System.EventHandler(this.StartTrainAndTestButton_Click);
            // 
            // TrainMethodComboBox
            // 
            this.TrainMethodComboBox.FormattingEnabled = true;
            this.TrainMethodComboBox.Items.AddRange(new object[] {
            "SVM",
            "RankSVM",
            "FastRankSvm"});
            this.TrainMethodComboBox.Location = new System.Drawing.Point(309, 463);
            this.TrainMethodComboBox.Name = "TrainMethodComboBox";
            this.TrainMethodComboBox.Size = new System.Drawing.Size(74, 21);
            this.TrainMethodComboBox.TabIndex = 36;
            this.TrainMethodComboBox.Text = "Svm";
            this.TrainMethodComboBox.SelectedIndexChanged += new System.EventHandler(this.TrainMethodComboBox_SelectedIndexChanged);
            // 
            // SplitLabelNumeric
            // 
            this.SplitLabelNumeric.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.SplitLabelNumeric.Location = new System.Drawing.Point(785, 74);
            this.SplitLabelNumeric.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.SplitLabelNumeric.Name = "SplitLabelNumeric";
            this.SplitLabelNumeric.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SplitLabelNumeric.Size = new System.Drawing.Size(52, 20);
            this.SplitLabelNumeric.TabIndex = 37;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(781, 57);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(56, 13);
            this.label23.TabIndex = 12;
            this.label23.Text = "Split Label";
            // 
            // ConvertFeatureFileButton
            // 
            this.ConvertFeatureFileButton.Location = new System.Drawing.Point(764, 144);
            this.ConvertFeatureFileButton.Name = "ConvertFeatureFileButton";
            this.ConvertFeatureFileButton.Size = new System.Drawing.Size(75, 23);
            this.ConvertFeatureFileButton.TabIndex = 39;
            this.ConvertFeatureFileButton.Text = "Convert";
            this.ConvertFeatureFileButton.UseVisualStyleBackColor = true;
            this.ConvertFeatureFileButton.Click += new System.EventHandler(this.ConvertFeatureFileButton_Click);
            // 
            // ZeroComboBox
            // 
            this.ZeroComboBox.FormattingEnabled = true;
            this.ZeroComboBox.Items.AddRange(new object[] {
            "1",
            "-1",
            "0",
            "Remove"});
            this.ZeroComboBox.Location = new System.Drawing.Point(768, 98);
            this.ZeroComboBox.Name = "ZeroComboBox";
            this.ZeroComboBox.Size = new System.Drawing.Size(70, 21);
            this.ZeroComboBox.TabIndex = 40;
            this.ZeroComboBox.Text = "ZeroOperation";
            // 
            // LabelFineCoarseCheckBox
            // 
            this.LabelFineCoarseCheckBox.AutoSize = true;
            this.LabelFineCoarseCheckBox.Location = new System.Drawing.Point(776, 124);
            this.LabelFineCoarseCheckBox.Name = "LabelFineCoarseCheckBox";
            this.LabelFineCoarseCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.LabelFineCoarseCheckBox.Size = new System.Drawing.Size(59, 17);
            this.LabelFineCoarseCheckBox.TabIndex = 41;
            this.LabelFineCoarseCheckBox.Text = "Coarse";
            this.LabelFineCoarseCheckBox.UseVisualStyleBackColor = true;
            // 
            // ProgressLBar
            // 
            this.ProgressLBar.Location = new System.Drawing.Point(771, 574);
            this.ProgressLBar.Name = "ProgressLBar";
            this.ProgressLBar.ProcessingText = "";
            this.ProgressLBar.ProgressMaximum = 100;
            this.ProgressLBar.ProgressPercentage = 0D;
            this.ProgressLBar.ProgressValue = 0;
            this.ProgressLBar.Size = new System.Drawing.Size(289, 22);
            this.ProgressLBar.TabIndex = 35;
            // 
            // ModelOpenFileTextBox
            // 
            this.ModelOpenFileTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.ModelOpenFileTextBox.FilePath = "";
            this.ModelOpenFileTextBox.Filter = null;
            this.ModelOpenFileTextBox.InitialDirectory = null;
            this.ModelOpenFileTextBox.Location = new System.Drawing.Point(79, 380);
            this.ModelOpenFileTextBox.Name = "ModelOpenFileTextBox";
            this.ModelOpenFileTextBox.OnlyShowFileName = true;
            this.ModelOpenFileTextBox.Size = new System.Drawing.Size(504, 34);
            this.ModelOpenFileTextBox.TabIndex = 19;
            // 
            // FeatureDevOpenFileTextBox
            // 
            this.FeatureDevOpenFileTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.FeatureDevOpenFileTextBox.FilePath = "";
            this.FeatureDevOpenFileTextBox.Filter = null;
            this.FeatureDevOpenFileTextBox.InitialDirectory = null;
            this.FeatureDevOpenFileTextBox.Location = new System.Drawing.Point(79, 230);
            this.FeatureDevOpenFileTextBox.Name = "FeatureDevOpenFileTextBox";
            this.FeatureDevOpenFileTextBox.OnlyShowFileName = true;
            this.FeatureDevOpenFileTextBox.Size = new System.Drawing.Size(504, 34);
            this.FeatureDevOpenFileTextBox.TabIndex = 19;
            // 
            // ConfigTextBox
            // 
            this.ConfigTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.ConfigTextBox.FilePath = "";
            this.ConfigTextBox.Filter = "";
            this.ConfigTextBox.InitialDirectory = null;
            this.ConfigTextBox.Location = new System.Drawing.Point(527, 536);
            this.ConfigTextBox.Name = "ConfigTextBox";
            this.ConfigTextBox.OnlyShowFileName = true;
            this.ConfigTextBox.Size = new System.Drawing.Size(504, 34);
            this.ConfigTextBox.TabIndex = 19;
            // 
            // CrossValidationOpenFileTextBox
            // 
            this.CrossValidationOpenFileTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.CrossValidationOpenFileTextBox.FilePath = "";
            this.CrossValidationOpenFileTextBox.Filter = null;
            this.CrossValidationOpenFileTextBox.InitialDirectory = null;
            this.CrossValidationOpenFileTextBox.Location = new System.Drawing.Point(79, 421);
            this.CrossValidationOpenFileTextBox.Name = "CrossValidationOpenFileTextBox";
            this.CrossValidationOpenFileTextBox.OnlyShowFileName = true;
            this.CrossValidationOpenFileTextBox.Size = new System.Drawing.Size(504, 34);
            this.CrossValidationOpenFileTextBox.TabIndex = 19;
            // 
            // FeatureTrainOpenFileTextBox
            // 
            this.FeatureTrainOpenFileTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.FeatureTrainOpenFileTextBox.FilePath = "";
            this.FeatureTrainOpenFileTextBox.Filter = null;
            this.FeatureTrainOpenFileTextBox.InitialDirectory = null;
            this.FeatureTrainOpenFileTextBox.Location = new System.Drawing.Point(79, 190);
            this.FeatureTrainOpenFileTextBox.Name = "FeatureTrainOpenFileTextBox";
            this.FeatureTrainOpenFileTextBox.OnlyShowFileName = true;
            this.FeatureTrainOpenFileTextBox.Size = new System.Drawing.Size(504, 34);
            this.FeatureTrainOpenFileTextBox.TabIndex = 19;
            // 
            // RawDevOpenFileTextBox
            // 
            this.RawDevOpenFileTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.RawDevOpenFileTextBox.FilePath = "";
            this.RawDevOpenFileTextBox.Filter = null;
            this.RawDevOpenFileTextBox.InitialDirectory = null;
            this.RawDevOpenFileTextBox.Location = new System.Drawing.Point(79, 103);
            this.RawDevOpenFileTextBox.Name = "RawDevOpenFileTextBox";
            this.RawDevOpenFileTextBox.OnlyShowFileName = true;
            this.RawDevOpenFileTextBox.Size = new System.Drawing.Size(504, 34);
            this.RawDevOpenFileTextBox.TabIndex = 19;
            // 
            // AdditionalFileTextBox
            // 
            this.AdditionalFileTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Save;
            this.AdditionalFileTextBox.FilePath = "";
            this.AdditionalFileTextBox.Filter = null;
            this.AdditionalFileTextBox.InitialDirectory = null;
            this.AdditionalFileTextBox.Location = new System.Drawing.Point(79, 23);
            this.AdditionalFileTextBox.Name = "AdditionalFileTextBox";
            this.AdditionalFileTextBox.OnlyShowFileName = true;
            this.AdditionalFileTextBox.Size = new System.Drawing.Size(504, 34);
            this.AdditionalFileTextBox.TabIndex = 19;
            // 
            // RawTrainOpenFileTextBox
            // 
            this.RawTrainOpenFileTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.RawTrainOpenFileTextBox.FilePath = "";
            this.RawTrainOpenFileTextBox.Filter = null;
            this.RawTrainOpenFileTextBox.InitialDirectory = null;
            this.RawTrainOpenFileTextBox.Location = new System.Drawing.Point(79, 63);
            this.RawTrainOpenFileTextBox.Name = "RawTrainOpenFileTextBox";
            this.RawTrainOpenFileTextBox.OnlyShowFileName = true;
            this.RawTrainOpenFileTextBox.Size = new System.Drawing.Size(504, 34);
            this.RawTrainOpenFileTextBox.TabIndex = 19;
            // 
            // MagnifyNumeric
            // 
            this.MagnifyNumeric.Location = new System.Drawing.Point(785, 305);
            this.MagnifyNumeric.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.MagnifyNumeric.Name = "MagnifyNumeric";
            this.MagnifyNumeric.Size = new System.Drawing.Size(52, 20);
            this.MagnifyNumeric.TabIndex = 42;
            this.MagnifyNumeric.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // MagnifyTextBox
            // 
            this.MagnifyTextBox.Location = new System.Drawing.Point(782, 330);
            this.MagnifyTextBox.Name = "MagnifyTextBox";
            this.MagnifyTextBox.Size = new System.Drawing.Size(55, 20);
            this.MagnifyTextBox.TabIndex = 43;
            // 
            // MagnifyButton
            // 
            this.MagnifyButton.Location = new System.Drawing.Point(781, 356);
            this.MagnifyButton.Name = "MagnifyButton";
            this.MagnifyButton.Size = new System.Drawing.Size(55, 23);
            this.MagnifyButton.TabIndex = 44;
            this.MagnifyButton.Text = "Magnify";
            this.MagnifyButton.UseVisualStyleBackColor = true;
            this.MagnifyButton.Click += new System.EventHandler(this.MagnifyButton_Click);
            // 
            // SplitComboBox
            // 
            this.SplitComboBox.FormattingEnabled = true;
            this.SplitComboBox.Items.AddRange(new object[] {
            "2:1:0",
            "1:1:0",
            "2:0:1",
            "1:0:1",
            "2:1:1",
            "1:1:1",
            "0:1:1",
            "0:1:2",
            "0:2:1"});
            this.SplitComboBox.Location = new System.Drawing.Point(768, 228);
            this.SplitComboBox.Name = "SplitComboBox";
            this.SplitComboBox.Size = new System.Drawing.Size(71, 21);
            this.SplitComboBox.TabIndex = 45;
            this.SplitComboBox.Text = "TDT Ratio";
            // 
            // SplitButton
            // 
            this.SplitButton.Location = new System.Drawing.Point(776, 201);
            this.SplitButton.Name = "SplitButton";
            this.SplitButton.Size = new System.Drawing.Size(62, 23);
            this.SplitButton.TabIndex = 46;
            this.SplitButton.Text = "Split";
            this.SplitButton.UseVisualStyleBackColor = true;
            this.SplitButton.Click += new System.EventHandler(this.SplitButton_Click);
            // 
            // SplitRandomCheckBox
            // 
            this.SplitRandomCheckBox.AutoSize = true;
            this.SplitRandomCheckBox.Location = new System.Drawing.Point(773, 255);
            this.SplitRandomCheckBox.Name = "SplitRandomCheckBox";
            this.SplitRandomCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SplitRandomCheckBox.Size = new System.Drawing.Size(66, 17);
            this.SplitRandomCheckBox.TabIndex = 47;
            this.SplitRandomCheckBox.Text = "Random";
            this.SplitRandomCheckBox.UseVisualStyleBackColor = true;
            // 
            // FeatureTestOpenFileListBox
            // 
            this.FeatureTestOpenFileListBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.FeatureTestOpenFileListBox.Files = ((System.Collections.Generic.List<string>)(resources.GetObject("FeatureTestOpenFileListBox.Files")));
            this.FeatureTestOpenFileListBox.Filter = null;
            this.FeatureTestOpenFileListBox.InitialDirectory = null;
            this.FeatureTestOpenFileListBox.Location = new System.Drawing.Point(83, 270);
            this.FeatureTestOpenFileListBox.Name = "FeatureTestOpenFileListBox";
            this.FeatureTestOpenFileListBox.OnlyShowFileName = true;
            this.FeatureTestOpenFileListBox.Size = new System.Drawing.Size(500, 109);
            this.FeatureTestOpenFileListBox.TabIndex = 63;
            // 
            // RawTestOpenFileListBox
            // 
            this.RawTestOpenFileListBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.RawTestOpenFileListBox.Files = ((System.Collections.Generic.List<string>)(resources.GetObject("RawTestOpenFileListBox.Files")));
            this.RawTestOpenFileListBox.Filter = null;
            this.RawTestOpenFileListBox.InitialDirectory = null;
            this.RawTestOpenFileListBox.Location = new System.Drawing.Point(83, 140);
            this.RawTestOpenFileListBox.Name = "RawTestOpenFileListBox";
            this.RawTestOpenFileListBox.OnlyShowFileName = true;
            this.RawTestOpenFileListBox.Size = new System.Drawing.Size(500, 45);
            this.RawTestOpenFileListBox.TabIndex = 64;
            // 
            // ReadToMemoryCheckBox
            // 
            this.ReadToMemoryCheckBox.AutoSize = true;
            this.ReadToMemoryCheckBox.Location = new System.Drawing.Point(495, 465);
            this.ReadToMemoryCheckBox.Name = "ReadToMemoryCheckBox";
            this.ReadToMemoryCheckBox.Size = new System.Drawing.Size(102, 17);
            this.ReadToMemoryCheckBox.TabIndex = 65;
            this.ReadToMemoryCheckBox.Text = "ReadToMemory";
            this.ReadToMemoryCheckBox.UseVisualStyleBackColor = true;
            // 
            // ChangeToDiskCheckBox
            // 
            this.ChangeToDiskCheckBox.AutoSize = true;
            this.ChangeToDiskCheckBox.Location = new System.Drawing.Point(83, 5);
            this.ChangeToDiskCheckBox.Name = "ChangeToDiskCheckBox";
            this.ChangeToDiskCheckBox.Size = new System.Drawing.Size(97, 17);
            this.ChangeToDiskCheckBox.TabIndex = 66;
            this.ChangeToDiskCheckBox.Text = "ChangeToDisk";
            this.ChangeToDiskCheckBox.UseVisualStyleBackColor = true;
            this.ChangeToDiskCheckBox.CheckedChanged += new System.EventHandler(this.ChangeToDiskCheckBox_CheckedChanged);
            // 
            // DiskComboBox
            // 
            this.DiskComboBox.FormattingEnabled = true;
            this.DiskComboBox.Items.AddRange(new object[] {
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z"});
            this.DiskComboBox.Location = new System.Drawing.Point(186, 3);
            this.DiskComboBox.Name = "DiskComboBox";
            this.DiskComboBox.Size = new System.Drawing.Size(38, 21);
            this.DiskComboBox.TabIndex = 67;
            // 
            // RawTrainReproduceButton
            // 
            this.RawTrainReproduceButton.Location = new System.Drawing.Point(588, 70);
            this.RawTrainReproduceButton.Name = "RawTrainReproduceButton";
            this.RawTrainReproduceButton.Size = new System.Drawing.Size(42, 19);
            this.RawTrainReproduceButton.TabIndex = 9;
            this.RawTrainReproduceButton.Text = "RePro";
            this.RawTrainReproduceButton.UseVisualStyleBackColor = true;
            this.RawTrainReproduceButton.Click += new System.EventHandler(this.RawTrainReproduceButton_Click);
            // 
            // RawReproduceAllButton
            // 
            this.RawReproduceAllButton.Location = new System.Drawing.Point(633, 70);
            this.RawReproduceAllButton.Name = "RawReproduceAllButton";
            this.RawReproduceAllButton.Size = new System.Drawing.Size(33, 100);
            this.RawReproduceAllButton.TabIndex = 9;
            this.RawReproduceAllButton.Text = "RePro All";
            this.RawReproduceAllButton.UseVisualStyleBackColor = true;
            this.RawReproduceAllButton.Click += new System.EventHandler(this.RawReproduceAllButton_Click);
            // 
            // RawDevReproduceButton
            // 
            this.RawDevReproduceButton.Location = new System.Drawing.Point(588, 110);
            this.RawDevReproduceButton.Name = "RawDevReproduceButton";
            this.RawDevReproduceButton.Size = new System.Drawing.Size(42, 19);
            this.RawDevReproduceButton.TabIndex = 8;
            this.RawDevReproduceButton.Text = "RePro";
            this.RawDevReproduceButton.UseVisualStyleBackColor = true;
            this.RawDevReproduceButton.Click += new System.EventHandler(this.RawDevReproduceButton_Click);
            // 
            // RawTestReproduceButton
            // 
            this.RawTestReproduceButton.Location = new System.Drawing.Point(588, 150);
            this.RawTestReproduceButton.Name = "RawTestReproduceButton";
            this.RawTestReproduceButton.Size = new System.Drawing.Size(42, 19);
            this.RawTestReproduceButton.TabIndex = 7;
            this.RawTestReproduceButton.Text = "RePro";
            this.RawTestReproduceButton.UseVisualStyleBackColor = true;
            this.RawTestReproduceButton.Click += new System.EventHandler(this.RawTestReproduceButton_Click);
            // 
            // SelectSvmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1570, 663);
            this.Controls.Add(this.DiskComboBox);
            this.Controls.Add(this.ChangeToDiskCheckBox);
            this.Controls.Add(this.ReadToMemoryCheckBox);
            this.Controls.Add(this.RawTestOpenFileListBox);
            this.Controls.Add(this.FeatureTestOpenFileListBox);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.ClearFilterButton);
            this.Controls.Add(this.ReverseFilterButton);
            this.Controls.Add(this.SelectAllFilterButton);
            this.Controls.Add(this.FilterCLBox);
            this.Controls.Add(this.InfoGainButton);
            this.Controls.Add(this.ReadModel);
            this.Controls.Add(this.SelectBottom);
            this.Controls.Add(this.FeatureBottom);
            this.Controls.Add(this.FeatureTop);
            this.Controls.Add(this.SelectTop);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.Reverse);
            this.Controls.Add(this.SelectAll);
            this.Controls.Add(this.AdditionalFileTextBox);
            this.Controls.Add(this.InfoGainDGView);
            this.Controls.Add(this.FeatureCLBox);
            this.Controls.Add(this.SplitRandomCheckBox);
            this.Controls.Add(this.SplitButton);
            this.Controls.Add(this.SplitComboBox);
            this.Controls.Add(this.MagnifyButton);
            this.Controls.Add(this.MagnifyTextBox);
            this.Controls.Add(this.MagnifyNumeric);
            this.Controls.Add(this.LabelFineCoarseCheckBox);
            this.Controls.Add(this.ZeroComboBox);
            this.Controls.Add(this.ConvertFeatureFileButton);
            this.Controls.Add(this.SplitLabelNumeric);
            this.Controls.Add(this.TrainMethodComboBox);
            this.Controls.Add(this.ProgressLBar);
            this.Controls.Add(this.StartTrainAndTestButton);
            this.Controls.Add(this.StartTest2Button);
            this.Controls.Add(this.ValueOperationComboBox);
            this.Controls.Add(this.OpenCaseButton);
            this.Controls.Add(this.CaseSetComboBox);
            this.Controls.Add(this.CaseQueryTextBox);
            this.Controls.Add(this.OpenPerformanceDirButton);
            this.Controls.Add(this.OpenConfigsButton);
            this.Controls.Add(this.OpenPerformanceButton);
            this.Controls.Add(this.ThreadCountNumeric);
            this.Controls.Add(this.StepJNumeric);
            this.Controls.Add(this.MinJNumeric);
            this.Controls.Add(this.MaxJNumeric);
            this.Controls.Add(this.StepCNumeric);
            this.Controls.Add(this.MinCNumeric);
            this.Controls.Add(this.MaxCNumeric);
            this.Controls.Add(this.MinFolderNumeric);
            this.Controls.Add(this.MaxFolderNumeric);
            this.Controls.Add(this.FolderCountNumeric);
            this.Controls.Add(this.IsPowercCheckBox);
            this.Controls.Add(this.DevIdenticalCheckBox);
            this.Controls.Add(this.StartTestButton);
            this.Controls.Add(this.PauseResumeButton);
            this.Controls.Add(this.StopThreadsButton);
            this.Controls.Add(this.CrossSeriesButton);
            this.Controls.Add(this.SeriesTrainButton);
            this.Controls.Add(this.CrossTrainButton);
            this.Controls.Add(this.StartTrainButton);
            this.Controls.Add(this.RawTestReproduceButton);
            this.Controls.Add(this.RawTestProduceButton);
            this.Controls.Add(this.RawDevReproduceButton);
            this.Controls.Add(this.RawDevProduceButton);
            this.Controls.Add(this.RawReproduceAllButton);
            this.Controls.Add(this.RawTrainReproduceButton);
            this.Controls.Add(this.RawProduceAllButton);
            this.Controls.Add(this.RawTrainProduceButton);
            this.Controls.Add(this.LoadConfigButton);
            this.Controls.Add(this.SaveConfigButton);
            this.Controls.Add(this.ModelOpenFileTextBox);
            this.Controls.Add(this.FeatureDevOpenFileTextBox);
            this.Controls.Add(this.ConfigTextBox);
            this.Controls.Add(this.CrossValidationOpenFileTextBox);
            this.Controls.Add(this.FeatureTrainOpenFileTextBox);
            this.Controls.Add(this.RawDevOpenFileTextBox);
            this.Controls.Add(this.RawTrainOpenFileTextBox);
            this.Controls.Add(this.ConsoleWinTextBox);
            this.Controls.Add(this.ArguTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Name = "SelectSvmForm";
            this.Text = "SelectSvmForm";
            ((System.ComponentModel.ISupportInitialize)(this.InfoGainDGView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeatureBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeatureTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FolderCountNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadCountNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxCNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinCNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxJNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinJNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StepCNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StepJNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxFolderNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinFolderNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SplitLabelNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MagnifyNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox FeatureCLBox;
        private System.Windows.Forms.DataGridView InfoGainDGView;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.Button Reverse;
        private System.Windows.Forms.Button SelectAll;
        private System.Windows.Forms.Button SelectBottom;
        private System.Windows.Forms.NumericUpDown FeatureBottom;
        private System.Windows.Forms.NumericUpDown FeatureTop;
        private System.Windows.Forms.Button SelectTop;
        private System.Windows.Forms.Button ReadModel;
        private System.Windows.Forms.Button InfoGainButton;
        private System.Windows.Forms.CheckedListBox FilterCLBox;
        private System.Windows.Forms.Button ClearFilterButton;
        private System.Windows.Forms.Button ReverseFilterButton;
        private System.Windows.Forms.Button SelectAllFilterButton;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox ArguTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartTestButton;
        private System.Windows.Forms.Button StartTrainButton;
        private System.Windows.Forms.TextBox ConsoleWinTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button RawTrainProduceButton;
        private System.Windows.Forms.Button RawDevProduceButton;
        private System.Windows.Forms.Button RawTestProduceButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox DevIdenticalCheckBox;
        private FileBrowserTextBox RawTrainOpenFileTextBox;
        private FileBrowserTextBox RawDevOpenFileTextBox;
        private FileBrowserTextBox FeatureTrainOpenFileTextBox;
        private FileBrowserTextBox FeatureDevOpenFileTextBox;
        private Label label10;
        private FileBrowserTextBox ModelOpenFileTextBox;
        private Button SaveConfigButton;
        private Button LoadConfigButton;
        private Label label11;
        protected FileBrowserTextBox AdditionalFileTextBox;
        private Button SeriesTrainButton;
        private Label label12;
        private FileBrowserTextBox CrossValidationOpenFileTextBox;
        private Button CrossTrainButton;
        private Button CrossSeriesButton;
        private NumericUpDown FolderCountNumeric;
        private Label label13;
        private Label label14;
        private NumericUpDown ThreadCountNumeric;
        private Label label15;
        private NumericUpDown MaxCNumeric;
        private Label label16;
        private NumericUpDown MinCNumeric;
        private Label label17;
        private NumericUpDown MaxJNumeric;
        private Label label18;
        private NumericUpDown MinJNumeric;
        private Label label19;
        private NumericUpDown StepCNumeric;
        private Label label20;
        private NumericUpDown StepJNumeric;
        private Label label21;
        private NumericUpDown MaxFolderNumeric;
        private Label label22;
        private NumericUpDown MinFolderNumeric;
        private Button StopThreadsButton;
        private Button PauseResumeButton;
        private Button RawProduceAllButton;
        private CheckBox IsPowercCheckBox;
        private Button OpenPerformanceButton;
        private Button OpenConfigsButton;
        private FileBrowserTextBox ConfigTextBox;
        private Button OpenPerformanceDirButton;
        private TextBox CaseQueryTextBox;
        private Label label6;
        private ComboBox CaseSetComboBox;
        private Button OpenCaseButton;
        private ComboBox ValueOperationComboBox;
        private Button StartTest2Button;
        private Button StartTrainAndTestButton;
        private ProgressLabelBar ProgressLBar;
        private ComboBox TrainMethodComboBox;
        private NumericUpDown SplitLabelNumeric;
        private Label label23;
        private Button ConvertFeatureFileButton;
        private ComboBox ZeroComboBox;
        private CheckBox LabelFineCoarseCheckBox;
        private NumericUpDown MagnifyNumeric;
        private TextBox MagnifyTextBox;
        private Button MagnifyButton;
        private ComboBox SplitComboBox;
        private Button SplitButton;
        private CheckBox SplitRandomCheckBox;
        private FileBrowserListBox FeatureTestOpenFileListBox;
        private FileBrowserListBox RawTestOpenFileListBox;
        private CheckBox ReadToMemoryCheckBox;
        private CheckBox ChangeToDiskCheckBox;
        private ComboBox DiskComboBox;
        private Button RawTrainReproduceButton;
        private Button RawReproduceAllButton;
        private Button RawDevReproduceButton;
        private Button RawTestReproduceButton;
    }
}