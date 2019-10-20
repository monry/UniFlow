using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UniFlow.Connector.Misc
{
    public abstract class BoundarySignalReceiverBase<TBoundarySignal> : ConnectorBase
    {
        protected abstract TBoundarySignal BoundarySignal { get; }

        [Inject] private IBoundarySignalReceiver<TBoundarySignal> BoundaryCoordinator { get; }

        public override IObservable<Message> OnConnectAsObservable()
        {
            return BoundaryCoordinator.OnReceiveAsObservable(BoundarySignal).AsMessageObservable(this);
        }
    }

    public abstract class FlexibleBoundarySignalReceiverBase<TBoundarySignal, TBoundarySignalCollector> : BoundarySignalReceiverBase<TBoundarySignal>, IMessageCollectable
        where TBoundarySignalCollector : ValueCollectorBase<TBoundarySignal>, new()
    {
        [SerializeField] private TBoundarySignal boundarySignal = default;
        [SerializeField] private TBoundarySignalCollector boundarySignalCollector = new TBoundarySignalCollector();

        protected override TBoundarySignal BoundarySignal => boundarySignal;

        private TBoundarySignalCollector BoundarySignalCollector => boundarySignalCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(BoundarySignalCollector, x => boundarySignal = x, nameof(BoundarySignal)),
            };
    }

    public abstract class FixedBoundarySignalReceiverBase<TBoundarySignal> : BoundarySignalReceiverBase<TBoundarySignal>
    {
        [SerializeField] private TBoundarySignal boundarySignal = default;

        protected override TBoundarySignal BoundarySignal => boundarySignal;
    }
}
