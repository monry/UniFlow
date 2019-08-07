using System;
using UniRx;

namespace EventConnector.Connector
{
    public class Empty : EventConnector
    {
        public override IObservable<EventMessage> FooAsObservable() =>
            Observable.Return(EventMessage.Create(EventType.Empty, this));

        protected override void Connect(EventMessages eventMessages)
        {
            Observable
                .EveryEndOfFrame()
                .Take(1)
                .SubscribeWithState(
                    eventMessages,
                    (_, em) => OnConnect(em.Append(EventMessage.Create(EventType.Empty, this)))
                );
        }
    }
}