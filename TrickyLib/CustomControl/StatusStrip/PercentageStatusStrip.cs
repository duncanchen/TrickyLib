using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TrickyLib.CustomControl.StatusStrip
{
    public partial class PercentageStatusStrip : UserControl
    {
        public PercentageStatusStrip()
        {
            InitializeComponent();

            // To report progress from the background worker we need to set this property
            BusyBackgroundWorker.WorkerReportsProgress = true;
            BusyBackgroundWorker.WorkerSupportsCancellation = true;
            // This event will be raised on the worker thread when the worker starts
            BusyBackgroundWorker.DoWork += new DoWorkEventHandler(BusyBackgroundWorker_DoWork);            
            // This event will be raised when we call ReportProgress
            BusyBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BusyBackgroundWorker_ProgressChanged);

            BusyBackgroundWorker.RunWorkerCompleted += BusyBackgroundWorker_RunWorkerCompleted;

        }

        void BusyBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        public void SetIsBusy(bool isBusy, string processingText = "")
        {
            BeginInvoke(new Action(() =>
                {
                    this.PercentageProgressBar.Visible = isBusy;
                    this.PercentageLabel.Visible = false;
                    this.PercentageProgressBar.Style = isBusy ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
                }));
            SetProcessingText(processingText);

            if (isBusy && !BusyBackgroundWorker.IsBusy)
                BusyBackgroundWorker.RunWorkerAsync();
            else if (!isBusy && BusyBackgroundWorker.IsBusy)
                BusyBackgroundWorker.CancelAsync();
        }
        public bool GetIsBusy()
        {
            return BusyBackgroundWorker.IsBusy;
        }

        // On worker thread so do our thing!
        void BusyBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            // Your background task goes here
            while(true)
            {
                //If detect the 
                if (BusyBackgroundWorker.CancellationPending)
                    return;

                // Report progress to 'UI' thread
                BusyBackgroundWorker.ReportProgress(i++ % 101);
                i %= 101;

                // Simulate long task
                System.Threading.Thread.Sleep(100);
            }
        }
        // Back on the 'UI' thread so we can update the progress bar
        void BusyBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // The progress percentage is a property of e
            try
            {
                this.PercentageProgressBar.Value = e.ProgressPercentage;
            }
            catch (Exception ex)
            {

            }
        }

        public delegate void PropertyChangedHandler(object sender, string labelText);
        private event PropertyChangedHandler _propertyChanged;
        public event PropertyChangedHandler PropertyChanged
        {
            add
            {
                if (_propertyChanged == null || !_propertyChanged.GetInvocationList().Contains(value))
                    _propertyChanged += value;
            }
            remove
            {
                _propertyChanged -= value;
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
                SetPercentage(value);
            }
        }

        public string PercentageAndProcessText
        {
            get
            {
                return (PercentageLabel.Text + " " + ProcessingText).Trim();
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
                SetProcessingText(value);
            }
        }

        private void SetPercentage(double percentage)
        {
            try
            {
                SetVisibility_Percentage(true);
                BeginInvoke(new Action(() =>
                    {
                        lock (this)
                        {
                            if (percentage < 0)
                                percentage = 0;
                            else if (percentage > 1)
                                percentage = 1;

                            this.PercentageProgressBar.Value = (int)(percentage * this.PercentageProgressBar.Maximum);

                            if (percentage >= 0)
                            {
                                try
                                {
                                    string percentageText = string.Format("{0:N1}%", percentage * 100);
                                    this.PercentageLabel.Text = percentageText;

                                    if (_propertyChanged != null)
                                        _propertyChanged(this, PercentageAndProcessText);
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
                    }));
            }
            catch (Exception ex)
            {

            }
        }
        private void SetProcessingText(string text)
        {
            try
            {
                SetVisibility_ProcessingText(true);
                BeginInvoke(new Action(() =>
                {
                    this.ProcessingLabel.Text = text;
                }));

                if (_propertyChanged != null)
                    _propertyChanged(this, PercentageAndProcessText);
            }
            catch (Exception ex)
            {

            }
        }

        public void SetVisibility_Percentage(bool visible)
        {
            try
            {
                BeginInvoke(new Action(() =>
                    {
                        this.PercentageProgressBar.Visible = visible;
                        this.PercentageLabel.Visible = visible;
                    }));
            }
            catch (Exception ex)
            {

            }
        }
        public void SetVisibility_ProcessingText(bool visible)
        {
            try
            {
                BeginInvoke(new Action(() => this.ProcessingLabel.Visible = visible));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
