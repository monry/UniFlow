using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace EventConnector
{
    public abstract class EventPublisher : EventConnector, IEventPublisher
    {
        [SerializeField] [Tooltip("Specify instances of IEventConnector directly")]
        private List<EventConnector> targetConnectorInstances = default;

        [SerializeField] [Tooltip("Specify identifiers of IEventConnector that resolve from Zenject.DiContainer")]
        private List<string> targetConnectorIds = default;

        [SerializeField] [Tooltip("Set true to allow to act as the entry point of events")]
        private bool actAsTrigger = false;

        private IEnumerable<IEventConnector> TargetConnectors =>
            new List<IEventConnector>()
                .Concat(targetConnectorInstances ?? new List<EventConnector>())
                .Concat((targetConnectorIds ?? new List<string>()).SelectMany(Container.ResolveIdAll<IEventConnector>))
                .Where(x => !ReferenceEquals(x, this))
                .ToArray();

        private bool ActAsTrigger => actAsTrigger;
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
        }

        public abstract IObservable<EventMessage> OnPublishAsObservable();
    }
}