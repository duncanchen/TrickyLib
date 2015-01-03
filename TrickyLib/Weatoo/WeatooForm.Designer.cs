using TrickyLib.CustomControl.PropertyGrid;
namespace TrickyLib.Weatoo
{
    partial class WeatooForm
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ExpLeftSplitor = new System.Windows.Forms.SplitContainer();
            this.ButtonTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.LoadButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.NewButton = new System.Windows.Forms.Button();
            this.RunButton = new System.Windows.Forms.Button();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.SaveAsButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ParameterGroupBox = new System.Windows.Forms.GroupBox();
            this.ParaGroupBoxSplitor = new System.Windows.Forms.SplitContainer();
            this.ParameterPropertyGrid = new TrickyLib.CustomControl.PropertyGrid.PropertyGridControl();
            this.CustomizeTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.AdoptCustomParaButton = new System.Windows.Forms.Button();
            this.CustomParaTextBox = new System.Windows.Forms.TextBox();
            this.ExperimentGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.ExpOptionPropertyGrid = new TrickyLib.CustomControl.PropertyGrid.PropertyGridControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ChangeDiskButton = new System.Windows.Forms.Button();
            this.DiskComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.MetricPropertyGrid = new TrickyLib.CustomControl.PropertyGrid.PropertyGridControl();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.RunTab = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.StopButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.CleanButton = new System.Windows.Forms.Button();
            this.RunTextBox = new System.Windows.Forms.RichTextBox();
            this.ResultTab = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.CleanPerformanceButton = new System.Windows.Forms.Button();
            this.LoadPerformanceConfig = new System.Windows.Forms.Button();
            this.ResultsDataGridView = new System.Windows.Forms.DataGridView();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.PercentStatusStrip = new TrickyLib.CustomControl.StatusStrip.PercentageStatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExpLeftSplitor)).BeginInit();
            this.ExpLeftSplitor.Panel1.SuspendLayout();
            this.ExpLeftSplitor.Panel2.SuspendLayout();
            this.ExpLeftSplitor.SuspendLayout();
            this.ButtonTableLayoutPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.ParameterGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParaGroupBoxSplitor)).BeginInit();
            this.ParaGroupBoxSplitor.Panel1.SuspendLayout();
            this.ParaGroupBoxSplitor.Panel2.SuspendLayout();
            this.ParaGroupBoxSplitor.SuspendLayout();
            this.CustomizeTableLayout.SuspendLayout();
            this.ExperimentGroupBox.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.RunTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.ResultTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ExpLeftSplitor);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.MainTabControl);
            this.splitContainer2.Size = new System.Drawing.Size(1016, 709);
            this.splitContainer2.SplitterDistance = 338;
            this.splitContainer2.TabIndex = 3;
            // 
            // ExpLeftSplitor
            // 
            this.ExpLeftSplitor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ExpLeftSplitor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExpLeftSplitor.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.ExpLeftSplitor.Location = new System.Drawing.Point(0, 0);
            this.ExpLeftSplitor.Name = "ExpLeftSplitor";
            this.ExpLeftSplitor.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // ExpLeftSplitor.Panel1
            // 
            this.ExpLeftSplitor.Panel1.Controls.Add(this.ButtonTableLayoutPanel);
            // 
            // ExpLeftSplitor.Panel2
            // 
            this.ExpLeftSplitor.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.ExpLeftSplitor.Size = new System.Drawing.Size(338, 709);
            this.ExpLeftSplitor.SplitterDistance = 70;
            this.ExpLeftSplitor.TabIndex = 1;
            // 
            // ButtonTableLayoutPanel
            // 
            this.ButtonTableLayoutPanel.ColumnCount = 4;
            this.ButtonTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ButtonTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ButtonTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ButtonTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ButtonTableLayoutPanel.Controls.Add(this.LoadButton, 0, 0);
            this.ButtonTableLayoutPanel.Controls.Add(this.SaveButton, 1, 0);
            this.ButtonTableLayoutPanel.Controls.Add(this.NewButton, 3, 0);
            this.ButtonTableLayoutPanel.Controls.Add(this.RunButton, 0, 1);
            this.ButtonTableLayoutPanel.Controls.Add(this.GenerateButton, 3, 1);
            this.ButtonTableLayoutPanel.Controls.Add(this.SaveAsButton, 2, 0);
            this.ButtonTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ButtonTableLayoutPanel.Name = "ButtonTableLayoutPanel";
            this.ButtonTableLayoutPanel.RowCount = 2;
            this.ButtonTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ButtonTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ButtonTableLayoutPanel.Size = new System.Drawing.Size(336, 68);
            this.ButtonTableLayoutPanel.TabIndex = 0;
            // 
            // LoadButton
            // 
            this.LoadButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadButton.Location = new System.Drawing.Point(3, 3);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(78, 28);
            this.LoadButton.TabIndex = 1;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SaveButton.Location = new System.Drawing.Point(87, 3);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(78, 28);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // NewButton
            // 
            this.NewButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NewButton.Location = new System.Drawing.Point(255, 3);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(78, 28);
            this.NewButton.TabIndex = 3;
            this.NewButton.Text = "New";
            this.NewButton.UseVisualStyleBackColor = true;
            this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // RunButton
            // 
            this.ButtonTableLayoutPanel.SetColumnSpan(this.RunButton, 2);
            this.RunButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RunButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RunButton.Location = new System.Drawing.Point(3, 37);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(162, 28);
            this.RunButton.TabIndex = 5;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // GenerateButton
            // 
            this.GenerateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GenerateButton.Location = new System.Drawing.Point(255, 37);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(78, 28);
            this.GenerateButton.TabIndex = 6;
            this.GenerateButton.Text = "Generate";
            this.GenerateButton.UseVisualStyleBackColor = true;
            // 
            // SaveAsButton
            // 
            this.SaveAsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SaveAsButton.Location = new System.Drawing.Point(171, 3);
            this.SaveAsButton.Name = "SaveAsButton";
            this.SaveAsButton.Size = new System.Drawing.Size(78, 28);
            this.SaveAsButton.TabIndex = 7;
            this.SaveAsButton.Text = "Save As";
            this.SaveAsButton.UseVisualStyleBackColor = true;
            this.SaveAsButton.Click += new System.EventHandler(this.SaveAsButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ParameterGroupBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ExperimentGroupBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(336, 633);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ParameterGroupBox
            // 
            this.ParameterGroupBox.Controls.Add(this.ParaGroupBoxSplitor);
            this.ParameterGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParameterGroupBox.Location = new System.Drawing.Point(3, 319);
            this.ParameterGroupBox.Name = "ParameterGroupBox";
            this.ParameterGroupBox.Size = new System.Drawing.Size(330, 152);
            this.ParameterGroupBox.TabIndex = 4;
            this.ParameterGroupBox.TabStop = false;
            this.ParameterGroupBox.Text = "Parameter Setting";
            // 
            // ParaGroupBoxSplitor
            // 
            this.ParaGroupBoxSplitor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParaGroupBoxSplitor.Location = new System.Drawing.Point(3, 16);
            this.ParaGroupBoxSplitor.Name = "ParaGroupBoxSplitor";
            this.ParaGroupBoxSplitor.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // ParaGroupBoxSplitor.Panel1
            // 
            this.ParaGroupBoxSplitor.Panel1.Controls.Add(this.ParameterPropertyGrid);
            // 
            // ParaGroupBoxSplitor.Panel2
            // 
            this.ParaGroupBoxSplitor.Panel2.Controls.Add(this.CustomizeTableLayout);
            this.ParaGroupBoxSplitor.Size = new System.Drawing.Size(324, 133);
            this.ParaGroupBoxSplitor.SplitterDistance = 104;
            this.ParaGroupBoxSplitor.TabIndex = 0;
            // 
            // ParameterPropertyGrid
            // 
            this.ParameterPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParameterPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.ParameterPropertyGrid.Name = "ParameterPropertyGrid";
            this.ParameterPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.ParameterPropertyGrid.Size = new System.Drawing.Size(324, 104);
            this.ParameterPropertyGrid.TabIndex = 3;
            this.ParameterPropertyGrid.ToolbarVisible = false;
            // 
            // CustomizeTableLayout
            // 
            this.CustomizeTableLayout.ColumnCount = 2;
            this.CustomizeTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.CustomizeTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.CustomizeTableLayout.Controls.Add(this.AdoptCustomParaButton, 0, 0);
            this.CustomizeTableLayout.Controls.Add(this.CustomParaTextBox, 1, 0);
            this.CustomizeTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CustomizeTableLayout.Location = new System.Drawing.Point(0, 0);
            this.CustomizeTableLayout.Name = "CustomizeTableLayout";
            this.CustomizeTableLayout.RowCount = 1;
            this.CustomizeTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.CustomizeTableLayout.Size = new System.Drawing.Size(324, 25);
            this.CustomizeTableLayout.TabIndex = 0;
            // 
            // AdoptCustomParaButton
            // 
            this.AdoptCustomParaButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdoptCustomParaButton.Location = new System.Drawing.Point(3, 3);
            this.AdoptCustomParaButton.Name = "AdoptCustomParaButton";
            this.AdoptCustomParaButton.Size = new System.Drawing.Size(74, 19);
            this.AdoptCustomParaButton.TabIndex = 0;
            this.AdoptCustomParaButton.Text = "Adopt Para";
            this.AdoptCustomParaButton.UseVisualStyleBackColor = true;
            this.AdoptCustomParaButton.Click += new System.EventHandler(this.AdoptCustomParaButton_Click);
            // 
            // CustomParaTextBox
            // 
            this.CustomParaTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CustomParaTextBox.Location = new System.Drawing.Point(83, 3);
            this.CustomParaTextBox.Name = "CustomParaTextBox";
            this.CustomParaTextBox.Size = new System.Drawing.Size(238, 20);
            this.CustomParaTextBox.TabIndex = 1;
            // 
            // ExperimentGroupBox
            // 
            this.ExperimentGroupBox.Controls.Add(this.tableLayoutPanel3);
            this.ExperimentGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExperimentGroupBox.Location = new System.Drawing.Point(3, 3);
            this.ExperimentGroupBox.Name = "ExperimentGroupBox";
            this.ExperimentGroupBox.Size = new System.Drawing.Size(330, 310);
            this.ExperimentGroupBox.TabIndex = 5;
            this.ExperimentGroupBox.TabStop = false;
            this.ExperimentGroupBox.Text = "Experiment Configuration";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.ExpOptionPropertyGrid, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.splitContainer1, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.06529F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.934708F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(324, 291);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // ExpOptionPropertyGrid
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.ExpOptionPropertyGrid, 2);
            this.ExpOptionPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExpOptionPropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.ExpOptionPropertyGrid.Name = "ExpOptionPropertyGrid";
            this.ExpOptionPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.ExpOptionPropertyGrid.Size = new System.Drawing.Size(318, 259);
            this.ExpOptionPropertyGrid.TabIndex = 5;
            this.ExpOptionPropertyGrid.ToolbarVisible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(165, 268);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ChangeDiskButton);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.DiskComboBox);
            this.splitContainer1.Size = new System.Drawing.Size(156, 20);
            this.splitContainer1.SplitterDistance = 81;
            this.splitContainer1.TabIndex = 6;
            // 
            // ChangeDiskButton
            // 
            this.ChangeDiskButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChangeDiskButton.Location = new System.Drawing.Point(0, 0);
            this.ChangeDiskButton.Name = "ChangeDiskButton";
            this.ChangeDiskButton.Size = new System.Drawing.Size(81, 20);
            this.ChangeDiskButton.TabIndex = 0;
            this.ChangeDiskButton.Text = "Change Disk";
            this.ChangeDiskButton.UseVisualStyleBackColor = true;
            this.ChangeDiskButton.Click += new System.EventHandler(this.ChangeDiskButton_Click);
            // 
            // DiskComboBox
            // 
            this.DiskComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DiskComboBox.FormattingEnabled = true;
            this.DiskComboBox.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
            this.DiskComboBox.Location = new System.Drawing.Point(0, 0);
            this.DiskComboBox.Name = "DiskComboBox";
            this.DiskComboBox.Size = new System.Drawing.Size(71, 21);
            this.DiskComboBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.MetricPropertyGrid);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(330, 153);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Metric Setting";
            // 
            // MetricPropertyGrid
            // 
            this.MetricPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MetricPropertyGrid.Location = new System.Drawing.Point(3, 16);
            this.MetricPropertyGrid.Name = "MetricPropertyGrid";
            this.MetricPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.MetricPropertyGrid.Size = new System.Drawing.Size(324, 134);
            this.MetricPropertyGrid.TabIndex = 4;
            this.MetricPropertyGrid.ToolbarVisible = false;
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.RunTab);
            this.MainTabControl.Controls.Add(this.ResultTab);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(674, 709);
            this.MainTabControl.TabIndex = 1;
            // 
            // RunTab
            // 
            this.RunTab.Controls.Add(this.splitContainer3);
            this.RunTab.Location = new System.Drawing.Point(4, 22);
            this.RunTab.Name = "RunTab";
            this.RunTab.Padding = new System.Windows.Forms.Padding(3);
            this.RunTab.Size = new System.Drawing.Size(666, 683);
            this.RunTab.TabIndex = 1;
            this.RunTab.Text = "Run";
            this.RunTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.RunTextBox);
            this.splitContainer3.Size = new System.Drawing.Size(660, 677);
            this.splitContainer3.SplitterDistance = 29;
            this.splitContainer3.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.StopButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.PauseButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.CleanButton, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(200, 29);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // StopButton
            // 
            this.StopButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StopButton.Location = new System.Drawing.Point(3, 3);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(60, 23);
            this.StopButton.TabIndex = 0;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PauseButton.Location = new System.Drawing.Point(69, 3);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(60, 23);
            this.PauseButton.TabIndex = 1;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // CleanButton
            // 
            this.CleanButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CleanButton.Location = new System.Drawing.Point(135, 3);
            this.CleanButton.Name = "CleanButton";
            this.CleanButton.Size = new System.Drawing.Size(62, 23);
            this.CleanButton.TabIndex = 2;
            this.CleanButton.Text = "Clean";
            this.CleanButton.UseVisualStyleBackColor = true;
            this.CleanButton.Click += new System.EventHandler(this.CleanButton_Click);
            // 
            // RunTextBox
            // 
            this.RunTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RunTextBox.Location = new System.Drawing.Point(0, 0);
            this.RunTextBox.Name = "RunTextBox";
            this.RunTextBox.Size = new System.Drawing.Size(660, 644);
            this.RunTextBox.TabIndex = 1;
            this.RunTextBox.Text = "";
            // 
            // ResultTab
            // 
            this.ResultTab.Controls.Add(this.splitContainer4);
            this.ResultTab.Location = new System.Drawing.Point(4, 22);
            this.ResultTab.Name = "ResultTab";
            this.ResultTab.Padding = new System.Windows.Forms.Padding(3);
            this.ResultTab.Size = new System.Drawing.Size(666, 683);
            this.ResultTab.TabIndex = 2;
            this.ResultTab.Text = "Results";
            this.ResultTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.tableLayoutPanel4);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.ResultsDataGridView);
            this.splitContainer4.Size = new System.Drawing.Size(660, 677);
            this.splitContainer4.SplitterDistance = 31;
            this.splitContainer4.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.CleanPerformanceButton, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.LoadPerformanceConfig, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(660, 31);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // CleanPerformanceButton
            // 
            this.CleanPerformanceButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CleanPerformanceButton.Location = new System.Drawing.Point(3, 3);
            this.CleanPerformanceButton.Name = "CleanPerformanceButton";
            this.CleanPerformanceButton.Size = new System.Drawing.Size(94, 25);
            this.CleanPerformanceButton.TabIndex = 0;
            this.CleanPerformanceButton.Text = "Clean";
            this.CleanPerformanceButton.UseVisualStyleBackColor = true;
            this.CleanPerformanceButton.Click += new System.EventHandler(this.CleanPerformanceButton_Click);
            // 
            // LoadPerformanceConfig
            // 
            this.LoadPerformanceConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadPerformanceConfig.Location = new System.Drawing.Point(103, 3);
            this.LoadPerformanceConfig.Name = "LoadPerformanceConfig";
            this.LoadPerformanceConfig.Size = new System.Drawing.Size(94, 25);
            this.LoadPerformanceConfig.TabIndex = 1;
            this.LoadPerformanceConfig.Text = "Load";
            this.LoadPerformanceConfig.UseVisualStyleBackColor = true;
            this.LoadPerformanceConfig.Click += new System.EventHandler(this.LoadPerformanceConfig_Click);
            // 
            // ResultsDataGridView
            // 
            this.ResultsDataGridView.AllowUserToAddRows = false;
            this.ResultsDataGridView.AllowUserToDeleteRows = false;
            this.ResultsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.ResultsDataGridView.Name = "ResultsDataGridView";
            this.ResultsDataGridView.Size = new System.Drawing.Size(660, 642);
            this.ResultsDataGridView.TabIndex = 1;
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.MainSplitContainer.Name = "MainSplitContainer";
            this.MainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // MainSplitContainer.Panel1
            // 
            this.MainSplitContainer.Panel1.Controls.Add(this.splitContainer2);
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.Controls.Add(this.PercentStatusStrip);
            this.MainSplitContainer.Size = new System.Drawing.Size(1016, 741);
            this.MainSplitContainer.SplitterDistance = 709;
            this.MainSplitContainer.TabIndex = 1;
            // 
            // PercentStatusStrip
            // 
            this.PercentStatusStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PercentStatusStrip.Location = new System.Drawing.Point(0, 6);
            this.PercentStatusStrip.Name = "PercentStatusStrip";
            this.PercentStatusStrip.ProcessingText = null;
            this.PercentStatusStrip.ProgressPercentage = 0D;
            this.PercentStatusStrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PercentStatusStrip.Size = new System.Drawing.Size(1016, 22);
            this.PercentStatusStrip.TabIndex = 0;
            // 
            // WeatooForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 741);
            this.Controls.Add(this.MainSplitContainer);
            this.Name = "WeatooForm";
            this.Text = "WeatooForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WeatooForm_FormClosing);
            this.Load += new System.EventHandler(this.WeatooForm_Load);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ExpLeftSplitor.Panel1.ResumeLayout(false);
            this.ExpLeftSplitor.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ExpLeftSplitor)).EndInit();
            this.ExpLeftSplitor.ResumeLayout(false);
            this.ButtonTableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ParameterGroupBox.ResumeLayout(false);
            this.ParaGroupBoxSplitor.Panel1.ResumeLayout(false);
            this.ParaGroupBoxSplitor.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ParaGroupBoxSplitor)).EndInit();
            this.ParaGroupBoxSplitor.ResumeLayout(false);
            this.CustomizeTableLayout.ResumeLayout(false);
            this.CustomizeTableLayout.PerformLayout();
            this.ExperimentGroupBox.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.MainTabControl.ResumeLayout(false);
            this.RunTab.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResultTab.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ResultsDataGridView)).EndInit();
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer ExpLeftSplitor;
        private System.Windows.Forms.TableLayoutPanel ButtonTableLayoutPanel;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button NewButton;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage RunTab;
        private System.Windows.Forms.TabPage ResultTab;
        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private CustomControl.StatusStrip.PercentageStatusStrip PercentStatusStrip;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.RichTextBox RunTextBox;
        private System.Windows.Forms.Button SaveAsButton;
        private System.Windows.Forms.GroupBox ParameterGroupBox;
        private System.Windows.Forms.SplitContainer ParaGroupBoxSplitor;
        private PropertyGridControl ParameterPropertyGrid;
        private System.Windows.Forms.TableLayoutPanel CustomizeTableLayout;
        private System.Windows.Forms.Button AdoptCustomParaButton;
        private System.Windows.Forms.TextBox CustomParaTextBox;
        private System.Windows.Forms.GroupBox ExperimentGroupBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private PropertyGridControl MetricPropertyGrid;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private PropertyGridControl ExpOptionPropertyGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button ChangeDiskButton;
        private System.Windows.Forms.ComboBox DiskComboBox;
        private System.Windows.Forms.Button CleanButton;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button CleanPerformanceButton;
        private System.Windows.Forms.Button LoadPerformanceConfig;
        private System.Windows.Forms.DataGridView ResultsDataGridView;


    }
}