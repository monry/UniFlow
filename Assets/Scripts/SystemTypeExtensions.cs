using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace UniFlow
{
    [PublicAPI]
    public static class SystemTypeExtensions
    {
        public static FieldInfo GetFieldRecursive(this Type type, string name, BindingFlags bindingFlags)
        {
            return type.GetFieldsRecursive(bindingFlags).First(x => x.Name == name);
        }

        public static IEnumerable<FieldInfo> GetFieldsRecursive(this Type type, BindingFlags bindingFlags)
        {
            var fields = type.GetFields(bindingFlags);
            if (type.BaseType != default)
            {
                fields = fields.Concat(type.BaseType.GetFieldsRecursive(bindingFlags)).ToArray();
            }

            return fields;
        }

        public static PropertyInfo GetPropertyRecursive(this Type type, string name, BindingFlags bindingFlags)
        {
            return type.GetPropertiesRecursive(bindingFlags).First(x => x.Name == name);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesRecursive(this Type type, BindingFlags bindingFlags)
        {
            var properties = type.GetProperties(bindingFlags);
            if (type.BaseType != default)
            {
                properties = properties.Concat(type.BaseType.GetPropertiesRecursive(bindingFlags)).ToArray();
            }

            return properties;
        }

        public static MethodInfo GetMethodRecursive(this Type type, string name, BindingFlags bindingFlags)
        {
            return type.GetMethodsRecursive(bindingFlags).First(x => x.Name == name);
        }

        public static IEnumerable<MethodInfo> GetMethodsRecursive(this Type type, BindingFlags bindingFlags)
        {
            var properties = type.GetMethods(bindingFlags);
            if (type.BaseType != default)
            {
                properties = properties.Concat(type.BaseType.GetMethodsRecursive(bindingFlags)).ToArray();
            }

            return properties;
        }
    }
}
