using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib.MachineLearning
{
    [Serializable]
    public enum RunMode
    {
        CrossValidation,
        Train,
        Test
    }

    [Serializable]
    public enum MachineLearningTask
    {
        BinaryClassification,
        MultiClassification,
        Regression,
        Ranking
    }

    [Serializable]
    public class Utility
    {
        public static readonly string[] BinaryClassificationLearners = new string[] { "SvmLight", "Boosted Tree Classification (Fast Rank)" };
        public static readonly string[] MultiClassificationLeaners = new string[] { "Multiclass SvmLight", "Multiclass Logistic Regression" };
        public static readonly string[] RankLeaners = new string[] { "SvmLight RankSvm", "SvmLight RankSvm Fast", "Bing FastRank (Boosted Tree)" };
        public static readonly string[] RegressionLeaners = new string[] { "Boosted Tree Regression (Fast Rank)", "Poisson Regression" };

        public static FeatureFileFormat GetFeatureFileFormat(MachineLearningTask task, string learner)
        {
            if (learner == BinaryClassificationLearners[1]
                || learner == MultiClassificationLeaners[1]
                || learner == RankLeaners[2]
                || learner == RegressionLeaners[0])
                return FeatureFileFormat.BingFastRank;

            return FeatureFileFormat.SvmLight;
        }
    }

    [Serializable]
    public enum FeatureFileFormat
    {
        SvmLight,
        BingFastRank,
    }
}
