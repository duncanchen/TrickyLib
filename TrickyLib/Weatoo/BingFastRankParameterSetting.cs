using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TrickyLib.CustomControl.PropertyGrid.Rule;

namespace TrickyLib.MachineLearning.Tools
{
    public class BingFastRankParameterSetting : BaseParameterSetting
    {
        private string _numberLeaves = "2,4,16,64";
        [Browsable(true)]
        [Category("Parameter Setting")]
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
        [Browsable(true)]
        [Category("Parameter Setting")]
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
        [Browsable(true)]
        [Category("Parameter Setting")]
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
        [Browsable(true)]
        [Category("Parameter Setting")]
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

        public override void ChangeVisibility(MachineLearningTask task, string learner)
        {
            
        }
    }
}
