using JetBrains.Annotations;

namespace UniFlow
{
    [PublicAPI]
    public class EventMessage
    {
        protected EventMessage(ConnectorType connectorType, object sender, object data)
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

    public class EventMessage2<TData>
    {
        protected EventMessage2(ConnectorType connectorType, TData data)
        {
            ConnectorType = connectorType;
            Data = data;
        }

        public ConnectorType ConnectorType { get; }
        public TData Data { get; }
    }
}