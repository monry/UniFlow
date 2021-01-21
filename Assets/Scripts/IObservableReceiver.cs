using System;
using UniRx;

namespace UniFlow
{
    public interface IObservableReceiver : IReceiver
    {
        IObservable<Unit> OnReceiveAsObservable();
    }
}
