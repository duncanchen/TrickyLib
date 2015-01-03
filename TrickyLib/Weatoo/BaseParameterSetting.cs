using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TrickyLib.MachineLearning.Tools
{
    public abstract class BaseParameterSetting
    {
        public const string floatRegexString = "( *\\d+(\\.\\d+)? *)";

        public abstract void ChangeVisibility(MachineLearningTask task, string learner);

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
    }
}
