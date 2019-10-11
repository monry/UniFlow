using System;
using UniFlow.Attribute;
using UniFlow.Connector.SignalPublisher;
using UniFlow.Connector.SignalReceiver;
using UniFlow.Utility;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    public abstract class SignalReceiverBase<TSignal> : ConnectorBase, ISignalReceiver<TSignal> where TSignal : ISignal
    {
        [SerializeField] private TSignal signal = default;
        [ValueReceiver] public TSignal Signal
        {
            get => signal;
            set => signal = value;
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            if (this is ISignalCreator<TSignal> signalCreator)
            {
                Signal = signalCreator.CreateSignal();
            }
            return ((ISignalReceiver<TSignal>) this).OnReceiveAsObservable().AsUnitObservable();
        }

        IObservable<TSignal> ISignalReceiver<TSignal>.OnReceiveAsObservable()
        {
            var observable = SignalHandler<TSignal>.OnReceiveAsObservable(Signal);
            if (this is ISignalFilter<TSignal> signalFilter)
            {
                observable = observable.Where(signalFilter.Filter);
            }
            return observable;
        }
    }
}
