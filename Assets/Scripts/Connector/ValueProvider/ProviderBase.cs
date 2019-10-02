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
        [ValuePublisher("Value")] private TPublishEvent Publisher => publisher;

        protected abstract TValue Provide();

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Publisher.Invoke(Provide());
            return Observable.ReturnUnit();
        }
    }
}
