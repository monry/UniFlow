using EventConnector.Connector;
using JetBrains.Annotations;

namespace EventConnector.Message
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