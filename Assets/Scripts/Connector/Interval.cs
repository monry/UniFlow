using System;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/Interval", 9001)]
    public class Interval : ConnectorBase
    {
        [SerializeField] private float seconds = default;
        private float Seconds => seconds;

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            Observable
                .Interval(TimeSpan.FromSeconds(Seconds))
                .Select(_ => EventMessage.Create(ConnectorType.Interval, this, Seconds));
    }
}