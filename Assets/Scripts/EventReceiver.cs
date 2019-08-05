using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace EventConnector
{
    public abstract class EventReceiver : MonoBehaviour, IEventReceiver
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

        private void Start()
        {
            GenerateSourceObservable()
                .Subscribe(Receive)
                .AddTo(this);
        }

        protected abstract void Receive(EventMessages eventMessages);

        private IObservable<EventMessages> GenerateSourceObservable() =>
            SourceConnectors.Any()
                ? SourceConnectors.Select(x => x.ConnectAsObservable()).Merge()
                : Observable.Return(EventMessages.Create());
    }
}