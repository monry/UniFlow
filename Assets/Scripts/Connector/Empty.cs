using System;
using UniRx;

namespace EventConnector.Connector
{
    public class Empty : EventConnector
    {
        protected override IObservable<EventMessages> Connect(EventMessages eventMessages)
        {
            return Observable.Return(eventMessages.Append((EventType.Empty, this, null)));
        }
    }
}