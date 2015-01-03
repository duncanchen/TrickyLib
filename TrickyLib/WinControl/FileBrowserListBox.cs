using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TrickyLib.Extension;

namespace TrickyLib.WinControl
{
    public partial class FileBrowserListBox : UserControl
    {
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

        public FileDialogTypes FileDialogType { get; set; }
        public string Filter { get; set; }
        public string InitialDirectory { get; set; }

        private List<string> _files = new List<string>();
        public List<string> Files
        {
            get
            {
                return this._files;
            }
            set
            {
                if (value == null)
                    this._files = new List<string>();
                else
                    this._files = value;

                SetListBox();
            }
        }
        public string[] SelectedFiles
        {
            get
            {
                if (this.FileListBox.SelectedItems == null)
                    return null;
                else
                {
                    List<string> files = new List<string>();
                    foreach (var index in this.FileListBox.SelectedIndices)
                        files.Add(this.Files[(int)index]);

                    return files.ToArray();
                }
            }
        }
        public int[] SelectedIndice
        {
            get
            {
                if (this.FileListBox.SelectedItems == null)
                    return null;
                else
                {
                    List<int> files = new List<int>();
                    foreach (var index in this.FileListBox.SelectedIndices)
                        files.Add((int)index);

                    return files.ToArray();
                }
            }
        }

        public FileBrowserListBox()
        {
            InitializeComponent();
            this.OnlyShowFileName = true;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (this.FileDialogType == FileDialogTypes.Open)
            {
                string selectFilePath = "";
                if (this.SelectedFiles != null && this.SelectedFiles.Length > 0)
                    selectFilePath = this.SelectedFiles[0];
                else if (this.Files != null && this.Files.Count > 0)
                    selectFilePath = this.Files[0];

                if ((Control.ModifierKeys & Keys.Control) == Keys.Control && !string.IsNullOrEmpty(selectFilePath))
                    this.InitialDirectory = Path.GetDirectoryName(selectFilePath);

                FileDialog fdg = new OpenFileDialog() { Filter = this.Filter, InitialDirectory = this.InitialDirectory, Multiselect = true };
                if (fdg.ShowDialog() == DialogResult.OK)
                {
                    this.Files.AddRange(fdg.FileNames);
                    SetListBox();
                }
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            var indice = this.FileListBox.SelectedIndices;
            for (int i = indice.Count - 1; i >= 0; i--)
                this.Files.RemoveAt(indice[i]);

            SetListBox();
        }

        private void FileListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && this.FileListBox.SelectedIndices != null && this.FileListBox.SelectedIndices.Count > 0)
                RemoveButton_Click(null, null);
        }

        private void SetListBox()
        {
            if (this.FileListBox.Items != null)
                this.FileListBox.Items.Clear();

            if (this.Files != null && this.Files.Count > 0)
            {
                if (this.OnlyShowFileName)
                    this.FileListBox.Items.AddRange(this.Files.Select(f => Path.GetFileName(f)).ToArray());
                else
                    this.FileListBox.Items.AddRange(this.Files.ToArray());
            }
        }

        private void OnlyShowFileNameCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetListBox();
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            if (this.FileListBox.SelectedIndices.Count > 0)
            {
                bool cannotMove = false;
                List<int> targetIndice = new List<int>();
                
                if (this.FileListBox.SelectedIndices.Contains(0))
                    targetIndice.Add(0);

                foreach (var index in this.FileListBox.SelectedIndices)
                {
                    cannotMove = ((int)index == 0) || cannotMove && this.FileListBox.SelectedIndices.Contains((int)index - 1);
                    if (!cannotMove)
                    {
                        int sourceIndex = (int)index;
                        int targetIndex = (int)index - 1;

                        targetIndice.Add(targetIndex);
                        this.Files.Move(sourceIndex, targetIndex);
                    }
                }
                SetListBox();
                foreach (var index in targetIndice)
                    this.FileListBox.SelectedIndices.Add(index);
            }
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            if (this.FileListBox.SelectedIndices.Count > 0)
            {
                bool cannotMove = false;
                List<int> targetIndice = new List<int>();

                if (this.FileListBox.SelectedIndices.Contains(this.Files.Count - 1))
                    targetIndice.Add(this.Files.Count - 1);

                for (int i = this.FileListBox.SelectedIndices.Count - 1; i >= 0; i-- )
                {
                    var index = this.FileListBox.SelectedIndices[i];
                    cannotMove = ((int)index == this.Files.Count - 1) || cannotMove && this.FileListBox.SelectedIndices.Contains((int)index + 1);
                    if (!cannotMove)
                    {
                        int sourceIndex = (int)index;
                        int targetIndex = (int)index + 1;

                        targetIndice.Add(targetIndex);
                        this.Files.Move(sourceIndex, targetIndex);
                    }
                }
                SetListBox();
                foreach (var index in targetIndice)
                    this.FileListBox.SelectedIndices.Add(index);
            }
        }
    }
}
