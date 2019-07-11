using System;
using EventConnector.Message;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EventConnector.Connector
{
    // Timeline SignalReceiver cannot serialize [Serializable] class
    // So I provide overloads to construct TimelineEvent
    [AddComponentMenu("Event Connector/TimelineSignal")]
    public class TimelineSignal : EventConnector
    {
        private ISubject<TimelineEvent> Subject { get; } = new Subject<TimelineEvent>();

        protected override IObservable<EventMessages> Connect(EventMessages eventMessages) =>
            Subject
                .Select(x => eventMessages.Append((this, x)));

        public void Dispatch()
        {
            Subject.OnNext(new TimelineEvent());
        }

        public void Dispatch(float floatParameter)
        {
            Subject.OnNext(new TimelineEvent(floatParameter));
        }

        public void Dispatch(int intParameter)
        {
            Subject.OnNext(new TimelineEvent(intParameter));
        }

        public void Dispatch(string stringParameter)
        {
            Subject.OnNext(new TimelineEvent(stringParameter));
        }

        public void Dispatch(Object objectReferenceParameter)
        {
            Subject.OnNext(new TimelineEvent(objectReferenceParameter));
        }
    }
}