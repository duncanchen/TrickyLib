using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace TrickyLib
{
    public class FeatureHandler
    {
        private static ValueOperations _valueOperation = ValueOperations.None;
        public static ValueOperations ValueOperation
        {
            get
            {
                return _valueOperation;
            }
            set
            {
                _valueOperation = value;
            }
        }

        public static string SVM_LearnExePath { get; set; }
        public static string SVM_ClassifyExePath { get; set; }

        public object DatasetLabel { get; set; }
        public string TrainFeatureFile { get; set; }
        public string DevFeatureFile { get; set; }
        public string TestFeatureFile { get; set; }
        public string SVM_ModelFile { get; set; }
        public string DevOutputFile { get; set; }
        public string TestOutputFile { get; set; }
        public string SVM_Argument { get; set; }

        private TrainMethods _trainMethod;
        public TrainMethods TrainMethod
        {
            get
            {
                return _trainMethod;
            }
            set
            {
                this._trainMethod = value;
                if (value == TrainMethods.SVM || value == TrainMethods.RankSVM)
                    SVMinitial();
            }
        }

        public string[] SelectedFeatures { get; set; }
        public List<FeatureVector> Items { get; set; }

        public Type FeatureVectorType { get; set; }

        public Dictionary<string, Triple<ValueTypes, int, int>> FeatureIndexDic
        {
            get
            {
                if (this.Items == null || this.Items.Count <= 0)
                    return ReadFeatureIndexDic(this.TrainFeatureFile);
                else
                    return this.Items[0].FeatureIndexDic;
            }
        }

        public string[] FeatureNames
        {
            get
            {
                return this.FeatureVectorType.GetMethods().Where(m => m.Name.StartsWith("Get") && m.Name.EndsWith("Feature")).Select(m => GetFeatureName(m.Name)).ToArray();
            }
        }

        public FeatureHandler(Type featureVectorType)
        {
            this.FeatureVectorType = featureVectorType;
            this.Items = new List<FeatureVector>();

            SVMinitial();
        }

        public void PrintItemsToFile(string outputFeatureFile, TrainMethods tm)
        {
            if (this.Items != null && this.Items.Count > 0)
            {
                SetItemParents();
                this.TrainMethod = tm;

                if (this.SelectedFeatures == null)
                    this.SelectedFeatures = this.Items[0].FeatureNames;

                if (this.SelectedFeatures != null && this.SelectedFeatures.Length > 0)
                {
                    List<string> vectors = new List<string>();
                    string header = string.Empty;
                    int i = 0;

                    foreach (var kv in this.FeatureIndexDic)
                    {
                        string typeString = string.Empty;
                        foreach (var type in Enum.GetNames(typeof(ValueTypes)))
                        {
                            ValueTypes value = (ValueTypes)(Enum.Parse(typeof(ValueTypes), type));
                            if (kv.Value.First.HasFlag(value))
                            {
                                if (typeString == string.Empty)
                                    typeString += type;
                                else
                                    typeString += "|" + type;
                            }
                        }

                        string str = kv.Value.Second.ToString() 
                            + ((kv.Value.Second != kv.Value.Third) ? "-" + kv.Value.Third : string.Empty)
                            + " " + kv.Key + "(" + typeString + ")";

                        if (i % 5 == 0)
                        {
                            if (header != string.Empty)
                                vectors.Add(header);

                            header = "# " + str;
                        }
                        else
                            header += ", " + str;

                        i++;
                    }
                    vectors.Add(header);

                    vectors.AddRange(this.Items.Select(fv => fv.Vector).ToList());
                    Printer.PrintCollection(outputFeatureFile, vectors);
                }
            }
            else
                throw new Exception("Items cannot be null, please add FeatureVector");
        }
        public double[] GetInformationGains(string featureName)
        {
            var categories = this.Items.Select(fv => fv.Category);
            double entropy = GetEntropy(categories);

            List<object[]> attributesList = new List<object[]>();
            foreach(var fv in this.Items)
                attributesList.Add(fv.GetValueCategories(featureName));

            double[] infoGrains = new double[attributesList[0].Length];

            for (int i = 0; i < attributesList[0].Length; i++)
            {
                List<object> attributes = new List<object>();

                foreach (var list in attributesList)
                    attributes.Add(list[i]);

                infoGrains[i] = entropy - GetExpConditionEntropy(categories, attributes);
            }

            return infoGrains;
        }
        private double GetExpConditionEntropy(IEnumerable<object> categories, IEnumerable<object> attributes)
        {
            var disAttributes = attributes.Distinct();

            double expCondEntropy = 0;
            foreach (var disA in disAttributes)
            {
                List<object> selectCtgr = new List<object>();
                for (int i = 0; i < attributes.Count(); i++)
                {
                    if (attributes.ElementAt(i).Equals(disA))
                        selectCtgr.Add(categories.ElementAt(i));
                }

                double prob = Convert.ToDouble(selectCtgr.Count()) / categories.Count();
                double entropy = GetEntropy(selectCtgr);

                expCondEntropy += prob * entropy;
            }

            return expCondEntropy;
        }
        private double GetEntropy(IEnumerable<object> categories)
        {
            var distinct = categories.Distinct();

            int[] counts = new int[distinct.Count()];
            for (int i = 0; i < counts.Length; i++)
                counts[i] = categories.Count(c => c.Equals(distinct.ElementAt(i)));

            double entropy = 0;
            foreach (var count in counts)
            {
                double property = Convert.ToDouble(count) / counts.Sum();

                if (property >= 0)
                    entropy += -1 * property * Math.Log(property, 2);
            }

            return entropy;
        }

        private void SetItemParents()
        {
            foreach (var fv in this.Items)
                fv.Parent = this;
        }

        private void SVMinitial()
        {
            SVM_LearnExePath = @"E:\v-haowu\QuerySegmentation\SVMLight\svm_learn.exe";
            SVM_ClassifyExePath = @"E:\v-haowu\QuerySegmentation\SVMLight\svm_classify.exe";
            
            this.SVM_Argument = @"-c 0.0001 -j 4";

            this.TrainFeatureFile = @"E:\v-haowu\QuerySegmentation\Data\BWC\Seg\Train_feature.txt";
            this.DevFeatureFile = @"E:\v-haowu\QuerySegmentation\Data\BWC\Seg\Develop_feature.txt";
            this.TestFeatureFile = @"E:\v-haowu\QuerySegmentation\Data\BWC\Seg\Test_feature.txt";

            this.SVM_ModelFile = @"E:\v-haowu\QuerySegmentation\Data\BWC\Seg\model.txt";
            this.DevOutputFile = @"E:\v-haowu\QuerySegmentation\Data\BWC\Seg\Develop_output.txt";
            this.TestOutputFile = @"E:\v-haowu\QuerySegmentation\Data\BWC\Seg\Test_output.txt";
        }

        public static string LearnSVM(string svm_argu, string featureFile, string modelFile, bool ShowWindow)
        {
            string trainArgu = svm_argu + " " + featureFile + " " + modelFile;
            return SoftwareOperator.Execute(SVM_LearnExePath, trainArgu, -1, ShowWindow);
        }

        public static string ClassifySVM(string featureFile, string modelFile, string outputFile, bool ShowWindow)
        {
            string argu = featureFile + " " + modelFile + " " + outputFile;
            return SoftwareOperator.Execute(SVM_ClassifyExePath, argu, -1, false);
        }

        private string GetFeatureName(string methodName)
        {
            if (methodName.StartsWith("Get"))
            {
                int end = methodName.IndexOf("Feature");
                string featureName = methodName.Substring(3, end - 3);

                return featureName;
            }
            else
                return null;
        }

        public Dictionary<string, Triple<ValueTypes, int, int>> ReadFeatureIndexDic(string featureFile)
        {
            Dictionary<string, Triple<ValueTypes, int, int>> featureIndexDic = new Dictionary<string, Triple<ValueTypes, int, int>>();

            StreamReader sr = new StreamReader(featureFile);
            string line = null;

            while ((line = sr.ReadLine()) != null && line.StartsWith("#"))
            {
                string[] lineArray = line.TrimStart('#').Split(',');

                foreach (var item in lineArray)
                {
                    string[] indexName = item.Trim().Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries);

                    string[] indices = indexName[0].Split('-');
                    int start = int.Parse(indices[0]);
                    int end = -1;
                    if (indices.Length > 1)
                        end = int.Parse(indices[1]);
                    else
                        end = start;

                    string[] featureType = indexName[1].Split("()".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                    string featureName = featureType[0];

                    string[] types = featureType[1].Split('|');
                    ValueTypes type = (ValueTypes)(Enum.Parse(typeof(ValueTypes), types[0]));
                    if (types.Length > 1)
                    { 
                        for(int i = 1; i < types.Length; i++)
                            type |= (ValueTypes)(Enum.Parse(typeof(ValueTypes), types[i]));
                    }

                    featureIndexDic.Add(featureName, new Triple<ValueTypes, int, int>() { First = type, Second = start, Third = end });
                }
            }

            return featureIndexDic;
        }
    }

    public class FeatureVector
    {
        public List<Feature> Items { get; private set; }

        public FeatureHandler Parent { get; set; }
        public object Category { get; set; }
        public TrainMethods TrainMethod
        {
            get
            {
                return Parent.TrainMethod;
            }
        }
        public int Qid { get; set; }

        private MethodInfo[] _featureMethods;
        public MethodInfo[] FeatureMethods
        {
            get
            {
                if (this._featureMethods == null || this._featureMethods.Length <= 0)
                {
                    MethodInfo[] methods = this.GetType().GetMethods();
                    this._featureMethods = methods.Where(mi => mi.Name.StartsWith("Get") && mi.Name.EndsWith("Feature")).ToArray();
                }

                return _featureMethods;
            }
        }

        public string[] FeatureNames
        {
            get
            {
                return this.FeatureMethods.Select(fm => GetFeatureName(fm.Name)).ToArray();
            }
        }

        public string[] SelectedFeatures
        {
            get
            {
                return this.Parent.SelectedFeatures;
            }
        }

        private Dictionary<string, Triple<ValueTypes, int, int>> _featureIndexDic;
        public Dictionary<string, Triple<ValueTypes, int, int>> FeatureIndexDic
        {
            get
            {
                if (this.FeatureMethods.Length <= 0 || this.SelectedFeatures.Length <= 0)
                    this._featureIndexDic = null;

                else
                    SetFeatureVector();

                return this._featureIndexDic;
            }
        }

        private string _vector;
        public string Vector
        {
            get
            { 
                //if(this._vector == null || this._vector == string.Empty)
                SetFeatureVector();

                return _vector;
            }
            private set
            {
                this._vector = value;
            }
        }

        public FeatureVector(object category, int qid)
        {
            this.Category = category;
            this.Qid = qid;

            this.Items = new List<Feature>();
        }

        public object[] GetValueCategories(string featureName)
        {
            Feature feature = null;
            
            if (this.Items.Count <= 0 || !this.Items.Exists(f => f.FeatureName == featureName))
                feature = InvokeFeature(featureName);
            else
                feature = this.Items.First(f => f.FeatureName == featureName);

            return feature.GetValueCategory();
        }

        private void SetFeatureVector()
        {
            this.Items.Clear();

            foreach (string feature in this.SelectedFeatures)
                this.AddFeature(InvokeFeature(feature));

            string categoryStr = string.Empty;
            string qidStr = string.Empty;

            if (this.TrainMethod == TrainMethods.SVM)
            {
                if (this.Category is bool)
                    categoryStr = (bool)this.Category ? "+1" : "-1";
                else
                    categoryStr = this.Category.ToString();
            }

            else if (this.TrainMethod == TrainMethods.RankSVM)
            {
                if (this.Category is bool)
                    categoryStr = (bool)this.Category ? (2).ToString() : (1).ToString();
                else
                    categoryStr = this.Category.ToString();

                qidStr = " qid:" + this.Qid.ToString();
            }

            string vector = categoryStr + qidStr;
            int currentIndex = 1;

            foreach (var feature in this.Items)
            {
                feature.StartIndex = currentIndex;
                string featureString = feature.FeatureString;

                if (featureString != string.Empty)
                    vector += " " + featureString;

                currentIndex += feature.ActualValueCount;
            }

            this.Vector = vector;

            SetFeatureIndexDic();
        }

        private void SetFeatureIndexDic()
        {
            this._featureIndexDic = new Dictionary<string, Triple<ValueTypes, int, int>>();
            foreach (var feature in this.Items)
            {
                Triple<ValueTypes, int, int> triple = new Triple<ValueTypes, int, int>();

                triple.First = feature.ValueType;
                triple.Second = feature.StartIndex;
                triple.Third = feature.StartIndex + feature.ActualValueCount - 1;

                this._featureIndexDic.Add(feature.FeatureName, triple);
            }
        }

        protected string GetFeatureName(string methodName)
        {
            if (methodName.StartsWith("Get"))
            {
                int end = methodName.IndexOf("Feature");
                string featureName = methodName.Substring(3, end - 3);

                return featureName;
            }
            else
                return null;
        }

        private Feature InvokeFeature(string featureName)
        {
            string methodName = "Get" + featureName + "Feature";

            MethodInfo mi = FeatureMethods.First(m => m.Name == methodName);
            return (Feature)mi.Invoke(this, null);
        }

        private void AddFeature(Feature feature)
        {
            foreach (object value in feature.ValueList)
            {
                if (value is short && !feature.ValueType.HasFlag(ValueTypes.Short))
                    throw new Exception("Feature type not match");
                else if (value is int && !feature.ValueType.HasFlag(ValueTypes.Int))
                    throw new Exception("Feature type not match");
                else if (value is long && !feature.ValueType.HasFlag(ValueTypes.Long))
                    throw new Exception("Feature type not match");
                else if (value is double && !feature.ValueType.HasFlag(ValueTypes.Double))
                    throw new Exception("Feature type not match");
                else if (value is decimal && !feature.ValueType.HasFlag(ValueTypes.Decimal))
                    throw new Exception("Feature type not match");
                else if (value is bool && !feature.ValueType.HasFlag(ValueTypes.Bool))
                    throw new Exception("Feature type not match");
                else if(value.GetType().BaseType == typeof(Enum) && !feature.ValueType.HasFlag(ValueTypes.Enum))
                    throw new Exception("Feature type not match");
            }

            this.Items.Add(feature);
        }
    }

    public class Feature
    {
        public static double MinDouble = 0.001;
        public int ShortRegions = 8;
        public int IntRegions = 15;
        public int LongRegions = 15;
        public int DoubleRegions = 20;
        public int DecimalRegions = 20;
        public int DecimalNegShift = 1;

        public delegate void FeatureProducerFunction(List<object> paras);


        public object Target { get; set; }

        private FeatureProducerFunction FeatureFunction { get; set; }
        private List<object> Paras;

        public string FeatureName { get; set; }
        public int StartIndex { get; set; }
        public double Amplify { get; set; }
        public ValueTypes ValueType { get; set; }
        public List<object> ValueList { get; set; }

        public int ActualValueCount { get; set; }

        public Feature(string featureName, ValueTypes valueType, FeatureProducerFunction function, params object[] paraArray)
            : this(featureName, valueType, 1)
        {
            this.FeatureFunction = function;

            Paras = new List<object>(paraArray);
            Paras.Add(this);

            //SetFeature();
        }

        public Feature(string featureName, ValueTypes valueType) : this(featureName, valueType, 1)
        {
        }

        public Feature(string featureName, ValueTypes valueType, double amplify)
        {
            this.FeatureName = featureName;
            this.Amplify = amplify;
            this.ValueType = valueType;
            this.ValueList = new List<object>();
        }

        private string _featureString;
        public string FeatureString
        {
            get
            {
                if (this.ValueList.Count <= 0 && this.FeatureFunction != null)
                    SetFeature();

                //if (this.ValueList.Count <= 0)
                //    throw new ArgumentNullException("There is no value in feature: " + this.FeatureName);

                if (FeatureHandler.ValueOperation == ValueOperations.None)
                    SetFeatureString();
                else if (FeatureHandler.ValueOperation == ValueOperations.Normalize)
                    SetFeatureString_Normalize();
                else if (FeatureHandler.ValueOperation == ValueOperations.Discrete)
                    SetFeatureString_Discrete();

                return _featureString;
            }
        }

        private void SetFeature()
        {
            this.FeatureFunction(Paras);
        }

        private void SetFeatureString()
        {
            List<object> actualValues = this.ValueList;

            this._featureString = string.Empty;
            int index = StartIndex;
            this.ActualValueCount = actualValues.Count;

            foreach (var value in actualValues)
            {
                object realValue = 0;
                if (value is bool)
                    realValue = (bool)value ? this.Amplify : 0;
                else
                    realValue = Convert.ToDouble(value) * this.Amplify;

                if (realValue.ToString() != "0")
                {
                    if (this._featureString == string.Empty)
                        this._featureString += index + ":" + realValue;
                    else
                        this._featureString += " " + index + ":" + realValue;
                }
                index++;
            }
        }

        private void SetFeatureString_Normalize()
        {
            try
            {
                List<double> actualValues = new List<double>();

                foreach (var value in this.ValueList)
                {
                    if (value is bool)
                        actualValues.Add(NormalizeBool(value));
                    else if (value is short)
                        actualValues.Add(NormalizeShort(value));
                    else if (value is int)
                        actualValues.AddRange(NormalizeInt(value));
                    else if (value is long)
                        actualValues.Add(NormalizeLong(value));
                    else if (value is double)
                        actualValues.Add(NormalizeDouble(value));
                    else if (value is decimal)
                        actualValues.Add(NormalizeDecimal(value));
                    else if (value.GetType().BaseType == typeof(Enum))
                        actualValues.Add(NormalizeEnum(value));
                }

                this._featureString = string.Empty;
                int index = StartIndex;
                this.ActualValueCount = actualValues.Count;

                foreach (var value in actualValues)
                {
                    double realValue = value * this.Amplify;

                    if (realValue.ToString() != "0")
                    {
                        if (this._featureString == string.Empty)
                            this._featureString += index + ":" + realValue.ToString();
                        else
                            this._featureString += " " + index + ":" + realValue.ToString();
                    }
                    index++;
                }
            }
            catch (Exception e)
            { 
            
            }
        }

        private void SetFeatureString_Discrete()
        {
            try
            {
                List<object> actualValues = new List<object>();

                foreach (var value in this.ValueList)
                {
                    if (value is bool)
                        actualValues.Add(value);
                    else if (value is short)
                        actualValues.AddRange(DiscreteShort(value));
                    else if (value is int)
                        actualValues.AddRange(DiscreteInt(value));
                    else if (value is long)
                        actualValues.AddRange(DiscreteLong(value));
                    else if (value is double)
                        actualValues.AddRange(DiscreteDouble(value));
                    else if (value is decimal)
                        actualValues.AddRange(DiscreteDecimal(value));
                    else if (value.GetType().BaseType == typeof(Enum))
                        actualValues.AddRange(DiscreteEnum(value));
                }

                this._featureString = string.Empty;
                int index = StartIndex;
                this.ActualValueCount = actualValues.Count;

                foreach (var value in actualValues)
                {
                    object realValue = 0;
                    if (value is bool)
                        realValue = (bool)value ? this.Amplify : 0;
                    else
                        realValue = Convert.ToDouble(value) * this.Amplify;

                    if (realValue.ToString() != "0")
                    {
                        if (this._featureString == string.Empty)
                            this._featureString += index + ":" + realValue.ToString();
                        else
                            this._featureString += " " + index + ":" + realValue.ToString();
                    }
                    index++;
                }
            }
            catch (Exception e)
            {

            }
        }

        #region Normalize
        private double NormalizeShort(object value)
        {
            return Convert.ToDouble(value) / ShortRegions;
        }

        private IEnumerable<double> NormalizeInt(object value)
        {
            double[] regions = new double[IntRegions];

            int region = Convert.ToInt32(value) <= 0 ? 0 : Convert.ToInt32(value);

            if (region >= regions.Length)
                regions[regions.Length - 1] = 0.5;
            else
                regions[region] = 0.5;

            return regions.AsEnumerable();
        }

        private double NormalizeLong(object value)
        {
            if (Convert.ToInt64(value) <= 0)
                return 0;

            long logReady = NGramHandler.GetTotalCount() / Convert.ToInt64(value);
            double output = 1 / Math.Log10(logReady);

            if (Math.Abs(output) < 0.001)
                return 0;
            else
                return output;
            //return Convert.ToInt64(value) * 1000000.0 / NGramHandler.GetTotalCount();
        }

        private double NormalizeDouble(object value)
        {
            if (Math.Abs(Convert.ToDouble(value)) < 0.0001)
                return 0;
            else
                return Convert.ToDouble(value);
        }

        private double NormalizeDecimal(object value)
        {
            double output = Convert.ToDouble(value) / 10;

            if (Math.Abs(output) < 0.0001)
                return 0;
            else
                return output;
        }

        private double NormalizeBool(object value)
        {
            if (Convert.ToBoolean(value))
                return 0.5;
            else
                return 0;
        }

        private double NormalizeEnum(object value)
        {
            return Convert.ToDouble(value);
        }
        #endregion

        #region Discrete
        private object[] DiscreteShort(object value)
        {
            object[] regions = new object[ShortRegions];
            for (int i = 0; i < regions.Length; i++)
                regions[i] = false;

            int region = Convert.ToInt16(value) <= 0 ? 0 : Convert.ToInt16(value);

            if (region >= regions.Length)
                regions[regions.Length - 1] = true;
            else if (region > 0)
                regions[region] = true;

            return regions;
        }

        private object[] DiscreteInt(object value)
        {
            object[] regions = new object[IntRegions];
            for (int i = 0; i < regions.Length; i++)
                regions[i] = false;

            int region = Convert.ToInt32(value) <= 0 ? 0 : Convert.ToInt32(value);

            if (region >= regions.Length)
                regions[regions.Length - 1] = true;
            else if(region > 0)
                regions[region] = true;

            return regions;
        }

        private object[] DiscreteLong(object value)
        {
            object[] regions = new object[LongRegions];
            for (int i = 0; i < regions.Length; i++)
                regions[i] = false;

            if (Convert.ToInt64(value) > 0)
            {
                int region = (int)Math.Log10(Convert.ToInt64(value));

                if (region >= regions.Length)
                    regions[regions.Length - 1] = true;
                else if (region > 0)
                    regions[region] = true;
            }
            return regions;
        }

        private object[] DiscreteDouble(object value)
        {
            object[] regions = new object[DoubleRegions];
            for (int i = 0; i < regions.Length; i++)
                regions[i] = false;

            if (Convert.ToDouble(value) > 0)
            {
                int region = (int)(Convert.ToDouble(value) * DoubleRegions);

                if (region >= regions.Length)
                    regions[regions.Length - 1] = true;
                else if (region >= 0)
                    regions[region] = true;
            }
            return regions;
        }

        private object[] DiscreteDecimal(object value)
        {
            object[] regions = new object[DecimalRegions];
            for (int i = 0; i < regions.Length; i++)
                regions[i] = false;

            if (Convert.ToDecimal(value) <= 0)
                regions[0] = true;
            else if (Convert.ToDecimal(value) > 0)
            {
                int region = (int)(Convert.ToDecimal(value));

                if (region >= regions.Length)
                    regions[regions.Length - 1] = true;
                else if (region >= 0)
                    regions[region] = true;
            }

            return regions;
        }

        private object[] DiscreteEnum(object value)
        {
            Array EnumNames = Enum.GetValues(value.GetType());
            object[] regions = new object[EnumNames.Length];
            for (int i = 0; i < regions.Length; i++)
            {
                if (((Enum)value).HasFlag(EnumNames.GetValue(i) as Enum))
                    regions[i] = true;
                else
                    regions[i] = false;
            }

            return regions;
        }
        #endregion

        public object[] GetValueCategory()
        {
            List<object> attributes = new List<object>();
            foreach (var value in this.ValueList)
            {
                if (value is bool)
                    attributes.Add(value);
                else if (value is long)
                {
                    if ((long)value >= Convert.ToInt64(LongCategory.Large))
                        attributes.Add(LongCategory.Large);
                    else if ((long)value >= Convert.ToInt64(LongCategory.Middle))
                        attributes.Add(LongCategory.Middle);
                    else
                        attributes.Add(LongCategory.Small);
                }
                else if (value is int)
                    attributes.Add(value);
                else if (value is decimal)
                {
                    if ((decimal)value >= Convert.ToInt32(DecimalCategory.Large))
                        attributes.Add(DecimalCategory.Large);
                    else if ((decimal)value >= Convert.ToInt32(DecimalCategory.Middle))
                        attributes.Add(DecimalCategory.Middle);
                    else if ((decimal)value >= Convert.ToInt32(DecimalCategory.Small))
                        attributes.Add(DecimalCategory.Small);
                    else
                        attributes.Add(DecimalCategory.Negative);
                }
                else if (value is double)
                {
                    if ((double)value * 10 >= Convert.ToDouble(DoubleCategory.Large))
                        attributes.Add(DoubleCategory.Large);
                    else if ((double)value * 10 >= Convert.ToDouble(DoubleCategory.Middle))
                        attributes.Add(DoubleCategory.Middle);
                    else if ((double)value * 10 >= Convert.ToDouble(DoubleCategory.Small))
                        attributes.Add(DoubleCategory.Small);
                    else
                        attributes.Add(DoubleCategory.Rare);
                }
                else
                    attributes.Add(value);
            }

            return attributes.ToArray();
        }
    }

    public enum ValueTypes
    {
        Bool = 1,
        Short = 2,
        Int = 4,
        Long = 8,
        Double = 16,
        Decimal = 32,
        Enum = 64
    }

    public enum ValueOperations
    { 
        None,
        Normalize,
        Discrete
    }

    public enum TrainMethods
    { 
        SVM,
        RankSVM
    }

    public enum LongCategory
    { 
        Large = 10000000,
        Middle = 100000,
        Small = 0
    }

    public enum DecimalCategory
    { 
        Large = 10,
        Middle = 5,
        Small = 0,
        Negative = -1
    }

    public enum DoubleCategory
    { 
        Large = 7,
        Middle = 4,
        Small = 1,
        Rare = 0
    }
}
