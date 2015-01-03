using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.IO;
using TrickyLib.Extension;
using System.IO;
using System.Text.RegularExpressions;
using TrickyLib.Struct;
using TrickyLib.Relevance.Metric;

namespace TrickyLib.MachineLearning
{
    public class FeatureFileProcessor
    {
        public static Regex QidReg = new Regex(@"(?<=qid:)\d+(?= )", RegexOptions.IgnoreCase);
        public const string LabelRegStr = @"^[\+-]?\d+(\s|$)";

        public static void SampleTrainAndTest_Accurate(string featureFile, double trainRatioOrCount, double testRatioOrCount)
        {
            if (trainRatioOrCount <= 0)
                throw new ArgumentException("ratioOrCount should larger than 0");

            bool hasQid = false;
            Regex qidReg = new Regex(@"(?<=qid:)\d+(?= )", RegexOptions.IgnoreCase);

            using (StreamReader sr = new StreamReader(featureFile))
            {
                string line = sr.ReadLine().Trim();
                while (line.StartsWith("#"))
                    line = sr.ReadLine();

                hasQid = qidReg.IsMatch(line);
            }

            List<string[]> itemsList = new List<string[]>();
            using (StreamReader sr = new StreamReader(featureFile))
            {
                if (hasQid)
                {
                    int lastQid = -1;
                    List<string> items = null;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine().Trim();
                        if (!line.StartsWith("#"))
                        {
                            int qid = int.Parse(qidReg.Match(line).Value);
                            if (qid != lastQid)
                            {
                                lastQid = qid;
                                if (items != null && items.Count > 0)
                                    itemsList.Add(items.ToArray());

                                items = new List<string>();
                            }

                            items.Add(line);
                        }
                    }

                    if (items != null && items.Count > 0)
                        itemsList.Add(items.ToArray());
                }
                else
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine().Trim();
                        if (!line.StartsWith("#"))
                        {
                            List<string> items = new List<string>();
                            items.Add(line);
                            itemsList.Add(items.ToArray());
                        }
                    }
                }
            }

            int trainTopN = trainRatioOrCount < 1 ? Convert.ToInt32(itemsList.Count * trainRatioOrCount) : Convert.ToInt32(Math.Min(trainRatioOrCount, itemsList.Count));
            int testTopN = testRatioOrCount < 1 ? Convert.ToInt32(itemsList.Count * testRatioOrCount) : Convert.ToInt32(Math.Min(testRatioOrCount, itemsList.Count));
            testTopN = Convert.ToInt32(Math.Min(testTopN, itemsList.Count - trainTopN));

            string trainFile = FilePath.ChangeExtension(featureFile, "Train.feature");
            string testFile = FilePath.ChangeExtension(featureFile, "Test.feature");

            var randomList = itemsList.ToRandomList();
            FileWriter.PrintCollection(trainFile, randomList.GetRange(0, trainTopN).ToTension());
            FileWriter.PrintCollection(testFile, randomList.GetRange(trainTopN, testTopN).ToTension());
        }

        public static Dictionary<string, int> SampleTrainAndTest_Rough(string featureFile, double trainRatio, double testRatio)
        {
            if (trainRatio <= 0 || testRatio <= 0 || trainRatio + testRatio > 1)
                throw new ArgumentException("trainRatio or testRatio out of range");

            double step1 = trainRatio;
            double step2 = trainRatio + testRatio;

            string trainFile = FilePath.ChangeExtension(featureFile, string.Format("SampleTrain.{0:F}.Rough.feature", trainRatio));
            string testFile = FilePath.ChangeExtension(featureFile, string.Format("SampleTest.{0:F}.Rough.feature", testRatio));

            bool hasQid = false;
            Regex qidReg = new Regex(@"(?<=qid:)\d+(?= )", RegexOptions.IgnoreCase);

            using (StreamReader sr = new StreamReader(featureFile))
            {
                string line = sr.ReadLine().Trim();
                while (line.StartsWith("#"))
                    line = sr.ReadLine();

                hasQid = qidReg.IsMatch(line);
            }

            Dictionary<string, int> sampleCountDic = new Dictionary<string, int>();
            sampleCountDic.Add("TotalQidCount", 0);
            sampleCountDic.Add("TotalItemCount", 0);
            sampleCountDic.Add("TrainQidCount", 0);
            sampleCountDic.Add("TrainItemCount", 0);
            sampleCountDic.Add("TestQidCount", 0);
            sampleCountDic.Add("TestItemCount", 0);

            using (StreamWriter trainSw = new StreamWriter(trainFile))
            using (StreamWriter testSw = new StreamWriter(testFile))
            using (StreamReader sr = new StreamReader(featureFile))
            {
                Random rd = new Random(DateTime.Now.Millisecond);

                if (hasQid)
                {
                    int lastQid = -1;
                    List<string> items = null;
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine().Trim();
                        if (!line.StartsWith("#"))
                        {
                            int qid = int.Parse(qidReg.Match(line).Value);
                            if (qid != lastQid)
                            {
                                sampleCountDic["TotalQidCount"]++;
                                lastQid = qid;
                                if (items != null && items.Count > 0)
                                {
                                    double wheelResult = rd.NextDouble();
                                    if (wheelResult <= step1)
                                    {
                                        foreach (var item in items)
                                            trainSw.WriteLine(item);

                                        sampleCountDic["TrainQidCount"]++;
                                        sampleCountDic["TrainItemCount"] += items.Count;
                                    }
                                    else if (wheelResult <= step2)
                                    {
                                        foreach (var item in items)
                                            testSw.WriteLine(item);

                                        sampleCountDic["TestQidCount"]++;
                                        sampleCountDic["TestItemCount"] += items.Count;
                                    }
                                }

                                items = new List<string>();
                            }

                            items.Add(line);
                            sampleCountDic["TotalItemCount"]++;
                        }
                    }

                    if (items != null && items.Count > 0)
                    {
                        double wheelResult = rd.NextDouble();
                        if (wheelResult <= step1)
                        {
                            foreach (var item in items)
                                trainSw.WriteLine(item);

                            sampleCountDic["TrainQidCount"]++;
                            sampleCountDic["TrainItemCount"] += items.Count;
                        }
                        else if (wheelResult <= step2)
                        {
                            foreach (var item in items)
                                testSw.WriteLine(item);

                            sampleCountDic["TestQidCount"]++;
                            sampleCountDic["TestItemCount"] += items.Count;
                        }
                    }
                }
                else
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine().Trim();
                        if (!line.StartsWith("#"))
                        {
                            sampleCountDic["TotalQidCount"]++;
                            sampleCountDic["TotalItemCount"]++;

                            double wheelResult = rd.NextDouble();
                            if (wheelResult <= step1)
                            {
                                trainSw.WriteLine(line);
                                sampleCountDic["TrainQidCount"]++;
                                sampleCountDic["TrainItemCount"]++;
                            }
                            else if (wheelResult <= step2)
                            {
                                testSw.WriteLine(line);
                                sampleCountDic["TestQidCount"]++;
                                sampleCountDic["TestItemCount"]++;
                            }
                        }
                    }
                }
            }

            return sampleCountDic;
        }

        public static string JoinFeatureFiles(string mainFeatureFile, string otherFeatureFile)
        {
            ConsoleWriter.WriteCurrentMethodStarted();
            bool hasQid = false;
            Regex qidReg = new Regex(@"(?<=qid:)\d+(?= )", RegexOptions.IgnoreCase);

            using (StreamReader sr = new StreamReader(mainFeatureFile))
            {
                string line = sr.ReadLine().Trim();
                while (line.StartsWith("#"))
                    line = sr.ReadLine();

                hasQid = qidReg.IsMatch(line);
            }

            if (!hasQid)
                throw new Exception("There is no qid in the featureFile");

            HashSet<int> qids = new HashSet<int>();
            using (StreamReader sr = new StreamReader(mainFeatureFile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    if (!line.StartsWith("#"))
                    {
                        int qid = int.Parse(qidReg.Match(line).Value);
                        if (!qids.Contains(qid))
                            qids.Add(qid);
                    }
                }
            }

            using (StreamReader sr = new StreamReader(otherFeatureFile))
            {
                string line = sr.ReadLine().Trim();
                while (line.StartsWith("#"))
                    line = sr.ReadLine();

                hasQid = qidReg.IsMatch(line);
            }

            if (!hasQid)
            {
                Console.WriteLine(string.Format("[Error]No qid in {0}", otherFeatureFile));
                return string.Empty;
            }

            string sampleFile = FilePath.ChangeExtension(otherFeatureFile, "Sample.feature");
            using (StreamWriter sw = new StreamWriter(sampleFile))
            using (StreamReader sr = new StreamReader(otherFeatureFile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    if (line.StartsWith("#"))
                        sw.WriteLine(line);
                    else
                    {
                        int qid = int.Parse(qidReg.Match(line).Value);
                        if (qids.Contains(qid))
                            sw.WriteLine(line);
                    }
                }
            }

            ConsoleWriter.WriteCurrentMethodFinished();
            return sampleFile;
        }

        public static void SelectFeatures(string featureFile, string outputFile, IEnumerable<int> featureIndice)
        {
            ConsoleWriter.WriteCurrentMethodStarted();
            HashSet<int> indiceHash = new HashSet<int>();
            foreach (var index in featureIndice)
                if (!indiceHash.Contains(index))
                    indiceHash.Add(index);

            using (StreamWriter sw = new StreamWriter(outputFile))
            using (StreamReader sr = new StreamReader(featureFile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    if (line.StartsWith("#"))
                        sw.WriteLine(line);
                    else
                    {
                        int commentPos = line.IndexOf('#');
                        string svmString = commentPos >= 0 ? line.Substring(0, commentPos) : line;
                        string commentString = commentPos >= 0 ? line.Substring(commentPos + 1) : string.Empty;

                        string[] items = svmString.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string printString = string.Empty;
                        foreach (var item in items)
                        {
                            //Label
                            if (!item.Contains(":"))
                                printString += " " + item;

                            //Qid
                            else if (item.ToLowerInvariant().Contains("qid"))
                                printString += " " + item;

                            //Index
                            else
                            {
                                string[] kv = item.Split(':');
                                int index = -1;
                                if (int.TryParse(kv[0], out index) && indiceHash.Contains(index))
                                    printString += " " + item;
                            }
                        }

                        if (commentString != string.Empty)
                            printString += " # " + commentString.Trim();

                        sw.WriteLine(printString.Trim());
                    }
                }
            }
            ConsoleWriter.WriteCurrentMethodFinished();
        }

        public static string NormalizeFeatureFile(string featureFile, FeatureFileFormat format = FeatureFileFormat.SvmLight)
        {
            string normalizedFile = FilePath.AddExtention(featureFile, "norm", 1);
            int dimentionCount = -1;
            if (format == FeatureFileFormat.BingFastRank)
                dimentionCount = FeatureFileProcessor.GetDimentionCount(featureFile);

            using (StreamReader sr = new StreamReader(featureFile))
            using (StreamWriter sw = new StreamWriter(normalizedFile))
            {
                Dictionary<int, Pair<double, double>> maxMinDic = new Dictionary<int, Pair<double, double>>();
                int lastQid = int.MinValue;

                List<FeatureLine> featureLines = new List<FeatureLine>();
                int guid = 1;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.TrimStart().StartsWith("#") && format == FeatureFileFormat.SvmLight)
                    {
                        sw.WriteLine(line);
                    }
                    else
                    {
                        FeatureLine tempFeatureLine = new FeatureLine(line);
                        if (tempFeatureLine.Qid != lastQid && featureLines.Count > 0)
                        {
                            foreach (var featureLine in featureLines)
                            {
                                featureLine.Normalize(maxMinDic);
                                if (format == FeatureFileFormat.SvmLight)
                                    sw.WriteLine(featureLine.GetFeatureVectorLine());
                                else
                                    sw.WriteLine(featureLine.ToBingFormat(guid++, dimentionCount));
                            }

                            maxMinDic.Clear();
                            featureLines.Clear();
                        }

                        foreach (var kv in tempFeatureLine.ItemsDic)
                        {
                            if (maxMinDic.ContainsKey(kv.Key))
                            {
                                if (maxMinDic[kv.Key].First < kv.Value)
                                    maxMinDic[kv.Key].First = kv.Value;

                                if (maxMinDic[kv.Key].Second > kv.Value)
                                    maxMinDic[kv.Key].Second = kv.Value;
                            }
                            else
                            {
                                maxMinDic.Add(kv.Key, new Pair<double, double>(kv.Value, kv.Value));
                            }
                        }

                        lastQid = tempFeatureLine.Qid;
                        featureLines.Add(tempFeatureLine);
                    }
                }

                foreach (var featureLine in featureLines)
                {
                    featureLine.Normalize(maxMinDic);
                    if (format == FeatureFileFormat.SvmLight)
                        sw.WriteLine(featureLine.GetFeatureVectorLine());
                    else
                        sw.WriteLine(featureLine.ToBingFormat(guid++, dimentionCount));
                }
            }

            return normalizedFile;
        }

        //public static void NormalizeFeatureFiles(params string[] featureFiles)
        //{
        //    var minMaxDic = GetNormalizeMinMaxDic(featureFiles);
        //    foreach (var featureFile in featureFiles)
        //        NormalizeFeatureFile(featureFile, minMaxDic);
        //}

        public static Dictionary<int, Pair<double, double>> GetNormalizeMaxMinDic(IEnumerable<string> featureLines)
        {
            Dictionary<int, Pair<double, double>> maxMinDic = new Dictionary<int, Pair<double, double>>();
            foreach (var line in featureLines.Select(l => l.Trim()))
            {
                if (!line.StartsWith("#"))
                {
                    int commentIndex = line.IndexOf('#');
                    string svm = (commentIndex >= 0 ? line.Substring(0, commentIndex) : line).Trim();

                    foreach (var item in svm.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (item.Contains(':') && !item.StartsWith("qid", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string[] kv = item.Split(':');
                            int key = int.Parse(kv[0]);
                            double value = double.Parse(kv[1]);

                            if (!maxMinDic.ContainsKey(key))
                                maxMinDic.Add(key, new Pair<double, double>(value, value));
                            else if (value > maxMinDic[key].First)
                                maxMinDic[key].First = value;
                            else if (value < maxMinDic[key].Second)
                                maxMinDic[key].Second = value;
                        }
                    }
                }
            }
            return maxMinDic;
        }

        public static string NormalizeFeatureLine(string featureLine, Dictionary<int, Pair<double, double>> maxMinDic)
        {
            if (featureLine.TrimStart().StartsWith("#"))
                return featureLine;

            var svmAndCom = SplitSvmAndComment(featureLine);
            string normalizedSvm = string.Empty;
            foreach (var item in svmAndCom[0].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                if (item.Contains(':') && !item.StartsWith("qid", StringComparison.InvariantCultureIgnoreCase))
                {
                    string[] kv = item.Split(':');
                    int key = int.Parse(kv[0]);
                    double value = double.Parse(kv[1]);

                    double normalizedValue = Calculator.Normalize(value, maxMinDic[key].Second, maxMinDic[key].First);
                    normalizedSvm += (normalizedSvm != string.Empty ? " " : string.Empty) + key + ":" + normalizedValue.ToString("f5");
                }
                else
                {
                    normalizedSvm += (normalizedSvm != string.Empty ? " " : string.Empty) + item;
                }
            }

            normalizedSvm += svmAndCom[1] != string.Empty ? " # " + svmAndCom[1] : string.Empty;
            return normalizedSvm;
        }

        //private static string NormalizeFeatureFile(string featureFile, Dictionary<int, Pair<double, double>> minMaxDic)
        //{
        //    var featureLines = FileReader.ReadRows(featureFile);
        //    var normalizedFeatureLines = NormalizeFeatureLines(featureLines.ToArray(), minMaxDic);
        //    string outputFile = FilePath.ChangeExtension(featureFile, "Normalized.feature");
        //    FileWriter.PrintCollection(outputFile, normalizedFeatureLines);
        //    return outputFile;
        //}

        //public static IEnumerable<string> NormalizeFeatureLines(IEnumerable<string> featureLines, Dictionary<int, Pair<double, double>> minMaxDic)
        //{
        //    List<string> normalizedFeatureLines = new List<string>();
        //    foreach (var line in featureLines)
        //    {
        //        if (line.StartsWith("#"))
        //            normalizedFeatureLines.Add(line);
        //        else
        //        {
        //            var svmAndCom = SplitSvmAndComment(line);
        //            string normalizedSvm = string.Empty;
        //            foreach (var item in svmAndCom[0].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
        //            {
        //                if (item.Contains(':') && !item.StartsWith("qid", StringComparison.InvariantCultureIgnoreCase))
        //                {
        //                    string[] kv = item.Split(':');
        //                    int key = int.Parse(kv[0]);
        //                    double value = double.Parse(kv[1]);

        //                    double normalizedValue = Calculator.Normalize(value, minMaxDic[key].First, minMaxDic[key].Second);
        //                    normalizedSvm += (normalizedSvm != string.Empty ? " " : string.Empty) + key + ":" + normalizedValue.ToString("f5");
        //                }
        //                else
        //                {
        //                    if (item == "qid:403")
        //                    { 

        //                    }
        //                    normalizedSvm += (normalizedSvm != string.Empty ? " " : string.Empty) + item;
        //                }
        //            }

        //            normalizedSvm += svmAndCom[1] != string.Empty ? " # " + svmAndCom[1] : string.Empty;
        //            normalizedFeatureLines.Add(normalizedSvm);
        //        }
        //    }
        //    return normalizedFeatureLines.ToArray();
        //}

        public static string[] SplitSvmAndComment(string svmString)
        {
            int commentIndex = svmString.IndexOf('#');
            string svm = commentIndex >= 0 ? svmString.Substring(0, commentIndex) : svmString;
            string comment = commentIndex >= 0 ? svmString.Substring(commentIndex + 1) : string.Empty;

            return new string[] { svm, comment };
        }

        public static string AppendFeatureFile(params string[] featureFiles)
        {
            int[] maxIndice = new int[featureFiles.Length];
            for (int i = 0; i < featureFiles.Length; i++)
            {
                using (StreamReader sr = new StreamReader(featureFiles[i]))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine().Trim();
                        if (!line.StartsWith("#"))
                        {
                            string svmString = SplitSvmAndComment(line)[0];
                            foreach (var item in svmString.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (item.Contains(':') && !item.StartsWith("qid", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    int qid = int.Parse(item.Split(':')[0]);
                                    if (maxIndice[i] < qid)
                                        maxIndice[i] = qid;
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 1; i < maxIndice.Length; i++)
                maxIndice[i] += maxIndice[i - 1];

            for (int i = maxIndice.Length - 1; i >= 1; i--)
                maxIndice[i] = maxIndice[i - 1];
            maxIndice[0] = 0;

            StreamReader[] srs = new StreamReader[featureFiles.Length];
            for (int i = 0; i < maxIndice.Length; i++)
                srs[i] = new StreamReader(featureFiles[i]);

            string outputFile = FilePath.ChangeExtension(featureFiles[0], "Appened.feature");
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                while (!srs[0].EndOfStream)
                {
                    string[] lines = srs.Select(sr => sr.ReadLine().Trim()).ToArray();
                    if (!(lines.FindIndex(line => line.StartsWith("#")) >= 0) && lines.FindIndex(line => line.StartsWith("#")) >= 0)
                        throw new Exception("[Comment Error]Some lines are not matched in feature files");

                    if (lines[0].StartsWith("#"))
                        sw.WriteLine(lines[0]);
                    else
                    {
                        string label = string.Empty;
                        string qid = string.Empty;
                        string appendSvm = string.Empty;
                        string comment = string.Empty;
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string[] svmComment = SplitSvmAndComment(lines[i]);
                            string svmString = svmComment[0];

                            if (comment == string.Empty)
                                comment = svmComment[1];

                            foreach (var item in svmString.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (item.Contains(':') && !item.StartsWith("qid", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    string[] kv = item.Split(':');
                                    int key = int.Parse(kv[0]);
                                    double value = double.Parse(kv[1]);

                                    appendSvm += (key + maxIndice[i]).ToString() + ":" + value + " ";
                                }
                                else if (item.StartsWith("qid", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (qid == string.Empty)
                                    {
                                        qid = item;
                                        appendSvm += qid + " ";
                                    }
                                    else if (qid != item)
                                        throw new Exception("[Qid Error]Some lines are not matched in feature files");
                                }
                                else if (!item.Contains(':'))
                                {
                                    if (label == string.Empty)
                                    {
                                        label = item;
                                        appendSvm += label + " ";
                                    }
                                    else if (label != item)
                                        throw new Exception("[Label Error]Some lines are not matched in feature files");
                                }
                            }
                        }

                        appendSvm += "#" + comment;
                        sw.WriteLine(appendSvm);
                    }
                }
            }

            return outputFile;
        }

        public static void SignTestEvalScore(string evalFile1, string evalFile2)
        {
            try
            {
                string lastOneRow1 = FileReader.ReadLastRow(evalFile1, 1, ItemOperator.RemoveEmptyItem);
                string lastOneRow2 = FileReader.ReadLastRow(evalFile2, 1, ItemOperator.RemoveEmptyItem);

                int qidCount1 = int.Parse(lastOneRow1.Split(':')[0].Split(' ').Last().Substring("query".Length)) + 1;
                int qidCount2 = int.Parse(lastOneRow2.Split(':')[0].Split(' ').Last().Substring("query".Length)) + 1;

                if (qidCount1 != qidCount2)
                    throw new Exception("Different Qid Count: " + qidCount1 + " != " + qidCount2);

                string[] files = new string[] { evalFile1, evalFile2 };
                Dictionary<string, double[]>[] evalScoreDics = new Dictionary<string, double[]>[files.Length];

                for (int index = 0; index < files.Length; index++)
                {
                    evalScoreDics[index] = new Dictionary<string, double[]>();
                    using (StreamReader sr = new StreamReader(files[index]))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();

                            if (line.Contains("of query"))
                            {
                                string[] kv = line.Split(':');
                                string header = kv[0];
                                string value = kv[1].Trim();

                                string[] headerItems = header.Split(' ');
                                string[] valueItems = value.Split('\t');

                                string headerKey = headerItems[0];
                                int qid = int.Parse(headerItems.Last().Substring("query".Length));

                                for (int i = 0; i < valueItems.Length; i++)
                                {
                                    string key = headerKey + (i + 1);
                                    if (!evalScoreDics[index].ContainsKey(key))
                                        evalScoreDics[index].Add(key, new double[qidCount1]);

                                    evalScoreDics[index][key][qid] = double.Parse(valueItems[i]);
                                }
                            }
                        }
                    }
                }

                string signTestName = FilePath.ReverseMergeDistinctFilePathPart(evalFile1, evalFile2);
                string SignTestFile = FilePath.ChangeFile(evalFile1, signTestName + ".SignTest.txt");
                using (StreamWriter sw = new StreamWriter(SignTestFile))
                {
                    string header = "Tolerance";
                    string methodHeader = "Method";
                    string[] methods = signTestName.Split(new string[] { "_To_" }, StringSplitOptions.None);

                    for (int i = 1; i <= 10; i++)
                    {
                        double tolerance = Math.Pow(10, -i);
                        header += "\t" + tolerance + "\t";
                        methodHeader += "\t" + methods[0] + "\t" + methods[1];
                    }

                    sw.WriteLine(header);
                    sw.WriteLine(methodHeader);

                    foreach (var kv in evalScoreDics[0])
                    {
                        string key = kv.Key;
                        string line = key;

                        for (int i = 1; i <= 10; i++)
                        {
                            double tolerance = Math.Pow(10, -i);
                            var winloss = Calculator.SignTest(evalScoreDics[0][key], evalScoreDics[1][key], tolerance);
                            line += "\t" + winloss.First + "\t" + winloss.Second;
                        }

                        sw.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("EvalFile Error, " + e.Message);
            }
        }

        public static int GetQid(string line)
        {
            if (QidReg.IsMatch(line))
                return int.Parse(QidReg.Match(line).Value);
            else
                return -1;
        }

        public static void MagnifyExamples(string featureFile, int magnification, params int[] seletedLabels)
        {
            var lines = FileReader.ReadRows(featureFile);
            using (StreamWriter sw = new StreamWriter(featureFile))
            {
                foreach (var line in lines)
                {
                    sw.WriteLine(line);
                    if (!line.Trim().StartsWith("#"))
                    {
                        int label = int.Parse(FeatureFileProcessor.SplitLabelQidVectorComment(line)[0]);
                        if (seletedLabels.Contains(label))
                        {
                            for (int i = 1; i < magnification; i++)
                                sw.WriteLine(line);
                        }
                    }
                }
            }
        }

        public static double GetInformationGain(IEnumerable<object> categories, IEnumerable<object> attributes)
        {
            double entropy = GetEntropy(categories);
            double expConditionEntropy = GetExpConditionEntropy(categories, attributes);
            double infoGain = entropy - expConditionEntropy;
            return infoGain;
        }

        public static double GetExpConditionEntropy(IEnumerable<object> categories, IEnumerable<object> attributes)
        {
            var disAttributes = attributes.Distinct();

            double expCondEntropy = 0;
            foreach (var disA in disAttributes)
            {
                List<object> selectCtgr = new List<object>();
                for (int i = 0; i < attributes.Count(); i++)
                {
                    if (attributes.ElementAt(i).Equals(disA))
                        selectCtgr.Add(categories.ElementAt(i));
                }

                double prob = Convert.ToDouble(selectCtgr.Count()) / categories.Count();
                double entropy = GetEntropy(selectCtgr);

                expCondEntropy += prob * entropy;
            }

            return expCondEntropy;
        }

        public static double GetEntropy(IEnumerable<object> categories)
        {
            var distinct = categories.Distinct();

            int[] counts = new int[distinct.Count()];
            for (int i = 0; i < counts.Length; i++)
                counts[i] = categories.Count(c => c.Equals(distinct.ElementAt(i)));

            double entropy = 0;
            foreach (var count in counts)
            {
                double property = Convert.ToDouble(count) / counts.Sum();

                if (property >= 0)
                    entropy += -1 * property * Math.Log(property, 2);
            }

            return entropy;
        }

        public static Dictionary<string, long> GetLabelDic(string svmFeatureFile)
        {
            Dictionary<string, long> labelDic = new Dictionary<string, long>();
            using (StreamReader sr = new StreamReader(svmFeatureFile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().TrimStart();
                    if (!line.StartsWith("#"))
                    {
                        int firstSpace = line.IndexOf(' ');
                        if (firstSpace > 0)
                        {
                            string label = line.Substring(0, firstSpace);
                            if (!labelDic.ContainsKey(label))
                                labelDic.Add(label, 1);
                            else
                                labelDic[label]++;
                        }
                    }
                }
            }

            return labelDic;
        }

        public static Pair<string, int> GetLabelQidPair(string svmLine)
        {
            Pair<string, int> labelQidPair = new Pair<string, int>(string.Empty, -1);
            svmLine = svmLine.TrimStart();
            if (!svmLine.StartsWith("#"))
            {
                int labelSpace = svmLine.IndexOf(' ');
                if (labelSpace > 0)
                {
                    labelQidPair.First = svmLine.Substring(0, labelSpace);
                    svmLine = svmLine.Substring(labelSpace + 1);
                }
                else
                    labelQidPair.First = svmLine;

                int qidIndex = svmLine.IndexOf("qid:", StringComparison.OrdinalIgnoreCase);
                if (qidIndex >= 0)
                {
                    svmLine = svmLine.Substring(qidIndex);
                    int qidSpace = svmLine.IndexOf(' ');
                    string qidString = svmLine;
                    if (qidSpace > 0)
                        qidString = svmLine.Substring("qid:".Length, qidSpace - "qid:".Length);
                    labelQidPair.Second = int.Parse(qidString);
                }
            }

            return labelQidPair;
        }

        public static string[] SplitLabelQidVectorComment(string svmLine)
        {
            string[] labelQidVectorComment = new string[4];
            try
            {
                //If comment, return null;
                svmLine = svmLine.TrimStart();
                if (svmLine.StartsWith("#"))
                    return null;

                //Label
                int labelSpace = svmLine.IndexOf(' ');
                if (labelSpace > 0)
                {
                    string labelString = svmLine.Substring(0, labelSpace);
                    int labelIndex = -1;
                    if (int.TryParse(labelString, out labelIndex))
                        labelQidVectorComment[0] = svmLine.Substring(0, labelSpace).Trim();
                }
                else
                {
                    int labelIndex = -1;
                    if (int.TryParse(svmLine, out labelIndex))
                        labelQidVectorComment[0] = svmLine.Trim();
                }
                if (labelQidVectorComment[0] != null)
                    svmLine = svmLine.Substring(labelQidVectorComment[0].Length);
                if (string.IsNullOrEmpty(svmLine))
                    return labelQidVectorComment;

                //Qid
                int qidIndex = svmLine.IndexOf("qid:", StringComparison.OrdinalIgnoreCase);
                if (qidIndex >= 0)
                {
                    svmLine = svmLine.Substring(qidIndex);
                    int qidSpace = svmLine.IndexOf(' ');
                    if (qidSpace > 0)
                        labelQidVectorComment[1] = svmLine.Substring(0, qidSpace).Trim();
                    else
                        labelQidVectorComment[1] = svmLine.Trim();

                    svmLine = svmLine.Substring(labelQidVectorComment[1].Length);
                }
                if (string.IsNullOrEmpty(svmLine))
                    return labelQidVectorComment;

                //Vector and comment
                int commentIndex = svmLine.IndexOf('#');
                if (commentIndex >= 0)
                {
                    labelQidVectorComment[2] = svmLine.Substring(0, commentIndex).Trim();
                    labelQidVectorComment[3] = svmLine.Substring(commentIndex + 1).Trim();
                }
                else
                    labelQidVectorComment[2] = svmLine.Trim();

                return labelQidVectorComment;
            }
            catch (Exception e)
            {
                return labelQidVectorComment;
            }
        }

        public static string TransferFeatureFile_SvmToRankSvm(string svmFeatureFile, string zeroOperation, bool labelCoarse)
        {
            var labelDic = GetLabelDic(svmFeatureFile);
            var labelInt = labelDic.Keys.Select(k => int.Parse(k)).ToList();

            if (labelCoarse)
            {
                bool containZero = labelInt.Contains(0);
                int minLabelInt = labelInt.Min();
                foreach (var label in labelInt)
                {
                    if (label < 0)
                        labelDic[label.ToString()] = label - minLabelInt + 1;
                    else if (label == 0)
                    {
                        if (zeroOperation == "1" || zeroOperation == "0")
                            labelDic[label.ToString()] = 1 - minLabelInt;
                        else if (zeroOperation == "-1")
                            labelDic[label.ToString()] = -minLabelInt;
                    }
                    else
                    {
                        if (containZero && zeroOperation == "0")
                            labelDic[label.ToString()] = label - minLabelInt + 1;
                        else
                            labelDic[label.ToString()] = label - minLabelInt;
                    }
                }
            }

            string rankSvmFilePath = FilePath.ChangeFile(svmFeatureFile, GetTransferFeatureFileName(svmFeatureFile, TrainMethods.RankSVM));
            var lines = FileReader.ReadRows(svmFeatureFile);
            using (StreamWriter sw = new StreamWriter(rankSvmFilePath))
            {
                bool zeroRemainAndExists = zeroOperation == "0" && lines.Exists(line => line.StartsWith("0"));
                foreach (var line in lines)
                {
                    if (line.TrimStart().StartsWith("#"))
                        sw.WriteLine(line);
                    else
                    {
                        var lqvc = SplitLabelQidVectorComment(line);
                        if (lqvc[0].Trim() == "0" && zeroOperation == "Remove")
                            continue;

                        if (labelCoarse)
                        {
                            if (int.Parse(lqvc[0]) > 0)
                                lqvc[0] = (zeroRemainAndExists ? 3 : 2).ToString();
                            else if (int.Parse(lqvc[0]) == 0)
                            {
                                if (zeroRemainAndExists || zeroOperation == "1")
                                    lqvc[0] = (2).ToString();
                                else
                                    lqvc[0] = (1).ToString();
                            }
                            else
                                lqvc[0] = (1).ToString();
                        }
                        else if (!string.IsNullOrEmpty(lqvc[0]))
                            lqvc[0] = labelDic[lqvc[0]].ToString();
                        else
                            continue;


                        var newLine = lqvc[0].Trim();               //Label
                        if (!string.IsNullOrEmpty(lqvc[1].Trim()))  //Qid
                            newLine += " " + lqvc[1].Trim();
                        if (!string.IsNullOrEmpty(lqvc[2].Trim()))  //Vector
                            newLine += " " + lqvc[2].Trim();
                        if (!string.IsNullOrEmpty(lqvc[3].Trim()))  //Comment
                            newLine += " # " + lqvc[3].Trim();

                        sw.WriteLine(newLine);
                    }
                }
            }

            return rankSvmFilePath;
        }

        public static string TransferFeatureFile_RankSvmToSvm(string rankSvmFeatureFile, double splitLabel, string zeroOperation, bool labelCoarse)
        {
            string svmFilePath = GetTransferFeatureFileName(rankSvmFeatureFile, TrainMethods.SVM);
            var lines = FileReader.ReadRows(rankSvmFeatureFile);
            using (StreamWriter sw = new StreamWriter(svmFilePath))
            {
                foreach (var line in lines)
                {
                    if (line.TrimStart().StartsWith("#"))
                        sw.WriteLine(line);
                    else
                    {
                        var lqvc = SplitLabelQidVectorComment(line);
                        if (!string.IsNullOrEmpty(lqvc[0]))
                        {
                            double label = -1;
                            if (!double.TryParse(lqvc[0], out label))
                                label = splitLabel;

                            if (label > splitLabel)
                                lqvc[0] = labelCoarse ? "+1" : (label - splitLabel).ToString();
                            else if (label == splitLabel)
                                lqvc[0] = zeroOperation;
                            else
                                lqvc[0] = labelCoarse ? "-1" : (label - splitLabel).ToString();
                        }
                        else
                            continue;

                        if (lqvc[0] == "Remove")
                            continue;

                        var newLine = lqvc[0].Trim();               //Label
                        if (!string.IsNullOrEmpty(lqvc[1].Trim()))  //Qid
                            newLine += " " + lqvc[1].Trim();
                        if (!string.IsNullOrEmpty(lqvc[2].Trim()))  //Vector
                            newLine += " " + lqvc[2].Trim();
                        if (!string.IsNullOrEmpty(lqvc[3].Trim()))  //Comment
                            newLine += " # " + lqvc[3].Trim();

                        sw.WriteLine(newLine);
                    }
                }
            }

            return svmFilePath;
        }

        public static string GetTransferFeatureFileName(string featureFile, TrainMethods destination)
        {
            List<string> fileNameItems = Path.GetFileNameWithoutExtension(featureFile).Split('.').ToList();
            int svmIndex = (int)Math.Max(fileNameItems.Select(s => s.ToLowerInvariant()).ToArray().IndexOf(TrainMethods.RankSVM.ToString().ToLowerInvariant()),
                fileNameItems.Select(s => s.ToLowerInvariant()).ToArray().IndexOf(TrainMethods.SVM.ToString().ToLowerInvariant()));
            if (svmIndex >= 0)
                fileNameItems[svmIndex] = destination == TrainMethods.None ? string.Empty : destination.ToString();
            else
                fileNameItems.Add(destination == TrainMethods.None ? string.Empty : destination.ToString());

            if (destination == TrainMethods.None)
                fileNameItems.RemoveAll(item => item.ToLowerInvariant() == "splittest" || item.ToLowerInvariant() == "splittrain" || item.ToLowerInvariant() == "splitdev" || item.Trim() == string.Empty);

            string extension = Path.GetExtension(featureFile).Trim('.');
            fileNameItems.Add(extension);
            return FilePath.ChangeFile(featureFile, fileNameItems.ConnectWords("."));
        }

        public static List<List<string>> ReadFeatureFile(string featureFile, out List<string> headers, FeatureFileFormat featureFileType = FeatureFileFormat.SvmLight)
        {
            if (featureFileType == FeatureFileFormat.SvmLight)
            {
                List<List<string>> outputs = new List<List<string>>();
                using (StreamReader sr = new StreamReader(featureFile))
                {
                    headers = new List<string>();
                    string line = sr.ReadLine().TrimStart();
                    while (line.StartsWith("#"))
                    {
                        headers.Add(line);
                        line = sr.ReadLine().TrimStart();
                    }
                    if (line.Substring(line.IndexOf(' ') + 1).StartsWith("qid", StringComparison.InvariantCultureIgnoreCase))
                    {
                        int lastQid = int.Parse(Regex.Match(line, @":\d+").Value.TrimStart(':'));
                        List<string> sample = new List<string>();

                        sample.Add(line);
                        outputs.Add(sample);

                        while (!sr.EndOfStream)
                        {
                            line = sr.ReadLine();
                            int qid = int.Parse(Regex.Match(line, @":\d+").Value.TrimStart(':'));

                            if (lastQid != qid)
                            {
                                sample = new List<string>();
                                sample.Add(line);

                                outputs.Add(sample);
                                lastQid = qid;
                            }
                            else
                                outputs.Last().Add(line);
                        }
                    }
                    else
                    {
                        List<string> sample = new List<string>();
                        sample.Add(line);
                        outputs.Add(sample);

                        while (!sr.EndOfStream)
                        {
                            sample = new List<string>();
                            sample.Add(sr.ReadLine());
                            outputs.Add(sample);
                        }
                    }
                }

                return outputs;
            }
            else if (featureFileType == FeatureFileFormat.BingFastRank)
            {
                List<List<string>> outputs = new List<List<string>>();
                using (StreamReader sr = new StreamReader(featureFile))
                {
                    string lastQid = "";
                    List<string> samples = new List<string>();

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] lineArray = line.Split('\t');

                        string qid = lineArray[1];
                        if (qid != lastQid)
                        {
                            samples = new List<string>();
                            outputs.Add(samples);
                        }

                        lastQid = qid;
                        samples.Add(line);
                    }

                    headers = null;
                    return outputs;
                }
            }
            else
            {
                throw new NotImplementedException("FeatureFileTypes Parameter of ReadFeatureFile(): " + featureFileType.ToString());
            }
        }

        public static List<List<string>> ReadFeatureFile(string featureFile, FeatureFileFormat featureFileType = FeatureFileFormat.SvmLight)
        {
            List<string> headers = null;
            return ReadFeatureFile(featureFile, out headers, featureFileType);
        }

        public static List<List<double>> ReadOutputFile(string outputFile, string featureFile, FeatureFileFormat featureFileType = FeatureFileFormat.SvmLight)
        {
            List<List<double>> outputsList = new List<List<double>>();

            using (StreamReader featureSr = new StreamReader(featureFile))
            using (StreamReader outputSr = new StreamReader(outputFile))
            {
                int qid = -1;
                List<double> outputs = new List<double>();

                while (!featureSr.EndOfStream && !outputSr.EndOfStream)
                {
                    string line = featureSr.ReadLine().TrimStart();
                    if (!line.StartsWith("#"))
                    {
                        double outputScore = double.Parse(outputSr.ReadLine());

                        //SvmLight format
                        FeatureLine fLine = new FeatureLine(line, false, false, false);
                        if (qid != fLine.Qid)
                        {
                            if (qid != -1)
                            {
                                outputsList.Add(outputs);
                                outputs = new List<double>();
                            }

                            qid = fLine.Qid;
                        }

                        outputs.Add(outputScore);
                    }
                }

                if (outputs.Count > 0)
                    outputsList.Add(outputs);

                return outputsList;
            }
        }

        public static List<List<Pair<string, double>>> ReadFeatureFileAndOutputFile(string featureFile, string outputFile, FeatureFileFormat featureFileType = FeatureFileFormat.SvmLight)
        {
            List<List<Pair<string, double>>> foPairsList = new List<List<Pair<string, double>>>();
            var featureLinesList = ReadFeatureFile(featureFile, featureFileType);

            using (StreamReader sr = new StreamReader(outputFile))
            {
                foreach (var featureLines in featureLinesList)
                {
                    List<Pair<string, double>> foPairs = new List<Pair<string, double>>();
                    foPairsList.Add(foPairs);

                    foreach (var featureLine in featureLines)
                    {
                        var output = double.Parse(sr.ReadLine());
                        foPairs.Add(new Pair<string, double>(featureLine, output));
                    }
                }
            }

            return foPairsList;
        }

        public static List<FeatureLine> ReadFeatureFileToFeatureLines(string featureFile, bool storeToItemsDic = true, bool saveVectorString = true, bool saveComment = true)
        {
            ConsoleWriter.WriteCurrentMethodStarted();
            Console.WriteLine(featureFile);

            long lineCount = FileReader.GetRowCount(featureFile, ItemOperator.None);
            List<FeatureLine> featureLines = new List<FeatureLine>();
            long finished = 0;

            using (StreamReader sr = new StreamReader(featureFile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().TrimStart();
                    if (!line.StartsWith("#"))
                        featureLines.Add(new FeatureLine(line, storeToItemsDic, saveVectorString, saveComment));

                    ++finished;
                    ConsoleWriter.WritePercentage(finished, lineCount);
                }
            }
            return featureLines;
        }

        public static IEnumerable<KeyValuePair<string, double>> ReadEvalScoreFile(string evalScoreFile)
        {
            using (StreamReader sr = new StreamReader(evalScoreFile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    if (line != string.Empty)
                    {
                        string[] lineArray = line.Split('\t');
                        if (lineArray[0].ToLower() == "precision:" || lineArray[0].ToLower() == "map:" || lineArray[0].ToLower() == "ndcg:")
                        {
                            string propertyName = lineArray[0].TrimEnd(':');

                            if (lineArray.Length == 2)
                            {
                                yield return new KeyValuePair<string, double>(propertyName, double.Parse(lineArray[1]));
                            }
                            else if (lineArray.Length > 2)
                            {
                                for (int i = 1; i < lineArray.Length; i++)
                                    yield return new KeyValuePair<string, double>(propertyName + i, double.Parse(lineArray[i]));
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<KeyValuePair<string, double>> ReadTTestFile(string tTestFile)
        {
            using (StreamReader sr = new StreamReader(tTestFile))
            {
                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split('\t');
                    if (lineArray.Length == 6)
                    {
                        double pValue = -1;
                        if (double.TryParse(lineArray[5], out pValue))
                            yield return new KeyValuePair<string, double>("P-Value " + lineArray[0], pValue);
                    }
                }
            }
        }

        public static string TransformToBingFastRankFormat(string file, int featureDimensionCount, bool forceReproduce = false)
        {
            string outputFile = FilePath.ChangeExtension(file, FeatureFileFormat.BingFastRank.ToString());
            if (!forceReproduce && File.Exists(outputFile))
                return outputFile;

            using (StreamWriter sw = new StreamWriter(outputFile))
            using (StreamReader sr = new StreamReader(file))
            {
                int lastQid = -1;
                int docID = 1;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().TrimStart();
                    if (line.StartsWith("#"))
                        continue;

                    FeatureLine featureLine = new FeatureLine(line);
                    if (lastQid == featureLine.Qid)
                    {
                        docID++;
                    }
                    else
                    {
                        lastQid = featureLine.Qid;
                        docID = 1;
                    }

                    string bingLine = featureLine.Label + "\t" + lastQid + "\t" + (lastQid * 1000000 + docID);
                    for (int i = 0; i < featureDimensionCount; ++i)
                    {
                        if (featureLine.ItemsDic.ContainsKey(i + 1))
                            bingLine += "\t" + featureLine.ItemsDic[i + 1];
                        else
                            bingLine += "\t0";
                    }
                    sw.WriteLine(bingLine);
                }
            }

            return outputFile;
        }

        public static int GetDimentionCount(string featureFile)
        {
            using (StreamReader sr = new StreamReader(featureFile))
            {
                int maxKey = 0;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().TrimStart();
                    if (!line.StartsWith("#"))
                    {
                        FeatureLine featureLine = new FeatureLine(line, true, false, false);
                        if (featureLine.ItemsDic.Count > 0)
                        {
                            var maxLineKey = featureLine.ItemsDic.Keys.Max();
                            if (maxLineKey > maxKey)
                                maxKey = maxLineKey;
                        }
                    }
                }

                return maxKey;
            }
        }

        public static string ConvertToIntergerValues(string featureFile, FeatureFileFormat featureFileType)
        {
            var samplesList = FeatureFileProcessor.ReadFeatureFile(featureFile, featureFileType);
            int additionalGrade = 4;
            int dimentionCount = FeatureFileProcessor.GetDimentionCount(featureFile);
            string intergerFile = FilePath.ChangeExtension(featureFile, "Interger.feature");

            int guid = 1;
            Dictionary<int, double> minValueDic = new Dictionary<int, double>();
            Dictionary<int, bool> isAllIntDic = new Dictionary<int, bool>();
            Dictionary<int, bool> isTooLargeDic = new Dictionary<int, bool>();
            List<List<FeatureLine>> featureLinesList = new List<List<FeatureLine>>();

            foreach (var samples in samplesList)
            {
                List<FeatureLine> featureLines = new List<FeatureLine>();
                featureLinesList.Add(featureLines);

                foreach (var sample in samples)
                {
                    FeatureLine featureLine = new FeatureLine(sample, true, false, true);
                    featureLines.Add(featureLine);

                    foreach (var kv in featureLine.ItemsDic)
                    {
                        if (!minValueDic.ContainsKey(kv.Key))
                            minValueDic.Add(kv.Key, Math.Abs(kv.Value));
                        else if (minValueDic[kv.Key] > Math.Abs(kv.Value))
                            minValueDic[kv.Key] = Math.Abs(kv.Value);

                        if (isAllIntDic.ContainsKey(kv.Key) && isAllIntDic[kv.Key])
                        {
                            int interger = -1;
                            isAllIntDic[kv.Key] = int.TryParse(kv.Value.ToString(), out interger);
                        }
                        else if (!isAllIntDic.ContainsKey(kv.Key))
                        {
                            int interger = -1;
                            isAllIntDic.Add(kv.Key, int.TryParse(kv.Value.ToString(), out interger));
                        }

                        if (isTooLargeDic.ContainsKey(kv.Key))
                        {
                            if (kv.Value > 1000000)
                                isTooLargeDic[kv.Key] = true;
                        }
                        else
                        {
                            if (kv.Value <= 1000000)
                                isTooLargeDic.Add(kv.Key, false);
                            else
                                isTooLargeDic.Add(kv.Key, true);
                        }
                    }
                }
            }

            Dictionary<int, int> multiplyDic = new Dictionary<int, int>();

            foreach (var key in minValueDic.Keys)
            {
                if (isAllIntDic[key] || isTooLargeDic[key])
                {
                    multiplyDic.Add(key, 1);
                }
                else
                {
                    if (minValueDic[key] >= 1)
                    {
                        multiplyDic.Add(key, Convert.ToInt32(Math.Pow(10, additionalGrade)));
                    }
                    else
                    {
                        int grade = 1;
                        while (minValueDic[key] * Math.Pow(10, grade) < 1)
                            ++grade;

                        multiplyDic.Add(key, Convert.ToInt32(Math.Pow(10, grade + additionalGrade)));
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(intergerFile))
            {
                foreach (var featureLines in featureLinesList)
                {
                    foreach (var featureLine in featureLines)
                    {
                        Dictionary<int, double> newItemsDic = new Dictionary<int, double>();
                        foreach (var kv in featureLine.ItemsDic)
                            newItemsDic.Add(kv.Key, Convert.ToInt64(kv.Value * multiplyDic[kv.Key]));
                        featureLine.ItemsDic = newItemsDic;

                        if (featureFileType == FeatureFileFormat.SvmLight)
                        {
                            featureLine.ResetVectorString();
                            sw.WriteLine(featureLine.GetFeatureVectorLine());
                        }
                        else if (featureFileType == FeatureFileFormat.BingFastRank)
                        {
                            sw.WriteLine(featureLine.ToBingFormat(guid++, dimentionCount));
                        }
                    }
                }
            }

            return intergerFile;
        }

        public static string BingFastRankFormatGetRange(string file, int start, int length)
        {
            string outputFile = FilePath.ChangeExtension(file, "selected.BingFastRank");
            using (StreamReader sr = new StreamReader(file))
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                while (!sr.EndOfStream)
                {
                    string[] lineArray = sr.ReadLine().Split('\t');
                    int validLength = Convert.ToInt32(Math.Min(length, lineArray.Length - 3 - start));

                    string outputLine = lineArray.GetRange(0, 3).ConnectWords("\t") + "\t" + lineArray.GetRange(3 + start, validLength).ConnectWords("\t");
                    sw.WriteLine(outputLine);
                }
            }
            return outputFile;
        }

        public static string BingFastRankFormatGetRange(string file, int start)
        {
            return BingFastRankFormatGetRange(file, start, int.MaxValue);
        }

        public static string GetRangeFeatureFile(string file, FeatureFileFormat format, int start)
        {
            return GetRangeFeatureFile(file, format, start, int.MaxValue);
        }

        public static string GetRangeFeatureFile(string file, FeatureFileFormat format, int start, int length)
        {
            string rangeFeatureFile_Svmlight = FilePath.ChangeExtension(file, "range.feature");
            using (StreamReader sr = new StreamReader(file))
            using (StreamWriter sw = new StreamWriter(rangeFeatureFile_Svmlight))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().TrimStart();
                    if (!line.StartsWith("#"))
                    {
                        FeatureLine featureLine = new FeatureLine(line, true, false, true);
                        sw.WriteLine(featureLine.GetRangeFeatureLine(start, length));
                    }
                    else
                    {
                        sw.WriteLine(line);
                    }
                }
            }

            if (format == FeatureFileFormat.BingFastRank)
            {
                int dimentionCount = GetDimentionCount(rangeFeatureFile_Svmlight);
                string rangeFeatureFile_BingFastRank = TransformToBingFastRankFormat(rangeFeatureFile_Svmlight, dimentionCount);

                File.Delete(rangeFeatureFile_Svmlight);
                return rangeFeatureFile_BingFastRank;
            }
            else
            {
                return rangeFeatureFile_Svmlight;
            }
        }

        public static FeatureFileFormat GetFeatureFileFormat(string featureFile)
        {
            using (StreamReader sr = new StreamReader(featureFile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().TrimStart();
                    if (!line.StartsWith("#"))
                    {
                        string label = Regex.Match(line, LabelRegStr).Value;
                        if (label.EndsWith(" "))
                            return FeatureFileFormat.SvmLight;
                        else
                            return FeatureFileFormat.BingFastRank;
                    }
                }

                return FeatureFileFormat.SvmLight;
            }
        }

        public static string TransferFeatureFileFormat(string featureFile, FeatureFileFormat sourceFormat, FeatureFileFormat targetFormat, bool isForceReproduce = false, int dimentionCount = -1)
        {
            if (sourceFormat == targetFormat)
                return featureFile;

            string targetFile = FilePath.ChangeExtension(featureFile, targetFormat.ToString());
            if (File.Exists(targetFile) && !isForceReproduce)
                return targetFile;

            if (sourceFormat == FeatureFileFormat.SvmLight && targetFormat == FeatureFileFormat.BingFastRank)
            {
                if(dimentionCount < 0)
                    dimentionCount = FeatureFileProcessor.GetDimentionCount(featureFile);

                using (StreamReader sr = new StreamReader(featureFile))
                using (StreamWriter sw = new StreamWriter(targetFile))
                {
                    int guid = 0;

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine().TrimStart();
                        if (!line.StartsWith("#"))
                        {
                            FeatureLine featureLine = new FeatureLine(line, true, false, true);
                            sw.WriteLine(featureLine.ToBingFormat(guid++, dimentionCount));
                        }
                    }
                }
            }
            else if (sourceFormat == FeatureFileFormat.BingFastRank && targetFormat == FeatureFileFormat.SvmLight)
            {
                using (StreamReader sr = new StreamReader(featureFile))
                using (StreamWriter sw = new StreamWriter(targetFile))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine().TrimStart();
                        if (!line.StartsWith("#"))
                        {
                            FeatureLine featureLine = new FeatureLine(line, true, false, true);
                            featureLine.ResetVectorString();
                            sw.WriteLine(featureLine.GetFeatureVectorLine());
                        }
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return targetFile;
        }
    }

    public class FeatureLine
    {
        public int Label;
        public int Qid;
        public Dictionary<int, double> ItemsDic;
        public string VectorString;
        public string Comment;

        public FeatureLine()
        {
            Label = 0;
            Qid = -1;
            VectorString = null;
            Comment = null;
        }

        public FeatureLine(string featureVectorLine, bool storeToItemsDic = true, bool saveVectorString = true, bool saveComment = true, FeatureFileFormat featureLineType = FeatureFileFormat.SvmLight)
        {
            try
            {
                FeatureFileFormat realType = featureLineType;
                if (Regex.IsMatch(featureVectorLine, MetricUtility.LabelRegStr)
                    && Regex.Match(featureVectorLine, MetricUtility.LabelRegStr).Value.EndsWith("\t"))
                    realType = FeatureFileFormat.BingFastRank;

                if (realType == FeatureFileFormat.SvmLight)
                {
                    var lqvc = FeatureFileProcessor.SplitLabelQidVectorComment(featureVectorLine);

                    if (!int.TryParse(lqvc[0], out Label))
                        Label = -1;

                    if (lqvc[1] == null || !int.TryParse(lqvc[1].Trim("qid:"), out Qid))
                        Qid = -1;

                    if (storeToItemsDic)
                        SetItemsDic(lqvc[2].Trim(), saveVectorString);

                    if (saveVectorString)
                        VectorString = lqvc[2].Trim();

                    if (saveComment && !string.IsNullOrEmpty(lqvc[3]))
                        Comment = lqvc[3];
                }
                else if (realType == FeatureFileFormat.BingFastRank)
                {
                    var items = featureVectorLine.Split('\t');
                    Label = int.Parse(items[0]);

                    if (!int.TryParse(items[1], out Qid))
                        Qid = -1;

                    if (storeToItemsDic)
                    {
                        ItemsDic = new Dictionary<int, double>();
                        for (int i = 3; i < items.Length; ++i)
                        {
                            int key = i - 2;
                            double value = 0;

                            if (items[i].StartsWith("#"))
                                break;

                            if (items[i].Contains(':'))
                            {
                                var kv = items[i].Split(':');
                                key = int.Parse(kv[0]);
                                value = double.Parse(kv[1]);
                            }
                            else
                            {
                                value = double.Parse(items[i]);
                            }

                            ItemsDic.Add(key, value);
                        }
                    }

                    if (saveVectorString)
                        VectorString = items.GetRange(3, items.Length - 3).ConnectWords("\t");

                    if (saveComment && featureVectorLine.Contains('#'))
                        Comment = featureVectorLine.Substring(featureVectorLine.IndexOf('#') + 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public void SetItemsDic(string vectorString, bool saveVectorString = true)
        {
            if (saveVectorString)
                this.VectorString = vectorString;

            ItemsDic = new Dictionary<int, double>();
            foreach (var item in vectorString.Split(' ').Select(str => str.Trim()))
            {
                if (item != string.Empty)
                {
                    var kv = item.Split(':');
                    if (kv.Length == 2)
                    {
                        int key = int.Parse(kv[0]);
                        double value = double.Parse(kv[1]);

                        ItemsDic.Add(key, value);
                    }
                }
            }
        }

        public void Normalize(Dictionary<int, Pair<double, double>> maxMinDic)
        {
            Dictionary<int, double> newItemDic = new Dictionary<int, double>();
            foreach (var key in ItemsDic.Keys)
            {
                if (!maxMinDic.ContainsKey(key))
                {
                    newItemDic.Add(key, ItemsDic[key]);
                    continue;
                }

                double min = maxMinDic[key].Second;
                double max = maxMinDic[key].First;

                if (max < min)
                    throw new Exception(string.Format("Error: Max({0}) < Min{1} at key: {2}", max, min, key));

                if (min == max)
                    newItemDic.Add(key, ItemsDic[key] == 0 ? 0 : 1);
                else
                    newItemDic.Add(key, (ItemsDic[key] - min) / (max - min));
            }

            ItemsDic = newItemDic;
            VectorString = "";

            foreach (var kv in ItemsDic.OrderBy(kv => kv.Key))
            {
                if (kv.Value != 0)
                    VectorString += kv.Key + ":" + kv.Value + " ";
            }

            VectorString = VectorString.TrimEnd();
        }

        private double Normalize(double value, double min, double max)
        {
            if (min == max)
            {
                if (value == 0)
                    return 0;
                else
                    return 1;
            }

            if (min > 0)
                min = 0;

            return (value - min) / (max - min);
        }

        public string GetFeatureVectorLine()
        {
            string outputLine = Label.ToString();

            if (Qid >= 0)
                outputLine += " qid:" + Qid;

            if (!string.IsNullOrEmpty(VectorString.Trim()))
                outputLine += " " + VectorString.Trim();

            if (!string.IsNullOrEmpty(Comment))
                outputLine += " # " + Comment;

            return outputLine;
        }

        public void ResetVectorString()
        {
            VectorString = "";
            foreach (var kv in ItemsDic.OrderBy(kv => kv.Key))
            {
                if (kv.Value != 0)
                    VectorString += kv.Key + ":" + kv.Value + " ";
            }
            VectorString = VectorString.TrimEnd();
        }

        public string GetRangeFeatureLine(int start)
        {
            return GetRangeFeatureLine(start, int.MaxValue);            
        }

        public string GetRangeFeatureLine(int start, int length)
        {
            string vector = "";
            foreach (var kv in ItemsDic.OrderBy(kv => kv.Key))
                if (kv.Value != 0 && kv.Key >= start && (length == int.MaxValue || kv.Key < start + length))
                    vector += kv.Key + ":" + kv.Value + " ";
            vector = vector.TrimEnd();

            string outputLine = Label.ToString();

            if (Qid >= 0)
                outputLine += " qid:" + Qid;

            if (!string.IsNullOrEmpty(vector.Trim()))
                outputLine += " " + vector.Trim();

            if (!string.IsNullOrEmpty(Comment))
                outputLine += " # " + Comment;

            return outputLine;
        }

        public string ToBingFormat(int guid, int dimentionCount)
        {
            string outputLine = Label.ToString() +
            "\t" + Qid +
            "\t" + guid;

            int startIndex = 1;
            while (startIndex <= dimentionCount)
            {
                if (ItemsDic.ContainsKey(startIndex))
                    outputLine += "\t" + ItemsDic[startIndex];
                else
                    outputLine += "\t0";

                ++startIndex;
            }

            return outputLine;
        }
    }
}
