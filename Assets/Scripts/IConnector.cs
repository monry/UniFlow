using System;
using UniRx;

namespace UniFlow
{
    public interface IConnector
    {
        IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage);
        void Connect(IObservable<(IMessage latestMessage, Messages massages)> source);
#if UNITY_EDITOR
        ISubject<IMessage> OnConnectSubject { get; }
#endif
    }
}
