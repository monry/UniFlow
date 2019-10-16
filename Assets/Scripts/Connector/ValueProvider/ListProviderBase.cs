using System;
using System.Collections.Generic;
using System.Linq;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class ListProviderBase<TValue, TPublishEvent, TValueCollector> : ConnectorBase,
        IMessageCollectable,
        IMessageComposable
        where TPublishEvent : UnityEvent<TValue>, new()
        where TValueCollector : ValueCollectorBase<IEnumerable<TValue>>, new()
    {
        [SerializeField] private TPublishEvent publisher = new TPublishEvent();
        [ValuePublisher("Value")] public TPublishEvent Publisher => publisher;

        [SerializeField] private List<TValue> values = default;

        [ValueReceiver]
        public IEnumerable<TValue> Values
        {
            get => values;
            set => values = value.ToList();
        }

        [SerializeField] private TValueCollector valuesCollector = default;
        private TValueCollector ValuesCollector => valuesCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            var list = Values.ToList();
            list.ForEach(Publisher.Invoke);
            return list
                .ToObservable()
                .AsMessageObservable(this, typeof(TValue).Name);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                new CollectableMessageAnnotation<IEnumerable<TValue>>(ValuesCollector, x => Values = x, $"{typeof(TValue).Name}List"),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                // Will compose parameter in OnConnectAsObservable()
                new ComposableMessageAnnotation<TValue>(null),
            };
    }

    [Serializable]
    public class CollectMessage
    {
    }
}
