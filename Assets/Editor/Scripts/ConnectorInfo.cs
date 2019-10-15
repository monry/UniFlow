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
        private ConnectorInfo(GameObject gameObject, IConnector connector, Type type, string name, IEnumerable<Parameter> parameters, IEnumerable<ValuePublisherInfo> valuePublishers, IEnumerable<ValueReceiverInfo> valueReceivers, IEnumerable<ComposableMessageAnnotation> composableMessageAnnotations, IEnumerable<CollectableMessageAnnotation> collectableMessageAnnotations)
        {
            GameObject = gameObject;
            Connector = connector;
            Type = type;
            Name = name;
            parameterList = parameters.ToList();
            ValuePublishers = valuePublishers;
            ValueReceivers = valueReceivers;
            ComposableMessageAnnotations = composableMessageAnnotations;
            CollectableMessageAnnotations = collectableMessageAnnotations;
        }

        public GameObject GameObject { get; set; }
        public IConnector Connector { get; set; }
        public Type Type { get; }
        public string Name { get; }
        [SerializeField] private List<Parameter> parameterList = default;
        public IEnumerable<Parameter> ParameterList => parameterList;
        public IEnumerable<ValuePublisherInfo> ValuePublishers { get; }
        public IEnumerable<ValueReceiverInfo> ValueReceivers { get; }
        public IEnumerable<ComposableMessageAnnotation> ComposableMessageAnnotations { get; }
        public IEnumerable<CollectableMessageAnnotation> CollectableMessageAnnotations { get; }

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
                type.GetFieldsRecursive(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(x => (x.GetCustomAttribute<SerializeField>() != null || x.IsPublic) && x.GetCustomAttribute<HideInInspector>() == null)
                    .Select(x => Parameter.Create(x.FieldType, x.Name, instance != default ? x.GetValue(instance) : default)),
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(x => typeof(UnityEventBase).IsAssignableFrom(x.PropertyType))
                    .Where(x => x.GetCustomAttribute<ValuePublisherAttribute>() != null)
                    .Select(ValuePublisherInfo.Create),
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(x => x.GetCustomAttribute<ValueReceiverAttribute>() != null)
                    .Select(ValueReceiverInfo.Create),
                typeof(IMessageComposable).IsAssignableFrom(type) && instance != null
                    ? ((IMessageComposable) instance).GetMessageComposableAnnotations()
                    : new ComposableMessageAnnotation[0],
                typeof(IMessageCollectable).IsAssignableFrom(type) && instance != null
                    ? ((IMessageCollectable) instance).GetMessageCollectableAnnotations()
                    : new CollectableMessageAnnotation[0]
            );
        }

        [PublicAPI]
        public static ConnectorInfo Create(
            GameObject gameObject,
            IConnector instance,
            Type type,
            string name,
            IEnumerable<Parameter> parameters,
            IEnumerable<ValuePublisherInfo> valuePublishers,
            IEnumerable<ValueReceiverInfo> valueReceivers,
            IEnumerable<ComposableMessageAnnotation> composableMessageAnnotations,
            IEnumerable<CollectableMessageAnnotation> collectableMessageAnnotations
        )
        {
            return new ConnectorInfo(gameObject, instance, type, name, parameters, valuePublishers, valueReceivers, composableMessageAnnotations, collectableMessageAnnotations);
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
            private ValuePublisherInfo(PropertyInfo propertyInfo)
            {
                var attribute = propertyInfo.GetCustomAttribute<ValuePublisherAttribute>();
                Name = string.IsNullOrEmpty(attribute.Name) ? propertyInfo.Name : attribute.Name;
                Type = GetGenericType(propertyInfo.PropertyType);
                PropertyInfo = propertyInfo;
            }

            public string Name { get; }
            public Type Type { get; }
            public PropertyInfo PropertyInfo { get; }

            public static ValuePublisherInfo Create(PropertyInfo propertyInfo)
            {
                return new ValuePublisherInfo(propertyInfo);
            }

            private static Type GetGenericType(Type type)
            {
                while (true)
                {
                    if (type == default)
                    {
                        return default;
                    }

                    if (type.IsGenericType)
                    {
                        return type.GetGenericArguments().First();
                    }

                    type = type.BaseType;
                }
            }
        }

        public class ValueReceiverInfo
        {
            private ValueReceiverInfo(PropertyInfo propertyInfo)
            {
                var attribute = propertyInfo.GetCustomAttribute<ValueReceiverAttribute>();
                Name = string.IsNullOrEmpty(attribute.Name) ? propertyInfo.Name : attribute.Name;
                Type = propertyInfo.PropertyType;
                PropertyInfo = propertyInfo;
            }

            public string Name { get; }
            public Type Type { get; }
            public PropertyInfo PropertyInfo { get; }

            public static ValueReceiverInfo Create(PropertyInfo propertyInfo)
            {
                return new ValueReceiverInfo(propertyInfo);
            }
        }
    }
}
