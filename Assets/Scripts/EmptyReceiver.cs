namespace UniFlow
{
    public class EmptyReceiver : Receiver
    {
        public override void OnReceive(EventMessages eventMessages)
        {
            // Do nothing.
        }
    }
}