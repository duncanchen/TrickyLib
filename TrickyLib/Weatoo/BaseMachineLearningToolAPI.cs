using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyLib.Threading;

namespace TrickyLib.MachineLearning.Tools
{
    public abstract class BaseMachineLearningToolAPI : BaseThreadPool
    {
        public BaseExperimentSetting ExpSetting { get; set; }

        public void Run(BaseParameterSetting paraSetting)
        { 
            
        }
    }
}
