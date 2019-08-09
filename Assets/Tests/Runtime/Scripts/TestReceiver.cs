namespace UniFlow.Tests.Runtime
{
    public class TestReceiver : ReceiverBase
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