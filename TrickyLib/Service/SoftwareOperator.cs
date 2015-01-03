using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace TrickyLib.Service
{
    public enum Softwares
    {
        //The names are the same with the registry names.
        //You can add any software exists in the "regedit" path:
        //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths
        NOTEPAD,        //Textpad
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

        public static void OpenSoftware(Softwares software, string fileName = "")
        {
            string softwarePath = string.Empty;
            if (!TryGetSoftwarePath(software, out softwarePath))
                throw new Exception("Software [" + software.ToString() + "] not found");

            if (fileName != string.Empty && !File.Exists(fileName))
                throw new FileNotFoundException("Not found", fileName);
            else if (fileName == string.Empty)
                System.Diagnostics.Process.Start(softwarePath);
            else
                System.Diagnostics.Process.Start(softwarePath, fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="argument"></param>
        /// <param name="waitMillionSeconds">-1 means wait until stop</param>
        /// <returns></returns>
        public static string Execute(string exePath, string argument, int waitMillionSeconds, ProcessWindowStyle windowStyle, bool needReturn)
        {
            try
            {
                if (!File.Exists(exePath))
                    throw new FileNotFoundException("", exePath);

                System.Diagnostics.Process Proc = new System.Diagnostics.Process();
                Proc.StartInfo.FileName = exePath;
                Proc.StartInfo.Arguments = argument;
                Proc.StartInfo.RedirectStandardOutput = windowStyle == ProcessWindowStyle.Hidden;
                Proc.StartInfo.UseShellExecute = windowStyle != ProcessWindowStyle.Hidden;
                Proc.StartInfo.WindowStyle = windowStyle;
                Proc.StartInfo.CreateNoWindow = windowStyle == ProcessWindowStyle.Hidden;
                Proc.Start();

                if (waitMillionSeconds >= 0)
                    Proc.WaitForExit(waitMillionSeconds);
                else
                    Proc.WaitForExit();

                if (windowStyle == ProcessWindowStyle.Hidden && needReturn)
                    return Proc.StandardOutput.ReadToEnd().Trim();
                else
                    return "Window showed. Finished at: " + DateTime.Now.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="argument"></param>
        /// <param name="waitMillionSeconds">-1 means wait until stop</param>
        /// <param name="windowStyle"></param>
        /// <param name="needReturn"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public static string Execute(string exePath, string argument, int waitMillionSeconds, ProcessWindowStyle windowStyle, bool needReturn, out Process process)
        {
            try
            {
                if (!File.Exists(exePath))
                    throw new FileNotFoundException("", exePath);

                process = new System.Diagnostics.Process();
                process.StartInfo.FileName = exePath;
                process.StartInfo.Arguments = argument;
                process.StartInfo.RedirectStandardOutput = windowStyle == ProcessWindowStyle.Hidden;
                process.StartInfo.UseShellExecute = windowStyle != ProcessWindowStyle.Hidden;
                process.StartInfo.WindowStyle = windowStyle;
                process.StartInfo.CreateNoWindow = windowStyle == ProcessWindowStyle.Hidden;
                process.Start();

                if (waitMillionSeconds >= 0)
                    process.WaitForExit(waitMillionSeconds);
                else
                    process.WaitForExit();

                string outputString;
                if (windowStyle == ProcessWindowStyle.Hidden && needReturn)
                    outputString = process.StandardOutput.ReadToEnd().Trim();
                else
                    outputString = "Window showed. Finished at: " + DateTime.Now.ToString();

                process = null;
                return outputString;
            }
            catch (Exception e)
            {
                process = null;
                Console.WriteLine(e.Message);
                throw e;
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

        public static string ExecuteCommand(string command, ref Process process)
        {
            try
            {
                process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "cmd.exe";//要执行的程序名称 
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;//可能接受来自调用程序的输入信息 
                process.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息 
                //Proc.StartInfo.CreateNoWindow = true;//不显示程序窗口 
                process.Start();//启动程序 //向CMD窗口发送输入信息： 
                process.StandardInput.AutoFlush = true;
                process.StandardInput.WriteLine(command);

                return process.StandardOutput.ReadToEnd();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Error";
            }
        }

    }
}
