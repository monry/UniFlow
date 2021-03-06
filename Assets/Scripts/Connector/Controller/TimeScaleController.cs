using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/TimeScaleController", (int) ConnectorType.TimeScaleController)]
    public class TimeScaleController : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private float timeScale = default;
        [SerializeField] private float duration = default;
        [SerializeField] private ObservableTween.EaseType easeType = default;

        private float TimeScale
        {
            get => timeScale;
            set => timeScale = value;
        }
        private float Duration
        {
            get => duration;
            set => duration = value;
        }
        private ObservableTween.EaseType EaseType
        {
            get => easeType;
            set => easeType = value;
        }

        [SerializeField] private FloatCollector timeScaleCollector = new FloatCollector();
        [SerializeField] private FloatCollector durationCollector = new FloatCollector();
        [SerializeField] private EaseTypeCollector easeTypeCollector = new EaseTypeCollector();

        private FloatCollector TimeScaleCollector => timeScaleCollector;
        private FloatCollector DurationCollector => durationCollector;
        private EaseTypeCollector EaseTypeCollector => easeTypeCollector;

        private ISubject<Unit> OnCompleteTweenSubject { get; } = new Subject<Unit>();

        public override IObservable<Message> OnConnectAsObservable()
        {
            ChangeTimeScale();
            return OnCompleteTweenSubject.Select(this.CreateMessage);
        }

        private void ChangeTimeScale()
        {
            if (Mathf.Approximately(0.0f, Duration))
            {
                Time.timeScale = TimeScale;
                OnCompleteTweenSubject.OnNext(Unit.Default);
                return;
            }

            ObservableTween
                .Tween(
                    Time.timeScale,
                    TimeScale,
                    Duration,
                    EaseType,
                    ObservableTween.LoopType.None,
                    () => OnCompleteTweenSubject.OnNext(Unit.Default),
                    true
                )
                .Subscribe(x => Time.timeScale = x)
                .AddTo(this);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(TimeScaleCollector, x => TimeScale = x, nameof(TimeScale)),
                CollectableMessageAnnotationFactory.Create(DurationCollector, x => Duration = x, nameof(Duration)),
                CollectableMessageAnnotationFactory.Create(EaseTypeCollector, x => EaseType = x, nameof(EaseType)),
            };
    }
}
