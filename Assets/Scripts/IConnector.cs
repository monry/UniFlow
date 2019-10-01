using System;
using UniRx;

namespace UniFlow
{
    public interface IConnector
    {
        IObservable<Unit> OnConnectAsObservable();
        void Connect(IObservable<Unit> source);
#if UNITY_EDITOR
        ISubject<Unit> OnConnectSubject { get; }
#endif
    }
}
