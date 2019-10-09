using System;
using UniFlow.Attribute;
using UniFlow.Utility;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    public abstract class SignalReceiverBase<TSignal> : ConnectorBase, ISignalReceiver<TSignal> where TSignal : ISignal
    {
        [SerializeField] private TSignal signal = default;
        [ValueReceiver] public TSignal Signal => signal;

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return ((ISignalReceiver<TSignal>) this).OnReceiveAsObservable().AsUnitObservable();
        }

        IObservable<TSignal> ISignalReceiver<TSignal>.OnReceiveAsObservable()
        {
            return SignalHandler<TSignal>.OnReceiveAsObservable(Signal);
        }
    }
}
