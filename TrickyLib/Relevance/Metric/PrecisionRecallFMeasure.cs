using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TrickyLib.MachineLearning;

namespace TrickyLib.Relevance.Metric
{
    public class PrecisionRecallFMeasure
    {
        public TrainTestResult GetResult(string featureFile, string outputFile)
        {
            int declareTrue = 0;
            int reallyTrue = 0;
            int correctTrue = 0;

            using (StreamReader fSr = new StreamReader(featureFile))
            using (StreamReader oSr = new StreamReader(outputFile))
            {
                while (!fSr.EndOfStream && !oSr.EndOfStream)
                {
                    string line = fSr.ReadLine().TrimStart();
                    if (!line.StartsWith("#"))
                    {
                        int label = int.Parse(Regex.Match(line, MetricUtility.LabelRegStr).Value);
                        bool isReallyTrue = false;
                        if (label > 0)
                            isReallyTrue = true;

                        double output = double.Parse(oSr.ReadLine().Trim());
                        bool isDeclareTrue = false;
                        if (output > MetricUtility.PositiveThreshold)
                            isDeclareTrue = true;

                        if (isReallyTrue)
                            ++reallyTrue;

                        if (isDeclareTrue)
                            ++declareTrue;

                        if (isReallyTrue && isDeclareTrue)
                            ++correctTrue;
                    }
                }
            }

            double precision = 0;
            double recall = 0;
            double fMeasure = 0;

            if (reallyTrue > 0)
                precision = correctTrue * 1.0 / reallyTrue;

            if (declareTrue > 0)
                recall = correctTrue * 1.0 / declareTrue;

            if (reallyTrue > 0 || declareTrue > 0)
                fMeasure = 2.0 * precision * recall / (precision + recall);

            TrainTestResult result = new TrainTestResult();
            result.Add("Precision", precision);
            result.Add("Recall", recall);
            result.Add("F-Measure", fMeasure);
            return result;
        }
    }
}
