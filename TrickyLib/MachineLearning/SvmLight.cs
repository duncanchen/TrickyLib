using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Service;
using TrickyLib.IO;
using System.IO;
using System.Text.RegularExpressions;
using TrickyLib.Extension;
using System.Diagnostics;
using TrickyLib.Struct;

namespace TrickyLib.MachineLearning
{
    public class SvmLight
    {
        public delegate TrainTestResult CustomizeCalculatePerformance(string modelFile, string featureFile, string outputFile, string rawFeatureFile, string additionalFile);
        public static string SVM_LearnExePath = FilePath.ChangeDriverToCurrentProgram(@"F:\users\v-haowu\Tools\svm_learn.exe");
        public static string SVM_ClassifyExePath = FilePath.ChangeDriverToCurrentProgram(@"F:\users\v-haowu\Tools\svm_classify.exe");

        public static string SVM_Rank_Learn_FastExePath = FilePath.ChangeDriverToCurrentProgram(@"F:\users\v-haowu\Tools\Svm_Rank_Learn_Fast.exe");
        public static string SVM_Rank_Classify_FastExePath = FilePath.ChangeDriverToCurrentProgram(@"F:\users\v-haowu\Tools\svm_rank_classify_Fast.exe");

        public static string Eval_ScoreExePath = FilePath.ChangeDriverToCurrentProgram(@"F:\users\v-haowu\Tools\Eval-Score-3.0.pl");
        public static string Eval_TTestExePath = FilePath.ChangeDriverToCurrentProgram(@"F:\users\v-haowu\Tools\Eval-TTest-3.0.pl");
        public static string[] DefaultPropertyNames = new string[] { "Precision", "Recall", "F-Mesure", "ErrorOccurs" };

        public static string LearnSVM(string svm_argu, string featureFile, string modelFile, TrainMethods trainMethod)
        {
            string trainArgu = svm_argu + " " + featureFile + " " + modelFile;
            return SoftwareOperator.Execute(
                trainMethod == TrainMethods.FastRankSvm ? SVM_Rank_Learn_FastExePath : SVM_LearnExePath,
                trainArgu,
                -1,
                System.Diagnostics.ProcessWindowStyle.Minimized,
                false);
        }

