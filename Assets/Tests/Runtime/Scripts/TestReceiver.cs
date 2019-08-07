using System;

namespace EventConnector.Tests.Runtime
{
    public class TestReceiver : EventReceiver
    {
        public EventMessages SentEventMessages { get; private set; }
        public int ReceiveCount { get; private set; }

        public override void Receive(EventMessages eventMessages)
        {
            SentEventMessages = eventMessages;
            ReceiveCount++;
        }

        public override IObservable<EventMessage> FooAsObservable()
        {
            return null;
        }
    }
}