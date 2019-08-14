using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/Interval", 9001)]
    public class Interval : ConnectorBase
    {
        [SerializeField] private float seconds = default;
        [UsedImplicitly] public float Seconds
        {
            get => seconds;
            set => seconds = value;
        }

        [SerializeField] private bool ignoreTimeScale = default;
        private bool IgnoreTimeScale => ignoreTimeScale;

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return Observable
                .Interval(TimeSpan.FromSeconds(Seconds), IgnoreTimeScale ? Scheduler.MainThreadIgnoreTimeScale : Scheduler.MainThread)
                .Select(_ => EventMessage.Create(ConnectorType.Interval, this, Seconds));
        }
    }
}