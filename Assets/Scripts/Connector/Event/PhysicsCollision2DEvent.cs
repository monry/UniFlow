using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/PhysicsCollision2DEvent", (int) ConnectorType.PhysicsCollision2DEvent)]
    public class PhysicsCollision2DEvent : ConnectorBase
    {
        [SerializeField] private Component component = default;
        [SerializeField] private PhysicsCollision2DEventType physicsCollision2DEventType = PhysicsCollision2DEventType.CollisionEnter2D;

        [ValuePublisher] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        [UsedImplicitly] public PhysicsCollision2DEventType PhysicsCollision2DEventType
        {
            get => physicsCollision2DEventType;
            set => physicsCollision2DEventType = value;
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return OnEventAsObservable()
                .AsUnitObservable();
        }

        private IObservable<Collision2D> OnEventAsObservable()
        {
            switch (PhysicsCollision2DEventType)
            {
                case PhysicsCollision2DEventType.CollisionEnter2D:
                    return Component.OnCollisionEnter2DAsObservable();
                case PhysicsCollision2DEventType.CollisionExit2D:
                    return Component.OnCollisionExit2DAsObservable();
                case PhysicsCollision2DEventType.CollisionStay2D:
                    return Component.OnCollisionStay2DAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum PhysicsCollision2DEventType
    {
        CollisionEnter2D,
        CollisionExit2D,
        CollisionStay2D,
    }
}
