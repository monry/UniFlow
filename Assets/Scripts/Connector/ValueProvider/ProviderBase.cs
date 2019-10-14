using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class ProviderBase<TValue, TPublishEvent> : ConnectorBase where TPublishEvent : UnityEvent<TValue>, new()
    {
        [SerializeField] private TPublishEvent publisher = new TPublishEvent();
        [ValuePublisher("Value")] public TPublishEvent Publisher => publisher;

        [SerializeField] private TValue value = default;
        [ValueReceiver] public TValue Value
        {
            get => value;
            set => this.value = value;
        }

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
    }
}