        public static TrainTestResult ClassifySVM(string featureFile, string modelFile, string outputFile, TrainMethods trainMethod, List<FeatureLine> instances = null)
        {
            try
            {
                if (trainMethod == TrainMethods.FastRankSvm)
                {
                    SVMModel model = new SVMModel();
                    if (instances != null && instances.Count > 0)
                        model.Classify(instances, modelFile, outputFile);
                    else
                        model.Classify(featureFile, modelFile, outputFile);

                    return new TrainTestResult();
                }

                string argu = featureFile + " " + modelFile + " " + outputFile;
                string consoleOutput = SoftwareOperator.Execute(
                    trainMethod == TrainMethods.FastRankSvm ? SVM_Rank_Classify_FastExePath : SVM_ClassifyExePath,
                    argu,
                    -1,
                    ProcessWindowStyle.Hidden,
                    true);

                string lastLine = consoleOutput.Split('\n').Last().Trim();
                if (lastLine.Contains("Precision/recall on test set:"))
                {
                    string[] pAndr = lastLine.Trim("Precision/recall on test set:".ToArray()).Trim().Split('/');

                    double p100 = 0;
                    bool successP = double.TryParse(pAndr[0].Trim('%'), out p100);
                    double p = p100 / 100;

                    double r100 = 0;
                    bool successR = double.TryParse(pAndr[1].Trim('%'), out r100);
                    double r = r100 / 100;

                    double f = successP && successR ? 2 * p * r / (p + r) : 0;

                    TrainTestResult testResult = new TrainTestResult(DefaultPropertyNames);
                    testResult.Properties.Add(p);
                    testResult.Properties.Add(r);
                    testResult.Properties.Add(f);
                    testResult.Properties.Add(successP && successR ? 0 : 1);
                    return testResult;
                }
                else
                    return new TrainTestResult();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static TrainTestResult CrossValidation(SvmLightInput input)
        {
            List<List<string>> samples = FeatureFileProcessor.ReadFeatureFile(input.TrainFeatureFile);
            if (input.FolderCount <= 1 || input.FolderCount > samples.Count)
                throw new ArgumentOutOfRangeException("folder count should be larger than 1 and not larger than samples count");

            string[] crossTempFiles = GetCrossValidationTempFiles(input.TrainFeatureFile, input.TrainMethod, input.ThreadGUID);
            string tempTrainFeatureFile = crossTempFiles[0];
            string tempTestFeatureFile = crossTempFiles[1];
            string tempTrainOutputFile = crossTempFiles[2];
            string tempTestOutputFile = crossTempFiles[3];
            string tempModelFile = crossTempFiles[4];

            int per = samples.Count / input.FolderCount;

            TrainTestResult trainTestResult = null;

            for (int i = 0; i < input.FolderCount; i++)
            {
                int testStartIndex = i * per;
                int testLength = i != input.FolderCount ? per : (samples.Count - (input.FolderCount - 1) * per);

                List<string> trainSet = new List<string>();
                List<string> testSet = new List<string>();

                trainSet.AddRange(samples.GetRange(0, testStartIndex).ToTension());
                trainSet.AddRange(samples.GetRange(testStartIndex + testLength, samples.Count - testStartIndex - testLength).ToTension());
                testSet.AddRange(samples.GetRange(testStartIndex, testLength).ToTension());

                FileWriter.PrintCollection(tempTrainFeatureFile, trainSet);
                FileWriter.PrintCollection(tempTestFeatureFile, testSet);

                Console.WriteLine("[Thread " + input.ThreadID + "]" + " Cross training...");
                LearnSVM(input.Argument, tempTrainFeatureFile, tempModelFile, input.TrainMethod);
                Console.WriteLine("[Thread " + input.ThreadID + "]" + " Finished Cross training");

                Console.WriteLine("[Thread " + input.ThreadID + "]" + " Cross classify temp TRAIN set...");
                TrainTestResult trainClassify = ClassifySVM(tempTrainFeatureFile, tempModelFile, tempTrainOutputFile, input.TrainMethod).InsertHeader("Train");
                Console.WriteLine("[Thread " + input.ThreadID + "]" + " Finished Cross classify temp TRAIN set");

                Console.WriteLine("[Thread " + input.ThreadID + "]" + " Cross calculate temp TRAIN performance...");
                TrainTestResult trainPerf = input.PerformanceCalculator(tempModelFile, tempTrainFeatureFile, tempTrainOutputFile, input.TrainRawFile, input.AdditionalFile).InsertHeader("Train");
                Console.WriteLine("[Thread " + input.ThreadID + "]" + " Finished Cross calculate temp TRAIN performance");

                Console.WriteLine("[Thread " + input.ThreadID + "]" + " Cross classify temp TEST set...");
                TrainTestResult testClassify = ClassifySVM(tempTestFeatureFile, tempModelFile, tempTestOutputFile, input.TrainMethod).InsertHeader("Test");
                Console.WriteLine("[Thread " + input.ThreadID + "]" + " Finihshed Cross classify temp TEST set");

                Console.WriteLine("[Thread " + input.ThreadID + "]" + " Cross calculate temp TEST performance...");
                TrainTestResult testPerf = input.PerformanceCalculator(tempModelFile, tempTestFeatureFile, tempTestOutputFile, input.TrainRawFile, input.AdditionalFile).InsertHeader("Test");
                Console.WriteLine("[Thread " + input.ThreadID + "]" + " Finished Cross calculate temp TEST performance");

                TrainTestResult tempResult = new TrainTestResult() { Argu = input.Argument, FolderCount = input.FolderCount };
                tempResult.AddRange(trainClassify);
                tempResult.AddRange(trainPerf);
                tempResult.AddRange(testClassify);
                tempResult.AddRange(testPerf);

                if (trainTestResult == null)
                    trainTestResult = tempResult;
                else
                {
                    for (int j = 0; j < tempResult.PropertyNames.Count; j++)
                        trainTestResult.Properties[j] += tempResult.Properties[j];
                }
            }

            trainTestResult.FolderCount = input.FolderCount;
            for (int j = 0; j < trainTestResult.PropertyNames.Count; j++)
                trainTestResult.Properties[j] /= input.FolderCount;

            return trainTestResult;
        }

        public static TrainTestResult NormalValidation(SvmLightInput input)
        {
            string[] normalTempFiles = GetNormalValidationFiles(input.TrainFeatureFile, input.DevFeatureFile, input.TrainMethod, input.ThreadGUID);
            string modelFile = normalTempFiles[0];
            string devOutputFile = normalTempFiles[1];

            string devHeader = "Dev";
            string testHeader = "Test";

            if(input.IsMultiThread) Console.WriteLine("[Thread " + input.ThreadID + "]" + " Train...");
            string informantion = LearnSVM(input.Argument, input.TrainFeatureFile, modelFile, input.TrainMethod);
            if(input.IsMultiThread) Console.WriteLine("[Thread " + input.ThreadID + "]" + " Finished Train");

            if (input.IsMultiThread) Console.WriteLine("[Thread " + input.ThreadID + "]" + " Classify DEV set");
            TrainTestResult devClassify = ClassifySVM(input.DevFeatureFile, modelFile, devOutputFile, input.TrainMethod, input.DevFeatureLines).InsertHeader(devHeader);
            if (input.IsMultiThread) Console.WriteLine("[Thread " + input.ThreadID + "]" + " Finished Classify DEV set");

            if (input.IsMultiThread) Console.WriteLine("[Thread " + input.ThreadID + "]" + " Calculate DEV set performance");
            TrainTestResult devPerf = input.PerformanceCalculator(modelFile, input.DevFeatureFile, devOutputFile, input.DevRawFile, input.AdditionalFile).InsertHeader(devHeader);
            if (input.IsMultiThread) Console.WriteLine("[Thread " + input.ThreadID + "]" + " Finshed Calculate DEV set performance");

            TrainTestResult trainTestResult = new TrainTestResult() { Argu = input.Argument, FolderCount = 1 };
            trainTestResult.AddRange(devClassify);
            trainTestResult.AddRange(devPerf);

            if (input.TestFeatureFiles != null)
            {
                for (int i = 0; i < input.TestFeatureFiles.Length; ++i)
                {
                    string testOutputFile = GetNormalValidationFiles(input.TrainFeatureFile, input.TestFeatureFiles[i], input.TrainMethod, input.ThreadGUID)[1];

                    if (input.IsMultiThread) Console.WriteLine("[Thread " + input.ThreadID + "]" + " Classify TEST set" + (input.TestFeatureFiles.Length > 1 ? i.ToString() : "") + "...");
                    TrainTestResult testClassify = ClassifySVM(input.TestFeatureFiles[i], modelFile, testOutputFile, input.TrainMethod, input.TestFeatureLinesArray != null && input.TestFeatureLinesArray.Length > i ? input.TestFeatureLinesArray[i] : null).InsertHeader(testHeader + " " + (input.TestFeatureFiles.Length > 1 ? i.ToString() : ""));
                    if (input.IsMultiThread) Console.WriteLine("[Thread " + input.ThreadID + "]" + " Finished Classify TEST set" + (input.TestFeatureFiles.Length > 1 ? i.ToString() : "") + "");

                    if (input.IsMultiThread) Console.WriteLine("[Thread " + input.ThreadID + "]" + " Calculate TEST set" + (input.TestFeatureFiles.Length > 1 ? i.ToString() : "") + " performance...");
                    TrainTestResult testPerf = input.PerformanceCalculator(modelFile, input.TestFeatureFiles[i], testOutputFile, input.TestRawFiles != null && input.TestRawFiles.Length > i ? input.TestRawFiles[i] : null, input.AdditionalFile).InsertHeader(testHeader + " " + (input.TestFeatureFiles.Length > 1 ? i.ToString() : ""));
                    if (input.IsMultiThread) Console.WriteLine("[Thread " + input.ThreadID + "]" + " Finished Calculate TEST set" + (input.TestFeatureFiles.Length > 1 ? i.ToString() : "") + " performance");

                    trainTestResult.AddRange(testClassify);
                    trainTestResult.AddRange(testPerf);
                }
            }

            return trainTestResult;
        }

        public static string EvalScore_MAP_NDCG(string featureFile, string svmOutputFile, bool concise)
        {
            string evalScoreFile = SvmLight.GetSvmFileName(svmOutputFile, MLFileTypes.EvalScore);
            string trainArgu = featureFile + " " + svmOutputFile + " " + evalScoreFile + " " + (concise ? 0 : 1);
            SoftwareOperator.Execute(Eval_ScoreExePath, trainArgu, -1, System.Diagnostics.ProcessWindowStyle.Normal, true);

            return evalScoreFile;
        }

        public static string EvalTTest_MAP_NDCG(string comparerEvalScoreFile, string baselineEvalScoreFile, bool reverse = false)
        {
            string ttestFile = SvmLight.GetSvmFileName(comparerEvalScoreFile, MLFileTypes.TTest);
            string argu = "";
            if (!reverse)
                argu = comparerEvalScoreFile + " " + baselineEvalScoreFile + " " + ttestFile;
            else
                argu = baselineEvalScoreFile + " " + comparerEvalScoreFile + " " + ttestFile;
            SoftwareOperator.Execute(Eval_TTestExePath, argu, -1, System.Diagnostics.ProcessWindowStyle.Normal, true);

            return ttestFile;
        }

        public static string GetSvmFileName(string file, MLFileTypes svmFile)
        {
            return FilePath.ChangeExtension(file, svmFile.ToString());
        }

        public static string GetSvmFileName(string file, MLFileTypes svmFile, string header)
        {
            return FilePath.ChangeExtension(file, header + "." + svmFile.ToString().ToLowerInvariant());
        }

        public static string[] GetCrossValidationTempFiles(string featureFile, TrainMethods trainMethod, string threadID)
        {
            string header = trainMethod.ToString() + (threadID == "" ? "" : ".") + threadID;
            string tempTrainFeatureFile = FilePath.ChangeExtension(featureFile, header + ".cross.tempTrain." + MLFileTypes.Feature);
            string tempTestFeatureFile = FilePath.ChangeExtension(featureFile, header + ".cross.tempTest." + MLFileTypes.Feature);
            string tempTrainOutputFile = GetSvmFileName(tempTrainFeatureFile, MLFileTypes.Output);
            string tempTestOutputFile = GetSvmFileName(tempTestFeatureFile, MLFileTypes.Output);
            string tempModelFile = GetSvmFileName(tempTrainFeatureFile, MLFileTypes.Model);
            string tempTrainEvalFile = SvmLight.GetSvmFileName(tempTrainOutputFile, MLFileTypes.EvalScore);
            string tempTrainTTestFile = SvmLight.GetSvmFileName(tempTrainOutputFile, MLFileTypes.TTest);
            string tempTestEvalFile = SvmLight.GetSvmFileName(tempTestOutputFile, MLFileTypes.EvalScore);
            string tempTestTTestFile = SvmLight.GetSvmFileName(tempTestOutputFile, MLFileTypes.TTest);
            string tempTrainWinLossFile = SvmLight.GetSvmFileName(tempTrainOutputFile, MLFileTypes.WinLoss);
            string tempTestWinLossFile = SvmLight.GetSvmFileName(tempTestOutputFile, MLFileTypes.WinLoss);

            return new string[] {
                tempTrainFeatureFile,
                tempTestFeatureFile,
                tempTrainOutputFile,
                tempTestOutputFile,
                tempModelFile,
                tempTrainEvalFile,
                tempTestEvalFile,
                tempTrainTTestFile,
                tempTestTTestFile,
                tempTrainWinLossFile,
                tempTestWinLossFile
            };
        }

        public static string[] GetNormalValidationFiles(string trainFeatureFile, string testFeatureFile, TrainMethods trainMethod, string threadID = "")
        {
            string header = trainMethod.ToString() + (threadID == "" ? "" : ".") + threadID;
            string modelFile = GetSvmFileName(trainFeatureFile, MLFileTypes.Model, header);
            string outputFile = GetSvmFileName(FilePath.ChangeFile(testFeatureFile, FilePath.MergeDistinctFilePathPart(trainFeatureFile, testFeatureFile) + "."), MLFileTypes.Output, header);
            string evalFile = SvmLight.GetSvmFileName(outputFile, MLFileTypes.EvalScore);
            string ttestFile = SvmLight.GetSvmFileName(outputFile, MLFileTypes.TTest);
            string winLossFile = SvmLight.GetSvmFileName(outputFile, MLFileTypes.WinLoss);

            return new string[] {
                modelFile,
                outputFile,
                evalFile,
                ttestFile,
                winLossFile
            };
        }

        public static void BuldPrecisionMapNDCGDicFromEvalScoreFile(string evalScoreFile, out Dictionary<int, List<double>> precisionDic, out Dictionary<int, double> mapDic, out Dictionary<int, List<double>> ndcgDic)
        {
            precisionDic = new Dictionary<int, List<double>>();
            mapDic = new Dictionary<int, double>();
            ndcgDic = new Dictionary<int, List<double>>();

            using (StreamReader sr = new StreamReader(evalScoreFile))
            {
                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split('\t');
                    if (lineArray[0].Contains(' '))
                    {
                        var items = lineArray[0].Split(' ');

                        string type = items[0].ToLower();
                        int id = int.Parse(items[2].ToLower().Trim("query:".ToArray()));

                        if (type == "precision")
                        {
                            List<double> values = new List<double>();
                            for (int i = 1; i < lineArray.Length; ++i)
                            {
                                if (lineArray[i] != string.Empty)
                                    values.Add(double.Parse(lineArray[i]));
                            }

                            precisionDic.Add(id, values);
                        }
                        else if (type == "map")
                        {
                            mapDic.Add(id, double.Parse(lineArray[1]));
                        }
                        else if (type == "ndcg")
                        {
                            List<double> values = new List<double>();
                            for (int i = 1; i < lineArray.Length; ++i)
                            {
                                if (lineArray[i] != string.Empty)
                                    values.Add(double.Parse(lineArray[i]));
                            }

                            ndcgDic.Add(id, values);
                        }
                    }
                }
            }
        }
    }

