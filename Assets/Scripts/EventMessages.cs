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
}