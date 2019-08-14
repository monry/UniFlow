using System;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/TimeScaleController", 9002)]
    public class TimeScaleController : ConnectorBase
    {
        [SerializeField] private float timeScale = default;
        private float TimeScale => timeScale;

        [SerializeField] private float duration = default;
        private float Duration => duration;

        [SerializeField] private ObservableTween.EaseType easeType = default;
        private ObservableTween.EaseType EaseType => easeType;

        private ISubject<Unit> OnCompleteTweenSubject { get; } = new Subject<Unit>();

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            ChangeTimeScale();
            return OnCompleteTweenSubject.Select(_ => EventMessage.Create(ConnectorType.TimeScaleController, this));
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
                .Subscribe(x => Time.timeScale = x);
        }
    }
}