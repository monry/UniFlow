using System;
using EventConnector.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace EventConnector.Connector
{
    public class PhysicsTriggerEvent : EventConnector, IEventPublisher
    {
        [SerializeField] private PhysicsTriggerEventType physicsTriggerEventType = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component = default;

        private PhysicsTriggerEventType PhysicsTriggerEventType => physicsTriggerEventType;
        private Component Component => component ? component : component = this;

        IObservable<EventMessage> IEventPublisher.OnPublishAsObservable() =>
            OnEventAsObservable()
                .Select(x => EventMessage.Create(EventType.PhysicsTriggerEvent, Component, PhysicsTriggerEventData.Create(PhysicsTriggerEventType, x)));

        private IObservable<Collider> OnEventAsObservable()
        {
            switch (PhysicsTriggerEventType)
            {
                case PhysicsTriggerEventType.TriggerEnter:
                    return Component.OnTriggerEnterAsObservable();
                case PhysicsTriggerEventType.TriggerExit:
                    return Component.OnTriggerExitAsObservable();
                case PhysicsTriggerEventType.TriggerStay:
                    return Component.OnTriggerStayAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum PhysicsTriggerEventType
    {
        TriggerEnter,
        TriggerExit,
        TriggerStay,
    }
}