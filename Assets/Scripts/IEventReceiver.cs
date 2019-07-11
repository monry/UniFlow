namespace EventConnector
{
    public interface IEventReceiver
    {
        void Receive(EventMessages eventMessages);
    }
}