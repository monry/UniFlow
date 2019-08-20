using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/PhysicsCollisionEvent", (int) ConnectorType.PhysicsCollisionEvent)]
    public class PhysicsCollisionEvent : ConnectorBase
    {
        [SerializeField] private PhysicsCollisionEventType physicsCollisionEventType = default;
        [SerializeField] private Component component = default;

        [UsedImplicitly] public PhysicsCollisionEventType PhysicsCollisionEventType
        {
            get => physicsCollisionEventType;
            set => physicsCollisionEventType = value;
        }
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }

        public override IObservable<EventMessage> OnConnectAsObservable() =>
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