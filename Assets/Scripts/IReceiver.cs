namespace UniFlow
{
    public interface IReceiver : IEventConnectable
    {
        void OnReceive(EventMessages eventMessages);
    }
}