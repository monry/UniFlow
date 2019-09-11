using System;
using JetBrains.Annotations;
using UniFlow.Message;
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

        [UsedImplicitly] private float TimeScale
        {
            get => timeScale;
            set => timeScale = value;
        }
        [UsedImplicitly] private float Duration
        {
            get => duration;
            set => duration = value;
        }
        [UsedImplicitly] private ObservableTween.EaseType EaseType
        {
            get => easeType;
            set => easeType = value;
        }

        private ISubject<Unit> OnCompleteTweenSubject { get; } = new Subject<Unit>();

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            ChangeTimeScale();
            return OnCompleteTweenSubject.Select(_ => Message.Create(this));
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

        public class Message : MessageBase<TimeScaleController>
        {
            public static Message Create(TimeScaleController sender)
            {
                return Create<Message>(ConnectorType.TimeScaleController, sender);
            }
        }
    }
}