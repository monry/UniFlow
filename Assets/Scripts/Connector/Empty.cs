using System;
using UniRx;

namespace EventConnector.Connector
{
    public class Empty : EventPublisher
    {
        public override IObservable<EventMessage> OnPublishAsObservable() =>
            Observable.Return(EventMessage.Create(EventType.Empty, this));
    }
}