namespace EventConnector
{
    public interface IEventReceiver : IEventConnector
    {
        void Receive(EventMessages eventMessages);
    }
}