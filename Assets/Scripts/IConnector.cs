using System;
using System.Collections.Generic;
using UniRx;

namespace UniFlow
{
    public interface IConnector
    {
        IObservable<Message> OnConnectAsObservable();
        void Connect(IObservable<Message> source);
        IList<Message> StreamedMessages { get; }
#if UNITY_EDITOR
        ISubject<Message> OnConnectSubject { get; }
#endif
    }
}
