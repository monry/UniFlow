namespace UniFlow
{
    public interface ISignalPublisher<in TSignal> where TSignal : ISignal
    {
        void Publish(TSignal signal);
    }
}
