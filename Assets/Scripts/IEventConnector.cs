using System;
using System.Collections.Generic;

namespace EventConnector
{
    public interface IEventConnector
    {
        void Connect(IObservable<EventMessages> source);
    }
}