using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;
using Zenject;

namespace UniFlow.Connector
{
    public abstract class SignalPublisherBase<TSignal> : ConnectorBase
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
}
