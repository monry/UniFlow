using System;
using System.Collections.Generic;
using UniFlow.Signal;
using UniFlow.Utility;
using UniRx;
using UnityEngine;
using Zenject;

namespace UniFlow.Connector
{
    public abstract class SignalReceiverBase<TSignal> : ConnectorBase, IMessageComposable
    {
        [SerializeField] private TSignal signal = default;

        [Inject] private ISignalReceiver<TSignal> SignalReceiver { get; }

        protected TSignal Signal
        {
            get => signal;
            set => signal = value;
        }

        protected TSignal ReceivedSignal { get; private set; }

        protected virtual TSignal GetSignal()
        {
            return Signal;
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            return SignalReceiver.OnReceiveAsObservable(GetSignal()).Do(x => ReceivedSignal = x).AsMessageObservable(this, "Signal");
        }

        public virtual IEnumerable<IComposableMessageAnnotation> GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create(() => ReceivedSignal, "Signal"),
            };
    }

    public abstract class SignalReceiverBase<TSignal, TSignalCollector> : SignalReceiverBase<TSignal>, IMessageCollectable
        where TSignalCollector : ValueCollectorBase<TSignal>, new()
    {
        [SerializeField] private TSignalCollector signalCollector = new TSignalCollector();

        private TSignalCollector SignalCollector => signalCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(SignalCollector, x => Signal = x, "Signal"),
            };
    }


    public abstract class SignalReceiverWithParameterBase<TSignal, TParameter> : SignalReceiverBase<TSignal>
        where TSignal : SignalBase<TSignal, TParameter>, new()
        where TParameter : Enum
    {
        [SerializeField] private TParameter enumValue = default;
        private TParameter EnumValue => enumValue;

        protected override TSignal GetSignal()
        {
            return SignalBase<TSignal, TParameter>.Create(EnumValue);
        }
    }
}
