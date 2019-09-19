using System;

namespace UniFlow
{
    public interface IConnector
    {
        IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage);
        void Connect(IObservable<(IMessage latestMessage, Messages massages)> source);
    }
}