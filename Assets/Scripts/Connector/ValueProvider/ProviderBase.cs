using System;
using System.Collections.Generic;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class ProviderBase<TValue, TPublishEvent, TValueCollector> : ConnectorBase, IMessageCollectable
        where TPublishEvent : UnityEvent<TValue>, new()
        where TValueCollector : ValueCollectorBase<TValue>, new()
    {
        [SerializeField] private TPublishEvent publisher = new TPublishEvent();
        [ValuePublisher("Value")] public TPublishEvent Publisher => publisher;

        [SerializeField] private TValue value = default;
        [ValueReceiver] public TValue Value
        {
            get => value;
            set => this.value = value;
        }

        [SerializeField] private TValueCollector valueCollector = default;
        private TValueCollector ValueCollector => valueCollector ?? (valueCollector = new TValueCollector());

        public override IObservable<Message> OnConnectAsObservable()
        {
            var message = this.CreateMessage().AddParameter(nameof(Value), Value);

            if (this is IValueProvider<TValue> valueProvider)
            {
                Value = valueProvider.Provide();
            }

            if (this is IValueCombiner<TValue> valueCombiner)
            {
                Value = valueCombiner.Combine();
            }

            if (this is IValueExtractor<TValue> valueExtractor)
            {
                valueExtractor.Extract(Value);
            }

            Publisher.Invoke(Value);

            return Observable.Return(message);
        }

        IEnumerable<CollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                new CollectableMessageAnnotation(typeof(TValue), ValueCollector, "Value"),
            };

        void IMessageCollectable.Collect()
        {
            ValueCollector.Collect(StreamedMessages);
        }

        void IMessageCollectable.RegisterCollectDelegates()
        {
            ValueCollector.Action = v => Value = v;
        }
    }
}
