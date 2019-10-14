using System;
using System.Collections.Generic;
using System.Linq;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class ListProviderBase<TValue, TPublishEvent> : ConnectorBase where TPublishEvent : UnityEvent<TValue>, new()
    {
        private const string MessageParameterKey = "Value";

        [SerializeField] private TPublishEvent publisher = new TPublishEvent();
        [ValuePublisher("Value")] public TPublishEvent Publisher => publisher;

        [SerializeField] private List<TValue> values = default;
        [ValueReceiver] public IEnumerable<TValue> Values
        {
            get => values;
            set => values = value.ToList();
        }

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
            return list.ToObservable().AsMessageObservable(this, MessageParameterKey);
        }
    }
}
