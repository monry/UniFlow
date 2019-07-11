using System.Collections.Generic;
using JetBrains.Annotations;

namespace EventConnector
{
    [PublicAPI]
    public class EventMessages : List<(object sender, object eventData)>
    {
        private EventMessages()
        {
        }

        public EventMessages Append((object sender, object eventData) eventMessage)
        {
            Add(eventMessage);
            return this;
        }

        public EventMessages AppendRange(IEnumerable<(object sender, object eventData)> eventMessages)
        {
            AddRange(eventMessages);
            return this;
        }

        public static EventMessages Create()
        {
            return new EventMessages();
        }

        public static EventMessages Create((object sender, object eventData) eventMessage)
        {
            return new EventMessages {eventMessage};
        }
    }
}