using System;

namespace UniFlow
{
    public interface IConnector : IEventConnectable
    {
        IObservable<EventMessage> OnPublishAsObservable();
        void Connect(IObservable<EventMessages> source);
    }
}