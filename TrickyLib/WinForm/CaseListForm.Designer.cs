namespace TrickyLib.WinForm
{
    partial class CaseListForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.CasesDataGrid = new System.Windows.Forms.DataGridView();
            this.OneCaseDataGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CasesDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OneCaseDataGrid)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.CasesDataGrid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.OneCaseDataGrid);
            this.splitContainer1.Size = new System.Drawing.Size(1004, 597);
            this.splitContainer1.SplitterDistance = 627;
            this.splitContainer1.TabIndex = 0;
            // 
            // CasesDataGrid
            // 
            this.CasesDataGrid.AllowUserToOrderColumns = true;
            this.CasesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CasesDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CasesDataGrid.Location = new System.Drawing.Point(0, 0);
            this.CasesDataGrid.Name = "CasesDataGrid";
            this.CasesDataGrid.Size = new System.Drawing.Size(627, 597);
            this.CasesDataGrid.TabIndex = 0;
            this.CasesDataGrid.SelectionChanged += new System.EventHandler(this.CasesDataGrid_SelectionChanged);
            // 
            // OneCaseDataGrid
            // 
            this.OneCaseDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OneCaseDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OneCaseDataGrid.Location = new System.Drawing.Point(0, 0);
            this.OneCaseDataGrid.Name = "OneCaseDataGrid";
            this.OneCaseDataGrid.Size = new System.Drawing.Size(373, 597);
            this.OneCaseDataGrid.TabIndex = 0;
            // 
            // CaseListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 597);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CaseListForm";
            this.Text = "CaseListForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CasesDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OneCaseDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView CasesDataGrid;
        private System.Windows.Forms.DataGridView OneCaseDataGrid;
    }
}