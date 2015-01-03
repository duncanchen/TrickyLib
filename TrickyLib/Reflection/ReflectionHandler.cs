using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using TrickyLib.IO;
using System.Reflection.Emit;

namespace TrickyLib.Reflection
{
    public class ReflectionHandler
    {
        public static IEnumerable<string> GetPropertyNames(Type type)
        {
            return type.GetProperties().Select(p => p.Name);
        }

        public static object GetProperty(object target, string propertyName)
        {
            Type type = target.GetType();
            var property = type.GetProperty(propertyName);
            if (property != null)
                return property.GetValue(target, null);
            else
                return null;
        }

        public static IEnumerable<object> GetProperties(object target)
        {
            return GetProperties(target, GetPropertyNames(target.GetType()));
        }

        public static IEnumerable<object> GetProperties(object target, IEnumerable<string> propertyNames)
        {
            Type type = target.GetType();
            List<object> properties = new List<object>();

            foreach (string name in propertyNames)
            {
                properties.Add(type.GetProperty(name).GetValue(target, null));
            }
            return properties;
        }

        public static T CreateInstanceWithProperty<T>(Dictionary<string, object> propertyDic)
        {
            T output = Activator.CreateInstance<T>();
            Type type = typeof(T);

            foreach (var kv in propertyDic)
            {
                PropertyInfo pi = type.GetProperty(kv.Key);
                Object value = Convert.ChangeType(kv.Value, pi.PropertyType);

                pi.SetValue(output, value, null);
            }

            return output;
        }

        public static void SetProperties(object target, Dictionary<string, object> propertyDic)
        {
            Type type = target.GetType();

            foreach (var kv in propertyDic)
            {
                PropertyInfo pi = type.GetProperty(kv.Key);
                if (pi != null)
                {
                    try
                    {
                        Object value = Convert.ChangeType(kv.Value, pi.PropertyType);
                        pi.SetValue(target, value, null);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        public static void SetProperty(object target, string propertyName, object property)
        {
            try
            {
                Type type = target.GetType();
                PropertyInfo pi = type.GetProperty(propertyName);

                if (pi != null)
                {
                    Object value = Convert.ChangeType(property, pi.PropertyType);
                    pi.SetValue(target, value, null);
                }
            }
            catch (Exception ex)
            { 
            
            }
        }

        public static IEnumerable<string> GetFieldNames(Type type)
        {
            return type.GetFields().Select(f => f.Name);
        }

        public static IEnumerable<object> GetFields(object target)
        {
            Type type = target.GetType();
            List<object> fields = new List<object>();

            foreach (string name in GetPropertyNames(type))
            {
                fields.Add(type.GetField(name).GetValue(target));
            }
            return fields.AsEnumerable();
        }

        public static T CreateInstanceWithField<T>(Dictionary<string, object> fieldDic)
        {
            T output = Activator.CreateInstance<T>();
            Type type = typeof(T);

            foreach (var kv in fieldDic)
            {
                FieldInfo fi = type.GetField(kv.Key);
                Object value = Convert.ChangeType(kv.Value, fi.FieldType);

                fi.SetValue(output, value);
            }

            return output;
        }

        public static void SetFields(object target, Dictionary<string, object> fieldDic)
        {
            Type type = target.GetType();

            foreach (var kv in fieldDic)
            {
                FieldInfo fi = type.GetField(kv.Key);
                Object value = Convert.ChangeType(kv.Value, fi.FieldType);

                fi.SetValue(target, value);
            }
        }

        public static MethodBase GetCurrentMethod()
        {
            System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1);
            return sf.GetMethod();
        }

        public static void SavePropertiesConfig(string configPath, object targetObj, int inheritLevel)
        {
            Type objType = targetObj.GetType();
            Type comparerType = objType.BaseType;

            for (int i = 1; i < inheritLevel; i++)
                comparerType = comparerType.BaseType;

            List<string> validPropertyNames = new List<string>();

            List<string> objPropertyNames = GetPropertyNames(objType).ToList();
            List<string> comparerPropertyNames = GetPropertyNames(comparerType).ToList();

            if (comparerPropertyNames.Count() <= 0)
                validPropertyNames = objPropertyNames.ToList();
            else
            {
                foreach (var propertyName in objPropertyNames)
                {
                    if (propertyName != comparerPropertyNames.ElementAt(0))
                        validPropertyNames.Add(propertyName);
                    else
                        break;
                }
            }

            var properties = GetProperties(targetObj, validPropertyNames);
            List<string[]> propertiesInfos = new List<string[]>();

            for (int i = 0; i < validPropertyNames.Count; i++)
                propertiesInfos.Add(new string[] { validPropertyNames[i], properties.ElementAt(i) != null ? properties.ElementAt(i).ToString() : string.Empty });

            FileWriter.PrintDoubleCollection(configPath, propertiesInfos);
        }

        public static void LoadPropertiesConfig(string configPath, object targetObj)
        {
            Dictionary<string, object> propertyDic = FileReader.ReadToDic<string, object>(configPath);
            SetProperties(targetObj, propertyDic);
        }
    }
}
