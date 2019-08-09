namespace UniFlow
{
    public abstract class ReceiverBase : EventConnectable, IReceiver
    {
        public abstract void OnReceive(EventMessages eventMessages);
    }
}