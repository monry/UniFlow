using System;
using System.Collections.Generic;

namespace EventConnector
{
    public interface IEventConnector
    {
        IEnumerable<IEventConnector> TargetConnectors { get; }
        void Con(EventMessages eventMessages);
        IObservable<EventMessage> FooAsObservable();
    }
}