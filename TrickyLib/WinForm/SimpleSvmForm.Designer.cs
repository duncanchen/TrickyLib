using System.Windows.Forms;
using TrickyLib.WinControl;
namespace TrickyLib.WinForm
{
    partial class SimpleSvmForm : Form
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
            this.LoadAdditionalFileButton = new System.Windows.Forms.Button();
            this.SaveAdditionalButton = new System.Windows.Forms.Button();
            this.RawProduceAllButton = new System.Windows.Forms.Button();
            this.RankSvmCheckBox = new System.Windows.Forms.CheckBox();
            this.FastRankLearnCheckBox = new System.Windows.Forms.CheckBox();
            this.IsPowercCheckBox = new System.Windows.Forms.CheckBox();
            this.OpenPerformanceButton = new System.Windows.Forms.Button();
            this.OpenConfigsButton = new System.Windows.Forms.Button();
            this.OpenPerformanceDirButton = new System.Windows.Forms.Button();
            this.CaseQueryTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CaseSetComboBox = new System.Windows.Forms.ComboBox();
            this.OpenCaseButton = new System.Windows.Forms.Button();
            this.NormalizeCheckBox = new System.Windows.Forms.CheckBox();
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
            this.FeatureTestOpenFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.FeatureDevOpenFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.ConfigTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.CrossValidationOpenFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.FeatureTrainOpenFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.RawTestOpenFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.RawDevOpenFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.AdditionalFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.RawTrainOpenFileTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.MagnifyNumeric = new System.Windows.Forms.NumericUpDown();
            this.MagnifyTextBox = new System.Windows.Forms.TextBox();
            this.MagnifyButton = new System.Windows.Forms.Button();
            this.SplitComboBox = new System.Windows.Forms.ComboBox();
            this.SplitButton = new System.Windows.Forms.Button();
            this.SplitRandomCheckBox = new System.Windows.Forms.CheckBox();
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
            // ArguTextBox
            // 
            this.ArguTextBox.Location = new System.Drawing.Point(92, 394);
            this.ArguTextBox.Name = "ArguTextBox";
            this.ArguTextBox.Size = new System.Drawing.Size(211, 20);
            this.ArguTextBox.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 397);
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
            this.StartTestButton.Location = new System.Drawing.Point(599, 278);
            this.StartTestButton.Name = "StartTestButton";
            this.StartTestButton.Size = new System.Drawing.Size(72, 19);
            this.StartTestButton.TabIndex = 11;
            this.StartTestButton.Text = "Test";
            this.StartTestButton.UseVisualStyleBackColor = true;
            this.StartTestButton.Click += new System.EventHandler(this.StartTestButton_Click);
            // 
            // StartTrainButton
            // 
            this.StartTrainButton.Location = new System.Drawing.Point(601, 193);
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
            this.ConsoleWinTextBox.Size = new System.Drawing.Size(259, 400);
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
            this.RawTrainProduceButton.Location = new System.Drawing.Point(599, 71);
            this.RawTrainProduceButton.Name = "RawTrainProduceButton";
            this.RawTrainProduceButton.Size = new System.Drawing.Size(72, 19);
            this.RawTrainProduceButton.TabIndex = 9;
            this.RawTrainProduceButton.Text = "Produce";
            this.RawTrainProduceButton.UseVisualStyleBackColor = true;
            this.RawTrainProduceButton.Click += new System.EventHandler(this.RawTrainProduceButton_Click);
            // 
            // RawDevProduceButton
            // 
            this.RawDevProduceButton.Location = new System.Drawing.Point(599, 111);
            this.RawDevProduceButton.Name = "RawDevProduceButton";
            this.RawDevProduceButton.Size = new System.Drawing.Size(72, 19);
            this.RawDevProduceButton.TabIndex = 8;
            this.RawDevProduceButton.Text = "Produce";
            this.RawDevProduceButton.UseVisualStyleBackColor = true;
            this.RawDevProduceButton.Click += new System.EventHandler(this.RawDevProduceButton_Click);
            // 
            // RawTestProduceButton
            // 
            this.RawTestProduceButton.Location = new System.Drawing.Point(599, 151);
            this.RawTestProduceButton.Name = "RawTestProduceButton";
            this.RawTestProduceButton.Size = new System.Drawing.Size(72, 19);
            this.RawTestProduceButton.TabIndex = 7;
            this.RawTestProduceButton.Text = "Produce";
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
            this.DevIdenticalCheckBox.Location = new System.Drawing.Point(649, 228);
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
            this.label10.Location = new System.Drawing.Point(42, 321);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Model:";
            // 
            // SaveConfigButton
            // 
            this.SaveConfigButton.Location = new System.Drawing.Point(636, 507);
            this.SaveConfigButton.Name = "SaveConfigButton";
            this.SaveConfigButton.Size = new System.Drawing.Size(53, 27);
            this.SaveConfigButton.TabIndex = 20;
            this.SaveConfigButton.Text = "Save";
            this.SaveConfigButton.UseVisualStyleBackColor = true;
            this.SaveConfigButton.Click += new System.EventHandler(this.SaveConfigButton_Click);
            // 
            // LoadConfigButton
            // 
            this.LoadConfigButton.Location = new System.Drawing.Point(703, 507);
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
            this.SeriesTrainButton.Location = new System.Drawing.Point(650, 195);
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
            this.label12.Location = new System.Drawing.Point(2, 362);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 13);
            this.label12.TabIndex = 14;
            this.label12.Text = "CrossValidation:";
            // 
            // CrossTrainButton
            // 
            this.CrossTrainButton.Location = new System.Drawing.Point(599, 359);
            this.CrossTrainButton.Name = "CrossTrainButton";
            this.CrossTrainButton.Size = new System.Drawing.Size(72, 19);
            this.CrossTrainButton.TabIndex = 10;
            this.CrossTrainButton.Text = "Cross Train";
            this.CrossTrainButton.UseVisualStyleBackColor = true;
            this.CrossTrainButton.Click += new System.EventHandler(this.CrossTrainButton_Click);
            // 
            // CrossSeriesButton
            // 
            this.CrossSeriesButton.Location = new System.Drawing.Point(673, 359);
            this.CrossSeriesButton.Name = "CrossSeriesButton";
            this.CrossSeriesButton.Size = new System.Drawing.Size(83, 19);
            this.CrossSeriesButton.TabIndex = 10;
            this.CrossSeriesButton.Text = "Cross Series";
            this.CrossSeriesButton.UseVisualStyleBackColor = true;
            this.CrossSeriesButton.Click += new System.EventHandler(this.CrossSeriesButton_Click);
            // 
            // FolderCountNumeric
            // 
            this.FolderCountNumeric.Location = new System.Drawing.Point(89, 504);
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
            this.label13.Location = new System.Drawing.Point(14, 507);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 13);
            this.label13.TabIndex = 14;
            this.label13.Text = "Folder Count:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(524, 436);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 13);
            this.label14.TabIndex = 14;
            this.label14.Text = "Thread Count:";
            // 
            // ThreadCountNumeric
            // 
            this.ThreadCountNumeric.Location = new System.Drawing.Point(599, 433);
            this.ThreadCountNumeric.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
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
            this.label15.Location = new System.Drawing.Point(199, 436);
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
            this.MaxCNumeric.Location = new System.Drawing.Point(248, 433);
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
            this.label16.Location = new System.Drawing.Point(202, 472);
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
            this.MinCNumeric.Location = new System.Drawing.Point(248, 469);
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
            this.label17.Location = new System.Drawing.Point(372, 437);
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
            this.MaxJNumeric.Location = new System.Drawing.Point(415, 434);
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
            this.label18.Location = new System.Drawing.Point(375, 472);
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
            this.MinJNumeric.Location = new System.Drawing.Point(415, 469);
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
            this.label19.Location = new System.Drawing.Point(197, 509);
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
            this.StepCNumeric.Location = new System.Drawing.Point(248, 506);
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
            this.label20.Location = new System.Drawing.Point(375, 509);
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
            this.StepJNumeric.Location = new System.Drawing.Point(415, 506);
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
            this.label21.Location = new System.Drawing.Point(14, 435);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(59, 13);
            this.label21.TabIndex = 14;
            this.label21.Text = "MaxFolder:";
            // 
            // MaxFolderNumeric
            // 
            this.MaxFolderNumeric.Location = new System.Drawing.Point(89, 432);
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
            this.label22.Location = new System.Drawing.Point(14, 471);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(56, 13);
            this.label22.TabIndex = 14;
            this.label22.Text = "MinFolder:";
            // 
            // MinFolderNumeric
            // 
            this.MinFolderNumeric.Location = new System.Drawing.Point(89, 468);
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
            this.StopThreadsButton.Location = new System.Drawing.Point(657, 432);
            this.StopThreadsButton.Name = "StopThreadsButton";
            this.StopThreadsButton.Size = new System.Drawing.Size(47, 19);
            this.StopThreadsButton.TabIndex = 10;
            this.StopThreadsButton.Text = "Stop";
            this.StopThreadsButton.UseVisualStyleBackColor = true;
            this.StopThreadsButton.Click += new System.EventHandler(this.StopThreadsButton_Click);
            // 
            // PauseResumeButton
            // 
            this.PauseResumeButton.Location = new System.Drawing.Point(709, 432);
            this.PauseResumeButton.Name = "PauseResumeButton";
            this.PauseResumeButton.Size = new System.Drawing.Size(47, 19);
            this.PauseResumeButton.TabIndex = 10;
            this.PauseResumeButton.Text = "Pause";
            this.PauseResumeButton.UseVisualStyleBackColor = true;
            this.PauseResumeButton.Click += new System.EventHandler(this.PauseResumeButton_Click);
            // 
            // LoadAdditionalFileButton
            // 
            this.LoadAdditionalFileButton.Location = new System.Drawing.Point(599, 31);
            this.LoadAdditionalFileButton.Name = "LoadAdditionalFileButton";
            this.LoadAdditionalFileButton.Size = new System.Drawing.Size(72, 19);
            this.LoadAdditionalFileButton.TabIndex = 9;
            this.LoadAdditionalFileButton.Text = "Load";
            this.LoadAdditionalFileButton.UseVisualStyleBackColor = true;
            this.LoadAdditionalFileButton.Click += new System.EventHandler(this.LoadAdditionalFileButton_Click);
            // 
            // SaveAdditionalButton
            // 
            this.SaveAdditionalButton.Location = new System.Drawing.Point(684, 31);
            this.SaveAdditionalButton.Name = "SaveAdditionalButton";
            this.SaveAdditionalButton.Size = new System.Drawing.Size(72, 19);
            this.SaveAdditionalButton.TabIndex = 9;
            this.SaveAdditionalButton.Text = "Save";
            this.SaveAdditionalButton.UseVisualStyleBackColor = true;
            this.SaveAdditionalButton.Click += new System.EventHandler(this.SaveAdditionalButton_Click);
            // 
            // RawProduceAllButton
            // 
            this.RawProduceAllButton.Location = new System.Drawing.Point(684, 97);
            this.RawProduceAllButton.Name = "RawProduceAllButton";
            this.RawProduceAllButton.Size = new System.Drawing.Size(72, 42);
            this.RawProduceAllButton.TabIndex = 9;
            this.RawProduceAllButton.Text = "Produce All";
            this.RawProduceAllButton.UseVisualStyleBackColor = true;
            this.RawProduceAllButton.Click += new System.EventHandler(this.RawProduceAllButton_Click);
            // 
            // RankSvmCheckBox
            // 
            this.RankSvmCheckBox.AutoSize = true;
            this.RankSvmCheckBox.Location = new System.Drawing.Point(309, 396);
            this.RankSvmCheckBox.Name = "RankSvmCheckBox";
            this.RankSvmCheckBox.Size = new System.Drawing.Size(76, 17);
            this.RankSvmCheckBox.TabIndex = 18;
            this.RankSvmCheckBox.Text = "Rank Svm";
            this.RankSvmCheckBox.UseVisualStyleBackColor = true;
            this.RankSvmCheckBox.CheckedChanged += new System.EventHandler(this.RankCheckBox_CheckedChanged);
            // 
            // FastRankLearnCheckBox
            // 
            this.FastRankLearnCheckBox.AutoSize = true;
            this.FastRankLearnCheckBox.Location = new System.Drawing.Point(390, 395);
            this.FastRankLearnCheckBox.Name = "FastRankLearnCheckBox";
            this.FastRankLearnCheckBox.Size = new System.Drawing.Size(99, 17);
            this.FastRankLearnCheckBox.TabIndex = 18;
            this.FastRankLearnCheckBox.Text = "FastRankLearn";
            this.FastRankLearnCheckBox.UseVisualStyleBackColor = true;
            this.FastRankLearnCheckBox.CheckedChanged += new System.EventHandler(this.RankCheckBox_CheckedChanged);
            // 
            // IsPowercCheckBox
            // 
            this.IsPowercCheckBox.AutoSize = true;
            this.IsPowercCheckBox.Checked = true;
            this.IsPowercCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsPowercCheckBox.Location = new System.Drawing.Point(203, 536);
            this.IsPowercCheckBox.Name = "IsPowercCheckBox";
            this.IsPowercCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.IsPowercCheckBox.Size = new System.Drawing.Size(59, 17);
            this.IsPowercCheckBox.TabIndex = 18;
            this.IsPowercCheckBox.Text = ":Power";
            this.IsPowercCheckBox.UseVisualStyleBackColor = true;
            this.IsPowercCheckBox.CheckedChanged += new System.EventHandler(this.RankCheckBox_CheckedChanged);
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
            this.OpenConfigsButton.Location = new System.Drawing.Point(527, 509);
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
            this.CaseQueryTextBox.Location = new System.Drawing.Point(89, 568);
            this.CaseQueryTextBox.Name = "CaseQueryTextBox";
            this.CaseQueryTextBox.Size = new System.Drawing.Size(123, 20);
            this.CaseQueryTextBox.TabIndex = 28;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 569);
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
            this.CaseSetComboBox.Location = new System.Drawing.Point(223, 568);
            this.CaseSetComboBox.Name = "CaseSetComboBox";
            this.CaseSetComboBox.Size = new System.Drawing.Size(121, 21);
            this.CaseSetComboBox.TabIndex = 29;
            // 
            // OpenCaseButton
            // 
            this.OpenCaseButton.Location = new System.Drawing.Point(354, 568);
            this.OpenCaseButton.Name = "OpenCaseButton";
            this.OpenCaseButton.Size = new System.Drawing.Size(75, 23);
            this.OpenCaseButton.TabIndex = 30;
            this.OpenCaseButton.Text = "Open Case";
            this.OpenCaseButton.UseVisualStyleBackColor = true;
            this.OpenCaseButton.Click += new System.EventHandler(this.OpenCaseButton_Click);
            // 
            // NormalizeCheckBox
            // 
            this.NormalizeCheckBox.AutoSize = true;
            this.NormalizeCheckBox.Location = new System.Drawing.Point(649, 248);
            this.NormalizeCheckBox.Name = "NormalizeCheckBox";
            this.NormalizeCheckBox.Size = new System.Drawing.Size(72, 17);
            this.NormalizeCheckBox.TabIndex = 31;
            this.NormalizeCheckBox.Text = "Normalize";
            this.NormalizeCheckBox.UseVisualStyleBackColor = true;
            this.NormalizeCheckBox.CheckedChanged += new System.EventHandler(this.NormalizeCheckBox_CheckedChanged);
            // 
            // ValueOperationComboBox
            // 
            this.ValueOperationComboBox.FormattingEnabled = true;
            this.ValueOperationComboBox.Items.AddRange(new object[] {
            "None",
            "Discrete"});
            this.ValueOperationComboBox.Location = new System.Drawing.Point(684, 150);
            this.ValueOperationComboBox.Name = "ValueOperationComboBox";
            this.ValueOperationComboBox.Size = new System.Drawing.Size(78, 21);
            this.ValueOperationComboBox.TabIndex = 32;
            this.ValueOperationComboBox.Text = "Discrete";
            // 
            // StartTest2Button
            // 
            this.StartTest2Button.Location = new System.Drawing.Point(717, 223);
            this.StartTest2Button.Name = "StartTest2Button";
            this.StartTest2Button.Size = new System.Drawing.Size(18, 76);
            this.StartTest2Button.TabIndex = 33;
            this.StartTest2Button.Text = "Test";
            this.StartTest2Button.UseVisualStyleBackColor = true;
            this.StartTest2Button.Click += new System.EventHandler(this.StartTest2Button_Click);
            // 
            // StartTrainAndTestButton
            // 
            this.StartTrainAndTestButton.Location = new System.Drawing.Point(740, 198);
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
            "None",
            "SVM",
            "RankSVM"});
            this.TrainMethodComboBox.Location = new System.Drawing.Point(766, 31);
            this.TrainMethodComboBox.Name = "TrainMethodComboBox";
            this.TrainMethodComboBox.Size = new System.Drawing.Size(74, 21);
            this.TrainMethodComboBox.TabIndex = 36;
            this.TrainMethodComboBox.Text = "None";
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
            this.ProgressLBar.Location = new System.Drawing.Point(771, 509);
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
            this.ModelOpenFileTextBox.Location = new System.Drawing.Point(89, 310);
            this.ModelOpenFileTextBox.Name = "ModelOpenFileTextBox";
            this.ModelOpenFileTextBox.OnlyShowFileName = true;
            this.ModelOpenFileTextBox.Size = new System.Drawing.Size(504, 34);
            this.ModelOpenFileTextBox.TabIndex = 19;
            // 
            // FeatureTestOpenFileTextBox
            // 
            this.FeatureTestOpenFileTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.FeatureTestOpenFileTextBox.FilePath = "";
            this.FeatureTestOpenFileTextBox.Filter = null;
            this.FeatureTestOpenFileTextBox.InitialDirectory = null;
            this.FeatureTestOpenFileTextBox.Location = new System.Drawing.Point(89, 270);
            this.FeatureTestOpenFileTextBox.Name = "FeatureTestOpenFileTextBox";
            this.FeatureTestOpenFileTextBox.OnlyShowFileName = true;
            this.FeatureTestOpenFileTextBox.Size = new System.Drawing.Size(504, 34);
            this.FeatureTestOpenFileTextBox.TabIndex = 19;
            // 
            // FeatureDevOpenFileTextBox
            // 
            this.FeatureDevOpenFileTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.FeatureDevOpenFileTextBox.FilePath = "";
            this.FeatureDevOpenFileTextBox.Filter = null;
            this.FeatureDevOpenFileTextBox.InitialDirectory = null;
            this.FeatureDevOpenFileTextBox.Location = new System.Drawing.Point(89, 230);
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
            this.ConfigTextBox.Location = new System.Drawing.Point(527, 466);
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
            this.CrossValidationOpenFileTextBox.Location = new System.Drawing.Point(89, 351);
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
            this.FeatureTrainOpenFileTextBox.Location = new System.Drawing.Point(89, 190);
            this.FeatureTrainOpenFileTextBox.Name = "FeatureTrainOpenFileTextBox";
            this.FeatureTrainOpenFileTextBox.OnlyShowFileName = true;
            this.FeatureTrainOpenFileTextBox.Size = new System.Drawing.Size(504, 34);
            this.FeatureTrainOpenFileTextBox.TabIndex = 19;
            // 
            // RawTestOpenFileTextBox
            // 
            this.RawTestOpenFileTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.RawTestOpenFileTextBox.FilePath = "";
            this.RawTestOpenFileTextBox.Filter = null;
            this.RawTestOpenFileTextBox.InitialDirectory = null;
            this.RawTestOpenFileTextBox.Location = new System.Drawing.Point(89, 143);
            this.RawTestOpenFileTextBox.Name = "RawTestOpenFileTextBox";
            this.RawTestOpenFileTextBox.OnlyShowFileName = true;
            this.RawTestOpenFileTextBox.Size = new System.Drawing.Size(504, 34);
            this.RawTestOpenFileTextBox.TabIndex = 19;
            // 
            // RawDevOpenFileTextBox
            // 
            this.RawDevOpenFileTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.RawDevOpenFileTextBox.FilePath = "";
            this.RawDevOpenFileTextBox.Filter = null;
            this.RawDevOpenFileTextBox.InitialDirectory = null;
            this.RawDevOpenFileTextBox.Location = new System.Drawing.Point(89, 103);
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
            this.AdditionalFileTextBox.Location = new System.Drawing.Point(89, 23);
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
            this.RawTrainOpenFileTextBox.Location = new System.Drawing.Point(89, 63);
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
            // SimpleSvmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 600);
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
            this.Controls.Add(this.NormalizeCheckBox);
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
            this.Controls.Add(this.FastRankLearnCheckBox);
            this.Controls.Add(this.RankSvmCheckBox);
            this.Controls.Add(this.DevIdenticalCheckBox);
            this.Controls.Add(this.StartTestButton);
            this.Controls.Add(this.PauseResumeButton);
            this.Controls.Add(this.StopThreadsButton);
            this.Controls.Add(this.CrossSeriesButton);
            this.Controls.Add(this.SeriesTrainButton);
            this.Controls.Add(this.CrossTrainButton);
            this.Controls.Add(this.StartTrainButton);
            this.Controls.Add(this.RawTestProduceButton);
            this.Controls.Add(this.RawDevProduceButton);
            this.Controls.Add(this.SaveAdditionalButton);
            this.Controls.Add(this.LoadAdditionalFileButton);
            this.Controls.Add(this.RawProduceAllButton);
            this.Controls.Add(this.RawTrainProduceButton);
            this.Controls.Add(this.LoadConfigButton);
            this.Controls.Add(this.SaveConfigButton);
            this.Controls.Add(this.ModelOpenFileTextBox);
            this.Controls.Add(this.FeatureTestOpenFileTextBox);
            this.Controls.Add(this.FeatureDevOpenFileTextBox);
            this.Controls.Add(this.ConfigTextBox);
            this.Controls.Add(this.CrossValidationOpenFileTextBox);
            this.Controls.Add(this.FeatureTrainOpenFileTextBox);
            this.Controls.Add(this.RawTestOpenFileTextBox);
            this.Controls.Add(this.RawDevOpenFileTextBox);
            this.Controls.Add(this.AdditionalFileTextBox);
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
            this.Name = "SimpleSvmForm";
            this.Text = "SimpleSvmForm";
            this.Load += new System.EventHandler(this.SimpleSvmForm_Load);
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
        private FileBrowserTextBox RawTestOpenFileTextBox;
        private FileBrowserTextBox FeatureTrainOpenFileTextBox;
        private FileBrowserTextBox FeatureDevOpenFileTextBox;
        private FileBrowserTextBox FeatureTestOpenFileTextBox;
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
        private Button LoadAdditionalFileButton;
        private Button SaveAdditionalButton;
        private Button RawProduceAllButton;
        private CheckBox RankSvmCheckBox;
        private CheckBox FastRankLearnCheckBox;
        private CheckBox IsPowercCheckBox;
        private Button OpenPerformanceButton;
        private Button OpenConfigsButton;
        private FileBrowserTextBox ConfigTextBox;
        private Button OpenPerformanceDirButton;
        private TextBox CaseQueryTextBox;
        private Label label6;
        private ComboBox CaseSetComboBox;
        private Button OpenCaseButton;
        private CheckBox NormalizeCheckBox;
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
    }
}