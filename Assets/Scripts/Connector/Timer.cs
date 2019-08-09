using System;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("Event Connector/Timer", 9000)]
    public class Timer : EventPublisher
    {
        [SerializeField] private float seconds = default;
        private float Seconds => seconds;

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            Observable
                .Timer(TimeSpan.FromSeconds(Seconds))
                .Select(_ => EventMessage.Create(EventType.Timer, this, Seconds));
    }
}