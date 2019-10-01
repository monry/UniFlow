using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/TimeScaleController", (int) ConnectorType.TimeScaleController)]
    public class TimeScaleController : ConnectorBase
    {
        [SerializeField] private float timeScale = default;
        [SerializeField] private float duration = default;
        [SerializeField] private ObservableTween.EaseType easeType = default;

        [ValueReceiver] private float TimeScale
        {
            get => timeScale;
            set => timeScale = value;
        }
        [ValueReceiver] private float Duration
        {
            get => duration;
            set => duration = value;
        }
        [ValueReceiver] private ObservableTween.EaseType EaseType
        {
            get => easeType;
            set => easeType = value;
        }

        private ISubject<Unit> OnCompleteTweenSubject { get; } = new Subject<Unit>();

        public override IObservable<Unit> OnConnectAsObservable()
        {
            ChangeTimeScale();
            return OnCompleteTweenSubject;
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
    }
}
