using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using TrickyLib.Struct;
using TrickyLib.IO;
using TrickyLib.Service;
using TrickyLib.Extension;
using TrickyLib.Parser.Json;

namespace TrickyLib.MachineLearning
{
    [Serializable]
    public class FeatureFactory
    {
        public Type FeatureVectorType { get; set; }
        public Dictionary<string, Quad<ValueTypes, int, int, FeatureTypes>> FeatureIndexDic
        {
            get
            {
                if (FeatureVectors == null || FeatureVectors.Count <= 0)
                    return null;
                else
                    return FeatureVectors.First().FeatureIndexDic;
            }
        }
        public List<FeatureVector> FeatureVectors { get; set; }

        private bool _loadedFromFeatureFile;

        public FeatureFactory(Type featureVectorType)
        {
            FeatureVectorType = featureVectorType;
            this.FeatureVectors = new List<FeatureVector>();
        }
        public void GenerateFeatureFile(string featureFile, ValueOperations valueOperation, TrainMethods trainMethod, string[] selectedFeatureNames, string[] selectedFilterNames = null)
        {
            HashSet<string> hashSelectedFeatureNames = new HashSet<string>(selectedFeatureNames);
            HashSet<string> hashSelectedFilterNames = new HashSet<string>(selectedFilterNames);

            if (this.FeatureVectors.Count() > 0 && hashSelectedFeatureNames != null && hashSelectedFeatureNames.Count > 0)
            {
                using (StreamWriter sw = new StreamWriter(featureFile))
                {
                    sw.WriteLine("# ValueOperation = " + valueOperation);
                    foreach (var description in GetFeatureIndexDecriptions(this.FeatureIndexDic))
                        sw.WriteLine("# " + description);

                    foreach (var fv in this.FeatureVectors)
                    {
                        if (hashSelectedFilterNames.Count <= 0 || !fv.IsFiltered(hashSelectedFilterNames))
                        {
                            if (!_loadedFromFeatureFile)
                                sw.WriteLine(fv.GetFeatureLine(hashSelectedFeatureNames, valueOperation, trainMethod));
                            else
                                sw.WriteLine(fv.GetFeatureLineAfterSet(hashSelectedFeatureNames, trainMethod));
                        }
                    }
                }

                if (valueOperation == ValueOperations.Normalize)
                    FeatureFileProcessor.NormalizeFeatureFile(featureFile);
            }
            else
            {
                throw new Exception("No featureFile generated, because: FeatureVectors == null || SelectedFeatures == null");
            }
        }
        public void GenerateOriginalFeatureFile(string originalFeatureFile)
        {
            _loadedFromFeatureFile = false;
            HashSet<string> hashSelectedFeatureNames = new HashSet<string>(FeatureVectorType.GetMethods().Where(mi => mi.Name.StartsWith("Get") && mi.Name.EndsWith("Feature")).Select(f => f.Name.Substring("Get".Length, f.Name.Length - "GetFeature".Length)));

            if (this.FeatureVectors.Count() > 0 && hashSelectedFeatureNames != null && hashSelectedFeatureNames.Count > 0)
            {
                using (StreamWriter sw = new StreamWriter(originalFeatureFile))
                {
                    sw.WriteLine("# ValueOperation = None");

                    foreach (var header in GetFeatureIndexDecriptions(this.FeatureIndexDic))
                        sw.WriteLine("# " + header);

                    foreach (var fv in this.FeatureVectors)
                        sw.WriteLine(fv.GetFullLine(TrainMethods.RankSVM));
                }
            }
            else
            {
                throw new Exception("No featureFile generated, because: FeatureVectors == null || SelectedFeatures == null");
            }
        }
        public void LoadOriginalFeatureFile(string originalFeatureFile)
        {
            if (FeatureVectors != null)
                FeatureVectors.Clear();
            else
                FeatureVectors = new List<FeatureVector>();

            _loadedFromFeatureFile = true;
            var featureIndexDic = GetFeatureIndexDic(originalFeatureFile);
            using (StreamReader sr = new StreamReader(originalFeatureFile))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().TrimStart();
                    if (!line.StartsWith("#"))
                    {
                        var lqvc = FeatureFileProcessor.SplitLabelQidVectorComment(line);
                        var fv = Activator.CreateInstance(FeatureVectorType, int.Parse(lqvc[0]), int.Parse(lqvc[1].Trim("qid:".ToArray())));

                        ((FeatureVector)fv).LoadFromLine(lqvc, featureIndexDic);
                        this.FeatureVectors.Add(((FeatureVector)fv));
                    }
                }
            }

