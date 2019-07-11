using System;

namespace EventConnector
{
    public interface IEventConnector
    {
        IObservable<EventMessages> ConnectAsObservable(EventMessages eventMessages);
    }
}