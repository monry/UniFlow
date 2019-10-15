using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniFlow
{
    [Serializable]
    public abstract class ValueCollectorBase<TValue> : IValueCollector
    {
        [SerializeField] private ConnectorBase connector = default;
        [SerializeField] private string typeString = default;
        [SerializeField] private string composerKey = default;
        [SerializeField] private string collectorLabel = default;

        public IConnector Connector
        {
            get => connector;
            set => connector = value as ConnectorBase;
        }

        public string TypeString
        {
            get => typeString;
            set => typeString = value;
        }

        public string ComposerKey
        {
            get => composerKey;
            set => composerKey = value;
        }

        public string CollectorLabel
        {
            get => collectorLabel;
            set => collectorLabel = value;
        }

        public Action<TValue> Action { get; set; }

        public void Collect(IList<Message> messages)
        {
            if (messages == null || Connector == null || !messages.HasMessage(Connector) || !messages.FindMessage(Connector).HasParameter<TValue>())
            {
                return;
            }

            var value = string.IsNullOrEmpty(ComposerKey)
                ? messages.FindMessage(Connector).GetParameter<TValue>()
                : messages.FindMessage(Connector).GetParameter<TValue>(ComposerKey);
            Action(value);
        }
    }
}
