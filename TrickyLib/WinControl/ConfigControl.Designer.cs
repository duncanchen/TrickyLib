namespace TrickyLib.WinControl
{
    partial class ConfigControl
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
            this.ConfigTextBox = new TrickyLib.WinControl.FileBrowserTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OpenDirButton = new System.Windows.Forms.Button();
            this.SaveConfigButton = new System.Windows.Forms.Button();
            this.LoadConfigButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConfigTextBox
            // 
            this.ConfigTextBox.FileDialogType = TrickyLib.WinControl.FileDialogTypes.Open;
            this.ConfigTextBox.FilePath = null;
            this.ConfigTextBox.Filter = null;
            this.ConfigTextBox.InitialDirectory = null;
            this.ConfigTextBox.Location = new System.Drawing.Point(50, 4);
            this.ConfigTextBox.Name = "ConfigTextBox";
            this.ConfigTextBox.OnlyShowFileName = true;
            this.ConfigTextBox.Size = new System.Drawing.Size(304, 34);
            this.ConfigTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Config:";
            // 
            // OpenDirButton
            // 
            this.OpenDirButton.Location = new System.Drawing.Point(55, 42);
            this.OpenDirButton.Name = "OpenDirButton";
            this.OpenDirButton.Size = new System.Drawing.Size(75, 23);
            this.OpenDirButton.TabIndex = 2;
            this.OpenDirButton.Text = "Open Dir";
            this.OpenDirButton.UseVisualStyleBackColor = true;
            this.OpenDirButton.Click += new System.EventHandler(this.OpenDirButton_Click);
            // 
            // SaveConfigButton
            // 
            this.SaveConfigButton.Location = new System.Drawing.Point(147, 42);
            this.SaveConfigButton.Name = "SaveConfigButton";
            this.SaveConfigButton.Size = new System.Drawing.Size(75, 23);
            this.SaveConfigButton.TabIndex = 2;
            this.SaveConfigButton.Text = "Save";
            this.SaveConfigButton.UseVisualStyleBackColor = true;
            this.SaveConfigButton.Click += new System.EventHandler(this.SaveConfigButton_Click);
            // 
            // LoadConfigButton
            // 
            this.LoadConfigButton.Location = new System.Drawing.Point(233, 42);
            this.LoadConfigButton.Name = "LoadConfigButton";
            this.LoadConfigButton.Size = new System.Drawing.Size(75, 23);
            this.LoadConfigButton.TabIndex = 2;
            this.LoadConfigButton.Text = "Load";
            this.LoadConfigButton.UseVisualStyleBackColor = true;
            this.LoadConfigButton.Click += new System.EventHandler(this.LoadConfigButton_Click);
            // 
            // ConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LoadConfigButton);
            this.Controls.Add(this.SaveConfigButton);
            this.Controls.Add(this.OpenDirButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConfigTextBox);
            this.Name = "ConfigControl";
            this.Size = new System.Drawing.Size(364, 75);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FileBrowserTextBox ConfigTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OpenDirButton;
        private System.Windows.Forms.Button SaveConfigButton;
        private System.Windows.Forms.Button LoadConfigButton;
    }
}
