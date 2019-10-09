using System;

namespace UniFlow
{
    public interface ISignalReceiver<out TSignal> where TSignal : ISignal
    {
        IObservable<TSignal> OnReceiveAsObservable();
    }
}
