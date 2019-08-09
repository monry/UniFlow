using System;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/PhysicsCollisionEvent", 200)]
    public class PhysicsCollisionEvent : ConnectorBase
    {
        [SerializeField] private PhysicsCollisionEventType physicsCollisionEventType = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component = default;

        private PhysicsCollisionEventType PhysicsCollisionEventType => physicsCollisionEventType;
        private Component Component => component ? component : component = this;

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            OnEventAsObservable()
                .Select(x => EventMessage.Create(ConnectorType.PhysicsCollisionEvent, Component, PhysicsCollisionEventData.Create(PhysicsCollisionEventType, x)));

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