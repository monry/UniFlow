using System;
using UniRx;

namespace EventConnector
{
    public class TestConnector : EventConnector
    {
        public EventMessages LatestEventMessages { get; private set; }
        public int InvokedCount { get; private set; }

        protected override IObservable<EventMessages> Connect(EventMessages eventMessages)
        {
            LatestEventMessages = eventMessages;
            InvokedCount++;
            return Observable.Return(eventMessages);
        }
    }
}