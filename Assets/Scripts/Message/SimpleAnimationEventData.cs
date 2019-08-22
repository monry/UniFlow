using JetBrains.Annotations;
using UniFlow.Connector;

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