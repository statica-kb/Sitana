using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sitana.Framework
{
    [Flags]
    public enum BindingFlags
    {
        Public = 1,
        GetProperty = 2,
        Static = 4
    }

    public static class ReflectionExtensions
    {
        public static FieldInfo GetField(this Type type, string name)
        {
            return type.GetRuntimeField(name);
        }

        public static PropertyInfo GetProperty(this Type type, string name)
        {
            return type.GetRuntimeProperty(name);
        }

        public static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            return type.GetRuntimeProperties();
        }

        public static IEnumerable<FieldInfo> GetFields(this Type type)
        {
            return type.GetRuntimeFields();
        }

        public static IEnumerable<MethodInfo> GetMethods(this Type type)
        {
            return type.GetRuntimeMethods();
        }
    }
}
