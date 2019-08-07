using System;
using UniRx;

namespace EventConnector.Tests.Runtime
{
    public class TestConnector : EventConnector
    {
        public EventMessages LatestEventMessages { get; private set; }
        public int InvokedCount { get; private set; }

        public override IObservable<EventMessage> FooAsObservable() =>
            Observable.Return(EventMessage.Create(EventType.Empty, this));

        protected override void Connect(EventMessages eventMessages)
        {
            // Do not invoke eventMessage.Append()
            LatestEventMessages = eventMessages;
            InvokedCount++;
        }
    }
}