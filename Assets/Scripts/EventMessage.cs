using JetBrains.Annotations;

namespace UniFlow
{
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