using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrickyLib.IO;
using System.IO;
using TrickyLib.Extension;
using TrickyLib.Service;
using TrickyLib.MachineLearning;
using TrickyLib.Struct;

namespace TrickyLib.WinForm
{
    public partial class CaseStudyForm : Form
    {
        public string SourceFile
        {
            get
            {
                return this.SourceFileBrowserTextBox.FilePath;
            }
            set
            {
                this.SourceFileBrowserTextBox.FilePath = value;
            }
        }
        public string FeatureFile
        {
            get
            {
                return this.FeatureFileBrowserTextBox.FilePath;
            }
            set
            {
                this.FeatureFileBrowserTextBox.FilePath = value;
            }
        }
        public string CaseQuery
        {
            get
            {
                return this.CaseQueryTextBox.Text;
            }
            set
            {
                this.CaseQueryTextBox.Text = value;
            }
        }

        public string OutputFile1
        {
            get
            {
                return this.OutputFileBrowserTextBox1.FilePath;
            }
            set
            {
                this.OutputFileBrowserTextBox1.FilePath = value;
            }
        }
        public string OutputFile2
        {
            get
            {
                return this.OutputFileBrowserTextBox2.FilePath;
            }
            set
            {
                this.OutputFileBrowserTextBox2.FilePath = value;
            }
        }

        public string EvalFile1
        {
            get
            {
                return this.EvalFileBrowserTextBox1.FilePath;
            }
            set
            {
                this.EvalFileBrowserTextBox1.FilePath = value;
            }
        }
        public string EvalFile2
        {
            get
            {
                return this.EvalFileBrowserTextBox2.FilePath;
            }
            set
            {
                this.EvalFileBrowserTextBox2.FilePath = value;
            }
        }

        public CaseStudyForm()
        {
            InitializeComponent();
            this.OutputFileBrowserTextBox1.Filter = "Output File|*.output|All File|*.*";
            this.OutputFileBrowserTextBox2.Filter = "Output File|*.output|All File|*.*";
        }

        private void SearchCaseButton_Click(object sender, EventArgs e)
        {
            try
            {
                string caseStudyFile = FilePath.ChangeExtension(this.FeatureFile, "CaseStudy.txt");
                using (StreamWriter sw = new StreamWriter(caseStudyFile))
                {
                    foreach (var line in GetCases(this.CaseQuery, this.SourceFile, this.FeatureFile, new string[]{this.OutputFile1, this.OutputFile2}))
                        sw.WriteLine(line);
                }

                SoftwareOperator.OpenSoftware(Softwares.EXCEL, caseStudyFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected virtual List<string> GetCases(string caseQuery, string sourceFile, string featureFile, string[] outputFiles)
        {
            throw new NotImplementedException();
        }

        private void OnlyTTestButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(this.EvalFile1))
                    throw new Exception("File not found: " + this.EvalFile1);
                if (!File.Exists(this.EvalFile2))
                    throw new Exception("File not found: " + this.EvalFile2);

                this.ConsoleTextBox.Text = SvmLight.EvalTTest_MAP_NDCG(this.EvalFile1, this.EvalFile2);
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
        }

        private void SignTestButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(this.EvalFile1))
                    throw new Exception("File not found: " + this.EvalFile1);
                if (!File.Exists(this.EvalFile2))
                    throw new Exception("File not found: " + this.EvalFile2);

                FeatureFileProcessor.SignTestEvalScore(this.EvalFile1, this.EvalFile2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CaseStudyButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(this.EvalFile1))
                    throw new Exception("File not found: " + this.EvalFile1);
                if (!File.Exists(this.EvalFile2))
                    throw new Exception("File not found: " + this.EvalFile2);

                string lastOneRow1 = FileReader.ReadLastRow(this.EvalFile1, 1, ItemOperator.RemoveEmptyItem);
                string lastOneRow2 = FileReader.ReadLastRow(this.EvalFile2, 1, ItemOperator.RemoveEmptyItem);

                int qidCount1 = int.Parse(lastOneRow1.Split(':')[0].Split(' ').Last().Substring("query".Length)) + 1;
                int qidCount2 = int.Parse(lastOneRow2.Split(':')[0].Split(' ').Last().Substring("query".Length)) + 1;

