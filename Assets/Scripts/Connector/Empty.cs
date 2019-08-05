namespace EventConnector.Connector
{
    public class Empty : EventConnector
    {
        protected override void Connect(EventMessages eventMessages)
        {
            OnConnect(eventMessages.Append(EventMessage.Create(EventType.Empty, this)));
        }
    }
}