namespace UniFlow.Receiver
{
    public class EmptyReceiver : ReceiverBase
    {
        public override void OnReceive(EventMessages eventMessages)
        {
            // Do nothing.
        }
    }
}