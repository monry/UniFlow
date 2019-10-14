using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/LifecycleEvent", (int) ConnectorType.LifecycleEvent)]
    public class LifecycleEvent : ConnectorBase
    {
        [SerializeField] private Component component = default;
        [SerializeField] private LifecycleEventType lifecycleEventType = LifecycleEventType.Start;

        [ValuePublisher] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        [UsedImplicitly] public LifecycleEventType LifecycleEventType
        {
            get => lifecycleEventType;
            set => lifecycleEventType = value;
        }

        private IReactiveProperty<bool> StartProperty { get; } = new BoolReactiveProperty(false);
        private IReactiveProperty<bool> OnEnableProperty { get; } = new BoolReactiveProperty(false);

        public override IObservable<Message> OnConnectAsObservable()
        {
            return OnEventAsObservable().Select(this.CreateMessage);
        }

        protected override void Start()
        {
            OnEnableProperty.Value = enabled;
            StartProperty.Value = true;
            base.Start();
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
