using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace UniFlow.Editor
{
    [Serializable]
    public class ConnectorInfo
    {
        private ConnectorInfo(
            GameObject gameObject,
            IConnector connector,
            Type type,
            string name,
            IEnumerable<Parameter> parameters,
            IEnumerable<IComposableMessageAnnotation> composableMessageAnnotations,
            IEnumerable<ICollectableMessageAnnotation> collectableMessageAnnotations
        )
        {
            GameObject = gameObject;
            Connector = connector;
            Type = type;
            Name = name;
            parameterList = parameters.ToList();
            ComposableMessageAnnotations = composableMessageAnnotations;
            CollectableMessageAnnotations = collectableMessageAnnotations;
        }

        public GameObject GameObject { get; set; }
        public IConnector Connector { get; set; }
        public Type Type { get; }
        public string Name { get; }
        [SerializeField] private List<Parameter> parameterList = default;
        public IEnumerable<Parameter> ParameterList => parameterList;
        public IEnumerable<IComposableMessageAnnotation> ComposableMessageAnnotations { get; private set; }
        public IEnumerable<ICollectableMessageAnnotation> CollectableMessageAnnotations { get; private set; }

        public void ApplyParameter(Parameter parameter)
        {
            if (Connector == default)
            {
                return;
            }

            GetFieldRecursive(Type, parameter.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?
                .SetValue(Connector, parameter.Value);
        }

        public void ReCalculateAnnotations()
        {
            if (Connector == default)
            {
                return;
            }

            ComposableMessageAnnotations = typeof(IMessageComposable).IsAssignableFrom(Type)
                ? ((IMessageComposable) Connector).GetMessageComposableAnnotations()
                : new IComposableMessageAnnotation[0];
            CollectableMessageAnnotations = typeof(IMessageCollectable).IsAssignableFrom(Type)
                ? ((IMessageCollectable) Connector).GetMessageCollectableAnnotations()
                : new ICollectableMessageAnnotation[0];
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
                typeof(IMessageComposable).IsAssignableFrom(type) && instance != null
                    ? ((IMessageComposable) instance).GetMessageComposableAnnotations()
                    : new IComposableMessageAnnotation[0],
                typeof(IMessageCollectable).IsAssignableFrom(type) && instance != null
                    ? ((IMessageCollectable) instance).GetMessageCollectableAnnotations()
                    : new ICollectableMessageAnnotation[0]
            );
        }

        [PublicAPI]
        public static ConnectorInfo Create(
            GameObject gameObject,
            IConnector instance,
            Type type,
            string name,
            IEnumerable<Parameter> parameters,
            IEnumerable<IComposableMessageAnnotation> composableMessageAnnotations,
            IEnumerable<ICollectableMessageAnnotation> collectableMessageAnnotations
        )
        {
            return new ConnectorInfo(gameObject, instance, type, name, parameters, composableMessageAnnotations, collectableMessageAnnotations);
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
    }
}
