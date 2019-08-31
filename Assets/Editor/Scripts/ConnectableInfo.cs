using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace UniFlow.Editor
{
    [Serializable]
    public class ConnectableInfo
    {
        private ConnectableInfo(GameObject gameObject, IConnectable connectable, Type type, string name, IEnumerable<Parameter> parameters)
        {
            GameObject = gameObject;
            Connectable = connectable;
            Type = type;
            Name = name;
            parameterList = parameters.ToList();
        }

        public GameObject GameObject { get; set; }
        public IConnectable Connectable { get; set; }
        public Type Type { get; }
        public string Name { get; }
        [SerializeField] private List<Parameter> parameterList = default;
        public IEnumerable<Parameter> ParameterList => parameterList;

        public void ApplyParameter(Parameter parameter)
        {
            if (Connectable == default)
            {
                return;
            }

            Type
                .GetField(parameter.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?
                .SetValue(Connectable, parameter.Value);
        }

        public void ApplyParameters()
        {
            if (Connectable == default)
            {
                return;
            }

            ParameterList.ToList().ForEach(ApplyParameter);
        }

        [PublicAPI]
        public static ConnectableInfo Create(IConnectable instance)
        {
            return Create(instance.GetType(), instance);
        }

        [PublicAPI]
        public static ConnectableInfo Create(Type type, IConnectable instance = default)
        {
            return Create(
                ((Component) instance)?.gameObject,
                instance,
                type,
                type.Name,
                type
                    .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(x => x.GetCustomAttribute<SerializeField>() != null || x.IsPublic)
                    .Select(x => Parameter.Create(x.FieldType, x.Name, instance != default ? x.GetValue(instance) : default))
            );
        }

        [PublicAPI]
        public static ConnectableInfo Create(GameObject gameObject, IConnectable instance, Type type, string name, IEnumerable<Parameter> parameters)
        {
            return new ConnectableInfo(gameObject, instance, type, name, parameters);
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