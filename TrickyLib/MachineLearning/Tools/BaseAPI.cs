using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.IO;
using TrickyLib.Extension;
using TrickyLib.Service;
using System.Diagnostics;

namespace TrickyLib.MachineLearning.Tools
{
    [Serializable]
    public abstract class BaseAPI
    {
        public delegate TrainTestResult GetCustomizePerformanceHandler(string featureFile, string outputFile, string rawFile, string[] additionalFiles);
        public GetCustomizePerformanceHandler GetCustomizePerformance { get; set; }

        public TrainTestResult CrossValidation(MachineLearningTask task, TrainFileSetting fileSetting, BaseParameterCombination paraCombination, int foldCount, MetricSetting metric, string threadID = "")
        {
            Process process;
            return CrossValidation(task, fileSetting, paraCombination, foldCount, metric, threadID, out process);
        }
        public TrainTestResult CrossValidation(MachineLearningTask task, TrainFileSetting fileSetting, BaseParameterCombination paraCombination, int foldCount, MetricSetting metric, string threadID, out Process process)
        {
            List<List<string>> samples = ReadFeatureFileToSamples(fileSetting.TrainFile);
            if (foldCount > samples.Count)
                throw new ArgumentOutOfRangeException("folder count should be less than samples count");

            int per = samples.Count / foldCount;
            process = null;
            TrainTestResult trainTestResult = new TrainTestResult()
            {
                Argu = paraCombination.GetParameterString(),
                FolderCount = foldCount
            };

            string tempTrainFile = FilePath.AddExtention(fileSetting.TrainFile, fileSetting.GUID + ".crossTrain", 1);
            string tempTestFile = FilePath.AddExtention(fileSetting.TrainFile, fileSetting.GUID + ".crossTest", 1);

            TrainFileSetting crossFileSetting = new TrainFileSetting()
            {
                TrainFile = tempTrainFile,
                TrainRawFile = fileSetting.TrainRawFile,

                ValidationFile = tempTrainFile,
                ValidationRawFile = fileSetting.TrainRawFile,

                TestFiles = new string[] { tempTestFile },
                TestRawFiles = new string[] { fileSetting.TrainRawFile },

                AdditionalFiles = fileSetting.AdditionalFiles
            };

            for (int i = 0; i < foldCount; i++)
            {
                int testStartIndex = i * per;
                int testLength = i != foldCount ? per : (samples.Count - (foldCount - 1) * per);

                List<string> trainSet = new List<string>();
                List<string> testSet = new List<string>();

                trainSet.AddRange(samples.GetRange(0, testStartIndex).ToTension());
                trainSet.AddRange(samples.GetRange(testStartIndex + testLength, samples.Count - testStartIndex - testLength).ToTension());

                if (i < foldCount - 1)
                    testSet.AddRange(samples.GetRange(testStartIndex, testLength).ToTension());
                else
                    testSet.AddRange(samples.GetRange(testStartIndex, samples.Count - testStartIndex).ToTension());

                FileWriter.PrintCollection(tempTrainFile, trainSet);
                FileWriter.PrintCollection(tempTestFile, testSet);

                var result = NormalValiation(task, crossFileSetting, paraCombination, metric, threadID + "-" + (i + 1), out process);

                //Record the result
                if (trainTestResult.Properties.Count <= 0)
                {
                    trainTestResult.AddRange(result);
                }
                else
                {
                    for (int j = 0; j < result.PropertyNames.Count; j++)
                        trainTestResult.Properties[j] += result.Properties[j];
                }
            }

            //for (int j = 0; j < trainTestResult.PropertyNames.Count; j++)
            //    trainTestResult.Properties[j] /= foldCount;

            return trainTestResult;
        }

        public TrainTestResult NormalValiation(MachineLearningTask task, TrainFileSetting fileSetting, BaseParameterCombination paraCombination, MetricSetting metric, string threadID = "")
        {
            Process process;
            return NormalValiation(task, fileSetting, paraCombination, metric, threadID, out process);
        }
        public TrainTestResult NormalValiation(MachineLearningTask task, TrainFileSetting fileSetting, BaseParameterCombination paraCombination, MetricSetting metric, string threadID, out Process process)
        {
            string threadIDString = "";
            if (!string.IsNullOrEmpty(threadID))
                threadIDString = string.Format("[Thread {0}]", threadID);

            BaseExperimentSetting.ReceiveMessage(string.Format("{0} Training ...", threadIDString).Trim());
            Learn(task, fileSetting.TrainFile, fileSetting.ModelFile, paraCombination, out process);

            var testResult = Test(task, fileSetting, metric, threadID);
            testResult.Argu = paraCombination.GetParameterString();

            return testResult;
        }

