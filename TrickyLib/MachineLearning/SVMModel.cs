using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Linq;
using TrickyLib.Extension;

namespace TrickyLib.MachineLearning
{
    public class FeaNode
    {
        public int m_nFeaId;
        public double m_dFeaValue;
    }

    public class SVMModel
    {
        public double m_dThreshold = 0;
        public Hashtable m_htFeaWeightTable = new Hashtable();
        public Dictionary<int, double> WeightDic;

        char[] m_chvSeparator = { ' ', '#' };
        char[] m_chvSepSem = { ':' };
        char[] m_chvSepTab = { '\t' };

        public SVMModel(string modelFile) { ReadSVMModel(modelFile); }

        public SVMModel() { }

        public List<FeatureName> ReadFeatureNames(string featureFile)
        {
            try
            {
                StreamReader sr = new StreamReader(featureFile);
                List<FeatureName> output = new List<FeatureName>();
                string strLine = null;

                while ((strLine = sr.ReadLine()) != null && strLine.StartsWith("#"))
                {
                    string[] lineArray = strLine.TrimStart('#').Trim().Split(',');
                    foreach (var item in lineArray)
                    {
                        string[] ndNamePair = item.Trim().Split(' ');

                        string[] indices = ndNamePair[0].Trim().Split('-');
                        string[] nameTypePair = ndNamePair[1].Trim().Split("()".ToCharArray());

                        FeatureName fn = new FeatureName() { Name = nameTypePair[0], Type = nameTypePair[1] };
                        if (indices.Length == 1)
                            fn.Start = fn.End = int.Parse(indices[0]);
                        else
                        {
                            fn.Start = int.Parse(indices[0]);
                            fn.End = int.Parse(indices[1]);
                        }

                        output.Add(fn);
                    }
                }

                sr.Close();
                return output;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Dictionary<int, double> ReadSVMModel(string strSVMModelFile)
        {
            Dictionary<int, double> output = new Dictionary<int, double>();

            try
            {
                using (StreamReader sr = new StreamReader(strSVMModelFile))
                {
                    int i = 0, j = 0;
                    string strLine;
                    string[] strNodes;
                    m_htFeaWeightTable.Clear();

                    while ((strLine = sr.ReadLine()) != null)
                    {
                        if (strLine.Trim().Length < 1)
                            continue;
                        else
                            i++;

                        //Get the threshold
                        if (i == 11)
                        {
                            strNodes = strLine.Split(m_chvSeparator);
                            m_dThreshold = -Convert.ToDouble(strNodes[0]);
                        }

                        //get featureNames the first 11 lines
                        if (i < 12)
                        {
                            continue;
                        }
                        int Pos = strLine.IndexOf("#");
                        if (Pos > 0)
                        {
                            strLine = strLine.Substring(0, Pos);
                        }
                        strNodes = strLine.Split(m_chvSeparator, StringSplitOptions.RemoveEmptyEntries);
                        double dWeight = Convert.ToDouble(strNodes[0]);
                        for (j = 1; j < strNodes.Length; j++)
                        {
                            string[] strFeaPair = strNodes[j].Split(m_chvSepSem);
                            int nFeaId = Convert.ToInt32(strFeaPair[0]);
                            double dValue = Convert.ToDouble(strFeaPair[1]) * dWeight;

                            if (!output.ContainsKey(nFeaId))
                                output.Add(nFeaId, dValue);
                            else
                                output[nFeaId] += dValue;

                            if (m_htFeaWeightTable[nFeaId] == null)
                                m_htFeaWeightTable[nFeaId] = Convert.ToString(dValue);
                            else
                                m_htFeaWeightTable[nFeaId] = Convert.ToString(dValue + Convert.ToDouble(m_htFeaWeightTable[nFeaId]));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("SVM model read error!\n" + ex.Message);
                return null;
            }
            WeightDic = output;
            return output;
        }

        public Dictionary<int, double> ReadSVMModel(Stream strSVMModelStream)
        {
            Dictionary<int, double> output = new Dictionary<int, double>();

            try
            {
                using (StreamReader sr = new StreamReader(strSVMModelStream))
                {
                    int i = 0, j = 0;
                    string strLine;
                    string[] strNodes;
                    m_htFeaWeightTable.Clear();

                    while ((strLine = sr.ReadLine()) != null)
                    {
                        if (strLine.Trim().Length < 1)
                            continue;
                        else
                            i++;

                        //Get the threshold
                        if (i == 11)
                        {
                            strNodes = strLine.Split(m_chvSeparator);
                            m_dThreshold = -Convert.ToDouble(strNodes[0]);
                        }

                        //get featureNames the first 11 lines
                        if (i < 12)
                        {
                            continue;
                        }
                        int Pos = strLine.IndexOf("#");
                        if (Pos > 0)
                        {
                            strLine = strLine.Substring(0, Pos);
                        }
                        strNodes = strLine.Split(m_chvSeparator, StringSplitOptions.RemoveEmptyEntries);
                        double dWeight = Convert.ToDouble(strNodes[0]);
                        for (j = 1; j < strNodes.Length; j++)
                        {
                            string[] strFeaPair = strNodes[j].Split(m_chvSepSem);
                            int nFeaId = Convert.ToInt32(strFeaPair[0]);
                            double dValue = Convert.ToDouble(strFeaPair[1]) * dWeight;

                            if (!output.ContainsKey(nFeaId))
                                output.Add(nFeaId, dValue);
                            else
                                output[nFeaId] += dValue;

                            if (m_htFeaWeightTable[nFeaId] == null)
                                m_htFeaWeightTable[nFeaId] = Convert.ToString(dValue);
                            else
                                m_htFeaWeightTable[nFeaId] = Convert.ToString(dValue + Convert.ToDouble(m_htFeaWeightTable[nFeaId]));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("SVM model read error!\n" + ex.Message);
                return null;
            }
            WeightDic = output;
            return output;
        }

        public bool SaveModel( string strFileName)
        {
            int j = 0;
            ArrayList m_alSortedFeaId = new ArrayList();
            ArrayList m_alSortedFeaWeight = new ArrayList();
            
            //foreach (DictionaryEntry de in m_htFeaWeightTable)
            //{
            //    j = 0;
            //    while (j < m_alSortedFeaId.Count)
            //    {
            //        if (Convert.ToInt32(de.Key) < Convert.ToInt32(m_alSortedFeaId[j])) break;
            //        j++;
            //    }
            //    m_alSortedFeaId.Insert(j, Convert.ToInt32(de.Key));
            //    m_alSortedFeaWeight.Insert(j, Convert.ToDouble(de.Value));
            //}

            int max = 1;
            foreach (object key in m_htFeaWeightTable.Keys)
            { 
                int feaId = Convert.ToInt32(key);
                if (feaId > max)
                    max = feaId;
            }

            for (int i = 1; i <= max; i++)
            {
                m_alSortedFeaId.Add(i);
                if (m_htFeaWeightTable.Contains(i))
                { 
                    m_alSortedFeaWeight.Add(Convert.ToDouble(m_htFeaWeightTable[i]));
                }
                else
                {
                    m_alSortedFeaWeight.Add(0.0);
                }
            }

            using(StreamWriter sw = new StreamWriter(strFileName))
            {
                sw.WriteLine( m_dThreshold  );

                for (int i = 0; i < m_alSortedFeaId.Count; i++)
                {
                    //if (Math.Abs(Convert.ToDouble(m_alSortedFeaWeight[i])) < 1e-16)
                    //    continue;
                    sw.WriteLine("{0}\t{1}", m_alSortedFeaId[i], m_alSortedFeaWeight[i]);
                }
            }

            return true;
        }

        public bool ReadModel(string strFileName)
        {
            using (StreamReader sr = new StreamReader(strFileName))
            {
                string strLine;
                string[] strNodes;

                m_dThreshold = 0;
                m_htFeaWeightTable.Clear();

                if ((strLine = sr.ReadLine()) != null)
                {
                    m_dThreshold = Convert.ToDouble(strLine);
                }
                else
                {
                    return false;
                }                

                while ((strLine = sr.ReadLine()) != null)
                {
                    if (strLine.Trim().Length < 1)
                        continue;
                    strNodes = strLine.Split(m_chvSepTab);
                    m_htFeaWeightTable[Convert.ToInt32(strNodes[0])] = Convert.ToDouble(strNodes[1]);
                }

            }
            return true;
        }

        public bool PUMTrain(string strTrainData, int nIterNum, double dPosMargin, double dNegMargin, double dLearnRate)
        {
            ArrayList alTrainDataList = new ArrayList();
            ArrayList alOriLabelList = new ArrayList();
            ArrayList alPreLabelList = new ArrayList();

            ReadTrainData(strTrainData, ref alTrainDataList, ref alOriLabelList);

            BatchLearning(alTrainDataList, alOriLabelList, nIterNum, dPosMargin, dNegMargin, dLearnRate);

            Predict(alTrainDataList, ref alPreLabelList);

            //LineEvaluation(alOriLabelList, alPreLabelList);

            return true;
        }

        private bool ReadTrainData(string strTrainData, ref ArrayList alTrainDataList, ref ArrayList alOriLabelList)
        {
            using (StreamReader sr = new StreamReader(strTrainData))
            {
                int j = 0;
                string strLine;
                string[] strNodes;

                m_dThreshold = 0;
                alOriLabelList.Clear();
                alTrainDataList.Clear();
                m_htFeaWeightTable.Clear();                              

                while ((strLine = sr.ReadLine()) != null)
                {
                    strNodes = strLine.Split(m_chvSeparator, StringSplitOptions.RemoveEmptyEntries);

                    string strLabel = strNodes[0];

                    //is label or not
                    if (strLabel == "+1" || strLabel == "-1")
                    {
                        alOriLabelList.Add(strLabel);
                        j = 1;
                    }
                    else
                    {
                        System.Console.WriteLine("Training data error!");
                        return false;
                    }

                    ArrayList alInstant = new ArrayList();                   
                    for (; j < strNodes.Length; j++)
                    {
                        string[] strFeaPair = strNodes[j].Split(m_chvSepSem);
                        FeaNode fNode = new FeaNode();
                        fNode.m_nFeaId = Convert.ToInt32(strFeaPair[0]);;
                        fNode.m_dFeaValue = Convert.ToDouble(strFeaPair[1]);
                        m_htFeaWeightTable[fNode.m_nFeaId] = 0;
                        alInstant.Add(fNode);
                    }

                    alTrainDataList.Add(alInstant);
                }
            }

            return true;
        }

        private bool BatchLearning(ArrayList alTrainDataList, ArrayList alOriLabelList, int nIterNum, double dPosMargin, double dNegMargin, double dLearnRate)
        {
            int i = 0, j = 0, k = 0;
            int nFeaId = 0;
            double dValue = 0;
            ArrayList alInstant = new ArrayList();

            m_dThreshold = 0;

            for (k = 0; k < nIterNum; k++)
            {
                if (k % 10 == 0)
                {
                    System.Console.Write("\r{0}",k);
                }

                for (i = 0; i < alTrainDataList.Count; i++)
                {
                    double dResult = m_dThreshold;

                    alInstant = alTrainDataList[i] as ArrayList;

                    //Get output value of instance
                    for (j = 0; j < alInstant.Count; j++)
                    {
                        nFeaId = (alInstant[j] as FeaNode).m_nFeaId;

                        #region

                        //dValue = (alInstant[j] as FeaNode).m_dFeaValue;

                        //if (m_htFeaWeightTable[nFeaId] != null)
                        //{
                        //    dValue *= Convert.ToDouble(m_htFeaWeightTable[nFeaId]);
                        //    dResult += dValue;
                        //}
                        //else
                        //{
                        //    m_htFeaWeightTable[nFeaId] = 0;
                        //}

                        #endregion

                        dResult += (alInstant[j] as FeaNode).m_dFeaValue * Convert.ToDouble(m_htFeaWeightTable[nFeaId]);
                    }

                    //Get yi-f(xi)
                    int nLabelValue = 0;
                    if ( dResult > dPosMargin) 
                    {
                        nLabelValue = 1;
                    }
                    else if (dResult > 0)
                    {
                        nLabelValue = -1;
                    }
                    else if (dResult > dNegMargin)
                    {
                        nLabelValue = 1;
                    }
                    else
                    {
                        nLabelValue = -1;
                    }
                    nLabelValue = Convert.ToInt32(alOriLabelList[i]) - nLabelValue;

                    //update model
                    if (nLabelValue != 0)
                    {
                        double dDelta = nLabelValue * dLearnRate;
                        //delta
                        m_dThreshold += dDelta;
                        //weight vector
                        for (j = 0; j < alInstant.Count; j++)
                        {
                            #region
                            //nFeaId = (alInstant[j] as FeaNode).m_nFeaId;
                            //dValue = (alInstant[j] as FeaNode).m_dFeaValue * dDelta;
                            //m_htFeaWeightTable[nFeaId] = Convert.ToDouble(m_htFeaWeightTable[nFeaId]) + dValue;
                            #endregion

                            nFeaId = (alInstant[j] as FeaNode).m_nFeaId;
                            m_htFeaWeightTable[nFeaId] = Convert.ToDouble(m_htFeaWeightTable[nFeaId]) + (alInstant[j] as FeaNode).m_dFeaValue * dDelta;

                        }
                    }
                }
            }
            System.Console.WriteLine("\r{0}", k);

            return true;
        }

        private bool Predict(ArrayList alTrainDataList, ref ArrayList alPrelableList)
        {
            int i = 0, j = 0;
            alPrelableList.Clear();
            for (i = 0; i < alTrainDataList.Count; i++)
            {
                double dResult = m_dThreshold;

                ArrayList alInstant = alTrainDataList[i] as ArrayList;

                //Get output value of instance
                for (j = 0; j < alInstant.Count; j++)
                {
                    int nFeaId = (alInstant[j] as FeaNode).m_nFeaId;
                    double dValue = (alInstant[j] as FeaNode).m_dFeaValue;

                    if (m_htFeaWeightTable[nFeaId] != null)
                    {
                        dValue *= Convert.ToDouble(m_htFeaWeightTable[nFeaId]);
                        dResult += dValue;
                    }
                    else
                    {
                        m_htFeaWeightTable[nFeaId] = 0;
                    }
                }

                //
                if (dResult > 0)
                    alPrelableList.Add("+1");
                else
                    alPrelableList.Add("-1");                    
                
            }
            return true;
        }

        public double GetSvmScore(string featureVector)
        {
            int commentIndex = featureVector.IndexOf('#');
            string svmString = commentIndex >= 0 && commentIndex < featureVector.Length ? featureVector.Substring(0, commentIndex) : featureVector;

            double svmValue = m_dThreshold;
            foreach (var item in svmString.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                try
                {
                    string[] kv = item.Split(':');
                    if (kv.Length > 1 && kv[0] != "qid")
                    {
                        int key = int.Parse(kv[0]);

                        if (WeightDic.ContainsKey(key))
                        {
                            double value = double.Parse(kv[1]);
                            svmValue += WeightDic[key] * value;
                        }
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            return svmValue;
        }

        public double[] GetSvmScores(string[] featureVectors)
        {
            double[] svmScores = new double[featureVectors.Length];
            for (int i = 0; i < featureVectors.Length; i++)
                svmScores[i] = GetSvmScore(featureVectors[i]);

            return svmScores;
        }

        public double GetRegressionLoss(string featureVector, int norm)
        {
            double labelScore = double.Parse(featureVector.Substring(0, featureVector.IndexOf(' ')));
            double svmScore = GetSvmScore(featureVector);

            return Math.Pow(Math.Abs(labelScore - svmScore), norm);
        }

        public double GetRegressionLoss(string featureVector)
        {
            return GetRegressionLoss(featureVector, 1);
        }

        public double GetOverallRegressionLoss(string[] featureVectors, int norm)
        {
            double overallLoss = 0;
            foreach (var fv in featureVectors)
                overallLoss += GetRegressionLoss(fv, norm);

            return overallLoss;
        }

        public double GetOverallRegressionLoss(string[] featureVectors)
        {
            return GetOverallRegressionLoss(featureVectors, 1);
        }

        public double GetOverallRegressionLoss(string featureFile, int norm)
        {
            double overallLoss = 0;
            using (StreamReader sr = new StreamReader(featureFile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    if (line != string.Empty && !line.StartsWith("#"))
                        overallLoss += GetRegressionLoss(line, norm);
                }
            }

            return overallLoss;
        }

        public double GetOverallRegressionLoss(string featureFile)
        {
            return GetOverallRegressionLoss(featureFile, 1);
        }

        private bool LineEvaluation(ArrayList alOriLabelList, ArrayList alPreLabelList)
        {
            if (alOriLabelList.Count != alPreLabelList.Count) return false;

            int nFindNum = 0, nTotalNum = 0, nCorrectNum = 0;

            string strOriLabel = "", strPreLabel = "";
            for (int i = 0; i < alOriLabelList.Count; i++)
            {
                strOriLabel = alOriLabelList[i] as string;
                if (strOriLabel == "+1")
                    nTotalNum++;

                strPreLabel = alPreLabelList[i] as string;
                if (strPreLabel == "+1")
                    nFindNum++;

                if (strOriLabel == "+1" && strOriLabel == strPreLabel)
                    nCorrectNum++;
            }

            double dPrecision = 0, dRecall = 0, dFMeasure = 0;

            if (nFindNum > 0)
                dPrecision = nCorrectNum / Convert.ToDouble(nFindNum);

            if (nTotalNum > 0)
                dRecall = nCorrectNum / Convert.ToDouble(nTotalNum);

            if (nCorrectNum > 0)
                dFMeasure = 2 * dPrecision * dRecall / (dPrecision + dRecall);

            System.Console.WriteLine("Line based evaluation result:");
            System.Console.WriteLine("P: {0} / {1} =\t{2:F4}", nCorrectNum, nFindNum, dPrecision);
            System.Console.WriteLine("R: {0} / {1} =\t{2:F4}", nCorrectNum, nTotalNum, dRecall);
            System.Console.WriteLine("F1:   \t\t{0:F4}", dFMeasure);

            return true;
        }

        public bool PredictToFile(string strTrainData, string strResult)
        {
            ArrayList alResultList = new ArrayList();
            ArrayList alOriLabelList = new ArrayList();
            ArrayList alPreLabelList = new ArrayList();

            using( StreamReader sr = new StreamReader(strTrainData) )
            {
                string strLine;
                string[] strNodes;
                int j = 0;               

                while ((strLine = sr.ReadLine()) != null)
                {
                    strNodes = strLine.Split(m_chvSeparator, StringSplitOptions.RemoveEmptyEntries);
                    
                    string strLabel = strNodes[0];

                    //is label or not
                    if (strLabel == "+1" || strLabel == "-1")
                    {
                        j = 1;
                        alOriLabelList.Add(strLabel);
                    }
                    else
                        j = 0;

                    double dResult = m_dThreshold;

                    for (; j < strNodes.Length; j++)
                    {
                        string[] strFeaPair = strNodes[j].Split(m_chvSepSem);
                        int nFeaId = Convert.ToInt32( strFeaPair[0]);
                        double dValue = Convert.ToDouble(strFeaPair[1]);
                        if (m_htFeaWeightTable[nFeaId] != null)
                        {
                            dValue *= Convert.ToDouble(m_htFeaWeightTable[nFeaId]);
                            dResult += dValue;
                        }
                    }

                    if (dResult > 0)
                        strLabel = "+1";
                    else
                        strLabel = "-1";
                    alPreLabelList.Add(strLabel);

                    alResultList.Add(dResult);
                }

            }
            if ((alOriLabelList.Count > 0) && (alPreLabelList.Count == alOriLabelList.Count))
            {
                LineEvaluation(alOriLabelList, alPreLabelList);
            }

            using (StreamWriter sw = new StreamWriter(strResult))
            {
                for (int i = 0; i < alResultList.Count; i++ )
                {
                    sw.WriteLine( alResultList[i] );
                }
            }

            return true;
        }

        public bool PredictToFile(string strTrainData, string strResult, bool bIsTrue)
        {
            ArrayList alResultList = new ArrayList();
            ArrayList alOriLabelList = new ArrayList();
            ArrayList alPreLabelList = new ArrayList();

            try
            {
                using (StreamReader sr = new StreamReader(strTrainData))
                {
                    string strLine;
                    string[] strNodes;
                    int j = 0;

                    while ((strLine = sr.ReadLine()) != null)
                    {
                        if (alResultList.Count == 278511)
                        {
                            int test = 0;
                        }

                        //Find label in the training data
                        string strLabel = "";
                        int Pos = strLine.IndexOf(' ');
                        if (Pos < 1)
                            return false;
                        else
                            strLabel = strLine.Substring(0, Pos).Trim();
                        if (strLabel == "+1" || strLabel == "-1")
                        {
                            alOriLabelList.Add(strLabel);
                            strLine = strLine.Substring(Pos + 1).Trim();
                        }


                        //Find comments
                        Pos = strLine.IndexOf('#');
                        if (Pos > 0)
                            strLine = strLine.Substring(0, Pos - 1).Trim();

                        //Find feature vector
                        double dResult = m_dThreshold;
                        strNodes = strLine.Split(m_chvSeparator, StringSplitOptions.RemoveEmptyEntries);
                        for (j = 0; j < strNodes.Length; j++)
                        {
                            string[] strFeaPair = strNodes[j].Split(m_chvSepSem);
                            int nFeaId = Convert.ToInt32(strFeaPair[0]);
                            double dValue = Convert.ToDouble(strFeaPair[1]);
                            if (m_htFeaWeightTable[nFeaId] != null)
                            {
                                dValue *= Convert.ToDouble(m_htFeaWeightTable[nFeaId]);
                                dResult += dValue;
                            }
                        }

                        if (dResult > 0)
                            strLabel = "+1";
                        else
                            strLabel = "-1";
                        alPreLabelList.Add(strLabel);

                        alResultList.Add(dResult);
                    }

                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error!"+ex.Message);
            }

            if ((alOriLabelList.Count > 0) && (alPreLabelList.Count == alOriLabelList.Count))
            {
                LineEvaluation(alOriLabelList, alPreLabelList);
            }

            using (StreamWriter sw = new StreamWriter(strResult))
            {
                for (int i = 0; i < alResultList.Count; i++)
                {
                    sw.WriteLine(alResultList[i]);
                }
            }

            return true;
        }

        public void Classify(string featureFile, string modelFile, string outputFile)
        {
            var weightDic = ReadSVMModel(modelFile);

            using (StreamReader sr = new StreamReader(featureFile))
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    if (!line.StartsWith("#"))
                    {
                        int commentIndex = line.IndexOf('#');
                        string svmString = commentIndex >= 0 && commentIndex < line.Length ? line.Substring(0, commentIndex) : line;

                        double svmValue = m_dThreshold;
                        foreach (var item in svmString.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                        {
                            try
                            {
                                string[] kv = item.Split(':');
                                if (kv.Length > 1 && kv[0] != "qid")
                                {
                                    int key = int.Parse(kv[0]);

                                    if (weightDic.ContainsKey(key))
                                    {
                                        double value = double.Parse(kv[1]);
                                        svmValue += weightDic[key] * value;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                        }

                        sw.WriteLine(svmValue);
                    }
                }
            }
        }

        public void Classify(List<FeatureLine> instances, string modelFile, string outputFile)
        {
            var weightDic = ReadSVMModel(modelFile);

            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                foreach (var instance in instances)
                {
                    double svmValue = m_dThreshold;
                    foreach (var kv in instance.ItemsDic)
                    {
                        if (weightDic.ContainsKey(kv.Key))
                            svmValue += weightDic[kv.Key] * kv.Value;
                    }

                    sw.WriteLine(svmValue);
                }
            }
        }

        //public FeatureVector GetMaxFeatureVector(FeatureVector[] featureVectors, HashSet<string> selectedFeatures)
        //{
        //    if (this.m_htFeaWeightTable == null)
        //        throw new Exception("The model is null");
        //    var scores = GetSvmScores(featureVectors.Select(fv => fv.GetFeatureVector(selectedFeatures)).ToArray());
        //    double maxScore = scores.Max();
        //    int maxIndex = scores.FindIndex(s => s == maxScore);
        //    return featureVectors[maxIndex];
        //}

        //public FeatureVector GetMaxFeatureVector(FeatureVector[] featureVectors, HashSet<int> selectedFeatureIndice)
        //{
        //    if (this.m_htFeaWeightTable == null)
        //        throw new Exception("The model is null");
        //    var scores = GetSvmScores(featureVectors.Select(fv => fv.GetFeatureVector(selectedFeatureIndice)).ToArray());
        //    double maxScore = scores.Max();
        //    int maxIndex = scores.FindIndex(s => s == maxScore);
        //    return featureVectors[maxIndex];
        //}

        public FeatureVector GetMaxFeatureVector(FeatureVector[] featureVectors)
        {
            if (this.m_htFeaWeightTable == null)
                throw new Exception("The model is null");
            var scores = GetSvmScores(featureVectors.Select(fv => fv.FullVector).ToArray());
            double maxScore = scores.Max();
            int maxIndex = scores.FindIndex(s => s == maxScore);
            return featureVectors[maxIndex];
        }

    }

    public class FeatureName
    {
        public string Name { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string Type { get; set; }
    }
}