    public class TrainTestResult
    {
        public string Argu { get; set; }
        public int FolderCount { get; set; }

        public List<string> PropertyNames { get; set; }
        public List<double> Properties { get; set; }

        public TrainTestResult()
        {
            this.PropertyNames = new List<string>();
            this.Properties = new List<double>();
        }

        public TrainTestResult(IEnumerable<string> propertyNames, IEnumerable<double> properties = null)
        {
            this.FolderCount = 1;
            this.PropertyNames = new List<string>();
            this.Properties = new List<double>();
            
            foreach (var pn in propertyNames)
                this.PropertyNames.Add(pn);

            if (properties != null)
            {
                if (properties.Count() == propertyNames.Count())
                    foreach (var p in properties)
                        this.Properties.Add(p);
                else
                    throw new Exception("The array length is not match: properties.Length != propertyNames.Length");
            }
        }

        public TrainTestResult(string propertyName, double property)
        {
            this.FolderCount = 1;
            this.PropertyNames = new List<string>();
            this.Properties = new List<double>();
            this.PropertyNames.Add(propertyName);
            this.Properties.Add(property);
        }

        public override string ToString()
        {
            string output = this.Argu;
            output += "\t" + this.FolderCount;

            for (int i = 0; i < this.Properties.Count; i++)
                output += "\t" + (this.Properties[i] != 0 ? this.Properties[i].ToString() : string.Empty);

            return output;
        }

