using System.Collections.Generic;
using JetBrains.Annotations;

namespace UniFlow
{
    [PublicAPI]
    public class EventMessages : List<EventMessage>
    {
        private EventMessages()
        {
        }

        public EventMessages Append(EventMessage eventMessage)
        {
            Add(eventMessage);
            return this;
        }

        public EventMessages AppendRange(IEnumerable<EventMessage> eventMessages)
        {
            AddRange(eventMessages);
            return this;
        }

        public static EventMessages Create()
        {
            return new EventMessages();
        }
    }

    [PublicAPI]
    public struct EventMessage
    {
        private EventMessage(EventType eventType, object sender, object eventData)
        {
            EventType = eventType;
            Sender = sender;
            EventData = eventData;
        }

        public EventType EventType { get; }
        public object Sender { get; }
        public object EventData { get; }

        public static EventMessage Create(EventType eventType, object sender, object eventData = default)
        {
            return new EventMessage(eventType, sender, eventData);
        }
    }
}