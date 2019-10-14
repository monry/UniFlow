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
        public Message(IConnector connector, IDictionary<Type, IDictionary<string, IList>> parameters = default)
        {
            this.connector = connector;
            this.parameters = parameters;
            streamedMessages = default;
        }

        private IConnector connector;
        private IDictionary<Type, IDictionary<string, IList>> parameters;
        private IList<Message> streamedMessages;

        private IDictionary<Type, IDictionary<string, IList>> Parameters => parameters ?? (parameters = new Dictionary<Type, IDictionary<string, IList>>());

        public IList<Message> StreamedMessages
        {
            get => streamedMessages ?? (streamedMessages = new List<Message>());
            set => streamedMessages = value;
        }

        public static Message Create(IConnector connector)
        {
            return new Message(connector);
        }

        public static Message Create<T>(IConnector connector, T value)
        {
            return Create(connector, typeof(T).Name, value);
        }

        public static Message Create<T>(IConnector connector, string key, T value)
        {
            return new Message(connector).AddParameter(key, value);
        }

        public static Message Create<T>(IConnector connector, IEnumerable<T> values)
        {
            return Create(connector, $"{typeof(T).Name}List", values);
        }

        public static Message Create<T>(IConnector connector, string key, IEnumerable<T> values)
        {
            return new Message(connector).AddParameters(key, values);
        }

        public Message AddParameter<T>(T value)
        {
            return AddParameter(typeof(T).Name, value);
        }

        public Message AddParameter<T>(string key, T value)
        {
            GetOrCreateParameterList<T>(key).Add(value);
            return this;
        }

        public Message AddParameters<T>(IEnumerable<T> values)
        {
            return AddParameters($"{typeof(T).Name}List", values);
        }

        public Message AddParameters<T>(string key, IEnumerable<T> values)
        {
            var list = GetOrCreateParameterList<T>(key);
            foreach (var value in values)
            {
                list.Add(value);
            }

            Parameters[typeof(T)][key] = list as IList;

            return this;
        }

        public bool HasParameter<T>(string key)
        {
            return GetOrCreateParameterList<T>(key).Any();
        }

        public bool HasParameter<T>(string key, Func<T, bool> predicate)
        {
            return GetOrCreateParameterList<T>(key).Any(predicate);
        }

        public IEnumerable<T> GetParameters<T>(string key)
        {
            return GetOrCreateParameterList<T>(key);
        }

        public T GetParameter<T>(string key, bool latest = true)
        {
            return latest ? GetOrCreateParameterList<T>(key).Last() : GetOrCreateParameterList<T>(key).First();
        }

        public T GetParameter<T>(string key, Func<T, bool> predicate, bool latest = true)
        {
            return latest ? GetOrCreateParameterList<T>(key).Last(predicate) : GetOrCreateParameterList<T>(key).First(predicate);
        }

        private IList<T> GetOrCreateParameterList<T>(string key)
        {
            if (!Parameters.ContainsKey(typeof(T)))
            {
                Parameters.Add(typeof(T), new Dictionary<string, IList>());
            }

            if (!Parameters[typeof(T)].ContainsKey(key))
            {
                Parameters[typeof(T)][key] = new List<T>();
            }

            return Parameters[typeof(T)][key] as IList<T>;
        }
    }
}
