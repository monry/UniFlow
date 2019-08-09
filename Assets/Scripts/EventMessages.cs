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
        private EventMessage(ConnectorType connectorType, object sender, object data)
        {
            ConnectorType = connectorType;
            Sender = sender;
            Data = data;
        }

        public ConnectorType ConnectorType { get; }
        public object Sender { get; }
        public object Data { get; }

        public static EventMessage Create(ConnectorType connectorType, object sender, object data = default)
        {
            return new EventMessage(connectorType, sender, data);
        }
    }
}