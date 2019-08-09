namespace UniFlow
{
    public interface IEventReceiver : IEventConnectable
    {
        void OnReceive(EventMessages eventMessages);
    }
}