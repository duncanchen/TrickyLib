using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib.MachineLearning.Tools
{
    [Serializable]
    public abstract class BaseParameterCombination
    {
        public BaseParameterSetting Parent { get; private set; }
        public MachineLearningTask Task { get; private set; }
        public string Learner { get; private set; }

        public BaseParameterCombination(BaseParameterSetting parent, MachineLearningTask task, string learner)
        {
            Task = task;
            Learner = learner;
        }

        public abstract string GetParameterString();
    }
}
