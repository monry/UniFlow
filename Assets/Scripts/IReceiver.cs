namespace UniFlow
{
    public interface IReceiver : IConnectable
    {
        void OnReceive(EventMessages eventMessages);
    }
}