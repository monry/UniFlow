using System;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/Interval", 9001)]
    public class Interval : EventPublisher
    {
        [SerializeField] private float seconds = default;
        private float Seconds => seconds;

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            Observable
                .Interval(TimeSpan.FromSeconds(Seconds))
                .Select(_ => EventMessage.Create(EventType.Interval, this, Seconds));
    }
}