using System;

namespace EventConnector
{
    public interface IEventPublisher : IEventConnector
    {
        IObservable<EventMessage> OnPublishAsObservable();
    }
}