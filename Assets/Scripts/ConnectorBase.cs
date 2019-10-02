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

        public ISubject<Unit> OnConnectSubject { get; } = new Subject<Unit>();
#endif

        [UsedImplicitly] public virtual bool ActAsTrigger
        {
            get => actAsTrigger;
            set => actAsTrigger = value;
        }

        [Inject] private DiContainer Container { get; }

        protected virtual void Start()
        {
            if (ActAsTrigger)
            {
                ((IConnector) this).Connect(Observable.ReturnUnit());
            }
        }

        void IConnector.Connect(IObservable<Unit> source)
        {
#if UNITY_EDITOR
            if (Logger.IsEnabled)
            {
                OnConnectSubject.Subscribe(_ => Logger.Log(this)).AddTo(this);
            }
#endif
            var observable = source
                .SelectMany(
                    _ =>
                    {
                        return (this as IConnector)
                            .OnConnectAsObservable()
#if UNITY_EDITOR
                            .Do(OnConnectSubject.OnNext)
#endif
                            ;
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
                observable.Subscribe().AddTo(this);
            }
        }

        public abstract IObservable<Unit> OnConnectAsObservable();

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
