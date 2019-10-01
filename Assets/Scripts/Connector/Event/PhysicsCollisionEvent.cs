using System;
using JetBrains.Annotations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/PhysicsCollisionEvent", (int) ConnectorType.PhysicsCollisionEvent)]
    public class PhysicsCollisionEvent : ConnectorBase
    {
        [SerializeField] private PhysicsCollisionEventType physicsCollisionEventType = PhysicsCollisionEventType.CollisionEnter;
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

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return OnEventAsObservable()
                .AsUnitObservable();
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
