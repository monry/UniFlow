using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

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
            SetValue(string.Empty, value);
            return this;
        }

        public Message AddParameter<T>(T value, string key)
        {
            SetValue(key, value);
            return this;
        }

        public bool HasParameter<T>()
        {
            return HasValue<T>(string.Empty);
        }

        public bool HasParameter<T>(string key)
        {
            return HasValue<T>(key);
        }

        public T GetParameter<T>()
        {
            return GetValue<T>(string.Empty);
        }

        public T GetParameter<T>(string key)
        {
            return GetValue<T>(key);
        }

        private bool HasValue<T>(string key)
        {
            var type = typeof(ScriptableObject).IsAssignableFrom(typeof(T)) ? typeof(ScriptableObject) : typeof(T);
            return Parameters.ContainsKey(type) && Parameters[type].ContainsKey(key);
        }

        private T GetValue<T>(string key)
        {
            var type = typeof(ScriptableObject).IsAssignableFrom(typeof(T)) ? typeof(ScriptableObject) : typeof(T);

            if (!Parameters.ContainsKey(type))
            {
                Parameters.Add(type, new Dictionary<string, object>());
            }

            if (!Parameters[type].ContainsKey(key))
            {
                return default;
            }

            return (T) Parameters[type][key];
        }

        private void SetValue<T>(string key, T value)
        {
            var type = typeof(ScriptableObject).IsAssignableFrom(typeof(T)) ? typeof(ScriptableObject) : typeof(T);

            if (!Parameters.ContainsKey(type))
            {
                Parameters.Add(type, new Dictionary<string, object>());
            }

            Parameters[type][key] = value;
        }
    }
}
