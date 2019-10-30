using System;
using System.Collections.Generic;
using UniFlow.Signal;
using UniFlow.Utility;
using UnityEngine;
using Zenject;

namespace UniFlow.Connector
{
    public abstract class SignalPublisherBase<TSignal> : ConnectorBase
        where TSignal : ISignal
    {
        [SerializeField] private TSignal signal = default;

        [Inject] private ISignalPublisher<TSignal> Publisher { get; }

        // ReSharper disable once ConvertToAutoPropertyWhenPossible
        protected TSignal Signal
        {
            get => signal;
            set => signal = value;
        }

        protected virtual TSignal GetSignal()
        {
            return Signal;
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            Publisher.Publish(GetSignal());
            return ObservableFactory.ReturnMessage(this, nameof(Signal), GetSignal());
        }
    }

    public abstract class SignalPublisherBase<TSignal, TSignalCollector> : SignalPublisherBase<TSignal>, IMessageCollectable
        where TSignal : ISignal
        where TSignalCollector : ValueCollectorBase<TSignal>, new()
    {
        [SerializeField] private TSignalCollector enumCollector = new TSignalCollector();

        private TSignalCollector EnumCollector => enumCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(EnumCollector, x => Signal = x, nameof(Signal)),
            };
    }

    public abstract class SignalPublisherWithParameterBase<TSignal, TParameter> : SignalPublisherBase<TSignal>
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

    public abstract class SignalPublisherWithParameterBase<TSignal, TParameter, TParameterCollector> : SignalPublisherBase<TSignal>, IMessageCollectable
        where TSignal : SignalBase<TSignal, TParameter>, new()
        where TParameterCollector : ValueCollectorBase<TParameter>, new()
        where TParameter : Enum
    {
        [SerializeField] private TParameter enumValue = default;
        private TParameter EnumValue
        {
            get => enumValue;
            set => enumValue = value;
        }

        [SerializeField] private TParameterCollector enumCollector = new TParameterCollector();
        private TParameterCollector EnumCollector => enumCollector;

        protected override TSignal GetSignal()
        {
            return SignalBase<TSignal, TParameter>.Create(EnumValue);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new []
            {
                CollectableMessageAnnotationFactory.Create(EnumCollector, x => EnumValue = x, nameof(EnumValue)),
            };
    }
}
