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
            return Create(connector, typeof(T).Name, value);
        }

        public static Message Create<T>(IConnector connector, string key, T value)
        {
            return new Message(connector).AddParameter(key, value);
        }

        public Message AddParameter<T>(T value)
        {
            return AddParameter(typeof(T).Name, value);
        }

        public Message AddParameter<T>(string key, T value)
        {
            GetOrCreateParameterDictionary<T>()[key] = value;
            return this;
        }

        public bool HasParameter<T>()
        {
            return HasParameter<T>(typeof(T).Name);
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
            return GetParameter<T>(typeof(T).Name);
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
