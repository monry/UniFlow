using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;
using Zenject;

namespace UniFlow.Connector.Misc
{
    public abstract class BoundarySignalSenderBase<TBoundarySignal> : ConnectorBase
    {
        protected abstract TBoundarySignal BoundarySignal { get; }

        [Inject] private IBoundarySignalSender<TBoundarySignal> BoundaryCoordinator { get; }

        public override IObservable<Message> OnConnectAsObservable()
        {
            BoundaryCoordinator.Send(BoundarySignal);
            return ObservableFactory.ReturnMessage(this, nameof(BoundarySignal), BoundarySignal);
        }
    }

    public abstract class FlexibleBoundarySignalSenderBase<TBoundarySignal, TBoundarySignalCollector> : BoundarySignalSenderBase<TBoundarySignal>, IMessageCollectable
        where TBoundarySignalCollector : ValueCollectorBase<TBoundarySignal>, new()
    {
        [SerializeField] private TBoundarySignal boundarySignal = default;
        [SerializeField] private TBoundarySignalCollector enumCollector = new TBoundarySignalCollector();

        protected override TBoundarySignal BoundarySignal => boundarySignal;

        private TBoundarySignalCollector EnumCollector => enumCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<TBoundarySignal>.Create(EnumCollector, x => boundarySignal = x),
            };
    }

    public abstract class FixedBoundarySignalSenderBase<TValue> : BoundarySignalSenderBase<TValue>
    {
        protected override TValue BoundarySignal => GetFixedValue();

        protected abstract TValue GetFixedValue();
    }
}
