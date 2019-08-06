using System;

namespace EventConnector
{
    public interface IEventConnector : IObserver<EventMessages>, IObservable<EventMessages>
    {
        IObservable<EventMessages> ConnectAsObservable();
    }
}