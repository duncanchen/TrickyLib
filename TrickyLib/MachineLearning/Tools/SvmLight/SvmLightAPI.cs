using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrickyLib.Extension;
using TrickyLib.IO;
using TrickyLib.Service;

namespace TrickyLib.MachineLearning.Tools.SvmLight
{
    [Serializable]
    public class SvmLightAPI : BaseAPI
    {
        public static string TrainExePath { get; set; }
        public static string TestExePath { get; set; }

        static SvmLightAPI()
        {
            TrainExePath = "Tools\\Svm_Learn.exe";
            TestExePath = "Tools\\Svm_Classify.exe";
        }

        public SvmLightAPI()
        {
        
        }

        protected override void Learn(MachineLearningTask task, string trainFile, string modelFile, BaseParameterCombination paraCombination, out Process process)
        {
            string runParameter = GetRunParameterString_Learn(task, trainFile, modelFile, paraCombination);
            SoftwareOperator.Execute(TrainExePath, runParameter, -1, System.Diagnostics.ProcessWindowStyle.Minimized, true, out process);
        }

        protected override void Classify(MachineLearningTask task, string testFile, string modelFile, string outputFile, out Process process)
        {
            string runParameter = GetRunParameterString_Test(task, testFile, modelFile, outputFile);
            SoftwareOperator.Execute(TestExePath, runParameter, -1, System.Diagnostics.ProcessWindowStyle.Minimized, true, out process);
        }

        protected override List<List<string>> ReadFeatureFileToSamples(string featureFile)
        {
            return FeatureFileProcessor.ReadFeatureFile(featureFile);
        }

        protected override string GetRunParameterString_Learn(MachineLearningTask task, string trainFile, string modelFile, BaseParameterCombination paraCombination)
        {
            return string.Format("{0} {1} {2} {3}", task == MachineLearningTask.Ranking ? "-z p" : "", paraCombination.GetParameterString(), trainFile, modelFile).Trim();
        }

        protected override string GetRunParameterString_Test(MachineLearningTask task, string testFile, string modelFile, string outputFile)
        {
            return string.Format("{0} {1} {2}", testFile, modelFile, outputFile);
        }
    }
}
