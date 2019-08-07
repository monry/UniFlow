using System;
using UniRx;

namespace EventConnector.Tests.Runtime
{
    public class TestConnector : EventConnector, IEventReceiver
    {
        public EventMessages LatestEventMessages { get; private set; }
        public int InvokedCount { get; private set; }

        public override IObservable<EventMessage> FooAsObservable() =>
            Observable.Return(EventMessage.Create(EventType.Empty, this));

        public void Receive(EventMessages eventMessages)
        {
            LatestEventMessages = eventMessages;
            InvokedCount++;
        }
    }
}