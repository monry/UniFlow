using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Editor
{
    [Serializable]
    public class ConnectorInfo
    {
        private ConnectorInfo(GameObject gameObject, IConnector connector, Type type, string name, IEnumerable<Parameter> parameters, IEnumerable<SuppliableParameter> suppliableParameters)
        {
            GameObject = gameObject;
            Connector = connector;
            Type = type;
            Name = name;
            parameterList = parameters.ToList();
            suppliableParameterList = suppliableParameters.ToList();
        }

        public GameObject GameObject { get; set; }
        public IConnector Connector { get; set; }
        public Type Type { get; }
        public string Name { get; }
        [SerializeField] private List<Parameter> parameterList = default;
        public IEnumerable<Parameter> ParameterList => parameterList;
        [SerializeField] private List<SuppliableParameter> suppliableParameterList = default;
        public IEnumerable<SuppliableParameter> SuppliableParameterList => suppliableParameterList;

        public void ApplyParameter(Parameter parameter)
        {
            if (Connector == default)
            {
                return;
            }

            GetFieldRecursive(Type, parameter.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?
                .SetValue(Connector, parameter.Value);
        }

        private static FieldInfo GetFieldRecursive(Type type, string name, BindingFlags bindingFlags)
        {
            do
            {
                var fieldInfo = type.GetField(name, bindingFlags);
                if (fieldInfo != default)
                {
                    return fieldInfo;
                }

                if (type.BaseType == default)
                {
                    break;
                }

                type = type.BaseType;
            } while (true);
            return default;
        }

        public void ApplyParameters()
        {
            if (Connector == default)
            {
                return;
            }

            ParameterList.ToList().ForEach(ApplyParameter);
        }

        [PublicAPI]
        public static ConnectorInfo Create(IConnector instance)
        {
            return Create(instance.GetType(), instance);
        }

        [PublicAPI]
        public static ConnectorInfo Create(Type type, IConnector instance = default)
        {
            return Create(
                ((Component) instance)?.gameObject,
                instance,
                type,
                type.Name,
                CollectFields(type)
                    .Where(x => (x.GetCustomAttribute<SerializeField>() != null || x.IsPublic) && x.GetCustomAttribute<SuppliableTypeAttribute>() == null && x.GetCustomAttribute<HideInInspector>() == null)
                    .Select(x => Parameter.Create(x.FieldType, x.Name, instance != default ? x.GetValue(instance) : default)),
                CollectFields(type)
                    .Where(x => (x.GetCustomAttribute<SerializeField>() != null || x.IsPublic) && x.GetCustomAttribute<SuppliableTypeAttribute>() != null && x.GetCustomAttribute<HideInInspector>() == null)
                    .Select(x => SuppliableParameter.Create(x.Name, x.GetCustomAttribute<SuppliableTypeAttribute>().Types.ToArray()))
            );
        }

        private static IEnumerable<FieldInfo> CollectFields(Type type)
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (typeof(IConnector).IsAssignableFrom(type.BaseType))
            {
                fields = fields.Concat(CollectFields(type.BaseType)).ToArray();
            }

            return fields;
        }

        [PublicAPI]
        public static ConnectorInfo Create(GameObject gameObject, IConnector instance, Type type, string name, IEnumerable<Parameter> parameters, IEnumerable<SuppliableParameter> suppliableParameters)
        {
            return new ConnectorInfo(gameObject, instance, type, name, parameters, suppliableParameters);
        }

        [Serializable]
        public class Parameter
        {
            private Parameter(Type type, string name, object value)
            {
                Type = type;
                Name = name;
                Value = value;
            }

            [SerializeField] private object value = default;

            public Type Type { get; }
            public string Name { get; }
            public object Value
            {
                get => value;
                set => this.value = value;
            }

            public static Parameter Create(Type type, string name, object value)
            {
                return new Parameter(type, name, value);
            }
        }

        public class SuppliableParameter
        {
            public SuppliableParameter(string name, IEnumerable<Type> types)
            {
                Name = name;
                Types = types;
            }

            public string Name { get; }
            public IEnumerable<Type> Types { get; }

            public static SuppliableParameter Create(string name, params Type[] types)
            {
                return new SuppliableParameter(name, types);
            }
        }
    }
}