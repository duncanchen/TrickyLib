using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace TrickyLib.MachineLearning.Tools
{
    [Serializable]
    public abstract class BaseParameterSetting
    {
        public const string floatRegexString = "( *\\d+(\\.\\d+)? *)";
        protected readonly Regex enumParaReg = new Regex("^( *\\d+(\\.\\d+)? *)(,( *\\d+(\\.\\d+)? *))*$");
        protected readonly Regex stepParaReg = new Regex("^( *\\d+(\\.\\d+)? *):( *\\d+(\\.\\d+)? *):( *\\d+(\\.\\d+)? *)$");
        protected readonly Regex gradParaReg = new Regex("^( *\\d+(\\.\\d+)? *): *:( *\\d+(\\.\\d+)? *)$");

        public virtual void ChangeVisibility(MachineLearningTask task, string learner)
        {
        }

        public void HideAllPropertyVisibility()
        {
            try
            {
                Type type = typeof(BrowsableAttribute);
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this);
                foreach (PropertyDescriptor property in props)
                {
                    FieldInfo fld = type.GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);
                    fld.SetValue(property.Attributes[type], false);
                }
            }
            catch (Exception ex)
            {
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

        public abstract IEnumerable<BaseParameterCombination> GetParameterCombinations(MachineLearningTask task, string learner);

        public abstract void ReadParameterString(string parameter);

        protected int[] GetParameterValues_Int(string valueString)
        {
            if (enumParaReg.IsMatch(valueString))
            {
                return valueString.Replace(" ", "").Split(',').Select(v => int.Parse(v)).ToArray();
            }
            else if (stepParaReg.IsMatch(valueString))
            {
                var startStepEnd = valueString.Replace(" ", "").Split(':');
                int start = int.Parse(startStepEnd[0]);
                int step = int.Parse(startStepEnd[1]);
                int end = int.Parse(startStepEnd[2]);

                List<int> values = new List<int>();
                int value = start;

                while (value <= end)
                {
                    values.Add(value);
                    value += step;
                }

                return values.ToArray();
            }
            else
            {
                throw new ArgumentException(string.Format(@"The format of ""{0}"" is error", "C"));
            }
        }

        protected double[] GetParameterValues_Double(string valueString)
        {
            if (enumParaReg.IsMatch(valueString))
            {
                return valueString.Replace(" ", "").Split(',').Select(v => double.Parse(v)).ToArray();
            }
            else if (stepParaReg.IsMatch(valueString))
            {
                var startStepEnd = valueString.Replace(" ", "").Split(':');
                decimal start = decimal.Parse(startStepEnd[0]);
                decimal step = decimal.Parse(startStepEnd[1]);
                decimal end = decimal.Parse(startStepEnd[2]);

                List<double> values = new List<double>();
                decimal value = start;

                while (value <= end)
                {
                    values.Add(Convert.ToDouble(value));
                    value = value + step;
                }

                return values.ToArray();
            }
            else if (gradParaReg.IsMatch(valueString))
            {
                var startStepEnd = valueString.Replace(" ", "").Split(':');
                double start = double.Parse(startStepEnd[0]);
                double end = double.Parse(startStepEnd[2]);

                //Initialize the array
                double[] gradeValues = new double[] { 1, 2, 5 };
                List<double> values = new List<double>();
                int minGrad = -5;
                int maxGrad = 5;
                int grade = minGrad;
                while (grade <= maxGrad)
                {
                    foreach (var gradeValue in gradeValues)
                        values.Add(gradeValue * Math.Pow(10, grade));

                    ++grade;
                }

                //Find the match values
                List<double> selectedValues = new List<double>();
                foreach (var value in values)
                    if (value >= start && value <= end)
                        selectedValues.Add(value);

                //return
                return selectedValues.ToArray();
            }
            else
            {
                throw new ArgumentException(string.Format(@"The format of ""{0}"" is error", "C"));
            }
        }
    }
}
