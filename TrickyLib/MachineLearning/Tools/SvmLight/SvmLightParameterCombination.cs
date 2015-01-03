using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib.MachineLearning.Tools.SvmLight
{
    [Serializable]
    public class SvmLightParameterCombination : BaseParameterCombination
    {
        public SvmLightParameterCombination(SvmLightParameterSetting parent, MachineLearningTask task, string learner) : base(parent, task, learner) { }

        public double C { get; set; }
        public double J { get; set; }
        public double B { get; set; }

        public override string GetParameterString()
        {
            if (Task == MachineLearningTask.BinaryClassification)
                return string.Format("-c {0} -j {1} -b {2}", C, J, B);
            else if (Task == MachineLearningTask.Ranking)
                return string.Format("-c {0} -j {1}", C, J);
            else
                throw new NotImplementedException();
        }
    }
}
