using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class ProviderBase<TValue> : ConnectorBase, IInjectable<TValue>
    {
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
