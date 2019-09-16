using System;
using JetBrains.Annotations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/LifecycleEvent", (int) ConnectorType.LifecycleEvent)]
    public class LifecycleEvent : ConnectorBase
    {
        [SerializeField] private LifecycleEventType lifecycleEventType = LifecycleEventType.Start;
        [SerializeField] private Component component = default;

        [UsedImplicitly] public LifecycleEventType LifecycleEventType
        {
            get => lifecycleEventType;
            set => lifecycleEventType = value;
        }
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }

        private IReactiveProperty<bool> StartProperty { get; } = new BoolReactiveProperty(false);
        private IReactiveProperty<bool> OnEnableProperty { get; } = new BoolReactiveProperty(false);

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return OnEventAsObservable()
                .Select(_ => Message.Create(this));
        }

        protected override void Awake()
        {
            base.Awake();
            OnEnableProperty.Value = enabled;
        }

        private void Start()
        {
            StartProperty.Value = true;
        }

        private void OnEnable()
        {
            OnEnableProperty.Value = true;
        }

        private void OnDisable()
        {
            OnEnableProperty.Value = false;
        }

        private IObservable<Unit> OnEventAsObservable()
        {
            switch (LifecycleEventType)
            {
                case LifecycleEventType.Start:
                    // Only the `Start()` method is handled by its own instance
                    return StartProperty.Where(x => x).AsUnitObservable();
                case LifecycleEventType.Update:
                    return Component.UpdateAsObservable();
                case LifecycleEventType.FixedUpdate:
                    return Component.FixedUpdateAsObservable();
                case LifecycleEventType.LateUpdate:
                    return Component.LateUpdateAsObservable();
                case LifecycleEventType.Destroy:
                    return Component.OnDestroyAsObservable();
                case LifecycleEventType.Enable:
                    return OnEnableProperty.Where(x => x).AsUnitObservable();
                case LifecycleEventType.Disable:
                    return OnEnableProperty.Where(x => !x).AsUnitObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public class Message : MessageBase<LifecycleEvent>
        {
            public static Message Create(LifecycleEvent sender)
            {
                return Create<Message>(ConnectorType.LifecycleEvent, sender);
            }
        }
    }

    public enum LifecycleEventType
    {
        Start,
        Update,
        FixedUpdate,
        LateUpdate,
        Destroy,
        Enable,
        Disable,
    }
}