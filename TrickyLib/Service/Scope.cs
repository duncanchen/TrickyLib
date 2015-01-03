using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrickyLib.Service
{
    public class Scope
    {
        public static string ScopePath = @"E:\v-haowu\Program Files\Develop\Scope\Scope.exe";

        public static void Upload(string source, string target)
        {
            try
            {
                string argument = string.Format("copy {0} {1} -l", source, target);
                Console.WriteLine(SoftwareOperator.Execute(ScopePath, argument, 0, System.Diagnostics.ProcessWindowStyle.Normal, false));
                Console.WriteLine("Upload success: {0}", source);
            }
            catch (Exception e)
            {
                Console.WriteLine("Upload fail: {0}", source);
                Console.WriteLine(e.Message);
            }
        }
    }
}
