using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Utility;
using UniRx;
using UnityEngine;
using Zenject;

namespace UniFlow
{
    public abstract class ConnectorBase : MonoBehaviour, IConnector
    {
        [SerializeField] [Tooltip("Specify instances of IConnector directly")]
        private List<ConnectorBase> targetComponents = new List<ConnectorBase>();

        [SerializeField] [Tooltip("Specify identifiers of IConnector that resolve from Zenject.DiContainer")]
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

        public ISubject<Message> OnConnectSubject { get; } = new Subject<Message>();
#endif

        [UsedImplicitly] public virtual bool ActAsTrigger
        {
            get => actAsTrigger;
            set => actAsTrigger = value;
        }

        [Inject] private DiContainer Container { get; }

        public IList<Message> StreamedMessages { get; private set; } = new List<Message>();

        protected virtual void Start()
        {
            if (ActAsTrigger)
            {
                ((IConnector) this).Connect(ObservableFactory.ReturnMessage(this));
            }
        }

        protected virtual IEnumerable<ICollectableMessageAnnotation> MergeMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[0];

        protected virtual IEnumerable<IComposableMessageAnnotation> MergeMessageComposableAnnotations() =>
            new IComposableMessageAnnotation[0];

        private ISubject<Unit> ShutdownSubject { get; } = new Subject<Unit>();

        void IConnector.Connect(IObservable<Message> source)
        {
#if UNITY_EDITOR
            if (Logger.IsEnabled)
            {
                OnConnectSubject.Subscribe(_ => Logger.Log(this)).AddTo(this);
            }
#endif
            ShutdownSubject.OnNext(Unit.Default);
            var observable = source
                .SelectMany(
                    message =>
                    {
                        StreamedMessages = message.StreamedMessages.ToList();

                        if (this is IMessageCollectable messageCollectable)
                        {
                            messageCollectable.GetMessageCollectableAnnotations().ToList().ForEach(x => x.Collect());
                        }

                        return (this as IConnector)
                            .OnConnectAsObservable()
                            .Select(x => this is IMessageComposable messageComposable ? messageComposable.ComposeAll(x) : x)
                            .Do(x => x.StreamedMessages?.Add(x))
#if UNITY_EDITOR
                            .Do(OnConnectSubject.OnNext)
#endif
                            ;
                    }
                );
            if (TargetConnectors.Count() > 1)
            {
                observable = observable.Replay().RefCount();
            }

            observable = observable.TakeUntil(ShutdownSubject);

#if UNITY_EDITOR
            TargetConnectors
                .Select((connector, index) => (connector, index))
                .Where(x => x.connector == default)
                .ToList()
                .ForEach(x => Debug.LogWarning($"{gameObject.name}.{GetType().Name} contains empty target connector at index: {x.index}"));
#endif
            TargetConnectors
                .Where(x => x != default)
                .ToList()
                .ForEach(x => x.Connect(observable));

            if (!TargetConnectors.Any())
            {
                observable.Subscribe().AddTo(this);
            }
        }

        public abstract IObservable<Message> OnConnectAsObservable();

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
