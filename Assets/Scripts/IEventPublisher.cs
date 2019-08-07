using System;

namespace EventConnector
{
    public interface IEventPublisher : IEventConnector
    {
        IObservable<EventMessage> OnPublishAsObservable();
        void Connect(IObservable<EventMessages> source);
    }
}