using System.Collections.Generic;
using JetBrains.Annotations;

namespace EventConnector
{
    [PublicAPI]
    public class EventMessages : List<(EventType eventType, object sender, object eventData)>
    {
        private EventMessages()
        {
        }

        public EventMessages Append((EventType eventType, object sender, object eventData) eventMessage)
        {
            Add(eventMessage);
            return this;
        }

        public EventMessages AppendRange(IEnumerable<(EventType eventType, object sender, object eventData)> eventMessages)
        {
            AddRange(eventMessages);
            return this;
        }

        public EventMessages ClearIfNeeded(IEventConnectable eventConnectable)
        {
            if (!eventConnectable.HasSourceConnectors())
            {
                Clear();
            }

            return this;
        }

        public static EventMessages Create()
        {
            return new EventMessages();
        }

        public static EventMessages Create((EventType eventType, object sender, object eventData) eventMessage)
        {
            return new EventMessages {eventMessage};
        }
    }
}