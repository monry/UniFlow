using System;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/Timer", 9000)]
    public class Timer : ConnectorBase
    {
        [SerializeField] private float seconds = default;
        private float Seconds => seconds;

        public override IObservable<EventMessage> OnConnectAsObservable() =>
            Observable
                .Timer(TimeSpan.FromSeconds(Seconds))
                .Select(_ => EventMessage.Create(ConnectorType.Timer, this, Seconds));
    }
}