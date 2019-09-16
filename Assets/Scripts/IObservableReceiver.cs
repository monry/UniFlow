using System;

namespace UniFlow
{
    public interface IObservableReceiver : IReceiver
    {
        IObservable<Messages> OnReceiveAsObservable();
    }
}