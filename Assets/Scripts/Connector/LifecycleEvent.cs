using System;
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
        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component = default;

        private LifecycleEventType LifecycleEventType => lifecycleEventType;
        private Component Component => component ? component : component = this;

        private IReactiveProperty<bool> StartProperty { get; } = new BoolReactiveProperty(false);
        private IReactiveProperty<bool> OnEnableProperty { get; } = new BoolReactiveProperty(false);

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            OnEventAsObservable()
                .Select(_ => EventMessage.Create(ConnectorType.LifecycleEvent, Component, LifecycleEventData.Create(LifecycleEventType)));

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