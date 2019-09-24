using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Zenject;

namespace UniFlow
{
    public abstract class ConnectorBase : MonoBehaviour, IConnector
    {
        [SerializeField] [Tooltip("Specify instances of IEventConnector directly")]
        private List<ConnectorBase> targetComponents = new List<ConnectorBase>();

        [SerializeField] [Tooltip("Specify identifiers of IEventConnector that resolve from Zenject.DiContainer")]
        private List<string> targetIds = new List<string>();

        [SerializeField] [Tooltip("Set true to allow to act as the entry point of events")]
        protected bool actAsTrigger = false;

        protected Messages Messages { get; private set; }

        protected virtual IEnumerable<IConnector> TargetConnectors =>
            new List<IConnector>()
                .Concat(targetComponents ?? new List<ConnectorBase>())
                .Concat((targetIds ?? new List<string>()).SelectMany(x => Container?.ResolveIdAll<IConnector>(x)))
                .Where(x => !ReferenceEquals(x, this))
                .ToArray();

#if UNITY_EDITOR
        [UsedImplicitly]
        public IEnumerable<ConnectorBase> TargetComponents
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

        [SerializeField] [HideInInspector] private Vector2 flowGraphNodePosition = default;
        public Vector2 FlowGraphNodePosition
        {
            get => flowGraphNodePosition;
            set => flowGraphNodePosition = value;
        }
#endif

        [UsedImplicitly] public virtual bool ActAsTrigger
        {
            get => actAsTrigger;
            set => actAsTrigger = value;
        }

        [Inject] private DiContainer Container { get; }

        protected virtual void Awake()
        {
            CollectSuppliedValues();
            if (ActAsTrigger)
            {
                ((IConnector) this).Connect(Observable.Return<(IMessage, Messages)>(default));
            }
        }

        void IConnector.Connect(IObservable<(IMessage latestMessage, Messages massages)> source)
        {
            var observable = source
                .SelectMany(
                    eventMessages =>
                    {
                        var (latestMessage, massages) = eventMessages;
                        Messages = massages;
                        return (this as IConnector)
                            .OnConnectAsObservable(latestMessage)
                            .Select(x => (latestMessage: x, messages: (massages ?? Messages.Create()).Append(x)));
                    }
                );
            if (TargetConnectors.Count() > 1)
            {
                observable = observable.Share();
            }
            TargetConnectors
                .ToList()
                .ForEach(x => x.Connect(observable));

            if (!TargetConnectors.Any())
            {
                observable.Subscribe();
            }
        }

        protected virtual void CollectSuppliedValues()
        {
            // Do nothing
        }

        public abstract IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage);

        [PublicAPI]
        public void AddConnector(ConnectorBase connectable)
        {
            if (targetComponents == default)
            {
                targetComponents = new List<ConnectorBase>();
            }
            targetComponents.Add(connectable);
        }
    }
}
