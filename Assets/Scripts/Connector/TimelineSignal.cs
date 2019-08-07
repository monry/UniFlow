using System;
using EventConnector.Message;
using JetBrains.Annotations;
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

        public override IObservable<EventMessage> FooAsObservable() =>
            Subject
                .Select(x => EventMessage.Create(EventType.TimelineSignal, this, x));

        [UsedImplicitly]
        public void Dispatch()
        {
            Subject.OnNext(new TimelineEventData());
        }

        [UsedImplicitly]
        public void Dispatch(float floatParameter)
        {
            Subject.OnNext(new TimelineEventData(floatParameter));
        }

        [UsedImplicitly]
        public void Dispatch(int intParameter)
        {
            Subject.OnNext(new TimelineEventData(intParameter));
        }

        [UsedImplicitly]
        public void Dispatch(string stringParameter)
        {
            Subject.OnNext(new TimelineEventData(stringParameter));
        }

        [UsedImplicitly]
        public void Dispatch(Object objectReferenceParameter)
        {
            Subject.OnNext(new TimelineEventData(objectReferenceParameter));
        }
    }
}