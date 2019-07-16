using System;
using EventConnector.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace EventConnector.Connector
{
    public class PhysicsCollisionEvent : EventConnector
    {
        [SerializeField] private PhysicsCollisionEventType physicsCollisionEventType;
        private PhysicsCollisionEventType PhysicsCollisionEventType => physicsCollisionEventType;

        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component;
        private Component Component => component ? component : component = this;

        protected override IObservable<EventMessages> Connect(EventMessages eventMessages)
        {
            return OnEventAsObservable().Select(x => eventMessages.Append((Component, PhysicsCollisionEventData.Create(PhysicsCollisionEventType, x))));
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