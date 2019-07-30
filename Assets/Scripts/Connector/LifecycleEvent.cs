using System;
using EventConnector.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace EventConnector.Connector
{
    public class LifecycleEvent : EventConnector
    {
        [SerializeField] private LifecycleEventType lifecycleEventType = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component = default;

        private LifecycleEventType LifecycleEventType => lifecycleEventType;
        private Component Component => component ? component : component = this;

        private ISubject<bool> StartSubject { get; } = new BehaviorSubject<bool>(false);

        protected override IObservable<EventMessages> Connect(EventMessages eventMessages)
        {
            return OnEventAsObservable().Select(_ => eventMessages.Append((EventType.LifecycleEvent, Component, LifecycleEventData.Create(LifecycleEventType))));
        }

        protected override void Start()
        {
            base.Start();
            StartSubject.OnNext(true);
        }

        private IObservable<Unit> OnEventAsObservable()
        {
            switch (LifecycleEventType)
            {
                case LifecycleEventType.Start:
                    // Only the `Start()` method is handled by its own instance
                    return StartSubject.Where(x => x).AsUnitObservable();
                case LifecycleEventType.Update:
                    return Component.UpdateAsObservable();
                case LifecycleEventType.FixedUpdate:
                    return Component.FixedUpdateAsObservable();
                case LifecycleEventType.LateUpdate:
                    return Component.LateUpdateAsObservable();
                case LifecycleEventType.Destroy:
                    return Component.OnDestroyAsObservable();
                case LifecycleEventType.Enable:
                    return Component.OnEnableAsObservable();
                case LifecycleEventType.Disable:
                    return Component.OnDisableAsObservable();
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