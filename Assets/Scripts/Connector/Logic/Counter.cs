using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Logic
{
    [AddComponentMenu("UniFlow/Logic/Counter", (int) ConnectorType.Counter)]
    public class Counter : ConnectorBase, IMessageCollectable, IMessageComposable
    {
        [SerializeField] private IntCollector initialCountCollector = default;
        private IntCollector InitialCountCollector => initialCountCollector;

        private IReactiveProperty<int> CounterProperty { get; } = new IntReactiveProperty();

        public override IObservable<Message> OnConnectAsObservable()
        {
            CounterProperty.Value++;
            return CounterProperty.AsMessageObservable(this, "Count");
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(InitialCountCollector, x => CounterProperty.Value = x, "InitialCount"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create<int>("Count"),
            };
    }
}
