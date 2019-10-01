using System;
using UniRx;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class Base<TValue> : ConnectorBase
    {
        public override IObservable<Unit> OnConnectAsObservable()
        {
            return Observable.ReturnUnit();
        }

        protected abstract TValue Provide();
    }
}
