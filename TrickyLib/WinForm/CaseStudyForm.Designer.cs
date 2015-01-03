using TrickyLib.WinControl;
namespace TrickyLib.WinForm
{
    partial class CaseStudyForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CaseQueryTextBox = new System.Windows.Forms.TextBox();
            this.SearchCaseButton = new System.Windows.Forms.Button();
            this.ConsoleTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.OnlyTTestButton = new System.Windows.Forms.Button();
            this.SignTestButton = new System.Windows.Forms.Button();
            this.EvalFileBrowserTextBox2 = new TrickyLib.WinControl.FileBrowserTextBox();
            this.EvalFileBrowserTextBox1 = new TrickyLib.WinControl.FileBrowserTextBox();
            this.configControl1 = new TrickyLib.WinControl.ConfigControl();
            this.SourceFileBrowserTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.FeatureFileBrowserTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.CaseStudyButton = new System.Windows.Forms.Button();
            this.OpenCasesButton = new System.Windows.Forms.Button();
            this.OutputFileBrowserTextBox1 = new TrickyLib.WinControl.FileBrowserTextBox();
            this.OutputFileBrowserTextBox2 = new TrickyLib.WinControl.FileBrowserTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ProduceEvalFileButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Feature File:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Source File:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "OutputFile1:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 325);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "CaseQuery:";
            // 
            // CaseQueryTextBox
            // 
            this.CaseQueryTextBox.Location = new System.Drawing.Point(76, 323);
            this.CaseQueryTextBox.Name = "CaseQueryTextBox";
            this.CaseQueryTextBox.Size = new System.Drawing.Size(394, 20);
            this.CaseQueryTextBox.TabIndex = 6;
            // 
            // SearchCaseButton
            // 
            this.SearchCaseButton.Location = new System.Drawing.Point(477, 320);
            this.SearchCaseButton.Name = "SearchCaseButton";
            this.SearchCaseButton.Size = new System.Drawing.Size(108, 23);
            this.SearchCaseButton.TabIndex = 7;
            this.SearchCaseButton.Text = "Search";
            this.SearchCaseButton.UseVisualStyleBackColor = true;
            this.SearchCaseButton.Click += new System.EventHandler(this.SearchCaseButton_Click);
            // 
            // ConsoleTextBox
            // 
            this.ConsoleTextBox.Location = new System.Drawing.Point(604, 25);
            this.ConsoleTextBox.Multiline = true;
            this.ConsoleTextBox.Name = "ConsoleTextBox";
            this.ConsoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ConsoleTextBox.Size = new System.Drawing.Size(269, 330);
            this.ConsoleTextBox.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(601, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Console Win";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 257);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "EvalFile2:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 217);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "EvalFile1:";
            // 
            // OnlyTTestButton
            // 
            this.OnlyTTestButton.Location = new System.Drawing.Point(477, 361);
            this.OnlyTTestButton.Name = "OnlyTTestButton";
            this.OnlyTTestButton.Size = new System.Drawing.Size(110, 23);
            this.OnlyTTestButton.TabIndex = 8;
            this.OnlyTTestButton.Text = "Only T-Test";
            this.OnlyTTestButton.UseVisualStyleBackColor = true;
            this.OnlyTTestButton.Click += new System.EventHandler(this.OnlyTTestButton_Click);
            // 
            // SignTestButton
            // 
            this.SignTestButton.Location = new System.Drawing.Point(396, 361);
            this.SignTestButton.Name = "SignTestButton";
            this.SignTestButton.Size = new System.Drawing.Size(75, 23);
            this.SignTestButton.TabIndex = 11;
            this.SignTestButton.Text = "Sign-Test";
            this.SignTestButton.UseVisualStyleBackColor = true;
            this.SignTestButton.Click += new System.EventHandler(this.SignTestButton_Click);
            // 
            // EvalFileBrowserTextBox2
            // 
            this.EvalFileBrowserTextBox2.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.EvalFileBrowserTextBox2.FilePath = null;
            this.EvalFileBrowserTextBox2.Filter = null;
            this.EvalFileBrowserTextBox2.InitialDirectory = null;
            this.EvalFileBrowserTextBox2.Location = new System.Drawing.Point(73, 251);
            this.EvalFileBrowserTextBox2.Name = "EvalFileBrowserTextBox2";
            this.EvalFileBrowserTextBox2.OnlyShowFileName = true;
            this.EvalFileBrowserTextBox2.Size = new System.Drawing.Size(511, 34);
            this.EvalFileBrowserTextBox2.TabIndex = 10;
            // 
            // EvalFileBrowserTextBox1
            // 
            this.EvalFileBrowserTextBox1.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.EvalFileBrowserTextBox1.FilePath = null;
            this.EvalFileBrowserTextBox1.Filter = null;
            this.EvalFileBrowserTextBox1.InitialDirectory = null;
            this.EvalFileBrowserTextBox1.Location = new System.Drawing.Point(73, 207);
            this.EvalFileBrowserTextBox1.Name = "EvalFileBrowserTextBox1";
            this.EvalFileBrowserTextBox1.OnlyShowFileName = true;
            this.EvalFileBrowserTextBox1.Size = new System.Drawing.Size(511, 34);
            this.EvalFileBrowserTextBox1.TabIndex = 10;
            // 
            // configControl1
            // 
            this.configControl1.ConfigFile = null;
            this.configControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.configControl1.Location = new System.Drawing.Point(0, 367);
            this.configControl1.Name = "configControl1";
            this.configControl1.Size = new System.Drawing.Size(885, 75);
            this.configControl1.TabIndex = 5;
            // 
            // SourceFileBrowserTextBox
            // 
            this.SourceFileBrowserTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.SourceFileBrowserTextBox.FilePath = null;
            this.SourceFileBrowserTextBox.Filter = null;
            this.SourceFileBrowserTextBox.InitialDirectory = null;
            this.SourceFileBrowserTextBox.Location = new System.Drawing.Point(76, 9);
            this.SourceFileBrowserTextBox.Name = "SourceFileBrowserTextBox";
            this.SourceFileBrowserTextBox.OnlyShowFileName = true;
            this.SourceFileBrowserTextBox.Size = new System.Drawing.Size(508, 34);
            this.SourceFileBrowserTextBox.TabIndex = 3;
            // 
            // FeatureFileBrowserTextBox
            // 
            this.FeatureFileBrowserTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.FeatureFileBrowserTextBox.FilePath = null;
            this.FeatureFileBrowserTextBox.Filter = null;
            this.FeatureFileBrowserTextBox.InitialDirectory = null;
            this.FeatureFileBrowserTextBox.Location = new System.Drawing.Point(76, 49);
            this.FeatureFileBrowserTextBox.Name = "FeatureFileBrowserTextBox";
            this.FeatureFileBrowserTextBox.OnlyShowFileName = true;
            this.FeatureFileBrowserTextBox.Size = new System.Drawing.Size(509, 34);
            this.FeatureFileBrowserTextBox.TabIndex = 0;
            // 
            // CaseStudyButton
            // 
            this.CaseStudyButton.Location = new System.Drawing.Point(397, 390);
            this.CaseStudyButton.Name = "CaseStudyButton";
            this.CaseStudyButton.Size = new System.Drawing.Size(75, 23);
            this.CaseStudyButton.TabIndex = 12;
            this.CaseStudyButton.Text = "CaseStudy";
            this.CaseStudyButton.UseVisualStyleBackColor = true;
            this.CaseStudyButton.Click += new System.EventHandler(this.CaseStudyButton_Click);
            // 
            // OpenCasesButton
            // 
            this.OpenCasesButton.Location = new System.Drawing.Point(477, 390);
            this.OpenCasesButton.Name = "OpenCasesButton";
            this.OpenCasesButton.Size = new System.Drawing.Size(75, 23);
            this.OpenCasesButton.TabIndex = 12;
            this.OpenCasesButton.Text = "Open Cases";
            this.OpenCasesButton.UseVisualStyleBackColor = true;
            this.OpenCasesButton.Click += new System.EventHandler(this.OpenCasesButton_Click);
            // 
            // OutputFileBrowserTextBox1
            // 
            this.OutputFileBrowserTextBox1.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.OutputFileBrowserTextBox1.FilePath = null;
            this.OutputFileBrowserTextBox1.Filter = null;
            this.OutputFileBrowserTextBox1.InitialDirectory = null;
            this.OutputFileBrowserTextBox1.Location = new System.Drawing.Point(75, 89);
            this.OutputFileBrowserTextBox1.Name = "OutputFileBrowserTextBox1";
            this.OutputFileBrowserTextBox1.OnlyShowFileName = true;
            this.OutputFileBrowserTextBox1.Size = new System.Drawing.Size(509, 34);
            this.OutputFileBrowserTextBox1.TabIndex = 0;
            // 
            // OutputFileBrowserTextBox2
            // 
            this.OutputFileBrowserTextBox2.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.OutputFileBrowserTextBox2.FilePath = null;
            this.OutputFileBrowserTextBox2.Filter = null;
            this.OutputFileBrowserTextBox2.InitialDirectory = null;
            this.OutputFileBrowserTextBox2.Location = new System.Drawing.Point(75, 129);
            this.OutputFileBrowserTextBox2.Name = "OutputFileBrowserTextBox2";
            this.OutputFileBrowserTextBox2.OnlyShowFileName = true;
            this.OutputFileBrowserTextBox2.Size = new System.Drawing.Size(509, 34);
            this.OutputFileBrowserTextBox2.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 137);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "OutputFile2:";
            // 
            // ProduceEvalFileButton
            // 
            this.ProduceEvalFileButton.Location = new System.Drawing.Point(79, 172);
            this.ProduceEvalFileButton.Name = "ProduceEvalFileButton";
            this.ProduceEvalFileButton.Size = new System.Drawing.Size(104, 23);
            this.ProduceEvalFileButton.TabIndex = 13;
            this.ProduceEvalFileButton.Text = "Produce Eval";
            this.ProduceEvalFileButton.UseVisualStyleBackColor = true;
            this.ProduceEvalFileButton.Click += new System.EventHandler(this.ProduceEvalFileButton_Click);
            // 
            // CaseStudyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 442);
            this.Controls.Add(this.ProduceEvalFileButton);
            this.Controls.Add(this.OpenCasesButton);
            this.Controls.Add(this.CaseStudyButton);
            this.Controls.Add(this.SignTestButton);
            this.Controls.Add(this.EvalFileBrowserTextBox2);
            this.Controls.Add(this.EvalFileBrowserTextBox1);
            this.Controls.Add(this.ConsoleTextBox);
            this.Controls.Add(this.OnlyTTestButton);
            this.Controls.Add(this.SearchCaseButton);
            this.Controls.Add(this.CaseQueryTextBox);
            this.Controls.Add(this.configControl1);
            this.Controls.Add(this.SourceFileBrowserTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OutputFileBrowserTextBox2);
            this.Controls.Add(this.OutputFileBrowserTextBox1);
            this.Controls.Add(this.FeatureFileBrowserTextBox);
            this.Name = "CaseStudyForm";
            this.Text = "CaseStudyForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FileBrowserTextBox FeatureFileBrowserTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private FileBrowserTextBox SourceFileBrowserTextBox;
        private System.Windows.Forms.Label label3;
        private ConfigControl configControl1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox CaseQueryTextBox;
        private System.Windows.Forms.Button SearchCaseButton;
        private System.Windows.Forms.TextBox ConsoleTextBox;
        private System.Windows.Forms.Label label5;
        private FileBrowserTextBox EvalFileBrowserTextBox1;
        private FileBrowserTextBox EvalFileBrowserTextBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button OnlyTTestButton;
        private System.Windows.Forms.Button SignTestButton;
        private System.Windows.Forms.Button CaseStudyButton;
        private System.Windows.Forms.Button OpenCasesButton;
        private FileBrowserTextBox OutputFileBrowserTextBox1;
        private FileBrowserTextBox OutputFileBrowserTextBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button ProduceEvalFileButton;
    }
}