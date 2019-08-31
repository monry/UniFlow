using JetBrains.Annotations;
using UniFlow.Connector;
using UniFlow.Connector.Event;

namespace UniFlow.Message
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