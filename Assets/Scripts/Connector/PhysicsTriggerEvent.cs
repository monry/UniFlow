using System;
using EventConnector.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace EventConnector.Connector
{
    public class PhysicsTriggerEvent : EventConnector
    {
        [SerializeField] private PhysicsTriggerEventType physicsTriggerEventType = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component = default;

        private PhysicsTriggerEventType PhysicsTriggerEventType => physicsTriggerEventType;
        private Component Component => component ? component : component = this;

        protected override IObservable<EventMessages> Connect(EventMessages eventMessages)
        {
            return OnEventAsObservable().Select(x => eventMessages.Append((Component, PhysicsTriggerEventData.Create(PhysicsTriggerEventType, x))));
        }

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