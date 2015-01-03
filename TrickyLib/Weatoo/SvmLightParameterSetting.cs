using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using TrickyLib.CustomControl.PropertyGrid.Rule;

namespace TrickyLib.MachineLearning.Tools
{
    public class SvmLightParameterSetting : BaseParameterSetting
    {
        private string _c = "0.1";
        [Browsable(true)]
        [Category("Parameter Setting")]
        [PatternRule("^(( *\\d+(\\.\\d+)? *)(,( *\\d+(\\.\\d+)? *))*|( *\\d+(\\.\\d+)? *):( *\\d+(\\.\\d+)? *):( *\\d+(\\.\\d+)? *))$", "1.1,2,3.4 or 1.0:0.1:5 (begin:step:end)")]
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

        public override void ChangeVisibility(MachineLearningTask task, string learner)
        {
            SetPropertyVisibility(this, "C", task != MachineLearningTask.Ranking);
        }
    }
}
