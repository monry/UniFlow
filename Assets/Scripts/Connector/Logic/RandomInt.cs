using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniFlow.Connector.Logic
{
    [AddComponentMenu("UniFlow/Logic/RandomInt", (int) ConnectorType.RandomInt)]
    public class RandomInt : ConnectorBase, IMessageCollectable, IMessageComposable
    {
        [SerializeField][Tooltip("Inclusive")] private int min = default;
        [SerializeField][Tooltip("Exclusive")] private int max = default;

        private int Min
        {
            get => min;
            set => min = value;
        }

        private int Max
        {
            get => max;
            set => max = value;
        }

        private int Value { get; set; }

        [SerializeField] private IntCollector minCollector = new IntCollector();
        [SerializeField] private IntCollector maxCollector = new IntCollector();
        private IntCollector MinCollector => minCollector;
        private IntCollector MaxCollector => maxCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            Value = Random.Range(Min, Max);
            return ObservableFactory.ReturnMessage(this, nameof(Value), Value);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(MinCollector, x => Min = x, nameof(Min)),
                CollectableMessageAnnotationFactory.Create(MaxCollector, x => Max = x, nameof(Max)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create<int>(null, nameof(Value)),
            };
    }
}
