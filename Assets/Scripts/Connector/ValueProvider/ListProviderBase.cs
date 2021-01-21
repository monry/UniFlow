using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Random = System.Random;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class ListProviderBase<TValue> : ConnectorBase, IInjectable<IEnumerable<TValue>>, IMessageComposable
    {
        [SerializeField] private List<TValue> values = default;

        [SerializeField] private EnumerationType enumerationType = default;

        private EnumerationType EnumerationType => enumerationType;

        [UsedImplicitly] public IEnumerable<TValue> Values
        {
            get => values;
            set => values = value.ToList();
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            IEnumerable<TValue> v;
            switch (EnumerationType)
            {
                case EnumerationType.Normal:
                    v = Values;
                    break;
                case EnumerationType.Reverse:
                    v = Values.Reverse();
                    break;
                case EnumerationType.Shuffle:
                    v = Values.Shuffle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return v
                .ToList()
                .ToObservable()
                .AsMessageObservable(this, typeof(TValue).Name);
        }

        void IInjectable<IEnumerable<TValue>>.Inject(IEnumerable<TValue> value)
        {
            Values = value;
        }

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                // Will compose parameter in OnConnectAsObservable()
                ComposableMessageAnnotationFactory.Create<TValue>(),
            };
    }

    public abstract class ListProviderBase<TValue, TValueCollector> : ConnectorBase,
        IMessageCollectable,
        IMessageComposable
        where TValueCollector : ValueCollectorBase<IEnumerable<TValue>>, new()
    {
        [SerializeField] private List<TValue> values = default;

        [SerializeField] private EnumerationType enumerationType = default;

        private EnumerationType EnumerationType => enumerationType;

        [UsedImplicitly] public IEnumerable<TValue> Values
        {
            get => values;
            set => values = value.ToList();
        }

        [SerializeField] private TValueCollector valuesCollector = new TValueCollector();
        private TValueCollector ValuesCollector => valuesCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            IEnumerable<TValue> v;
            switch (EnumerationType)
            {
                case EnumerationType.Normal:
                    v = Values;
                    break;
                case EnumerationType.Reverse:
                    v = Values.Reverse();
                    break;
                case EnumerationType.Shuffle:
                    v = Values.Shuffle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return v
                .ToList()
                .ToObservable()
                .AsMessageObservable(this, typeof(TValue).Name);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(ValuesCollector, x => Values = x, $"{typeof(TValue).Name}List"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                // Will compose parameter in OnConnectAsObservable()
                ComposableMessageAnnotationFactory.Create<TValue>(),
            };
    }

    public enum EnumerationType
    {
        Normal,
        Reverse,
        Shuffle,
    }

    internal static class ListExtensions
    {
        private static Random Random { get; } = new Random();

        internal static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var elements = source.ToArray();

            for (var n = 0; n < elements.Length; n++)
            {
                var k = Random.Next(n, elements.Length);
                yield return elements[k];

                elements[k] = elements[n];
            }
        }
    }
}
