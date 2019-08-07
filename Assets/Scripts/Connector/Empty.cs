using System;
using UniRx;

namespace EventConnector.Connector
{
    public class Empty : EventConnector
    {
        public override IObservable<EventMessage> FooAsObservable() =>
            Observable.Return(EventMessage.Create(EventType.Empty, this));
    }
}