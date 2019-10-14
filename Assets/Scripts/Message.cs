using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace UniFlow
{
    [PublicAPI]
    public struct Message
    {
        public Message(IConnector connector, IDictionary<Type, IList> parameters = default)
        {
            this.connector = connector;
            this.parameters = parameters;
        }

        private IConnector connector;
        private IDictionary<Type, IList> parameters;
        private IDictionary<Type, IList> Parameters => parameters ?? (parameters = new Dictionary<Type, IList>());

        public static Message Create(IConnector connector)
        {
            return new Message(connector);
        }

        public static Message Create<T>(IConnector connector, T value)
        {
            return new Message(connector).AddParameter(value);
        }

        public static Message Create<T>(IConnector connector, IEnumerable<T> values)
        {
            return new Message(connector).AddParameters(values);
        }

        public Message AddParameter<T>(T value)
        {
            GetOrCreateParameterList<T>().Add(value);
            return this;
        }

        public Message AddParameters<T>(IEnumerable<T> values)
        {
            var list = GetOrCreateParameterList<T>();
            foreach (var value in values)
            {
                list.Add(value);
            }

            Parameters[typeof(T)] = (IList) list;

            return this;
        }

        public bool HasParameter<T>()
        {
            return GetOrCreateParameterList<T>().Any();
        }

        public bool HasParameter<T>(Func<T, bool> predicate)
        {
            return GetOrCreateParameterList<T>().Any(predicate);
        }

        public IEnumerable<T> GetParameters<T>()
        {
            return GetOrCreateParameterList<T>();
        }

        public T GetParameter<T>()
        {
            return GetOrCreateParameterList<T>().First();
        }

        public T GetParameter<T>(Func<T, bool> predicate)
        {
            return GetOrCreateParameterList<T>().First(predicate);
        }

        private IList<T> GetOrCreateParameterList<T>()
        {
            if (!Parameters.ContainsKey(typeof(T)))
            {
                Parameters.Add(typeof(T), new List<T>());
            }

            return Parameters[typeof(T)].Cast<T>().ToList();
        }
    }
}
