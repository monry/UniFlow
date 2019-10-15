using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace UniFlow
{
    [PublicAPI]
    public struct Message
    {
        public Message(IConnector connector, IDictionary<Type, IDictionary<string, object>> parameters = default)
        {
            this.connector = connector;
            this.parameters = parameters;
            streamedMessages = default;
        }

        private IConnector connector;
        private IDictionary<Type, IDictionary<string, object>> parameters;
        private IList<Message> streamedMessages;

        private IDictionary<Type, IDictionary<string, object>> Parameters => parameters ?? (parameters = new Dictionary<Type, IDictionary<string, object>>());

        public IConnector Connector => connector;
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
            return new Message(connector).AddParameter(value);
        }

        public static Message Create<T>(IConnector connector, T value, string key)
        {
            return new Message(connector).AddParameter(value, key);
        }

        public Message AddParameter<T>(T value)
        {
            GetOrCreateParameterDictionary<T>()[string.Empty] = value;
            return this;
        }

        public Message AddParameter<T>(T value, string key)
        {
            GetOrCreateParameterDictionary<T>()[key] = value;
            return this;
        }

        public bool HasParameter<T>()
        {
            return GetOrCreateParameterDictionary<T>().ContainsKey(string.Empty);
        }

        public bool HasParameter<T>(string key)
        {
            return GetOrCreateParameterDictionary<T>().ContainsKey(key);
        }

        public IDictionary<string, T> GetParameters<T>()
        {
            return GetOrCreateParameterDictionary<T>();
        }

        public T GetParameter<T>()
        {
            return GetOrCreateParameterDictionary<T>()[string.Empty];
        }

        public T GetParameter<T>(string key)
        {
            return GetOrCreateParameterDictionary<T>()[key];
        }

        private IDictionary<string, T> GetOrCreateParameterDictionary<T>()
        {
            if (!Parameters.ContainsKey(typeof(T)))
            {
                Parameters.Add(typeof(T), new Dictionary<string, object>());
            }

            return Parameters[typeof(T)] as IDictionary<string, T>;
        }
    }
}
