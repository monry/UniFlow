using System;
using UnityEngine;

namespace UniFlow
{
    [Serializable]
    public abstract class ValueCollectorBase<TValue> : IValueCollector<TValue>
    {
        [SerializeField] private ConnectorBase sourceConnector = default;
        [SerializeField] private ConnectorBase targetConnector = default;
        [SerializeField] private string typeString = default;
        [SerializeField] private string composerKey = default;
        [SerializeField] private string collectorKey = default;

        public IConnector SourceConnector
        {
            get => sourceConnector;
            set => sourceConnector = value as ConnectorBase;
        }

        public IConnector TargetConnector
        {
            get => targetConnector;
            set => targetConnector = value as ConnectorBase;
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

        public string CollectorKey
        {
            get => collectorKey;
            set => collectorKey = value;
        }

        public bool CanCollect()
        {
            return
                SourceConnector != null
                && TargetConnector?.StreamedMessages != null
                && TargetConnector.StreamedMessages.HasMessage(SourceConnector)
                && TargetConnector.StreamedMessages.GetMessage(SourceConnector).HasParameter<TValue>(ComposerKey);
        }

        public TValue Collect()
        {
            return TargetConnector.StreamedMessages.GetMessage(SourceConnector).GetParameter<TValue>(ComposerKey);
        }
    }
}
