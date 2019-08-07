namespace EventConnector
{
    public class EmptyEventReceiver : EventReceiver
    {
        public override void OnReceive(EventMessages eventMessages)
        {
            // Do nothing.
        }
    }
}