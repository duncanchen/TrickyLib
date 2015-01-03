using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace TrickyLib
{
    public enum Softwares
    {
        //The names are the same with the registry names.
        //You can add any software exists in the "regedit" path:
        //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths

        EXCEL,      //Office Excel
        WINWORD,    //Office Word
        MSACCESS,   //Office Access
        POWERPNT,   //Office PowerPoint
        OUTLOOK,    //Office Outlook
        INFOPATH,   //Office InfoPath
        MSPUB,      //Office Publisher
        VISIO,      //Office Visio
        IEXPLORE,   //IE
        ITUNES      //Apple ITunes
        //.........
    }

    public class SoftwareOperator
    {
        //When you do not want to use string name, then use the Enum instead
        public static bool TryGetSoftwarePath(Softwares softName, out string path)
        {
            return TryGetSoftwarePath(softName.ToString(), out path);
        }

        public static bool TryGetSoftwarePath(string softName, out string path)
        {
            string strPathResult = string.Empty;
            string strKeyName = "";     //"(Default)" key, which contains the intalled path
            object objResult = null;

            Microsoft.Win32.RegistryValueKind regValueKind;
            Microsoft.Win32.RegistryKey regKey = null;
            Microsoft.Win32.RegistryKey regSubKey = null;

            try
            {
                //Read the key
                regKey = Microsoft.Win32.Registry.LocalMachine;
                regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + softName.ToString() + ".exe", false);

                //Read the path
                objResult = regSubKey.GetValue(strKeyName);
                regValueKind = regSubKey.GetValueKind(strKeyName);

                //Set the path
                if (regValueKind == Microsoft.Win32.RegistryValueKind.String)
                {
                    strPathResult = objResult.ToString();
                }
            }
            catch (System.Security.SecurityException ex)
            {
                throw new System.Security.SecurityException("You have no right to read the registry!", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Reading registry error!", ex);
            }
            finally
            {

                if (regKey != null)
                {
                    regKey.Close();
                    regKey = null;
                }

                if (regSubKey != null)
                {
                    regSubKey.Close();
                    regSubKey = null;
                }
            }

            if (strPathResult != string.Empty)
            {
                //Found
                path = strPathResult;
                return true;
            }
            else
            {
                //Not found
                path = null;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="argument"></param>
        /// <param name="waitTime">-1 means wait until stop</param>
        /// <returns></returns>
        public static string Execute(string exePath, string argument, int waitTime, bool showWindow)
        {
            try
            {
                if (!File.Exists(exePath))
                    return exePath + " does not exists.";

                System.Diagnostics.Process Proc = new System.Diagnostics.Process();
                Proc.StartInfo = new System.Diagnostics.ProcessStartInfo();
                Proc.StartInfo.FileName = exePath;
                Proc.StartInfo.Arguments = argument;
                Proc.StartInfo.RedirectStandardOutput = !showWindow;
                Proc.StartInfo.UseShellExecute = false;
                Proc.StartInfo.WindowStyle = showWindow ? System.Diagnostics.ProcessWindowStyle.Normal : System.Diagnostics.ProcessWindowStyle.Hidden;
                Proc.StartInfo.CreateNoWindow = !showWindow;
                Proc.Start();

                if (waitTime >= 0)
                    Proc.WaitForExit(waitTime);
                else
                    Proc.WaitForExit();

                if (!showWindow)
                {
                    string output = Proc.StandardOutput.ReadToEnd().Trim();
                    string lastLine = output.Split('\n').Last().Trim();
                    string[] pAndr = lastLine.Trim("Precision/recall on test set:".ToArray()).Trim().Split('/');

                    double p = Convert.ToDouble(pAndr[0].Trim('%')) / 100;
                    double r = Convert.ToDouble(pAndr[1].Trim('%')) / 100;
                    double f = 2 * p * r / (p + r);

                    string fMeasure = "F Measure on test set: " + f.ToString("f3");
                    return output + "\r\n" + fMeasure + "\r\n\nFinished at: " + DateTime.Now.ToString();
                }
                else
                    return "Finished at: " + DateTime.Now.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }

        public static string ExecuteCommand(string command)
        {
            try
            {
                System.Diagnostics.Process Proc = new System.Diagnostics.Process();

                Proc.StartInfo.FileName = "cmd.exe";//要执行的程序名称 
                Proc.StartInfo.UseShellExecute = false;
                Proc.StartInfo.RedirectStandardInput = true;//可能接受来自调用程序的输入信息 
                Proc.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息 
                //Proc.StartInfo.CreateNoWindow = true;//不显示程序窗口 
                Proc.Start();//启动程序 //向CMD窗口发送输入信息： 
                Proc.StandardInput.AutoFlush = true;
                Proc.StandardInput.WriteLine(command);

                return Proc.StandardOutput.ReadToEnd();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Error";
            }
        }
    }
}
