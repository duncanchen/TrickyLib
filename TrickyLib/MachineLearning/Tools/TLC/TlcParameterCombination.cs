using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib.MachineLearning.Tools.TLC
{
    [Serializable]
    public class TlcParameterCombination : BaseParameterCombination
    {
        public TlcParameterCombination(TlcParameterSetting parent, MachineLearningTask task, string learner) : base(parent, task, learner) { }

        public int NumberLeaves { get; set; }
        public double LearningRate { get; set; }
        public int NumTrees { get; set; }
        public int MinInstancesPerLeaf { get; set; }

        public override string GetParameterString()
        {
            return string.Format("nl:{0};lr:{1};iter:{2};mil:{3}", NumberLeaves, LearningRate, NumTrees, MinInstancesPerLeaf);
        }
    }
}
