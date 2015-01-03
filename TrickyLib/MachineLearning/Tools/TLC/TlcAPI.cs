using Microsoft.TMSN.TMSNlearn;
using Microsoft.TMSN.TMSNlearn.FastRank;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TrickyLib.Service;
using TrickyLib.Extension;
using TrickyLib.IO;
using System.Diagnostics;

namespace TrickyLib.MachineLearning.Tools.TLC
{
    [Serializable]
    public class TlcAPI : BaseAPI
    {
        public static string ExePath { get; set; }

        static TlcAPI()
        {
            ExePath = @"Tools\TLC\TL.exe";
        }

        protected override void Learn(MachineLearningTask task, string trainFile, string modelFile, BaseParameterCombination paraCombination, out Process process)
        {
            #region tool exe to call training
            string runParameter = GetRunParameterString_Learn(task, trainFile, modelFile, paraCombination);
            SoftwareOperator.Execute(ExePath, runParameter, -1, System.Diagnostics.ProcessWindowStyle.Hidden, true, out process);
            #endregion

            #region C# API to call training
            //var tlcParaCombination = paraCombination as TlcParameterCombination;

            //// assume data is in memory in matrix/vector form.  Sparse format also supported.
            //double[][] data;
            //double[] labels;
            //// below just reads some actual data into these arrays 
            ////PredictionUtil.ReadInstancesAsArrays(trainFile, out data, out labels);
            //ReadData(trainFile, out data, out labels);

            //// create an Instances dataset.  
            //// For sparse data, create empty dataset, add data one-by-one 
            //// using Instance constructor that takes indices and values.
            //ListInstances instances = new ListInstances(data, labels);

            //// Create a predictor and specify some non-default settings
            //IPredictor<double> predictor = PredictorUtils.CreatePredictor("FastRank") as IPredictor<double>;
            
            //predictor.Initialize(new string[] { 
            //    "nl:" + tlcParaCombination.NumberLeaves,
            //    "lr:" + tlcParaCombination.LearningRate,
            //    "iter:" + tlcParaCombination.NumTrees,
            //    "mil:" + tlcParaCombination.MinInstancesPerLeaf }, instances);

            //// Train a predictor 
            //predictor.Train(instances);

            /////*********  Several ways to save models.  Only binary can be used to-reload in TLC. *******//

            //// Save the model in internal binary format that can be used for loading it.
            //PredictorUtils.Save<double>(predictor, modelFile);

            //// Save the model as a plain-text description
            ////PredictorUtils.SaveText<double>(predictor, modelFile);

            ////// Save the model in Bing's INI format
            ////PredictorUtils.SaveIni<double>(predictor, modelFile);
            #endregion
        }

        protected override void Classify(MachineLearningTask task, string testFile, string modelFile, string outputFile, out Process process)
        {
            process = null;
            int dimentionCount = FeatureFileProcessor.GetDimentionCount(testFile);
            FastRankPredictor predictor = new FastRankPredictor(modelFile);

            using (StreamReader sr = new StreamReader(testFile))
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                while (!sr.EndOfStream)
                {
                    double[] feature = new double[dimentionCount];
                    string[] items = sr.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if ((items.Length - 3) != dimentionCount)
                        throw new Exception("Error in data dimension in TL FastRank");

                    for (int i = 3; i < items.Length; i++)
                        feature[i - 3] = double.Parse(items[i]);

                    sw.WriteLine(predictor.Predict(feature));
                }
            }
        }

        protected override string GetRunParameterString_Learn(MachineLearningTask task, string trainFile, string modelFile, BaseParameterCombination paraCombination)
        {
            string trainer;
            string groupString;
            if (task == MachineLearningTask.Ranking)
            {
                trainer = "FRRANK";
                groupString = ";groupKey:n0";
            }
            else if (task == MachineLearningTask.Regression)
            {
                trainer = "FRR";
                groupString = "";
            }
            else
            {
                throw new NotImplementedException();
            }

            return string.Format(@"/c Train {0} /cl {1} /cls {2} /instset:name:1-2{3} /m {4}", trainFile, trainer, paraCombination.GetParameterString(), groupString, modelFile); ;
        }

        protected override List<List<string>> ReadFeatureFileToSamples(string featureFile)
        {
            return FeatureFileProcessor.ReadFeatureFile(featureFile, FeatureFileFormat.BingFastRank);
        }

        private void ReadData(string featureFile, out double[][] data, out double[] labels)
        {
            List<double[]> instanceList = new List<double[]>();
            List<double> labelList = new List<double>();

            using (StreamReader sr = new StreamReader(featureFile))
            {
                while (!sr.EndOfStream)
                {
                    var items = sr.ReadLine().Split('\t');
                    double label = double.Parse(items[0]);
                    double[] instance = items.GetRange(3, items.Length - 3).Select(i => double.Parse(i)).ToArray();

                    instanceList.Add(instance);
                    labelList.Add(label);
                }
            }

            data = instanceList.ToArray();
            labels = labelList.ToArray();
        }
    }

    public class FastRankPredictor
    {
        //IProbabilityPredictor<double> _predictor;
        FastRankTrainingWrapper _predictor;
            
        public FastRankPredictor(string modelFile)
        {
            //_predictor = PredictorUtils.Load(modelFile) as IProbabilityPredictor<double>;
            LoadModel(modelFile);
        }

        //public FastRankPredictor()
        //{ 
        
        //}

        public void LoadModel(string modelFile)
        {
            if (!File.Exists(modelFile))
                throw new FileNotFoundException(modelFile);

            using (Stream stream = new FileStream(modelFile, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                _predictor = (FastRankTrainingWrapper)formatter.Deserialize(stream);
            }
        }

        public double Predict(double[] features)
        {
            Instance inst = new Instance(features);
            //double output = _predictor.PredictWithProbability(inst, out probability);
            double output = _predictor.Predict(inst);
            return output;
        }

        public double Predict(string featureLine, int dimentionCount)
        {
            double[] features = new double[dimentionCount];
            FeatureLine feature = new FeatureLine(featureLine, true, false, false);
            foreach (var kv in feature.ItemsDic)
                    features[kv.Key - 1] = kv.Value;

            Instance inst = new Instance(features);
            //double output = _predictor.PredictWithProbability(inst, out probability);
            double output = _predictor.Predict(inst);
            return output;
        }
    }
}
