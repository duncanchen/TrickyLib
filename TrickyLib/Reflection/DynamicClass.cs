using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace TrickyLib.Reflection
{
    public class DynamicClass
    {
        /// <summary>
        /// 属性类
        /// </summary>

        /// <summary>
        /// 动态创建类
        /// </summary>
        /// <param name="list">属性类集合</param>
        /// <returns>创建出来的类</returns>
        public static Type CreateClass(List<DynamicDetail> list)
        {
            TypeBuilder tb = GetTypeBuilder(list.GetHashCode());
            ConstructorBuilder constructor =
                        tb.DefineDefaultConstructor(
                                    MethodAttributes.Public |
                                    MethodAttributes.SpecialName |
                                    MethodAttributes.RTSpecialName);
            foreach (var item in list)
            {
                CreateProperty(tb, item.PropertyName, item.PropertyType);
            }
            return tb.CreateType();
        }
        private static TypeBuilder GetTypeBuilder(int code)
        {
            AssemblyName an = new AssemblyName("TempAssembly" + code);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType("TempType" + code
                                , TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout
                                , typeof(object));
            return tb;
        }
        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, System.Reflection.PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr =
                tb.DefineMethod("get_" + propertyName,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig,
                    propertyType, Type.EmptyTypes);
            ILGenerator getIL = getPropMthdBldr.GetILGenerator();
            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getIL.Emit(OpCodes.Ret);
            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new Type[] { propertyType });
            ILGenerator setIL = setPropMthdBldr.GetILGenerator();
            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, fieldBuilder);
            setIL.Emit(OpCodes.Ret);
            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
        public static object CreateInstance(IEnumerable<string> propertyNames, IEnumerable<Type> propertyTypes, IEnumerable<object> properties)
        {
            var propertyList = new List<DynamicDetail>();
            for (int i = 0; i < propertyNames.Count(); i++)
                propertyList.Add(new DynamicDetail(propertyNames.ElementAt(i), propertyTypes.ElementAt(i)));

            var type = DynamicClass.CreateClass(propertyList);
            var instance = Activator.CreateInstance(type);

            for (int i = 0; i < propertyNames.Count(); i++)
            {
                PropertyInfo propertyInfo = type.GetProperty(propertyNames.ElementAt(i));
                propertyInfo.SetValue(instance, Convert.ChangeType(properties.ElementAt(i), propertyTypes.ElementAt(i)), null);
            }

            return instance;
        }
        public static List<object> CreateInstances(IEnumerable<string> propertyNames, IEnumerable<Type> propertyTypes, IEnumerable<IEnumerable<object>> propertiesList)
        {
            var propertyList = new List<DynamicDetail>();
            for (int i = 0; i < propertyNames.Count(); i++)
                propertyList.Add(new DynamicDetail(propertyNames.ElementAt(i), propertyTypes.ElementAt(i)));

            var type = DynamicClass.CreateClass(propertyList);
            List<object> instances = new List<object>();

            foreach (var properties in propertiesList)
            {
                var instance = Activator.CreateInstance(type);
                for (int i = 0; i < propertyNames.Count(); i++)
                {
                    PropertyInfo propertyInfo = type.GetProperty(propertyNames.ElementAt(i));
                    propertyInfo.SetValue(instance, Convert.ChangeType(properties.ElementAt(i), propertyTypes.ElementAt(i)), null);
                }

                instances.Add(instance);
            }

            return instances;
        }
    }

    public class DynamicDetail
    {
        /// <summary>
        /// 属性数据类型
        /// </summary>
        public Type PropertyType { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }

        public DynamicDetail(string propertyName, Type propertyType)
        {
            this.PropertyName = propertyName;
            this.PropertyType = propertyType;
        }

        public DynamicDetail() { }
    }
}
