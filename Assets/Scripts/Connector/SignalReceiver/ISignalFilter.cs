namespace UniFlow.Connector.SignalReceiver
{
    public interface ISignalFilter<TSignal> : ISignalReceiver<TSignal> where TSignal : ISignal
    {
        bool Filter(TSignal signal);
    }
}
