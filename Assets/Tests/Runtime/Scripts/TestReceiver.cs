namespace UniFlow.Tests.Runtime
{
    public class TestReceiver : EventReceiver
    {
        public EventMessages SentEventMessages { get; private set; }
        public int ReceiveCount { get; private set; }

        public override void OnReceive(EventMessages eventMessages)
        {
            SentEventMessages = eventMessages;
            ReceiveCount++;
        }
    }
}