        public TrainTestResult Test(MachineLearningTask task, TrainFileSetting fileSetting, MetricSetting metric, string threadID = "")
        {
            Process process;
            return Test(task, fileSetting, metric, threadID, out process);
        }
        public TrainTestResult Test(MachineLearningTask task, TrainFileSetting fileSetting, MetricSetting metric, string threadID, out Process process)
        {
            if (!string.IsNullOrEmpty(threadID))
                threadID = string.Format("[Thread {0}]", threadID);

            TrainTestResult testResult = new TrainTestResult();
            process = null;

            if (fileSetting.ValidationFile != "")
            {

                BaseExperimentSetting.ReceiveMessage(string.Format("{0} Testing validation file ...", threadID).Trim());
                Classify(task, fileSetting.ValidationFile, fileSetting.ModelFile, fileSetting.ValidationOutputFile, out process);

                BaseExperimentSetting.ReceiveMessage(string.Format("{0} Calculating selected performance for validation file ...", threadID).Trim());
                testResult.AddRange(metric.GetSelectedPerformace(
                    fileSetting.ValidationFile,
                    fileSetting.ValidationOutputFile)
                    .InsertHeader("Dev"));

                if (GetCustomizePerformance != null)
                {
                    BaseExperimentSetting.ReceiveMessage(string.Format("{0} Calculating customized performance for validation file ...", threadID).Trim());
                    var customizePerformance = GetCustomizePerformance(fileSetting.ValidationFile
                        , fileSetting.ValidationOutputFile
                        , fileSetting.ValidationRawFile
                        , fileSetting.AdditionalFiles);

                    if (customizePerformance != null)
                        testResult.AddRange(customizePerformance.InsertHeader("Dev_"));
                }
            }

            if (fileSetting.TestFiles != null)
            {
                for (int i = 0; i < fileSetting.TestFiles.Length; ++i)
                {
                    BaseExperimentSetting.ReceiveMessage(string.Format("{0} Testing test file {1}...", threadID, fileSetting.TestFiles.Length > 1 ? (i + 1).ToString() : "").Trim());
                    Classify(task, fileSetting.TestFiles[i], fileSetting.ModelFile, fileSetting.TestOutputFiles[i], out process);

                    BaseExperimentSetting.ReceiveMessage(string.Format("{0} Calculating selected performance for test file {1}...", threadID, fileSetting.TestFiles.Length > 1 ? (i + 1).ToString() : "").Trim());
                    testResult.AddRange(metric.GetSelectedPerformace(
                        fileSetting.TestFiles[i],
                        fileSetting.TestOutputFiles[i])
                        .InsertHeader("Test" + i.ToString() + "_"));

                    if (GetCustomizePerformance != null)
                    {
                        BaseExperimentSetting.ReceiveMessage(string.Format("{0} Calculating customized performance for test file {1}...", threadID, fileSetting.TestFiles.Length > 1 ? (i + 1).ToString() : "").Trim());
                        var cutomizePerformance = GetCustomizePerformance(
                            fileSetting.TestFiles[i],
                            fileSetting.TestOutputFiles[i],
                            i < fileSetting.TestRawFiles.Length ? fileSetting.TestRawFiles[i] : string.Empty,
                            fileSetting.AdditionalFiles);

                        if (cutomizePerformance != null)
                            testResult.AddRange(cutomizePerformance.InsertHeader("Test" + i.ToString() + "_"));
                    }
                }
            }

            return testResult;
        }

        protected abstract void Learn(MachineLearningTask task, string trainFile, string modelFile, BaseParameterCombination paraCombination, out Process process);
        protected abstract void Classify(MachineLearningTask task, string testFile, string modelFile, string outputFile, out Process process);
        protected abstract List<List<string>> ReadFeatureFileToSamples(string featureFile);

        protected virtual string GetRunParameterString_Learn(MachineLearningTask task, string trainFile, string modelFile, BaseParameterCombination paraCombination) { return string.Empty; }
        protected virtual string GetRunParameterString_Test(MachineLearningTask task, string testFile, string modelFile, string outputFile) { return string.Empty; }
    }
}
