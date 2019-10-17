using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Logic
{
    [AddComponentMenu("UniFlow/Logic/Interval", (int) ConnectorType.Interval)]
    public class Interval : ConnectorBase, IMessageCollectable
    {
        private const string MessageParameterKey = "Count";

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

        [SerializeField] private FloatCollector secondsCollector = new FloatCollector();
        [SerializeField] private BoolCollector ignoreTimeScaleCollector = new BoolCollector();

        private FloatCollector SecondsCollector => secondsCollector;
        private BoolCollector IgnoreTimeScaleCollector => ignoreTimeScaleCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return Observable
                .Interval(TimeSpan.FromSeconds(Seconds), IgnoreTimeScale ? Scheduler.MainThreadIgnoreTimeScale : Scheduler.MainThread)
                .AsMessageObservable(this, MessageParameterKey);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                CollectableMessageAnnotation<float>.Create(SecondsCollector, x => Seconds = x, nameof(Seconds)),
                CollectableMessageAnnotation<bool>.Create(IgnoreTimeScaleCollector, x => IgnoreTimeScale = x, nameof(IgnoreTimeScale)),
            };
    }
}
