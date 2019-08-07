using System;
using UniRx;

namespace EventConnector.Connector
{
    public class Empty : EventConnector, IEventPublisher
    {
        IObservable<EventMessage> IEventPublisher.OnPublishAsObservable() =>
            Observable.Return(EventMessage.Create(EventType.Empty, this));
    }
}