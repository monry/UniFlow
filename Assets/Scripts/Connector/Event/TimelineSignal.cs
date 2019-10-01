using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniFlow.Connector.Event
{
    // Timeline SignalReceiver cannot serialize [Serializable] class
    // So I provide overloads to construct TimelineEventData
    [AddComponentMenu("UniFlow/Event/TimelineSignal", (int) ConnectorType.TimelineSignal)]
    public class TimelineSignal : ConnectorBase
    {
        private ISubject<(int intParameter, float floatParameter, string stringParameter, Object objectParameter)> Subject { get; } = new Subject<(int intParameter, float floatParameter, string stringParameter, Object objectParameter)>();

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return Subject
                // Prevents the previous flow from being re-invoked when triggered multiple times
                .Take(1)
                .AsUnitObservable();
        }

        [UsedImplicitly]
        public void Dispatch()
        {
            Subject.OnNext((default, default, default, default));
        }

        [UsedImplicitly]
        public void Dispatch(int intParameter)
        {
            Subject.OnNext((intParameter, default, default, default));
        }

        [UsedImplicitly]
        public void Dispatch(float floatParameter)
        {
            Subject.OnNext((default, floatParameter, default, default));
        }

        [UsedImplicitly]
        public void Dispatch(string stringParameter)
        {
            Subject.OnNext((default, default, stringParameter, default));
        }

        [UsedImplicitly]
        public void Dispatch(Object objectReferenceParameter)
        {
            Subject.OnNext((default, default, default, objectReferenceParameter));
        }
    }
}
