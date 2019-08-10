using System;

namespace UniFlow
{
    public interface IConnector : IConnectable
    {
        IObservable<EventMessage> OnConnectAsObservable();
        void Connect(IObservable<EventMessages> source);
    }
}