using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using TrickyLib.MachineLearning;

namespace TrickyLib
{
    class Program
    {
        static void Main(string[] args)
        {
            SVMModel model = new SVMModel();

            #region Read linear model and predict to file

            string strLineModel = @"D:\WorkSpace\MSClassifier\SmoxTraining\TestData\LineModel.md";
            string strTrain = @"D:\WorkSpace\MSClassifier\SmoxTraining\TestData\MSWeb_Doc.data";
            string strResult = @"D:\WorkSpace\MSClassifier\SmoxTraining\TestData\Result.rst";

            //model.ReadModel(strLineModel);
            //model.PredictToFile(strTrain, strResult,true);

            if (args.Length != 3)
            {
                System.Console.WriteLine("Usage: Classifier [LinearModel] [TestData] [ScoreFile]");
                return;
            }

            try
            {
                model.ReadModel(args[0]);
                model.PredictToFile(args[1], args[2], true);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Input format error!");
                return;
            }

            return;

            #endregion

            #region Read SVM model and convert to line model (SVMToLineModel.exe)

            //string strSVMModel = @"D:\SigmaProject\AuthorExtraction\Exp\Model\BestClose_Namelist_U\PptModel.dat";
            //string strLineModelName = @"D:\SigmaProject\AuthorExtraction\Exp\Model\BestClose_Namelist_U\PptLineModel.dat";
            ////string strTrainData = "D:\\SigmaProject\\DateExtraction\\Test\\feature\\TrainingData\\dotcomPpt_data.txt";
            ////string strResult = "D:\\SigmaProject\\DateExtraction\\Test\\feature\\Exp\\PptLineResult.data";

            //if (args.Length != 2)
            //{
            //    System.Console.WriteLine("Usage: Classifier [SVMModel] [LinearModel]");
            //    return;
            //}

            //try
            //{
            //    model.ReadSVMModel(args[0]);
            //    model.SaveModel(args[1]);
            //    //model.PredictToFile(strTrainData, strResult);
            //}
            //catch (Exception ex)
            //{
            //    System.Console.WriteLine("Input format error!");
            //    return;
            //}

            ////strSVMModel = @"D:\SigmaProject\AuthorExtraction\Exp\Model\BestClose_Namelist_U\DocModel.dat";
            ////strLineModelName = @"D:\SigmaProject\AuthorExtraction\Exp\Model\BestClose_Namelist_U\DocLineModel.dat";
            //////strTrainData = "D:\\SigmaProject\\DateExtraction\\Test\\feature\\TrainingData\\dotcomdoc_data.txt";
            //////strResult = "D:\\SigmaProject\\DateExtraction\\Test\\feature\\Exp\\DocLineResult.data";

            ////model.ReadSVMModel(strSVMModel);
            ////model.SaveModel(strLineModelName);
            //////model.PredictToFile(strTrainData, strResult);

            //return;

            #endregion

            #region Perceptron learning
            //int nIterNum = 10;
            //double dPosMargin = 1;
            //double dNegMargin = -1;
            //double dLearnRate = 0.05;
            ////string strTrainData = "..\\TrainingData\\MsWebBojuanDoc_data.txt";
            //string strTrainData = "..\\TrainingData\\DotComDoc_data.txt";
            //string strModel = "DocModel.dat";

            //string strTestData = "..\\TrainingData\\DotComDoc_data.txt";
            //string strResult = "Docresult.data";

            //if (args[0] == "-l")
            //{
            //    strTrainData = args[1];
            //    strModel = args[2];
            //    nIterNum = Convert.ToInt32(args[3]);
            //    dPosMargin = Convert.ToDouble(args[4]);
            //    dNegMargin = Convert.ToDouble(args[5]);
            //    dLearnRate = Convert.ToDouble(args[6]);

            //    model.PUMTrain(strTrainData, nIterNum, dPosMargin, dNegMargin, dLearnRate);

            //    model.SaveModel(strModel);

            //}
            //else if (args[0] == "-c")
            //{
            //    strTestData = args[1];
            //    strModel = args[2];
            //    strResult = args[3];
            //    model.ReadModel(strModel);
            //    model.PredictToFile(strTestData, strResult);
            //}
            //else
            //{
            //    System.Console.WriteLine("Usage:");
            //    System.Console.WriteLine("[Learn]    -l TrainData Model IterationNum PositiveMargin NegativeMargin LearningRate");
            //    System.Console.WriteLine("[Classify] -c TestData  Model Result");
            //}
            #endregion

            ////DOC
            //string strDefaultDir = "D:\\SigmaProject\\DateExtraction\\Test\\feature\\Exp\\";
            //Process pro = new Process();            
            //pro.StartInfo.WorkingDirectory = @strDefaultDir;
            //pro.StartInfo.FileName = @strDefaultDir + "FeaExtract.exe";
            //pro.StartInfo.Arguments = @"-sta ..\ConfigFile\DotComDoc.txt";//-c  0.125 
            //pro.Start();
            //pro.WaitForExit();

        }
    }
}
