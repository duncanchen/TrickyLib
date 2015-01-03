using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TrickyLib.IO;
using TrickyLib.Threading;

namespace TrickyLib.MachineLearning.Tools
{
    public class ToolAPI_Thread : BaseThread_One
    {
        public BaseAPI ToolAPI
        {
            get
            {
                return ((API_ThreadPool)Parent).ToolAPI;
            }
        }
        public TrainFileSetting FileSetting { get; protected set; }
        public RunMode Mode
        {
            get
            {
                return ((API_ThreadPool)Parent).Mode;
            }
        }
        public MetricSetting Metric
        {
            get
            {
                return ((API_ThreadPool)Parent).Metric;
            }
        }
        public MachineLearningTask Task
        {
            get
            {
                return ((API_ThreadPool)Parent).Task;
            }
        }
        public int FolderCount
        {
            get
            {
                return ((API_ThreadPool)Parent).FolderCount;
            }
        }
        private Process _process;

        public ToolAPI_Thread(API_ThreadPool parent, object[] items)
            : base(parent, items)
        {
            FileSetting = new TrainFileSetting()
            {
                TrainFile = parent.ExpSetting.TrainFile,
                TrainRawFile = parent.ExpSetting.TrainRawFile,

                ValidationFile = parent.ExpSetting.ValidationFile,
                ValidationRawFile = parent.ExpSetting.ValidationRawFile,

                TestFiles = parent.ExpSetting.TestFiles.Split(';'),
                TestRawFiles = parent.ExpSetting.TestRawFiles.Split(';'),

                AdditionalFiles = parent.ExpSetting.AdditionalFiles.Split(';'),
                GUID = parent.ThreadCount > 1 || Mode == RunMode.CrossValidation ? this.ThreadGUID : ""
            };
        }

        protected override object DoItem(object item)
        {
            var paraCombination = item as BaseParameterCombination;

            //Begin work
            if (Mode == RunMode.CrossValidation)
            {
                BaseExperimentSetting.ReceiveMessage(string.Format("[Thread {0}] Start cross validation, parameter: {1}", ThreadID, paraCombination.GetParameterString()));
                return ToolAPI.CrossValidation(Task, FileSetting, paraCombination, FolderCount, Metric, ThreadID.ToString(), out _process);
            }
            else if (Mode == RunMode.Train)
            {
                BaseExperimentSetting.ReceiveMessage(string.Format("[Thread {0}] Start train, parameter: {1}", ThreadID, paraCombination.GetParameterString()));
                return ToolAPI.NormalValiation(Task, FileSetting, paraCombination, Metric, ThreadID.ToString(), out _process);
            }
            else if (Mode == RunMode.Test)
            {
                BaseExperimentSetting.ReceiveMessage(string.Format("[Thread {0}] Start test", ThreadID));
                return ToolAPI.Test(Task, FileSetting, Metric, ThreadID.ToString(), out _process);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override void Stop()
        {
            base.Stop();

            try
            {
                _process.Kill();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
