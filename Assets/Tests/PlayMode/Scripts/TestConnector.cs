namespace EventConnector
{
    public class TestConnector : EventConnector
    {
        public EventMessages LatestEventMessages { get; private set; }
        public int InvokedCount { get; private set; }

        protected override void Connect(EventMessages eventMessages)
        {
            LatestEventMessages = eventMessages;
            InvokedCount++;
            OnConnect(eventMessages.Append(EventMessage.Create(EventType.Empty, this, null)));
        }
    }
}