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
        [SerializeField] private TPublishEvent publisher = new TPublishEvent();
        [ValuePublisher("Value")] public TPublishEvent Publisher => publisher;

        [SerializeField] private List<TValue> values = default;
        [ValueReceiver] public IEnumerable<TValue> Values
        {
            get => values;
            set => values = value.ToList();
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            if (this is IListValueProvider<TValue> listValueProvider)
            {
                Values = listValueProvider.Provide();
            }

            var list = Values.ToList();
            list.ForEach(Publisher.Invoke);
            return list.ToObservable().AsUnitObservable();
        }
    }
}
