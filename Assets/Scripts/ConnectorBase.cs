using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Zenject;

namespace UniFlow
{
    public abstract class ConnectorBase : ConnectableBase, IConnector
    {
        [SerializeField] [Tooltip("Specify instances of IEventConnectable directly")]
        private List<ConnectableBase> targetComponents = new List<ConnectableBase>();

        [SerializeField] [Tooltip("Specify identifiers of IEventConnectable that resolve from Zenject.DiContainer")]
        private List<string> targetIds = new List<string>();

        [SerializeField] [Tooltip("Set true to allow to act as the entry point of events")]
        protected bool actAsTrigger = false;

        private IEnumerable<IConnectable> TargetConnectors =>
            new List<IConnectable>()
                .Concat(targetComponents ?? new List<ConnectableBase>())
                .Concat((targetIds ?? new List<string>()).SelectMany(x => Container?.ResolveIdAll<IConnectable>(x)))
                .Where(x => !ReferenceEquals(x, this))
                .ToArray();

#if UNITY_EDITOR
        [UsedImplicitly]
        public IEnumerable<ConnectableBase> TargetComponents
        {
            get => targetComponents;
            set => targetComponents = value.ToList();
        }

        [UsedImplicitly]
        public IEnumerable<string> TargetIds
        {
            get => targetIds;
            set => targetIds = value.ToList();
        }
#endif

        [UsedImplicitly] public virtual bool ActAsTrigger
        {
            get => actAsTrigger;
            set => actAsTrigger = value;
        }

        [Inject] private DiContainer Container { get; }

        protected override void Awake()
        {
            base.Awake();
            if (ActAsTrigger)
            {
                ((IConnector) this).Connect(Observable.Return<(IMessage, Messages)>(default));
            }
        }

        void IConnector.Connect(IObservable<(IMessage latestMessage, Messages massages)> source)
        {
            var observable = source
                .SelectMany(
                    eventMessages => (this as IConnector)
                        .OnConnectAsObservable(eventMessages.latestMessage)
                        .Select(x => (latestMessage: x, messages: (eventMessages.massages ?? Messages.Create()).Append(x)))
                )
                .Share();
            TargetConnectors
                .OfType<IConnector>()
                .ToList()
                .ForEach(x => x.Connect(observable));
            TargetConnectors
                .OfType<IReceiver>()
                .ToList()
                .ForEach(x => observable.Subscribe(y => x.OnReceive(y.messages)));

            if (!TargetConnectors.Any())
            {
                observable.Subscribe();
            }
        }

        public abstract IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage);

        [PublicAPI]
        public void AddConnectable(ConnectableBase connectable)
        {
            if (targetComponents == default)
            {
                targetComponents = new List<ConnectableBase>();
            }
            targetComponents.Add(connectable);
        }
    }
}