                if (qidCount1 != qidCount2)
                    throw new Exception("Different Qid Count: " + qidCount1 + " != " + qidCount2);

                string[] files = new string[] { this.EvalFile1, this.EvalFile2 };
                Dictionary<int, Dictionary<string, double>>[] evalScoreDics = new Dictionary<int, Dictionary<string, double>>[files.Length];

                for (int index = 0; index < files.Length; index++)
                {
                    evalScoreDics[index] = new Dictionary<int, Dictionary<string, double>>();
                    using (StreamReader sr = new StreamReader(files[index]))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();

                            if (line.Contains("of query"))
                            {
                                string[] kv = line.Split(':');
                                string header = kv[0];
                                string value = kv[1].Trim();

                                string[] headerItems = header.Split(' ');
                                string[] valueItems = value.Split('\t');

                                string headerKey = headerItems[0];
                                int qid = int.Parse(headerItems.Last().Substring("query".Length));

                                for (int i = 0; i < valueItems.Length; i++)
                                {
                                    string key = headerKey + (i + 1);
                                    if (!evalScoreDics[index].ContainsKey(qid))
                                        evalScoreDics[index].Add(qid, new Dictionary<string, double>());

                                    if (!evalScoreDics[index][qid].ContainsKey(key))
                                        evalScoreDics[index][qid].Add(key, double.Parse(valueItems[i]));
                                }
                            }
                        }
                    }
                }

                List<string> Queries = GetCaseStudyComparers(this.FeatureFile);
                if (Queries.Count != evalScoreDics[0].Count)
                    throw new Exception("CaseStudyComparers.Count != evalScoreDics[0].Count");

                string caseStudyFile = FilePath.ChangeFile(this.EvalFile1, FilePath.MergeDistinctFilePathPart(this.EvalFile1, this.EvalFile2) + ".CaseStudy.txt");
                using (StreamWriter sw = new StreamWriter(caseStudyFile))
                {
                    HashSet<string> keys = new HashSet<string>(evalScoreDics[0].Select(kv => kv.Value.Keys.ToArray()).ToTension().Distinct());
                    string header = "Queries";

                    foreach (var key in keys)
                    {
                        header += "\t" + key + "_1"
                                + "\t" + key + "_2"
                                + "\t" + key + "_1-2"
                                + "\t" + key + "_1%"
                                + "\t" + key + "_2%";
                    }
                    sw.WriteLine(header);

                    for (int i = 0; i < Queries.Count; i++)
                    {
                        string line = "\'" + Queries[i];
                        foreach (var key in keys)
                        {
                            if (evalScoreDics[0][i].ContainsKey(key))
                            {
                                double eval1 = evalScoreDics[0][i][key];
                                double eval2 = evalScoreDics[1][i][key];
                                double minus = eval1 - eval2;

                                double per1 = minus == 0 ? 0 : 1;
                                double per2 = minus == 0 ? 0 : 1;

                                if (eval1 != 0)
                                    per1 = Math.Sign(-minus) * Math.Abs(100 * minus / eval1);
                                if (eval2 != 0)
                                    per2 = Math.Sign(minus) * Math.Abs(100 * minus / eval2);

                                line += "\t" + eval1 + "\t" + eval2 + "\t" + minus + "\t" + per1 + "\t" + per2;
                            }
                            else
                                line += "\t\t\t\t\t";
                        }

                        sw.WriteLine(line);
                    }
                }

                SoftwareOperator.OpenSoftware(Softwares.EXCEL, caseStudyFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected virtual List<string> GetCaseStudyComparers(string featureFile)
        {
            throw new NotImplementedException();
        }

        private void OpenCasesButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(this.EvalFile1))
                    throw new Exception("File not found: " + this.EvalFile1);
                if (!File.Exists(this.EvalFile2))
                    throw new Exception("File not found: " + this.EvalFile2);

                string lastOneRow1 = FileReader.ReadLastRow(this.EvalFile1, 1, ItemOperator.RemoveEmptyItem);
                string lastOneRow2 = FileReader.ReadLastRow(this.EvalFile2, 1, ItemOperator.RemoveEmptyItem);

                int qidCount1 = int.Parse(lastOneRow1.Split(':')[0].Split(' ').Last().Substring("query".Length)) + 1;
                int qidCount2 = int.Parse(lastOneRow2.Split(':')[0].Split(' ').Last().Substring("query".Length)) + 1;

                if (qidCount1 != qidCount2)
                    throw new Exception("Different Qid Count: " + qidCount1 + " != " + qidCount2);

                string[] files = new string[] { this.EvalFile1, this.EvalFile2 };
                Dictionary<int, Dictionary<string, double>>[] evalScoreDics = new Dictionary<int, Dictionary<string, double>>[files.Length];

                for (int index = 0; index < files.Length; index++)
                {
                    evalScoreDics[index] = new Dictionary<int, Dictionary<string, double>>();
                    using (StreamReader sr = new StreamReader(files[index]))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();

                            if (line.Contains("of query"))
                            {
                                string[] kv = line.Split(':');
                                string header = kv[0];
                                string value = kv[1].Trim();

                                string[] headerItems = header.Split(' ');
                                string[] valueItems = value.Split('\t');

                                string headerKey = headerItems[0];
                                int qid = int.Parse(headerItems.Last().Substring("query".Length));

                                for (int i = 0; i < valueItems.Length; i++)
                                {
                                    string key = headerKey + (i + 1);
                                    if (!evalScoreDics[index].ContainsKey(qid))
                                        evalScoreDics[index].Add(qid, new Dictionary<string, double>());

                                    if (!evalScoreDics[index][qid].ContainsKey(key))
                                        evalScoreDics[index][qid].Add(key, double.Parse(valueItems[i]));
                                }
                            }
                        }
                    }
                }

                List<string> queries = GetCaseStudyComparers(this.FeatureFile);
                if (queries.Count != evalScoreDics[0].Count)
                    throw new Exception("CaseStudyComparers.Count != evalScoreDics[0].Count");

                HashSet<string> keys = new HashSet<string>(evalScoreDics[0].Select(kv => kv.Value.Keys.ToArray()).ToTension().Distinct());

                List<string> casesPropertyNames = new List<string>();
                casesPropertyNames.Add("Comparers");
                casesPropertyNames.AddRange(keys);

                List<Type> casesPropertyTypes = new List<Type>();
                casesPropertyTypes.Add(typeof(string));
                for (int i = 1; i < casesPropertyNames.Count; i++)
                    casesPropertyTypes.Add(typeof(double));

                List<List<object>> casesProperties = new List<List<object>>();
                for (int i = 0; i < queries.Count; i++)
                {
                    casesProperties.Add(new List<object>());
                    casesProperties.Last().Add(queries[i]);

                    foreach (var key in keys)
                    {
                        if (evalScoreDics[0][i].ContainsKey(key))
                        {
                            double eval1 = evalScoreDics[0][i][key];
                            double eval2 = evalScoreDics[1][i][key];
                            double minus = eval1 - eval2;

                            double per1 = minus == 0 ? 0 : 1;
                            double per2 = minus == 0 ? 0 : 1;

                            if (eval1 != 0)
                                per1 = Math.Sign(-minus) * Math.Abs(100 * minus / eval1);
                            if (eval2 != 0)
                                per2 = Math.Sign(minus) * Math.Abs(100 * minus / eval2);

                            casesProperties.Last().Add(eval1);
                            casesProperties.Last().Add(eval2);
                            casesProperties.Last().Add(minus);
                            casesProperties.Last().Add(per1);
                            casesProperties.Last().Add(per2);
                        }
                        else
                        {
                            casesProperties.Last().Add(Convert.ToDouble(0));
                            casesProperties.Last().Add(Convert.ToDouble(0));
                            casesProperties.Last().Add(Convert.ToDouble(0));
                            casesProperties.Last().Add(Convert.ToDouble(0));
                            casesProperties.Last().Add(Convert.ToDouble(0));
                        }
                    }
                }

                List<string> additionalFiles = new List<string>();
                additionalFiles.Add(this.FeatureFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ProduceEvalFileButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(this.OutputFile1))
                this.EvalFile1 = SvmLight.EvalScore_MAP_NDCG(this.FeatureFile, this.OutputFile1, false);

            if (File.Exists(this.OutputFile2))
                this.EvalFile2 = SvmLight.EvalScore_MAP_NDCG(this.FeatureFile, this.OutputFile2, false);
        }
    }
}