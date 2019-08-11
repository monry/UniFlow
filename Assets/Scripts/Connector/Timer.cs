using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/Timer", 9000)]
    public class Timer : ConnectorBase
    {
        [SerializeField] private float seconds = default;
        private float Seconds
        {
            get => seconds;
            [UsedImplicitly]
            set => seconds = value;
        }

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return Observable
                .Timer(TimeSpan.FromSeconds(Seconds))
                .Select(_ => EventMessage.Create(ConnectorType.Timer, this, Seconds));
        }
    }
}