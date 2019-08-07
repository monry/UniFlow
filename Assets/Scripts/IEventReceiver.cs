namespace EventConnector
{
    public interface IEventReceiver : IEventConnector
    {
        void OnReceive(EventMessages eventMessages);
    }
}