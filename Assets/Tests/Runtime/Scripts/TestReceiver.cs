namespace UniFlow.Tests.Runtime
{
    public class TestReceiver : ReceiverBase
    {
        public Messages SentMessages { get; private set; }
        public int ReceiveCount { get; private set; }

        public override void OnReceive(Messages messages)
        {
            SentMessages = messages;
            ReceiveCount++;
        }
    }
}