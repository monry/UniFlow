using System;
using EventConnector.Message;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EventConnector.Connector
{
    // Timeline SignalReceiver cannot serialize [Serializable] class
    // So I provide overloads to construct TimelineEventData
    [AddComponentMenu("Event Connector/TimelineSignal")]
    public class TimelineSignal : EventConnector
    {
        private ISubject<TimelineEventData> Subject { get; } = new Subject<TimelineEventData>();

        protected override IObservable<EventMessages> Connect(EventMessages eventMessages) =>
            Subject
                .Select(x => eventMessages.Append((this, x)));

        public void Dispatch()
        {
            Subject.OnNext(new TimelineEventData());
        }

        public void Dispatch(float floatParameter)
        {
            Subject.OnNext(new TimelineEventData(floatParameter));
        }

        public void Dispatch(int intParameter)
        {
            Subject.OnNext(new TimelineEventData(intParameter));
        }

        public void Dispatch(string stringParameter)
        {
            Subject.OnNext(new TimelineEventData(stringParameter));
        }

        public void Dispatch(Object objectReferenceParameter)
        {
            Subject.OnNext(new TimelineEventData(objectReferenceParameter));
        }
    }
}