            if (this.FeatureVectors.Count > 0)
                this.FeatureVectors.First().FeatureIndexDic = featureIndexDic;
        }
        public static IEnumerable<string> GetFeatureIndexDecriptions(Dictionary<string, Quad<ValueTypes, int, int, FeatureTypes>> featureIndexDic)
        {
            int count = 0;
            foreach (var featureIndexKv in featureIndexDic.OrderBy(kv => kv.Value.Second))
            {
                string typeString = string.Empty;
                foreach (var type in Enum.GetNames(typeof(ValueTypes)))
                {
                    ValueTypes value = (ValueTypes)(Enum.Parse(typeof(ValueTypes), type));
                    if (featureIndexKv.Value.First.HasFlag(value))
                        typeString += "|" + type;
                }

                JsonObject featureIndexJsonObject = new JsonObject();
                featureIndexJsonObject.Add("ID", ++count);
                featureIndexJsonObject.Add("FeatureName", featureIndexKv.Key);
                featureIndexJsonObject.Add("ValueType", typeString.Trim('|'));
                featureIndexJsonObject.Add("StartIndex", featureIndexKv.Value.Second);
                featureIndexJsonObject.Add("EndIndex", featureIndexKv.Value.Third);
                featureIndexJsonObject.Add("FeatureType", featureIndexKv.Value.Forth.ToString());
                yield return featureIndexJsonObject.ToString();
            }
        }
        //public Dictionary<string, Triple<int, string, double>[]> GetInformationGainDic(string[] selectedFeatures)
        //{
        //    Dictionary<string, Triple<int, string, double>[]> infoGainDic = new Dictionary<string, Triple<int, string, double>[]>();
        //    foreach (var featureName in selectedFeatures)
        //        infoGainDic.Add(featureName, GetInformationGains(featureName));
        //    return infoGainDic;
        //}
        //public Triple<int, string, double>[] GetInformationGains(string featureName)
        //{
        //    var categories = this.FeatureVectors.Select(fv => fv.Label);
        //    double entropy = FeatureFileProcessor.GetEntropy(categories);

        //    List<object[]> attributesList = new List<object[]>();
        //    foreach (var fv in this.FeatureVectors)
        //        attributesList.Add(fv.GetValueCategories(featureName));

        //    var valueType = this.FeatureVectors[0].GetValueType(featureName);
        //    Triple<int, string, double>[] infoGains = new Triple<int, string, double>[attributesList[0].Length];

        //    for (int i = 0; i < attributesList[0].Length; i++)
        //    {
        //        List<object> attributes = new List<object>();

        //        foreach (var list in attributesList)
        //            attributes.Add(list[i]);


        //        infoGains[i] = new Triple<int, string, double>(i + 1, valueType.ToString(), entropy - FeatureFileProcessor.GetExpConditionEntropy(categories, attributes));
        //    }

        //    return infoGains;
        //}
        public static Dictionary<string, Quad<ValueTypes, int, int, FeatureTypes>> GetFeatureIndexDic(string originalFeatureFile)
        {
            Dictionary<string, Quad<ValueTypes, int, int, FeatureTypes>> featureIndexDic = new Dictionary<string, Quad<ValueTypes, int, int, FeatureTypes>>();
            using (StreamReader sr = new StreamReader(originalFeatureFile))
            {
                string line = null;
                while ((line = sr.ReadLine().TrimStart()) != null && line.StartsWith("#"))
                {
                    if (!line.StartsWith("# ValueOperation ="))
                    {
                        JsonObject featureIndexJsonObject = new JsonObject(line.TrimStart('#'));
                        string[] types = featureIndexJsonObject["ValueType"].ToString().Split('|');
                        ValueTypes valueType = (ValueTypes)(Enum.Parse(typeof(ValueTypes), types[0]));
                        for (int i = 1; i < types.Length; i++)
                            valueType |= (ValueTypes)(Enum.Parse(typeof(ValueTypes), types[i]));

                        featureIndexDic.Add(featureIndexJsonObject["FeatureName"].ToString(),
                            new Quad<ValueTypes, int, int, FeatureTypes>(
                                valueType,
                                int.Parse(featureIndexJsonObject["StartIndex"].ToString()),
                                int.Parse(featureIndexJsonObject["EndIndex"].ToString()),
                                (FeatureTypes)Enum.Parse(typeof(FeatureTypes), featureIndexJsonObject["FeatureType"].ToString())));
                    }
                }
            }

            if (featureIndexDic.Count <= 0)
                return null;
            else
                return featureIndexDic;
        }
    }

    [Serializable]
    public class FeatureVector
    {
        public List<Feature> Features { get; set; }
        public int Qid { get; set; }
        public object Label { get; set; }
        public bool IsInvoked { get; set; }
        public double Score { get; set; }
        //public TrainMethods TrainMethod { get; set; }
        //private ValueOperations _valueOperation = ValueOperations.None;
        //public ValueOperations ValueOperation
        //{
        //    get
        //    {
        //        return this._valueOperation;
        //    }
        //    set
        //    {
        //        if (this._valueOperation != value && this.Items != null)
        //        {
        //            this._fullVector = null;
        //            foreach (var feature in this.Items)
        //                feature.FeatureString = null;
        //        }
        //        this._valueOperation = value;
        //    }
        //}

        private Dictionary<string, Quad<ValueTypes, int, int, FeatureTypes>> _featureIndexDic;
        public Dictionary<string, Quad<ValueTypes, int, int, FeatureTypes>> FeatureIndexDic
        {
            get
            {
                if (this._featureIndexDic == null)
                    this._featureIndexDic = GetFeatureIndexDic();
                return this._featureIndexDic;
            }
            set
            {
                this._featureIndexDic = value;
            }
        }
        private string _fullVector;
        public string FullVector
        {
            get
            {
                if (this._fullVector == null)
                    InvokeFeatureMethods();

                return _fullVector;
            }
            set
            {
                this._fullVector = value;
                //DivideFeatureVector(value);
            }
        }
        private string _normalizedFullVector;
        public string NormalizedFullVector
        {
            get
            {
                if (this._normalizedFullVector == null)
                    throw new Exception("Please call NormalizeFullVector(Dic)");
                else
                    return _normalizedFullVector;
            }
            set
            {
                this._normalizedFullVector = value;
            }
        }
        public string Comment { get; set; }

        public FeatureVector(object label, int qid)
        {
            Label = label;
            Qid = qid;
            IsInvoked = false;
            Features = new List<Feature>();
        }
        public Dictionary<string, Quad<ValueTypes, int, int, FeatureTypes>> GetFeatureIndexDic()
        {
            Dictionary<string, Quad<ValueTypes, int, int, FeatureTypes>> featureIndexDic = new Dictionary<string, Quad<ValueTypes, int, int, FeatureTypes>>();
            foreach (var feature in this.Features)
            {
                Quad<ValueTypes, int, int, FeatureTypes> quad = new Quad<ValueTypes, int, int, FeatureTypes>(
                feature.ValueType,
                feature.StartIndex,
                feature.StartIndex + feature.ActualValueCount - 1,
                feature.FeatureType);
                featureIndexDic.Add(feature.FeatureName, quad);
            }

            return featureIndexDic;
        }
        //public object[] GetValueCategories(string featureName)
        //{
        //    Feature feature = null;

        //    if (this.Items.Count <= 0 || !this.Items.Exists(f => f.FeatureName == featureName))
        //        feature = InvokeFeature(featureName);
        //    else
        //        feature = this.Items.First(f => f.FeatureName == featureName);

        //    return feature.GetValueCategory();
        //}

        //Set the vector with feature names
        public string GetFeatureVectorAfterSet(HashSet<string> selectedFeatures)
        {
            string vector = string.Empty;
            foreach (var feature in this.Features)
            {
                if (selectedFeatures.Contains(feature.FeatureName))
                    vector += " " + feature.FeatureString;
            }

            return vector.RemoveMultiSpace();
        }
        public string GetFeatureVector(HashSet<string> selectedFeatures, ValueOperations valueOperation)
        {
            string vector = string.Empty;
            foreach (var feature in this.Features)
            {
                if (selectedFeatures.Contains(feature.FeatureName))
                {
                    feature.SetFeatureString(valueOperation);
                    vector += " " + feature.FeatureString;
                }
            }

            return vector.RemoveMultiSpace();
        }
        //public string GetFeatureVector(HashSet<int> selectedIndice)
        //{
        //    var items = this.FullVector.Trim().Split(' ');
        //    string vector = string.Empty;
        //    foreach (var item in items)
        //    {
        //        var kv = item.Split(' ');
        //        int key = int.Parse(kv[0]);
        //        if (selectedIndice.Contains(key))
        //            vector += " " + item;
        //    }

        //    return vector.Trim();
        //}
        public string GetFeatureLineAfterSet(HashSet<string> selectedFeatures, TrainMethods trainMethod)
        {
            string categoryStr = string.Empty;

            //Label
            if (this.Label != null)
            {
                if (trainMethod == TrainMethods.SVM)
                {
                    if (this.Label is bool)
                        categoryStr = (bool)this.Label ? "+1" : "-1";
                    else
                        categoryStr = this.Label.ToString();
                }
                else if (trainMethod == TrainMethods.RankSVM)
                {
                    if (this.Label is bool)
                        categoryStr = (bool)this.Label ? (2).ToString() : (1).ToString();
                    else
                        categoryStr = this.Label.ToString();
                }
                else if (trainMethod == TrainMethods.None)
                {
                    categoryStr = this.Label.ToString();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            //Qid
            string qidStr = string.Empty;
            if (this.Qid > 0)
                qidStr = "qid:" + this.Qid.ToString();

            string output = categoryStr + " " + qidStr;

            //Vector
            string selectedFeatureVector = GetFeatureVectorAfterSet(selectedFeatures);
            if (selectedFeatureVector != null)
                output += " " + selectedFeatureVector;

            //Comment
            if (this.Comment != null)
                output += " # " + this.Comment;

            return output.RemoveMultiSpace();
        }
        public string GetFeatureLine(HashSet<string> selectedFeatures, ValueOperations valueOperation, TrainMethods trainMethod)
        {
            string categoryStr = string.Empty;

            //Label
            if (this.Label != null)
            {
                if (trainMethod == TrainMethods.SVM)
                {
                    if (this.Label is bool)
                        categoryStr = (bool)this.Label ? "+1" : "-1";
                    else
                        categoryStr = this.Label.ToString();
                }
                else if (trainMethod == TrainMethods.RankSVM)
                {
                    if (this.Label is bool)
                        categoryStr = (bool)this.Label ? (2).ToString() : (1).ToString();
                    else
                        categoryStr = this.Label.ToString();
                }
                else if (trainMethod == TrainMethods.None)
                {
                    categoryStr = this.Label.ToString();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            //Qid
            string qidStr = string.Empty;
            if (this.Qid > 0)
                qidStr = "qid:" + this.Qid.ToString();

            string output = categoryStr + " " + qidStr;

            //Vector
            string selectedFeatureVector = GetFeatureVector(selectedFeatures, valueOperation);
            if (selectedFeatureVector != null)
                output += " " + selectedFeatureVector;

            //Comment
            if (this.Comment != null)
                output += " # " + this.Comment;

            return output.RemoveMultiSpace();
        }
        public string GetFullLine(TrainMethods trainMethod = TrainMethods.None)
        {
            string categoryStr = string.Empty;

            //Label
            if (this.Label != null)
            {
                if (trainMethod == TrainMethods.SVM)
                {
                    if (this.Label is bool)
                        categoryStr = (bool)this.Label ? "+1" : "-1";
                    else
                        categoryStr = this.Label.ToString();
                }
                else if (trainMethod == TrainMethods.RankSVM)
                {
                    if (this.Label is bool)
                        categoryStr = (bool)this.Label ? (2).ToString() : (1).ToString();
                    else
                        categoryStr = this.Label.ToString();
                }
                else if (trainMethod == TrainMethods.None)
                {
                    categoryStr = this.Label.ToString();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            //Qid
            string qidStr = string.Empty;
            if (this.Qid > 0)
                qidStr = "qid:" + this.Qid.ToString();

            string output = categoryStr + " " + qidStr;

            //Vector
            if (FullVector != null)
                output += " " + FullVector;

            //Comment
            if (this.Comment != null)
                output += " # " + this.Comment;

            return output.RemoveMultiSpace();
        }
        public string GetNormalizeFullLine(TrainMethods trainMethod = TrainMethods.None)
        {
            string categoryStr = string.Empty;

            //Label
            if (this.Label != null)
            {
                if (trainMethod == TrainMethods.SVM)
                {
                    if (this.Label is bool)
                        categoryStr = (bool)this.Label ? "+1" : "-1";
                    else
                        categoryStr = this.Label.ToString();
                }
                else if (trainMethod == TrainMethods.RankSVM)
                {
                    if (this.Label is bool)
                        categoryStr = (bool)this.Label ? (2).ToString() : (1).ToString();
                    else
                        categoryStr = this.Label.ToString();
                }
                else if (trainMethod == TrainMethods.None)
                {
                    categoryStr = this.Label.ToString();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            //Qid
            string qidStr = string.Empty;
            if (this.Qid > 0)
                qidStr = "qid:" + this.Qid.ToString();

            string output = categoryStr + " " + qidStr;

            //Vector
            if (FullVector != null)
                output += " " + NormalizedFullVector;

            //Comment
            if (this.Comment != null)
                output += " # " + this.Comment;

            return output.RemoveMultiSpace();
        }
        public void InvokeFeatureMethods()
        {
            if (this.Features == null || this.Features.Count == 0)
                foreach (var featureMethod in this.GetType().GetMethods().Where(mi => mi.Name.StartsWith("Get") && mi.Name.EndsWith("Feature")))
                    this.Features.Add((Feature)featureMethod.Invoke(this, null));

            string vector = string.Empty;
            int currentIndex = 1;

            foreach (var feature in this.Features)
            {
                feature.StartIndex = currentIndex;
                feature.SetFeatureString(ValueOperations.None);

                vector += " " + feature.FeatureString;
                currentIndex += feature.ActualValueCount;
            }

            this._fullVector = vector.RemoveMultiSpace();
        }
        public int GetDimentionCount()
        {
            return Features.Sum(f => f.ActualValueCount);
        }

        //public string AttachFeatureVectors(params FeatureVector[] featureVectors)
        //{
        //    //Initialize
        //    string attachedVector = this.FullVector;
        //    int secStartIndex = this.FeatureIndexDic.Values.Max(v => v.Third);

        //    //Attach each featureVector
        //    foreach (var fv in featureVectors)
        //    {
        //        string[] items = fv.FullVector.Split(' ');
        //        foreach (var item in items)
        //        {
        //            string[] kv = item.Split(':');
        //            int key = int.Parse(kv[0]);
        //            double value = double.Parse(kv[1]);
        //            attachedVector += " " + (key + secStartIndex) + ":" + value;
        //        }

        //        //ReAssign the start index
        //        secStartIndex += fv.FeatureIndexDic.Values.Max(v => v.Third);
        //    }

        //    return attachedVector;
        //}
        //public string AttachFeatureVectors(HashSet<string> selectedFeatures, params FeatureVector[] featureVectors)
        //{
        //    //Initialize
        //    string attachedVector = GetFeatureVector(selectedFeatures);
        //    int secStartIndex = this.FeatureIndexDic.Values.Max(v => v.Third);

        //    //Attach each featureVector
        //    foreach (var fv in featureVectors)
        //    {
        //        string[] items = fv.GetFeatureVector(selectedFeatures).Split(' ');
        //        foreach (var item in items)
        //        {
        //            string[] kv = item.Split(':');
        //            int key = int.Parse(kv[0]);
        //            double value = double.Parse(kv[1]);
        //            attachedVector += " " + (key + secStartIndex) + ":" + value;
        //        }

        //        //ReAssign the start index
        //        secStartIndex += fv.FeatureIndexDic.Values.Max(v => v.Third);
        //    }

        //    return attachedVector;
        //}
        //public string AttachFeatureVectors(HashSet<int> selectedIndice, params FeatureVector[] featureVectors)
        //{
        //    //Initialize
        //    string attachedVector = GetFeatureVector(selectedIndice);
        //    int secStartIndex = this.FeatureIndexDic.Values.Max(v => v.Third);

        //    //Attach each featureVector
        //    foreach (var fv in featureVectors)
        //    {
        //        string[] items = fv.GetFeatureVector(selectedIndice).Split(' ');
        //        foreach (var item in items)
        //        {
        //            string[] kv = item.Split(':');
        //            int key = int.Parse(kv[0]);
        //            double value = double.Parse(kv[1]);
        //            attachedVector += " " + (key + secStartIndex) + ":" + value;
        //        }

        //        //ReAssign the start index
        //        secStartIndex += fv.FeatureIndexDic.Values.Max(v => v.Third);
        //    }

        //    return attachedVector;
        //}

        //private Feature InvokeFeature(string featureName)
        //{
        //    string methodName = "Get" + featureName + "Feature";

        //    MethodInfo mi = FeatureMethods.First(m => m.Name == methodName);
        //    return (Feature)mi.Invoke(this, null);
        //}

        //public static string GetFeatureName(string methodName)
        //{
        //    if (methodName.StartsWith("Get") && methodName.EndsWith("Feature"))
        //    {
        //        int end = methodName.IndexOf("Feature");
        //        string featureName = methodName.Substring(3, end - 3);

        //        return featureName;
        //    }
        //    else
        //        return null;
        //}
        //public static string GetFilterName(string methodName)
        //{
        //    if (methodName.StartsWith("Get") && methodName.EndsWith("Filter"))
        //    {
        //        int end = methodName.IndexOf("Filter");
        //        string filterName = methodName.Substring(3, end - 3);

        //        return filterName;
        //    }
        //    else
        //        return null;
        //}
        //public ValueTypes GetValueType(string featureName)
        //{
        //    Feature feature = null;

        //    if (this.Items.Count <= 0 || !this.Items.Exists(f => f.FeatureName == featureName))
        //        feature = InvokeFeature(featureName);
        //    else
        //        feature = this.Items.First(f => f.FeatureName == featureName);

        //    return feature.ValueType;
        //}

        //public void DivideFeatureVector(string fv)
        //{
        //    DivideFeatureVector(fv, this.FeatureIndexDic);
        //}
        //public void DivideFeatureVector(string fv, Dictionary<string, Triple<ValueTypes, int, int>> featureIndexDic)
        //{
        //    this.FeatureIndexDic = featureIndexDic;
        //    var lqvc = FeatureFileProcessor.SplitLabelQidVectorComment(fv);
        //    if (string.IsNullOrEmpty(lqvc[2]))
        //        return;

        //    var items = lqvc[2].Trim().Split(' ');
        //    string tempFeatureStr = string.Empty;
        //    string lastFeatureName = string.Empty;
        //    foreach (var item in items)
        //    {
        //        var kv = item.Split(':');
        //        int key = int.Parse(kv[0]);
        //        var featureName = featureIndexDic.First(dickv => dickv.Value.Second <= key && dickv.Value.Third >= key).Key;
        //        if (lastFeatureName != featureName && lastFeatureName != string.Empty)
        //        {
        //            Feature feature = null;
        //            if (this.Items == null || this.Items.Count == 0 || !this.Items.Exists(f => f.FeatureName == lastFeatureName))
        //            {
        //                feature = new Feature(this, lastFeatureName, featureIndexDic[lastFeatureName].First);
        //                this.Items.Add(feature);
        //            }
        //            else
        //                feature = this.Items.Find(f => f.FeatureName == lastFeatureName);

        //            feature.FeatureString = tempFeatureStr.Trim();
        //            feature.StartIndex = featureIndexDic[lastFeatureName].Second;
        //            feature.ActualValueCount = featureIndexDic[lastFeatureName].Third + 1 - feature.StartIndex;
        //            tempFeatureStr = string.Empty;
        //        }
        //        lastFeatureName = featureName;
        //        tempFeatureStr += " " + item;
        //    }

        //    Feature lastFeature = null;
        //    if (this.Items == null || this.Items.Count == 0 || !this.Items.Exists(f => f.FeatureName == lastFeatureName))
        //    {
        //        lastFeature = new Feature(this, lastFeatureName, featureIndexDic[lastFeatureName].First);
        //        this.Items.Add(lastFeature);
        //    }
        //    else
        //        lastFeature = this.Items.Find(f => f.FeatureName == lastFeatureName);

        //    lastFeature.FeatureString = tempFeatureStr.Trim();
        //    lastFeature.StartIndex = featureIndexDic[lastFeatureName].Second;
        //    lastFeature.ActualValueCount = featureIndexDic[lastFeatureName].Third + 1 - lastFeature.StartIndex;
        //}
        public bool IsFiltered(HashSet<string> selectedFilterNames)
        {
            if (selectedFilterNames != null && selectedFilterNames.Count > 0)
            {
                foreach (var filterMethod in this.GetType().GetMethods().Where(mi => mi.Name.StartsWith("Get") && mi.Name.EndsWith("Filter")))
                {
                    if (selectedFilterNames.Contains(filterMethod.Name.Substring("Get".Length, filterMethod.Name.Length - "GetFilter".Length))
                        && (bool)filterMethod.Invoke(this, null))
                        return true;
                }
            }

            return false;
        }
        public virtual void LoadFromLine(string[] lqvc, Dictionary<string, Quad<ValueTypes, int, int, FeatureTypes>> featureIndexDic)
        {
            if (featureIndexDic == null)
                return;

            Comment = lqvc[3];
            Features.Clear();

            var items = lqvc[2].RemoveMultiSpace().Split(' ');
            string lastFeatureName = string.Empty;

            foreach (var item in items)
            {
                var kv = item.Split(':');
                int key = int.Parse(kv[0]);

                if (Features.Count <= 0 || featureIndexDic[lastFeatureName].Third < key)
                {
                    var featureQuad = featureIndexDic.First(dickv => dickv.Value.Second <= key && dickv.Value.Third >= key);
                    lastFeatureName = featureQuad.Key;

                    Features.Add(new Feature(featureQuad.Key, featureQuad.Value.First)
                    {
                        FeatureString = item,
                        StartIndex = featureQuad.Value.Second,
                        ActualValueCount = featureQuad.Value.Third - featureQuad.Value.Second + 1
                    });
                }
                else
                {
                    Features.Last().FeatureString += " " + item;
                }
            }
        }
        public void Normalize(Dictionary<int, Pair<double, double>> maxMinDic)
        {
            NormalizedFullVector = FeatureFileProcessor.NormalizeFeatureLine(FullVector, maxMinDic);
        }
    }

    [Serializable]
    public class Feature
    {
        public static int ShortRegions = 21;    //(0, 20)               negative: [0] = true 
        public static int IntRegions = 17;      //(0, 65536)            negative: [0] = true 
        public static int LongRegions = 13;     //(0, 1,000,000,000,000)negative: [0] = true 
        public static int RatioRegions = 21;   //(-1, 1)                negative: [0] = true 
        public static int DoubleRegions = 21;  //(0, 20)               negative: [0] = true 

        public string FeatureName { get; set; }
        public FeatureTypes FeatureType { get; set; }
        public int StartIndex { get; set; }
        public double Amplify { get; set; }
        public ValueTypes ValueType { get; set; }
        public string FeatureString { get; set; }
        public int ActualValueCount { get; set; }

        public Feature(string featureName, ValueTypes valueType, double amplify = 1)
        {
            if (featureName.StartsWith("Get"))
                featureName = featureName.Substring("Get".Length);
            if (featureName.EndsWith("Feature"))
                featureName = featureName.Substring(0, featureName.Length - "Feature".Length);

            FeatureName = featureName;
            Amplify = amplify;
            ValueType = valueType;
            ActualValueCount = 0;
        }

        public void SetFeatureString(ValueOperations valueOperation)
        {
            if (valueOperation == ValueOperations.Discrete)
                SetFeatureString_Discrete();
            else
                SetFeatureString_None();
        }

        protected virtual void SetFeatureString_None() { throw new NotImplementedException(); }

        protected virtual void SetFeatureString_Discrete() { throw new NotImplementedException(); }

        #region Discrete
        protected bool[] DiscreteShort(short value)
        {
            bool[] regions = new bool[ShortRegions];
            for (int i = 0; i < regions.Length; i++)
                regions[i] = false;

            var region = value < 0 ? 0 : (value + 1);

            if (region >= regions.Length)
                regions[regions.Length - 1] = true;
            else
                regions[region] = true;

            return regions;
        }

        protected bool[] DiscreteInt(int value)
        {
            bool[] regions = new bool[IntRegions];
            for (int i = 0; i < regions.Length; i++)
                regions[i] = false;

            int region = value <= 0 ? 0 : ((int)Math.Log(value, 2) + 1);

            if (region >= regions.Length)
                regions[regions.Length - 1] = true;
            else
                regions[region] = true;

            return regions;
        }

        protected bool[] DiscreteLong(long value)
        {
            bool[] regions = new bool[LongRegions];
            for (int i = 0; i < regions.Length; i++)
                regions[i] = false;

            int region = value < 0 ? 0 : ((int)Math.Log10(value) + 1);

            if (region >= regions.Length)
                regions[regions.Length - 1] = true;
            else
                regions[region] = true;

            return regions;
        }

        protected bool[] DiscreteRatio(double value)
        {
            bool[] regions = new bool[RatioRegions];
            for (int i = 0; i < regions.Length; i++)
                regions[i] = false;

            int region = value < -1 ? 0 : (Convert.ToInt32((value + 1) * 10) + 1);

            if (region >= regions.Length)
                regions[regions.Length - 1] = true;
            else
                regions[region] = true;

            return regions;
        }

        protected bool[] DiscreteDouble(double value)
        {
            return DiscreteShort(Convert.ToInt16(value));
        }

        protected bool[] DiscreteEnum(Enum value)
        {
            Array EnumNames = Enum.GetValues(value.GetType());
            bool[] regions = new bool[EnumNames.Length];
            for (int i = 0; i < regions.Length; i++)
            {
                if (value.HasFlag(EnumNames.GetValue(i) as Enum))
                    regions[i] = true;
                else
                    regions[i] = false;
            }

            return regions;
        }
        #endregion

        //public object[] GetValueCategory()
        //{
        //    List<object> attributes = new List<object>();
        //    foreach (var value in this.ValueList)
        //    {
        //        if (value is bool)
        //            attributes.Add(value);
        //        else if (value is long)
        //        {
        //            if ((long)value >= Convert.ToInt64(LongCategory.Large))
        //                attributes.Add(LongCategory.Large);
        //            else if ((long)value >= Convert.ToInt64(LongCategory.Middle))
        //                attributes.Add(LongCategory.Middle);
        //            else
        //                attributes.Add(LongCategory.Small);
        //        }
        //        else if (value is int)
        //            attributes.Add(value);
        //        else if (value is decimal)
        //        {
        //            if ((decimal)value >= Convert.ToInt32(DecimalCategory.Large))
        //                attributes.Add(DecimalCategory.Large);
        //            else if ((decimal)value >= Convert.ToInt32(DecimalCategory.Middle))
        //                attributes.Add(DecimalCategory.Middle);
        //            else if ((decimal)value >= Convert.ToInt32(DecimalCategory.Small))
        //                attributes.Add(DecimalCategory.Small);
        //            else
        //                attributes.Add(DecimalCategory.Negative);
        //        }
        //        else if (value is double)
        //        {
        //            if ((double)value * 10 >= Convert.ToDouble(DoubleCategory.Large))
        //                attributes.Add(DoubleCategory.Large);
        //            else if ((double)value * 10 >= Convert.ToDouble(DoubleCategory.Middle))
        //                attributes.Add(DoubleCategory.Middle);
        //            else if ((double)value * 10 >= Convert.ToDouble(DoubleCategory.Small))
        //                attributes.Add(DoubleCategory.Small);
        //            else
        //                attributes.Add(DoubleCategory.Rare);
        //        }
        //        else
        //            attributes.Add(value);
        //    }

        //    return attributes.ToArray();
        //}
    }

    public class NormalFeature : Feature
    {
        public List<object> ValueList { get; private set; }

        public NormalFeature(string featureName, ValueTypes valueType, double amplify = 1)
            : base(featureName, valueType, amplify)
        {
            FeatureType = FeatureTypes.Normal;
            ValueList = new List<object>();
        }

        protected override void SetFeatureString_None()
        {
            this.FeatureString = string.Empty;
            int index = StartIndex;

            if (ActualValueCount <= 0)
                ActualValueCount = this.ValueList.Count;

            foreach (var value in this.ValueList)
            {
                object realValue = 0;
                if (value is bool)
                    realValue = (bool)value ? this.Amplify : 0;
                else
                    realValue = Convert.ToDouble(value) * this.Amplify;

                if (realValue.ToString() != "0")
                {
                    if (this.FeatureString == string.Empty)
                        this.FeatureString += index + ":" + realValue;
                    else
                        this.FeatureString += " " + index + ":" + realValue;
                }
                index++;
            }
        }

        protected override void SetFeatureString_Discrete()
        {
            try
            {
                List<bool> actualValues = new List<bool>();

                foreach (var value in this.ValueList)
                {
                    if (value is bool)
                        actualValues.Add((bool)value);
                    else if (value is short)
                        actualValues.AddRange(DiscreteShort(Convert.ToInt16(value)));
                    else if (value is int)
                        actualValues.AddRange(DiscreteInt(Convert.ToInt32(value)));
                    else if (value is long)
                        actualValues.AddRange(DiscreteLong(Convert.ToInt64(value)));
                    else if (value is double)
                        actualValues.AddRange(DiscreteRatio(Convert.ToDouble(value)));
                    else if (value is decimal)
                        actualValues.AddRange(DiscreteDouble(Convert.ToDouble(value)));
                    else if (value is Enum)
                        actualValues.AddRange(DiscreteEnum((Enum)value));
                }

                this.FeatureString = string.Empty;
                int index = StartIndex;

                if (ActualValueCount <= 0)
                    ActualValueCount = actualValues.Count;

                foreach (var value in actualValues)
                {
                    object realValue = value ? this.Amplify : 0;

                    if (realValue.ToString() != "0")
                    {
                        if (this.FeatureString == string.Empty)
                            this.FeatureString += index + ":" + realValue.ToString();
                        else
                            this.FeatureString += " " + index + ":" + realValue.ToString();
                    }
                    index++;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void AddFeature(object value)
        {
            if ((value is short && !ValueType.HasFlag(ValueTypes.Short))
            || (value is int && !ValueType.HasFlag(ValueTypes.Int))
            || (value is long && !ValueType.HasFlag(ValueTypes.Long))
            || (value is double && !ValueType.HasFlag(ValueTypes.Ratio) && !ValueType.HasFlag(ValueTypes.Double))
            || (value is bool && !ValueType.HasFlag(ValueTypes.Bool))
            || (value is Enum && !ValueType.HasFlag(ValueTypes.Enum)))
                throw new Exception("Feature type not match");

            ValueList.Add(value);
        }
    }

    public class BagOfWordFeature : Feature
    {
        private Dictionary<int, object> ValueDic { get; set; }

        public BagOfWordFeature(string featureName, ValueTypes valueType, int actualValueCount, double amplify = 1)
            : base(featureName, valueType, amplify)
        {
            FeatureType = FeatureTypes.BagOfWord;
            ActualValueCount = actualValueCount;
        }

        protected override void SetFeatureString_None()
        {
            this.FeatureString = string.Empty;

            foreach (var kv in ValueDic.OrderBy(v => v.Key))
            {
                object realValue = 0;
                if (kv.Value is bool)
                    realValue = Convert.ToBoolean(kv.Value) ? this.Amplify : 0;
                else
                    realValue = Convert.ToDouble(kv.Value) * this.Amplify;

                if (realValue.ToString() != "0")
                    this.FeatureString += " " + (StartIndex + kv.Key).ToString() + ":" + realValue;
            }

            this.FeatureString = this.FeatureString.RemoveMultiSpace();
        }

        protected override void SetFeatureString_Discrete()
        {
            try
            {
                int indexPerValue;
                switch (ValueType)
                {
                    case ValueTypes.Bool: indexPerValue = 1; break;
                    case ValueTypes.Short: indexPerValue = ShortRegions; break;
                    case ValueTypes.Int: indexPerValue = IntRegions; break;
                    case ValueTypes.Long: indexPerValue = LongRegions; break;
                    case ValueTypes.Ratio: indexPerValue = RatioRegions; break;
                    case ValueTypes.Double: indexPerValue = DoubleRegions; break;
                    case ValueTypes.Enum: indexPerValue = Enum.GetValues(ValueDic.First().Value.GetType()).Length; break;
                    default: throw new NotImplementedException();
                }

                this.FeatureString = string.Empty;
                foreach (var kv in ValueDic.OrderBy(v => v.Key))
                {
                    int start = kv.Key * indexPerValue;
                    bool[] discreteValues;

                    switch (ValueType)
                    {
                        case ValueTypes.Bool: discreteValues = new bool[] { (bool)kv.Value }; break;
                        case ValueTypes.Short: discreteValues = DiscreteShort((short)kv.Value); break;
                        case ValueTypes.Int: discreteValues = DiscreteInt((int)kv.Value); break;
                        case ValueTypes.Long: discreteValues = DiscreteLong((long)kv.Value); break;
                        case ValueTypes.Ratio: discreteValues = DiscreteRatio((double)kv.Value); break;
                        case ValueTypes.Double: discreteValues = DiscreteDouble((double)kv.Value); break;
                        case ValueTypes.Enum: discreteValues = DiscreteEnum((Enum)kv.Value); break;
                        default: throw new NotImplementedException();
                    }

                    for (int i = 0; i < discreteValues.Length; ++i)
                        this.FeatureString += discreteValues[i] ? (" " + (StartIndex + start + i).ToString() + this.Amplify) : "";
                }

                this.FeatureString = this.FeatureString.Trim();
            }
            catch (Exception ex)
            {

            }
        }

        public void AddFeature(int index, object value)
        {
            if ((value is short && ValueType != ValueTypes.Short)
            || (value is int && ValueType != ValueTypes.Int)
            || (value is long && ValueType != ValueTypes.Long)
            || (value is double && !ValueType.HasFlag(ValueTypes.Ratio) && !ValueType.HasFlag(ValueTypes.Double))
            || (value is bool && ValueType != ValueTypes.Bool)
            || (value is Enum && ValueType != ValueTypes.Enum))
                throw new Exception("Feature type not match");

            ValueDic.Add(index, value);
        }
    }

    [Serializable]
    public enum ValueTypes
    {
        Bool = 1,
        Short = 2,
        Int = 4,
        Long = 8,
        Ratio = 16,
        Double = 32,
        Enum = 64,
        NotClear = 128
    }

    [Serializable]
    public enum FeatureTypes
    {
        Normal,
        BagOfWord
    }

    [Serializable]
    public enum ValueOperations
    {
        None,
        Normalize,
        Discrete
    }

    [Serializable]
    public enum TrainMethods
    {
        SVM,
        RankSVM,
        FastRankSvm,
        None
    }

    [Serializable]
    public enum LongCategory
    {
        Large = 10000000,
        Middle = 100000,
        Small = 0
    }

    [Serializable]
    public enum DecimalCategory
    {
        Large = 10,
        Middle = 5,
        Small = 0,
        Negative = -1
    }

    [Serializable]
    public enum DoubleCategory
    {
        Large = 7,
        Middle = 4,
        Small = 1,
        Rare = 0
    }
}
