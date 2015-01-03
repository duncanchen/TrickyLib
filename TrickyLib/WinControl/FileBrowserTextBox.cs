using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TrickyLib.Service;

namespace TrickyLib.WinControl
{
    public partial class FileBrowserTextBox : UserControl
    {
        public delegate void FilePathChangedHandler(object sender, EventArgs e);
        public event FilePathChangedHandler FilePathChanged;

        public bool OnlyShowFileName
        {
            get
            {
                return this.OnlyShowFileNameCheckBox.Checked;
            }
            set
            {
                this.OnlyShowFileNameCheckBox.Checked = value;
            }
        }

        private string _filePath;
        public string FilePath
        {
            get
            {
                return this._filePath;
            }
            set
            {
                this._filePath = value;

                if (this.OnlyShowFileName)
                    this.FilePathTextBox.Text = Path.GetFileName(value);
                else
                    this.FilePathTextBox.Text = value;

                if (this.FilePathChanged != null)
                    this.FilePathChanged(this, new EventArgs());
            }
        }

        public string Filter { get; set; }
        public string InitialDirectory { get; set; }

        public FileDialogTypes FileDialogType { get; set; }

        public FileBrowserTextBox()
        {
            InitializeComponent();
            this.FileDialogType = FileDialogTypes.Open;
        }

        private void OnlyShowFileNameCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.OnlyShowFileNameCheckBox.Checked && (this.FileDialogType == FileDialogTypes.Open || this.FileDialogType == FileDialogTypes.Save))
                this.FilePathTextBox.Text = Path.GetFileName(this.FilePath);
            else
                this.FilePathTextBox.Text = this.FilePath;
        }

        private void FilePathTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.OnlyShowFileNameCheckBox.Checked)
                this._filePath = IO.FilePath.ChangeFile(this._filePath, this.FilePathTextBox.Text);
            else
                this._filePath = this.FilePathTextBox.Text;

            if (this.FilePathChanged != null)
                this.FilePathChanged(this, new EventArgs());
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (this.FilePath != string.Empty && this.FilePath.Trim() != string.Empty)
            {
                if (File.Exists(this.FilePath))
                    SoftwareOperator.OpenSoftware(Softwares.NOTEPAD, this.FilePath);
                else
                    MessageBox.Show("File not found: " + this.FilePath);
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (this.FileDialogType == FileDialogTypes.Open || this.FileDialogType == FileDialogTypes.Save)
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control && !string.IsNullOrEmpty(this.FilePath))
                    this.InitialDirectory = Path.GetDirectoryName(this.FilePath);
                FileDialog fdg = null;
                if (this.FileDialogType == FileDialogTypes.Open)
                    fdg = new OpenFileDialog() { Filter = this.Filter, InitialDirectory = this.InitialDirectory };
                else
                    fdg = new SaveFileDialog() { Filter = this.Filter, InitialDirectory = this.InitialDirectory };

                if (fdg.ShowDialog() == DialogResult.OK)
                    this.FilePath = fdg.FileName;
            }
            else if (this.FileDialogType == FileDialogTypes.Folder)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                    this.FilePath = fbd.SelectedPath;
            }

        }
    }

    public enum FileDialogTypes
    {
        Open,
        Save,
        Folder
    }
}
