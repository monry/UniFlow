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
        private List<EventConnector> sourceConnectorInstances = default;
        [SerializeField]
        [Tooltip("Specify identifiers of IEventConnector that resolve from Zenject.DiContainer")]
        private List<string> sourceConnectorIds = default;

        private IEnumerable<IEventConnector> SourceConnectors =>
            new List<IEventConnector>()
                .Concat(sourceConnectorInstances ?? new List<EventConnector>())
                .Concat((sourceConnectorIds ?? new List<string>()).SelectMany(Container.ResolveIdAll<IEventConnector>));

        [Inject] private DiContainer Container { get; }

        IObservable<EventMessages> IEventConnector.ConnectAsObservable()
        {
            return GenerateSourceObservable()
                .SelectMany(Connect);
        }

        protected abstract IObservable<EventMessages> Connect(EventMessages eventMessages);

        private bool HasSourceConnectors() =>
            SourceConnectors.Any();

        private IObservable<EventMessages> GenerateSourceObservable() =>
            HasSourceConnectors()
                ? SourceConnectors.Select(x => x.ConnectAsObservable()).Merge()
                : Observable.Return(EventMessages.Create());
    }
}