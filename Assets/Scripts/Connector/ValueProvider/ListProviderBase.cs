using System;
using System.Collections.Generic;
using System.Linq;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class ListProviderBase<TValue, TPublishEvent, TValueCollector> : ConnectorBase, IMessageComposable, IMessageCollectable
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
        private TValueCollector ValuesCollector => valuesCollector ?? (valuesCollector = new TValueCollector());

        public override IObservable<Message> OnConnectAsObservable()
        {
            if (this is IListValueProvider<TValue> listValueProvider)
            {
                var temporaryValues = listValueProvider.Provide();
                if (listValueProvider is IFilteredListValueProvider<TValue> filteredListValueProvider)
                {
                    temporaryValues = temporaryValues.Where(filteredListValueProvider.Predicate);
                }

                Values = temporaryValues;
            }

            var list = Values.ToList();
            list.ForEach(Publisher.Invoke);
            return list
                .ToObservable()
                .AsMessageObservable(this, typeof(TValue).Name);
        }

        IEnumerable<ComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                new ComposableMessageAnnotation(this, typeof(TValue)),
            };

        Message IMessageComposable.Compose(Message message) => message;

        IEnumerable<CollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                new CollectableMessageAnnotation(typeof(IEnumerable<TValue>), ValuesCollector, "List"),
            };

        void IMessageCollectable.Collect()
        {
            ValuesCollector.Collect(StreamedMessages);
        }

        void IMessageCollectable.RegisterCollectDelegates()
        {
            ValuesCollector.Action = value => Values = value;
        }
    }

    [Serializable]
    public class CollectMessage
    {
    }
}
