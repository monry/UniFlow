namespace EventConnector
{
    public class TestComponent : EventReceiver
    {
        public EventMessages SentEventMessages { get; private set; }

        public override void Receive(EventMessages eventMessages)
        {
            SentEventMessages = eventMessages;
        }
    }
}