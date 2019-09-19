namespace UniFlow
{
    public interface IReceiver : IConnector
    {
        void OnReceive(Messages messages);
    }
}