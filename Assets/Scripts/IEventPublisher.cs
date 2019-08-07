using System;

namespace EventConnector
{
    public interface IEventPublisher : IEventConnectable
    {
        IObservable<EventMessage> OnPublishAsObservable();
        void Connect(IObservable<EventMessages> source);
    }
}