        public string ToConsoleString(string header)
        {
            string output = header + "\r\n" + (this.Argu == string.Empty ? string.Empty : string.Format("Argu\t{0}\r\n", this.Argu));
            for (int i = 0; i < this.PropertyNames.Count; i++)
                output += string.Format("{0}\t{1}\r\n", this.PropertyNames[i], this.Properties[i]);

            return output;
        }

        public void Add(string propertyName, double property)
        {
            if (this.Properties.Count != this.PropertyNames.Count)
                throw new Exception("Source TrainTestResult Error: Properties.Length != PropertyNames.Length");

            this.PropertyNames.Add(propertyName);
            this.Properties.Add(property);
        }

        public void AddRange(TrainTestResult item)
        {
            if (item == null || item.Properties == null || item.PropertyNames == null)
                return;

            if (item.Properties.Count != item.PropertyNames.Count)
                throw new Exception("Target TrainTestResult Error: Properties.Length != PropertyNames.Length");

            if (this.Properties.Count != this.PropertyNames.Count)
                throw new Exception("Source TrainTestResult Error: Properties.Length != PropertyNames.Length");

            this.PropertyNames.AddRange(item.PropertyNames);
            this.Properties.AddRange(item.Properties);
        }

        public void AddRange(IEnumerable<string> propertyNames, IEnumerable<double> properties)
        {
            if (propertyNames == null || properties == null)
                return;

            if (propertyNames.Count() != properties.Count())
                throw new Exception("Target TrainTestResult Error: Properties.Length != PropertyNames.Length");

            if (this.Properties.Count != this.PropertyNames.Count)
                throw new Exception("Source TrainTestResult Error: Properties.Length != PropertyNames.Length");

            this.PropertyNames.AddRange(propertyNames);
            this.Properties.AddRange(properties);
        }

