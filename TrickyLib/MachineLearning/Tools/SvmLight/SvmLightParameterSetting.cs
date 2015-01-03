using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using TrickyLib.CustomControl.PropertyGrid.Rule;
using System.Text.RegularExpressions;

namespace TrickyLib.MachineLearning.Tools.SvmLight
{
    [Serializable]
    public class SvmLightParameterSetting : BaseParameterSetting
    {
        private string _c = "0.1";
        [Browsable(true)]
        [Category("Parameter Setting")]
        [PatternRule("^(( *\\d+(\\.\\d+)? *)(,( *\\d+(\\.\\d+)? *))*|( *\\d+(\\.\\d+)? *):( *\\d+(\\.\\d+)? *)?:( *\\d+(\\.\\d+)? *))$", "1.1,2,3.4 or 1.0:0.1:5 (begin:step:end) or 0.1::1 (0.1,0.2,0.5,1)")]
        public string C
        {
            get
            {
                return _c;
            }
            set
            {
                _c = value;
            }
        }

        private string _j = "1";
        [Browsable(true)]
        [Category("Parameter Setting")]
        [PatternRule("^(( *\\d+(\\.\\d+)? *)(,( *\\d+(\\.\\d+)? *))*|( *\\d+(\\.\\d+)? *):( *\\d+(\\.\\d+)? *):( *\\d+(\\.\\d+)? *))$", "1.1,2,3.4 or 1.0:0.1:5 (begin:step:end)")]
        public string J
        {
            get
            {
                return _j;
            }
            set
            {
                _j = value;
            }
        }

        private string _b = "1";
        [Browsable(true)]
        [Category("Parameter Setting")]
        [PatternRule("^ *(0|1|0 *, *1)$", "Only valid: 0 or 1 or 0, 1")]
        public string B
        {
            get
            {
                return _b;
            }
            set
            {
                _b = value;
            }
        }

        public override void ChangeVisibility(MachineLearningTask task, string learner)
        {
            base.ChangeVisibility(task, learner);
            SetPropertyVisibility(this, "B", task != MachineLearningTask.Ranking);
            //SetPropertyVisibility(this, "J", task != MachineLearningTask.Ranking);
            //SetPropertyVisibility(this, "C", true);
        }

        public override IEnumerable<BaseParameterCombination> GetParameterCombinations(MachineLearningTask task, string learner)
        {
            if (task == MachineLearningTask.Ranking)
            {
                foreach (var c in GetParameterValues_Double(C))
                    foreach (var j in GetParameterValues_Double(J))
                        yield return new SvmLightParameterCombination(this, task, learner) { C = c, J = j };
            }
            else if (task == MachineLearningTask.BinaryClassification)
            {
                foreach (var c in GetParameterValues_Double(C))
                    foreach (var j in GetParameterValues_Double(J))
                        foreach (var b in GetParameterValues_Int(B))
                            yield return new SvmLightParameterCombination(this, task, learner) { C = c, J = j, B = b };
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override void ReadParameterString(string parameter)
        {
            var items = parameter.Split(' ');
            for (int i = 0; i < items.Length; i += 2)
            {
                if (items[i].ToLower() == "-c")
                    C = items[i + 1];
                else if (items[i].ToLower() == "-j")
                    J = items[i + 1];
                else if (items[i].ToLower() == "-b")
                    B = items[i + 1];
                else
                    throw new Exception("Cannot parse parameter. Correct example: -c 1 -j 2 -b 1");
            }
        }
    }
}
