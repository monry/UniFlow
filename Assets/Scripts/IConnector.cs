using System;

namespace UniFlow
{
    public interface IConnector : IConnectable
    {
        IObservable<IMessage> OnConnectAsObservable(IMessage previousMessage);
        void Connect(IObservable<Messages> source);
    }
}