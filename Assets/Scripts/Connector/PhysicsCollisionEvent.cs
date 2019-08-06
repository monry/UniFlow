using System;
using EventConnector.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace EventConnector.Connector
{
    public class PhysicsCollisionEvent : EventConnector
    {
        [SerializeField] private PhysicsCollisionEventType physicsCollisionEventType = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component = default;

        private PhysicsCollisionEventType PhysicsCollisionEventType => physicsCollisionEventType;
        private Component Component => component ? component : component = this;

        protected override void Connect(EventMessages eventMessages)
        {
            OnEventAsObservable()
                .SubscribeWithState(
                    eventMessages,
                    (x, em) => em.Append(EventMessage.Create(EventType.PhysicsCollisionEvent, Component, PhysicsCollisionEventData.Create(PhysicsCollisionEventType, x)))
                );
        }

        private IObservable<Collision> OnEventAsObservable()
        {
            switch (PhysicsCollisionEventType)
            {
                case PhysicsCollisionEventType.CollisionEnter:
                    return Component.OnCollisionEnterAsObservable();
                case PhysicsCollisionEventType.CollisionExit:
                    return Component.OnCollisionExitAsObservable();
                case PhysicsCollisionEventType.CollisionStay:
                    return Component.OnCollisionStayAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum PhysicsCollisionEventType
    {
        CollisionEnter,
        CollisionExit,
        CollisionStay,
    }
}