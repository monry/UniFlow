namespace UniFlow
{
    public abstract class ReceiverBase : ConnectableBase, IReceiver
    {
        public abstract void OnReceive(EventMessages eventMessages);
    }
}