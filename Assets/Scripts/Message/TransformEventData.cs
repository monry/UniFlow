using EventConnector.Connector;
using JetBrains.Annotations;

namespace EventConnector.Message
{
    [PublicAPI]
    public class TransformEventData
    {
        private TransformEventData(TransformEventType eventType)
        {
            EventType = eventType;
        }

        public TransformEventType EventType { get; }

        public static TransformEventData Create(TransformEventType eventType) =>
            new TransformEventData(eventType);
    }
}