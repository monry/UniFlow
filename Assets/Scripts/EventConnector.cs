using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace EventConnector
{
    public abstract class EventConnector : MonoBehaviour, IEventConnector, IEventConnectable
    {
        [SerializeField]
        [Tooltip("Specify instances of IEventConnector directly")]
        private List<EventConnector> sourceConnectorInstances = default;
        [SerializeField]
        [Tooltip("Specify identifiers of IEventConnector that resolve from Zenject.DiContainer")]
        private List<string> sourceConnectorIds = default;
        [SerializeField]
        [Tooltip("Check if you want to act as Receiver")]
        private bool actAsReceiver = false;

        private IEnumerable<IEventConnector> SourceConnectors =>
            new List<IEventConnector>()
                .Concat(sourceConnectorInstances ?? new List<EventConnector>())
                .Concat((sourceConnectorIds ?? new List<string>()).SelectMany(Container.ResolveIdAll<IEventConnector>));
        private bool ActAsReceiver => actAsReceiver;

        [Inject] private DiContainer Container { get; }

        protected virtual void Start()
        {
            if (ActAsReceiver)
            {
                ((IEventConnector) this)
                    .ConnectAsObservable()
                    .Subscribe()
                    .AddTo(this);
            }
        }

        IObservable<EventMessages> IEventConnector.ConnectAsObservable()
        {
            return GenerateSourceObservable().SelectMany(Connect);
        }

        protected abstract IObservable<EventMessages> Connect(EventMessages eventMessages);

        bool IEventConnectable.HasSourceConnectors() =>
            SourceConnectors.Any();

        private IObservable<EventMessages> GenerateSourceObservable() =>
            ((IEventConnectable) this).HasSourceConnectors()
                ? SourceConnectors.Select(x => x.ConnectAsObservable()).Merge()
                : Observable.Defer(() => Observable.Return(EventMessages.Create()));
    }
}