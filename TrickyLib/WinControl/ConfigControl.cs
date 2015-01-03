using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrickyLib.Reflection;
using System.IO;

namespace TrickyLib.WinControl
{
    public partial class ConfigControl : UserControl
    {
        public const string LastConfigPath = "SimpleSvmForm.LastConfig.txt";

        public string ConfigFile
        {
            get
            {
                return this.ConfigTextBox.FilePath;
            }
            set
            {
                this.ConfigTextBox.FilePath = value;
            }
        }

        public ConfigControl()
        {
            InitializeComponent();
            this.Load += new EventHandler(ConfigControl_Load);
            this.ConfigTextBox.FilePathChanged += new FileBrowserTextBox.FilePathChangedHandler(ConfigTextBox_FilePathChanged);
            this.ConfigTextBox.InitialDirectory = System.Environment.CurrentDirectory;
        }

        void ConfigControl_Load(object sender, EventArgs e)
        {
            if (File.Exists(LastConfigPath))
            {
                using (StreamReader sr = new StreamReader(LastConfigPath))
                    this.ConfigFile = sr.ReadLine().Trim();

                if (File.Exists(this.ConfigFile))
                    LoadConfigButton_Click(null, null);
            }
        }

        void ConfigTextBox_FilePathChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(this.ConfigFile))
                this.Parent.Text = Path.GetFileNameWithoutExtension(this.ConfigFile);
        }

        private void OpenDirButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(System.Environment.CurrentDirectory);
        }

        private void SaveConfigButton_Click(object sender, EventArgs e)
        {
            if (this.ConfigFile.Trim() == string.Empty)
                MessageBox.Show("ConfigTextBox cannot be empty");
            else
            {
                ReflectionHandler.SavePropertiesConfig(this.ConfigFile, this.Parent, 2);
                SaveLastConfig();
            }
        }

        private void LoadConfigButton_Click(object sender, EventArgs e)
        {
            if (this.ConfigFile.Trim() == string.Empty)
            {
                MessageBox.Show("ConfigTextBox cannot be empty");
                return;
            }

            if (File.Exists(this.ConfigFile.Trim()))
            {
                ReflectionHandler.LoadPropertiesConfig(this.ConfigFile, this.Parent);
                SaveLastConfig();
            }
            else
                MessageBox.Show(string.Format("{0} does not exist", this.ConfigFile.Trim()));
        }

        private void SaveLastConfig()
        {
            if (this.ConfigFile.Trim() != string.Empty)
            {
                using (StreamWriter sw = new StreamWriter(LastConfigPath))
                {
                    sw.WriteLine(this.ConfigFile.Trim());
                }
            }
        }
    }
}
