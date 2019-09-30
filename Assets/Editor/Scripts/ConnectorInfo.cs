using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Editor
{
    [Serializable]
    public class ConnectorInfo
    {
        private ConnectorInfo(GameObject gameObject, IConnector connector, Type type, string name, IEnumerable<Parameter> parameters, IEnumerable<ValuePublisherInfo> valuePublishers, IEnumerable<ValueReceiverInfo> valueReceivers)
        {
            GameObject = gameObject;
            Connector = connector;
            Type = type;
            Name = name;
            parameterList = parameters.ToList();
            ValuePublishers = valuePublishers;
            ValueReceivers = valueReceivers;
        }

        public GameObject GameObject { get; set; }
        public IConnector Connector { get; set; }
        public Type Type { get; }
        public string Name { get; }
        [SerializeField] private List<Parameter> parameterList = default;
        public IEnumerable<Parameter> ParameterList => parameterList;
        public IEnumerable<ValuePublisherInfo> ValuePublishers { get; }
        public IEnumerable<ValueReceiverInfo> ValueReceivers { get; }

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
        public static ConnectorInfo Create(Type type, IConnector instance)
        {
            return Create(
                ((Component) instance)?.gameObject,
                instance,
                type,
                type.Name,
                CollectFields(type)
                    .Where(x => (x.GetCustomAttribute<SerializeField>() != null || x.IsPublic) && x.GetCustomAttribute<HideInInspector>() == null)
                    .Select(x => Parameter.Create(x.FieldType, x.Name, instance != default ? x.GetValue(instance) : default)),
                CollectProperties(type)
                    .Where(x => typeof(UnityEventBase).IsAssignableFrom(x.PropertyType))
                    .Where(x => x.GetCustomAttribute<ValuePublisherAttribute>() != null)
                    .Select(x => ValuePublisherInfo.Create(x, instance)),
                CollectProperties(type)
                    .Where(x => x.GetCustomAttribute<ValueReceiverAttribute>() != null)
                    .Select(x => ValueReceiverInfo.Create(x, instance))
            );
        }

        [PublicAPI]
        public static ConnectorInfo Create(GameObject gameObject, IConnector instance, Type type, string name, IEnumerable<Parameter> parameters, IEnumerable<ValuePublisherInfo> valuePublishers, IEnumerable<ValueReceiverInfo> valueReceivers)
        {
            return new ConnectorInfo(gameObject, instance, type, name, parameters, valuePublishers, valueReceivers);
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

        private static IEnumerable<PropertyInfo> CollectProperties(Type type)
        {
            var fields = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (typeof(IConnector).IsAssignableFrom(type.BaseType))
            {
                fields = fields.Concat(CollectProperties(type.BaseType)).ToArray();
            }

            return fields;
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

        public class ValuePublisherInfo
        {
            private ValuePublisherInfo(PropertyInfo propertyInfo, object instance)
            {
                var attribute = propertyInfo.GetCustomAttribute<ValuePublisherAttribute>();
                Name = attribute.Name;
                Type = attribute.Type;
                Instance = instance;
                PropertyInfo = propertyInfo;
            }

            public string Name { get; }
            public ValueInjectionType Type { get; }
            public object Instance { get; }
            public PropertyInfo PropertyInfo { get; }

            public static ValuePublisherInfo Create(PropertyInfo propertyInfo, object instance)
            {
                return new ValuePublisherInfo(propertyInfo, instance);
            }
        }

        public class ValueReceiverInfo
        {
            private ValueReceiverInfo(PropertyInfo propertyInfo, object instance)
            {
                var attribute = propertyInfo.GetCustomAttribute<ValueReceiverAttribute>();
                Name = attribute.Name;
                Type = attribute.Type;
                Instance = instance;
                PropertyInfo = propertyInfo;
            }

            public string Name { get; }
            public ValueInjectionType Type { get; }
            public object Instance { get; }
            public PropertyInfo PropertyInfo { get; }

            public static ValueReceiverInfo Create(PropertyInfo propertyInfo, object instance)
            {
                return new ValueReceiverInfo(propertyInfo, instance);
            }
        }
    }
}
