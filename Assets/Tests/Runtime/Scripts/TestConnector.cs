namespace EventConnector.Tests.Runtime
{
    public class TestConnector : EventConnector
    {
        public EventMessages LatestEventMessages { get; private set; }
        public int InvokedCount { get; private set; }

        protected override void Connect(EventMessages eventMessages)
        {
            // Do not invoke eventMessage.Append()
            LatestEventMessages = eventMessages;
            InvokedCount++;
        }
    }
}