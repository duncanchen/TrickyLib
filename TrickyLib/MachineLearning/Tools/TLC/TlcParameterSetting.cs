using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TrickyLib.CustomControl.PropertyGrid.Rule;

namespace TrickyLib.MachineLearning.Tools.TLC
{
    [Serializable]
    public class TlcParameterSetting : BaseParameterSetting
    {
        private string _numberLeaves = "2,4,16,64";
        [Browsable(true), Category("Parameter Setting")]
        [PatternRule("^(( *\\d+ *)(,( *\\d+ *))*|( *\\d+ *):( *\\d+ *):( *\\d+ *))$", "10,20,30 or 1:5:11 (begin:step:end)")]
        public string NumberLeaves
        {
            get
            {
                return _numberLeaves;
            }
            set
            {
                _numberLeaves = value;
            }
        }

        private string _learningRate = "0.01:0.05:0.41";
        [Browsable(true), Category("Parameter Setting")]
        [PatternRule("^(( *\\d+(\\.\\d+)? *)(,( *\\d+(\\.\\d+)? *))*|( *\\d+(\\.\\d+)? *):( *\\d+(\\.\\d+)? *):( *\\d+(\\.\\d+)? *))$", "1.1,2,3.4 or 1.0:0.1:5 (begin:step:end)")]
        public string LearningRate
        {
            get
            {
                return _learningRate;
            }
            set
            {
                _learningRate = value;
            }
        }

        private string _numTrees = "10, 50, 200, 1000";
        [Browsable(true), Category("Parameter Setting")]
        [PatternRule("^(( *\\d+ *)(,( *\\d+ *))*|( *\\d+ *):( *\\d+ *):( *\\d+ *))$", "10,20,30 or 1:5:11 (begin:step:end)")]
        public string NumTrees
        {
            get
            {
                return _numTrees;
            }
            set
            {
                _numTrees = value;
            }
        }

        private string _minInstancesPerLeaf = "10, 50, 250";
        [Browsable(true), Category("Parameter Setting")]
        [PatternRule("^(( *\\d+ *)(,( *\\d+ *))*|( *\\d+ *):( *\\d+ *):( *\\d+ *))$", "10,20,30 or 1:5:11 (begin:step:end)")]
        public string MinInstancesPerLeaf
        {
            get
            {
                return _minInstancesPerLeaf;
            }
            set
            {
                _minInstancesPerLeaf = value;
            }
        }

        public override IEnumerable<BaseParameterCombination> GetParameterCombinations(MachineLearningTask task, string learner)
        {
            if (task == MachineLearningTask.Ranking)
            {
                foreach (var nl in GetParameterValues_Int(NumberLeaves))
                    foreach (var lr in GetParameterValues_Double(LearningRate))
                        foreach (var nt in GetParameterValues_Int(NumTrees))
                            foreach (var mipl in GetParameterValues_Int(MinInstancesPerLeaf))
                                yield return new TlcParameterCombination(this, task, learner) { NumberLeaves = nl, LearningRate = lr, NumTrees = nt, MinInstancesPerLeaf = mipl };
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override void ReadParameterString(string parameter)
        {
            var items = parameter.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in items)
            {
                var kv = item.Split(':');
                if (kv[0].ToLower() == "nl")
                    NumberLeaves = kv[1];

                else if (kv[0].ToLower() == "lr")
                    LearningRate = kv[1];

                else if (kv[0].ToLower() == "iter")
                    NumTrees = kv[1];

                else if (kv[0].ToLower() == "mil")
                    MinInstancesPerLeaf = kv[1];

                else
                    throw new Exception("Cannot parse the parameter. Example: nl:1 lr:1.1 iter:100 mil:2");
            }
        }
    }
}
