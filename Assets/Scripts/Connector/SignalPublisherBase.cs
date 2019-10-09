using System;
using UniFlow.Attribute;
using UniFlow.Utility;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    public abstract class SignalPublisherBase<TSignal> : ConnectorBase, ISignalPublisher<TSignal> where TSignal : ISignal
    {
        [SerializeField] private TSignal signal = default;
        [ValueReceiver] public TSignal Signal => signal;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            ((ISignalPublisher<TSignal>) this).Publish(Signal);
            return Observable.ReturnUnit();
        }

        void ISignalPublisher<TSignal>.Publish(TSignal value)
        {
            SignalHandler<TSignal>.Publish(value);
        }
    }
}
