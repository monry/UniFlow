using System;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("Event Connector/PhysicsTrigger2DEvent", 203)]
    public class PhysicsTrigger2DEvent : EventPublisher
    {
        [SerializeField] private PhysicsTrigger2DEventType physicsTrigger2DEventType = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component = default;

        private PhysicsTrigger2DEventType PhysicsTrigger2DEventType => physicsTrigger2DEventType;
        private Component Component => component ? component : component = this;

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            OnEventAsObservable()
                .Select(x => EventMessage.Create(EventType.PhysicsTrigger2DEvent, Component, PhysicsTrigger2DEventData.Create(PhysicsTrigger2DEventType, x)));

        private IObservable<Collider2D> OnEventAsObservable()
        {
            switch (PhysicsTrigger2DEventType)
            {
                case PhysicsTrigger2DEventType.TriggerEnter2D:
                    return Component.OnTriggerEnter2DAsObservable();
                case PhysicsTrigger2DEventType.TriggerExit2D:
                    return Component.OnTriggerExit2DAsObservable();
                case PhysicsTrigger2DEventType.TriggerStay2D:
                    return Component.OnTriggerStay2DAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum PhysicsTrigger2DEventType
    {
        TriggerEnter2D,
        TriggerExit2D,
        TriggerStay2D,
    }
}