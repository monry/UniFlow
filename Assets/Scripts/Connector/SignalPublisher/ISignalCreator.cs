namespace UniFlow.Connector.SignalPublisher
{
    public interface ISignalCreator<out TSignal> where TSignal : ISignal
    {
        TSignal CreateSignal();
    }
}
