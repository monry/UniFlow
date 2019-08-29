using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UniFlow.Editor
{
    public class ConnectableInfo
    {
        private ConnectableInfo(GameObject gameObject, Type type, string name, IEnumerable<Parameter> parameters)
        {
            GameObject = gameObject;
            Type = type;
            Name = name;
            Parameters = parameters;
        }

        public GameObject GameObject { get; set; }
        public Type Type { get; }
        public string Name { get; }
        public IEnumerable<Parameter> Parameters { get; }

        public class Parameter
        {
            private Parameter(Type type, string name, object value)
            {
                Type = type;
                Name = name;
                Value = value;
            }

            public Type Type { get; }
            public string Name { get; }
            public object Value { get; set; }

            public static Parameter Create(Type type, string name, object value)
            {
                return new Parameter(type, name, value);
            }
        }

        public static ConnectableInfo Create(IConnectable instance)
        {
            return Create(instance.GetType(), instance);
        }

        public static ConnectableInfo Create(Type type, IConnectable instance = default)
        {
            return
                new ConnectableInfo(
                    ((Component) instance)?.gameObject,
                    type,
                    type.Name,
                    type
                        .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Where(x => x.GetCustomAttribute<SerializeField>() != null || x.IsPublic)
                        .Select(x => Parameter.Create(x.FieldType, x.Name, instance != default ? x.GetValue(instance) : default))
                );
        }

        public static ConnectableInfo Create(GameObject gameObject, Type type, string name, IEnumerable<Parameter> parameters)
        {
            return new ConnectableInfo(gameObject, type, name, parameters);
        }
    }
}