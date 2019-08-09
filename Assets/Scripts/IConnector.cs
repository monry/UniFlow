using System;

namespace UniFlow
{
    public interface IConnector : IConnectable
    {
        IObservable<EventMessage> OnPublishAsObservable();
        void Connect(IObservable<EventMessages> source);
    }
}