using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrickyLib.WinControl
{
    public partial class ProgressLabelBar : UserControl
    {
        public delegate void ProgressPercentageChangedHandler(object sender, string labelText);
        public event ProgressPercentageChangedHandler ProgressPercentageChanged;

        public int ProgressValue
        {
            get
            {
                return this.ProgressBar.Value;
            }
            set
            {
                this.ProgressBar.Value = value;
            }
        }
        public int ProgressMaximum
        {
            get
            {
                return this.ProgressBar.Maximum;
            }
            set
            {
                this.ProgressBar.Maximum = value;
            }
        }

        public double _progressPercentage;
        public double ProgressPercentage
        {
            get
            {
                return this._progressPercentage;
            }
            set
            {
                this._progressPercentage = value;
                this.ProgressValue = (int)(value * this.ProgressMaximum);

                if (value > 0)
                {
                    try
                    {
                        string percentageText = string.Format("{0:N1}%", value * 100);
                        this.PercentageLabel.Text = percentageText;

                        if (this.ProgressPercentageChanged != null)
                            this.ProgressPercentageChanged(this, this.PercentageLabel.Text);
                    }
                    catch (Exception ex)
                    { 
                        
                    }
                }
                else
                {
                    this.PercentageLabel.Text = string.Empty;
                    this.ProcessingText = string.Empty;
                }
            }
        }

        public string ProgressPercentageText
        {
            get
            {
                return this.PercentageLabel.Text;
            }
        }

        public string _processingText;
        public string ProcessingText
        {
            get
            {
                return this._processingText;
            }
            set
            {
                this._processingText = value;
                this.ProcessingLabel.Text = value;
            }
        }

        public ProgressLabelBar()
        {
            InitializeComponent();
        }
    }
}
