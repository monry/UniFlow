using JetBrains.Annotations;
using UniFlow.Connector;
using UniFlow.Connector.Event;

namespace UniFlow.Message
{
    [PublicAPI]
    public class RectTransformEventData
    {
        private RectTransformEventData(RectTransformEventType eventType)
        {
            EventType = eventType;
        }

        public RectTransformEventType EventType { get; }

        public static RectTransformEventData Create(RectTransformEventType eventType) =>
            new RectTransformEventData(eventType);
    }
}