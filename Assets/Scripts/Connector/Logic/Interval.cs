using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Logic
{
    [AddComponentMenu("UniFlow/Logic/Interval", (int) ConnectorType.Interval)]
    public class Interval : ConnectorBase
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

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return Observable
                .Interval(TimeSpan.FromSeconds(Seconds), IgnoreTimeScale ? Scheduler.MainThreadIgnoreTimeScale : Scheduler.MainThread)
                .AsUnitObservable();
        }
    }
}
