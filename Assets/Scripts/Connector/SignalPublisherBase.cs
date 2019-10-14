using System;
using UniFlow.Attribute;
using UniFlow.Connector.SignalPublisher;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector
{
    public abstract class SignalPublisherBase<TSignal> : ConnectorBase, ISignalPublisher<TSignal> where TSignal : ISignal
    {
        private const string MessageParameterKey = "Signal";

        [SerializeField] private TSignal signal = default;
        [ValueReceiver] public TSignal Signal
        {
            get => signal;
            set => signal = value;
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            if (this is ISignalCreator<TSignal> signalCreator)
            {
                Signal = signalCreator.CreateSignal();
            }
            ((ISignalPublisher<TSignal>) this).Publish(Signal);
            return ObservableFactory.ReturnMessage(this, MessageParameterKey, Signal);
        }

        void ISignalPublisher<TSignal>.Publish(TSignal value)
        {
            SignalHandler<TSignal>.Publish(value);
        }
    }
}
