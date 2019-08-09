using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniFlow.Connector
{
    // Timeline SignalReceiver cannot serialize [Serializable] class
    // So I provide overloads to construct TimelineEventData
    [AddComponentMenu("UniFlow/TimelineSignal", 306)]
    public class TimelineSignal : EventPublisher
    {
        private ISubject<TimelineEventData> Subject { get; } = new Subject<TimelineEventData>();

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            Subject
                .Take(1)
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