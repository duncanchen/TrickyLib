using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExpassistLib.Extension;


namespace ExpassistLib.IO
{
    public class FilePath
    {
        public static string GetFilePathWithoutExtenstion(string file)
        {
            return Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
        }

        public static string ChangeExtension(string file, string ext)
        {
            return GetFilePathWithoutExtenstion(file) + "." + ext.TrimStart('.');
        }

        public static string ChangeFile(string originalFile, string targetFile)
        {
            try
            {
                if (string.IsNullOrEmpty(originalFile))
                    originalFile = System.Environment.CurrentDirectory + "\\";

                return Path.Combine(Path.GetDirectoryName(originalFile), targetFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return targetFile;
            }
        }

        public static string ChangeDriver(string file, char driver)
        {
            if (file.Length <= 0)
                return string.Empty;
            else if (Path.GetFileName(file) == file)
                return file;
            else
                return driver + file.Trim().Substring(1);
        }

        public static string ChangeDriverToCurrentProgram(string file)
        {
            char driver = Application.ExecutablePath.Trim()[0];
            return ChangeDriver(file, driver);
        }

        public static string AddExtention(string file, string ext, int lastPos)
        {
            string fileName = Path.GetFileName(file);
            List<string> extentions = fileName.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Where(s => s != null).ToList();
            extentions.Insert(extentions.Count - lastPos, ext);
            string newFileName = extentions.ConnectWords(".");
            return FilePath.ChangeFile(file, newFileName);
        }

        public static int IsValidFilePath(string filePath)
        {
            filePath = filePath.ToLowerInvariant();
            if (!Regex.IsMatch(filePath, @"^[a-z]:(\\[^\\/:\*?""<>\|]+)+$", RegexOptions.IgnoreCase))
                return -1;

            if (Directory.Exists(filePath))
                return -1;

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                return 0;

            return 1;
        }

        public static int IsValidDirectoryPath(string filePath)
        {
            filePath = filePath.ToLowerInvariant();
            if (!Regex.IsMatch(filePath, @"^[a-z]:(\\[^\\/:\*?""<>\|]+)+$", RegexOptions.IgnoreCase))
                return -1;

            if (File.Exists(filePath))
                return -1;

            if (!Directory.Exists(filePath))
                return 0;

            return 1;
        }

        public static string MergeDistinctFilePathPart(params string[] files)
        {
            if (files.Length <= 1)
                throw new Exception("MergeDistinctFilePathPart has to little files(<= 1)");

            List<string> withoutExtensions = new List<string>();
            foreach (var file in files)
                withoutExtensions.Add(Path.GetFileNameWithoutExtension(file));

            int i = -1;
            bool outOfRange = false;
            bool identical = true;

            while (!outOfRange && identical)
            {
                i++;
                foreach (var file in withoutExtensions)
                {
                    if (i >= file.Length)
                    {
                        outOfRange = true;
                        break;
                    }
                }

                if (!outOfRange)
                {
                    char c = withoutExtensions[0][i];
                    foreach (var file in withoutExtensions)
                        if (file[i] != c)
                        {
                            identical = false;
                            break;
                        }
                }
            }

            List<string> distincts = new List<string>();
            foreach (var file in withoutExtensions)
            {
                if (i >= file.Length)
                    distincts.Add(string.Empty);
                else
                    distincts.Add(file.Substring(i));
            }

            if (distincts.All(f => string.IsNullOrEmpty(f)))
                return withoutExtensions[0];
            else
            {
                string distinctMerge = distincts[0];
                for (int j = 1; j < distincts.Count; j++)
                    distinctMerge += "_To_" + distincts[j];

                return distinctMerge;
            }
        }

        public static string ReverseMergeDistinctFilePathPart(params string[] files)
        {
            if (files.Length <= 1)
                throw new Exception("ReverseMergeDistinctFilePathPart has to little files(<= 1)");

            List<string> withoutExtensions = new List<string>();
            foreach (var file in files)
                withoutExtensions.Add(Path.GetFileNameWithoutExtension(file));

            int i = -1;
            bool outOfRange = false;
            bool identical = true;

            while (!outOfRange && identical)
            {
                i++;
                foreach (var file in withoutExtensions)
                {
                    if (i >= file.Length)
                    {
                        outOfRange = true;
                        break;
                    }
                }

                if (!outOfRange)
                {
                    char c = withoutExtensions[0][withoutExtensions[0].Length - i - 1];
                    foreach (var file in withoutExtensions)
                        if (file[file.Length - i - 1] != c)
                        {
                            identical = false;
                            break;
                        }
                }
            }

            List<string> distincts = new List<string>();
            foreach (var file in withoutExtensions)
            {
                if (i >= file.Length)
                    distincts.Add(string.Empty);
                else
                    distincts.Add(file.Substring(0, file.Length - i));
            }

            if (distincts.All(f => string.IsNullOrEmpty(f)))
                return withoutExtensions[0];
            else
            {
                string distinctMerge = distincts[0];
                for (int j = 1; j < distincts.Count; j++)
                    distinctMerge += "_To_" + distincts[j];

                return distinctMerge;
            }
        }
    }
}
