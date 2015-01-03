using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TrickyLib.IO;
using System.Text.RegularExpressions;

namespace TrickyLib.Parser
{
    public class CodeParser
    {
        /// <summary>
        /// Fix the disassembler dir for the subfolder and files in it.
        /// </summary>
        /// <param name="dirPath">Path of the directory</param>
        public static void FixReflectorDisassemberDirectory(string dirPath)
        {
            DirectoryInfo dir = Directory.CreateDirectory(dirPath);
            foreach (var file in dir.GetFiles())
                FixReflectorDisassemberFile(file.FullName);

            foreach (var subDir in dir.GetDirectories())
                FixReflectorDisassemberDirectory(subDir.FullName);
        }

        /// <summary>
        /// Fix the disassembler file
        /// </summary>
        /// <param name="filePath"></param>
        public static void FixReflectorDisassemberFile(string filePath)
        {
            //Regex for "this._xXXXXXEvent != null" or "this._xXXXXXEvent(this, e)"
            Regex ThisRegex = new Regex(@"this\.\w+(?= != null|\((this|sender), .+\))");

            //Regex for functions
            Regex FunctionDeclarationRegex = new Regex(@"(?=.*)\w+(?=\(.*\))");

            //Regex for using(IEnumerator xxxxx)
            Regex UsingEnuRegex = new Regex(@"using \(IEnumerator \w+ = .*\)");

            //only do ".cs" file
            if (Path.GetExtension(filePath) != ".cs")
                return;

            //memebers recorder
            Dictionary<string, string> members = new Dictionary<string, string>();

            //Functions recorder
            HashSet<string> functions = new HashSet<string>();

            //Record the members and functions
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim(';', ' ');
                    if ((line.StartsWith("public ") || line.StartsWith("private ") || line.StartsWith("protected ") || line.StartsWith("internal ")))
                    {
                        if (!line.Contains("("))
                        {
                            string eventName = line.Split(new string[] { " = ", " : ", " { " }, StringSplitOptions.None)[0].Trim().Split(' ').Last();

                            if (!members.ContainsKey(eventName.ToLower()))
                                members.Add(eventName.ToLower(), eventName);
                        }
                        else if (line.EndsWith(")"))
                        {
                            if (FunctionDeclarationRegex.IsMatch(line))
                            {
                                string functionName = FunctionDeclarationRegex.Match(line).Value;
                                if (!functions.Contains(functionName))
                                    functions.Add(functionName);
                            }
                        }
                    }
                }
            }

            //Create a temp file
            string tempFile = FilePath.ChangeExtension(filePath, "temp");
            using (StreamReader sr = new StreamReader(filePath))
            using (StreamWriter sw = new StreamWriter(tempFile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (UsingEnuRegex.IsMatch(line))
                    {
                        string usingEnuString = UsingEnuRegex.Match(line).Value;
                        string replacement = usingEnuString.Substring(7, usingEnuString.Length - 8);
                        line = line.Replace(usingEnuString, replacement) + (line.EndsWith(";") ? "" : ";");
                    }
                    if (ThisRegex.IsMatch(line))
                    {
                        string eventString = ThisRegex.Match(line).Value;
                        string replacement = "this." + GetEventReplacement(eventString, members, functions);

                        if (eventString != replacement)
                            line = line.Replace(eventString, replacement);
                    }

                    sw.WriteLine(line);
                }
            }

            File.Delete(filePath);
            FileInfo tempInfo = new FileInfo(tempFile);
            tempInfo.MoveTo(filePath);
        }

        /// <summary>
        /// Given an possible eventString, output the correct replacement
        /// </summary>
        /// <param name="eventString"></param>
        /// <param name="members">The members in the file</param>
        /// <param name="functions">The function names in the file</param>
        /// <returns></returns>
        public static string GetEventReplacement(string eventString, Dictionary<string, string> members, HashSet<string> functions)
        {
            string trimThis = eventString.Substring(5);

            //Exists in function, then no change
            if (functions.Contains(trimThis))
                return trimThis;

            //Exists in events
            if (members.ContainsKey(trimThis.ToLower()))
                return members[trimThis.ToLower()];

            //Do with the (this._onXXX) situation
            if (trimThis.StartsWith("_on"))
            {
                string removeOn = trimThis.Substring(3);

                //contained in memebers
                if (members.ContainsKey(removeOn.ToLower()))
                    return members[removeOn.ToLower()];

                if (removeOn.EndsWith("Event"))
                {
                    //remove "Event"
                    string removeOnRemoveEvent = removeOn.Substring(0, removeOn.Length - 5);

                    if (members.ContainsKey(removeOnRemoveEvent.ToLower()))
                        return members[removeOnRemoveEvent.ToLower()];

                    return removeOnRemoveEvent;
                }

                return removeOn;
            }
            //Do with the (this._xXX) situation
            else if (trimThis.StartsWith("_"))
            {
                //Make "this_xXX" to "this.XXX"
                string removeUnderLine = trimThis[1].ToString().ToUpper() + trimThis.Substring(2);

                //contained in member
                if (members.ContainsKey(removeUnderLine.ToLower()))
                    return members[removeUnderLine.ToLower()];

                //End with "Event"
                if (removeUnderLine.EndsWith("Event"))
                {
                    //Remove "Event"
                    string removeOnRemoveEvent = removeUnderLine.Substring(0, removeUnderLine.Length - 5);

                    if (members.ContainsKey(removeOnRemoveEvent.ToLower()))
                        return members[removeOnRemoveEvent.ToLower()];

                    return removeOnRemoveEvent;
                }

                return removeUnderLine;
            }
            //Do with the (this.XXX) situation
            else
            {
                if (members.ContainsKey(trimThis.ToLower()))
                    return members[trimThis.ToLower()];

                return trimThis;
            }
        }
    }
}
