using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/LifecycleEvent", 100)]
    public class LifecycleEvent : ConnectorBase
    {
        [SerializeField] private LifecycleEventType lifecycleEventType = default;
        [UsedImplicitly] public LifecycleEventType LifecycleEventType
        {
            get => lifecycleEventType;
            set => lifecycleEventType = value;
        }

        private Component component = default;
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }

        private IReactiveProperty<bool> StartProperty { get; } = new BoolReactiveProperty(false);
        private IReactiveProperty<bool> OnEnableProperty { get; } = new BoolReactiveProperty(false);

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return OnEventAsObservable()
                .Select(_ => EventMessage.Create(ConnectorType.LifecycleEvent, Component, LifecycleEventData.Create(LifecycleEventType)));
        }

        private void Awake()
        {
            OnEnableProperty.Value = enabled;
        }

        protected override void Start()
        {
            base.Start();

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