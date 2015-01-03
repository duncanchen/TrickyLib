using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TrickyLib.Relevance.Metric;

namespace TrickyLib.MachineLearning.Tools
{
    [Serializable]
    public class MetricSetting
    {
        #region Metrics
        [Browsable(true), Category("RankMetric")]
        public bool RWA
        {
            get
            {
                return _rwa;
            }
            set
            {
                _rwa = value;
            }
        }
        private bool _rwa = true;

        [Browsable(true), Category("RankMetric")]
        public bool NDCG
        {
            get
            {
                return _ndcg;
            }
            set
            {
                _ndcg = value;
            }
        }
        private bool _ndcg = true;

        [Browsable(true), Category("RankMetric")]
        public bool MAP
        {
            get
            {
                return _map;
            }
            set
            {
                _map = value;
            }
        }
        private bool _map = true;

        [Browsable(true), Category("RankMetric")]
        public bool RankPrecision
        {
            get
            {
                return _rankPrecision;
            }
            set
            {
                _rankPrecision = value;
            }
        }
        private bool _rankPrecision = true;

        [Browsable(true), Category("RankMetric")]
        public int TopN
        {
            get
            {
                return _topN;
            }
            set
            {
                int original = _topN;
                if (value < 1 || value > 50)
                {
                    MessageBox.Show(string.Format("ThreadCount out of range, it must between [1, {0}]", Environment.ProcessorCount));
                    _topN = original;
                }
                else
                {
                    _topN = value;
                }
            }
        }
        public int _topN = 10;

        [Browsable(true), Category("ClassifyMetric")]
        public bool Precision
        {
            get
            {
                return _precision;
            }
            set
            {
                _precision = value;
            }
        }
        private bool _precision = true;

        [Browsable(true), Category("ClassifyMetric")]
        public bool Recall
        {
            get
            {
                return _recall;
            }
            set
            {
                _recall = value;
            }
        }
        private bool _recall = true;

        [Browsable(true), Category("ClassifyMetric")]
        public bool F_Measure
        {
            get
            {
                return _f_measure;
            }
            set
            {
                _f_measure = value;
            }
        }
        private bool _f_measure = true;

        [Browsable(true), Category("ClassifyMetric")]
        public double PositiveThreshold
        {
            get
            {
                return _positiveThreshold;
            }
            set
            {
                _positiveThreshold = value;
            }
        }
        private double _positiveThreshold = 0;
        #endregion

        public virtual void ChangeVisibility(MachineLearningTask task, string learner)
        {
            Type type = typeof(CategoryAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this);
            foreach (PropertyDescriptor property in props)
            {
                bool match = false;
                if (task == MachineLearningTask.Ranking && property.Category.Contains("Rank"))
                    match = true;
                else if (task == MachineLearningTask.BinaryClassification && property.Category.Contains("Classify"))
                    match = true;

                SetPropertyVisibility(this, property.Name, match);
            }
        }

        protected void SetPropertyVisibility(object obj, string propertyName, bool visible)
        {
            try
            {
                Type type = typeof(BrowsableAttribute);
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
                AttributeCollection attrs = props[propertyName].Attributes;
                FieldInfo fld = type.GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);
                fld.SetValue(attrs[type], visible);
            }
            catch (Exception ex)
            {
            }
        }

        public bool GetWhetherMetricIsNeed(string propertyName)
        {
            try
            {
                Type type = typeof(BrowsableAttribute);
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this);
                AttributeCollection attrs = props[propertyName].Attributes;
                FieldInfo fld = type.GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);
                var isVisible = fld.GetValue(attrs[type]);
                var isNeeded = Reflection.ReflectionHandler.GetProperty(this, propertyName);

                if (isVisible != null && isNeeded != null)
                    return (bool)isVisible && (bool)isNeeded;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public TrainTestResult GetSelectedPerformace(string featureFile, string outputFile)
        {
            MetricUtility.MaxTopN = TopN;
            MetricUtility.PositiveThreshold = PositiveThreshold;
            TrainTestResult result = new TrainTestResult();

            if (GetWhetherMetricIsNeed("RWA"))
                result.AddRange((new RWA()).GetResult(featureFile, outputFile));

            if (GetWhetherMetricIsNeed("NDCG"))
                result.AddRange((new NDCG()).GetResult_TopN(featureFile, outputFile));

            if (GetWhetherMetricIsNeed("RankPrecision"))
                result.AddRange((new RankPrecision()).GetResult_TopN(featureFile, outputFile));

            if (GetWhetherMetricIsNeed("MAP"))
                result.AddRange((new MAP()).GetResult(featureFile, outputFile));

            if (GetWhetherMetricIsNeed("Precision")
                || GetWhetherMetricIsNeed("Recall")
                || GetWhetherMetricIsNeed("F_Measure"))
            {
                var prf = (new PrecisionRecallFMeasure()).GetResult(featureFile, outputFile);

                if (Precision)
                    result.Add("Precision", prf.Properties[0]);
                if (Recall)
                    result.Add("Recall", prf.Properties[1]);
                if (F_Measure)
                    result.Add("F-Measure", prf.Properties[2]);
            }

            return result;
        }
    }
}
