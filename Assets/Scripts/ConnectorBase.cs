using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace UniFlow
{
    public abstract class ConnectorBase : ConnectableBase, IConnector
    {
        [SerializeField] [Tooltip("Specify instances of IEventConnectable directly")]
        private List<ConnectableBase> targetConnectorInstances = default;

        [SerializeField] [Tooltip("Specify identifiers of IEventConnectable that resolve from Zenject.DiContainer")]
        private List<string> targetConnectorIds = default;

        [SerializeField] [Tooltip("Set true to allow to act as the entry point of events")]
        private bool actAsTrigger = false;

        [SerializeField] [Tooltip("Set true to allow to act as the receiver")]
        private bool actAsReceiver = false;

        private IEnumerable<IConnectable> TargetConnectors =>
            new List<IConnectable>()
                .Concat(targetConnectorInstances ?? new List<ConnectableBase>())
                .Concat((targetConnectorIds ?? new List<string>()).SelectMany(Container.ResolveIdAll<IConnectable>))
                .Where(x => !ReferenceEquals(x, this))
                .ToArray();

        private bool ActAsTrigger => actAsTrigger;
        private bool ActAsReceiver => actAsReceiver;
        [Inject] private DiContainer Container { get; }

        protected virtual void Start()
        {
            if (ActAsTrigger)
            {
                ((IConnector) this).Connect(Observable.Return<EventMessages>(default));
            }
        }

        void IConnector.Connect(IObservable<EventMessages> source)
        {
            var observable = source
                .SelectMany(
                    eventMessages => (this as IConnector)
                        .OnPublishAsObservable()
                        .Select(x => (eventMessages ?? EventMessages.Create()).Append(x))
                );
            TargetConnectors
                .OfType<IConnector>()
                .ToList()
                .ForEach(x => x.Connect(observable));
            TargetConnectors
                .OfType<IReceiver>()
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