using System;
using UniRx;
using UnityEngine;

namespace EventConnector.Connector
{
    [AddComponentMenu("Event Connector/Empty", 10000)]
    public class Empty : EventPublisher
    {
        public override IObservable<EventMessage> OnPublishAsObservable() =>
            Observable.Return(EventMessage.Create(EventType.Empty, this));
    }
}