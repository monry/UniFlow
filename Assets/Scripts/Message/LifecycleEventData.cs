using System;
using EventConnector.Connector;
using JetBrains.Annotations;

namespace EventConnector.Message
{
    [Serializable][PublicAPI]
    public class LifecycleEventData
    {
        private LifecycleEventData(LifecycleEventType eventType)
        {
            EventType = eventType;
        }

        public LifecycleEventType EventType { get; }

        public static LifecycleEventData Create(LifecycleEventType lifecycleEventType) =>
            new LifecycleEventData(lifecycleEventType);
    }
}