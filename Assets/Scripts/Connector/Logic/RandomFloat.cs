using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniFlow.Connector.Logic
{
    [AddComponentMenu("UniFlow/Logic/RandomFloat", (int) ConnectorType.RandomFloat)]
    public class RandomFloat : ConnectorBase, IMessageCollectable, IMessageComposable
    {
        [SerializeField][Tooltip("Inclusive")] private float min = default;
        [SerializeField][Tooltip("Inclusive")] private float max = default;

        private float Min
        {
            get => min;
            set => min = value;
        }

        private float Max
        {
            get => max;
            set => max = value;
        }

        private float Value { get; set; }

        [SerializeField] private FloatCollector minCollector = new FloatCollector();
        [SerializeField] private FloatCollector maxCollector = new FloatCollector();
        private FloatCollector MinCollector => minCollector;
        private FloatCollector MaxCollector => maxCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            Value = Random.Range(Min, Max);
            return ObservableFactory.ReturnMessage(this, nameof(Value), Value);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<float>.Create(MinCollector, x => Min = x, nameof(Min)),
                CollectableMessageAnnotation<float>.Create(MaxCollector, x => Max = x, nameof(Max)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotation<float>.Create(null, nameof(Value)),
            };
    }
}
