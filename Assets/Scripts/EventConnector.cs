using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace EventConnector
{
    public abstract class EventConnector : MonoBehaviour, IEventConnector
    {
        [SerializeField]
        [Tooltip("Specify instances of IEventConnector directly")]
        private List<EventConnector> targetConnectorInstances = default;
        [SerializeField]
        [Tooltip("Specify identifiers of IEventConnector that resolve from Zenject.DiContainer")]
        private List<string> targetConnectorIds = default;

        public IEnumerable<IEventConnector> TargetConnectors =>
            new List<IEventConnector>()
                .Concat(targetConnectorInstances ?? new List<EventConnector>())
                .Concat((targetConnectorIds ?? new List<string>()).SelectMany(Container.ResolveIdAll<IEventConnector>));

        [Inject] private DiContainer Container { get; }

        public void Con(EventMessages eventMessages)
        {
            FooAsObservable()
                .ForEachAsync(
                    eventMessage =>
                    {
                        eventMessages.Append(eventMessage);
                        TargetConnectors
                            .Where(x => !ReferenceEquals(x, this))
                            .Where(x => !(x is IEventReceiver))
                            .ToList()
                            .ForEach(x =>x.Con(eventMessages));
                        TargetConnectors
                            .Where(x => !ReferenceEquals(x, this))
                            .OfType<IEventReceiver>()
                            .ToList()
                            .ForEach(x => x.Receive(eventMessages));
                    }
                );
        }

        public abstract IObservable<EventMessage> FooAsObservable();
    }
}