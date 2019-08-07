using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace EventConnector
{
    public abstract class EventPublisher : EventConnectable, IEventPublisher
    {
        [SerializeField] [Tooltip("Specify instances of IEventConnectable directly")]
        private List<EventConnectable> targetConnectorInstances = default;

        [SerializeField] [Tooltip("Specify identifiers of IEventConnectable that resolve from Zenject.DiContainer")]
        private List<string> targetConnectorIds = default;

        [SerializeField] [Tooltip("Set true to allow to act as the entry point of events")]
        private bool actAsTrigger = false;

        [SerializeField] [Tooltip("Set true to allow to act as the receiver")]
        private bool actAsReceiver = false;

        private IEnumerable<IEventConnectable> TargetConnectors =>
            new List<IEventConnectable>()
                .Concat(targetConnectorInstances ?? new List<EventConnectable>())
                .Concat((targetConnectorIds ?? new List<string>()).SelectMany(Container.ResolveIdAll<IEventConnectable>))
                .Where(x => !ReferenceEquals(x, this))
                .ToArray();

        private bool ActAsTrigger => actAsTrigger;
        private bool ActAsReceiver => actAsReceiver;
        [Inject] private DiContainer Container { get; }

        protected virtual void Start()
        {
            if (ActAsTrigger)
            {
                ((IEventPublisher) this).Connect(Observable.Return<EventMessages>(default));
            }
        }

        void IEventPublisher.Connect(IObservable<EventMessages> source)
        {
            var observable = source
                .SelectMany(
                    eventMessages => (this as IEventPublisher)
                        .OnPublishAsObservable()
                        .Select(x => (eventMessages ?? EventMessages.Create()).Append(x))
                );
            TargetConnectors
                .OfType<IEventPublisher>()
                .ToList()
                .ForEach(x => x.Connect(observable));
            TargetConnectors
                .OfType<IEventReceiver>()
                .ToList()
                .ForEach(x => observable.Subscribe(x.OnReceive));

            if (ActAsReceiver)
            {
                observable.Subscribe();
            }
        }

        public abstract IObservable<EventMessage> OnPublishAsObservable();
    }
}