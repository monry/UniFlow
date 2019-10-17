using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class ListProviderBase<TValue, TValueCollector> : ConnectorBase,
        IMessageCollectable,
        IMessageComposable
        where TValueCollector : ValueCollectorBase<IEnumerable<TValue>>, new()
    {
        [SerializeField] private List<TValue> values = default;

        public IEnumerable<TValue> Values
        {
            get => values;
            set => values = value.ToList();
        }

        [SerializeField] private TValueCollector valuesCollector = new TValueCollector();
        private TValueCollector ValuesCollector => valuesCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return Values
                .ToList()
                .ToObservable()
                .AsMessageObservable(this, typeof(TValue).Name);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<IEnumerable<TValue>>.Create(ValuesCollector, x => Values = x, $"{typeof(TValue).Name}List"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                // Will compose parameter in OnConnectAsObservable()
                ComposableMessageAnnotation<TValue>.Create(null),
            };
    }

    [Serializable]
    public class CollectMessage
    {
    }
}