        public string[] GetPropertyNames()
        {
            if (this.PropertyNames == null || this.PropertyNames.Count == 0)
                return new string[0];
            else
            {
                List<string> propertyNames = new List<string>();
                propertyNames.Add("Argument");
                propertyNames.Add("FolderCount");

                propertyNames.AddRange(this.PropertyNames);
                return propertyNames.ToArray();
            }
        }

        public TrainTestResult InsertHeader(string header)
        {
            TrainTestResult output = new TrainTestResult(this.PropertyNames, this.Properties);

            for (int i = 0; i < output.PropertyNames.Count; i++)
                output.PropertyNames[i] = header + output.PropertyNames[i];

            return output;
        }
    }

    public enum MLFileTypes
    {
        Feature,
        Model,
        Output,
        Perf,
        EvalScore,
        TTest,
        WinLoss
    }

    public enum TrainDevTestTypes
    { 
        Train,
        Development,
        Test
    }

    public class SvmLightInput
    {
        public string Argument { get; set; }

        public string TrainFeatureFile { get; set; }
        public string DevFeatureFile { get; set; }
        public string[] TestFeatureFiles { get; set; }

        public string AdditionalFile { get; set; }
        public string TrainRawFile { get; set; }
        public string DevRawFile { get; set; }
        public string[] TestRawFiles { get; set; }

        public TrickyLib.MachineLearning.SvmLight.CustomizeCalculatePerformance PerformanceCalculator { get; set; }
        public TrainMethods TrainMethod { get; set; }
        public string ThreadGUID { get; set; }
        public int ThreadID { get; set; }
        public int FolderCount { get; set; }
        public bool IsMultiThread { get; set; }

        public List<FeatureLine> TrainFeatureLines { get; set; }
        public List<FeatureLine> DevFeatureLines { get; set; }
        public List<FeatureLine>[] TestFeatureLinesArray { get; set; } 

        public SvmLightInput()
        {
            ThreadGUID = "";
            FolderCount = 1;
            ThreadID = -1;
        }
    }
}
