using System;
using UniRx;
using UnityEngine;

namespace EventConnector.Connector
{
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