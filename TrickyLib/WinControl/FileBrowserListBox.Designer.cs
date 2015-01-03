namespace TrickyLib.WinControl
{
    partial class FileBrowserListBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.FileListBox = new System.Windows.Forms.ListBox();
            this.OnlyShowFileNameCheckBox = new System.Windows.Forms.CheckBox();
            this.DownButton = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.FileListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.OnlyShowFileNameCheckBox);
            this.splitContainer1.Panel2.Controls.Add(this.DownButton);
            this.splitContainer1.Panel2.Controls.Add(this.UpButton);
            this.splitContainer1.Panel2.Controls.Add(this.RemoveButton);
            this.splitContainer1.Panel2.Controls.Add(this.AddButton);
            this.splitContainer1.Size = new System.Drawing.Size(291, 136);
            this.splitContainer1.SplitterDistance = 225;
            this.splitContainer1.TabIndex = 0;
            // 
            // FileListBox
            // 
            this.FileListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileListBox.FormattingEnabled = true;
            this.FileListBox.Location = new System.Drawing.Point(0, 0);
            this.FileListBox.Name = "FileListBox";
            this.FileListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.FileListBox.Size = new System.Drawing.Size(225, 136);
            this.FileListBox.TabIndex = 0;
            this.FileListBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FileListBox_KeyUp);
            // 
            // OnlyShowFileNameCheckBox
            // 
            this.OnlyShowFileNameCheckBox.AutoSize = true;
            this.OnlyShowFileNameCheckBox.Checked = true;
            this.OnlyShowFileNameCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OnlyShowFileNameCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.OnlyShowFileNameCheckBox.Location = new System.Drawing.Point(0, 92);
            this.OnlyShowFileNameCheckBox.Name = "OnlyShowFileNameCheckBox";
            this.OnlyShowFileNameCheckBox.Size = new System.Drawing.Size(62, 17);
            this.OnlyShowFileNameCheckBox.TabIndex = 0;
            this.OnlyShowFileNameCheckBox.Text = "Name";
            this.OnlyShowFileNameCheckBox.UseVisualStyleBackColor = true;
            this.OnlyShowFileNameCheckBox.CheckedChanged += new System.EventHandler(this.OnlyShowFileNameCheckBox_CheckedChanged);
            // 
            // DownButton
            // 
            this.DownButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.DownButton.Location = new System.Drawing.Point(0, 69);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(62, 23);
            this.DownButton.TabIndex = 4;
            this.DownButton.Text = "↓";
            this.DownButton.UseVisualStyleBackColor = true;
            this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // UpButton
            // 
            this.UpButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.UpButton.Location = new System.Drawing.Point(0, 46);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(62, 23);
            this.UpButton.TabIndex = 3;
            this.UpButton.Text = "↑";
            this.UpButton.UseVisualStyleBackColor = true;
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.RemoveButton.Location = new System.Drawing.Point(0, 23);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(62, 23);
            this.RemoveButton.TabIndex = 2;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.AddButton.Location = new System.Drawing.Point(0, 0);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(62, 23);
            this.AddButton.TabIndex = 1;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // FileBrowserListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "FileBrowserListBox";
            this.Size = new System.Drawing.Size(291, 136);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox FileListBox;
        private System.Windows.Forms.CheckBox OnlyShowFileNameCheckBox;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button UpButton;
    }
}
