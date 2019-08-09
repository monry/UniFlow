using System;
using JetBrains.Annotations;
using UniFlow.Connector;

namespace UniFlow.Message
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