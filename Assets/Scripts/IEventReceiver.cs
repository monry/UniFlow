namespace EventConnector
{
    public interface IEventReceiver : IEventConnectable
    {
        void OnReceive(EventMessages eventMessages);
    }
}