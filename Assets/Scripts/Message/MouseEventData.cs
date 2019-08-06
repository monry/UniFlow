using EventConnector.Connector;

namespace EventConnector.Message
{
    public class MouseEventData
    {
        private MouseEventData(MouseEventType eventType)
        {
            EventType = eventType;
        }

        public MouseEventType EventType { get; }

        public static MouseEventData Create(MouseEventType eventType) =>
            new MouseEventData(eventType);
    }
}