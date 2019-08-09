namespace UniFlow
{
    public abstract class Receiver : EventConnectable, IReceiver
    {
        public abstract void OnReceive(EventMessages eventMessages);
    }
}