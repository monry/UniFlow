using System;
using UniRx;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class ProviderBase<TValue> : ConnectorBase
    {
        protected abstract UnityEvent<TValue> Publisher { get; }
        protected abstract TValue Provide();

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Publisher.Invoke(Provide());
            return Observable.ReturnUnit();
        }
    }
}
