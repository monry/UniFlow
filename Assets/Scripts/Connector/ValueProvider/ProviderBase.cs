using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class ProviderBase<TValue, TPublishEvent> : ConnectorBase, IInjectable<TValue>
        where TPublishEvent : UnityEvent<TValue>, new()
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
            return Observable.Return(this.CreateMessage());
        }

        void IInjectable<TValue>.Inject(TValue v)
        {
            Value = v;
        }
    }
}
