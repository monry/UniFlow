using JetBrains.Annotations;
using UniFlow.Connector;
using UniFlow.Connector.Event;

namespace UniFlow.Message
{
    [PublicAPI]
    public class SimpleAnimationEventData
    {
        private SimpleAnimationEventData(SimpleAnimationEventType eventType)
        {
            EventType = eventType;
        }

        public SimpleAnimationEventType EventType { get; }

        public static SimpleAnimationEventData Create(SimpleAnimationEventType eventType)
        {
            return new SimpleAnimationEventData(eventType);
        }
    }
}