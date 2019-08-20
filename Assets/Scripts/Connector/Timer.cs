using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/Timer", (int) ConnectorType.Timer)]
    public class Timer : ConnectorBase
    {
        [SerializeField] private float seconds = default;
        [SerializeField] private bool ignoreTimeScale = default;

        [UsedImplicitly] public float Seconds
        {
            get => seconds;
            set => seconds = value;
        }
        [UsedImplicitly] private bool IgnoreTimeScale
        {
            get => ignoreTimeScale;
            set => ignoreTimeScale = value;
        }

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return Observable
                .Timer(TimeSpan.FromSeconds(Seconds), IgnoreTimeScale ? Scheduler.MainThreadIgnoreTimeScale : Scheduler.MainThread)
                .Select(_ => EventMessage.Create(ConnectorType.Timer, this, Seconds));
        }
    }
}