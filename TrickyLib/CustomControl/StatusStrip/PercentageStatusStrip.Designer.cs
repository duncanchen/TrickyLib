namespace TrickyLib.CustomControl.StatusStrip
{
    partial class PercentageStatusStrip
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.PercentageProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.PercentageLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProcessingLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.BusyBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PercentageProgressBar,
            this.PercentageLabel,
            this.ProcessingLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip1.Size = new System.Drawing.Size(715, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // PercentageProgressBar
            // 
            this.PercentageProgressBar.Name = "PercentageProgressBar";
            this.PercentageProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // PercentageLabel
            // 
            this.PercentageLabel.Name = "PercentageLabel";
            this.PercentageLabel.Size = new System.Drawing.Size(23, 17);
            this.PercentageLabel.Text = "0%";
            // 
            // ProcessingLabel
            // 
            this.ProcessingLabel.Name = "ProcessingLabel";
            this.ProcessingLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ProcessingLabel.Size = new System.Drawing.Size(73, 17);
            this.ProcessingLabel.Text = "Processing...";
            // 
            // PercentageStatusStrip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip1);
            this.Name = "PercentageStatusStrip";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Size = new System.Drawing.Size(715, 22);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar PercentageProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel PercentageLabel;
        private System.Windows.Forms.ToolStripStatusLabel ProcessingLabel;
        private System.ComponentModel.BackgroundWorker BusyBackgroundWorker;
        
    }
}
