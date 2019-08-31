using System;

namespace UniFlow
{
    public interface IObservableReceiver : IReceiver
    {
        IObservable<EventMessages> OnReceiveAsObservable();
    }
}