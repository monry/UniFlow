namespace EventConnector.Tests.Runtime
{
    public class TestReceiver : EventReceiver
    {
        public EventMessages SentEventMessages { get; private set; }
        public int ReceiveCount { get; private set; }

        protected override void Receive(EventMessages eventMessages)
        {
            SentEventMessages = eventMessages;
            ReceiveCount++;
        }
    }
}