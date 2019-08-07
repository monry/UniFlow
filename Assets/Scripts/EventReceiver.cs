namespace EventConnector
{
    public abstract class EventReceiver : EventConnector, IEventReceiver
    {
        public abstract void OnReceive(EventMessages eventMessages);
    }
}