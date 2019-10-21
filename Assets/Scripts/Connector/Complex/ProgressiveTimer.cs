using System;
using System.Collections.Generic;
using UniFlow;
using UniFlow.Utility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace PretendLand.Edwin.Scripts.Connector.Complex
{
    [AddComponentMenu("UniFlow/Complex/ProgressiveTimer", (int) ConnectorType.ProgressiveTimer)]
    public class ProgressiveTimer : ConnectorBase, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private float seconds = default;
        [SerializeField] private ProgressDirection direction = default;
        private float Seconds
        {
            get => seconds;
            set => seconds = value;
        }
        private ProgressDirection Direction => direction;

        [SerializeField] private FloatCollector secondsCollector = new FloatCollector();
        private FloatCollector SecondsCollector => secondsCollector;

        private IObservable<float> ProgressObservable { get; set; }
        private float ProceedTime { get; set; } = 0.0f;

        public override IObservable<Message> OnConnectAsObservable()
        {
            var observable = this.UpdateAsObservable().TakeUntil(Observable.Timer(TimeSpan.FromSeconds(Seconds))).Share();
            observable.Subscribe(_ => ProceedTime += Time.deltaTime);
            switch (Direction)
            {
                case ProgressDirection.Increase:
                    ProgressObservable = observable.Select(_ => ProceedTime / Seconds);
                    break;
                case ProgressDirection.Decrease:
                    ProgressObservable = observable.Select(_ => 1.0f - ProceedTime / Seconds);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return ObservableFactory.ReturnMessage(this);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(SecondsCollector, x => Seconds = x, nameof(Seconds)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create(() => ProgressObservable, nameof(ProgressObservable)),
            };

        private enum ProgressDirection
        {
            Increase,
            Decrease,
        }
    }
}
